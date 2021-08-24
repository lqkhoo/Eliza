using Eliza.UI.Helpers;

namespace Eliza.UI.Widgets
{
    public class CheckBox : GenericWidget
    {
        private Eto.Forms.CheckBox checkBox = new();
        public CheckBox(Ref<bool> value, string text = "") : base(text)
        {
            this._value = value;
            this.checkBox.Checked = value.Value;
            this.Items.Add(this.checkBox);
            this.checkBox.CheckedChanged += this.CheckBox_CheckedChanged;

        }

        public CheckBox(string text = "") : base(text)
        {
            this.Items.Add(this.checkBox);
            this.checkBox.CheckedChanged += this.CheckBox_CheckedChanged;
        }

        public void ChangeReferenceValue(Ref<bool> value)
        {
            this._value = value;
            this.checkBox.Checked = value.Value;
        }
        private void CheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            ((Ref<bool>)this._value).Value = (bool)this.checkBox.Checked;
        }
    }
}