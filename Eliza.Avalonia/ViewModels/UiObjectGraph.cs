using Eliza.Core.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using Avalonia.Input;
using ReactiveUI;
using System.Reflection;

namespace Eliza.Avalonia.ViewModels
{
    // Wrapper for Eliza.Core.Serialization.ObjectGraph
    public class UiObjectGraph : ViewModelBase
    {
        // public Type Type;
        // public object Value;
        // public TypeCode LengthType; // For arrays only
        // public int ArrayIndex = NULL_ARRAY_INDEX; // For array members only

        protected UiObjectGraph _This;
        public UiObjectGraph This
        {
            get => this._This;
            set => this.RaiseAndSetIfChanged(ref this._This, value);
        }

        protected Type? _Type;
        public Type? Type
        {
            get => this._Type;
            set => this.RaiseAndSetIfChanged(ref this._Type, value);
        }

        protected object? _Value;
        public object? Value
        {
            get => this._Value;
            set => this.RaiseAndSetIfChanged(ref this._Value, value);
        }

        protected TypeCode _LengthType;
        public TypeCode LengthType
        {
            get => this._LengthType;
            set => this.RaiseAndSetIfChanged(ref this._LengthType, value);
        }

        protected int _ArrayIndex;
        public int ArrayIndex
        {
            get => this._ArrayIndex;
            set => this.RaiseAndSetIfChanged(ref this._ArrayIndex, value);
        }

        protected FieldInfo? _FieldInfo;
        public FieldInfo? FieldInfo
        {
            get => this._FieldInfo;
            set => this.RaiseAndSetIfChanged(ref this._FieldInfo, value);
        }

        protected object[]? _Attrs;
        public object[]? Attrs
        {
            get => this._Attrs;
            set => this.RaiseAndSetIfChanged(ref this._Attrs, value);
        }

        protected UiObjectGraph? _Parent = null;
        public UiObjectGraph? Parent
        {
            get => this._Parent;
            set => this.RaiseAndSetIfChanged(ref this._Parent, value);
        }

        protected List<UiObjectGraph> _Keys = new List<UiObjectGraph>();
        public List<UiObjectGraph> Keys
        {
            get => this._Keys;
            set => this.RaiseAndSetIfChanged(ref this._Keys, value);
        }

        protected List<UiObjectGraph> _Values = new List<UiObjectGraph>();
        public List<UiObjectGraph> Values
        {
            get => this._Values;
            set => this.RaiseAndSetIfChanged(ref this._Values, value);
        }


        // Non-reactive properties

        public double PrimitiveMax
        {
            get
            {
                if (this.Type != null && this.Type.IsPrimitive) {
                    TypeCode typeCode = Type.GetTypeCode(this.Type);
                    switch(typeCode) {
                        case TypeCode.Boolean: return 1;
                        case TypeCode.Byte: return byte.MaxValue;
                        // case TypeCode.Char: return char.MaxValue; // 65535, which is not what we want
                        case TypeCode.Char: return byte.MaxValue;
                        case TypeCode.UInt16: return UInt16.MaxValue;
                        case TypeCode.UInt32: return UInt32.MaxValue;
                        case TypeCode.UInt64: return UInt64.MaxValue;
                        case TypeCode.SByte: return SByte.MaxValue;
                        case TypeCode.Int16: return Int16.MaxValue;
                        case TypeCode.Int32: return Int32.MaxValue;
                        case TypeCode.Int64: return Int64.MaxValue;
                        case TypeCode.Single: return Single.MaxValue;
                        case TypeCode.Double: return Double.MaxValue;
                    }
                }
                return 0;
            }
        }

        public double PrimitiveMin
        {
            get
            {
                if(this.Type != null && this.Type.IsPrimitive) {
                    TypeCode typeCode = Type.GetTypeCode(this.Type);
                    switch (typeCode) {
                        case TypeCode.Boolean: return 0;
                        case TypeCode.Byte: return byte.MinValue;
                        // case TypeCode.Char: return char.MinValue;
                        case TypeCode.Char: return byte.MinValue;
                        case TypeCode.UInt16: return UInt16.MinValue;
                        case TypeCode.UInt32: return UInt32.MinValue;
                        case TypeCode.UInt64: return UInt64.MinValue;
                        case TypeCode.SByte: return SByte.MinValue;
                        case TypeCode.Int16: return Int16.MinValue;
                        case TypeCode.Int32: return Int32.MinValue;
                        case TypeCode.Int64: return Int64.MinValue;
                        case TypeCode.Single: return Single.MinValue;
                        case TypeCode.Double: return Double.MinValue;
                    }
                }
                return 0;
            }
        }

