using UnityEngine;
using MessagePack;
using Eliza.Model.Item;
using Eliza.Core.Serialization.MessagePackFormatters;

namespace Eliza.Model.Farm
{
    [MessagePackObject]
    [MessagePackFormatter(typeof(FarmCropDataFormatter))]
    public class FarmCropData
    {
        // Fields
        [Key(0)]
        public Vector3Int CellSetId;
        [Key(1)]
        public int CellId;
        [Key(2)]
        public bool IsCultivated;
        [Key(3)]
        public bool IsWatering;
        [Key(4)]
        public bool IsPlanted;
        [Key(5)]
        public bool IsLargeSize;
        [Key(6)]
        public int PlantStatusLevel;
        [Key(7)]
        public int PlantStatusLevelMax;
        [Key(8)]
        public ItemData CropItemData;
        //[IgnoreMember]
        //public CropDataTable CropData;
    }
}
