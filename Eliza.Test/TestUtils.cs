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
            // Just check every byte. Completely load both files into memory for speed.

            using (FileStream fs1 = new (path1, FileMode.Open))
            using (FileStream fs2 = new(path2, FileMode.Open)) {

                if(fs1.Length != fs2.Length) {
                    return false;
                }

                byte[] arr1 = new byte[fs1.Length];
                byte[] arr2 = new byte[fs2.Length];

                fs1.Read(arr1, 0, arr1.Length);
                fs2.Read(arr2, 0, arr2.Length);

                for(int i=0; i<arr1.Length; i++) {
                    if(arr1[i] != arr2[i]) {
                        return false;
                    }
                }
                return true;
            }
        }

    }
}
