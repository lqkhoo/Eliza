using MessagePack;

namespace Eliza.Model.Shipping
{
    [MessagePackObject]
    public class SeedLevelRecords
    {
        [Key(0)]
        public int ItemId;
        [Key(1)]
        public int maxLv;
    }
}
