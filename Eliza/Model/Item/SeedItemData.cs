using MessagePack;
using System.Collections.Generic;

namespace Eliza.Model.Item
{
    [MessagePackObject]
    public class SeedItemData : AmountItemData
    {
        public SeedItemData()
        {
            this.LevelAmount = new List<int>();
        }
    }
}
