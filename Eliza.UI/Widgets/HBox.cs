using Eto.Forms;

namespace Eliza.UI.Widgets
{
    class HBox : StackLayout
    {
        public HBox() : base()
        {
            this.Orientation = Orientation.Horizontal;
            this.Padding = 5;
            this.Spacing = 5;
        }
    }
}