        public List<UiObjectGraph> Ancestors
        {
            get
            {
                List<UiObjectGraph> ancestors = new();
                UiObjectGraph currentNode = this;
                UiObjectGraph parentNode;
                do {
                    parentNode = currentNode.Parent;
                    ancestors.Add(currentNode);
                    currentNode = parentNode;
                }
                while (parentNode != null);
                return ancestors;
            }
        }


        public UiObjectGraph(ObjectGraph node)
        {
            this._This = this;
            this._Type = node.Type;
            this._Value = node.Value;
            this._LengthType = node.LengthType;
            this._ArrayIndex = node.ArrayIndex;
            this._FieldInfo = node.FieldInfo;
            this._Attrs = node.Attrs;

            foreach(ObjectGraph keyNode in node.Keys) {
                UiObjectGraph childKeyNode = new(keyNode);
                childKeyNode.Parent = this;
                this.Keys.Add(childKeyNode);
            }

            foreach (ObjectGraph valueNode in node.Values) {
                UiObjectGraph childValueNode = new(valueNode);
                childValueNode.Parent = this;
                this.Values.Add(childValueNode);
            }

        }

        public static UiObjectGraph Wrap(ObjectGraph root)
        {
            return new UiObjectGraph(root);
        }

        protected static ObjectGraph ShallowUnwrap(UiObjectGraph uiNode)
        {
            ObjectGraph root = new(
                type: uiNode.Type,
                value: uiNode.Value,
                parent: null,
                keys: new List<ObjectGraph>(),
                values: new List<ObjectGraph>(),
                lengthType: uiNode.LengthType,
                arrayIndex: uiNode.ArrayIndex,
                fieldInfo: uiNode.FieldInfo,
                attrs: uiNode.Attrs
            );
            return root;
        }

        public static ObjectGraph Unwrap(UiObjectGraph uiNode)
        {
            ObjectGraph node = UiObjectGraph.ShallowUnwrap(uiNode);

            foreach(UiObjectGraph uiKeyNode in uiNode.Keys) {
                ObjectGraph child = UiObjectGraph.Unwrap(uiKeyNode);
                child.Parent = node;
                node.Keys.Add(child);
            }

            foreach(UiObjectGraph uiValueNode in uiNode.Values) {
                ObjectGraph child = UiObjectGraph.Unwrap(uiValueNode);
                child.Parent = node;
                node.Values.Add(child);
            }
            return node;
        }


        // These are value converter implementations to bypass Avalonia's constraints.
        public string DisplayType
        {
            get
            {
                string str;
                if(this.Type != null) {
                    str = this.Type.Name;
                    if (this.ArrayIndex != ObjectGraph.NULL_ARRAY_INDEX) {
                        str += " " + this.ArrayIndex.ToString();
                    }
                } else { str = ""; }
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
                string str = (this.FieldInfo != null) ? this.FieldInfo.Name : "";
                if (this.Type != null) {
                    if (BaseSerializer.IsList(this.Type)
                        || BaseSerializer.IsDictionary(this.Type)) {
                        str += String.Format("[{0}]", this.Values.Count);
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
                if(this.Type == typeof(string)) {
                    if(this.Value == null) {
                        return "";
                    } else {
                        return this.Value.ToString();
                    }
                }
                if (this.Type == null || !this.Type.IsPrimitive) {
                    return "";
                } else {
                    return this.Value.ToString();
                }
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
                List<UiObjectGraph> ancestors = this.Ancestors;
                ancestors.Reverse();
                for (int idx = 0; idx < ancestors.Count; idx++) {
                    UiObjectGraph node = ancestors[idx];
                    if (idx < ancestors.Count - 1) {
                        displayString += node.DisplayType + ": " + node.DisplayFieldName + "   >   ";
                    } else {
                        displayString += node.DisplayType + ": " + node.DisplayFieldName;
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
}
