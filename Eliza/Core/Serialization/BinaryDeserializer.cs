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
            Reader = new BinaryReader(baseStream);
        }

        public T Deserialize<T>()
        {
            return (T)ReadValue(typeof(T));
        }

        private object ReadValue(Type type)
        {
            if (type.IsPrimitive)
            {
                return ReadPrimitive(Type.GetTypeCode(type));
            }
            else if (IsList(type))
            {
                return ReadList(type);
            }
            else if (type == typeof(string))
            {
                return ReadString();
            }
            else if (type == typeof(SaveFlagStorage))
            {
                return ReadSaveFlagStorage();
            }
            else if (type == typeof(RF5SaveDataFooter))
            {
                return ReadSaveDataFooter(type);
            }
            else if (IsDictionary(type))
            {
                return ReadDictionary(type);
            }
            else
            {
                return ReadObject(type);
            }
        }

        private object ReadPrimitive(TypeCode type)
        {
            switch (type)
            {
                case TypeCode.Boolean: return Reader.ReadBoolean();
                case TypeCode.Byte: return Reader.ReadByte();
                case TypeCode.Char: return Reader.ReadChar();
                case TypeCode.UInt16: return Reader.ReadUInt16();
                case TypeCode.UInt32: return Reader.ReadUInt32();
                case TypeCode.UInt64: return Reader.ReadUInt64();
                case TypeCode.SByte: return Reader.ReadSByte();
                case TypeCode.Int16: return Reader.ReadInt16();
                case TypeCode.Int32: return Reader.ReadInt32();
                case TypeCode.Int64: return Reader.ReadInt64();
                case TypeCode.Single: return Reader.ReadSingle();
                case TypeCode.Double: return Reader.ReadDouble();

                default: return null;
            }
        }

        IList ReadList(Type type, TypeCode lengthType = TypeCode.Int32, int length = 0, int max = 0, bool isMessagePackList = false)
        {
            IList ilist;

            length = length == 0 ? Convert.ToInt32(ReadPrimitive(lengthType)) : length;

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
                    var value = ReadValue(type);

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
                    var value = isMessagePackList ? ReadMessagePackObject(type) : ReadValue(type);

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

        private string ReadString(int size = 0, int max = 0)
        {
            List<byte> dataString = new List<byte>();

            //Might be deprecated
            if (size != 0)
            {
                var data = Reader.ReadBytes(size);

                return Encoding.Unicode.GetString(
                    data
                );
            }
            else if (max != 0)
            {
                var length = Reader.ReadInt32();
                var data = Reader.ReadBytes(length);
                BaseStream.Seek(max - length, SeekOrigin.Current);

                return Encoding.Unicode.GetString(
                    data
                );
            }
            else
            {
                do
                {
                    var character = Reader.ReadByte();
                    var nullCharacter = Reader.ReadByte();

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

        private SaveFlagStorage ReadSaveFlagStorage()
        {
            var data = new List<byte>();
            var length = Reader.ReadInt32();
            int index = 0;
            bool flag;
            do
            {
                data.Add(Reader.ReadByte());
                flag = index++ < (length - 1) >> 3;
            } while (flag);

            return new SaveFlagStorage(data.ToArray(), length);
        }

        private RF5SaveDataFooter ReadSaveDataFooter(Type type)
        {
            //Aligned relative to data 256bits due to Rijndael crypto
            BaseStream.Position = ((BaseStream.Position - 0x20 + 0x1F) & ~0x1F) + 0x20;
            return (RF5SaveDataFooter)ReadObject(type);
        }

        private IDictionary ReadDictionary(Type type)
        {
            Type[] arguments = type.GetGenericArguments();
            Type keyType = arguments[0];
            Type valueType = arguments[1];

            Type dictType = typeof(Dictionary<,>).MakeGenericType(keyType, valueType);

            var dict = (IDictionary)Activator.CreateInstance(dictType);

            var length = Reader.ReadInt32();


            for (int index = 0; index < length; index++)
            {
                if (valueType == typeof(HumanStatusData))
                {

                }
                var keyValue = ReadValue(keyType);
                var valueValue = ReadValue(valueType);
                dict.Add(keyValue, valueValue);
            }

            return dict;
        }

        private object ReadObject(Type objectType)
        {
            var objectValue = Activator.CreateInstance(objectType);

            int fieldCount = 0;

            // MessagePackObject
            if (objectType.IsDefined(typeof(MessagePackObjectAttribute)))
            {
                objectValue = ReadMessagePackObject(objectType);
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
                                fieldValue = ReadList(fieldType, isMessagePackList: true);
                            }
                        }

                        var messagePackRawAttribute = (ElizaMessagePackRawAttribute)info.GetCustomAttribute(typeof(ElizaMessagePackRawAttribute));
                        if (messagePackRawAttribute != null)
                        {
                            fieldValue = ReadMessagePackObject(fieldType);
                        }

                        var lengthAttribute = (ElizaSizeAttribute)info.GetCustomAttribute(typeof(ElizaSizeAttribute));
                        if (lengthAttribute != null)
                        {
                            if (lengthAttribute.Fixed != 0)
                            {
                                if (IsList(fieldType))
                                {
                                    fieldValue = ReadList(fieldType, length: lengthAttribute.Fixed);
                                }
                                else if (fieldType == typeof(string))
                                {
                                    fieldValue = ReadString(lengthAttribute.Fixed);
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
                                    fieldValue = ReadList(fieldType, lengthType: lengthAttribute.LengthType);
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
                                    fieldValue = ReadList(fieldType, max: lengthAttribute.Max);
                                }
                                else if (fieldType == typeof(string))
                                {
                                    fieldValue = ReadString(max: lengthAttribute.Max);
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

                        fieldValue = fieldValue == null ? ReadValue(fieldType) : fieldValue;

                        if (fieldValue != null) info.SetValue(objectValue, fieldValue);
                    }
                }
            }
            return objectValue;
        }
        private object ReadMessagePackObject(Type type)
        {
            var length = Reader.ReadInt32();
            var data = Reader.ReadBytes(length);
            return MessagePackSerializer.Deserialize(type, data);
        }
    }
}
