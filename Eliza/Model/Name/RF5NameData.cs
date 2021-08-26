using Eliza.Core.Serialization;
namespace Eliza.Model.Name
{
    public class RF5NameData
    {
        [ElizaString(MaxSize = 32)]
        public string Name_Farm_Soil;
        [ElizaString(MaxSize = 32)]
        public string Name_Farm_Fire;
        [ElizaString(MaxSize = 32)]
        public string Name_Farm_Ice;
        [ElizaString(MaxSize = 32)]
        public string Name_Farm_Wind;
        [ElizaString(MaxSize = 32)]
        public string Name_Farm_Ground;
        [ElizaString(MaxSize = 32)]
        public string Name_FarmPet_Soil_A;
        [ElizaString(MaxSize = 32)]
        public string Name_FarmPet_Soil_B;
        [ElizaString(MaxSize = 32)]
        public string Name_FarmPet_Fire_A;
        [ElizaString(MaxSize = 32)]
        public string Name_FarmPet_Fire_B;
        [ElizaString(MaxSize = 32)]
        public string Name_FarmPet_Ice_A;
        [ElizaString(MaxSize = 32)]
        public string Name_FarmPet_Ice_B;
        [ElizaString(MaxSize = 32)]
        public string Name_FarmPet_Wind_A;
        [ElizaString(MaxSize = 32)]
        public string Name_FarmPet_Wind_B;
        [ElizaString(MaxSize = 32)]
        public string Name_FarmPet_Ground_A;
        [ElizaString(MaxSize = 32)]
        public string Name_FarmPet_Ground_B;
    }
}