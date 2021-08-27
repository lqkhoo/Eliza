
using System.IO;
using System;
using System.Linq;
using Eliza.Core;
using Eliza.Core.Serialization;
using Eliza.Model.Save;
using System.Collections.Generic;
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
        public const int HEADER_NBYTES = 0x20;
        public const int FOOTER_NBYTES = 0x10;

        public enum LOCALE : Int32 { JP=0, EN=1, ANY=2 }
        public static readonly Dictionary<string, LOCALE> LocaleMap = new() {
            { "JP", LOCALE.JP },
            { "EN", LOCALE.EN }
        };

        public const int LATEST_JP_VER = 7;
        public static readonly Dictionary<string, int> JpVersionMap = new() {
            { "1.0.7-1.0.9", LATEST_JP_VER },
            { "1.0.4-1.0.6", 4 },
            { "1.0.2-1.0.3", 2 }
        };

        public const int LATEST_EN_VER = -1;
        public static readonly Dictionary<string, int> EnVersionMap = new() {};

        public readonly LOCALE Locale;
        public readonly int Version;

        public RF5SaveDataHeader header;
        public RF5SaveData saveData;
        public RF5SaveDataFooter footer;

        public readonly byte[] _originalHeader;
        public readonly byte[] _originalSaveData;
        public readonly byte[] _originalFooter;

        protected SaveData(byte[] headerBytes, byte[] decryptedDataBytes, byte[] footerBytes,
                           int version= LATEST_JP_VER, LOCALE locale=LOCALE.JP)
        {

            this.Locale = locale;
            this.Version = version;

            using (MemoryStream header = new(headerBytes))
            using (MemoryStream saveData = new(decryptedDataBytes))
            using (MemoryStream footer = new(footerBytes))
            {
                this.header = new BinaryDeserializer(header, locale, version).ReadSaveDataHeader();
                this.saveData = new BinaryDeserializer(saveData, locale, version).ReadSaveData();
                this.footer = new BinaryDeserializer(footer, locale, version).ReadSaveDataFooter();
            }
            this._originalHeader = headerBytes;
            this._originalSaveData = decryptedDataBytes;
            this._originalFooter = footerBytes;
        }

        protected static Tuple<byte[], byte[], byte[]> DecryptFile(string path)
        {
            byte[] header;
            byte[] encryptedData;
            byte[] decryptedData;
            byte[] footer;

            using (FileStream fs = new(path, FileMode.Open, FileAccess.Read)) {
                BinaryReader reader = new(fs);
                int total_nbytes = (int)fs.Length;
                int data_length = total_nbytes - SaveData.HEADER_NBYTES - SaveData.FOOTER_NBYTES;
                header = reader.ReadBytes(SaveData.HEADER_NBYTES);
                encryptedData = reader.ReadBytes(data_length);
                footer = reader.ReadBytes(SaveData.FOOTER_NBYTES);
                decryptedData = Cryptography.Decrypt(encryptedData);

                // decrypted_data contains undefined bytes at the end.
                // These are from uninitialized buffer indices from
                // the padding operation for the previous encryption.
                // We need to reproduce these to maintain the checksum
                // for an unmodified file.
            }
            return new Tuple<byte[], byte[], byte[]>(header, decryptedData, footer);
        }


        public static SaveData FromEncryptedFile(string path, int version, LOCALE locale)
        {
            var (header, data, footer) = DecryptFile(path);
            return new SaveData(header, data, footer, version, locale);
        }


        public static void JustDecryptFile(string inputPath, string outputPath,
                                           int version, LOCALE locale,
                                           bool bypassSerialization = true)
        {
            if(bypassSerialization) {
                using (FileStream fs = new(outputPath, FileMode.OpenOrCreate, FileAccess.Write)) {
                    var (header, decryptedData, footer) = SaveData.DecryptFile(inputPath);
                    fs.SetLength(0); // Empty previous file contents
                    fs.Write(header);
                    fs.Write(decryptedData);
                    fs.Write(footer);
                }
            } else {
                SaveData save = SaveData.FromEncryptedFile(inputPath, version, locale);
                save.ToDecryptedFile(outputPath);
            }
        }


        public void ToDecryptedFile(string outputPath)
        {
            using (FileStream fs = new(outputPath, FileMode.OpenOrCreate, FileAccess.Write)) {
                using MemoryStream ms = new();
                BinarySerializer serializer = new(ms, this.Locale, this.Version);
                serializer.WriteSaveDataHeader(this.header);
                serializer.WriteSaveData(this.saveData); // Plain write. No encryption.
                serializer.WriteSaveDataFooter(this.footer);
                serializer.BaseStream.SetLength(serializer.BaseStream.Position); // Truncate
                fs.SetLength(0);
                ms.CopyTo(fs);
            }
        }

        public void ToEncryptedFile(string path)
        {
            using (FileStream fs = new(path, FileMode.Create, FileAccess.ReadWrite))
            {
                using MemoryStream buffer = new();

                // Write old unencrypted data including junk
                buffer.Write(this._originalHeader);
                buffer.Write(this._originalSaveData);
                long junkLength = buffer.Position; // should equal paddedLength
                buffer.Position = 0x0;

                // Write new unencrypted data to buffer
                using MemoryStream serializerBuffer = new();
                BinarySerializer serializer = new(serializerBuffer, this.Locale, this.Version);
                serializer.BaseStream.Position = 0x0;
                serializer.WriteSaveDataHeader(this.header);
                serializer.WriteSaveData(this.saveData);
                long bodyLength = serializer.BaseStream.Position;
                long paddedLength = ((bodyLength - 0x20 + 0x1F) & ~0x1F) + 0x20;
                serializer.BaseStream.CopyTo(buffer);

                int headerLength = SaveData.HEADER_NBYTES;
                buffer.Position = 0x0;
                byte[] headerData = new byte[headerLength];
                byte[] paddedSaveData = new byte[paddedLength];
                buffer.Read(headerData, offset: 0, count: headerLength);
                buffer.Read(paddedSaveData, offset: 0, count: (int)paddedLength);

                // Compute
                byte[] encryptedData = Cryptography.Encrypt(paddedSaveData);

                // The checksum only computes up to the point of the 
                // encrypted data, which should be 0x20 shorter than
                // paddedLength.
                List<byte> bodyData = new();
                bodyData.AddRange(headerData);
                bodyData.AddRange(encryptedData);
                var foo = bodyData.Count;
                uint checksum = Cryptography.Checksum(bodyData.ToArray());

                buffer.Position = headerLength;
                buffer.Write(encryptedData);

                // Write footer
                serializer = new(buffer, this.Locale, this.Version);
                serializer.Writer.Write((int)bodyLength);
                serializer.Writer.Write((int)paddedLength);
                serializer.Writer.Write(checksum);
                serializer.Writer.Write(this.footer.Blank);
                buffer.SetLength(serializer.BaseStream.Position);
                buffer.Position = 0x0;

                fs.SetLength(0);
                buffer.CopyTo(fs, (int)buffer.Length);
            }
        }
    }
}
