using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Eliza.Avalonia.ViewModels;
using System;
using System.Collections.Generic;

namespace Eliza.Avalonia.Views.Editors
{
    public partial class ItemDataEditorView : BaseEditorView
    {

        protected ItemDataEditorViewModel? ViewModel;

        protected List<AutoCompleteBox> AutoCompleteBoxes = new();
        protected Dictionary<string, Action<int>> AutoboxValueDispatcher = new();

        public ItemDataEditorView() { }

        public ItemDataEditorView(ItemDataEditorViewModel viewModel)
        {
            this.ViewModel = viewModel;
            InitializeComponent();
            this.InitAutoboxes();
        }

        public void LoadContext(ItemDataEditorViewModel context)
        {
            this.ViewModel = context;
            this.DataContext = this.ViewModel;
            this.PrePopulateAutoBoxes();
        }

        protected void InitAutoboxes()
        {
            this.AutoCompleteBoxes.Add(this.Find<AutoCompleteBox>("AutoBox_ItemId"));
            this.AutoCompleteBoxes.Add(this.Find<AutoCompleteBox>("AutoBox_AddedItems0"));
            this.AutoCompleteBoxes.Add(this.Find<AutoCompleteBox>("AutoBox_AddedItems1"));
            this.AutoCompleteBoxes.Add(this.Find<AutoCompleteBox>("AutoBox_AddedItems2"));
            this.AutoCompleteBoxes.Add(this.Find<AutoCompleteBox>("AutoBox_AddedItems3"));
            this.AutoCompleteBoxes.Add(this.Find<AutoCompleteBox>("AutoBox_AddedItems4"));
            this.AutoCompleteBoxes.Add(this.Find<AutoCompleteBox>("AutoBox_AddedItems5"));
            this.AutoCompleteBoxes.Add(this.Find<AutoCompleteBox>("AutoBox_AddedItems6"));
            this.AutoCompleteBoxes.Add(this.Find<AutoCompleteBox>("AutoBox_AddedItems7"));
            this.AutoCompleteBoxes.Add(this.Find<AutoCompleteBox>("AutoBox_AddedItems8"));
            this.AutoCompleteBoxes.Add(this.Find<AutoCompleteBox>("AutoBox_SourceItems0"));
            this.AutoCompleteBoxes.Add(this.Find<AutoCompleteBox>("AutoBox_SourceItems1"));
            this.AutoCompleteBoxes.Add(this.Find<AutoCompleteBox>("AutoBox_SourceItems2"));
            this.AutoCompleteBoxes.Add(this.Find<AutoCompleteBox>("AutoBox_SourceItems3"));
            this.AutoCompleteBoxes.Add(this.Find<AutoCompleteBox>("AutoBox_SourceItems4"));
            this.AutoCompleteBoxes.Add(this.Find<AutoCompleteBox>("AutoBox_SourceItems5"));
            this.AutoCompleteBoxes.Add(this.Find<AutoCompleteBox>("AutoBox_ArrangeItems0"));
            this.AutoCompleteBoxes.Add(this.Find<AutoCompleteBox>("AutoBox_ArrangeItems1"));
            this.AutoCompleteBoxes.Add(this.Find<AutoCompleteBox>("AutoBox_ArrangeItems2"));
            this.AutoCompleteBoxes.Add(this.Find<AutoCompleteBox>("AutoBox_ArrangeOverride"));

            foreach (AutoCompleteBox autobox in this.AutoCompleteBoxes) {
                autobox.TextChanged += this.AutoCompleteTextChangedHandler;
                // autobox.LostFocus += this.AutoCompleteLostFocusHandler;
            }

            if(this.ViewModel != null) {
                this.AutoboxValueDispatcher["AutoBox_ItemId"] = (x) => this.ViewModel.ItemId = x;
                this.AutoboxValueDispatcher["AutoBox_AddedItems0"] = (x) => this.ViewModel.AddedItems0 = x;
                this.AutoboxValueDispatcher["AutoBox_AddedItems1"] = (x) => this.ViewModel.AddedItems1 = x;
                this.AutoboxValueDispatcher["AutoBox_AddedItems2"] = (x) => this.ViewModel.AddedItems2 = x;
                this.AutoboxValueDispatcher["AutoBox_AddedItems3"] = (x) => this.ViewModel.AddedItems3 = x;
                this.AutoboxValueDispatcher["AutoBox_AddedItems4"] = (x) => this.ViewModel.AddedItems4 = x;
                this.AutoboxValueDispatcher["AutoBox_AddedItems5"] = (x) => this.ViewModel.AddedItems5 = x;
                this.AutoboxValueDispatcher["AutoBox_AddedItems6"] = (x) => this.ViewModel.AddedItems6 = x;
                this.AutoboxValueDispatcher["AutoBox_AddedItems7"] = (x) => this.ViewModel.AddedItems7 = x;
                this.AutoboxValueDispatcher["AutoBox_AddedItems8"] = (x) => this.ViewModel.AddedItems8 = x;
                this.AutoboxValueDispatcher["AutoBox_SourceItems0"] = (x) => this.ViewModel.SourceItems0 = x;
                this.AutoboxValueDispatcher["AutoBox_SourceItems1"] = (x) => this.ViewModel.SourceItems1 = x;
                this.AutoboxValueDispatcher["AutoBox_SourceItems2"] = (x) => this.ViewModel.SourceItems2 = x;
                this.AutoboxValueDispatcher["AutoBox_SourceItems3"] = (x) => this.ViewModel.SourceItems3 = x;
                this.AutoboxValueDispatcher["AutoBox_SourceItems4"] = (x) => this.ViewModel.SourceItems4 = x;
                this.AutoboxValueDispatcher["AutoBox_SourceItems5"] = (x) => this.ViewModel.SourceItems5 = x;
                this.AutoboxValueDispatcher["AutoBox_ArrangeItems0"] = (x) => this.ViewModel.ArrangeItems0 = x;
                this.AutoboxValueDispatcher["AutoBox_ArrangeItems1"] = (x) => this.ViewModel.ArrangeItems1 = x;
                this.AutoboxValueDispatcher["AutoBox_ArrangeItems2"] = (x) => this.ViewModel.ArrangeItems2 = x;
                this.AutoboxValueDispatcher["AutoBox_ArrangeOverride"] = (x) => this.ViewModel.ArrangeOverride = x;
            }
        }

