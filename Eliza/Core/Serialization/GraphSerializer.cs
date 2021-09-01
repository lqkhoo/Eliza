using System;
using System.Collections;
using System.Reflection;
using Eliza.Model;
using Eliza.Model.Save;

namespace Eliza.Core.Serialization
{
    // For a graph, anything that writes primitives e.g. string
    // should return the primitive. Otherwise return a node.

    // Remember: for WriteList, WriteDict or any method that handles
    // unwrapping some already-serialized object,
    // since we don't have FieldInfo to operate off of, we always need
    // to append type information to the ObjectGraph being generated off
    // of the list elements, because the value itself could be null,
    // which means there's no other way to infer type information.

    // The GraphSerializer takes any object and outputs an ObjectGraph
    public class GraphSerializer : BaseSerializer
    {

        public GraphSerializer(SaveData.LOCALE locale, int version)
            : base(locale, version) { }

        // Be aware that this method only serializes the header, save, and footer.
        public ObjectGraph WriteRF5Save(SaveData save)
        {
            ObjectGraph baseNode = new(save);

            // Make use of IElizaEnumerableClass
            foreach (Tuple<FieldInfo, object> tup in save) {
                FieldInfo fieldInfo = tup.Item1;
                object objectValue = tup.Item2;
                ObjectGraph child = this.WriteObject(objectValue, fieldInfo);
                baseNode.AppendChild(child);
            }
            return baseNode;
        }

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

        protected ObjectGraph WriteNull(FieldInfo fieldInfo)
        {
            ObjectGraph node = new(objectValue: null, fieldInfo: fieldInfo);
            return node;
        }

        protected ObjectGraph WriteValue(object value, FieldInfo fieldInfo=null)
        {
            ObjectGraph node;
            Type type;
            if(value == null) {
                node = this.WriteNull(fieldInfo);
            } else {
                type = value.GetType();
                if (type.IsPrimitive) {
                    node = this.WritePrimitive(value, fieldInfo);
                } else if (IsList(type)) {
                    node = this.WriteList((IList)value, fieldInfo: fieldInfo);
                } else if (type == typeof(string)) {
                    node = this.WriteString((string)value, fieldInfo: fieldInfo);
                // } else if (type == typeof(SaveFlagStorage)) {
                //     node = this.WriteSaveFlagStorage((SaveFlagStorage)value, fieldInfo);
                } else if (IsDictionary(type)) {
                    node = this.WriteDictionary((IDictionary)value, fieldInfo);
                } else {
                    node = this.WriteObject(value, fieldInfo);
                }
            }

            return node;
        }

        protected ObjectGraph WritePrimitive(object value, FieldInfo fieldInfo)
        {
            ObjectGraph node;
            if(value == null) {
                node = this.WriteNull(fieldInfo);
            } else {
                node = new(objectValue: value, fieldInfo: fieldInfo);
            }
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
                                    lengthType: lengthType,
                                    fieldInfo: fieldInfo);
            for(int idx=0; idx<list.Count; idx++) {

                ObjectGraph child = this.WriteValue(list[idx]);
                if(list[idx] != null) {
                    child.Type = list[idx].GetType();
                } else {
                    child.Type = list.GetType().GetElementType();
                }
                node.AppendChild(child, idx);
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
            node.isUtf16UuidString = isUtf16Uuid;
            return node;
        }

        /*
        protected ObjectGraph WriteSaveFlagStorage(SaveFlagStorage saveFlagStorage, FieldInfo fieldInfo=null)
        {
            return this.WriteObject(saveFlagStorage, fieldInfo);
        }
        */

        // Note that we simply interleave key and value nodes.
        // Packing as a tuple makes UI binding difficult.
        // Alternative is to make the value node a child of the key node, but that
        // breaks node ancestry.
        protected ObjectGraph WriteDictionary(IDictionary dictionary, FieldInfo fieldInfo=null)
        {
            ObjectGraph node = new(objectValue: dictionary,
                                    fieldInfo: fieldInfo);
            Type[] arguments = dictionary.GetType().GetGenericArguments();
            Type keyType = arguments[0];
            Type valueType = arguments[1];
            int idx = 0;
            foreach(var key in dictionary.Keys) {
                ObjectGraph keyNode = this.WriteValue(key);
                keyNode.Type = keyType; // Specify type info from here.
                ObjectGraph valueNode = this.WriteValue(dictionary[key]);
                valueNode.Type = valueType;
                node.AppendChild(keyNode, idx);
                node.AppendChild(valueNode, idx);
                idx++;
            }

            return node;
        }

        public ObjectGraph WriteObject(object objectValue, FieldInfo fieldInfo=null)
        {
            ObjectGraph node;
            if(objectValue == null) {
                node = this.WriteNull(fieldInfo);
            } else {
                var objectType = objectValue.GetType();
                node = new(objectValue: objectValue, fieldInfo: fieldInfo);
                foreach (FieldInfo childFieldInfo in GetFieldsOrdered(objectType)) {
                    object fieldValue = childFieldInfo.GetValue(objectValue);
                    this.WriteField(node, fieldValue, childFieldInfo);                    
                }
            }
            return node;
        }

        protected void WriteField(ObjectGraph parent, object fieldValue, FieldInfo fieldInfo=null)
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
            if(node != null) {
                parent.AppendChild(node);
            }
        }

    }
}
