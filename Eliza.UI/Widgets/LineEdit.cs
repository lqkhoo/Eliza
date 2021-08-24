using Eliza.UI.Helpers;
using Eto.Forms;
using System;
using System.Text;

namespace Eliza.UI.Widgets
{
    class LineEdit : GenericWidget
    {
        public TextBox textBox = new();

        public LineEdit(Ref<char[]> value, string text = "") : base(text)
        {
            this._valueType = value.Value.GetType();
            this._value = value;
            this.Setup();
        }

        public LineEdit(Ref<string> value, string text = "") : base(text)
        {
            this._valueType = value.Value.GetType();
            this._value = value;
            this.Setup();
        }

        public LineEdit(string text = "") : base(text)
        {
            this.Items.Add(this.textBox);
        }

        public void ChangeReferenceValue(Ref<char[]> value)
        {
            if (value != null)
            {
                if (value.Value != null)
                {
                    this._value = value;
                    this._valueType = value.Value.GetType();
                    this.SetValue();
                }
            }
        }

        public void ChangeReferenceValue(Ref<string> value)
        {
            if (value != null)
            {
                if (value.Value != null)
                {
                    this._value = value;
                    this._valueType = value.Value.GetType();
                    this.SetValue();
                }
            }
        }

        private void Setup()
        {
            this.Items.Add(this.textBox);
            this.textBox.TextChanged += this.LineEdit_TextChanged;
            this.SetValue();
        }

        private void SetValue()
        {
            if (this._valueType == typeof(char[]))
            {
                this.textBox.Text = Encoding.UTF8.GetString(
                    Encoding.UTF8.GetBytes(((Ref<char[]>)this._value).Value)
                 );
            }
            else
            {
                this.textBox.Text = ((Ref<string>)this._value).Value;
            }
        }

        private void LineEdit_TextChanged(object sender, EventArgs e)
        {
            if (this._valueType == typeof(char[]))
            {
                // Need to check on encoding, but should be fine
                ((Ref<char[]>)this._value).Value = this.textBox.Text.ToCharArray();
            }
            else
            {
                ((Ref<string>)this._value).Value = this.textBox.Text;
            }
        }
    }
}