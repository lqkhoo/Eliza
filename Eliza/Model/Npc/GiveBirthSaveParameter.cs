using MessagePack;

namespace Eliza.Model.Npc
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class GiveBirthSaveParameter
    {
        public int Targetdays;
        public int NowType;
    }
}
