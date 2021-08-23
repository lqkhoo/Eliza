using MessagePack;

namespace Eliza.Model.Item
{
    [MessagePackObject]
    public class ItemStorageData
    {
        [Key(0)]
        public ItemData[] ItemDatas;
    }
}
