using MessagePack;
using MessagePack.Formatters;

using Eliza.Model.Farm;

namespace Eliza.Core.Serialization.MessagePackFormatters
{
    public class HarvestCountFormatter : IMessagePackFormatter<HarvestCount>
    {

        public HarvestCount Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            options.Security.DepthStep(ref reader);

            HarvestCount harvestCount = new();
            harvestCount.Count = reader.ReadInt32();
            return harvestCount;
        }

        public void Serialize(ref MessagePackWriter writer, HarvestCount value, MessagePackSerializerOptions options)
        {
            writer.Write(value.Count);
        }
    }
}
