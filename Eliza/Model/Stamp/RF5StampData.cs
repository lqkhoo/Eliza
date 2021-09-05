using Eliza.Core.Serialization;

namespace Eliza.Model.Stamp
{
    public class RF5StampData
    {
        // Added to ensure correct re-serialization. Sometimes the game records the
        // size of the list as zero for some reason. Instead of making this a dynamic
        // array, we treat StampRecordData as a fixed array (which it is) and store
        // the length in this int field, and then when it comes time to re-serialize,
        // just spit that value back out.
        public int _NumRecords;
        // [ElizaList(MaxSize = 80)]
        [ElizaList(FixedSize=80)]
        public StampRecordData[] StampRecordData;
    }
}
