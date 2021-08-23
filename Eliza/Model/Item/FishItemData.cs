using MessagePack;

namespace Eliza.Model.Item
{
    [MessagePackObject]
    public class FishItemData : NotAmountItemData
    {
        [Key(2)]
        public int Size;
    }
}
