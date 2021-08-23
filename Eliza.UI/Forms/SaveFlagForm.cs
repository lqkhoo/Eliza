﻿using Eliza.Model;
using Eliza.UI.Widgets;
using Eto.Forms;

namespace Eliza.UI.Forms
{
    internal class SaveFlagForm : Form
    {
        public SaveFlagForm(SaveFlagStorage saveFlag)
        {
            Title = "Save Flag";

            Content = new SaveFlagStorageGroup(saveFlag, "Save Flag");
        }

    }
}
