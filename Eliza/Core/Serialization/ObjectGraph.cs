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
    public class ObjectGraph
    {
        public List<ObjectGraph> Keys; // Used for dictionaries only.
        public List<ObjectGraph> Values;
        public Type Type;
        public object Value;
        public TypeCode LengthType; // For arrays only
        public object[] Attrs;

        protected ObjectGraph()
        {
            this.Keys = new List<ObjectGraph>();
            this.Values = new List<ObjectGraph>();
            this.Attrs = Array.Empty<object>();
        }

        public ObjectGraph(object objectValue, Type objectType=null,
                            TypeCode lengthType=TypeCode.Empty, FieldInfo fieldInfo=null)
        {
            this.Value = objectValue;
            if(objectType == null) {
                this.Type = objectValue.GetType();
            }
            if(lengthType != TypeCode.Empty) {
                this.LengthType = lengthType;
            }
            if(fieldInfo != null) {
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

        /*
        public override string ToString()
        {
            //TODO
            return this.Type.Name;
        }
        */

    }
}
