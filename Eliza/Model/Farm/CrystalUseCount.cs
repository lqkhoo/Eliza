using MessagePack;
using Eliza.Core.Serialization.MessagePackFormatters;

namespace Eliza.Model.Farm
{
    [MessagePackObject]
    [MessagePackFormatter(typeof(CrystalUseCountFormatter))]
    public class CrystalUseCount
    {
        [Key(0)]
        public int Count;
    }
}
