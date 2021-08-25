using MessagePack;
using MessagePack.Formatters;
using Eliza.Model;
using Eliza.Model.Farm;
using Eliza.Model.Item;


namespace Eliza.Core.Serialization.MessagePackFormatters
{
    public class FarmCropDataFormatter : IMessagePackFormatter<FarmCropData>
    {
        public FarmCropData Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            IFormatterResolver resolver = options.Resolver;
            options.Security.DepthStep(ref reader);

            FarmCropData farmCropData = new();

            reader.ReadArrayHeader(); // 9
            farmCropData.CellSetId = resolver.GetFormatterWithVerify<Vector3Int>().Deserialize(ref reader, options);
            farmCropData.CellId = reader.ReadInt32();
            farmCropData.IsCultivated = reader.ReadBoolean();
            farmCropData.IsWatering = reader.ReadBoolean();
            farmCropData.IsPlanted = reader.ReadBoolean();
            farmCropData.IsLargeSize = reader.ReadBoolean();
            farmCropData.PlantStatusLevel = reader.ReadInt32();
            farmCropData.PlantStatusLevelMax = reader.ReadInt32();
            farmCropData.CropItemData = resolver.GetFormatterWithVerify<ItemData>().Deserialize(ref reader, options);
            return farmCropData;
        }

        public void Serialize(ref MessagePackWriter writer, FarmCropData value, MessagePackSerializerOptions options)
        {
            IFormatterResolver resolver = options.Resolver;

            writer.WriteArrayHeader(9);
            resolver.GetFormatterWithVerify<Vector3Int>().Serialize(ref writer, value.CellSetId, options);
            writer.Write(value.CellId);
            writer.Write(value.IsCultivated);
            writer.Write(value.IsWatering);
            writer.Write(value.IsPlanted);
            writer.Write(value.IsLargeSize);
            writer.Write(value.PlantStatusLevel);
            writer.Write(value.PlantStatusLevelMax);
            resolver.GetFormatterWithVerify<ItemData>().Serialize(ref writer, value.CropItemData, options);
        }
    }
}
