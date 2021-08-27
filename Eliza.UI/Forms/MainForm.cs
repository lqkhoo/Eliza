using Eto.Drawing;
using Eto.Forms;
using Eliza.UI.Helpers;
using Eliza.UI.Widgets;
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

        DropDown el_dd_version = new() {
            Items = { "0", "1", "2", "3", "4", "5", "6", "7" }
        };
        TreeGridView tree = new();

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

            var el_layout_versionBox = new StackLayout() {
                Orientation = Orientation.Horizontal,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Spacing = 5,
                Padding = new Padding(10)
            };

            var el_label_versionLabel = new Label() { Text = "Version" };

            var layout = new StackLayout {
                Orientation = Orientation.Horizontal,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Spacing = 5,
                Padding = new Padding(10)
            };

            StackLayoutItem[] stackLayoutItems =
            {
                el_label_versionLabel,
                this.el_dd_version
            };

            foreach (var item in stackLayoutItems) {
                layout.Items.Add(item);
            }

            StackLayoutItem[] stackLayoutItems2 =
            {
                layout,
                this.headerButton,
                this.dataButton,
                this.footerButton,
                this.tree
            };

            layout = new StackLayout {
                Orientation = Orientation.Vertical,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Spacing = 5,
                Padding = new Padding(10)
            };

            foreach (var item in stackLayoutItems2)
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
                    this._saveData = SaveData.FromEncryptedFile(this._path, version:7, locale: SaveData.LOCALE.JP);
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
                    this._saveData.ToEncryptedFile(path);
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
