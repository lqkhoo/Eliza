using MessagePack;
using MessagePack.Formatters;

using Eliza.Model;

namespace Eliza.Core.Serialization.MessagePackFormatters
{
    public class MessagePackIntFormatter : IMessagePackFormatter<MessagePackInt>
    {
        public MessagePackInt Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            options.Security.DepthStep(ref reader);

            MessagePackInt mpackInt = new();
            mpackInt.Value = reader.ReadInt32();
            return mpackInt;
        }

        public void Serialize(ref MessagePackWriter writer, MessagePackInt value, MessagePackSerializerOptions options)
        {
            writer.Write(value.Value);
        }
    }
}
