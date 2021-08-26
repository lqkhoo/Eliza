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
        [ElizaString(IsUtf16Uuid=true)]
        public string UniqueId;
        public int Point;
        public int Hp;
        public bool Have;
    }
}
