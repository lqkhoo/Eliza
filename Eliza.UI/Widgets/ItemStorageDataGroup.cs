using Eliza.Model.Item;
using Eliza.UI.Helpers;
using Eto.Forms;
using System;

namespace Eliza.UI.Widgets
{
    public class ItemStorageDataGroup : GenericGroupBox
    {
        private ItemStorageData _itemStorageData;
        ListBox list = new();
        HBox parentHBox = new();

        delegate void PageUpdate(ItemData item);
        PageUpdate amountItemDataPageUpdate;
        PageUpdate seedItemDataPageUpdate;
        PageUpdate foodItemDataPageUpdate;
        PageUpdate equipItemDataPageUpdate;
        PageUpdate runeAbilityItemDataPageUpdate;
        PageUpdate fishItemDataPageUpdate;
        PageUpdate potToolItemDataPageUpdate;
        PageUpdate tabUpdate;

        public ItemStorageDataGroup(ItemStorageData itemStorageData, string text = "Item Storage Data") : base(text)
        {
            this._itemStorageData = itemStorageData;

            for (int i = 0; i < itemStorageData.ItemDatas.Length; i++)
            {
                switch (itemStorageData.ItemDatas[i])
                {
                    case PotToolItemData:
                        {
                            this.list.Items.Add($"Pot Tool Item {i}");
                            break;
                        }
                    case FishItemData:
                        {
                            this.list.Items.Add($"Fish Item {i}");
                            break;
                        }
                    case RuneAbilityItemData:
                        {
                            this.list.Items.Add($"Rune Ability Item {i}");
                            break;
                        }
                    case EquipItemData:
                        {
                            this.list.Items.Add($"Equip Item {i}");
                            break;
                        }
                    case FoodItemData:
                        {
                            this.list.Items.Add($"Food Item {i}");
                            break;
                        }
                    case SeedItemData:
                        {
                            this.list.Items.Add($"Seed Item {i}");
                            break;
                        }
                    case AmountItemData:
                        {
                            this.list.Items.Add($"Amount Item {i}");
                            break;
                        }
                    default:
                        {
                            this.list.Items.Add($"Item {i}");
                            break;
                        }
                }
            }

            this.list.SelectedIndexChanged += (object sender, EventArgs e) =>
            {
                this.tabUpdate(this._itemStorageData.ItemDatas[this.list.SelectedIndex]);
            };

            this.list.MouseDown += this.List_MouseDown;

            this.Setup();
        }

        private void List_MouseDown(object sender, MouseEventArgs e)
        { 
            var contextMenu = new ContextMenu();

            var amountItemDataMenuItem = new RadioMenuItem()
            {
                Text = "Amount Item Data"
            };

            var seedItemDataMenuItem = new ButtonMenuItem()
            {
                Text = "Seed Item Data"
            };

            var foodItemDataMenuItem = new ButtonMenuItem()
            {
                Text = "Food Item Data"
            };

            var equipItemDataMenuItem = new ButtonMenuItem()
            {
                Text = "Equip Item Data"
            };

            var runeAbilityItemDataMenuItem = new ButtonMenuItem()
            {
                Text = "Rune Ability Item Data"
            };

            var fishItemDataMenuItem = new ButtonMenuItem()
            {
                Text = "FishItem Data"
            };

            var potToolItemDataMenuItem = new ButtonMenuItem()
            {
                Text = "Pot Tool Item Data"
            };

            amountItemDataMenuItem.Click += (object sender, EventArgs e) =>
            {
                if (this.list.SelectedIndex >= 0 && (this._itemStorageData.ItemDatas[this.list.SelectedIndex] == null || this._itemStorageData.ItemDatas[this.list.SelectedIndex].GetType() == typeof(AmountItemData)))
                {
                    this._itemStorageData.ItemDatas[this.list.SelectedIndex] = new AmountItemData();
                    this.tabUpdate(this._itemStorageData.ItemDatas[this.list.SelectedIndex]);
                }
            };

            seedItemDataMenuItem.Click += (object sender, EventArgs e) =>
            {
                if (this.list.SelectedIndex >= 0 && (this._itemStorageData.ItemDatas[this.list.SelectedIndex] == null || this._itemStorageData.ItemDatas[this.list.SelectedIndex].GetType() == typeof(SeedItemData)))
                {
                    this._itemStorageData.ItemDatas[this.list.SelectedIndex] = new SeedItemData();
                    this.tabUpdate(this._itemStorageData.ItemDatas[this.list.SelectedIndex]);
                }
            };

            foodItemDataMenuItem.Click += (object sender, EventArgs e) =>
            {
                if (this.list.SelectedIndex >= 0 && (this._itemStorageData.ItemDatas[this.list.SelectedIndex] == null || this._itemStorageData.ItemDatas[this.list.SelectedIndex].GetType() == typeof(FoodItemData)))
                {
                    this._itemStorageData.ItemDatas[this.list.SelectedIndex] = new FoodItemData();
                    this.tabUpdate(this._itemStorageData.ItemDatas[this.list.SelectedIndex]);
                }
            };

            equipItemDataMenuItem.Click += (object sender, EventArgs e) =>
            {
                if (this.list.SelectedIndex >= 0 && (this._itemStorageData.ItemDatas[this.list.SelectedIndex] == null || this._itemStorageData.ItemDatas[this.list.SelectedIndex].GetType() == typeof(EquipItemData)))
                {
                    this._itemStorageData.ItemDatas[this.list.SelectedIndex] = new EquipItemData();
                    this.tabUpdate(this._itemStorageData.ItemDatas[this.list.SelectedIndex]);
                }
            };

            runeAbilityItemDataMenuItem.Click += (object sender, EventArgs e) =>
            {
                if (this.list.SelectedIndex >= 0 && (this._itemStorageData.ItemDatas[this.list.SelectedIndex] == null || this._itemStorageData.ItemDatas[this.list.SelectedIndex].GetType() != typeof(RuneAbilityItemData)))
                {
                    this._itemStorageData.ItemDatas[this.list.SelectedIndex] = new RuneAbilityItemData();
                    this.tabUpdate(this._itemStorageData.ItemDatas[this.list.SelectedIndex]);
                }
            };

            fishItemDataMenuItem.Click += (object sender, EventArgs e) =>
            {
                if (this.list.SelectedIndex >= 0 && (this._itemStorageData.ItemDatas[this.list.SelectedIndex] == null || this._itemStorageData.ItemDatas[this.list.SelectedIndex].GetType() != typeof(FishItemData)))
                {
                    this._itemStorageData.ItemDatas[this.list.SelectedIndex] = new FishItemData();
                    this.tabUpdate(this._itemStorageData.ItemDatas[this.list.SelectedIndex]);
                }
            };

            potToolItemDataMenuItem.Click += (object sender, EventArgs e) =>
            {
                if (this.list.SelectedIndex >= 0 && (this._itemStorageData.ItemDatas[this.list.SelectedIndex] == null || this._itemStorageData.ItemDatas[this.list.SelectedIndex].GetType() != typeof(PotToolItemData)))
                {
                    this._itemStorageData.ItemDatas[this.list.SelectedIndex] = new PotToolItemData();
                    this.tabUpdate(this._itemStorageData.ItemDatas[this.list.SelectedIndex]);
                }
            };
            contextMenu.Items.Add(amountItemDataMenuItem);
            contextMenu.Items.Add(seedItemDataMenuItem);
            contextMenu.Items.Add(foodItemDataMenuItem);
            contextMenu.Items.Add(equipItemDataMenuItem);
            contextMenu.Items.Add(runeAbilityItemDataMenuItem);
            contextMenu.Items.Add(fishItemDataMenuItem);
            contextMenu.Items.Add(potToolItemDataMenuItem);

            this.list.ContextMenu = contextMenu;
        }

