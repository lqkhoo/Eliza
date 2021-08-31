using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Eliza.Avalonia.ViewModels;
using System.Collections.Generic;

namespace Eliza.Avalonia.Views.Editors
{
    public partial class ItemDataEditorView : BaseEditorView
    {

        protected ItemDataEditorViewModel ViewModel;

        public ItemDataEditorView() { }

        public ItemDataEditorView(ItemDataEditorViewModel viewModel)
        {
            this.ViewModel = viewModel;
            InitializeComponent();
        }

        public void LoadContext(ItemDataEditorViewModel context)
        {
            this.ViewModel = context;
            this.DataContext = this.ViewModel;
        }

        protected void StartEditing()
        {

        }


        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
