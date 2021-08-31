namespace Eliza.Model.Fishing
{
    public class FishRecord
    {
        public int ItemId; // Original: Id
        public int Min;
        public int Max;
        public int Stamp;

        //The game uses this, but we won't for widget generation purposes
        //public Vector3Int Size;
    }
}