        public ItemStorageDataGroup(string text = "Item Storage Data") : base(text)
        {
            this.Setup();
        }

        public void ChangeReferenceValue(ItemStorageData itemStorageData)
        {
            this._itemStorageData = itemStorageData;

            for (int i = 0; i < itemStorageData.ItemDatas.Length; i++)
            {
                switch (itemStorageData.ItemDatas[i])
                {
                    case PotToolItemData:
                        {
                            this.list.Items.Add($"Pot Tool Item {i}");
                            break;
                        }
                    case FishItemData:
                        {
                            this.list.Items.Add($"Fish Item {i}");
                            break;
                        }
                    case RuneAbilityItemData:
                        {
                            this.list.Items.Add($"Rune Ability Item {i}");
                            break;
                        }
                    case EquipItemData:
                        {
                            this.list.Items.Add($"Equip Item {i}");
                            break;
                        }
                    case FoodItemData:
                        {
                            this.list.Items.Add($"Food Item {i}");
                            break;
                        }
                    case SeedItemData:
                        {
                            this.list.Items.Add($"Seed Item {i}");
                            break;
                        }
                    case AmountItemData:
                        {
                            this.list.Items.Add($"Amount Item {i}");
                            break;
                        }
                    default:
                        {
                            this.list.Items.Add($"Item {i}");
                            break;
                        }
                }
            }

            this.list.SelectedIndexChanged += (object sender, EventArgs e) =>
            {
                this.tabUpdate(this._itemStorageData.ItemDatas[this.list.SelectedIndex]);
            };
        }

