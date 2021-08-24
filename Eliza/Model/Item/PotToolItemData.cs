using MessagePack;

namespace Eliza.Model.Item
{
    [MessagePackObject]
    public class PotToolItemData : EquipItemData
    {
        [Key(12)]
        public int Capacity;

        public PotToolItemData()
        {
            this.AddedItems = new int[0];
            this.ArrangeItems = new int[0];
            this.SourceItems = new int[0];
        }
    }
}
