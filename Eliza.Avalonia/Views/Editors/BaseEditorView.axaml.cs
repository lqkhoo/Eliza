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

        /// <summary>
        /// This is for when the view needs to set things up after a new DataContext is set.
        /// Think of it as a function that sets DataContext and then calls other functions,
        /// rather than just doing view.DataContext = newContext;
        /// Default behavior is to just set DataContext. Override in subclasses.
        /// </summary>
        /// <param name="dataContext">Data Context object</param>
        public virtual void LoadContext(object dataContext)
        {
            this.DataContext = dataContext;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
