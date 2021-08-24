using Eliza.UI.Helpers;
using Eto.Forms;
using System;

namespace Eliza.UI.Widgets
{

    class SpinBox : GenericWidget
    {
        public NumericStepper numericStepper = new();

        public SpinBox(Ref<byte> value, string text = "") : base(text)
        {
            this._valueType = value.Value.GetType();
            this._value = value;
            //numericStepper.MinValue = byte.MinValue;
            //numericStepper.MaxValue = byte.MaxValue;
            this.Setup();
        }

        public SpinBox(Ref<ushort> value, string text = "") : base(text)
        {
            this._valueType = value.Value.GetType();
            this._value = value;
            //numericStepper.MinValue = ushort.MinValue;
            //numericStepper.MaxValue = ushort.MaxValue;
            this.Setup();
        }

        public SpinBox(Ref<uint> value, string text = "") : base(text)
        {
            this._valueType = value.Value.GetType();
            this._value = value;
            //numericStepper.MinValue = uint.MinValue;
            //numericStepper.MaxValue = uint.MaxValue;
            this.Setup();
        }

        public SpinBox(Ref<ulong> value, string text = "") : base(text)
        {
            this._valueType = value.Value.GetType();
            this._value = value;
            //numericStepper.MinValue = ulong.MinValue;
            //numericStepper.MaxValue = ulong.MaxValue;
            this.Setup();
        }

        public SpinBox(Ref<sbyte> value, string text = "") : base(text)
        {
            this._valueType = value.Value.GetType();
            this._value = value;
            //numericStepper.MinValue = sbyte.MinValue;
            //numericStepper.MaxValue = sbyte.MaxValue;
            this.Setup();
        }

        public SpinBox(Ref<short> value, string text = "") : base(text)
        {
            this._valueType = value.Value.GetType();
            this._value = value;
            //numericStepper.MinValue = short.MinValue;
            //numericStepper.MaxValue = short.MaxValue;
            this.Setup();
        }

        public SpinBox(Ref<int> value, string text = "") : base(text)
        {
            this._valueType = value.Value.GetType();
            this._value = value;
            //numericStepper.MinValue = int.MinValue;
            //numericStepper.MaxValue = int.MaxValue;
            this.Setup();
        }

        public SpinBox(Ref<long> value, string text = "") : base(text)
        {
            this._valueType = value.Value.GetType();
            this._value = value;
            //numericStepper.MinValue = long.MinValue;
            //numericStepper.MaxValue = long.MaxValue;
            this.Setup();
        }

        public SpinBox(Ref<float> value, string text = "") : base(text)
        {
            this._valueType = value.Value.GetType();
            this._value = value;
            //numericStepper.MinValue = float.MinValue;
            //numericStepper.MaxValue = float.MaxValue;
            this.Setup();
        }

        public SpinBox(Ref<double> value, string text = "") : base(text)
        {
            this._valueType = value.Value.GetType();
            this._value = value;
            //numericStepper.MinValue = double.MinValue;
            //numericStepper.MaxValue = double.MaxValue;
            this.Setup();
        }

        public SpinBox(string text = "") : base(text)
        {
            this.Items.Add(this.numericStepper);
            this.numericStepper.ValueChanged += this.OnValueChanged;
            this.numericStepper.Width = 100;
        }

        private void Setup()
        {
            this.Items.Add(this.numericStepper);
            this.numericStepper.ValueChanged += this.OnValueChanged;
            this.numericStepper.Width = 100;
            this.SetValue();
        }

