using System.Collections.Generic;
using Eliza.Core.Serialization;

namespace Eliza.Model.Status
{
    public class RF5StatusData
    {
        [ElizaFlowControl(Locale=SaveData.LOCALE.JP, FromVersion = 4)]
        public MessagePackInt Unk;
        public Dictionary<int, HumanStatusData> HumanStatusDatas;
        public List<FriendMonsterStatusData> FriendMonsterStatusDatas;
    }
}
