using System.Collections.Generic;
using static Eliza.Core.Serialization.ElizaAttribute;

namespace Eliza.Model.Status
{
    public class RF5StatusData
    {
        public MessagePackInt Unk;
        public Dictionary<int, HumanStatusData> HumanStatusDatas;
        public List<FriendMonsterStatusData> FriendMonsterStatusDatas;
    }


    public class RF5StatusDataV102
    {
        // No Message pack in 102
        public Dictionary<int, HumanStatusData> HumanStatusDatas;
        public List<FriendMonsterStatusData> FriendMonsterStatusDatas;

        public RF5StatusData AdaptTo()
        {
            RF5StatusData rf5StatusData = new()
            {
                HumanStatusDatas = this.HumanStatusDatas,
                FriendMonsterStatusDatas = this.FriendMonsterStatusDatas
            };
            return rf5StatusData;
        }

        public RF5StatusDataV102 AdaptFrom(RF5StatusData rf5StatusData)
        {
            this.HumanStatusDatas = rf5StatusData.HumanStatusDatas;
            this.FriendMonsterStatusDatas = rf5StatusData.FriendMonsterStatusDatas;
            return this;
        }
    }
}
