using static Eliza.Core.Serialization.ElizaFlowControlAttribute;

namespace Eliza.Model.Player
{
    public class RF5PlayerData
    {
        public int PlayerGold;
        [ElizaString(MaxSize = 32)]
        public string PlayerName;
        public bool IsPlayerMale;
        public bool IsPlayerModelMale;
        public int VariationNo;
        public int PlayerBirthday;
        public int MarriedNPCID;
        public int SeedPoint;
        public int PoliceRank;
        public int Stone;
        public int Lumber;
        public int Compost;
        public int Esa;
        public int DailyRecipePan_Bakery;
        public int DailyRecipePan_Restaurant;
        public int BathroomBlocked;
        public SkillData[] SkillDatas;
        public int DualSmithActor;
        public int DualCookActor;
        public int DualFishingActor;
    }
}
