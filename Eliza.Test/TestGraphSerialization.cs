using Eliza.Core.Serialization;
using Eliza.Model;
using Eliza.Model.Save;
using Eliza.Test.Utils;
using KellermanSoftware.CompareNetObjects;
using System;
using Xunit;

namespace Eliza.Test
{
    [Collection("JP107")]
    public class TestGraphSerializationJP107
    {
        [Fact]
        public void TestGraphJP107()
        {
            string inputPath = "../../../Saves/107/rf5_gm003";
            string outputPath = inputPath + "_test_graph_serialization";
            SaveData save = SaveData.FromEncryptedFile(inputPath, version: 7, locale: SaveData.LOCALE.JP);
            ObjectGraph graph = save.ToObjectGraph();
            save.FromObjectGraph(graph);
            save.ToEncryptedFile(outputPath);
            Assert.True(TestUtils.AreFilesIdentical(inputPath, outputPath));

            string inputPath2 = "../../../Saves/107/rf5_gm004";
            string outputPath2 = inputPath2 + "_test_graph_serialization";
            SaveData save2 = SaveData.FromEncryptedFile(inputPath2, version: 7, locale: SaveData.LOCALE.JP);
            ObjectGraph graph2 = save2.ToObjectGraph();
            save2.FromObjectGraph(graph2);
            save2.ToEncryptedFile(outputPath2);
            Assert.True(TestUtils.AreFilesIdentical(inputPath2, outputPath2));
        }

    }

    [Collection("106")]
    public class TestGraphSerializationJP106
    {
        [Fact]
        public void TestGraphJP106()
        {
            string inputPath = "../../../Saves/106/rf5_gm000";
            string outputPath = inputPath + "_test_graph_serialization";
            SaveData save = SaveData.FromEncryptedFile(inputPath, version: 6, locale: SaveData.LOCALE.JP);
            ObjectGraph graph = save.ToObjectGraph();
            save.FromObjectGraph(graph);
            save.ToEncryptedFile(outputPath);
            Assert.True(TestUtils.AreFilesIdentical(inputPath, outputPath));
        }
    }

    [Collection("102")]
    public class TestGraphSerializationJP102
    {
        [Fact]
        public void TestGraphJP102()
        {
            string inputPath = "../../../Saves/102/rf5_gm001";
            string outputPath = inputPath + "_test_graph_serialization";
            SaveData save = SaveData.FromEncryptedFile(inputPath, version: 2, locale: SaveData.LOCALE.JP);
            ObjectGraph graph = save.ToObjectGraph();
            save.FromObjectGraph(graph);
            save.ToEncryptedFile(outputPath);
            Assert.True(TestUtils.AreFilesIdentical(inputPath, outputPath));
        }
    }




}
