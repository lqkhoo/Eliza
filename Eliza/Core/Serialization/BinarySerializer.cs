using Eliza.Model;
using Eliza.Model.Save;
using MessagePack;
using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using static Eliza.Core.Serialization.ElizaAttribute;
using Eliza.Core;

namespace Eliza.Core.Serialization
{
    class BinarySerializer : BinarySerialization
    {


        public readonly bool Encrypt;
        public BinaryWriter Writer;

        public BinarySerializer(Stream baseStream, bool encrypt=true) : base(baseStream)
        {
            this.Encrypt = encrypt;
            this.Writer = new BinaryWriter(baseStream);
        }

        // TODO: deprecate
        public void Serialize<T>(T obj)
        {
            this.WriteValue(obj);
        }

        public void WriteSaveFile(SaveData save, bool encrypt=true)
        {
            if (! encrypt) {
                this.WriteObject(save.header);
                this.WriteObject(save.saveData);
                this.WriteObject(save.footer);
                long pos = this.BaseStream.Position;
                this.BaseStream.SetLength(pos); // Truncate rest of file
                return;
            } else {

                this.WriteObject(save.header);
                this.WriteObject(save.saveData);

                using (var reader = new BinaryReader(this.BaseStream)) {
                    var bodyLength = this.BaseStream.Length;
                    //Aligned relative to data 256 bits due to Rijndael crypto
                    var paddedSize = (int)((this.BaseStream.Position - 0x20 + 0x1F) & ~0x1F) + 0x20;
                    this.BaseStream.SetLength(paddedSize);

                    this.BaseStream.Position = 0x0;
                    var headerSize = 0x20;
                    var header = reader.ReadBytes(headerSize);

                    var data = reader.ReadBytes(paddedSize - headerSize);

                    var encryptedData = Cryptography.Encrypt(data);

                    //Overwrite save data with encrypted data
                    this.BaseStream.Position = headerSize;
                    this.Writer.Write(encryptedData);

                    var bodyData = new List<byte>();
                    bodyData.AddRange(header);
                    bodyData.AddRange(encryptedData);
                    var checksum = Cryptography.Checksum(bodyData.ToArray());

                    this.Writer.Write((int)bodyLength);
                    this.Writer.Write(paddedSize);
                    this.Writer.Write(checksum);
                    // Writer.Write((int)0x0);
                    this.Writer.Write((int)save.footer.Blank); // Files will be identical if content unchanged.

                }

            }

        }



        protected void WriteValue(object value)
        {
            var type = value.GetType();

            if (type.IsPrimitive) {
                this.WritePrimitive(value);
            } else if (IsList(type)) {
                this.WriteList((IList)value);
            } else if (type == typeof(string)) {
                this.WriteString((string)value);
            } else if (type == typeof(SaveFlagStorage)) {
                this.WriteSaveFlagStorage((SaveFlagStorage)value);
            } else if (IsDictionary(type)) {
                this.WriteDictionary((IDictionary)value);
            } else {
                this.WriteObject(value);
            }
        }

        protected void WritePrimitive(object value)
        {
            var type = value.GetType();

            switch (Type.GetTypeCode(type)) {
                case TypeCode.Boolean: this.Writer.Write((bool)value); break;
                case TypeCode.Byte: this.Writer.Write((byte)value); break;
                case TypeCode.Char: this.Writer.Write((char)value); break;
                case TypeCode.UInt16: this.Writer.Write((ushort)value); break;
                case TypeCode.UInt32: this.Writer.Write((uint)value); break;
                case TypeCode.UInt64: this.Writer.Write((ulong)value); break;
                case TypeCode.SByte: this.Writer.Write((sbyte)value); break;
                case TypeCode.Int16: this.Writer.Write((short)value); break;
                case TypeCode.Int32: this.Writer.Write((int)value); break;
                case TypeCode.Int64: this.Writer.Write((long)value); break;
                case TypeCode.Single: this.Writer.Write((float)value); break;
                case TypeCode.Double: this.Writer.Write((double)value); break;
            }
        }

