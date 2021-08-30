using MessagePack;

namespace Eliza.Model.Item
{
    [MessagePackObject]
    public abstract class NotAmountItemData : ItemData
    {
        [Key(1)]
        public int Level;
        [IgnoreMember]
        #pragma warning disable IDE0051 // Remove unused private members
        private bool Poped;
        #pragma warning restore IDE0051 // Remove unused private members
    }
}
