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
using Avalonia.Collections;

namespace Eliza.Avalonia.ViewModels
{

    // Any reactive prooperties have to be defined within a class
    // which inherits from ReactiveObject, i.e. ViewModelBase.

    // On the other hand, event handlers can only be defined
    // inside views.

    public class MainWindowViewModel : ViewModelBase
    {

        protected MainWindow? View; // View of viewmodel

        public SaveData.LOCALE RequestedLocale { get; protected set; }
        public int RequestedVersion { get; protected set; }

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

        protected UiObjectGraph? _UiBaseNode;
        protected AvaloniaList<UiObjectGraph>? _ObsvObjectGraph; // ObjectGraph generated from savedata
        public AvaloniaList<UiObjectGraph>? OBSV_Graph
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


        public MainWindowViewModel() : base()
        {
            this.View = MainWindow.Instance;
            this.GenerateObjectGraph();
        }

        protected void GenerateObjectGraph()
        {
            if(this._SaveData != null) {
                SaveData save = this._SaveData;
                GraphSerializer serializer = new(save.Locale, save.Version);
                ObjectGraph baseNode = serializer.WriteRF5Save(save);
                // this.baseNode = serializer.WriteRF5Save(save);
                this._UiBaseNode = UiObjectGraph.Wrap(baseNode);
                // Due to treeview limitations, we're binding the children of the root node.
                this.OBSV_Graph = this._UiBaseNode.Children;
            }
        }

        public void OpenEncryptedFile(string inputPath, SaveData.LOCALE locale, int version)
        {
            this._SaveData = SaveData.FromEncryptedFile(path: inputPath, version: version, locale: locale);
            this.GenerateObjectGraph();
        }


        public void SaveEncryptedFile(string outputPath)
        {
            if(this._UiBaseNode != null) {
                ObjectGraph baseNode = UiObjectGraph.Unwrap(this._UiBaseNode);
                GraphDeserializer deserializer = new(this.RequestedLocale, this.RequestedVersion);
                SaveData modifiedSaveData = deserializer.ReadRF5Save(baseNode);
                modifiedSaveData.ToEncryptedFile(outputPath);
            }
        }

        public void SaveDecryptedFile(string outputPath)
        {
            if(this._UiBaseNode != null) {
                ObjectGraph baseNode = UiObjectGraph.Unwrap(this._UiBaseNode);
                GraphDeserializer deserializer = new(this.RequestedLocale, this.RequestedVersion);
                SaveData modifiedSaveData = deserializer.ReadRF5Save(baseNode);
                modifiedSaveData.ToDecryptedFile(outputPath);
            }
        }
    }
}
