namespace Eliza.Model.Event
{
    public class RF5EventData
    {

        public EventSaveParameter EventSaveParameter;
        public SaveFlagStorage SaveFlag;
        public SubEventSaveData SubEventSaveDatas;
        // Add this to the UI
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

    public class RF5EventDataV106
    {
        public EventSaveParameter EventSaveParameter;
        public SaveFlagStorage SaveFlag;
        public SubEventSaveData SubEventSaveDatas;
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

        public RF5EventData AdaptTo()
        {
            RF5EventData eventData = new()
            {
                EventSaveParameter = this.EventSaveParameter,
                SaveFlag = this.SaveFlag,
                SubEventSaveDatas = this.SubEventSaveDatas,
                ScenarioSupport = new SaveScenarioSupport(),
                MainScenarioStep = this.MainScenarioStep,
                PresentSendActorList = this.PresentSendActorList,
                PresentRecvActorList = this.PresentRecvActorList,
                IsStartFishing = this.IsStartFishing,
                FesJoinActorList = this.FesJoinActorList,
                FesVisitorActorList = this.FesVisitorActorList,
                FesNpcScoreList = this.FesNpcScoreList,
                IsCalcFesId = this.IsCalcFesId,
                VictoryCandidateNpcId = this.VictoryCandidateNpcId,
                JudgeChildId = this.JudgeChildId,
                FishTypeList = this.FishTypeList
            };
            return eventData;
        }

        public RF5EventDataV106 AdaptFrom(RF5EventData eventData)
        {
            this.EventSaveParameter = eventData.EventSaveParameter;
            this.SaveFlag = eventData.SaveFlag;
            this.SubEventSaveDatas = eventData.SubEventSaveDatas;
            this.MainScenarioStep = eventData.MainScenarioStep;
            this.PresentSendActorList = eventData.PresentSendActorList;
            this.PresentRecvActorList = eventData.PresentRecvActorList;
            this.IsStartFishing = eventData.IsStartFishing;
            this.FesJoinActorList = eventData.FesJoinActorList;
            this.FesVisitorActorList = eventData.FesVisitorActorList;
            this.FesNpcScoreList = eventData.FesNpcScoreList;
            this.IsCalcFesId = eventData.IsCalcFesId;
            this.VictoryCandidateNpcId = eventData.VictoryCandidateNpcId;
            this.JudgeChildId = eventData.JudgeChildId;
            this.FishTypeList = eventData.FishTypeList;
            return this;
        }

    }

}