        private void Setup()
        {
            var tabControl = new TabControl();

            //AmountItemData
            var amountItemDataPage = new TabPage()
            {
                Text = "Amount Item Data"
            };

            {
                //ItemData
                var itemID = new SpinBox("Item ID");

                //AmountItemData
                var levelAmount = new GroupBox()
                {
                    Text = "Level Amount"
                };
                var levelAmountList = new ListBox();
                var levelAmountHBox = new HBox();
                var levelAmountData = new VBox();

                var spinBox = new SpinBox();

                levelAmountList.SelectedIndexChanged += (object sender, EventArgs e) =>
                {
                    spinBox.ChangeReferenceValue(
                        new Ref<int>(() => ((AmountItemData)this._itemStorageData.ItemDatas[this.list.SelectedIndex]).LevelAmount[levelAmountList.SelectedIndex], v => { ((AmountItemData)this._itemStorageData.ItemDatas[this.list.SelectedIndex]).LevelAmount[levelAmountList.SelectedIndex] = v; })
                    );
                };
               levelAmountData.Items.Add(spinBox);

                levelAmountHBox.Items.Add(levelAmountList);
                levelAmountHBox.Items.Add(levelAmountData);

                levelAmount.Content = levelAmountHBox;

                this.amountItemDataPageUpdate = delegate (ItemData item)
                {
                    levelAmountList.Items.Clear();
                    itemID.ChangeReferenceValue(
                        new Ref<int>(() => item.ItemId, v => { item.ItemId = v; })
                    );

                    if (this.list.SelectedIndex >= 0)
                    {
                        for (int i = 0; i < ((AmountItemData)item).LevelAmount.Count; i++)
                        {
                            levelAmountList.Items.Add($"Level Amount {i}");
                        }
                    }
                };

                var vBox = new VBox();
                StackLayoutItem[] vBoxItems =
                {
                    //ItemData
                    itemID,
                    //AmountItemData
                    levelAmount,
                };


                foreach (var _ in vBoxItems)
                {
                    vBox.Items.Add(_);
                }

                var scroll = new Scrollable();
                scroll.Content = vBox;
                amountItemDataPage.Content = scroll;
            }

            //SeedItemData
            var seedItemDataPage = new TabPage()
            {
                Text = "Seed Item Data"
            };

            {
                //ItemData
                var itemID = new SpinBox("Item ID");

                //AmountItemData
                var levelAmount = new GroupBox()
                {
                    Text = "Level Amount"
                };
                var levelAmountList = new ListBox();
                var levelAmountHBox = new HBox();
                var levelAmountData = new VBox();

                var spinBox = new SpinBox();

                levelAmountList.SelectedIndexChanged += (object sender, EventArgs e) =>
                {
                    spinBox.ChangeReferenceValue(
                        new Ref<int>(() => ((SeedItemData)this._itemStorageData.ItemDatas[this.list.SelectedIndex]).LevelAmount[levelAmountList.SelectedIndex], v => { ((SeedItemData)this._itemStorageData.ItemDatas[this.list.SelectedIndex]).LevelAmount[levelAmountList.SelectedIndex] = v; })
                    );
                };

                levelAmountData.Items.Add(spinBox);

                levelAmountHBox.Items.Add(levelAmountList);
                levelAmountHBox.Items.Add(levelAmountData);

                levelAmount.Content = levelAmountHBox;

                this.seedItemDataPageUpdate = delegate (ItemData item)
                {
                    levelAmountList.Items.Clear();
                    itemID.ChangeReferenceValue(
                        new Ref<int>(() => item.ItemId, v => { item.ItemId = v; })
                    );

                    if (this.list.SelectedIndex >= 0)
                    {
                        for (int i = 0; i < ((SeedItemData)item).LevelAmount.Count; i++)
                        {
                            levelAmountList.Items.Add($"Level Amount {i}");
                        }
                    }
                };

                StackLayoutItem[] vBoxItems =
                {
                    //ItemData
                    itemID,
                    //AmountItemData
                    levelAmount,
                };

                var vBox = new VBox();
                foreach (var _ in vBoxItems)
                {
                    vBox.Items.Add(_);
                }

                var scroll = new Scrollable();
                scroll.Content = vBox;
                seedItemDataPage.Content = scroll;
            }

            //FoodItemData
            var foodItemDataPage = new TabPage()
            {
                Text = "Food Item Data"
            };

            {
                //ItemData
                var itemID = new SpinBox("Item ID");

                //NotAmountItemData
                var level = new SpinBox("Level");

                //SynthesisItemData
                var sourceItems = new GroupBox()
                {
                    Text = "Source Items"
                };

                var sourceItemsList = new ListBox();
                var sourceItemsHBox = new HBox();
                var sourceItemsData = new VBox();

                var sourceItemsSpinBox = new SpinBox();

                sourceItemsList.SelectedIndexChanged += (object sender, EventArgs e) =>
                {
                    sourceItemsSpinBox.ChangeReferenceValue(
                        new Ref<int>(() => ((FoodItemData)this._itemStorageData.ItemDatas[this.list.SelectedIndex]).SourceItems[sourceItemsList.SelectedIndex], v => { ((FoodItemData)this._itemStorageData.ItemDatas[this.list.SelectedIndex]).SourceItems[sourceItemsList.SelectedIndex] = v; })
                    );
                };

                sourceItemsData.Items.Add(sourceItemsSpinBox);

                sourceItemsHBox.Items.Add(sourceItemsList);
                sourceItemsHBox.Items.Add(sourceItemsData);

                sourceItems.Content = sourceItemsHBox;

                //FoodItemData
                var isArange = new Widgets.CheckBox("Arrange");

                this.foodItemDataPageUpdate = delegate(ItemData item)
                {
                    sourceItemsList.Items.Clear();
                    //ItemData
                    itemID.ChangeReferenceValue(
                        new Ref<int>(() => ((FoodItemData)item).ItemId, v => { ((FoodItemData)item).ItemId = v; })
                    );

                    //NotAmountItemData
                    level.ChangeReferenceValue(
                        new Ref<int>(() => ((FoodItemData)item).Level, v => { ((FoodItemData)item).Level = v; })
                    );

                    //SynthesisItemData
                    if (this.list.SelectedIndex >= 0)
                    {
                        for (int i = 0; i < ((FoodItemData)item).SourceItems.Length; i++)
                        {
                            sourceItemsList.Items.Add($"Item {i}");
                        }
                    }

                    //FoodItemData
                    isArange.ChangeReferenceValue(
                        new Ref<bool>(() => ((FoodItemData)item).IsArrange, v => { ((FoodItemData)item).IsArrange = v; })
                     );
                };

                StackLayoutItem[] vBoxItems =
                {
                    //ItemData
                    itemID,
                    //NotAmountItemData
                    level,
                    //SynthesisItemData
                    sourceItems,
                    //FoodItemData
                    isArange
                };

                var vBox = new VBox();
                foreach (var _ in vBoxItems)
                {
                    vBox.Items.Add(_);
                }

                var scroll = new Scrollable();
                scroll.Content = vBox;
                foodItemDataPage.Content = scroll;
            }

            var equipItemDataPage = new TabPage()
            {
                Text = "Equip Item Data"
            };

            {
                //ItemData
                var itemID = new SpinBox("Item ID");

                //NotAmountItemData
                var level = new SpinBox("Level");

                //SynthesisItemData
                var sourceItems = new GroupBox()
                {
                    Text = "Source Items"
                };

                var sourceItemsList = new ListBox();
                var sourceItemsHBox = new HBox();
                var sourceItemsData = new VBox();

                var sourceItemsSpinBox = new SpinBox();

                sourceItemsData.Items.Add(sourceItemsSpinBox);

                sourceItemsHBox.Items.Add(sourceItemsList);
                sourceItemsHBox.Items.Add(sourceItemsData);

                sourceItems.Content = sourceItemsHBox;

                //EquipItemData
                var addedItems = new GroupBox()
                {
                    Text = "Added Items"
                };

                var addedItemsList = new ListBox();
                var addedItemsHBox = new HBox();
                var addedItemsData = new VBox();

                var addedItemsSpinBox = new SpinBox();

                addedItemsData.Items.Add(addedItemsSpinBox);

                addedItemsHBox.Items.Add(addedItemsList);
                addedItemsHBox.Items.Add(addedItemsData);

                addedItems.Content = addedItemsHBox;

                var arrangedItems = new GroupBox()
                {
                    Text = "Arranged Items"
                };

                var arrangedItemsList = new ListBox();
                var arrangedItemsHBox = new HBox();
                var arrangedItemsData = new VBox();


                var arrangedItemsSpinBox = new SpinBox();

                arrangedItemsData.Items.Add(arrangedItemsSpinBox);

                arrangedItemsHBox.Items.Add(arrangedItemsList);
                arrangedItemsHBox.Items.Add(arrangedItemsData);

                arrangedItems.Content = arrangedItemsHBox;

                var arrangeOverride = new SpinBox("Arrange Override");
                var baseLevel = new SpinBox("Base Level");
                var sozaiLevel = new SpinBox("Sozai Level");
                var dualWorkSmithBonusType = new SpinBox("Dual Work Smith Bonus Type");
                var dualWorkLoveLevel = new SpinBox("Dual Work Love Level");
                var dualWorkActor = new SpinBox("Dual Work Actor");
                var dualWorkParam = new SpinBox("Dual Work Param");

                sourceItemsList.SelectedIndexChanged += (object sender, EventArgs e) =>
                {
                    sourceItemsSpinBox.ChangeReferenceValue(
                        new Ref<int>(() => ((EquipItemData)this._itemStorageData.ItemDatas[this.list.SelectedIndex]).SourceItems[sourceItemsList.SelectedIndex], v => { ((EquipItemData)this._itemStorageData.ItemDatas[this.list.SelectedIndex]).SourceItems[sourceItemsList.SelectedIndex] = v; })
                    );
                };

                addedItemsList.SelectedIndexChanged += (object sender, EventArgs e) =>
                {
                    addedItemsSpinBox.ChangeReferenceValue(
                        new Ref<int>(() => ((EquipItemData)this._itemStorageData.ItemDatas[this.list.SelectedIndex]).AddedItems[addedItemsList.SelectedIndex], v => { ((EquipItemData)this._itemStorageData.ItemDatas[this.list.SelectedIndex]).AddedItems[addedItemsList.SelectedIndex] = v; })
                    );
                };

                arrangedItemsList.SelectedIndexChanged += (object sender, EventArgs e) =>
                {
                    arrangedItemsSpinBox.ChangeReferenceValue(
                        new Ref<int>(() => ((EquipItemData)this._itemStorageData.ItemDatas[this.list.SelectedIndex]).ArrangeItems[arrangedItemsList.SelectedIndex], v => { ((EquipItemData)this._itemStorageData.ItemDatas[this.list.SelectedIndex]).ArrangeItems[arrangedItemsList.SelectedIndex] = v; })
                    );
                };

                this.equipItemDataPageUpdate = delegate (ItemData item)
                {
                    sourceItemsList.Items.Clear();
                    addedItemsList.Items.Clear();
                    arrangedItemsList.Items.Clear();
                    //ItemData
                    itemID.ChangeReferenceValue(
                        new Ref<int>(() => ((EquipItemData)item).ItemId, v => { ((EquipItemData)item).ItemId = v; })
                    );

                    //NotAmountItemData
                    level.ChangeReferenceValue(
                        new Ref<int>(() => ((EquipItemData)item).Level, v => { ((EquipItemData)item).Level = v; })
                    );

                    //SynthesisItemData
                    if (this.list.SelectedIndex >= 0)
                    {
                        for (int i = 0; i < ((EquipItemData)item).SourceItems.Length; i++)
                        {
                            sourceItemsList.Items.Add($"Item {i}");
                        }
                    }

                    //EquipItemData
                    if (this.list.SelectedIndex >= 0)
                    {
                        for (int i = 0; i < ((EquipItemData)item).SourceItems.Length; i++)
                        {
                            addedItemsList.Items.Add($"Item {i}");
                        }
                    }

                    if (this.list.SelectedIndex >= 0)
                    {
                        for (int i = 0; i < ((EquipItemData)item).SourceItems.Length; i++)
                        {
                            arrangedItemsList.Items.Add($"Item {i}");
                        }
                    }

                    arrangeOverride.ChangeReferenceValue(
                        new Ref<int>(() => ((EquipItemData)item).ArrangeOverride, v => { ((EquipItemData)item).ArrangeOverride = v; })
                    );

                    baseLevel.ChangeReferenceValue(
                        new Ref<int>(() => ((EquipItemData)item).BaseLevel, v => { ((EquipItemData)item).BaseLevel = v; })
                    );

                    sozaiLevel.ChangeReferenceValue(
                        new Ref<int>(() => ((EquipItemData)item).SozaiLevel, v => { ((EquipItemData)item).SozaiLevel = v; })
                    );

                    dualWorkSmithBonusType.ChangeReferenceValue(
                        new Ref<int>(() => ((EquipItemData)item).DualWorkSmithBonusType, v => { ((EquipItemData)item).DualWorkSmithBonusType = v; })
                    );

                    dualWorkLoveLevel.ChangeReferenceValue(
                        new Ref<int>(() => ((EquipItemData)item).DualWorkLoveLevel, v => { ((EquipItemData)item).DualWorkLoveLevel = v; })
                    );

                    dualWorkActor.ChangeReferenceValue(
                        new Ref<int>(() => ((EquipItemData)item).DualWorkActor, v => { ((EquipItemData)item).DualWorkActor = v; })
                    );

                    dualWorkParam.ChangeReferenceValue(
                        new Ref<int>(() => ((EquipItemData)item).DualWorkParam, v => { ((EquipItemData)item).DualWorkParam = v; })
                    );
                };

                StackLayoutItem[] vBoxItems =
                {
                    //ItemData
                    itemID,
                    //NotAmountItemData
                    level,
                    //SynthesisItemData
                    sourceItems,
                    //EquipItemData
                    addedItems,
                    arrangedItems,
                    arrangeOverride,
                    baseLevel,
                    sozaiLevel,
                    dualWorkSmithBonusType,
                    dualWorkLoveLevel,
                    dualWorkActor,
                    dualWorkParam,
                };

                var vBox = new VBox();
                foreach (var _ in vBoxItems)
                {
                    vBox.Items.Add(_);
                }

                var scroll = new Scrollable();
                scroll.Content = vBox;
                equipItemDataPage.Content = scroll;
            }

            var runeAbilityItemDataPage = new TabPage()
            {
                Text = "Rune Ability Item Data"
            };

            {
                //ItemData
                var itemID = new SpinBox("Item ID");

                //NotAmountItemData
                var level = new SpinBox("Level");

                this.runeAbilityItemDataPageUpdate = delegate (ItemData item)
                {
                    //ItemData
                    itemID.ChangeReferenceValue(
                        new Ref<int>(() => ((RuneAbilityItemData)item).ItemId, v => { ((RuneAbilityItemData)item).ItemId = v; })
                    );

                    //NotAmountItemData
                    level.ChangeReferenceValue(
                        new Ref<int>(() => ((RuneAbilityItemData)item).Level, v => { ((RuneAbilityItemData)item).Level = v; })
                    );
                };

                StackLayoutItem[] vBoxItems =
                {
                        //ItemData
                        itemID,
                        //NotAmountItemData
                        level,
                    };

                var vBox = new VBox();
                foreach (var _ in vBoxItems)
                {
                    vBox.Items.Add(_);
                }

                var scroll = new Scrollable();
                scroll.Content = vBox;
                runeAbilityItemDataPage.Content = scroll;
            }

            var fishItemDataPage = new TabPage()
            {
                Text = "Fish Item Data"
            };

            {
                //ItemData
                var itemID = new SpinBox("Item ID");

                //NotAmountItemData
                var level = new SpinBox("Level");

                this.fishItemDataPageUpdate = delegate (ItemData item)
                {
                    //ItemData
                    itemID.ChangeReferenceValue(
                        new Ref<int>(() => ((FishItemData)item).ItemId, v => { ((FishItemData)item).ItemId = v; })
                    );

                    //NotAmountItemData
                    level.ChangeReferenceValue(
                        new Ref<int>(() => ((FishItemData)item).Level, v => { ((FishItemData)item).Level = v; })
                    );
                };

                StackLayoutItem[] vBoxItems =
{
                        //ItemData
                        itemID,
                        //NotAmountItemData
                        level,
                    };

                var vBox = new VBox();
                foreach (var _ in vBoxItems)
                {
                    vBox.Items.Add(_);
                }

                var scroll = new Scrollable();
                scroll.Content = vBox;
                fishItemDataPage.Content = scroll;
            }

            var potToolItemDataPage = new TabPage()
            {
                Text = "Pot Tool Item Data"
            };

            {
                //ItemData
                var itemID = new SpinBox("Item ID");

                //NotAmountItemData
                var level = new SpinBox("Level");

                //SynthesisItemData
                var sourceItems = new GroupBox()
                {
                    Text = "Source Items"
                };

                var sourceItemsList = new ListBox();
                var sourceItemsHBox = new HBox();
                var sourceItemsData = new VBox();

                var sourceItemsSpinBox = new SpinBox();

                sourceItemsData.Items.Add(sourceItemsSpinBox);

                sourceItemsHBox.Items.Add(sourceItemsList);
                sourceItemsHBox.Items.Add(sourceItemsData);

                sourceItems.Content = sourceItemsHBox;

                //PotToolItemData
                var addedItems = new GroupBox()
                {
                    Text = "Added Items"
                };

                var addedItemsList = new ListBox();
                var addedItemsHBox = new HBox();
                var addedItemsData = new VBox();

                var addedItemsSpinBox = new SpinBox();

                addedItemsData.Items.Add(addedItemsSpinBox);

                addedItemsHBox.Items.Add(addedItemsList);
                addedItemsHBox.Items.Add(addedItemsData);

                addedItems.Content = addedItemsHBox;

                var arrangedItems = new GroupBox()
                {
                    Text = "Arranged Items"
                };

                var arrangedItemsList = new ListBox();
                var arrangedItemsHBox = new HBox();
                var arrangedItemsData = new VBox();


                var arrangedItemsSpinBox = new SpinBox();

                arrangedItemsData.Items.Add(arrangedItemsSpinBox);

                arrangedItemsHBox.Items.Add(arrangedItemsList);
                arrangedItemsHBox.Items.Add(arrangedItemsData);

                arrangedItems.Content = arrangedItemsHBox;

                var arrangeOverride = new SpinBox("Arrange Override");
                var baseLevel = new SpinBox("Base Level");
                var sozaiLevel = new SpinBox("Sozai Level");
                var dualWorkSmithBonusType = new SpinBox("Dual Work Smith Bonus Type");
                var dualWorkLoveLevel = new SpinBox("Dual Work Love Level");
                var dualWorkActor = new SpinBox("Dual Work Actor");
                var dualWorkParam = new SpinBox("Dual Work Param");

                //PotToolItemData
                var capacity = new SpinBox("Capacity");

                sourceItemsList.SelectedIndexChanged += (object sender, EventArgs e) =>
                {
                    sourceItemsSpinBox.ChangeReferenceValue(
                        new Ref<int>(() => ((PotToolItemData)this._itemStorageData.ItemDatas[this.list.SelectedIndex]).SourceItems[sourceItemsList.SelectedIndex], v => { ((PotToolItemData)this._itemStorageData.ItemDatas[this.list.SelectedIndex]).SourceItems[sourceItemsList.SelectedIndex] = v; })
                    );
                };

                addedItemsList.SelectedIndexChanged += (object sender, EventArgs e) =>
                {
                    addedItemsSpinBox.ChangeReferenceValue(
                        new Ref<int>(() => ((PotToolItemData)this._itemStorageData.ItemDatas[this.list.SelectedIndex]).AddedItems[addedItemsList.SelectedIndex], v => { ((PotToolItemData)this._itemStorageData.ItemDatas[this.list.SelectedIndex]).AddedItems[addedItemsList.SelectedIndex] = v; })
                    );
                };

                arrangedItemsList.SelectedIndexChanged += (object sender, EventArgs e) =>
                {
                    arrangedItemsSpinBox.ChangeReferenceValue(
                        new Ref<int>(() => ((PotToolItemData)this._itemStorageData.ItemDatas[this.list.SelectedIndex]).ArrangeItems[arrangedItemsList.SelectedIndex], v => { ((PotToolItemData)this._itemStorageData.ItemDatas[this.list.SelectedIndex]).ArrangeItems[arrangedItemsList.SelectedIndex] = v; })
                    );
                };

                this.potToolItemDataPageUpdate = delegate (ItemData item)
                {
                    sourceItemsList.Items.Clear();
                    addedItemsList.Items.Clear();
                    arrangedItemsList.Items.Clear();
                    //ItemData
                    itemID.ChangeReferenceValue(
                        new Ref<int>(() => ((PotToolItemData)item).ItemId, v => { ((PotToolItemData)item).ItemId = v; })
                    );

                    //NotAmountItemData
                    level.ChangeReferenceValue(
                        new Ref<int>(() => ((PotToolItemData)item).Level, v => { ((PotToolItemData)item).Level = v; })
                    );

                    //SynthesisItemData
                    if (this.list.SelectedIndex >= 0)
                    {
                        for (int i = 0; i < ((PotToolItemData)item).SourceItems.Length; i++)
                        {
                            sourceItemsList.Items.Add($"Item {i}");
                        }
                    }

                    //PotToolItemData
                    if (this.list.SelectedIndex >= 0)
                    {
                        for (int i = 0; i < ((PotToolItemData)item).SourceItems.Length; i++)
                        {
                            addedItemsList.Items.Add($"Item {i}");
                        }
                    }

                    if (this.list.SelectedIndex >= 0)
                    {
                        for (int i = 0; i < ((PotToolItemData)item).SourceItems.Length; i++)
                        {
                            arrangedItemsList.Items.Add($"Item {i}");
                        }
                    }

                    arrangeOverride.ChangeReferenceValue(
                        new Ref<int>(() => ((PotToolItemData)item).ArrangeOverride, v => { ((PotToolItemData)item).ArrangeOverride = v; })
                    );

                    baseLevel.ChangeReferenceValue(
                        new Ref<int>(() => ((PotToolItemData)item).BaseLevel, v => { ((PotToolItemData)item).BaseLevel = v; })
                    );

                    sozaiLevel.ChangeReferenceValue(
                        new Ref<int>(() => ((PotToolItemData)item).SozaiLevel, v => { ((PotToolItemData)item).SozaiLevel = v; })
                    );

                    dualWorkSmithBonusType.ChangeReferenceValue(
                        new Ref<int>(() => ((PotToolItemData)item).DualWorkSmithBonusType, v => { ((PotToolItemData)item).DualWorkSmithBonusType = v; })
                    );

                    dualWorkLoveLevel.ChangeReferenceValue(
                        new Ref<int>(() => ((PotToolItemData)item).DualWorkLoveLevel, v => { ((PotToolItemData)item).DualWorkLoveLevel = v; })
                    );

                    dualWorkActor.ChangeReferenceValue(
                        new Ref<int>(() => ((PotToolItemData)item).DualWorkActor, v => { ((PotToolItemData)item).DualWorkActor = v; })
                    );

                    dualWorkParam.ChangeReferenceValue(
                        new Ref<int>(() => ((PotToolItemData)item).DualWorkParam, v => { ((PotToolItemData)item).DualWorkParam = v; })
                    );

                    //PotToolItemData
                    capacity.ChangeReferenceValue(
                        new Ref<int>(() => ((PotToolItemData)item).Capacity, v => { ((PotToolItemData)item).Capacity = v; })
                    );
                };

                StackLayoutItem[] vBoxItems =
                {
                    //ItemData
                    itemID,
                    //NotAmountItemData
                    level,
                    //SynthesisItemData
                    sourceItems,
                    //PotToolItemData
                    addedItems,
                    arrangedItems,
                    arrangeOverride,
                    baseLevel,
                    sozaiLevel,
                    dualWorkSmithBonusType,
                    dualWorkLoveLevel,
                    dualWorkActor,
                    dualWorkParam,
                    //PotToolItemData
                    capacity
                };

                var vBox = new VBox();
                foreach (var _ in vBoxItems)
                {
                    vBox.Items.Add(_);
                }

                var scroll = new Scrollable();
                scroll.Content = vBox;
                potToolItemDataPage.Content = scroll;
            }

            amountItemDataPage.Enabled = false;
            seedItemDataPage.Enabled = false;
            foodItemDataPage.Enabled = false;
            equipItemDataPage.Enabled = false;
            runeAbilityItemDataPage.Enabled = false;
            fishItemDataPage.Enabled = false;
            potToolItemDataPage.Enabled = false;

            this.tabUpdate = delegate (ItemData itemData)
            {
                switch (itemData)
                {
                    case PotToolItemData item:
                        {
                            amountItemDataPage.Enabled = false;
                            seedItemDataPage.Enabled = false;
                            foodItemDataPage.Enabled = false;
                            equipItemDataPage.Enabled = false;
                            runeAbilityItemDataPage.Enabled = false;
                            fishItemDataPage.Enabled = false;
                            potToolItemDataPage.Enabled = true;

                            this.potToolItemDataPageUpdate(item);
                            break;
                        }
                    case FishItemData item:
                        {
                            amountItemDataPage.Enabled = false;
                            seedItemDataPage.Enabled = false;
                            foodItemDataPage.Enabled = false;
                            equipItemDataPage.Enabled = false;
                            runeAbilityItemDataPage.Enabled = false;
                            fishItemDataPage.Enabled = true;
                            potToolItemDataPage.Enabled = false;

                            this.fishItemDataPageUpdate(item);
                            break;
                        }
                    case RuneAbilityItemData item:
                        {
                            amountItemDataPage.Enabled = false;
                            seedItemDataPage.Enabled = false;
                            foodItemDataPage.Enabled = false;
                            equipItemDataPage.Enabled = false;
                            runeAbilityItemDataPage.Enabled = true;
                            fishItemDataPage.Enabled = false;
                            potToolItemDataPage.Enabled = false;

                            this.runeAbilityItemDataPageUpdate(item);
                            break;
                        }
                    case EquipItemData item:
                        {
                            amountItemDataPage.Enabled = false;
                            seedItemDataPage.Enabled = false;
                            foodItemDataPage.Enabled = false;
                            equipItemDataPage.Enabled = true;
                            runeAbilityItemDataPage.Enabled = false;
                            fishItemDataPage.Enabled = false;
                            potToolItemDataPage.Enabled = false;

                            this.equipItemDataPageUpdate(item);
                            break;
                        }
                    case FoodItemData item:
                        {
                            amountItemDataPage.Enabled = false;
                            seedItemDataPage.Enabled = false;
                            foodItemDataPage.Enabled = true;
                            equipItemDataPage.Enabled = false;
                            runeAbilityItemDataPage.Enabled = false;
                            fishItemDataPage.Enabled = false;
                            potToolItemDataPage.Enabled = false;

                            this.foodItemDataPageUpdate(item);
                            break;
                        }
                    case SeedItemData item:
                        {
                            amountItemDataPage.Enabled = false;
                            seedItemDataPage.Enabled = true;
                            foodItemDataPage.Enabled = false;
                            equipItemDataPage.Enabled = false;
                            runeAbilityItemDataPage.Enabled = false;
                            fishItemDataPage.Enabled = false;
                            potToolItemDataPage.Enabled = false;

                            this.seedItemDataPageUpdate(item);
                            break;
                        }
                    case AmountItemData item:
                        {
                            amountItemDataPage.Enabled = true;
                            seedItemDataPage.Enabled = false;
                            foodItemDataPage.Enabled = false;
                            equipItemDataPage.Enabled = false;
                            runeAbilityItemDataPage.Enabled = false;
                            fishItemDataPage.Enabled = false;
                            potToolItemDataPage.Enabled = false;

                            this.amountItemDataPageUpdate(item);
                            break;
                        }
                    default:
                        {
                            amountItemDataPage.Enabled = false;
                            seedItemDataPage.Enabled = false;
                            foodItemDataPage.Enabled = false;
                            equipItemDataPage.Enabled = false;
                            runeAbilityItemDataPage.Enabled = false;
                            fishItemDataPage.Enabled = false;
                            potToolItemDataPage.Enabled = false;
                            break;
                        }
                }
            };

            tabControl.Pages.Add(amountItemDataPage);
            tabControl.Pages.Add(seedItemDataPage);
            tabControl.Pages.Add(foodItemDataPage);
            tabControl.Pages.Add(equipItemDataPage);
            tabControl.Pages.Add(runeAbilityItemDataPage);
            tabControl.Pages.Add(fishItemDataPage);
            tabControl.Pages.Add(potToolItemDataPage);

            tabControl.Width = 500;
            tabControl.Height = 500;

            this.parentHBox.Items.Add(this.list);
            this.parentHBox.Items.Add(tabControl);

            this.Content = this.parentHBox;
            
        }

