using MessagePack;

namespace Eliza.Model.Item
{
    [MessagePackObject]
    public class FoodItemData : SynthesisItemData
    {
        [Key(3)]
        public bool IsArrange;

        public FoodItemData()
        {
            this.SourceItems = new int[0];
        }
    }
}