        protected void PrePopulateAutoBoxes()
        {       
            if(this.ViewModel != null) {
                this.Find<AutoCompleteBox>("AutoBox_ItemId").Text = this.ViewModel.ItemId.ToString();
                this.Find<AutoCompleteBox>("AutoBox_AddedItems0").Text = this.ViewModel.AddedItems0.ToString();
                this.Find<AutoCompleteBox>("AutoBox_AddedItems1").Text = this.ViewModel.AddedItems1.ToString();
                this.Find<AutoCompleteBox>("AutoBox_AddedItems2").Text = this.ViewModel.AddedItems2.ToString();
                this.Find<AutoCompleteBox>("AutoBox_AddedItems3").Text = this.ViewModel.AddedItems3.ToString();
                this.Find<AutoCompleteBox>("AutoBox_AddedItems4").Text = this.ViewModel.AddedItems4.ToString();
                this.Find<AutoCompleteBox>("AutoBox_AddedItems5").Text = this.ViewModel.AddedItems5.ToString();
                this.Find<AutoCompleteBox>("AutoBox_AddedItems6").Text = this.ViewModel.AddedItems6.ToString();
                this.Find<AutoCompleteBox>("AutoBox_AddedItems7").Text = this.ViewModel.AddedItems7.ToString();
                this.Find<AutoCompleteBox>("AutoBox_AddedItems8").Text = this.ViewModel.AddedItems8.ToString();
                this.Find<AutoCompleteBox>("AutoBox_SourceItems0").Text = this.ViewModel.SourceItems0.ToString();
                this.Find<AutoCompleteBox>("AutoBox_SourceItems1").Text = this.ViewModel.SourceItems1.ToString();
                this.Find<AutoCompleteBox>("AutoBox_SourceItems2").Text = this.ViewModel.SourceItems2.ToString();
                this.Find<AutoCompleteBox>("AutoBox_SourceItems3").Text = this.ViewModel.SourceItems3.ToString();
                this.Find<AutoCompleteBox>("AutoBox_SourceItems4").Text = this.ViewModel.SourceItems4.ToString();
                this.Find<AutoCompleteBox>("AutoBox_SourceItems5").Text = this.ViewModel.SourceItems5.ToString();
                this.Find<AutoCompleteBox>("AutoBox_ArrangeItems0").Text = this.ViewModel.ArrangeItems0.ToString();
                this.Find<AutoCompleteBox>("AutoBox_ArrangeItems1").Text = this.ViewModel.ArrangeItems1.ToString();
                this.Find<AutoCompleteBox>("AutoBox_ArrangeItems2").Text = this.ViewModel.ArrangeItems2.ToString();
                this.Find<AutoCompleteBox>("AutoBox_ArrangeOverride").Text = this.ViewModel.ArrangeOverride.ToString();
            }
        }

        public void AutoCompleteTextChangedHandler(object? sender, object eventArgs)
        {
            if(sender != null && eventArgs != null && this.ViewModel != null) {
                RoutedEventArgs e = (RoutedEventArgs)eventArgs;

                AutoCompleteBox source = (AutoCompleteBox)sender;
                string text = source.Text;
                int itemId = int.MinValue;
                if(source.Text != null) {
                    // First try to see if the user selected from the dropdown.
                    if (ItemDataEditorViewModel._AutoCompleteStringToIdMap.ContainsKey(text)) {
                        itemId = ItemDataEditorViewModel._AutoCompleteStringToIdMap[text];
                    } else {
                        // Otherwise, try to see if it's a numeric input and match it against itemIds.
                        try {
                            int tryItemId = int.Parse(text);
                            if (Eliza.Data.Items.ItemIds.Contains(itemId)) {
                                itemId = tryItemId;
                            }
                        } catch (Exception) {
                            // On parse error, give up. Input is not well-formed.
                        }
                    }
                    if (itemId != int.MinValue) {
                        // Dispatch value to ViewModel observable properties depending on sender's id
                        if (source.Name != null) {
                            this.AutoboxValueDispatcher[source.Name](itemId);
                        }
                        source.Text = itemId.ToString();
                    }
                    e.Handled = true;
                }
            }
        }

        public void AutoCompleteLostFocusHandler(object? sender, RoutedEventArgs args)
        {
            if (sender != null && args != null && this.ViewModel != null) {
                AutoCompleteBox source = (AutoCompleteBox)sender;
                string text = source.Text;
                int itemId = int.MinValue;
                if (ItemDataEditorViewModel._AutoCompleteStringToIdMap.ContainsKey(text)) {
                    itemId = ItemDataEditorViewModel._AutoCompleteStringToIdMap[text];
                } else {
                    try {
                        int tryItemId = int.Parse(text);
                        if (Eliza.Data.Items.ItemIds.Contains(itemId)) {
                            itemId = tryItemId;
                        }
                    } catch (Exception) { }
                }
                if (itemId != int.MinValue) {
                    if(source.Name != null) {
                        this.AutoboxValueDispatcher[source.Name](itemId);
                    }
                }
            }
        }



        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
