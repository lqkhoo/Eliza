using Eliza.Model;
using Eliza.Test.Utils;
using System;
using Xunit;

namespace Eliza.Test
{

    // Use different files to avoid race conditions.
    // Otherwise put the tests in different collections to run them serially.

    [Collection("JP107")]
    public class TestBinarySerialization107
    {
        [Fact]
        public void TestDecryptEncryptJP107()
        {
            // Ensure byte-level reproduction when we haven't modified anything.
            // This is far more tricky than it sounds.

            string inputPath = "../../../Saves/107/rf5_gm000";
            string outputPath = inputPath + "_test_decrypt_encrypt_107";
            SaveData save = SaveData.FromEncryptedFile(inputPath, version: 7, locale: SaveData.LOCALE.JP);
            save.ToEncryptedFile(outputPath);
            Assert.True(TestUtils.AreFilesIdentical(inputPath, outputPath));

            string inputPath2 = "../../../Saves/107/rf5_gm001";
            string outputPath2 = inputPath2 + "_test_decrypt_encrypt_107";
            SaveData save2 = SaveData.FromEncryptedFile(inputPath2, version: 7, locale: SaveData.LOCALE.JP);
            save2.ToEncryptedFile(outputPath2);
            Assert.True(TestUtils.AreFilesIdentical(inputPath2, outputPath2));

        }

        [Fact]
        public void TestJustDecryptJP107()
        {
            string inputPath = "../../../Saves/107/rf5_gm002";
            string outputPath = inputPath + "_test_decrypt_107";
            SaveData.JustDecryptFile(
                    inputPath: inputPath,
                    outputPath: outputPath,
                    bypassSerialization: true,
                    version: 7,
                    locale: SaveData.LOCALE.JP
                );
            Assert.True(true);
            // No tests here. Just make sure no exceptions are thrown anywhere.
            // Need to check output manually in hex.
            // If this fails, nothing else will work, so start here.
        }
    }

    [Collection("106")]
    public class TestSerializationJP106
    {
        [Fact]
        public void TestDecryptEncryptJP106() {
            string inputPath = "../../../Saves/106/rf5_gm000";
            string outputPath = inputPath + "_test_decrypt_encrypt_106";
            SaveData save = SaveData.FromEncryptedFile(inputPath, version: 6, locale: SaveData.LOCALE.JP);
            save.ToEncryptedFile(outputPath);
            Assert.True(TestUtils.AreFilesIdentical(inputPath, outputPath));
        }

    }

    [Collection("102")]
    public class TestSerializationJP102
    {
        [Fact]
        public void TestDecryptEncryptJP102() {
            string inputPath = "../../../Saves/102/rf5_gm001";
            string outputPath = inputPath + "_test_decrypt_encrypt_102";
            SaveData save = SaveData.FromEncryptedFile(inputPath, version: 2, locale: SaveData.LOCALE.JP);
            save.ToEncryptedFile(outputPath);
            Assert.True(TestUtils.AreFilesIdentical(inputPath, outputPath));
        }

    }
}
