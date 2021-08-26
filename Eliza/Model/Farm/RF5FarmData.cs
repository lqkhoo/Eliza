using System.Collections.Generic;
using static Eliza.Core.Serialization.ElizaFlowControlAttribute;
using Eliza.Model;

namespace Eliza.Model.Farm
{
    public class RF5FarmData
    {
        [ElizaList(IsMessagePackList = true)]
        public bool[] FirstVisitFarm;

        [ElizaList(IsMessagePackList = true)]
        public MessagePackInt[] FarmSizeLevels;

        [ElizaList(IsMessagePackList = true)]
        public FarmCropData[] FarmCropDatas;

        [ElizaList(IsMessagePackList = true)]
        public MessagePackInt[] CrystalUseCounts;

        [ElizaList(IsMessagePackList = true)]
        public RF4_CROP_STATE[] Crop;

        [ElizaList(IsMessagePackList = true)]
        public RF4_SOIL_INFO[] Soil;

        [ElizaList(IsMessagePackList = true)]
        public bool[] MonsterHutReleaseFlags;

        [ElizaList(IsMessagePackList = true)]
        public MessagePackInt[] HarvestCounts;

        [ElizaList(IsMessagePackList = true)]
        public MessagePackInt[] ItemHarvestIdList;

        [ElizaList(IsMessagePackList = true)]
        public MonsterHutSaveData[] MonsterHutSaveDatas;
    }
}
