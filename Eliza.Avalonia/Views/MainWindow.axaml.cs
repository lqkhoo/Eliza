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
using ReactiveUI;
using System.Reactive;
using System.Threading.Tasks;

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
        protected ItemDataEditorView ItemDataEditorView;

        protected Dictionary<TypeCode, BaseEditorView> PrimitiveEditorMap = new();
        protected Dictionary<Type, BaseEditorView> ObjectEditorMap = new();

        // Commands. See https://www.reactiveui.net/docs/handbook/commands/
        protected ReactiveCommand<Unit, Unit> CMD_Open_JP_100 { get; }
        protected ReactiveCommand<Unit, Unit> CMD_Open_JP_101 { get; }
        protected ReactiveCommand<Unit, Unit> CMD_Open_JP_102 { get; }
        protected ReactiveCommand<Unit, Unit> CMD_Open_JP_103 { get; }
        protected ReactiveCommand<Unit, Unit> CMD_Open_JP_104 { get; }
        protected ReactiveCommand<Unit, Unit> CMD_Open_JP_105 { get; }
        protected ReactiveCommand<Unit, Unit> CMD_Open_JP_106 { get; }
        protected ReactiveCommand<Unit, Unit> CMD_Open_JP_107 { get; }
        protected ReactiveCommand<Unit, Unit> CMD_Open_JP_108 { get; }
        protected ReactiveCommand<Unit, Unit> CMD_Open_JP_109 { get; }
        protected ReactiveCommand<Unit, Unit> CMD_Open_JP_110 { get; }
        protected ReactiveCommand<Unit, Unit> CMD_SaveEncrypted { get; }
        protected ReactiveCommand<Unit, Unit> CMD_SaveDecrypted { get; }


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

            this.ItemDataEditorView = new(this, new ItemDataEditorViewModel());
            this.ObjectEditorMap.Add(typeof(ItemData), this.ItemDataEditorView);
            this.ObjectEditorMap.Add(typeof(EquipItemData), this.ItemDataEditorView);
            this.ObjectEditorMap.Add(typeof(SeedItemData), this.ItemDataEditorView);
            this.ObjectEditorMap.Add(typeof(PotToolItemData), this.ItemDataEditorView);
            this.ObjectEditorMap.Add(typeof(RuneAbilityItemData), this.ItemDataEditorView);
            this.ObjectEditorMap.Add(typeof(AmountItemData), this.ItemDataEditorView);
            this.ObjectEditorMap.Add(typeof(FoodItemData), this.ItemDataEditorView);

            this.TreeView = this.Find<TreeView>("TreeView_MainTree");
            this.TreeView.AddHandler(PointerPressedEvent, OnTreeViewClickHandler, handledEventsToo: true);

            this.CMD_Open_JP_100 = ReactiveCommand.Create(() => { Task task = this.Cmd_OpenAsync(SaveData.LOCALE.JP, 0); });
            this.CMD_Open_JP_101 = ReactiveCommand.Create(() => { Task task = this.Cmd_OpenAsync(SaveData.LOCALE.JP, 1); });
            this.CMD_Open_JP_102 = ReactiveCommand.Create(() => { Task task = this.Cmd_OpenAsync(SaveData.LOCALE.JP, 2); });
            this.CMD_Open_JP_103 = ReactiveCommand.Create(() => { Task task = this.Cmd_OpenAsync(SaveData.LOCALE.JP, 3); });
            this.CMD_Open_JP_104 = ReactiveCommand.Create(() => { Task task = this.Cmd_OpenAsync(SaveData.LOCALE.JP, 4); });
            this.CMD_Open_JP_105 = ReactiveCommand.Create(() => { Task task = this.Cmd_OpenAsync(SaveData.LOCALE.JP, 5); });
            this.CMD_Open_JP_106 = ReactiveCommand.Create(() => { Task task = this.Cmd_OpenAsync(SaveData.LOCALE.JP, 6); });
            this.CMD_Open_JP_107 = ReactiveCommand.Create(() => { Task task = this.Cmd_OpenAsync(SaveData.LOCALE.JP, 7); });
            this.CMD_Open_JP_108 = ReactiveCommand.Create(() => { Task task = this.Cmd_OpenAsync(SaveData.LOCALE.JP, 8); });
            this.CMD_Open_JP_109 = ReactiveCommand.Create(() => { Task task = this.Cmd_OpenAsync(SaveData.LOCALE.JP, 9); });
            this.CMD_Open_JP_110 = ReactiveCommand.Create(() => { Task task = this.Cmd_OpenAsync(SaveData.LOCALE.JP, 10); });
            this.CMD_SaveEncrypted = ReactiveCommand.Create(() => { Task task = this.Cmd_SaveEncrypted(); });
            this.CMD_SaveDecrypted = ReactiveCommand.Create(() => { Task task = this.Cmd_SaveDecrypted(); });

            this.FindControl<MenuItem>("MenuItem_Open_JP_1.0.7-1.0.9").Command = this.CMD_Open_JP_107;
            this.FindControl<MenuItem>("MenuItem_Open_JP_1.0.4-1.0.6").Command = this.CMD_Open_JP_104;
            this.FindControl<MenuItem>("MenuItem_Open_JP_1.0.2-1.0.3").Command = this.CMD_Open_JP_102;

            this.FindControl<MenuItem>("MenuItem_Open_JP_1.0.10").Command = this.CMD_Open_JP_110;
            this.FindControl<MenuItem>("MenuItem_Open_JP_1.0.9").Command = this.CMD_Open_JP_109;
            this.FindControl<MenuItem>("MenuItem_Open_JP_1.0.8").Command = this.CMD_Open_JP_108;
            this.FindControl<MenuItem>("MenuItem_Open_JP_1.0.7").Command = this.CMD_Open_JP_107;
            this.FindControl<MenuItem>("MenuItem_Open_JP_1.0.6").Command = this.CMD_Open_JP_106;
            this.FindControl<MenuItem>("MenuItem_Open_JP_1.0.5").Command = this.CMD_Open_JP_105;
            this.FindControl<MenuItem>("MenuItem_Open_JP_1.0.4").Command = this.CMD_Open_JP_104;
            this.FindControl<MenuItem>("MenuItem_Open_JP_1.0.3").Command = this.CMD_Open_JP_103;
            this.FindControl<MenuItem>("MenuItem_Open_JP_1.0.2").Command = this.CMD_Open_JP_102;
            this.FindControl<MenuItem>("MenuItem_Open_JP_1.0.1").Command = this.CMD_Open_JP_101;
            this.FindControl<MenuItem>("MenuItem_Open_JP_1.0.0").Command = this.CMD_Open_JP_100;

            this.FindControl<MenuItem>("MenuItem_Save").Command = this.CMD_SaveEncrypted;
            this.FindControl<MenuItem>("MenuItem_SaveDecrypted").Command = this.CMD_SaveDecrypted;

            this.LogWrite("Ready.");


            #if DEBUG
            this.AttachDevTools();
            #endif
        }

        // Rather than work with arcane rules of what could be bound to what,
        // We capture the click event at the window level and dispatch the event
        // to whatever we see fit.
        public void OnTreeViewClickHandler(object? sender, RoutedEventArgs e)
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
                    BaseEditorView editorView = this.GetEditorView(node);
                    if(editorView.GetType() != typeof(ItemDataEditorView)) {
                        editorView.LoadContext(node);
                    } else {
                        ItemDataEditorView itemEditorView = (ItemDataEditorView)editorView;
                        itemEditorView.LoadContext(
                            new ItemDataEditorViewModel(itemEditorView, ref node,
                                    this.ViewModel.RequestedLocale,
                                    this.ViewModel.RequestedVersion
                            )
                        );
                    }
                    
                    this.ViewModel.SUBVIEW_EditorPane = editorView;
                    this.ViewModel.STRING_EditorContext = node.DisplayAncestry;
                }
            }
            // if(this.ViewModel != null) { this.ViewModel.LogWrite("Treeview clicked."); }
            e.Handled = true;
        }

        protected BaseEditorView GetEditorView(UiObjectGraph node)
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


        protected async Task Cmd_OpenAsync(SaveData.LOCALE locale, int version)
        {
            if(this.ViewModel != null) {
                OpenFileDialog dialog = new() {
                    Title = String.Format("Open File ({0} ver {1})", locale, version),
                    AllowMultiple = false,
                    // Filters = new List<FileDialogFilter> { }, // Nothing to filter
                };
                string[] inputPaths = await dialog.ShowAsync(this);

                this.ViewModel.OpenEncryptedFile(inputPaths[0], locale, version);
                this.LogWrite(String.Format("Loaded file from {0}.", inputPaths[0]));
            }
        }


        protected async Task Cmd_SaveEncrypted()
        {
            if(this.ViewModel != null) {

                SaveFileDialog dialog = new() {
                    Title = String.Format("Open File ({0} ver {1})", this.ViewModel.RequestedLocale, this.ViewModel.RequestedVersion)
                    // Filters = new List<FileDialogFilter> { }, // Nothing to filter
                };
                string outputPath = await dialog.ShowAsync(this);
                // this.ViewModel.SaveEncryptedFile(outputPath);
                // this.LogWrite(String.Format("Saved file to {0}", outputPath));
                //TODO
            }
        }

        protected async Task Cmd_SaveDecrypted()
        {
            if(this.ViewModel != null) {
                SaveFileDialog dialog = new() {
                    Title = String.Format("Open File ({0} ver {1})", this.ViewModel.RequestedLocale, this.ViewModel.RequestedVersion)
                    // Filters = new List<FileDialogFilter> { }, // Nothing to filter
                };
                string outputPath = await dialog.ShowAsync(this);
                // this.ViewModel.SaveDecryptedFile(outputPath);
                // this.LogWrite(String.Format("Saved file without encryption to {0}", outputPath));

                //TODO
            }
        }

        public void LogWrite(string str)
        {
            if(this.ViewModel != null) {
                this.ViewModel.OBSV_Log = str + "\n" + this.ViewModel.OBSV_Log;
            }
        }



        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }

}
