using MessagePack;

namespace Eliza.Model.Battle
{
    [MessagePackObject]
    public class EnemyStatusData
    {
        [Key(0)]
        public int MonsterDataID;
        [Key(1)]
        public int PartnerMovementOrderType;
    }
}
