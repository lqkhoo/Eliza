using System.IO;
using Eliza.Model;

namespace Eliza.Core.Serialization
{
    public abstract class BinaryBaseSerializer : BaseSerializer
    {
        public readonly Stream BaseStream;

        public BinaryBaseSerializer(Stream baseStream, SaveData.LOCALE locale, int version)
            : base(locale, version)
        {
            this.BaseStream = baseStream;
        }

    }
}
