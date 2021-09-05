using System.Numerics;
using Eliza.Core.Serialization;

namespace Eliza.Model.Furniture
{
    public class FurnitureSaveData
    {
        public Vector3 Pos;
        public Quaternion Rot;
        public int SceneId;
        public int Id;
        // This is either a UUID, or if it's null,
        // it gets serialized to 6 bytes: FD 00 FF 00 00 00.
        [ElizaString(IsUtf16Uuid=true)]
        public string UniqueId;
        public int Point;
        public int Hp;
        public bool Have;
    }
}
