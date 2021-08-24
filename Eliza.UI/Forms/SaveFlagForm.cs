using Eliza.Model;
using Eliza.UI.Widgets;
using Eto.Forms;

namespace Eliza.UI.Forms
{
    internal class SaveFlagForm : Form
    {
        public SaveFlagForm(SaveFlagStorage saveFlag)
        {
            this.Title = "Save Flag";

            this.Content = new SaveFlagStorageGroup(saveFlag, "Save Flag");
        }

    }
}
