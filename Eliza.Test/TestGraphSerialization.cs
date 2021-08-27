using Eliza.Core.Serialization;
using Eliza.Model;
using Eliza.Test.Utils;
using KellermanSoftware.CompareNetObjects;
using System;
using Xunit;

namespace Eliza.Test
{
    [Collection("107")]
    public class TestGraphSerialization
    {
        [Fact]
        public void TestGraphJP107()
        {
            string inputPath = "../../../Saves/107/rf5_gm000";
            SaveData save = SaveData.FromEncryptedFile(inputPath, version: 7, locale: SaveData.LOCALE.JP);
            ObjectGraph graph = save.ToObjectGraph();
            var foo = graph;
        }

        CompareLogic foo = new();
    }
}
