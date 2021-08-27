using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using ReactiveUI;
using Eliza.Model;
using Eliza.Avalonia.Views;
using Avalonia.Controls;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.ObjectModel;
using System.Reflection;
using Eliza.Core.Serialization;

namespace Eliza.Avalonia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        // protected ViewModelBase Content; // Shouldn't be necessary. We only have one view.
        protected MainWindow? View;

        protected SaveData.LOCALE Requested_Locale;
        protected int Requested_Version;

        private SaveData _saveData;
        private string _log = ""; // Just use a backing string. We're not doing anything crazy.
        private ObservableCollection<object> _baseList;

        // Reactive properties

        public SaveData SaveData
        {
            get => this._saveData;
            set => this.RaiseAndSetIfChanged(ref this._saveData, value);
        }

        public ObservableCollection<object> OBSV_BaseList
        {
            get => this._baseList;
            set => this.RaiseAndSetIfChanged(ref this._baseList, value);
        }

        public string OBSV_Log
        {
            get => this._log;
            set => this.RaiseAndSetIfChanged(ref this._log, value);
        }

        // Commands. See https://www.reactiveui.net/docs/handbook/commands/
        // ReactiveComand<TInput, TOutput> cmd_name
        public ReactiveCommand<Unit, Unit> CMD_Open_JP_102_103 { get; }
        public ReactiveCommand<Unit, Unit> CMD_Open_JP_104_106 { get; }
        public ReactiveCommand<Unit, Unit> CMD_Open_JP_107_109 { get; }
        public ReactiveCommand<Unit, Unit> CMD_SaveEncrypted { get; }
        public ReactiveCommand<Unit, Unit> CMD_SaveDecrypted { get; }

        public MainWindowViewModel() : base()
        {
            this.View = MainWindow.Instance;

            this.CMD_Open_JP_102_103 = ReactiveCommand.Create(() => { Task task = this.Cmd_OpenAsync(SaveData.LOCALE.JP, 2); });
            this.CMD_Open_JP_104_106 = ReactiveCommand.Create(() => { Task task = this.Cmd_OpenAsync(SaveData.LOCALE.JP, 4); });
            this.CMD_Open_JP_107_109 = ReactiveCommand.Create(() => { Task task = this.Cmd_OpenAsync(SaveData.LOCALE.JP, 7); });
            this.CMD_SaveEncrypted = ReactiveCommand.Create(() => { Task task = this.Cmd_SaveEncrypted(); });
            this.CMD_SaveEncrypted = ReactiveCommand.Create(() => { Task task = this.Cmd_SaveDecrypted(); });
            this.GenerateBaseListFromSave();
            this.LogWrite("Ready.");
        }

        protected void GenerateBaseListFromSave()
        {
            SaveData save = this._saveData;
            List<object> list = new();
            if(save != null) {
                list.Add(save.header);
                list.Add(save.saveData);
                list.Add(save.footer);
            }
            this.OBSV_BaseList = new ObservableCollection<object>(list);
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
            string[] inputPaths = await dialog.ShowAsync(this.View);
            this.SaveData = SaveData.FromEncryptedFile(path: inputPaths[0],
                                                    version: this.Requested_Version,
                                                    locale: this.Requested_Locale);
            this.GenerateBaseListFromSave();
            this.LogWrite(String.Format("Loaded file from {0}.", inputPaths[0]));
        }

        protected async Task Cmd_SaveEncrypted()
        {
            SaveFileDialog dialog = new() {
                Title = String.Format("Open File ({0} ver {1})", this.Requested_Locale, this.Requested_Version)
                // Filters = new List<FileDialogFilter> { }, // Nothing to filter
            };
            string outputPath = await dialog.ShowAsync(this.View);
            this.SaveData.ToEncryptedFile(outputPath);
            this.LogWrite(String.Format("File saved to {0}.", outputPath));
        }

        protected async Task Cmd_SaveDecrypted()
        {
            SaveFileDialog dialog = new() {
                Title = String.Format("Open File ({0} ver {1})", this.Requested_Locale, this.Requested_Version)
                // Filters = new List<FileDialogFilter> { }, // Nothing to filter
            };
            string outputPath = await dialog.ShowAsync(this.View);
            this.SaveData.ToDecryptedFile(outputPath);
            this.LogWrite(String.Format("Dev: File saved to {0} without encryption.", outputPath));
        }

        protected void LogWrite(string str)
        {
            this.OBSV_Log = str + "\n" + this.OBSV_Log;
        }

    }
}
