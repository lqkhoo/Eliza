using MessagePack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using static Eliza.Core.Serialization.ElizaAttribute;
using Eliza.Model.Status;
using Eliza.Model.Save;
using Eliza.Model;

namespace Eliza.Core.Serialization
{
    class BinaryDeserializer : BinarySerialization
    {
        public readonly BinaryReader Reader;

        public BinaryDeserializer(Stream baseStream) : base(baseStream)
        {
            this.Reader = new BinaryReader(baseStream);
        }

        // Public read methods preserve the position of this.Reader
        // from previous reads. For most intents and purposes we stick
        // with default arguments and simply read the header, data,
        // and footer sequentially.
        public RF5SaveDataHeader ReadSaveDataHeader()
        {
            return (RF5SaveDataHeader)this.ReadObject(typeof(RF5SaveDataHeader));
        }

        public RF5SaveData ReadSaveData()
        {
            return (RF5SaveData)this.ReadObject(typeof(RF5SaveData));
        }

        public RF5SaveDataFooter ReadSaveDataFooter()
        {
            //Aligned relative to data 256bits due to Rijndael crypto
            this.BaseStream.Position = ((this.BaseStream.Position - 0x20 + 0x1F) & ~0x1F) + 0x20;
            return (RF5SaveDataFooter)this.ReadObject(typeof(RF5SaveDataFooter));
        }

        protected object ReadValue(Type type)
        {
            if (type.IsPrimitive) {
                return this.ReadPrimitive(Type.GetTypeCode(type));
            } else if (IsList(type)) {
                return this.ReadList(type);
            } else if (type == typeof(string)) {
                return this.ReadString();
            } else if (type == typeof(SaveFlagStorage)) {
                return this.ReadSaveFlagStorage();
            } else if (IsDictionary(type)) {
                return this.ReadDictionary(type);
            } else {
                return this.ReadObject(type);
            }
        }

        protected object ReadPrimitive(TypeCode type)
        {
            switch (type)
            {
                case TypeCode.Boolean: return this.Reader.ReadBoolean();
                case TypeCode.Byte: return this.Reader.ReadByte();
                case TypeCode.Char: return this.Reader.ReadChar();
                case TypeCode.UInt16: return this.Reader.ReadUInt16();
                case TypeCode.UInt32: return this.Reader.ReadUInt32();
                case TypeCode.UInt64: return this.Reader.ReadUInt64();
                case TypeCode.SByte: return this.Reader.ReadSByte();
                case TypeCode.Int16: return this.Reader.ReadInt16();
                case TypeCode.Int32: return this.Reader.ReadInt32();
                case TypeCode.Int64: return this.Reader.ReadInt64();
                case TypeCode.Single: return this.Reader.ReadSingle();
                case TypeCode.Double: return this.Reader.ReadDouble();

                default: return null;
            }
        }

        protected IList ReadList(Type type, TypeCode lengthType=DEFAULT_LENGTH_TYPECODE,
                                    int length=UNKNOWN_LENGTH, int maxSize=UNKNOWN_LENGTH,
                                    bool isMessagePackList=false)
        {
            IList ilist;
            Type contentType;

            // If the list has dynamic length, then read in the length int preceding the data.
            if (length == UNKNOWN_LENGTH) {
                length = Convert.ToInt32(this.ReadPrimitive(lengthType));
                // This can still be zero, in which case it serializes to
                // just the length int, and we read nothing else.
            }

            // Even if we know the max size, we still need to read empty data
            // because they may be encoded differently.
            if (maxSize != UNKNOWN_LENGTH) {
                length = maxSize;
            }

            if (type.IsArray) {
                contentType = type.GetElementType();
                ilist = Array.CreateInstance(contentType, length);
            } else {
                ilist = (IList)Activator.CreateInstance(type);
                contentType = type.GetGenericArguments()[0];
            }

            for (int idx=0; idx<length; idx++) {

                object value;
                if (isMessagePackList) {
                    value = this.ReadMessagePackObject(contentType);
                } else {
                    value = this.ReadValue(contentType);
                }

                if (ilist.IsFixedSize) {
                    ilist[idx] = value;
                } else {
                    ilist.Add(value);
                }
            }

            return ilist;
        }

        protected string ReadString(int size=UNKNOWN_LENGTH,
                                    int max=UNKNOWN_LENGTH, bool isUtf16Uuid=false)
        {
            List<byte> dataString = new();

            //Might be deprecated
            if (size != 0) {
                byte[] data = this.Reader.ReadBytes(size);
                return Encoding.Unicode.GetString(data);

            } else if (max != 0) {
                int length = this.Reader.ReadInt32();
                byte[] data = this.Reader.ReadBytes(length);
                this.BaseStream.Seek(max - length, SeekOrigin.Current);
                return Encoding.Unicode.GetString(data);

            } else {

                //TODO: handle UUID16

                // This handles stringId in the FurnitureSaveData struct.
                // These are most likely UUIDs including the hyphens
                // encoded as UTF-16 to be passed directly into function calls.
                // Since UUIDs only have ASCII bytes, every other byte will be zero.
                // We only extract every other (first) byte.
                // See: https://stackoverflow.com/q/50070289

                do {
                    byte character = this.Reader.ReadByte();
                    byte nullCharacter = this.Reader.ReadByte();

                    if (character != 0 & nullCharacter == 0) {
                        dataString.Add(character);
                    } else if (character == 0 & nullCharacter == 0) {
                        break;
                    }

                } while (true);

                return Encoding.Unicode.GetString(dataString.ToArray());
            }
        }

