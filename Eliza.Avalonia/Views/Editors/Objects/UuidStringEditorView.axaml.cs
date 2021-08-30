using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Eliza.Avalonia.Views.Editors
{
    public partial class UuidStringEditorView : BaseEditorView
    {
        public UuidStringEditorView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
