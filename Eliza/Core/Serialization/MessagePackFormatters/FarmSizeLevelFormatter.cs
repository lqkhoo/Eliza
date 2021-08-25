using MessagePack;
using MessagePack.Formatters;

using Eliza.Model.Farm;

namespace Eliza.Core.Serialization.MessagePackFormatters
{
    // This class is required because the game writes the int as a single byte (ufixedint 0x0-0x7)

    class FarmSizeLevelFormatter : IMessagePackFormatter<FarmSizeLevel>
    {
        public FarmSizeLevel Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            FarmSizeLevel farmSizeLevel = new();
            options.Security.DepthStep(ref reader);
            farmSizeLevel.Level = reader.ReadByte();
            return farmSizeLevel;
        }

        public void Serialize(ref MessagePackWriter writer, FarmSizeLevel value, MessagePackSerializerOptions options)
        {
            writer.Write(value.Level);
        }
    }
}
