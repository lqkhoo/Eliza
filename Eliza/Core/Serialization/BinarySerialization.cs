using System.IO;
using Eliza.Model;

namespace Eliza.Core.Serialization
{
    public abstract class BinarySerialization : BaseSerializer
    {
        public readonly Stream BaseStream;

        public BinarySerialization(Stream baseStream, SaveData.LOCALE locale, int version)
            : base(locale, version)
        {
            this.BaseStream = baseStream;
        }

    }
}
