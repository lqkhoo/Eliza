using System;
using System.Collections;
using System.Reflection;
using Eliza.Model;
using Eliza.Model.Save;

namespace Eliza.Core.Serialization
{
    // For a graph, anything that writes primitives e.g. string
    // should return the primitive. Otherwise return a node.

    public class GraphSerializer : BaseSerializer
    {

        public GraphSerializer(SaveData.LOCALE locale, int version)
            : base(locale, version) { }

        public ObjectGraph WriteSaveDataHeader(RF5SaveDataHeader header)
        {
            return this.WriteObject(header);
        }

        public ObjectGraph WriteSaveData(RF5SaveData saveData)
        {
            return this.WriteObject(saveData);
        }

        public ObjectGraph WriteSaveDataFooter(RF5SaveDataFooter footer)
        {
            return this.WriteObject(footer);
        }

        protected ObjectGraph WriteValue(object value, FieldInfo fieldInfo)
        {
            ObjectGraph node;
            var type = value.GetType();

            if (type.IsPrimitive) {
                node = this.WritePrimitive(value, fieldInfo);
            } else if (IsList(type)) {
                node = this.WriteList((IList)value, fieldInfo: fieldInfo);
            } else if (type == typeof(string)) {
                node = this.WriteString((string)value, fieldInfo: fieldInfo);
            } else if (type == typeof(SaveFlagStorage)) {
                node = this.WriteSaveFlagStorage((SaveFlagStorage)value, fieldInfo);
            } else if (IsDictionary(type)) {
                node = this.WriteDictionary((IDictionary)value, fieldInfo);
            } else {
                node = this.WriteObject(value, fieldInfo);
            }
            return node;
        }

        protected ObjectGraph WritePrimitive(object value, FieldInfo fieldInfo)
        {
            ObjectGraph node = new(objectValue: value, fieldInfo: fieldInfo);
            return node;
        }

        protected ObjectGraph WriteList(IList list,
                                TypeCode lengthType = ElizaListAttribute.DEFAULT_LENGTH_TYPECODE,
                                int length = ElizaListAttribute.UNKNOWN_SIZE,
                                int max = ElizaListAttribute.UNKNOWN_SIZE,
                                bool isMessagePackList = ElizaListAttribute.DEFAULT_ISMESSAGEPACK_LIST,
                                FieldInfo fieldInfo=null)
        {
            ObjectGraph node = new(objectValue: list,
                                    objectType: list.GetType(),
                                    lengthType: lengthType,
                                    fieldInfo: fieldInfo);
            foreach(var item in list) {
                node.AppendChild(this.WriteObject(item));
            }
            return node;
        }

        protected ObjectGraph WriteString(string value,
                                   TypeCode lengthType = ElizaStringAttribute.DEFAULT_LENGTH_TYPECODE,
                                   int maxLength = ElizaStringAttribute.UNKNOWN_SIZE,
                                   bool isUtf16Uuid = ElizaStringAttribute.DEFAULT_IS_UTF16_UUID,
                                   FieldInfo fieldInfo=null)
        {
            ObjectGraph node = new(value, fieldInfo: fieldInfo);
            return node;
        }

        protected ObjectGraph WriteSaveFlagStorage(SaveFlagStorage saveFlagStorage, FieldInfo fieldInfo=null)
        {
            return this.WriteObject(saveFlagStorage, fieldInfo);
        }

        protected ObjectGraph WriteDictionary(IDictionary dictionary, FieldInfo fieldInfo=null)
        {
            // objectValue isn't used but null might be confused with
            // an actual null field, so just copy the original dict.
            ObjectGraph node = new(objectValue: dictionary,
                                    objectType: dictionary.GetType(),
                                    fieldInfo: fieldInfo);

            var keys = dictionary.Keys;
            foreach(var key in keys) {
                node.AppendKey(this.WriteObject(key));
            }
            var values = dictionary.Values;
            foreach(var value in values) {
                node.AppendChild(this.WriteObject(value));
            }

            return node;
        }

        protected ObjectGraph WriteObject(object objectValue, FieldInfo fi=null)
        {
            var objectType = objectValue.GetType();

            ObjectGraph node = new(objectValue: objectValue, fieldInfo: fi);
            foreach (FieldInfo fieldInfo in GetFieldsOrdered(objectType)) {
                object fieldValue = fieldInfo.GetValue(objectValue);
                ObjectGraph child = this.WriteField(fieldValue, fieldInfo);
                node.AppendChild(child);
            }
            return node;
        }

        protected ObjectGraph WriteField(object fieldValue, FieldInfo fieldInfo=null)
        {
            // See BinaryDeserializer for notes.
            ObjectGraph node = null;

            Type fieldType = fieldInfo.FieldType;

            bool hasWritten = false;
            bool hasControlTag = false;

            // Check if the field has been annotated with one of our attributes
            if (fieldInfo.IsDefined(typeof(ElizaFlowControlAttribute), inherit: true)) {

                hasControlTag = true;

                var elizaAttrs = fieldInfo.GetCustomAttributes(typeof(ElizaFlowControlAttribute), inherit: true);
                foreach (ElizaFlowControlAttribute elizaAttr in elizaAttrs) {

                    // Do the version check here. If it doesn't match, skip to the next attribute.
                    bool isRelevant = ((this.Locale == elizaAttr.Locale
                                        || elizaAttr.Locale == SaveData.LOCALE.ANY)
                                        && this.Version >= elizaAttr.FromVersion);

                    // Don't fail silently if it's trying to write twice. Means wrong use of tags.
                    if (hasWritten && isRelevant) { throw new UnsupportedAttributeException(elizaAttr, fieldInfo); }

                    if (hasWritten || (!isRelevant)) { continue; }

                    // Now process the tag based on its type.
                    Type attrType = elizaAttr.GetType();

                    if (attrType == typeof(ElizaStringAttribute)) {
                        var stringAttr = (ElizaStringAttribute)elizaAttr;
                        if (fieldType == typeof(string)) {
                            node = this.WriteString(
                                            value: (string)fieldValue,
                                            maxLength: stringAttr.MaxSize,
                                            isUtf16Uuid: stringAttr.IsUtf16Uuid,
                                            fieldInfo: fieldInfo
                                        );
                            
                            hasWritten = true;
                        } else { throw new UnsupportedAttributeException(stringAttr, fieldInfo); }

                    } else if (attrType == typeof(ElizaListAttribute)) {
                        var listAttr = (ElizaListAttribute)elizaAttr;
                        if (IsList(fieldType)) {
                            node = this.WriteList(
                                        list: (IList)fieldValue,
                                        lengthType: listAttr.LengthType,
                                        length: listAttr.FixedSize,
                                        max: listAttr.MaxSize,
                                        isMessagePackList: listAttr.IsMessagePackList,
                                        fieldInfo: fieldInfo
                                    );
                            hasWritten = true;
                        } else { throw new UnsupportedAttributeException(listAttr, fieldInfo); }

                    } else {
                        // This is a normal field with a tag.
                        node = this.WriteValue(fieldValue, fieldInfo);
                        hasWritten = true;
                    }

                }
            }

            if ((!hasControlTag) && (!hasWritten)) {
                node = this.WriteValue(fieldValue, fieldInfo);
            }

            return node;
        }

    }
}