        protected SaveFlagStorage ReadSaveFlagStorage()
        {
            var data = new List<byte>();
            var length = this.Reader.ReadInt32();
            int index = 0;
            bool flag;
            do
            {
                data.Add(this.Reader.ReadByte());
                flag = index++ < (length - 1) >> 3;
            } while (flag);

            return new SaveFlagStorage(data.ToArray(), length);
        }

        protected IDictionary ReadDictionary(Type type)
        {
            Type[] arguments = type.GetGenericArguments();
            Type keyType = arguments[0];
            Type valueType = arguments[1];

            Type dictType = typeof(Dictionary<,>).MakeGenericType(keyType, valueType);

            var dict = (IDictionary)Activator.CreateInstance(dictType);

            var length = this.Reader.ReadInt32();


            for (int index = 0; index < length; index++)
            {
                if (valueType == typeof(HumanStatusData))
                {

                }
                var keyValue = this.ReadValue(keyType);
                var valueValue = this.ReadValue(valueType);
                dict.Add(keyValue, valueValue);
            }

            return dict;
        }

        protected object ReadObject(Type objectType)
        {
            var objectValue = Activator.CreateInstance(objectType);

            // If known to be MessagePackObject
            if (objectType.IsDefined(typeof(MessagePackObjectAttribute))) {
                objectValue = this.ReadMessagePackObject(objectType);

            } else {
                foreach (FieldInfo fieldInfo in GetFieldsOrdered(objectType)) {
                    if (!fieldInfo.IsDefined(typeof(CompilerGeneratedAttribute))) {
                        this.ReadField(objectValue, fieldInfo);
                    }
                }
            }
            return objectValue;

        }

        protected void ReadField(object objectValue, FieldInfo fieldInfo)
        {

            Type fieldType = fieldInfo.FieldType;
            object fieldValue = null;

            // First, check if the field was decorated with at least one 
            // ElizaAttribute. This can change how we process the field.
            if (fieldInfo.IsDefined(typeof(ElizaAttribute), inherit: true)) {

                //TODO: versioning tags

                if (fieldInfo.IsDefined(typeof(ElizaMessagePackListAttribute))) {
                    if (IsList(fieldType)) {
                        fieldValue = this.ReadList(fieldType, isMessagePackList: true);
                    } else {
                        var messagePackListAttribute = (ElizaMessagePackListAttribute)fieldInfo.GetCustomAttribute(typeof(ElizaMessagePackListAttribute));
                        throw new UnsupportedAttributeException(messagePackListAttribute, fieldInfo);
                    }
                }

                if (fieldInfo.IsDefined(typeof(ElizaSizeAttribute))) {
                    var lengthAttribute = (ElizaSizeAttribute)fieldInfo.GetCustomAttribute(typeof(ElizaSizeAttribute));

                    if (lengthAttribute.Fixed != 0) {
                        if (IsList(fieldType)) {
                            fieldValue = this.ReadList(fieldType, length: lengthAttribute.Fixed);
                        } else if (fieldType == typeof(string)) {
                            fieldValue = this.ReadString(lengthAttribute.Fixed);
                        } else {
                            throw new UnsupportedAttributeException(lengthAttribute, fieldInfo);
                        }

                    } else if (lengthAttribute.LengthType != TypeCode.Empty) {
                        if (IsList(fieldType)) {
                            fieldValue = this.ReadList(fieldType, lengthType: lengthAttribute.LengthType);
                        } else if (fieldType == typeof(string)) {
                            throw new UnsupportedAttributeException(lengthAttribute, fieldInfo);
                        } else {
                            throw new UnsupportedAttributeException(lengthAttribute, fieldInfo);
                        }

                    } else if (lengthAttribute.Max != 0) {
                        if (IsList(fieldType)) {
                            fieldValue = this.ReadList(fieldType, maxSize: lengthAttribute.Max);
                        } else if (fieldType == typeof(string)) {
                            fieldValue = this.ReadString(max: lengthAttribute.Max);
                        } else {
                            throw new UnsupportedAttributeException(lengthAttribute, fieldInfo);
                        }
                    } else {
                        throw new UnsupportedAttributeException(lengthAttribute, fieldInfo);
                    }
                }

            }

            // If there were no Eliza attributes, or if the ElizaAttribute
            // was not a directive that changes how we read the field, do the simplest case.
            if (fieldValue == null) {
                fieldValue = this.ReadValue(fieldType);
            }

            fieldInfo.SetValue(objectValue, fieldValue);
            return;
        }

        protected object ReadMessagePackObject(Type type)
        {
            var length = this.Reader.ReadInt32();
            var data = this.Reader.ReadBytes(length);
            return MessagePackSerializer.Deserialize(type, data);
        }
    }
}
