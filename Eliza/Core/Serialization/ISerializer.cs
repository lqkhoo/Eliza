using System;
using System.Collections;
using System.Reflection;
using Eliza.Model;

namespace Eliza.Core.Serialization
{
    public interface ISerializer
    {
        protected void WriteValue(object value);
        protected void WritePrimitive(object value, TypeCode typeCode = TypeCode.Empty);
        protected void WriteList(IList list,
                                TypeCode lengthType = ElizaListAttribute.DEFAULT_LENGTH_TYPECODE,
                                int length = ElizaListAttribute.UNKNOWN_SIZE,
                                int max = ElizaListAttribute.UNKNOWN_SIZE,
                                bool isMessagePackList = ElizaListAttribute.DEFAULT_ISMESSAGEPACK_LIST);
        protected void WriteString(string value,
                                   TypeCode lengthType = ElizaStringAttribute.DEFAULT_LENGTH_TYPECODE,
                                   int maxLength = ElizaStringAttribute.UNKNOWN_SIZE,
                                   bool isUtf16Uuid = ElizaStringAttribute.DEFAULT_IS_UTF16_UUID);
        protected void WriteSaveFlagStorage(SaveFlagStorage saveFlagStorage);
        protected void WriteDictionary(IDictionary dictionary);
        protected void WriteObject(object objectValue);
        protected void WriteMessagePackObject(object value);
        protected void WriteField(object fieldValue, FieldInfo fieldInfo);
    }
}
