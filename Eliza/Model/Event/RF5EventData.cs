﻿using Eliza.Core.Serialization;

namespace Eliza.Model.Event
{
    public class RF5EventData
    {
        public EventSaveParameter EventSaveParameter;
        public SaveFlagStorage SaveFlag;
        public SubEventSaveData SubEventSaveDatas;
        [ElizaFlowControl(Locale=SaveData.LOCALE.JP, FromVersion=7)]
        public SaveScenarioSupport ScenarioSupport;
        public int MainScenarioStep;
        public int[] PresentSendActorList;
        public int[] PresentRecvActorList;
        public bool IsStartFishing;
        public int[] FesJoinActorList;
        public int[] FesVisitorActorList;
        public FesNpcScore[] FesNpcScoreList;
        public int IsCalcFesId;
        public int VictoryCandidateNpcId;
        public int JudgeChildId;
        public int[] FishTypeList;
    }
}
