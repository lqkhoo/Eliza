using Eliza.Core.Serialization;

namespace Eliza.Model.Event
{
    public class SaveScenarioSupport
    {
        [ElizaList(FixedSize = 24)]
        public int[] m_talked;
    }
}