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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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

        /*
        // For storing the original contents of this node.
        // Used to undo from item editing.
        // Cache(), RestoreFromCache(), and ClearChildren() governs behavior of this field.
        // Note that the cache is a shallow copy, so if we want to overwrite this
        // node, make sure we clear the children and populate it with new UiObjectGraph
        // instances, otherwise operations will modify items inside this cache.
        protected UiObjectGraph _ShallowCache;
        */

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

        protected UiObjectGraph? _Parent = null;
        public UiObjectGraph? Parent
        {
            get => this._Parent;
            set => this.RaiseAndSetIfChanged(ref this._Parent, value);
        }

        protected List<UiObjectGraph> _Children = new List<UiObjectGraph>();
        public List<UiObjectGraph> Children
        {
            get => this._Children;
            set => this.RaiseAndSetIfChanged(ref this._Children, value);
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

        protected bool _IsUtf16UuidString;
        public bool IsUtf16UuidString
        {
            get => this._IsUtf16UuidString;
            set => this.RaiseAndSetIfChanged(ref this._IsUtf16UuidString, value);
        }

        protected int _MaxLength;
        public int MaxLength
        {
            get => this._MaxLength;
            set => this.RaiseAndSetIfChanged(ref this._MaxLength, value);
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

        // Used for hex input maxlength
        public double NumNibbles
        {
            get
            {
                if (this.Type != null && this.Type.IsPrimitive) {
                    TypeCode typeCode = Type.GetTypeCode(this.Type);
                    switch (typeCode) {
                        // case TypeCode.Boolean: return 1; Unused. Ill-defined anyway
                        case TypeCode.Byte: return 2;
                        // case TypeCode.Char: return char.MinValue;
                        case TypeCode.Char: return 2;
                        case TypeCode.UInt16: return 4;
                        case TypeCode.UInt32: return 8;
                        case TypeCode.UInt64: return 16;
                        case TypeCode.SByte: return 2;
                        case TypeCode.Int16: return 4;
                        case TypeCode.Int32: return 8;
                        case TypeCode.Int64: return 16;
                        case TypeCode.Single: return 8;
                        case TypeCode.Double: return 16;
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
                UiObjectGraph? parentNode = currentNode.Parent;
                ancestors.Add(this);
                while(true) {
                    if(parentNode != null) {
                        currentNode = parentNode;
                        ancestors.Add(currentNode);
                        parentNode = currentNode.Parent;
                    } else {
                        break;
                    }
                }
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
            this._IsUtf16UuidString = node.isUtf16UuidString;
            this._FieldInfo = node.FieldInfo;
            this._Attrs = node.Attrs;

            foreach (ObjectGraph valueNode in node.Children) {
                UiObjectGraph childValueNode = new(valueNode);
                childValueNode.Parent = this;
                this.Children.Add(childValueNode);
            }

        }

        protected UiObjectGraph()
        {

        }

        /*
        public void Cache()
        {
            // Cache-related function. Shallow copy.
            UiObjectGraph cacheNode = new();
            cacheNode.This = cacheNode;
            cacheNode.Type = this.Type;
            cacheNode.Value = this.Value;
            cacheNode.LengthType = this.LengthType;
            cacheNode.IsUtf16UuidString = this.IsUtf16UuidString;
            cacheNode.FieldInfo = this.FieldInfo;
            cacheNode.Attrs = this.Attrs;
            foreach(UiObjectGraph child in this.Children) {
                cacheNode.Children.Add(child);
            }
        }

        public void RestoreFromCache()
        {
            // Cache-related function. Shallow copy back.
            this.Type = this._ShallowCache.Type;
            this.Value = this._ShallowCache.Value;
            this.LengthType = this._ShallowCache.LengthType;
            this.IsUtf16UuidString = this._ShallowCache.IsUtf16UuidString;
            this.FieldInfo = this._ShallowCache.FieldInfo;
            this.Attrs = this._ShallowCache.Attrs;
            foreach(UiObjectGraph child in this._ShallowCache.Children) {
                this.Children.Add(child);
            }
        }

        public void ClearChildren()
        {
            // Cache - related function. Clear children references so we keep
            // the cache clean from modification.
            this.Children = new List<UiObjectGraph>();
        }
        */


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
                children: new List<ObjectGraph>(),
                lengthType: uiNode.LengthType,
                arrayIndex: uiNode.ArrayIndex,
                isUtf16UuidString: uiNode.IsUtf16UuidString,
                maxLength: uiNode.MaxLength,
                fieldInfo: uiNode.FieldInfo,
                attrs: uiNode.Attrs
            );
            return root;
        }

        public static ObjectGraph Unwrap(UiObjectGraph uiNode)
        {
            ObjectGraph node = UiObjectGraph.ShallowUnwrap(uiNode);

            foreach(UiObjectGraph uiValueNode in uiNode.Children) {
                ObjectGraph child = UiObjectGraph.Unwrap(uiValueNode);
                child.Parent = node;
                node.Children.Add(child);
            }
            return node;
        }











        // These are one-way properties that bind values that won't change.
        // Basically lazy-mode value converters so we don't pollute that namespace.
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
                        str += String.Format("[{0}]", this.Children.Count);
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
                if(this.Value == null) {
                    return "null";
                } else  {
                    if(this.Type != null && this.Type.IsPrimitive) {
                        #pragma warning disable CS8603 // Possible null reference return.
                        return this.Value.ToString();
                        #pragma warning restore CS8603 // Possible null reference return.
                    } else {
                        return "";
                    }
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

        public override string ToString()
        {
            string typeName = (this.Type == null) ? "" : this.Type.Name;
            return String.Format("Graph: [{1}] {2}: {3}", this.Children.Count, this.DisplayFieldName, typeName);
        }
    }
}
