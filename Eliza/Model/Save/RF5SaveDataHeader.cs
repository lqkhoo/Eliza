using Eliza.Core.Serialization;

namespace Eliza.Model.Save
{
    public class RF5SaveDataHeader
    {
        public ulong uid;
        public uint version;
        [ElizaList(FixedSize = 4)]
        public char[] project;
        public uint wCnt;
        public uint wOpt;
        public SaveTime saveTime;
    }
}
