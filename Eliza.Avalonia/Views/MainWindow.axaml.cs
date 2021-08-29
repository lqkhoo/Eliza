using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Eliza.Model;
using Eliza.Model.Save;
using Eliza.Core.Serialization;
using Eliza.Avalonia.ViewModels;
using Avalonia.Input;
using Avalonia.Interactivity;
using System.Collections.Generic;
using System;

using Eliza.Avalonia.Views.Editors;

namespace Eliza.Avalonia.Views
{
    public partial class MainWindow : Window
    {
        public static MainWindow? Instance { get; private set; }
        protected MainWindowViewModel ViewModel => DataContext as MainWindowViewModel;

        // Sub-views
        protected ContextView ContextView = new();
        // Editors
        protected DummyEditorView DummyEditorView = new();
        protected BooleanEditorView BooleanEditorView = new();
        protected ByteEditorView ByteEditorView = new();
        protected CharEditorView CharEditorView = new();
        protected UInt16EditorView UInt16EditorView = new();
        protected UInt32EditorView UInt32EditorView = new();
        protected UInt64EditorView UInt64EditorView = new();
        protected Int16EditorView Int16EditorView = new();
        protected Int32EditorView Int32EditorView = new();
        protected Int64EditorView Int64EditorView = new();
        protected SingleEditorView SingleEditorView = new();
        protected DoubleEditorView DoubleEditorView = new();

        protected Dictionary<TypeCode, UserControl> PrimitiveEditorMap = new();
        protected Dictionary<Type, UserControl> ObjectEditorMap = new();

        public MainWindow()
        {
            Instance = this;
            this.InitializeComponent();

            this.PrimitiveEditorMap.Add(TypeCode.Boolean, this.BooleanEditorView);
            this.PrimitiveEditorMap.Add(TypeCode.Byte, this.ByteEditorView);
            this.PrimitiveEditorMap.Add(TypeCode.Char, this.CharEditorView);
            this.PrimitiveEditorMap.Add(TypeCode.UInt16, this.UInt16EditorView);
            this.PrimitiveEditorMap.Add(TypeCode.UInt32, this.UInt32EditorView);
            this.PrimitiveEditorMap.Add(TypeCode.UInt64, this.UInt64EditorView);
            this.PrimitiveEditorMap.Add(TypeCode.Int16, this.Int16EditorView);
            this.PrimitiveEditorMap.Add(TypeCode.Int32, this.Int32EditorView);
            this.PrimitiveEditorMap.Add(TypeCode.Int64, this.Int64EditorView);
            this.PrimitiveEditorMap.Add(TypeCode.Single, this.SingleEditorView);
            this.PrimitiveEditorMap.Add(TypeCode.Double, this.DoubleEditorView);
            //TODO: object editor views


            // This will catch all click events and we'll capture the context
            // from the event itself.
            this.AddHandler(PointerPressedEvent, OnClickHandler, handledEventsToo: true);

            #if DEBUG
            this.AttachDevTools();
            #endif
        }

        // Rather than work with arcane rules of what could be bound to what,
        // We capture the click event at the window level and dispatch the event
        // to whatever we see fit.
        public void OnClickHandler(object sender, RoutedEventArgs e)
        {
            var source = e.Source as Control;

            // Maybe check if it's a button

            if(source != null && source.DataContext != null) {
                Type contextType = source.DataContext.GetType();
                if (contextType == typeof(UiObjectGraph)) {
                    UiObjectGraph node = source.DataContext as UiObjectGraph; // Context

                    // UserControl contextView = this.ViewModel.SUBVIEW_ContextPane;
                    // contextView.DataContext = source.DataContext;
                    UserControl editorView = this.GetEditorView(node);
                    
                    editorView.DataContext = source.DataContext;
                    this.ViewModel.SUBVIEW_EditorPane = editorView;
                    this.ViewModel.STRING_EditorContext = node.DisplayAncestry;
                }

                switch (source.Name) {

                }
            }

            this.ViewModel.LogWrite("Clicked");
            e.Handled = true;
        }

        protected UserControl GetEditorView(UiObjectGraph node)
        {
            Type nodeType = node.Type;
            if (nodeType.IsPrimitive) {
                TypeCode typeCode = Type.GetTypeCode(nodeType);
                if (this.PrimitiveEditorMap.ContainsKey(typeCode)) {
                    return this.PrimitiveEditorMap[typeCode];
                }
            } else {
                if (this.ObjectEditorMap.ContainsKey(nodeType)) {
                    return this.ObjectEditorMap[nodeType];
                }
            }
            return this.DummyEditorView;
        }



        /*
        public void OnPointerEnter(object sender, RoutedEventArgs e)
        {
            var source = e.Source as Control;
            switch (source.Name) {
            }
            this.ViewModel.LogWrite("Pointer enter.");
        }
        */




        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
