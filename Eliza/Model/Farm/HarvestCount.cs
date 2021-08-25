using MessagePack;
using Eliza.Core.Serialization.MessagePackFormatters;


namespace Eliza.Model.Farm
{
    [MessagePackObject]
    [MessagePackFormatter(typeof(HarvestCountFormatter))]
    public class HarvestCount
    {
        [Key(0)]
        public int Count;
    }
}
