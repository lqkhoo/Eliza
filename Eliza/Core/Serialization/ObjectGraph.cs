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

    // This is a semantically-rich object encapsulating
    // both the functions of the class it is derived from, as well
    // as exposing values in a flat list of properties so that
    // they could be bound easily to the UI.
    public class ObjectGraph : IComparable<ObjectGraph>
    {
        public const int NULL_ARRAY_INDEX = -1;

        // Core serialization fields
        public Type Type;
        public object Value;
        public ObjectGraph Parent = null;
        public TypeCode LengthType; // For arrays only
        public int ArrayIndex = NULL_ARRAY_INDEX; // For array members only
        public List<ObjectGraph> Keys;
        public List<ObjectGraph> Values;

        // Attribute-related properties
        public FieldInfo FieldInfo;
        public object[] Attrs;

        // "Method-like properties"
        public bool HasAttrs { get => this.Attrs.Length == 0; }
        public bool IsRoot { get => this.Parent == null; }
        public bool IsLeaf { get => this.Values.Count == 0; }

        // UI binding properties
        public ObjectGraph UiThis { get => this; }
        // public ObjectGraphUiWrapper Ui { get; protected set; }

        protected ObjectGraph()
        {
            this.Keys = new List<ObjectGraph>();
            this.Values = new List<ObjectGraph>();
            this.Attrs = Array.Empty<object>();
        }

        public ObjectGraph(object objectValue, Type objectType=null,
                            TypeCode lengthType=TypeCode.Empty,
                            int arrayIndex=NULL_ARRAY_INDEX,
                            FieldInfo fieldInfo=null)
            : this()
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

            if (lengthType != TypeCode.Empty) {
                this.LengthType = lengthType;
            }
            if (fieldInfo != null) {
                this.FieldInfo = fieldInfo;
                this.Attrs = fieldInfo.GetCustomAttributes(typeof(ElizaAttribute), inherit: true);
            }

        }


        // For the wrapper to call. Can't reference wrapper due to circular dependency.
        public ObjectGraph(Type type, object value, ObjectGraph parent, List<ObjectGraph> keys,
            List<ObjectGraph> values, TypeCode lengthType, int arrayIndex, FieldInfo fieldInfo, object[] attrs)
            : this()
        {
            this.Type = type;
            this.Value = value;
            this.Parent = parent;
            this.Keys = keys;
            this.Values = values;
            this.LengthType = lengthType;
            this.ArrayIndex = arrayIndex;
            this.Attrs = attrs;
        }

        public void AppendKey(ObjectGraph child, int arrayIndex=NULL_ARRAY_INDEX)
        {
            if (arrayIndex != NULL_ARRAY_INDEX) {
                child.ArrayIndex = arrayIndex;
            }
            child.Parent = this;
            this.Keys.Add(child);
        }

        public void AppendChild(ObjectGraph child, int arrayIndex=NULL_ARRAY_INDEX)
        {
            if(arrayIndex != NULL_ARRAY_INDEX) {
                child.ArrayIndex = arrayIndex;
            }
            child.Parent = this;
            this.Values.Add(child);
        }

        protected string GetDisplayFieldName()
        {
            string str = (this.FieldInfo != null) ? this.FieldInfo.Name : "";
            if (this.Type != null) {
                if (BaseSerializer.IsList(this.Type)
                    || BaseSerializer.IsDictionary(this.Type)) {
                    str += String.Format("[{0}]", this.Values.Count);
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
            return String.Format("Graph: [{0}][{1}] {2}: {3}", this.Keys.Count, this.Values.Count, this.GetDisplayFieldName(), this.Type.Name); 
        }

        public int CompareTo(ObjectGraph other)
        {
            // TODO
            throw new NotImplementedException();
        }
    }



    /*
    public class ObjectGraphUiWrapper : ObjectGraph
    {
        public readonly ObjectGraph Node;
        public ObjectGraphUiWrapper(ObjectGraph node)
        {
            this.Node = node;
        }

        // These are value converter implementations to bypass Avalonia's constraints.
        public string DisplayType
        {
            get
            {
                string str = this.Node.Type.Name;
                if (this.Node.ArrayIndex != ObjectGraph.NULL_ARRAY_INDEX) {
                    str += " " + this.Node.ArrayIndex.ToString();
                }
                return str;
            }
            set
            {
                throw new NotImplementedException(); // One-way
            }
        }

        public string DisplayFieldName
        {
            get
            {
                string str = (this.Node.FieldInfo != null) ? this.Node.FieldInfo.Name : "";
                if (this.Node.Type != null) {
                    if (BaseSerializer.IsList(this.Node.Type)
                        || BaseSerializer.IsDictionary(this.Node.Type)) {
                        str += String.Format("[{0}]", this.Node.Values.Count);
                    }
                }
                return str;
            }
            set
            {
                throw new NotImplementedException(); // One-way
            }
        }

        public string DisplayValue
        {
            get
            {
                string str;
                if (this.Node.Type == null || !this.Node.Type.IsPrimitive) {
                    str = "";
                } else {
                    str = this.Node.Value.ToString();
                }
                return str;
            }
            set
            {
                throw new NotImplementedException(); // One-way
            }
        }

        public string DisplayAncestry
        {
            get
            {
                string displayString = "";
                List<ObjectGraph> ancestors = this.Node.Ancestors;
                ancestors.Reverse();
                for(int idx=0; idx<ancestors.Count; idx++) {
                    ObjectGraph node = ancestors[idx];
                    if(idx < ancestors.Count-1) {
                        displayString += node.Ui.DisplayType + ": " + node.Ui.DisplayFieldName + "   >   ";
                    } else {
                        displayString += node.Ui.DisplayType + ": " + node.Ui.DisplayFieldName;
                    }
                }
                return displayString;
            }
            set
            {
                throw new NotImplementedException(); // One-way
            }
        }

    }
    */

}