        //        public void GenerateWidgets()
        //        {
        //            for (int i = 0; i < parentHBox.Items.Count; i++)
        //            {
        //                if (parentHBox.Items[i].Control.Equals(vBox))
        //                {
        //                    parentHBox.Items.RemoveAt(i);
        //                    vBox.Items.Clear();
        //                }
        //            }

        //            if (_itemStorageData != null && list.SelectedIndex >= 0 && _itemStorageData.ItemDatas[list.SelectedIndex] != null)
        //            {
        //                switch (_itemStorageData.ItemDatas[list.SelectedIndex])
        //                {
        //                    case PotToolItemData item:
        //                        {


        //                            //NotAmountItemData
        //                            var level = new SpinBox(
        //                                new Ref<int>(() => item.Level, v => { item.Level = v; }),
        //                                "Level"
        //                            );

        //                            //SynthesisItemData
        //                            var sourceItems = new GroupBox()
        //                            {
        //                                Text = "Source Items"
        //                            };

        //                            {
        //                                var sourceItemsList = new ListBox();
        //                                var sourceItemsHBox = new HBox();
        //                                var sourceItemsData = new VBox();

        //                                if (list.SelectedIndex >= 0)
        //                                {
        //                                    for (int i = 0; i < item.SourceItems.Length; i++)
        //                                    {
        //                                        sourceItemsList.Items.Add("Item {i}");
        //                                    }
        //                                }

