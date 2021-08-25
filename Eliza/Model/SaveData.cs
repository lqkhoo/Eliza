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

        protected SaveData(byte[] headerBytes, byte[] decryptedDataBytes, byte[] footerBytes)
        {
            this.header = new BinaryDeserializer(new MemoryStream(headerBytes)).ReadSaveDataHeader();
            this.saveData = new BinaryDeserializer(new MemoryStream(decryptedDataBytes)).ReadSaveData();
            this.footer = new BinaryDeserializer(new MemoryStream(footerBytes)).ReadSaveDataFooter();
        }

        protected static Tuple<byte[], byte[], byte[]> DecryptFile(string path)
        {
            const int HEADER_NBYTES = 0x20;
            const int FOOTER_NBYTES = 0x10;
            byte[] header;
            byte[] encrypted_data;
            byte[] decrypted_data;
            byte[] footer;

            using (FileStream fs = new(path, FileMode.Open, FileAccess.Read)) {
                BinaryReader reader = new(fs);
                int total_nbytes = (int)fs.Length;
                int data_length = total_nbytes - HEADER_NBYTES - FOOTER_NBYTES;
                header = reader.ReadBytes(HEADER_NBYTES);
                encrypted_data = reader.ReadBytes(data_length);
                footer = reader.ReadBytes(FOOTER_NBYTES);
                decrypted_data = Cryptography.Decrypt(encrypted_data);
            }
            return new Tuple<byte[], byte[], byte[]>(header, decrypted_data, footer);
        }


        public static SaveData FromEncryptedFile(string path, int version=7)
        {
            var (header, data, footer) = DecryptFile(path);
            return new SaveData(header, data, footer);
        }


        // In this case we don't care about byte-for-byte reproduction because
        // we aren't going to re-encrypt it. This is also useful to ensure that
        // deserialization is working properly, to troubleshoot the serialization bit.
        public static void JustDecryptFile(string inputPath, string outputPath, int version=7)
        {
            // Note that we're not just concatenating the contents from
            // DecryptFile() together. We actually want to test serialization.
            SaveData save = SaveData.FromEncryptedFile(inputPath);
            using (var fs = new FileStream(outputPath, FileMode.OpenOrCreate, FileAccess.Write)) {

                fs.SetLength(0); // Empty previous file contents.
                var serializer = new BinarySerializer(fs, encrypt: false);
                serializer.WriteSaveFile(save, encrypt: false);
            }
        }


        public void Write(string path, int version=7)
        {
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
            {
                var serializer = new BinarySerializer(fs);
                serializer.WriteSaveFile(this, encrypt: true);
            }
        }
    }
}
