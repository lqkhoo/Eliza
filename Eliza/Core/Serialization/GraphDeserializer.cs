using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Eliza.Model;
using Eliza.Model.Save;
using Eliza.Model.Status;

namespace Eliza.Core.Serialization
{
    // The GraphDeserializer takes an ObjectGraph and outputs
    // an object.
    public class GraphDeserializer : BaseSerializer
    {
        public GraphDeserializer(SaveData.LOCALE locale, int version)
            : base(locale, version) { }


        public SaveData ReadRF5Save(ObjectGraph node)
        {
            SaveData save = new(this.Locale, this.Version);
            save.header = this.ReadSaveDataHeader(node.Children[0]);
            save.saveData = this.ReadSaveData(node.Children[1]);
            save.footer = this.ReadSaveDataFooter(node.Children[2]);
            return save;
        }

        public RF5SaveDataHeader ReadSaveDataHeader(ObjectGraph node)
        {
            return (RF5SaveDataHeader)this.ReadObject(node);
        }

        public RF5SaveData ReadSaveData(ObjectGraph node)
        {
            return (RF5SaveData)this.ReadObject(node);
        }

        public RF5SaveDataFooter ReadSaveDataFooter(ObjectGraph node)
        {
            return (RF5SaveDataFooter)this.ReadObject(node);
        }

        protected object ReadValue(ObjectGraph node)
        {
            // Type CANNOT be null. If it is, there's
            // an error in the serialization process somewhere.
            if(node.Value == null) {
                return null;
            } else {
                Type type = node.Type;
                if(type.IsPrimitive) {
                    return this.ReadPrimitive(node);
                } else if ( IsList(type)) {
                    return this.ReadList(node);
                } else if (type == typeof(string)) {
                    return this.ReadString(node);
                } else if (IsDictionary(type)) {
                    return this.ReadDictionary(node);
                } else {
                    return this.ReadObject(node);
                }
            }
        }

        protected object ReadPrimitive(ObjectGraph node)
        {
            if(node.Value == null) {
                return null;
            } else {
                TypeCode type = Type.GetTypeCode(node.Type);
                switch (type) {
                    case TypeCode.Boolean: return (bool)node.Value;
                    case TypeCode.Byte: return (byte)node.Value;
                    case TypeCode.Char: return (char)node.Value;
                    case TypeCode.UInt16: return (UInt16)node.Value;
                    case TypeCode.UInt32: return (UInt32)node.Value;
                    case TypeCode.UInt64: return (UInt64)node.Value;
                    case TypeCode.SByte: return (SByte)node.Value;
                    case TypeCode.Int16: return (Int16)node.Value;
                    case TypeCode.Int32: return (Int32)node.Value;
                    case TypeCode.Int64: return (Int64)node.Value;
                    case TypeCode.Single: return (Single)node.Value;
                    case TypeCode.Double: return (Double)node.Value;
                    default: return null;
                }
            }
        }

        protected IList ReadList(ObjectGraph node)
        {
            if (node.Value == null) {
                return null;
            } else {
                Type nodeType = node.Type;
                Type contentType;
                IList ilist;

                int length;
                if (node.MaxLength == ObjectGraph.NULL_MAX_LENGTH) {
                    length = node.Children.Count;
                } else {
                    length = node.MaxLength;
                }

                if (nodeType.IsArray) {
                    contentType = nodeType.GetElementType();
                    ilist = Array.CreateInstance(contentType, length);
                } else {
                    ilist = (IList)Activator.CreateInstance(nodeType);
                    contentType = nodeType.GetGenericArguments()[0];
                }

                for (int idx = 0; idx < length; idx++) {
                    object value = this.ReadValue(node.Children[idx]);
                    if (ilist.IsFixedSize) {
                        ilist[idx] = value;
                    } else {
                        ilist.Add(value);
                    }
                }
                return ilist;
            }
        }

        protected string ReadString(ObjectGraph node)
        {
            return (node.Value == null) ? null : (string)node.Value;
        }

        protected IDictionary ReadDictionary(ObjectGraph node)
        {
            if(node.Value == null) {
                return null;
            } else {
                Type nodeType = node.Type;
                Type[] arguments = nodeType.GetGenericArguments();
                Type keyType = arguments[0];
                Type valueType = arguments[1];
                Type dictType = typeof(Dictionary<,>).MakeGenericType(keyType, valueType);

                var dict = (IDictionary)Activator.CreateInstance(dictType);
                int length = node.Children.Count / 2;
                for(int idx=0; idx<length; idx++) {
                    object keyValue = this.ReadValue(node.Children[2*idx]);
                    object valueValue = this.ReadValue(node.Children[2*idx+1]);
                    dict.Add(keyValue, valueValue);
                }
                return dict;
            }
        }

        protected object ReadObject(ObjectGraph node)
        {
            if(node.Value == null) {
                return null;
            } else {

                Type objectType = node.Type;
                object fieldValue = null;
                var objectValue = Activator.CreateInstance(objectType);
                
                int idx = 0;
                foreach (FieldInfo fieldInfo in GetFieldsOrdered(objectType)) {

                    // Can't do check like this because it will break polymorphic fields.
                    /*
                    if(fieldInfo.FieldType != node.Children[idx].Type) {
                        // If field was skipped for whatever reason, just move onto
                        // the next. GraphSerializer does not reorder children.
                        continue;
                    }
                    */

                    bool shouldWrite = false;
                    bool hasControlTag = false;

                    // Check if the field has been annotated with one of our attributes
                    if (fieldInfo.IsDefined(typeof(ElizaFlowControlAttribute), inherit: true)) {

                        hasControlTag = true;

                        var elizaAttrs = fieldInfo.GetCustomAttributes(typeof(ElizaFlowControlAttribute), inherit: true);
                        foreach (ElizaFlowControlAttribute elizaAttr in elizaAttrs) {

                            bool isRelevant = ((this.Locale == elizaAttr.Locale
                                                || elizaAttr.Locale == SaveData.LOCALE.ANY)
                                                && this.Version >= elizaAttr.FromVersion);
                            // Don't fail silently if it's trying to write twice. Means wrong use of tags.
                            if (shouldWrite && isRelevant) { throw new UnsupportedAttributeException(elizaAttr, fieldInfo); }
                            if (shouldWrite || (!isRelevant)) { continue; }

                            fieldValue = this.ReadValue(node.Children[idx]);
                            shouldWrite = true;
                        }
                    }

                    if((! hasControlTag) && (! shouldWrite)) {
                        fieldValue = this.ReadValue(node.Children[idx]);
                        shouldWrite = true;
                    }
                    if(shouldWrite == true) {
                        fieldInfo.SetValue(objectValue, fieldValue);
                        idx++;
                    }
                    

                }

                return objectValue;

            }
        }


        // Our job is particularly easy here compared to the other
        // serializers as we don't care about attributes other than
        // version checks. The function has been moved to within ReadObject
        // as it it's easier to manipulate the child index.
        // protected object ReadField(ObjectGraph parent, ObjectGraph child, FieldInfo fieldInfo) { }
    }
}