        protected void WriteList(IList list, TypeCode lengthType = TypeCode.Int32, int length = 0, int max = 0, bool isMessagePackList = false)
        {
            if (length == 0) {
                switch (lengthType) {
                    case TypeCode.Byte: this.Writer.Write((byte)list.Count); break;
                    case TypeCode.Char: this.Writer.Write((char)list.Count); break;
                    case TypeCode.UInt16: this.Writer.Write((ushort)list.Count); break;
                    case TypeCode.UInt32: this.Writer.Write((uint)list.Count); break;
                    case TypeCode.UInt64: this.Writer.Write((ulong)list.Count); break;
                    case TypeCode.SByte: this.Writer.Write((sbyte)list.Count); break;
                    case TypeCode.Int16: this.Writer.Write((short)list.Count); break;
                    case TypeCode.Int32: this.Writer.Write((int)list.Count); break;
                    case TypeCode.Int64: this.Writer.Write((long)list.Count); break;
                }
            }

            foreach (object value in list) {
                if (isMessagePackList) {
                    this.WriteMessagePackObject(value);
                } else {
                    this.WriteValue(value);
                }
            }
            //The only instance of the use of max, doesn't seem to have an effect regardless of the length is (i.e. FurnitureData)
        }

        protected void WriteString(string value, int max = 0)
        {
            var data = Encoding.Unicode.GetBytes(value);
            if (max != 0) {
                this.Writer.Write(data.Length);
                for (int index = 0; index < max; index++) {
                    if (index < data.Length) {
                        this.Writer.Write(data[index]);
                    } else {
                        this.Writer.Write((byte)0x0);
                    }
                }
            } else {
                // This assumes everything else adds 0 to the end. Might need another attribute
                for (int index = 0; index < data.Length; index++) {
                    this.Writer.Write(data[index]);
                    this.Writer.Write((byte)0x0);
                }
                this.Writer.Write((byte)0x0);
                this.Writer.Write((byte)0x0);
                //for (int index = 0; index < data.Length; index++)
                //{
                //    Writer.Write(data[index]);
                //}
            }
            return;
        }
        protected void WriteSaveFlagStorage(SaveFlagStorage saveFlagStorage)
        {
            this.Writer.Write(saveFlagStorage.Length);
            this.Writer.Write(saveFlagStorage.Data);
        }


        protected void WriteDictionary(IDictionary dictionary)
        {
            this.Writer.Write(dictionary.Count);

            foreach (DictionaryEntry item in dictionary) {
                this.WriteValue(item.Key);
                this.WriteValue(item.Value);
            }
        }

        protected void WriteField(object fieldValue, FieldInfo fieldInfo)
        {

            Type fieldType = fieldInfo.FieldType;

            if (fieldInfo.IsDefined(typeof(ElizaAttribute), inherit: true)) {

                //TODO: Versioning tags

                if (fieldInfo.IsDefined(typeof(ElizaMessagePackListAttribute))) {
                    if (IsList(fieldType)) {
                        this.WriteList((IList)fieldValue, isMessagePackList: true);
                        return;
                    }
                }

                if (fieldInfo.IsDefined(typeof(ElizaMessagePackRawAttribute))) {
                    this.WriteMessagePackObject(fieldValue);
                    return;
                }

                if (fieldInfo.IsDefined(typeof(ElizaSizeAttribute))) {
                    var lengthAttribute = (ElizaSizeAttribute)fieldInfo.GetCustomAttribute(typeof(ElizaSizeAttribute));
                    if (lengthAttribute.Fixed != 0) {
                        if (IsList(fieldType)) {
                            this.WriteList((IList)fieldValue, length: lengthAttribute.Fixed);
                            return;
                        } else if (fieldType == typeof(string)) {
                            this.WriteString((string)fieldValue, lengthAttribute.Fixed);
                            return;
                        }
                    } else if (lengthAttribute.LengthType != TypeCode.Empty) {
                        if (IsList(fieldType)) {
                            this.WriteList((IList)fieldValue, lengthType: lengthAttribute.LengthType);
                            return;
                        } else if (fieldType == typeof(string)) {
                            // Not supported for strings
                        }
                    } else if (lengthAttribute.Max != 0) {
                        if (IsList(fieldType)) {
                            this.WriteList((IList)fieldValue, max: lengthAttribute.Max);
                            return;
                        } else if (fieldType == typeof(string)) {
                            this.WriteString((string)fieldValue, max: lengthAttribute.Max);
                            return;
                        }
                    }
                }

            }

            this.WriteValue(fieldValue);
        }

        protected void WriteObject(object objectValue)
        {
            var objectType = objectValue.GetType();

            // MessagePackObject
            if (objectType.IsDefined(typeof(MessagePackObjectAttribute))) {
                this.WriteMessagePackObject(objectValue);
                return;
            }

            foreach (FieldInfo fieldInfo in GetFieldsOrdered(objectType)) {
                object fieldValue = fieldInfo.GetValue(objectValue);
                this.WriteField(fieldValue, fieldInfo);
            }

        }

        protected void WriteMessagePackObject(object value)
        {
            var bytes = MessagePackSerializer.Serialize(value);
            this.Writer.Write(bytes.Length);
            this.Writer.Write(bytes);
        }
    }
}
