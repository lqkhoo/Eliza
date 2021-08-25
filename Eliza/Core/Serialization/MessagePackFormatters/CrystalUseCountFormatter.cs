using MessagePack;
using MessagePack.Formatters;

using Eliza.Model.Farm;

namespace Eliza.Core.Serialization.MessagePackFormatters
{
    class CrystalUseCountFormatter : IMessagePackFormatter<CrystalUseCount>
    {
        public CrystalUseCount Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            CrystalUseCount farmSizeLevel = new();
            options.Security.DepthStep(ref reader);
            farmSizeLevel.Count = reader.ReadByte();
            return farmSizeLevel;
        }

        public void Serialize(ref MessagePackWriter writer, CrystalUseCount value, MessagePackSerializerOptions options)
        {
            writer.Write(value.Count);
        }

    }
}
