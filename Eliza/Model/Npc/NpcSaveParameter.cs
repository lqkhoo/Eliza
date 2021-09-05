using Eliza.Core.Serialization;
using MessagePack;
using System.Collections.Generic;
using UnityEngine;

namespace Eliza.Model.Npc
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class NpcSaveParameter
    {
        // [Key("SavePosition")]
        public Vector3 SavePosition;
        // [Key("SaveRotation")]
        public Quaternion SaveRotation;
        // [Key("forceDisabled")]
        public bool forceDisabled;
        // [Key("isShortPlay")]
        public bool isShortPlay;
        // [Key("AnimationState")]
        public int AnimationState;
        // [Key("")]
        // public byte Unknown;
        // [Key("AnimationSitting")]
        public bool AnimationSitting;
        // [Key("NpcGroupId")]
        public int NpcGroupId;
        // [Key("CurrentFieldPlaceId")]
        public int CurrentFieldPlaceId;
        // [Key("CurrentLifecycleState")]
        public int CurrentLifecycleState;
        // [Key("CurrentPlace")]
        public int CurrentPlace;
        // [Key("RotatePatternNumber")]
        public int RotatePatternNumber;
        // [Key("IsTalking")]
        public bool IsTalking;
        // [Key("TodayTalkCount")]
        public int TodayTalkCount;
        // [Key("NowEventId")]
        public int NowEventId;
        // [Key("Home")]
        public int Home;
        // [Key("Job")]
        public int Job;
        // [Key("IsPartner")]
        public bool IsPartner;
        // [Key("IsSpouses")]
        public bool IsSpouses;
        // [Key("IsLover")]
        public bool IsLover;
        // [Key("IsRefused")]
        public bool IsRefused;
        // [Key("IsJealousy")]
        public bool IsJealousy;
        // [Key("IsDateRefrain")]
        public bool IsDateRefrain;
        // [Key("IsExclamation")]
        public bool IsExclamation;
        // [Key("AngryStep")]
        public int AngryStep;
        // [Key("LovePoint")]
        public int LovePoint;
        // [Key("DatingNum")]
        public int DatingNum;
        // [Key("PresentCount")]
        public int PresentCount;
        // [Key("NickNameToPlayerId")]
        public int NickNameToPlayerId;
        // [Key("NickNameFromPlayerId")]
        public int NickNameFromPlayerId;
        // [Key("WeddingAnniversary")]
        public int WeddingAnniversary;
        // [Key("PresentItemTypes")]
        public List<int> PresentItemTypes;
        // [Key("IsVoiceSleepPlayed")]
        public bool IsVoiceSleepPlayed;
        // [Key("IsVoiceGreeted")]
        public bool IsVoiceGreeted;
        // [Key("TalkedTime")]
        public long[] TalkedTime;
        // [Key("FriendlyMilestoneTalk")]
        public int FriendlyMilestoneTalk;
        // [Key("ChatTalkLv")]
        public int ChatTalkLv;
        // [Key("ChatTalkCount")]
        public int ChatTalkCount;
        // [Key("ChatTalkLotteryType")]
        public int ChatTalkLotteryType;
        // [Key("ChatTalkLotteryID")]
        public int ChatTalkLotteryID;
        // [Key("HomeTalkedLv")]
        public int HomeTalkedLv;
        // [Key("ModelType")]
        public int ModelType;
        // [Key("LoveStroyState")]
        public int LoveStroyState;
        // [Key("FlowerFesDateNum")]
        public int FlowerFesDateNum;
        // [Key("IsDateReserved")]
        public bool IsDateReserved;
        // [Key("dateDay")]
        public int dateDay;
    }
}
