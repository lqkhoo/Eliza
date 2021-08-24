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

        /*
        public T Deserialize<T>()
        {
            return (T)ReadValue(typeof(T));
        }
        */

        // Public read methods preserve the position of this.Reader
        // from previous reads. For most intents and purposes we stick
        // with default arguments and simply read the header, data,
        // and footer sequentially.

        public RF5SaveDataHeader ReadSaveDataHeader(bool reposition=false,
                                                    long newPosition=0x0)
        {
            if (reposition) { this.BaseStream.Position = newPosition; }
            return (RF5SaveDataHeader)this.ReadObject(typeof(RF5SaveDataHeader));
        }

        public RF5SaveData ReadSaveData(bool reposition=false,
                                        long newPosition=BinarySerializer.HEADER_LENGTH_NBYTES)
        {
            if (reposition) { this.BaseStream.Position = newPosition; }
            return (RF5SaveData)this.ReadObject(typeof(RF5SaveData));
        }

        public RF5SaveDataFooter ReadSaveDataFooter(bool reposition=false, long newPosition=-1)
        {
            if (reposition) {
                newPosition = newPosition == -1 ? this.BaseStream.Position : newPosition;
                this.BaseStream.Position = newPosition;
            } else {
                //Aligned relative to data 256bits due to Rijndael crypto
                this.BaseStream.Position = ((this.BaseStream.Position - 0x20 + 0x1F) & ~0x1F) + 0x20;
            }
            return (RF5SaveDataFooter)this.ReadObject(typeof(RF5SaveDataFooter));
        }

        protected object ReadValue(Type type)
        {
            if (type.IsPrimitive)
            {
                return this.ReadPrimitive(Type.GetTypeCode(type));
            }
            else if (IsList(type))
            {
                return this.ReadList(type);
            }
            else if (type == typeof(string))
            {
                return this.ReadString();
            }
            else if (type == typeof(SaveFlagStorage))
            {
                return this.ReadSaveFlagStorage();
            }
            else if (IsDictionary(type))
            {
                return this.ReadDictionary(type);
            }
            else
            {
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

        protected IList ReadList(Type type, TypeCode lengthType = TypeCode.Int32, int length = 0, int max = 0, bool isMessagePackList = false)
        {
            IList ilist;

            length = length == 0 ? Convert.ToInt32(this.ReadPrimitive(lengthType)) : length;

            // Overrides length
            if (max != 0)
            {
                if (type.IsArray)
                {
                    type = type.GetElementType();
                    ilist = Array.CreateInstance(type, max);
                }
                else
                {
                    ilist = (IList)Activator.CreateInstance(type);
                    type = type.GetGenericArguments()[0];
                }

                for (int index = 0; index < max; index++)
                {
                    var value = this.ReadValue(type);

                    if (ilist.IsFixedSize)
                    {
                        ilist[index] = value;
                    }
                    else
                    {
                        ilist.Add(value);
                    }
                }
            }
            else
            {
                if (type.IsArray)
                {
                    type = type.GetElementType();
                    ilist = Array.CreateInstance(type, length);
                }
                else
                {
                    ilist = (IList)Activator.CreateInstance(type);
                    type = type.GetGenericArguments()[0];
                }

                for (int index = 0; index < length; index++)
                {
                    var value = isMessagePackList ? this.ReadMessagePackObject(type) : this.ReadValue(type);

                    if (ilist.IsFixedSize)
                    {
                        ilist[index] = value;
                    }
                    else
                    {
                        ilist.Add(value);
                    }
                }
            }
            return ilist;
        }

        protected string ReadString(int size = 0, int max = 0)
        {
            List<byte> dataString = new List<byte>();

            //Might be deprecated
            if (size != 0)
            {
                var data = this.Reader.ReadBytes(size);

                return Encoding.Unicode.GetString(
                    data
                );
            }
            else if (max != 0)
            {
                var length = this.Reader.ReadInt32();
                var data = this.Reader.ReadBytes(length);
                this.BaseStream.Seek(max - length, SeekOrigin.Current);

                return Encoding.Unicode.GetString(
                    data
                );
            }
            else
            {
                do
                {
                    var character = this.Reader.ReadByte();
                    var nullCharacter = this.Reader.ReadByte();

                    if (character != 0 & nullCharacter == 0)
                    {
                        dataString.Add(character);
                    }
                    else if (character == 0 & nullCharacter == 0)
                    {
                        break;
                    }

                } while (true);

                return Encoding.Unicode.GetString(
                    dataString.ToArray()
                );
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

        protected RF5SaveDataFooter ReadSaveDataFooter(Type type)
        {
            //Aligned relative to data 256bits due to Rijndael crypto
            this.BaseStream.Position = ((this.BaseStream.Position - 0x20 + 0x1F) & ~0x1F) + 0x20;
            return (RF5SaveDataFooter)this.ReadObject(type);
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

            int fieldCount = 0;

            // MessagePackObject
            if (objectType.IsDefined(typeof(MessagePackObjectAttribute)))
            {
                objectValue = this.ReadMessagePackObject(objectType);
            }
            else
            {
                foreach (FieldInfo info in GetFieldsOrdered(objectType))
                {
                    fieldCount++;

                    if (!info.IsDefined(typeof(CompilerGeneratedAttribute)))
                    {
                        Type fieldType = info.FieldType;

                        object fieldValue = null;

                        var messagePackListAttribute = (ElizaMessagePackListAttribute)info.GetCustomAttribute(typeof(ElizaMessagePackListAttribute));
                        if (messagePackListAttribute != null)
                        {
                            if (IsList(fieldType))
                            {
                                fieldValue = this.ReadList(fieldType, isMessagePackList: true);
                            }
                        }

                        var messagePackRawAttribute = (ElizaMessagePackRawAttribute)info.GetCustomAttribute(typeof(ElizaMessagePackRawAttribute));
                        if (messagePackRawAttribute != null)
                        {
                            fieldValue = this.ReadMessagePackObject(fieldType);
                        }

                        var lengthAttribute = (ElizaSizeAttribute)info.GetCustomAttribute(typeof(ElizaSizeAttribute));
                        if (lengthAttribute != null)
                        {
                            if (lengthAttribute.Fixed != 0)
                            {
                                if (IsList(fieldType))
                                {
                                    fieldValue = this.ReadList(fieldType, length: lengthAttribute.Fixed);
                                }
                                else if (fieldType == typeof(string))
                                {
                                    fieldValue = this.ReadString(lengthAttribute.Fixed);
                                }
                                else
                                {
                                    //If attribute was applied by mistake
                                    fieldValue = null;
                                }
                            }
                            else if (lengthAttribute.LengthType != TypeCode.Empty)
                            {
                                if (IsList(fieldType))
                                {
                                    fieldValue = this.ReadList(fieldType, lengthType: lengthAttribute.LengthType);
                                }
                                else if (fieldType == typeof(string))
                                {
                                    //Not supported for strings ATM
                                    fieldValue = null;
                                }
                                else
                                {
                                    //If attribute was applied by mistake
                                    fieldValue = null;
                                }
                            }
                            else if (lengthAttribute.Max != 0)
                            {
                                if (IsList(fieldType))
                                {
                                    fieldValue = this.ReadList(fieldType, max: lengthAttribute.Max);
                                }
                                else if (fieldType == typeof(string))
                                {
                                    fieldValue = this.ReadString(max: lengthAttribute.Max);
                                }
                                else
                                {
                                    //If attribute was applied by mistake
                                    fieldValue = null;
                                }
                            }
                            else
                            {
                                //If attribute was applied by mistake
                                fieldValue = null;
                            }
                        }

                        fieldValue = fieldValue == null ? this.ReadValue(fieldType) : fieldValue;

                        if (fieldValue != null) info.SetValue(objectValue, fieldValue);
                    }
                }
            }
            return objectValue;
        }
        protected object ReadMessagePackObject(Type type)
        {
            var length = this.Reader.ReadInt32();
            var data = this.Reader.ReadBytes(length);
            return MessagePackSerializer.Deserialize(type, data);
        }
    }
}
