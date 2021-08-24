using Eto.Forms;

namespace Eliza.UI.Widgets
{
    public class VBox : StackLayout
    {
        public VBox() : base()
        {
            this.Orientation = Orientation.Vertical;
            this.Padding = 5;
            this.Spacing = 5;
        }
    }
}