        //                                var spinBox = new SpinBox();

        //                                sourceItemsList.SelectedIndexChanged += (object sender, EventArgs e) =>
        //                                {
        //                                    spinBox.ChangeReferenceValue(
        //                                        new Ref<int>(() => item.SourceItems[sourceItemsList.SelectedIndex], v => { item.SourceItems[sourceItemsList.SelectedIndex] = v; })
        //                                    );
        //                                };

        //                                sourceItemsData.Items.Add(spinBox);

        //                                sourceItemsHBox.Items.Add(sourceItemsList);
        //                                sourceItemsHBox.Items.Add(sourceItemsData);

        //                                sourceItems.Content = sourceItemsHBox;
        //                            }

        //                            //EquipItemData
        //                            var addedItems = new GroupBox()
        //                            {
        //                                Text = "Added Items"
        //                            };

        //                            {
        //                                var addedItemsList = new ListBox();
        //                                var addedItemsHBox = new HBox();
        //                                var addedItemsData = new VBox();

        //                                if (list.SelectedIndex >= 0)
        //                                {
        //                                    for (int i = 0; i < item.SourceItems.Length; i++)
        //                                    {
        //                                        addedItemsList.Items.Add("Item {i}");
        //                                    }
        //                                }

        //                                var spinBox = new SpinBox();

