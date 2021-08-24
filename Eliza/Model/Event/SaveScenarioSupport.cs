using static Eliza.Core.Serialization.ElizaAttribute;

namespace Eliza.Model.Event
{
    public class SaveScenarioSupport
    {
        [ElizaSize(Fixed = 24)]
        public int[] m_talked;
    }
}