using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using Eliza.Model;
using Eliza.Model.Item;
using Eliza.Model.Save;
using Eliza.Core.Serialization;
using Eliza.Avalonia.ViewModels;
using System.Collections.Generic;
using System;

using Eliza.Avalonia.Views.Editors;
using Avalonia.Data.Converters;
using System.Globalization;
using System.Reflection;
using Avalonia.Platform;

namespace Eliza.Avalonia.Views
{
    public partial class MainWindow : Window
    {
        public static MainWindow? Instance { get; private set; }
        protected MainWindowViewModel? ViewModel => DataContext as MainWindowViewModel;

        public TreeView TreeView;

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

        protected StringEditorView StringEditorView = new();
        protected UnsafeStringEditorView UnsafeStringEditorView = new();
        protected UuidStringEditorView UuidStringEditorView = new();
        protected ItemDataEditorView ItemDataEditorView = new(new ItemDataEditorViewModel());

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

            this.ObjectEditorMap.Add(typeof(string), this.StringEditorView);
            this.ObjectEditorMap.Add(typeof(ItemData), this.ItemDataEditorView);
            this.ObjectEditorMap.Add(typeof(EquipItemData), this.ItemDataEditorView);
            this.ObjectEditorMap.Add(typeof(SeedItemData), this.ItemDataEditorView);
            this.ObjectEditorMap.Add(typeof(PotToolItemData), this.ItemDataEditorView);
            this.ObjectEditorMap.Add(typeof(RuneAbilityItemData), this.ItemDataEditorView);
            this.ObjectEditorMap.Add(typeof(AmountItemData), this.ItemDataEditorView);
            this.ObjectEditorMap.Add(typeof(FoodItemData), this.ItemDataEditorView);

            this.TreeView = this.Find<TreeView>("TreeView_MainTree");
            #pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
            this.TreeView.AddHandler(PointerPressedEvent, OnTreeViewClickHandler, handledEventsToo: true);
            #pragma warning restore CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).


#if DEBUG
            this.AttachDevTools();
            #endif
        }

        // Rather than work with arcane rules of what could be bound to what,
        // We capture the click event at the window level and dispatch the event
        // to whatever we see fit.
        public void OnTreeViewClickHandler(object sender, RoutedEventArgs e)
        {
            var source = e.Source as Control;
            if(source != null && source.DataContext != null && this.ViewModel != null) {
                object dataContext = source.DataContext;
                Type contextType = dataContext.GetType();
                if (contextType == typeof(UiObjectGraph)) {
                    // Reset previously-used editor context to blank.
                    UserControl? previousEditor = this.ViewModel.SUBVIEW_EditorPane;
                    if(previousEditor != null) {
                        previousEditor.DataContext = null;
                    }
                    // Configure new editor.
                    UiObjectGraph node = (UiObjectGraph)dataContext; // Context
                    UserControl editorView = this.GetEditorView(node);
                    if(editorView.GetType() != typeof(ItemDataEditorView)) {
                        editorView.DataContext = node;
                    } else {
                        ItemDataEditorView itemEditorView = (ItemDataEditorView)editorView;
                        itemEditorView.LoadContext(
                            new ItemDataEditorViewModel(itemEditorView, ref node,
                                    this.ViewModel.RequestedLocale,
                                    this.ViewModel.RequestedVersion)
                        );
                    }
                    
                    this.ViewModel.SUBVIEW_EditorPane = editorView;
                    this.ViewModel.STRING_EditorContext = node.DisplayAncestry;
                }
            }
            // if(this.ViewModel != null) { this.ViewModel.LogWrite("Treeview clicked."); }
            e.Handled = true;
        }

        protected UserControl GetEditorView(UiObjectGraph node)
        {
            if(node.Type != null) {
                Type nodeType = node.Type;
                if (nodeType.IsPrimitive) {
                    TypeCode typeCode = Type.GetTypeCode(nodeType);
                    if (this.PrimitiveEditorMap.ContainsKey(typeCode)) {
                        return this.PrimitiveEditorMap[typeCode];
                    }
                } else if (nodeType == typeof(string)) {
                    if(node.MaxLength != ObjectGraph.NULL_MAX_LENGTH) {
                        return this.StringEditorView;
                    } else if(node.IsUtf16UuidString) {
                        return this.UuidStringEditorView;
                    } else {
                        return this.UnsafeStringEditorView;
                    }
                } else {
                    if (this.ObjectEditorMap.ContainsKey(nodeType)) {
                        return this.ObjectEditorMap[nodeType];
                    }
                }
            }
            return this.DummyEditorView;
        }





        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }

}
