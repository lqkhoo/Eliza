using MessagePack;
using System.Collections.Generic;

namespace Eliza.Model.Item
{
    [MessagePackObject]
    public class AmountItemData : ItemData
    {
        [Key(1)]
        public List<int> LevelAmount;

        public AmountItemData()
        {
            this.LevelAmount = new List<int>();
        }
    }
}
