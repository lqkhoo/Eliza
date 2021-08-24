using Eto.Drawing;
using Eto.Forms;
using System;
using Eliza.UI.Forms;
using Eliza.Model;

namespace Eliza.Forms
{
    public partial class MainForm : Form
    {
        private SaveData _saveData;
        private string _path;

        Button headerButton = new() { Text = "Header" };
        Button dataButton = new() { Text = "Data" };
        Button footerButton = new() { Text = "Footer" };

        public MainForm()
        {
            this.Title = "Eliza";
            this.MinimumSize = new Size(200, 200);

            this.headerButton.Enabled = false;
            this.headerButton.Click += this.HeaderButton_Click;

            this.dataButton.Enabled = false;
            this.dataButton.Click += this.DataButton_Click;

            this.footerButton.Enabled = false;
            this.footerButton.Click += this.FooterButton_Click;

            var layout = new StackLayout { Orientation = Orientation.Vertical, HorizontalContentAlignment = HorizontalAlignment.Center, Spacing = 5, Padding = new Padding(10) };

            StackLayoutItem[] stackLayoutItems =
            {
                this.headerButton,
                this.dataButton,
                this.footerButton
            };

            foreach (var item in stackLayoutItems)
            {
                layout.Items.Add(item);
            }

            //Need this, so it doesn't an error of no instance
            this.Content = layout;
            
            var openMenuButton = new Command { MenuText = "Open", Shortcut = Keys.Control | Keys.O };
            openMenuButton.Executed += (sender, e) => this.OpenMenuButton_Executed(sender, e);

            var saveMenuButton = new Command { MenuText = "Save", Shortcut = Keys.Control | Keys.S };
            saveMenuButton.Executed += this.SaveMenuButton_Executed;

            this.Menu = new MenuBar
            {
                Items =
                {
					// File submenu
					new SubMenuItem 
                    { 
                        Text = "&File", Items = 
                        { 
                            openMenuButton,
                            saveMenuButton
                        } 
                    },
				},
            };
        }

        void OpenMenuButton_Executed(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filters.Add(
                    new FileFilter("All Files", ".*")
                );

                if (openFileDialog.ShowDialog(this.Parent) == DialogResult.Ok)
                {
                    this._path = openFileDialog.FileName;

                    // try
                    // {
                    this._saveData = SaveData.FromEncryptedFile(this._path);
                    this.headerButton.Enabled = true;
                    this.dataButton.Enabled = true;
                    this.footerButton.Enabled = true;
                    // }
                    // catch
                    // {
                    //     MessageBox.Show("Error: Incompatible/invalid save file found. Please report the issue on Github.");
                    // }
                }
            }
        }

        void SaveMenuButton_Executed(object sender, EventArgs e)
        {
            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filters.Add(
                    new FileFilter("All Files", ".*")
                );

                if (saveFileDialog.ShowDialog(this.Parent) == DialogResult.Ok)
                {
                    var path = saveFileDialog.FileName;
                    this._saveData.Write(path);
                }
            }
        }

        void HeaderButton_Click(object sender, EventArgs e)
        {
            var headerForm = new HeaderForm(this._saveData);
            headerForm.Show();
        }

        void DataButton_Click(object sender, EventArgs e)
        {
            var dataForm = new DataForm(this._saveData);
            dataForm.Show();
        }

        void FooterButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

    }
}
