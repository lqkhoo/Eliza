using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using ReactiveUI;
using Eliza.Model;
using Eliza.Avalonia.Views;
using Avalonia.Controls;
using System.Threading.Tasks;

namespace Eliza.Avalonia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        // We don't have a 'Model' per-se. The UI operates on a single
        // instance of Eliza.Model.SaveData, so we'll just have that
        // in here. Not much point putting it in MainWindow.
        protected SaveData? Model;


        protected SaveData.LOCALE Requested_Locale;
        protected int Requested_Version;

        public string Greeting => "Welcome to Avalonia!";
        public string MyString => "MyString";

        // Commands. See https://www.reactiveui.net/docs/handbook/commands/
        // ReactiveComand<TInput, TOutput> cmd_name

        public ReactiveCommand<Unit, Unit> CMD_Open_JP_102_103 { get; }
        public ReactiveCommand<Unit, Unit> CMD_Open_JP_104_106 { get; }
        public ReactiveCommand<Unit, Unit> CMD_Open_JP_107_109 { get; }

        public ReactiveCommand<Unit, Unit> CMD_Save { get; }

        public MainWindowViewModel() : base()
        {
            this.CMD_Open_JP_102_103 = ReactiveCommand.Create(() => { Task task = this.Cmd_OpenAsync(SaveData.LOCALE.JP, 2); });
            this.CMD_Open_JP_104_106 = ReactiveCommand.Create(() => { Task task = this.Cmd_OpenAsync(SaveData.LOCALE.JP, 4); });
            this.CMD_Open_JP_107_109 = ReactiveCommand.Create(() => { Task task = this.Cmd_OpenAsync(SaveData.LOCALE.JP, 7); });
            this.CMD_Save = ReactiveCommand.Create(() => { Task task = this.Cmd_Save(); });
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
            string[] inputPaths = await dialog.ShowAsync(MainWindow.Instance);
            this.LoadSaveFile(inputPaths[0]);
        }

        protected async Task Cmd_Save()
        {
            SaveFileDialog dialog = new() {
                Title = String.Format("Open File ({0} ver {1})", this.Requested_Locale, this.Requested_Version)
                // Filters = new List<FileDialogFilter> { }, // Nothing to filter
            };
            string outputPath = await dialog.ShowAsync(MainWindow.Instance);
        }

        protected void LoadSaveFile(string inputPath)
        {
            this.Model = SaveData.FromEncryptedFile(path: inputPath,
                                                    version: this.Requested_Version,
                                                    locale: this.Requested_Locale);
        }

    }
}
