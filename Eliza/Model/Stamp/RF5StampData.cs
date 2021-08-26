using static Eliza.Core.Serialization.ElizaFlowControlAttribute;

namespace Eliza.Model.Stamp
{
    public class RF5StampData
    {
        [ElizaList(MaxSize = 80)]
        public StampRecordData[] StampRecordData;
    }
}
