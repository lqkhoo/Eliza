using static Eliza.Core.Serialization.Attributes;

namespace Eliza.Model.Stamp
{
    public class RF5StampData
    {
        [Length(Max = 80)]
        public StampRecordData[] StampRecordData;
    }
}