        //                                addedItemsList.SelectedIndexChanged += (object sender, EventArgs e) =>
        //                                {
        //                                    spinBox.ChangeReferenceValue(
        //                                        new Ref<int>(() => item.AddedItems[addedItemsList.SelectedIndex], v => { item.AddedItems[addedItemsList.SelectedIndex] = v; })
        //                                    );
        //                                };

        //                                addedItemsData.Items.Add(spinBox);

        //                                addedItemsHBox.Items.Add(addedItemsList);
        //                                addedItemsHBox.Items.Add(addedItemsData);

        //                                addedItems.Content = addedItemsHBox;
        //                            }

        //                            var arrangedItems = new GroupBox()
        //                            {
        //                                Text = "Arranged Items"
        //                            };

        //                            {
        //                                var arrangedItemsList = new ListBox();
        //                                var arrangedItemsHBox = new HBox();
        //                                var arrangedItemsData = new VBox();

        //                                if (list.SelectedIndex >= 0)
        //                                {
        //                                    for (int i = 0; i < item.SourceItems.Length; i++)
        //                                    {
        //                                        arrangedItemsList.Items.Add("Item {i}");
        //                                    }
        //                                }

        //                                var spinBox = new SpinBox();

        //                                arrangedItemsList.SelectedIndexChanged += (object sender, EventArgs e) =>
        //                                {
        //                                    spinBox.ChangeReferenceValue(
        //                                        new Ref<int>(() => item.ArrangeItems[arrangedItemsList.SelectedIndex], v => { item.ArrangeItems[arrangedItemsList.SelectedIndex] = v; })
        //                                    );
        //                                };

        //                                arrangedItemsData.Items.Add(spinBox);

        //                                arrangedItemsHBox.Items.Add(arrangedItemsList);
        //                                arrangedItemsHBox.Items.Add(arrangedItemsData);

        //                                arrangedItems.Content = arrangedItemsHBox;
        //                            }

        //                            var arrangeOverride = new SpinBox(
        //                                new Ref<int>(() => item.ArrangeOverride, v => { item.ArrangeOverride = v; }),
        //                                "Arrange Override"
        //                            );
        //                            var baseLevel = new SpinBox(
        //                                new Ref<int>(() => item.BaseLevel, v => { item.BaseLevel = v; }),
        //                                "Base Level"
        //                            );
        //                            var sozaiLevel = new SpinBox(
        //                                new Ref<int>(() => item.SozaiLevel, v => { item.SozaiLevel = v; }),
        //                                "Sozai Level"
        //                            );
        //                            var dualWorkSmithBonusType = new SpinBox(
        //                                new Ref<int>(() => item.DualWorkSmithBonusType, v => { item.DualWorkSmithBonusType = v; }),
        //                                "Dual Work Smith Bonus Type"
        //                            );
        //                            var dualWorkLoveLevel = new SpinBox(
        //                                new Ref<int>(() => item.DualWorkLoveLevel, v => { item.DualWorkLoveLevel = v; }),
        //                                "Dual Work Love Level"
        //                            );
        //                            var dualWorkActor = new SpinBox(
        //                                new Ref<int>(() => item.DualWorkActor, v => { item.DualWorkActor = v; }),
        //                                "Dual Work Actor"
        //                            );
        //                            var dualWorkParam = new SpinBox(
        //                                new Ref<int>(() => item.DualWorkParam, v => { item.DualWorkParam = v; }),
        //                                "Dual Work Param"
        //                            );

        //                            //PotToolItemData
        //                            var capacity = new SpinBox(
        //                                new Ref<int>(() => item.Capacity, v => { item.Capacity = v; }),
        //                                "Capacity"
        //                            );

        //                            StackLayoutItem[] vBoxItems =
        //                            {
        //                                    //ItemData
        //                                    itemID,
        //                                    //NotAmountItemData
        //                                    level,
        //                                    //SynthesisItemData
        //                                    sourceItems,
        //                                    //EquipItemData
        //                                    addedItems,
        //                                    arrangedItems,
        //                                    arrangeOverride,
        //                                    baseLevel,
        //                                    sozaiLevel,
        //                                    dualWorkSmithBonusType,
        //                                    dualWorkLoveLevel,
        //                                    dualWorkActor,
        //                                    dualWorkParam,
        //                                    //PotToolData
        //                                    capacity
        //                                };

