using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using System.Text;
using System.Security.Cryptography;
using System;

namespace Eliza.Core
{
    public class Cryptography
    {
        public static byte[] Encrypt(byte[] data)
        {
            return RijndaelCrypto(data, isEncrypting: true);
        }

        public static byte[] Decrypt(byte[] data)
        {
            return RijndaelCrypto(data, isEncrypting: false);
        }

        public static byte[] RijndaelCrypto(byte[] data, bool isEncrypting)
        {

            //RF5 uses 256bit block Rijndael Encryption
            var aesKey = Encoding.UTF8.GetBytes("1cOSvkZ4HQCi6z/yQpEEl4neB+AIXwTX");
            var aesIV = Encoding.UTF8.GetBytes("XuMigxpK61gLwgo1RsreLLGPcw3vJFze");
            var engine = new RijndaelEngine(256);
            var blockCipher = new CbcBlockCipher(engine);
            var cipher = new BufferedBlockCipher(blockCipher);
            var keyParam = new KeyParameter(aesKey);
            var keyParamWithIV = new ParametersWithIV(keyParam, aesIV, 0, 32);

            cipher.Init(isEncrypting, keyParamWithIV);
            var output = new byte[cipher.GetOutputSize(data.Length)];
            var length = cipher.ProcessBytes(data, output, 0);
            cipher.DoFinal(output, length);

            // This is when using BouncyCastle.NetCore 1.8.8.
            // Their implementation returns an extra 32 bytes during encryption
            // compared to the following reference implementations:
            // 
            // * RF5's original encryption routines
            // * Py3rijndael: https://github.com/meyt/py3rijndael
            // 
            // It's nice to preserve the property inputSize == outputSize.
            // However, to keep things consistent with the game,
            // we truncate the last block.
            if (isEncrypting) {
                var destArray = new byte[output.Length - 0x20];
                Array.Copy(output, destArray, destArray.Length);
                output = destArray;
            }

            return output;
        }

        public static uint Checksum(byte[] buffer)
        {
            uint sum = 0xcbf29ce4;
            uint lo = 0;
            uint running_sum = 0x39;
            for (int index = 0; index < buffer.Length; index++)
            {
                uint value = buffer[index];
                uint delta = value - lo;
                lo = (lo & 0xff) + 0xb2;
                sum = sum * 0x1b3 ^ (delta ^ running_sum) & 0xff;
                running_sum = value;
            }
            return sum;
        }
    }
}
