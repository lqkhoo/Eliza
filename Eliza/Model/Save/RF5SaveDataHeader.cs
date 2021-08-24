using static Eliza.Core.Serialization.ElizaAttribute;

namespace Eliza.Model.Save
{
    public class RF5SaveDataHeader
    {
        public ulong uid;
        public uint version;
        [ElizaSize(Fixed = 4)]
        public char[] project;
        public uint wCnt;
        public uint wOpt;
        public SaveTime saveTime;
    }
}
