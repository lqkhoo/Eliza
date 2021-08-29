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
    public class MainWindowViewModel : ViewModelBase
    {
        // protected ViewModelBase Content; // Shouldn't be necessary. We only have one view.
        protected MainWindow? MainWindowView;


        protected SaveData.LOCALE Requested_Locale;
        protected int Requested_Version;
        protected SaveData _SaveData;


        // Reactive properties and subviews for binding
        /*
        protected UserControl _ContextPane;
        public UserControl SUBVIEW_ContextPane
        {
            get => this._ContextPane;
            set => this.RaiseAndSetIfChanged(ref this._ContextPane, value);
        }
        */

        protected UserControl _EditorPane;
        public UserControl SUBVIEW_EditorPane
        {
            get => this._EditorPane;
            set => this.RaiseAndSetIfChanged(ref this._EditorPane, value);
        }

        protected string _EditorContext;
        public string STRING_EditorContext
        {
            get => this._EditorContext;
            set => this.RaiseAndSetIfChanged(ref this._EditorContext, value);
        }

        protected ObservableCollection<UiObjectGraph> _ObsvObjectGraph; // ObjectGraph generated from savedata
        public ObservableCollection<UiObjectGraph> OBSV_Graph
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
        public ReactiveCommand<Unit, Unit> CMD_Open_JP_102_103 { get; }
        public ReactiveCommand<Unit, Unit> CMD_Open_JP_104_106 { get; }
        public ReactiveCommand<Unit, Unit> CMD_Open_JP_107_109 { get; }
        public ReactiveCommand<Unit, Unit> CMD_SaveEncrypted { get; }
        public ReactiveCommand<Unit, Unit> CMD_SaveDecrypted { get; }

        // Event handlers have to be defined within the views themselves.



        public MainWindowViewModel() : base()
        {
            this.MainWindowView = MainWindow.Instance;

            this.CMD_Open_JP_102_103 = ReactiveCommand.Create(() => { Task task = this.Cmd_OpenAsync(SaveData.LOCALE.JP, 2); });
            this.CMD_Open_JP_104_106 = ReactiveCommand.Create(() => { Task task = this.Cmd_OpenAsync(SaveData.LOCALE.JP, 4); });
            this.CMD_Open_JP_107_109 = ReactiveCommand.Create(() => { Task task = this.Cmd_OpenAsync(SaveData.LOCALE.JP, 7); });
            this.CMD_SaveEncrypted = ReactiveCommand.Create(() => { Task task = this.Cmd_SaveEncrypted(); });
            this.CMD_SaveEncrypted = ReactiveCommand.Create(() => { Task task = this.Cmd_SaveDecrypted(); });

            this.MainWindowView.FindControl<MenuItem>("MenuItem_Open_JP_1.0.7-1.0.9").Command = this.CMD_Open_JP_107_109;
            this.MainWindowView.FindControl<MenuItem>("MenuItem_Open_JP_1.0.4-1.0.6").Command = this.CMD_Open_JP_104_106;
            this.MainWindowView.FindControl<MenuItem>("MenuItem_Open_JP_1.0.2-1.0.3").Command = this.CMD_Open_JP_102_103;

            this.MainWindowView.FindControl<MenuItem>("MenuItem_Save").Command = this.CMD_SaveEncrypted;
            this.MainWindowView.FindControl<MenuItem>("MenuItem_SaveDecrypted").Command = this.CMD_SaveEncrypted;

            // No idea how to bind these from code. Currently bound via XAML

            // mainTree.Bind(mainTree.Items, OBSV_Graph);
            // TextBox console = this.MainWindowView.Find<TextBox>("TextBox_Log").Text = this.OBSV_Log;
            // console.Bind(console.Text, this.OBSV_Log);

            this.GenerateObjectGraph();
            this.LogWrite("Ready.");
        }

        protected void GenerateObjectGraph()
        {
            SaveData save = this._SaveData;
            if(save != null) {
                GraphSerializer serializer = new(save.Locale, save.Version);
                ObjectGraph baseNode = serializer.WriteRF5Save(save);
                UiObjectGraph uiBaseNode = UiObjectGraph.Wrap(baseNode);
                this.OBSV_Graph = new ObservableCollection<UiObjectGraph>(uiBaseNode.Values);
            }
        }


        protected async Task Cmd_OpenAsync(SaveData.LOCALE locale, int version)
        {
            this.Requested_Locale = locale;
            this.Requested_Version = version;

            OpenFileDialog dialog = new() {
                Title = String.Format("Open File ({0} ver {1})", locale, version),
                AllowMultiple = false,
                // Filters = new List<FileDialogFilter> { }, // Nothing to filter
            };
            string[] inputPaths = await dialog.ShowAsync(this.MainWindowView);
            this._SaveData = SaveData.FromEncryptedFile(path: inputPaths[0],
                                                    version: this.Requested_Version,
                                                    locale: this.Requested_Locale);
            this.GenerateObjectGraph();

            this.LogWrite(String.Format("Loaded file from {0}.", inputPaths[0]));
        }

        protected async Task Cmd_SaveEncrypted()
        {
            SaveFileDialog dialog = new() {
                Title = String.Format("Open File ({0} ver {1})", this.Requested_Locale, this.Requested_Version)
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
                Title = String.Format("Open File ({0} ver {1})", this.Requested_Locale, this.Requested_Version)
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
