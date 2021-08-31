using MessagePack;
using Eliza.Core.Serialization.MessagePackFormatters;

namespace Eliza.Model.Item
{

    // The union works. However, if we want byte-level
    // reproduction, RF5 serializes the union keys i.e. 1,2,3
    // as fixed-width 4-byte int32. The MessagePack C#
    // implementation cannot do that.

    // [Union(0, typeof(AmountItemData))]
    // [Union(1, typeof(SeedItemData))]
    // [Union(2, typeof(FoodItemData))]
    // [Union(3, typeof(EquipItemData))]
    // [Union(4, typeof(RuneAbilityItemData))]
    // [Union(5, typeof(FishItemData))]
    // [Union(6, typeof(PotToolItemData))]
    [MessagePackObject]
    [MessagePackFormatter(typeof(ItemDataFormatter))]
    public abstract class ItemData
    {
        [Key(0)]
        public int ItemId; // Original: ItemID
    }
}
