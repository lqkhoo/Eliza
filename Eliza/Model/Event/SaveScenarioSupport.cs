using static Eliza.Core.Serialization.Attributes;

namespace Eliza.Model.Event
{
    public class SaveScenarioSupport
    {
        [Length(Size = 24)]
        public int[] m_talked;
    }
}