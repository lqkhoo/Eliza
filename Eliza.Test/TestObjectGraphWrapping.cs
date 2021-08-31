using Eliza.Avalonia.ViewModels;
using Eliza.Core.Serialization;
using Eliza.Model;
using Eliza.Test.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Eliza.Test
{
    [Collection("107")]
    public class TestObjectGraphWrapping
    {
        [Fact]
        public void TestUiObjectGraphWrapping()
        {
            string inputPath = "../../../Saves/107/rf5_gm000";
            string outputPath = "../../../Saves/107/rf5_gm000_test_objectgraph_wrapper";
            SaveData save = SaveData.FromEncryptedFile(inputPath, version: 7, locale: SaveData.LOCALE.JP);
            ObjectGraph graph = save.ToObjectGraph();
            UiObjectGraph uiGraph = new(graph);
            ObjectGraph otherGraph = UiObjectGraph.Unwrap(uiGraph);
            save.FromObjectGraph(otherGraph);
            save.ToEncryptedFile(outputPath);
            Assert.True(TestUtils.AreFilesIdentical(inputPath, outputPath));

            string inputPath2 = "../../../Saves/107/rf5_gm001";
            string outputPath2 = "../../../Saves/107/rf5_gm001_test_objectgraph_wrapper";
            SaveData save2 = SaveData.FromEncryptedFile(inputPath2, version: 7, locale: SaveData.LOCALE.JP);
            ObjectGraph graph2 = save2.ToObjectGraph();
            UiObjectGraph uiGraph2 = new(graph2);
            ObjectGraph otherGraph2 = UiObjectGraph.Unwrap(uiGraph2);
            save2.FromObjectGraph(otherGraph2);
            save2.ToEncryptedFile(outputPath2);
            Assert.True(TestUtils.AreFilesIdentical(inputPath2, outputPath2));
        }
    }
}
