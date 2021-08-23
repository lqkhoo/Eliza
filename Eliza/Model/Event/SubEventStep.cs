using MessagePack;

namespace Eliza.Model.Event
{
    [MessagePackObject()]
    public class SubEventStep
    {
        [Key(0)]
        public int TargetEventStep;
        [Key(1)]
        public int KeepEventStep;
        [Key(2)]
        public int[] MainEventSteps;
    }
}
