using Eliza.Core.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Eliza.Core.Serialization
{
    // TODO: Implement IComparable

    // This is our basis for constructing a graph out of 
    // the classes representing RF5's save file.
    public class ObjectGraph : IComparable<ObjectGraph>
    {
        public const int NULL_ARRAY_INDEX = -1;
        public const int NULL_MAX_LENGTH = 0;
        public const TypeCode NULL_LENGTH_TYPE = TypeCode.Empty;

        // Core serialization fields
        public Type Type;
        public object Value;
        public ObjectGraph Parent;
        public List<ObjectGraph> Children;

        public TypeCode LengthType = NULL_LENGTH_TYPE; // For arrays only
        // public int ArrayLength = 0; // We don't store dynamic length because that can change
        public int ArrayIndex = NULL_ARRAY_INDEX; // For array-like members only

        // Cached properties. These are redundant. For convenience of access for the wrapper
        // so we don't have to search through the whole array of FieldInfo every time.
        // Cache of the property from ElizaStringAttribute
        public bool isUtf16UuidString = false; // For strings only.
        // Cache of the property MaxSize from either ElizaString or ElizaList attributes.
        public int MaxLength = NULL_MAX_LENGTH;

        // Attribute-related properties
        public FieldInfo FieldInfo;
        public object[] Attrs;

        // "Method-like properties"
        public bool HasAttrs { get => this.Attrs.Length == 0; }
        public bool IsRoot { get => this.Parent == null; }
        public bool IsLeaf { get => this.Children.Count == 0; }

        protected ObjectGraph()
        {
            this.Children = new List<ObjectGraph>();
            this.Attrs = Array.Empty<object>();
        }

        public ObjectGraph(object objectValue,
                            TypeCode lengthType= NULL_LENGTH_TYPE,
                            int arrayIndex=NULL_ARRAY_INDEX,
                            FieldInfo fieldInfo=null)
            : this()
        {
            // TODO: max/min ranges

            this.Value = objectValue;

            // Get type information. At least one case must be true.
            // The check order here is critical to handle polymorphism correctly.
            // If the object has a type, use its type.
            // Otherwise the object is null so just use declared field type,
            // which could be an abstract class or interface.
            if(objectValue != null) {
                this.Type = objectValue.GetType();
            } else {
                if(fieldInfo != null) {
                    this.Type = fieldInfo.FieldType;
                } else {
                    // This should never happen
                }
            }

            if (lengthType != TypeCode.Empty) {
                this.LengthType = lengthType;
            }
            if (fieldInfo != null) {
                this.FieldInfo = fieldInfo;
                this.Attrs = fieldInfo.GetCustomAttributes(typeof(ElizaAttribute), inherit: true);
            }

        }


        // For the wrapper to call. Can't reference wrapper due to circular dependency.
        public ObjectGraph(Type type, object value, ObjectGraph parent, List<ObjectGraph> children,
            TypeCode lengthType, int arrayIndex, bool isUtf16UuidString, int maxLength,
            FieldInfo fieldInfo, object[] attrs)
            : this()
        {
            this.Type = type;
            this.Value = value;
            this.Parent = parent;
            this.Children = children;
            this.LengthType = lengthType;
            this.ArrayIndex = arrayIndex;
            this.isUtf16UuidString = isUtf16UuidString;
            this.MaxLength = maxLength;
            this.Attrs = attrs;
        }

        public void AppendChild(ObjectGraph child, int arrayIndex=NULL_ARRAY_INDEX)
        {
            if(arrayIndex != NULL_ARRAY_INDEX) {
                child.ArrayIndex = arrayIndex;
            }
            child.Parent = this;
            this.Children.Add(child);
        }

        protected string GetDisplayFieldName()
        {
            string str = (this.FieldInfo != null) ? this.FieldInfo.Name : "";
            if (this.Type != null) {
                if (BaseSerializer.IsList(this.Type)
                    || BaseSerializer.IsDictionary(this.Type)) {
                    str += String.Format("[{0}]", this.Children.Count);
                }
            }
            return str;
        }

        private List<ObjectGraph> GetAncestors()
        {
            List<ObjectGraph> ancestors = new();
            ObjectGraph currentNode = this;
            ObjectGraph parentNode;
            do {
                parentNode = currentNode.Parent;
                ancestors.Add(currentNode);
                currentNode = parentNode;
            }
            while (parentNode != null);
            return ancestors;
        }

        public override string ToString()
        {
            return String.Format("Graph: [{1}] {2}: {3}", this.Children.Count, this.GetDisplayFieldName(), this.Type.Name); 
        }

        public int CompareTo(ObjectGraph other)
        {
            // TODO
            throw new NotImplementedException();
        }
    }

}
