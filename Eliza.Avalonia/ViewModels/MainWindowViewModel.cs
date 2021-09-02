using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using ReactiveUI;
using Eliza.Avalonia.Views;
using Avalonia.Controls;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.ObjectModel;
using System.Reflection;
using Eliza.Model;
using Eliza.Core.Serialization;
using Avalonia.Interactivity;
using Avalonia.Input;

namespace Eliza.Avalonia.ViewModels
{

    // Any reactive prooperties have to be defined within a class
    // which inherits from ReactiveObject, i.e. ViewModelBase.

    // On the other hand, event handlers can only be defined
    // inside views.

    public class MainWindowViewModel : ViewModelBase
    {

        protected MainWindow? MainWindowView; // View of viewmodel

        public SaveData.LOCALE RequestedLocale;
        public int RequestedVersion;

        protected SaveData? _SaveData;
        protected UserControl? _EditorPane;

        public UserControl? SUBVIEW_EditorPane
        {
            get => this._EditorPane;
            set => this.RaiseAndSetIfChanged(ref this._EditorPane, value);
        }

        protected string _EditorContext = "";
        public string STRING_EditorContext
        {
            get => this._EditorContext;
            set => this.RaiseAndSetIfChanged(ref this._EditorContext, value);
        }

        protected ObservableCollection<UiObjectGraph>? _ObsvObjectGraph; // ObjectGraph generated from savedata
        public ObservableCollection<UiObjectGraph>? OBSV_Graph
        {
            get => this._ObsvObjectGraph;
            set => this.RaiseAndSetIfChanged(ref this._ObsvObjectGraph, value);
        }

        protected string _Log = ""; // Just use a backing string. We're not doing anything crazy.
        public string OBSV_Log
        {
            get => this._Log;
            set => this.RaiseAndSetIfChanged(ref this._Log, value);
        }


        // Commands. See https://www.reactiveui.net/docs/handbook/commands/
        // ReactiveComand<TInput, TOutput> cmd_name
        public ReactiveCommand<Unit, Unit> CMD_Open_JP_100 { get; }
        public ReactiveCommand<Unit, Unit> CMD_Open_JP_101 { get; }
        public ReactiveCommand<Unit, Unit> CMD_Open_JP_102 { get; }
        public ReactiveCommand<Unit, Unit> CMD_Open_JP_103 { get; }
        public ReactiveCommand<Unit, Unit> CMD_Open_JP_104 { get; }
        public ReactiveCommand<Unit, Unit> CMD_Open_JP_105 { get; }
        public ReactiveCommand<Unit, Unit> CMD_Open_JP_106 { get; }
        public ReactiveCommand<Unit, Unit> CMD_Open_JP_107 { get; }
        public ReactiveCommand<Unit, Unit> CMD_Open_JP_108 { get; }
        public ReactiveCommand<Unit, Unit> CMD_Open_JP_109 { get; }
        public ReactiveCommand<Unit, Unit> CMD_Open_JP_110 { get; }
        public ReactiveCommand<Unit, Unit> CMD_SaveEncrypted { get; }
        public ReactiveCommand<Unit, Unit> CMD_SaveDecrypted { get; }

        // Event handlers have to be defined within the views themselves.


