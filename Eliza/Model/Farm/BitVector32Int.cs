using MessagePack;

namespace Eliza.Model.Farm
{
    [MessagePackObject]
    public class BitVector32Int
    {
        [Key(0)]
        public uint data;
    }
}
