using MessagePack;

namespace Eliza.Model.Item
{
    [MessagePackObject]
    public class FieldOnGroundItemStorage
    {
        [Key(0)]
        public FieldOnGroundItemInfo[] Datas;
    }
}
