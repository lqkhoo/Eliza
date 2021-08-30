using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Eliza.Avalonia.Views.Editors
{
    public partial class StringEditorView : BaseEditorView
    {
        public StringEditorView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}