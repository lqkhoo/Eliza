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
    public class ObjectGraph : IComparable<ObjectGraph>
    {
        public List<ObjectGraph> Keys;
        public List<ObjectGraph> Values;
        public string FieldName;
        public Type Type;
        public object Value;
        public TypeCode LengthType; // For arrays only
        public FieldInfo FieldInfo;
        public object[] Attrs;

        protected ObjectGraph()
        {
            this.Keys = new List<ObjectGraph>();
            this.Values = new List<ObjectGraph>();
            this.Attrs = Array.Empty<object>();
        }

        public ObjectGraph(object objectValue, Type objectType=null,
                            TypeCode lengthType=TypeCode.Empty, FieldInfo fieldInfo=null) : this()
        {
            // TODO: max/min ranges

            this.Value = objectValue;
            if(objectValue != null) {
                if (objectType == null) {
                    this.Type = objectValue.GetType();
                } else if (fieldInfo != null) {
                    this.Type = fieldInfo.FieldType;
                } else {
                    this.Type = null;
                }
            }
            // If value and type are both null, nothing we can do. This is just null.

            this.FieldName = (this.FieldInfo != null) ? this.FieldInfo.Name : "";
            if (lengthType != TypeCode.Empty) {
                this.LengthType = lengthType;
            }
            if(fieldInfo != null) {
                this.FieldInfo = fieldInfo;
                this.Attrs = fieldInfo.GetCustomAttributes(typeof(ElizaAttribute), inherit: true);
            }
        }

        public void AppendKey(ObjectGraph node)
        {
            this.Keys.Add(node);
        }

        public void AppendChild(ObjectGraph node)
        {
            this.Values.Add(node);
        }

        public bool HasAttrs()
        {
            return (this.Attrs.Length == 0);
        }

        public bool IsLeaf()
        {
            return (this.Values.Count == 0);
        }

        public override string ToString()
        {
            return String.Format("Graph: [{0}][{1}] {2}: {3}", this.Keys.Count, this.Values.Count, this.FieldName, this.Type.Name.ToString()); 
        }

        public int CompareTo(ObjectGraph other)
        {
            // TODO
            throw new NotImplementedException();
        }
    }
}