        public MainWindowViewModel() : base()
        {
            this.MainWindowView = MainWindow.Instance;

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

            this.MainWindowView.FindControl<MenuItem>("MenuItem_Open_JP_1.0.7-1.0.9").Command = this.CMD_Open_JP_107;
            this.MainWindowView.FindControl<MenuItem>("MenuItem_Open_JP_1.0.4-1.0.6").Command = this.CMD_Open_JP_104;
            this.MainWindowView.FindControl<MenuItem>("MenuItem_Open_JP_1.0.2-1.0.3").Command = this.CMD_Open_JP_102;

            this.MainWindowView.FindControl<MenuItem>("MenuItem_Open_JP_1.0.10").Command = this.CMD_Open_JP_110;
            this.MainWindowView.FindControl<MenuItem>("MenuItem_Open_JP_1.0.9").Command = this.CMD_Open_JP_109;
            this.MainWindowView.FindControl<MenuItem>("MenuItem_Open_JP_1.0.8").Command = this.CMD_Open_JP_108;
            this.MainWindowView.FindControl<MenuItem>("MenuItem_Open_JP_1.0.7").Command = this.CMD_Open_JP_107;
            this.MainWindowView.FindControl<MenuItem>("MenuItem_Open_JP_1.0.6").Command = this.CMD_Open_JP_106;
            this.MainWindowView.FindControl<MenuItem>("MenuItem_Open_JP_1.0.5").Command = this.CMD_Open_JP_105;
            this.MainWindowView.FindControl<MenuItem>("MenuItem_Open_JP_1.0.4").Command = this.CMD_Open_JP_104;
            this.MainWindowView.FindControl<MenuItem>("MenuItem_Open_JP_1.0.3").Command = this.CMD_Open_JP_103;
            this.MainWindowView.FindControl<MenuItem>("MenuItem_Open_JP_1.0.2").Command = this.CMD_Open_JP_102;
            this.MainWindowView.FindControl<MenuItem>("MenuItem_Open_JP_1.0.1").Command = this.CMD_Open_JP_101;
            this.MainWindowView.FindControl<MenuItem>("MenuItem_Open_JP_1.0.0").Command = this.CMD_Open_JP_100;

            this.MainWindowView.FindControl<MenuItem>("MenuItem_Save").Command = this.CMD_SaveEncrypted;
            this.MainWindowView.FindControl<MenuItem>("MenuItem_SaveDecrypted").Command = this.CMD_SaveDecrypted;

            // No idea how to bind these from code. Currently bound via XAML

            // mainTree.Bind(mainTree.Items, OBSV_Graph);
            // TextBox console = this.MainWindowView.Find<TextBox>("TextBox_Log").Text = this.OBSV_Log;
            // console.Bind(console.Text, this.OBSV_Log);

            this.GenerateObjectGraph();
            this.LogWrite("Ready.");
        }

        protected void GenerateObjectGraph()
        {
            if(this._SaveData != null) {
                SaveData save = this._SaveData;
                GraphSerializer serializer = new(save.Locale, save.Version);
                ObjectGraph baseNode = serializer.WriteRF5Save(save);
                // this.baseNode = serializer.WriteRF5Save(save);
                UiObjectGraph uiBaseNode = UiObjectGraph.Wrap(baseNode);
                this.OBSV_Graph = new ObservableCollection<UiObjectGraph>(uiBaseNode.Children);
            }
        }


        protected async Task Cmd_OpenAsync(SaveData.LOCALE locale, int version)
        {
            this.RequestedLocale = locale;
            this.RequestedVersion = version;

            OpenFileDialog dialog = new() {
                Title = String.Format("Open File ({0} ver {1})", locale, version),
                AllowMultiple = false,
                // Filters = new List<FileDialogFilter> { }, // Nothing to filter
            };
            string[] inputPaths = await dialog.ShowAsync(this.MainWindowView);
            this._SaveData = SaveData.FromEncryptedFile(path: inputPaths[0],
                                                    version: this.RequestedVersion,
                                                    locale: this.RequestedLocale);
            this.GenerateObjectGraph();

            this.LogWrite(String.Format("Loaded file from {0}.", inputPaths[0]));
        }

        protected async Task Cmd_SaveEncrypted()
        {
            SaveFileDialog dialog = new() {
                Title = String.Format("Open File ({0} ver {1})", this.RequestedLocale, this.RequestedVersion)
                // Filters = new List<FileDialogFilter> { }, // Nothing to filter
            };
            
            string outputPath = await dialog.ShowAsync(this.MainWindowView);

            // TODO: serialize from graph then save
            // this._SaveData.ToEncryptedFile(outputPath);
            // this.LogWrite(String.Format("File saved to {0}.", outputPath));
        }

        protected async Task Cmd_SaveDecrypted()
        {
            SaveFileDialog dialog = new() {
                Title = String.Format("Open File ({0} ver {1})", this.RequestedLocale, this.RequestedVersion)
                // Filters = new List<FileDialogFilter> { }, // Nothing to filter
            };
            
            string outputPath = await dialog.ShowAsync(this.MainWindowView);

            // TODO: serialize from graph then save
            // this._SaveData.ToDecryptedFile(outputPath);
            // this.LogWrite(String.Format("Dev: File saved to {0} without encryption.", outputPath));
        }

        public void LogWrite(string str)
        {
            this.OBSV_Log = str + "\n" + this.OBSV_Log;
        }

    }
}
