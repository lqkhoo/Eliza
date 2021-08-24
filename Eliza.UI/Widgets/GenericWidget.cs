using Eto.Forms;
using System;

namespace Eliza.UI.Widgets
{
    public abstract class GenericWidget : StackLayout
    {
        protected Type _valueType;
        protected object _value;
        protected Label Label = new();

        public GenericWidget(string text) : base()
        {
            this.Orientation = Orientation.Horizontal;
            this.Items.Add(this.Label);
            this.Label.Text = text;
            this.Label.Width = 200;
        }
    }
}
