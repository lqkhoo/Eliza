using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eliza.Test.Utils
{
    class TestUtils
    {

        // https://docs.microsoft.com/en-us/troubleshoot/dotnet/csharp/create-file-compare
        public static bool AreFilesIdentical(string path1, string path2)
        {
            // Just check every byte
            int f1byte;
            int f2byte;
            FileStream fs1 = new FileStream(path1, FileMode.Open);
            FileStream fs2 = new FileStream(path2, FileMode.Open);
            if (fs1.Length != fs2.Length) {
                return false;
            }

            do {
                f1byte = (byte)fs1.ReadByte();
                f2byte = (byte)fs2.ReadByte();
            } while ((f1byte==f2byte) && (f1byte != -1));

            fs1.Close();
            fs2.Close();

            return ((f1byte - f2byte) == 0);
        }


    }
}
