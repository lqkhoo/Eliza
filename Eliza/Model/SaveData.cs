using Eliza.Core;
using System.IO;
using Eliza.Core.Serialization;
using System;
using Eliza.Model.Save;
//TODO:
//Generate JSON:
//https://github.com/SinsofSloth/RF5-global-metadata/blob/1a4dc9fb55263296c8a8564176591a7ab9fa1745/_no_namespace/FarmManager.FARM_ID.cs
//https://github.com/SinsofSloth/RF5-global-metadata/blob/1a4dc9fb55263296c8a8564176591a7ab9fa1745/_no_namespace/CropID.cs
//https://github.com/SinsofSloth/RF5-global-metadata/blob/1a4dc9fb55263296c8a8564176591a7ab9fa1745/_no_namespace/MineTypeID.cs
//https://github.com/SinsofSloth/RF5-global-metadata/blob/1a4dc9fb55263296c8a8564176591a7ab9fa1745/_no_namespace/ItemID.cs
//https://github.com/SinsofSloth/RF5-global-metadata/blob/1a4dc9fb55263296c8a8564176591a7ab9fa1745/Define/VariationNo.cs
//https://github.com/SinsofSloth/RF5-global-metadata/blob/1a4dc9fb55263296c8a8564176591a7ab9fa1745/Define/NPCID.cs
//https://github.com/SinsofSloth/RF5-global-metadata/blob/1a4dc9fb55263296c8a8564176591a7ab9fa1745/Define/PoliceRank.cs
//https://github.com/SinsofSloth/RF5-global-metadata/blob/1a4dc9fb55263296c8a8564176591a7ab9fa1745/Define/ActorID.cs
//https://github.com/SinsofSloth/RF5-global-metadata/blob/1a4dc9fb55263296c8a8564176591a7ab9fa1745/Define/Season.cs
//https://github.com/SinsofSloth/RF5-global-metadata/blob/1a4dc9fb55263296c8a8564176591a7ab9fa1745/Define/TimeZone.cs
//https://github.com/SinsofSloth/RF5-global-metadata/blob/1a4dc9fb55263296c8a8564176591a7ab9fa1745/Define/Weather.cs
//https://github.com/SinsofSloth/RF5-global-metadata/blob/1a4dc9fb55263296c8a8564176591a7ab9fa1745/Define/NpcEventType.cs
////https://github.com/SinsofSloth/RF5-global-metadata/blob/1a4dc9fb55263296c8a8564176591a7ab9fa1745/_no_namespace/EventProceedType.cs
//https://github.com/SinsofSloth/RF5-global-metadata/blob/1a4dc9fb55263296c8a8564176591a7ab9fa1745/Define/EventScriptID.cs
//https://github.com/SinsofSloth/RF5-global-metadata/blob/1a4dc9fb55263296c8a8564176591a7ab9fa1745/Define/EventPointID.cs
//https://github.com/SinsofSloth/RF5-global-metadata/blob/1a4dc9fb55263296c8a8564176591a7ab9fa1745/_no_namespace/EventCheckType.cs
//https://github.com/SinsofSloth/RF5-global-metadata/blob/1a4dc9fb55263296c8a8564176591a7ab9fa1745/Define/IconType.cs
//https://github.com/SinsofSloth/RF5-global-metadata/blob/1a4dc9fb55263296c8a8564176591a7ab9fa1745/Define/IconViewType.cs
//https://github.com/SinsofSloth/RF5-global-metadata/blob/1a4dc9fb55263296c8a8564176591a7ab9fa1745/Define/GameFlagData.cs
//https://github.com/SinsofSloth/RF5-global-metadata/blob/d89ff0d373c436b086dde5248049073d24bc85ff/Field/FieldSceneId.cs
//https://github.com/SinsofSloth/RF5-global-metadata/blob/1a4dc9fb55263296c8a8564176591a7ab9fa1745/Define/Place.cs
//https://github.com/SinsofSloth/RF5-global-metadata/blob/1a4dc9fb55263296c8a8564176591a7ab9fa1745/Define/NpcEventType.cs
//https://github.com/SinsofSloth/RF5-global-metadata/search?q=DatProgressType
//DateType
//DateSpotID
//ItemID
//FarmManager.RF4_CROP_GROW_STATE 
namespace Eliza.Model
{
    public class SaveData
    {
        public RF5SaveDataHeader header;
        public RF5SaveData saveData;
        public RF5SaveDataFooter footer;

        public SaveData(RF5SaveDataHeader header, RF5SaveData saveData, RF5SaveDataFooter footer)
        {
            this.header = header;
            this.saveData = saveData;
            this.footer = footer;
        }

        public static SaveData Read(string path)
        {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
            using (var ms = new MemoryStream())
            {
                //Don't want to overwrite the actual save
                fs.CopyTo(ms);
                Decrypt(ms);

                var deserializer = new BinaryDeserializer(ms);
                // var save = deserializer.Deserialize<SaveData>();
                // var reader = deserializer.Reader;

                // Parse header first, to extract file version
                var header = deserializer.Deserialize<RF5SaveDataHeader>();
                var version = header.version;

                RF5SaveData data;

                // Now parse the rest of the file
                switch (version)
                {
                    case >= 7: // v1.0.7 - ??
                        data = deserializer.Deserialize<RF5SaveData>();
                        break;
                    case >= 4: // v1.0.4 - v1.0.6
                        data = deserializer.Deserialize<RF5SaveData>(); // 107 toggle
                        // data = deserializer.Deserialize<RF5SaveDataV106>().AdaptTo(); // 106 toggle
                        break;
                    case >= 2: // v1.0.2 - v1.0.3
                        data = deserializer.Deserialize<RF5SaveDataV102>().AdaptTo();
                        break;
                    default:
                        throw new NotImplementedException("Unsupported version");
                }

                var footer = deserializer.Deserialize<RF5SaveDataFooter>();

                var save = new SaveData(header, data, footer);
                return save;
            };
        }

        private static void Decrypt(MemoryStream ms)
        {
            var reader = new BinaryReader(ms);
            var pos = ms.Seek(0x20, SeekOrigin.Begin);
            var encryptedData = reader.ReadBytes((int)(ms.Length - (0x20 + 0x10)));

            ms.Seek(pos, SeekOrigin.Begin);
            var decryptedData = Cryptography.Decrypt(encryptedData);
            ms.Write(decryptedData, 0, decryptedData.Length);
            ms.Seek(0, SeekOrigin.Begin);
        }

        public static void Write(string path, SaveData save)
        {
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
            {
                var serializer = new BinarySerializer(fs);
                var version = save.header.version;

                switch (version)
                {
                    case >= 7: // v1.0.7 - ??
                        serializer.Serialize(save);
                        break;
                    case >= 4: // v1.0.4 - v1.0.6
                        serializer.Serialize(save); // 107 toggle
                        // RF5SaveDataV106 data106 = new RF5SaveDataV106().AdaptFrom(save.saveData);
                        // serializer.Serialize(save.header);
                        // serializer.Serialize(data106);
                        // serializer.Serialize(save.footer);
                        break;
                    case >= 2: // v1.0.2 - v1.0.3
                        RF5SaveDataV102 data102 = new RF5SaveDataV102().AdaptFrom(save.saveData);
                        serializer.Serialize(save.header);
                        serializer.Serialize(data102);
                        serializer.Serialize(save.footer);
                        break;
                    default:
                        throw new NotImplementedException("Unsupported version");
                }

                // var writer = serializer.Writer;
            }
        }
    }
}
