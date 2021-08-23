using MessagePack;

namespace Eliza.Model.Event
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class EventProceedRequirement
    {
        public int EventProceedType;
    }
}
