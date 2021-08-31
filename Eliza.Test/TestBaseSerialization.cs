using Eliza.Model;
using Eliza.Test.Utils;
using System;
using Xunit;
using Eliza.Model.Item;
using System.Reflection;
using System.Collections.Generic;
using Eliza.Core.Serialization;

namespace Eliza.Test
{
    [Collection("Base")]
    public class TestBaseSerialization
    {
        [Fact]
        public void TestNoRepeatedFieldsFromInheritance()
        {
            // This is based off of JP 107 data
            List<FieldInfo> list = new();
            IEnumerable<FieldInfo> orderedFields = BaseSerializer.GetFieldsOrdered(typeof(SeedItemData));
            foreach(FieldInfo fieldInfo in orderedFields) {
                list.Add(fieldInfo);
            }
            Assert.True(list.Count == 2);
            Assert.True(list[0].Name == "ItemId");
            Assert.True(list[1].Name == "LevelAmount");

        }
    }
}
