using System.Collections.Generic;
using static Eliza.Core.Serialization.ElizaAttribute;

namespace Eliza.Model.Farm
{
    public class RF5FarmData
    {
        [ElizaMessagePackList]
        public bool[] FirstVisitFarm;
        [ElizaMessagePackList]
        public FarmSizeLevel[] FarmSizeLevels;
        [ElizaMessagePackList]
        public FarmCropData[] FarmCropDatas;
        [ElizaMessagePackList]
        public int[] CrystalUseCounts;
        [ElizaMessagePackList]
        public RF4_CROP_STATE[] Crop;
        [ElizaMessagePackList]
        public RF4_SOIL_INFO[] Soil;
        [ElizaMessagePackList]
        public bool[] MonsterHutReleaseFlags;
        [ElizaMessagePackList]
        public int[] HarvestCount;
        [ElizaMessagePackList]
        public List<int> ItemHarvestIdList;
        [ElizaMessagePackList]
        public MonsterHutSaveData[] MonsterHutSaveDatas;
    }
}
