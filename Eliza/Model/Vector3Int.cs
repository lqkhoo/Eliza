using MessagePack;
using Eliza.Core.Serialization.MessagePackFormatters;

namespace Eliza.Model
{
    // This is a mirror of UnityEngine.Vector3Int.
    // The game serializes these as fixed-width Int32 (0xd2) values,
    // so we need custom a MessagePackFormatter

    [MessagePackObject]
    [MessagePackFormatter(typeof(Vector3IntFormatter))]
    public class Vector3Int
    {
        [Key(0)]
        public int x;
        [Key(1)]
        public int y;
        [Key(2)]
        public int z;

        // Required for GraphDeserializer
        public Vector3Int()
        {
            this.x = 0;
            this.y = 0;
            this.z = 0;
        }

        public Vector3Int(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }


}