        //                            foreach (var _ in vBoxItems)
        //                            {
        //                                vBox.Items.Add(_);
        //                            }

        //                            parentHBox.Items.Add(vBox);
        //                            break;
        //                        }
        //                    case FishItemData item:
        //                        {
        //                            //ItemData
        //                            var itemID = new SpinBox(
        //                                new Ref<int>(() => item.ItemId, v => { item.ItemId = v; }),
        //                                "Item ID"
        //                            );

        //                            //NotAmountItemData
        //                            var level = new SpinBox(
        //                                new Ref<int>(() => item.Level, v => { item.Level = v; }),
        //                                "Level"
        //                            );

        //                            //FishItemData
        //                            var size = new SpinBox(
        //                                new Ref<int>(() => item.Size, v => { item.Size = v; }),
        //                                "Size"
        //                            );

        //                            StackLayoutItem[] vBoxItems =
        //                            {
        //                                    //ItemData
        //                                    itemID,
        //                                    //NotAmountItemData
        //                                    level,
        //                                    //FishItemData
        //                                    size
        //                                };


        //                            foreach (var _ in vBoxItems)
        //                            {
        //                                vBox.Items.Add(_);
        //                            }

        //                            parentHBox.Items.Add(vBox);
        //                            break;
        //                        }
        //                    case RuneAbilityItemData item:
        //                        {
        //                            //ItemData
        //                            var itemID = new SpinBox(
        //                                new Ref<int>(() => item.ItemId, v => { item.ItemId = v; }),
        //                                "Item ID"
        //                            );

        //                            //NotAmountItemData
        //                            var level = new SpinBox(
        //                                new Ref<int>(() => item.Level, v => { item.Level = v; }),
        //                                "Level"
        //                            );

        //                            StackLayoutItem[] vBoxItems =
        //                            {
        //                                    //ItemData
        //                                    itemID,
        //                                    //NotAmountItemData
        //                                    level,
        //                                };


        //                            foreach (var _ in vBoxItems)
        //                            {
        //                                vBox.Items.Add(_);
        //                            }

        //                            parentHBox.Items.Add(vBox);
        //                            break;
        //                        }
        //                    case EquipItemData item:
        //                        {
        //                            //ItemData
        //                            var itemID = new SpinBox(
        //                                new Ref<int>(() => item.ItemId, v => { item.ItemId = v; }),
        //                                "Item ID"
        //                            );

        //                            //NotAmountItemData
        //                            var level = new SpinBox(
        //                                new Ref<int>(() => item.Level, v => { item.Level = v; }),
        //                                "Level"
        //                            );

        //                            //SynthesisItemData
        //                            var sourceItems = new GroupBox()
        //                            {
        //                                Text = "Source Items"
        //                            };

        //                            {
        //                                var sourceItemsList = new ListBox();
        //                                var sourceItemsHBox = new HBox();
        //                                var sourceItemsData = new VBox();

        //                                if (list.SelectedIndex >= 0)
        //                                {
        //                                    for (int i = 0; i < item.SourceItems.Length; i++)
        //                                    {
        //                                        sourceItemsList.Items.Add("Item {i}");
        //                                    }
        //                                }

        //                                var spinBox = new SpinBox();

        //                                sourceItemsList.SelectedIndexChanged += (object sender, EventArgs e) =>
        //                                {
        //                                    spinBox.ChangeReferenceValue(
        //                                        new Ref<int>(() => item.SourceItems[sourceItemsList.SelectedIndex], v => { item.SourceItems[sourceItemsList.SelectedIndex] = v; })
        //                                    );
        //                                };

        //                                sourceItemsData.Items.Add(spinBox);

        //                                sourceItemsHBox.Items.Add(sourceItemsList);
        //                                sourceItemsHBox.Items.Add(sourceItemsData);

        //                                sourceItems.Content = sourceItemsHBox;
        //                            }

        //                            //EquipItemData
        //                            var addedItems = new GroupBox()
        //                            {
        //                                Text = "Added Items"
        //                            };

        //                            {
        //                                var addedItemsList = new ListBox();
        //                                var addedItemsHBox = new HBox();
        //                                var addedItemsData = new VBox();

        //                                if (list.SelectedIndex >= 0)
        //                                {
        //                                    for (int i = 0; i < item.SourceItems.Length; i++)
        //                                    {
        //                                        addedItemsList.Items.Add("Item {i}");
        //                                    }
        //                                }

        //                                var spinBox = new SpinBox();

        //                                addedItemsList.SelectedIndexChanged += (object sender, EventArgs e) =>
        //                                {
        //                                    spinBox.ChangeReferenceValue(
        //                                        new Ref<int>(() => item.AddedItems[addedItemsList.SelectedIndex], v => { item.AddedItems[addedItemsList.SelectedIndex] = v; })
        //                                    );
        //                                };

        //                                addedItemsData.Items.Add(spinBox);

        //                                addedItemsHBox.Items.Add(addedItemsList);
        //                                addedItemsHBox.Items.Add(addedItemsData);

        //                                addedItems.Content = addedItemsHBox;
        //                            }

        //                            var arrangedItems = new GroupBox()
        //                            {
        //                                Text = "Arranged Items"
        //                            };

        //                            {
        //                                var arrangedItemsList = new ListBox();
        //                                var arrangedItemsHBox = new HBox();
        //                                var arrangedItemsData = new VBox();

        //                                if (list.SelectedIndex >= 0)
        //                                {
        //                                    for (int i = 0; i < item.SourceItems.Length; i++)
        //                                    {
        //                                        arrangedItemsList.Items.Add("Item {i}");
        //                                    }
        //                                }

        //                                var spinBox = new SpinBox();

        //                                arrangedItemsList.SelectedIndexChanged += (object sender, EventArgs e) =>
        //                                {
        //                                    spinBox.ChangeReferenceValue(
        //                                        new Ref<int>(() => item.ArrangeItems[arrangedItemsList.SelectedIndex], v => { item.ArrangeItems[arrangedItemsList.SelectedIndex] = v; })
        //                                    );
        //                                };

        //                                arrangedItemsData.Items.Add(spinBox);

        //                                arrangedItemsHBox.Items.Add(arrangedItemsList);
        //                                arrangedItemsHBox.Items.Add(arrangedItemsData);

        //                                arrangedItems.Content = arrangedItemsHBox;
        //                            }

        //                            var arrangeOverride = new SpinBox(
        //                                new Ref<int>(() => item.ArrangeOverride, v => { item.ArrangeOverride = v; }),
        //                                "Arrange Override"
        //                            );
        //                            var baseLevel = new SpinBox(
        //                                new Ref<int>(() => item.BaseLevel, v => { item.BaseLevel = v; }),
        //                                "Base Level"
        //                            );
        //                            var sozaiLevel = new SpinBox(
        //                                new Ref<int>(() => item.SozaiLevel, v => { item.SozaiLevel = v; }),
        //                                "Sozai Level"
        //                            );
        //                            var dualWorkSmithBonusType = new SpinBox(
        //                                new Ref<int>(() => item.DualWorkSmithBonusType, v => { item.DualWorkSmithBonusType = v; }),
        //                                "Dual Work Smith Bonus Type"
        //                            );
        //                            var dualWorkLoveLevel = new SpinBox(
        //                                new Ref<int>(() => item.DualWorkLoveLevel, v => { item.DualWorkLoveLevel = v; }),
        //                                "Dual Work Love Level"
        //                            );
        //                            var dualWorkActor = new SpinBox(
        //                                new Ref<int>(() => item.DualWorkActor, v => { item.DualWorkActor = v; }),
        //                                "Dual Work Actor"
        //                            );
        //                            var dualWorkParam = new SpinBox(
        //                                new Ref<int>(() => item.DualWorkParam, v => { item.DualWorkParam = v; }),
        //                                "Dual Work Param"
        //                            );

