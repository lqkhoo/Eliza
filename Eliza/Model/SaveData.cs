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

        protected MemoryStream _originalFileCopy;

        public SaveData(RF5SaveDataHeader header, RF5SaveData saveData,
                            RF5SaveDataFooter footer, MemoryStream originalFileCopy)
        {
            this.header = header;
            this.saveData = saveData;
            this.footer = footer;
            this._originalFileCopy = originalFileCopy;
        }

        public static SaveData FromEncryptedFile(string path, int version=7)
        {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (var ms = new MemoryStream())
            {
                // For byte-level reproduction, we need to emulate the Switch
                // reading and writing to the same file, even though we are 
                // operating within a buffer. Therefore we need to store a copy
                // of the original encrypted file, in order to be able to
                // reproduce it exactly when writing back.

                fs.CopyTo(ms); // Copy file contents to a buffer
                Decrypt(ms);

                BinaryDeserializer deserializer = new(ms);

                RF5SaveDataHeader header = deserializer.ReadSaveDataHeader();
                RF5SaveData data = deserializer.ReadSaveData();
                RF5SaveDataFooter footer = deserializer.ReadSaveDataFooter();
                MemoryStream originalFileCopy = new();
                fs.CopyTo(originalFileCopy);

                SaveData save = new (header, data, footer, originalFileCopy);
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

        // This just concatenates the original header, decrypted data, and
        // the original footer into a file for downstream processing or inspection.
        // In this case we don't care about byte-for-byte reproduction because
        // we aren't going to re-encrypt it. This is also useful to ensure that
        // deserialization is working properly, to troubleshoot the serialization bit.
        public static void JustDecryptFile(string inputPath, string outputPath, int version=7)
        {

            SaveData save = SaveData.FromEncryptedFile(inputPath);

            using (var fs = new FileStream(outputPath, FileMode.OpenOrCreate, FileAccess.Write)) {

                fs.SetLength(0); // Empty previous file contents.
                var serializer = new BinarySerializer(fs, encrypt: false);
                serializer.Serialize(save.header);
                serializer.Serialize(save.saveData);
                serializer.Serialize(save.footer);

                /*
                // var version = save.header.version;
                switch (version) {
                    case >= 7: // v1.0.7 - ??
                        serializer.Serialize(save.header);
                        serializer.Serialize(save.saveData);
                        serializer.Serialize(save.footer);
                        break;
                    case >= 4: // v1.0.4 - v1.0.6
                        // serializer.Serialize(save); // 107 toggle
                        RF5SaveDataV106 data106 = new RF5SaveDataV106().AdaptFrom(save.saveData);
                        serializer.Serialize(save.header);
                        serializer.Serialize(data106);
                        serializer.Serialize(save.footer);
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
                */

            }
        }

        public void Write(string path, int version=7)
        {
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
            {
                var serializer = new BinarySerializer(fs);

                serializer.Serialize(this);

                /*
                // var version = save.header.version;
                switch (version)
                {
                    case >= 7: // v1.0.7 - ??
                        serializer.Serialize(this);
                        break;
                    case >= 4: // v1.0.4 - v1.0.6
                        // serializer.Serialize(save); // 107 toggle
                        RF5SaveDataV106 data106 = new RF5SaveDataV106().AdaptFrom(this.saveData);
                        serializer.Serialize(this.header);
                        serializer.Serialize(data106);
                        serializer.Serialize(this.footer);
                        break;
                    case >= 2: // v1.0.2 - v1.0.3
                        RF5SaveDataV102 data102 = new RF5SaveDataV102().AdaptFrom(this.saveData);
                        serializer.Serialize(this.header);
                        serializer.Serialize(data102);
                        serializer.Serialize(this.footer);
                        break;
                    default:
                        throw new NotImplementedException("Unsupported version");
                }
                */
            }
        }
    }
}
