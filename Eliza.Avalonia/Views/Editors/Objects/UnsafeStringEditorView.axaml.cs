using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Eliza.Avalonia.Views.Editors
{
    public partial class UnsafeStringEditorView : BaseEditorView
    {
        public UnsafeStringEditorView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
