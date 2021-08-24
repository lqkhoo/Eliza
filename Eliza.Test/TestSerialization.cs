using Microsoft.VisualStudio.TestTools.UnitTesting;
using Eliza.Model;
using Eliza.Test.Utils;
using System;

namespace Eliza.Test
{
    [TestClass]
    public class TestSerialization107
    {

        // Use different files to avoid race conditions
        // throwing IOExceptions. Otherwise we need Xunit.

        [TestMethod]
        public void TestDecryptEncrypt107()
        {

            // Ensure byte-level reproduction when we haven't modified anything.
            // This is more tricky than it sounds, due to side-effects from
            // MessagePack and transformed zero-padding from Rijndael. However,
            // this is the only way to ensure that serialization works end-to-end.

            string inputPath = "../../../Saves/107/rf5_gm000";
            string outputPath = inputPath + "_test_decrypt_encrypt_107-mk2";

            SaveData save = SaveData.FromEncryptedFile(inputPath);
            save.Write(outputPath);

            Assert.IsTrue(TestUtils.AreFilesIdentical(inputPath, outputPath));
        }

        [TestMethod]
        public void TestJustDecrypt107()
        {
            string inputPath = "../../../Saves/107/rf5_gm001";
            string outputPath = inputPath + "_test_decrypt_107";

            SaveData.JustDecryptFile(
                    inputPath: inputPath,
                    outputPath: outputPath,
                    version: 7
                );

            // No tests here. Just make sure no exceptions are thrown anywhere.
            // Need to check output manually in hex.
            // If this fails, nothing else will work, so start here.
            Assert.IsTrue(true);
        }
    }

    [TestClass]
    public class TestSerialization104
    {

    }

    [TestClass]
    public class TestSerialization102
    {

    }
}
