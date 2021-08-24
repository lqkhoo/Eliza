using static Eliza.Core.Serialization.ElizaAttribute;

namespace Eliza.Model.Name
{
    public class RF5NameData
    {
        [ElizaSize(Max = 32)]
        public string Name_Farm_Soil;
        [ElizaSize(Max = 32)]
        public string Name_Farm_Fire;
        [ElizaSize(Max = 32)]
        public string Name_Farm_Ice;
        [ElizaSize(Max = 32)]
        public string Name_Farm_Wind;
        [ElizaSize(Max = 32)]
        public string Name_Farm_Ground;
        [ElizaSize(Max = 32)]
        public string Name_FarmPet_Soil_A;
        [ElizaSize(Max = 32)]
        public string Name_FarmPet_Soil_B;
        [ElizaSize(Max = 32)]
        public string Name_FarmPet_Fire_A;
        [ElizaSize(Max = 32)]
        public string Name_FarmPet_Fire_B;
        [ElizaSize(Max = 32)]
        public string Name_FarmPet_Ice_A;
        [ElizaSize(Max = 32)]
        public string Name_FarmPet_Ice_B;
        [ElizaSize(Max = 32)]
        public string Name_FarmPet_Wind_A;
        [ElizaSize(Max = 32)]
        public string Name_FarmPet_Wind_B;
        [ElizaSize(Max = 32)]
        public string Name_FarmPet_Ground_A;
        [ElizaSize(Max = 32)]
        public string Name_FarmPet_Ground_B;
    }
}