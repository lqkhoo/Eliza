using Eliza.Core.Serialization;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eliza.Avalonia.ViewModels
{
    public class ItemDataEditorViewModel : ViewModelBase
    {

        public const int InputMax = Int32.MaxValue;
        public const int InputMin = -1;

        protected UiObjectGraph OriginalContext;

        // Break everything down. We'll leverage the serializers to
        // reassemble a UiObjectNode afterwards.

        protected int _ItemId = 0; // Key 0 ItemData
        protected int _Level = 0;  // Key 1 NotAmountItemData : ItemData

        // Remember to serlialize this back into int List<int>
        protected int[] _LevelAmount = new int[9];  // Key 1 AmountItemData
        protected int[] _SourceItems = new int[6];  // Key 2 SynthesisItemData : NotAmountItemData
        protected int[] _AddedItems = new int[9];   // Key 3 EquipItemData
        protected bool _IsArrange = false;          // Key 3 FoodItemData : SynthesisItemData
        protected int[] _ArrangeItems = new int[3]; // Key 4
        protected int _ArrangeOverride = 0;         // Key 5
        protected int _BaseLevel = 0;               // Key 6
        protected int _SozaiLevel = 0;              // Key 7
        protected int _DualWorkSmithBonusType = -1; // Key 8
        protected int _DualWorkLoveLevel = -1;      // Key 9
        protected int _DualWorkActor = -1;          // Key 10
        protected int _DualWorkParam = -1;          // Key 11
        protected int _Capacity = 0;                // Key 12
        protected int _Size = 0;                    // Key 2 FishItemData : NotAmountItemData

        protected Dictionary<string, Action<UiObjectGraph>> MethodDispatcher = new() { };

        public ItemDataEditorViewModel() {
            this.Init();
        }

        public ItemDataEditorViewModel(UiObjectGraph context)
        {
            this.Init();
            this.LoadContext(context);
            var foo = 3;
        }

        public void Init()
        {
            this.MethodDispatcher.Add("ItemID", (x) => this.UnwrapItemId(x));
            this.MethodDispatcher.Add("Level", (x) => this.UnwrapLevel(x));
            this.MethodDispatcher.Add("LevelAmount", (x) => this.UnwrapLevelAmount(x));
            this.MethodDispatcher.Add("SourceItems", (x) => this.UnwrapSourceItems(x));
            this.MethodDispatcher.Add("IsArrange", (x) => this.UnwrapIsArrange(x));
            this.MethodDispatcher.Add("AddedItems", (x) => this.UnwrapAddedItems(x));
            this.MethodDispatcher.Add("ArrangeItems", (x) => this.UnwrapArrangeItems(x));
            this.MethodDispatcher.Add("BaseLevel", (x) => this.UnwrapBaseLevel(x));
            this.MethodDispatcher.Add("SozaiLevel", (x) => this.UnwrapSozaiLevel(x));
            this.MethodDispatcher.Add("DualWorkSmithBonusType", (x) => this.UnwrapDualWorkSmithBonusType(x));
            this.MethodDispatcher.Add("DualWorkLoveLevel", (x) => this.UnwrapDualWorkLoveLevel(x));
            this.MethodDispatcher.Add("DualWorkActor", (x) => this.UnwrapDualWorkActor(x));
            this.MethodDispatcher.Add("DualWorkParam", (x) => this.UnwrapDualWorkParam(x));
            this.MethodDispatcher.Add("Capacity", (x) => this.UnwrapCapacity(x));
            this.MethodDispatcher.Add("Size", (x) => this.UnwrapSize(x));
        }


        public void LoadContext(UiObjectGraph context)
        {
            this.OriginalContext = context;
            foreach (UiObjectGraph child in context.Children) {
                int foo = 3;
                // I loathe to key by string of fieldname, but what else could we do?
                if(child.FieldInfo != null && child.Value != null) {
                    string fieldName = child.FieldInfo.Name;
                    if(this.MethodDispatcher.ContainsKey(fieldName)) {
                        this.MethodDispatcher[fieldName](child);
                    }
                }
            }
        }

        protected int ReadIntHelper(UiObjectGraph uiNode)
        {
            int val;
            if(uiNode.Value != null) {
                val = (int)uiNode.Value;
            } else {
                val = 0;
            }
            return val;
        }

        protected void UnwrapItemId(UiObjectGraph x)  { this.ItemId = this.ReadIntHelper(x); }

        protected void UnwrapLevel(UiObjectGraph x) { this.Level = this.ReadIntHelper(x); }

        protected void UnwrapLevelAmount(UiObjectGraph x)
        {
            if (x.Value != null) {
                int arrayLength = ((List<int>)x.Value).Count;
                for (int idx = 0; idx < arrayLength; idx++) {
                    UiObjectGraph child = x.Children[idx];
                    if (child.Value != null) {
                        this._LevelAmount[idx] = (int)child.Value;
                    } else {
                        this._LevelAmount[idx] = 0;
                    }
                }
            }
        }

        protected void UnwrapSourceItems(UiObjectGraph x)
        {
            if (x.Value != null) {
                int arrayLength = ((int[])x.Value).Length;
                for (int idx = 0; idx < arrayLength; idx++) {
                    UiObjectGraph child = x.Children[idx];
                    if (child.Value != null) {
                        this._SourceItems[idx] = (int)child.Value;
                    } else {
                        this._SourceItems[idx] = 0;
                    }
                }
            }
        }

        protected void UnwrapIsArrange(UiObjectGraph x)
        {
            bool val;
            if(x.Value != null) {
                val = (bool)x.Value;
            } else {
                val = false;
            }
            this.IsArrange = val;
        }

        protected void UnwrapAddedItems(UiObjectGraph x)
        {
            if (x.Value != null) {
                int arrayLength = ((int[])x.Value).Length;
                for (int idx = 0; idx < arrayLength; idx++) {
                    UiObjectGraph child = x.Children[idx];
                    if (child.Value != null) {
                        this._AddedItems[idx] = (int)child.Value;
                    } else {
                        this._AddedItems[idx] = 0;
                    }
                }
            }
        }

        protected void UnwrapArrangeItems(UiObjectGraph x)
        {
            if (x.Value != null) {
                int arrayLength = ((int[])x.Value).Length;
                for (int idx = 0; idx < arrayLength; idx++) {
                    UiObjectGraph child = x.Children[idx];
                    if (child.Value != null) {
                        this._ArrangeItems[idx] = (int)child.Value;
                    } else {
                        this._ArrangeItems[idx] = 0;
                    }
                }
            }
        }

        protected void UnwrapArrangeOverride(UiObjectGraph x) { this.ArrangeOverride = this.ReadIntHelper(x); }

        protected void UnwrapBaseLevel(UiObjectGraph x) { this.BaseLevel = this.ReadIntHelper(x); }

        protected void UnwrapSozaiLevel(UiObjectGraph x) { this.SozaiLevel = this.ReadIntHelper(x); }

        protected void UnwrapDualWorkSmithBonusType(UiObjectGraph x) { this.DualWorkSmithBonusType = this.ReadIntHelper(x); }

        protected void UnwrapDualWorkLoveLevel(UiObjectGraph x) { this.DualWorkLoveLevel = this.ReadIntHelper(x); }

        protected void UnwrapDualWorkActor(UiObjectGraph x) { this.DualWorkActor = this.ReadIntHelper(x); }

        protected void UnwrapDualWorkParam(UiObjectGraph x) { this.DualWorkParam = this.ReadIntHelper(x); }

        protected void UnwrapCapacity(UiObjectGraph x) { this.Capacity = this.ReadIntHelper(x); }

        protected void UnwrapSize(UiObjectGraph x) { this.Size = this.ReadIntHelper(x); }





        /*
        protected int _LevelAmount0 = 0;
        protected int _LevelAmount1 = 0;
        protected int _LevelAmount2 = 0;
        protected int _LevelAmount3 = 0;
        protected int _LevelAmount4 = 0;
        protected int _LevelAmount5 = 0;
        protected int _LevelAmount6 = 0;
        protected int _LevelAmount7 = 0;
        protected int _LevelAmount8 = 0;
        protected int _SourceItem0 = 0;
        protected int _SourceItem1 = 0;
        protected int _SourceItem2 = 0;
        protected int _SourceItem3 = 0;
        protected int _SourceItem4 = 0;
        protected int _SourceItem5 = 0;
        protected int _AddedItems0 = 0;
        protected int _AddedItems1 = 0;
        protected int _AddedItems2 = 0;
        protected int _AddedItems3 = 0;
        protected int _AddedItems4 = 0;
        protected int _AddedItems5 = 0;
        protected int _AddedItems6 = 0;
        protected int _AddedItems7 = 0;
        protected int _AddedItems8 = 0;
        protected int _ArrangeItems0 = 0;
        protected int _ArrangeItems1 = 0;
        protected int _ArrangeItems2 = 0;
        */


        public int ItemId
        {
            get => this._ItemId;
            set => this.RaiseAndSetIfChanged(ref this._ItemId, value);
        }
        public int Level
        {
            get => this._Level;
            set => this.RaiseAndSetIfChanged(ref this._Level, value);
        }

        public int LevelAmount0
        {
            get => this._LevelAmount[0];
            set => this.RaiseAndSetIfChanged(ref this._LevelAmount[0], value);
        }
        public int LevelAmount1
        {
            get => this._LevelAmount[1];
            set => this.RaiseAndSetIfChanged(ref this._LevelAmount[1], value);
        }
        public int LevelAmount2
        {
            get => this._LevelAmount[2];
            set => this.RaiseAndSetIfChanged(ref this._LevelAmount[2], value);
        }
        public int LevelAmount3
        {
            get => this._LevelAmount[3];
            set => this.RaiseAndSetIfChanged(ref this._LevelAmount[3], value);
        }
        public int LevelAmount4
        {
            get => this._LevelAmount[4];
            set => this.RaiseAndSetIfChanged(ref this._LevelAmount[4], value);
        }
        public int LevelAmount5
        {
            get => this._LevelAmount[5];
            set => this.RaiseAndSetIfChanged(ref this._LevelAmount[5], value);
        }
        public int LevelAmount6
        {
            get => this._LevelAmount[6];
            set => this.RaiseAndSetIfChanged(ref this._LevelAmount[6], value);
        }
        public int LevelAmount7
        {
            get => this._LevelAmount[7];
            set => this.RaiseAndSetIfChanged(ref this._LevelAmount[7], value);
        }
        public int LevelAmount8
        {
            get => this._LevelAmount[8];
            set => this.RaiseAndSetIfChanged(ref this._LevelAmount[8], value);
        }

        public int SourceItems0
        {
            get => this._SourceItems[0];
            set => this.RaiseAndSetIfChanged(ref this._SourceItems[0], value);
        }
        public int SourceItems1
        {
            get => this._SourceItems[1];
            set => this.RaiseAndSetIfChanged(ref this._SourceItems[1], value);
        }
        public int SourceItems2
        {
            get => this._SourceItems[2];
            set => this.RaiseAndSetIfChanged(ref this._SourceItems[2], value);
        }
        public int SourceItems3
        {
            get => this._SourceItems[3];
            set => this.RaiseAndSetIfChanged(ref this._SourceItems[3], value);
        }
        public int SourceItems4
        {
            get => this._SourceItems[4];
            set => this.RaiseAndSetIfChanged(ref this._SourceItems[4], value);
        }
        public int SourceItems5
        {
            get => this._SourceItems[5];
            set => this.RaiseAndSetIfChanged(ref this._SourceItems[5], value);
        }

        public int AddedItems0
        {
            get => this._AddedItems[0];
            set => this.RaiseAndSetIfChanged(ref this._AddedItems[0], value);
        }

        public int AddedItems1
        {
            get => this._AddedItems[1];
            set => this.RaiseAndSetIfChanged(ref this._AddedItems[1], value);
        }
        public int AddedItems2
        {
            get => this._AddedItems[2];
            set => this.RaiseAndSetIfChanged(ref this._AddedItems[2], value);
        }
        public int AddedItems3
        {
            get => this._AddedItems[3];
            set => this.RaiseAndSetIfChanged(ref this._AddedItems[3], value);
        }
        public int AddedItems4
        {
            get => this._AddedItems[4];
            set => this.RaiseAndSetIfChanged(ref this._AddedItems[4], value);
        }
        public int AddedItems5
        {
            get => this._AddedItems[5];
            set => this.RaiseAndSetIfChanged(ref this._AddedItems[5], value);
        }
        public int AddedItems6
        {
            get => this._AddedItems[6];
            set => this.RaiseAndSetIfChanged(ref this._AddedItems[6], value);
        }
        public int AddedItems7
        {
            get => this._AddedItems[7];
            set => this.RaiseAndSetIfChanged(ref this._AddedItems[7], value);
        }
        public int AddedItems8
        {
            get => this._AddedItems[8];
            set => this.RaiseAndSetIfChanged(ref this._AddedItems[8], value);
        }

        public bool IsArrange
        {
            get => this._IsArrange;
            set => this.RaiseAndSetIfChanged(ref this._IsArrange, value);
        }


        public int ArrangeItems0
        {
            get => this._ArrangeItems[0];
            set => this.RaiseAndSetIfChanged(ref this._ArrangeItems[0], value);
        }
        public int ArrangeItems1
        {
            get => this._ArrangeItems[1];
            set => this.RaiseAndSetIfChanged(ref this._ArrangeItems[1], value);
        }
        public int ArrangeItems2
        {
            get => this._ArrangeItems[2];
            set => this.RaiseAndSetIfChanged(ref this._ArrangeItems[2], value);
        }

        public int ArrangeOverride
        {
            get => this._ArrangeOverride;
            set => this.RaiseAndSetIfChanged(ref this._ArrangeOverride, value);
        }
        public int BaseLevel
        {
            get => this._BaseLevel;
            set => this.RaiseAndSetIfChanged(ref this._BaseLevel, value);
        }
        public int SozaiLevel
        {
            get => this._SozaiLevel;
            set => this.RaiseAndSetIfChanged(ref this._SozaiLevel, value);
        }
        public int DualWorkSmithBonusType
        {
            get => this._DualWorkSmithBonusType;
            set => this.RaiseAndSetIfChanged(ref this._DualWorkSmithBonusType, value);
        }
        public int DualWorkLoveLevel
        {
            get => this._DualWorkLoveLevel;
            set => this.RaiseAndSetIfChanged(ref this._DualWorkLoveLevel, value);
        }
        public int DualWorkActor
        {
            get => this._DualWorkActor;
            set => this.RaiseAndSetIfChanged(ref this._DualWorkActor, value);
        }
        public int DualWorkParam
        {
            get => this._DualWorkParam;
            set => this.RaiseAndSetIfChanged(ref this._DualWorkParam, value);
        }
        public int Capacity
        {
            get => this._Capacity;
            set => this.RaiseAndSetIfChanged(ref this._Capacity, value);
        }
        public int Size
        {
            get => this._Size;
            set => this.RaiseAndSetIfChanged(ref this._Size, value);
        }

    }
}
