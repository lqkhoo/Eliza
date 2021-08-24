using static Eliza.Core.Serialization.ElizaAttribute;

namespace Eliza.Model.Stamp
{
    public class RF5StampData
    {
        [ElizaSize(Max = 80)]
        public StampRecordData[] StampRecordData;
    }
}
