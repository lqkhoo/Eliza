using static Eliza.Core.Serialization.ElizaAttribute;

namespace Eliza.Model.Stamp
{
    public class StampRecordData
    {
        // For an uninitialized stamp, the fields are initialized as:

        [ElizaValue(Undefined=new byte[4] { 0x00,0x00,0x00,0x00 })]
        public int StampLevel;

        [ElizaValue(Undefined = new byte[4] { 0x00,0x00,0x00,0x00 })]
        public float MaxRecord;

        [ElizaValue(Undefined = new byte[4] { 0xFF,0xFF,0xFF,0xFF })]
        public float MinRecord;
    }
}
