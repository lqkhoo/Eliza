using MessagePack;

namespace Eliza.Model.Event
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class SubEventSaveData
    {
        public int ProgressingSubEventID;
    }
}
