using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Eliza.Avalonia.Views.Editors
{
    public partial class BaseEditorView : UserControl
    {
        public BaseEditorView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
