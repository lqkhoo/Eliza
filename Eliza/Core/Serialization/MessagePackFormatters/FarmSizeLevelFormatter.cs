using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagePack;
using MessagePack.Formatters;

using Eliza.Model.Farm;

namespace Eliza.Core.Serialization.MessagePackFormatters
{
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
