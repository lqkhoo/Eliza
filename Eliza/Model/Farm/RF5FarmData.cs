using System.Collections.Generic;
using static Eliza.Core.Serialization.ElizaAttribute;
using Eliza.Model;

namespace Eliza.Model.Farm
{
    public class RF5FarmData
    {
        [ElizaMessagePackList]
        public bool[] FirstVisitFarm;
        [ElizaMessagePackList]
        public MessagePackInt[] FarmSizeLevels;
        [ElizaMessagePackList]
        public FarmCropData[] FarmCropDatas;
        [ElizaMessagePackList]
        public MessagePackInt[] CrystalUseCounts;
        [ElizaMessagePackList]
        public RF4_CROP_STATE[] Crop;
        [ElizaMessagePackList]
        public RF4_SOIL_INFO[] Soil;
        [ElizaMessagePackList]
        public bool[] MonsterHutReleaseFlags;
        [ElizaMessagePackList]
        public MessagePackInt[] HarvestCounts;
        [ElizaMessagePackList]
        public MessagePackInt[] ItemHarvestIdList;
        [ElizaMessagePackList]
        public MonsterHutSaveData[] MonsterHutSaveDatas;
    }
}