        private void SetValue()
        {
            switch (Type.GetTypeCode(this._valueType))
            {
                case TypeCode.Byte: this.numericStepper.Value = Convert.ToDouble(((Ref<byte>)this._value).Value); break;
                case TypeCode.UInt16: this.numericStepper.Value = Convert.ToDouble(((Ref<ushort>)this._value).Value); break;
                case TypeCode.UInt32: this.numericStepper.Value = Convert.ToDouble(((Ref<uint>)this._value).Value); break;
                case TypeCode.UInt64: this.numericStepper.Value = Convert.ToDouble(((Ref<ulong>)this._value).Value); break;
                case TypeCode.SByte: this.numericStepper.Value = Convert.ToDouble(((Ref<sbyte>)this._value).Value); break;
                case TypeCode.Int16: this.numericStepper.Value = Convert.ToDouble(((Ref<short>)this._value).Value); break;
                case TypeCode.Int32: this.numericStepper.Value = Convert.ToDouble(((Ref<int>)this._value).Value); break;
                case TypeCode.Int64: this.numericStepper.Value = Convert.ToDouble(((Ref<long>)this._value).Value); break;
                case TypeCode.Single: this.numericStepper.Value = Convert.ToDouble(((Ref<float>)this._value).Value); break;
                case TypeCode.Double: this.numericStepper.Value = Convert.ToDouble(((Ref<double>)this._value).Value); break;
            }
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            switch (Type.GetTypeCode(this._valueType))
            {
                case TypeCode.Byte: ((Ref<byte>)this._value).Value = (byte)this.numericStepper.Value; break;
                case TypeCode.UInt16: ((Ref<ushort>)this._value).Value = (ushort)this.numericStepper.Value; break;
                case TypeCode.UInt32: ((Ref<uint>)this._value).Value = (uint)this.numericStepper.Value; break;
                case TypeCode.UInt64: ((Ref<ulong>)this._value).Value = (ulong)this.numericStepper.Value; break;
                case TypeCode.SByte: ((Ref<sbyte>)this._value).Value = (sbyte)this.numericStepper.Value; break;
                case TypeCode.Int16: ((Ref<short>)this._value).Value = (short)this.numericStepper.Value; break;
                case TypeCode.Int32: ((Ref<int>)this._value).Value = (int)this.numericStepper.Value; break;
                case TypeCode.Int64: ((Ref<long>)this._value).Value = (long)this.numericStepper.Value; break;
                case TypeCode.Single: ((Ref<float>)this._value).Value = (float)this.numericStepper.Value; break;
                case TypeCode.Double: ((Ref<double>)this._value).Value = (double)this.numericStepper.Value; break;
            }
        }

        public void ChangeReferenceValue(Ref<byte> value)
        {
            this._valueType = value.Value.GetType();
            this._value = value;
            this.SetValue();
        }

        public void ChangeReferenceValue(Ref<ushort> value)
        {
            this._valueType = value.Value.GetType();
            this._value = value;
            this.SetValue();
        }

        public void ChangeReferenceValue(Ref<uint> value)
        {
            this._valueType = value.Value.GetType();
            this._value = value;
            this.SetValue();
        }

        public void ChangeReferenceValue(Ref<ulong> value)
        {
            this._valueType = value.Value.GetType();
            this._value = value;
            this.SetValue();
        }

        public void ChangeReferenceValue(Ref<sbyte> value)
        {
            this._valueType = value.Value.GetType();
            this._value = value;
            this.SetValue();
        }

        public void ChangeReferenceValue(Ref<short> value)
        {
            this._valueType = value.Value.GetType();
            this._value = value;
            this.SetValue();
        }

        public void ChangeReferenceValue(Ref<int> value)
        {
            this._valueType = value.Value.GetType();
            this._value = value;
            this.SetValue();
        }

        public void ChangeReferenceValue(Ref<long> value)
        {
            this._valueType = value.Value.GetType();
            this._value = value;
            this.SetValue();
        }

        public void ChangeReferenceValue(Ref<float> value)
        {
            this._valueType = value.Value.GetType();
            this._value = value;
            this.SetValue();
        }

        public void ChangeReferenceValue(Ref<double> value)
        {
            this._valueType = value.Value.GetType();
            this._value = value;
            this.SetValue();
        }
    }
}