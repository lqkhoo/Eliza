using MessagePack;
using MessagePack.Formatters;
using Eliza.Core.Serialization.MessagePackFormatters;

namespace Eliza.Model
{
    // These are message-packed integers that
    // for most purposes fit within a single byte.
    [MessagePackObject]
    [MessagePackFormatter(typeof(MessagePackIntFormatter))]
    public class MessagePackInt
    {
        [Key(0)]
        public int Value;
    }
}
