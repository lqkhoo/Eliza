using MessagePack;
using System.Collections.Generic;

namespace Eliza.Model.Item
{
    [MessagePackObject]
    public class EquipItemData : SynthesisItemData
    {
        [Key(3)]
        public int[] AddedItems;
        [Key(4)]
        public int[] ArrangeItems;
        [Key(5)]
        public int ArrangeOverride;
        [Key(6)]
        public int BaseLevel;
        [Key(7)]
        public int SozaiLevel;
        [Key(8)]
        public int DualWorkSmithBonusType;
        [Key(9)]
        public int DualWorkLoveLevel;
        [Key(10)]
        public int DualWorkActor;
        [Key(11)]
        public int DualWorkParam;
        [IgnoreMember]
        #pragma warning disable IDE0051 // Remove unused private members
        private static Dictionary<int, int> SameItemNum;
        #pragma warning restore IDE0051 // Remove unused private members

        public EquipItemData()
        {
            this.AddedItems = new int[0];
            this.ArrangeItems = new int[0];
            this.SourceItems = new int[0];
        }
    }
}