        //                            StackLayoutItem[] vBoxItems =
        //                            {
        //                                    //ItemData
        //                                    itemID,
        //                                    //NotAmountItemData
        //                                    level,
        //                                    //SynthesisItemData
        //                                    sourceItems,
        //                                    //EquipItemData
        //                                    addedItems,
        //                                    arrangedItems,
        //                                    arrangeOverride,
        //                                    baseLevel,
        //                                    sozaiLevel,
        //                                    dualWorkSmithBonusType,
        //                                    dualWorkLoveLevel,
        //                                    dualWorkActor,
        //                                    dualWorkParam,
        //                                };


        //                            foreach (var _ in vBoxItems)
        //                            {
        //                                vBox.Items.Add(_);
        //                            }

        //                            parentHBox.Items.Add(vBox);
        //                            break;
        //                        }
        //                    case FoodItemData item:
        //                        {
        //                            //ItemData
        //                            var itemID = new SpinBox(
        //                                new Ref<int>(() => item.ItemId, v => { item.ItemId = v; }),
        //                                "Item ID"
        //                            );

        //                            //NotAmountItemData
        //                            var level = new SpinBox(
        //                                new Ref<int>(() => item.Level, v => { item.Level = v; }),
        //                                "Level"
        //                            );

        //                            //SynthesisItemData
        //                            var sourceItems = new GroupBox()
        //                            {
        //                                Text = "Source Items"
        //                            };

        //                            {
        //                                var sourceItemsList = new ListBox();
        //                                var sourceItemsHBox = new HBox();
        //                                var sourceItemsData = new VBox();

        //                                if (list.SelectedIndex >= 0)
        //                                {
        //                                    for (int i = 0; i < item.SourceItems.Length; i++)
        //                                    {
        //                                        sourceItemsList.Items.Add("Item {i}");
        //                                    }
        //                                }

        //                                var spinBox = new SpinBox();

        //                                sourceItemsList.SelectedIndexChanged += (object sender, EventArgs e) =>
        //                                {
        //                                    spinBox.ChangeReferenceValue(
        //                                        new Ref<int>(() => item.SourceItems[sourceItemsList.SelectedIndex], v => { item.SourceItems[sourceItemsList.SelectedIndex] = v; })
        //                                    );
        //                                };

        //                                sourceItemsData.Items.Add(spinBox);

        //                                sourceItemsHBox.Items.Add(sourceItemsList);
        //                                sourceItemsHBox.Items.Add(sourceItemsData);

        //                                sourceItems.Content = sourceItemsHBox;
        //                            }

        //                            //FoodItemData
        //                            var isArange = new Widgets.CheckBox(
        //                                new Ref<bool>(() => item.IsArrange, v => { item.IsArrange = v; }),
        //                                "Arrange"
        //                             );

        //                            StackLayoutItem[] vBoxItems =
        //                            {
        //                                    //ItemData
        //                                    itemID,
        //                                    //NotAmountItemData
        //                                    level,
        //                                    //SynthesisItemData
        //                                    sourceItems,
        //                                    //FoodItemData
        //                                    isArange
        //                                };

        //                            foreach (var _ in vBoxItems)
        //                            {
        //                                vBox.Items.Add(_);
        //                            }

        //                            parentHBox.Items.Add(vBox);
        //                            break;
        //                        }
        //                    case SeedItemData item:
        //                        {
        //                            //ItemData
        //                            var itemID = new SpinBox(
        //                                new Ref<int>(() => item.ItemId, v => { item.ItemId = v; }),
        //                                "Item ID"
        //                            );

        //                            //AmountItemData
        //                            var levelAmount = new GroupBox()
        //                            {
        //                                Text = "Level Amount"
        //                            };

        //                            {
        //                                var levelAmountList = new ListBox();
        //                                var levelAmountHBox = new HBox();
        //                                var levelAmountData = new VBox();

        //                                if (list.SelectedIndex >= 0)
        //                                {
        //                                    for (int i = 0; i < item.LevelAmount.Count; i++)
        //                                    {
        //                                        levelAmountList.Items.Add($"Level Amount {i}");
        //                                    }
        //                                }

        //                                var spinBox = new SpinBox();

        //                                levelAmountList.SelectedIndexChanged += (object sender, EventArgs e) =>
        //                                {
        //                                    spinBox.ChangeReferenceValue(
        //                                        new Ref<int>(() => item.LevelAmount[levelAmountList.SelectedIndex], v => { item.LevelAmount[levelAmountList.SelectedIndex] = v; })
        //                                    );
        //                                };

        //                                levelAmountData.Items.Add(spinBox);

        //                                levelAmountHBox.Items.Add(levelAmountList);
        //                                levelAmountHBox.Items.Add(levelAmountData);

        //                                levelAmount.Content = levelAmountHBox;
        //                            }

        //                            StackLayoutItem[] vBoxItems =
        //{
        //                                    //ItemData
        //                                    itemID,
        //                                    //AmountItemData
        //                                    levelAmount,
        //                                };

        //                            foreach (var _ in vBoxItems)
        //                            {
        //                                vBox.Items.Add(_);
        //                            }

        //                            parentHBox.Items.Add(vBox);
        //                            break;
        //                        }
        //                    case AmountItemData item:
        //                        {
        //                            //ItemData
        //                            var itemID = new SpinBox(
        //                                new Ref<int>(() => item.ItemId, v => { item.ItemId = v; }),
        //                                "Item ID"
        //                            );

        //                            //AmountItemData
        //                            var levelAmount = new GroupBox()
        //                            {
        //                                Text = "Level Amount"
        //                            };

        //                            {
        //                                var levelAmountList = new ListBox();
        //                                var levelAmountHBox = new HBox();
        //                                var levelAmountData = new VBox();

        //                                if (list.SelectedIndex >= 0)
        //                                {
        //                                    for (int i = 0; i < item.LevelAmount.Count; i++)
        //                                    {
        //                                        levelAmountList.Items.Add($"Level Amount {i}");
        //                                    }
        //                                }

        //                                var spinBox = new SpinBox();

        //                                levelAmountList.SelectedIndexChanged += (object sender, EventArgs e) =>
        //                                {
        //                                    spinBox.ChangeReferenceValue(
        //                                        new Ref<int>(() => item.LevelAmount[levelAmountList.SelectedIndex], v => { item.LevelAmount[levelAmountList.SelectedIndex] = v; })
        //                                    );
        //                                };

        //                                levelAmountData.Items.Add(spinBox);

        //                                levelAmountHBox.Items.Add(levelAmountList);
        //                                levelAmountHBox.Items.Add(levelAmountData);

        //                                levelAmount.Content = levelAmountHBox;
        //                            }

        //                            StackLayoutItem[] vBoxItems =
        //{
        //                                    //ItemData
        //                                    itemID,
        //                                    //AmountItemData
        //                                    levelAmount,
        //                                };


        //                            foreach (var _ in vBoxItems)
        //                            {
        //                                vBox.Items.Add(_);
        //                            }

        //                            parentHBox.Items.Add(vBox);
        //                            break;
        //                        }
        //                }
        //            }
        //        }
    }
}
