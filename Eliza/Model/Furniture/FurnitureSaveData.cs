using System.Numerics;
using static Eliza.Core.Serialization.ElizaAttribute;

namespace Eliza.Model.Furniture
{
    public class FurnitureSaveData
    {
        public Vector3 Pos;
        public Quaternion Rot;
        public int SceneId;
        public int Id;
        [ElizaUtf16Uuid]
        public string UniqueId;
        public int Point;
        public int Hp;
        public bool Have;
    }
}
