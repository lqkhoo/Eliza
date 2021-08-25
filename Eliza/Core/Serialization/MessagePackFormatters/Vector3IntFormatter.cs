using MessagePack;
using MessagePack.Formatters;
using Eliza.Model;

namespace Eliza.Core.Serialization.MessagePackFormatters
{
    public class Vector3IntFormatter : IMessagePackFormatter<Vector3Int>
    {
        public Vector3Int Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            options.Security.DepthStep(ref reader);
            reader.ReadArrayHeader(); // 3
            int x = reader.ReadInt32();
            int y = reader.ReadInt32();
            int z = reader.ReadInt32();
            return new Vector3Int(x, y, z);
        }

        public void Serialize(ref MessagePackWriter writer, Vector3Int value, MessagePackSerializerOptions options)
        {
            writer.WriteArrayHeader(3);
            writer.WriteInt32(value.x);
            writer.WriteInt32(value.y);
            writer.WriteInt32(value.z);
        }
    }
}
