﻿using MessagePack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Eliza.Model;
using Eliza.Model.Save;
using static Eliza.Core.Serialization.ElizaFlowControlAttribute;

namespace Eliza.Core.Serialization
{
    class BinarySerializer : BinarySerialization
    {
        public BinaryWriter Writer;

        protected Dictionary<Type, long> _DebugAddressMap;

        public BinarySerializer(Stream baseStream) : base(baseStream)
        {
            this.Writer = new BinaryWriter(baseStream);
            this._DebugAddressMap = new Dictionary<Type, long>();
        }

        public void WriteSaveDataHeader(RF5SaveDataHeader header) {
            this.WriteObject(header);
        }

        public void WriteSaveData(RF5SaveData saveData) {
            this.WriteObject(saveData);
        }

        public void WriteSaveDataFooter(RF5SaveDataFooter footer) {
            this.WriteObject(footer);
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

        protected void WriteList(IList list,
                                TypeCode lengthType=ElizaListAttribute.DEFAULT_LENGTH_TYPECODE,
                                int length=ElizaListAttribute.UNKNOWN_SIZE,
                                int max=ElizaListAttribute.UNKNOWN_SIZE,
                                bool isMessagePackList=ElizaListAttribute.DFEAULT_ISMESSAGEPACK_LIST)
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

            // The max parameter isn't used here because we've already
            // allocated memory equal to max items in the list.

            foreach (object value in list) {
                if (isMessagePackList) {
                    this.WriteMessagePackObject(value);
                } else {
                    this.WriteValue(value);
                }
            }

        }
        protected void WriteString(string value,
                                   int max = ElizaStringAttribute.UNKNOWN_SIZE,
                                   bool isUtf16Uuid=ElizaStringAttribute.DEFAULT_IS_UTF16_UUID)
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
            bool hasWritten = false;

            if (fieldInfo.IsDefined(typeof(ElizaFlowControlAttribute), inherit: true)) {

                //TODO: Versioning tags

                if (fieldInfo.IsDefined(typeof(ElizaStringAttribute))) {
                    var stringAttr = (ElizaStringAttribute)fieldInfo.GetCustomAttribute(typeof(ElizaStringAttribute));
                    if (fieldType == typeof(string)) {
                        this.WriteString(
                            value: (string)fieldValue,
                            max: stringAttr.MaxSize,
                            isUtf16Uuid: stringAttr.IsUtf16Uuid
                        );
                        hasWritten = true;
                    } else {
                        throw new UnsupportedAttributeException(stringAttr, fieldInfo);
                    }

                } else if (fieldInfo.IsDefined(typeof(ElizaListAttribute))) {
                    var listAttr = (ElizaListAttribute)fieldInfo.GetCustomAttribute(typeof(ElizaListAttribute));
                    if (IsList(fieldType)) {
                        this.WriteList(
                            list: (IList)fieldValue,
                            lengthType: listAttr.LengthType,
                            length: listAttr.FixedSize,
                            max: listAttr.MaxSize,
                            isMessagePackList: listAttr.IsMessagePackList
                        );
                        hasWritten = true;
                    } else {
                        throw new UnsupportedAttributeException(listAttr, fieldInfo);
                    }
                }

            }

            // If there were no Eliza attributes, or if it was not a directive
            // that changes how we write the field, do the simplest case.
            if (! hasWritten) {
                this.WriteValue(fieldValue);
            }
            
        }

        protected void WriteObject(object objectValue)
        {
            var objectType = objectValue.GetType();

            // Populate this._DebugList
            if (this._DebugTypeSet.Contains(objectType)) {
                // Helpful to breakpoint this line.
                this._DebugAddressMap.Add(objectType, this.BaseStream.Position);
            }

            if (objectType.IsDefined(typeof(MessagePackObjectAttribute))) {
                // MessagePackObject
                this.WriteMessagePackObject(objectValue);

            } else {
                foreach (FieldInfo fieldInfo in GetFieldsOrdered(objectType))
                {
                    object fieldValue = fieldInfo.GetValue(objectValue);
                    this.WriteField(fieldValue, fieldInfo);
                }
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
