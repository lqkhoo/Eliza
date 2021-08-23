using MessagePack;

namespace Eliza.Model.Farm
{
    [MessagePackObject]
    public class KeyAndValue<TKey, TValue>
    {
        [Key(0)]
        public TKey Key;
        [Key(1)]
        public TValue Value;
    }
}
