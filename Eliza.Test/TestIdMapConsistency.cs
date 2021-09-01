using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Eliza.Data;
using Eliza.Model.Item;

namespace Eliza.Test
{

    public class TestIdMapConsistency
    {
        [Fact]
        public void TestItemIdMappingConsistency()
        {
            int itemCount = Items.ItemIds.Count;
            Assert.True(Items.ItemIdToEnglishName.Count == itemCount, String.Format("Counts: {0} {1}", itemCount, Items.ItemIdToEnglishName.Count));
            Assert.True(Items.ItemIdToJapaneseName.Count == itemCount, String.Format("Counts: {0} {1}", itemCount, Items.ItemIdToJapaneseName.Count));
            Assert.True(Items.ItemIdToItemType.Count == itemCount, String.Format("Counts: {0} {1}", itemCount, Items.ItemIdToItemType.Count));

            // None-type item doesn't have an icon.
            Assert.True(Items.ItemIdToAssemblyResourceUri.Count == itemCount, String.Format("Counts: {0} {1}", itemCount, Items.ItemIdToAssemblyResourceUri.Count));

            foreach (int itemId in Items.ItemIds) {
                Assert.True(Items.ItemIdToEnglishName.ContainsKey(itemId));
                Assert.True(Items.ItemIdToJapaneseName.ContainsKey(itemId));
                Assert.True(Items.ItemIdToItemType.ContainsKey(itemId));
            }

        }

        [Fact]
        public void TestItemTypeMappingConsistency()
        {
            int itemCount = Items.ItemIds.Count;
            Assert.True(itemCount ==
                Items.IsItemData.Count
                + Items.IsAmountItemData.Count
                + Items.IsSeedItemData.Count
                + Items.IsEquipItemData.Count
                + Items.IsFishItemData.Count
                + Items.IsFoodItemData.Count
                + Items.IsPotToolItemData.Count
                + Items.IsRuneAbilityItemData.Count
                );

            Dictionary<Type, HashSet<int>> testMap = new() {
                { typeof(ItemData), Items.IsItemData },
                { typeof(AmountItemData), Items.IsAmountItemData },
                { typeof(SeedItemData), Items.IsSeedItemData },
                { typeof(EquipItemData), Items.IsEquipItemData },
                { typeof(FishItemData), Items.IsFishItemData },
                { typeof(FoodItemData), Items.IsFoodItemData },
                { typeof(PotToolItemData), Items.IsPotToolItemData },
                { typeof(RuneAbilityItemData), Items.IsRuneAbilityItemData }
            };

            foreach(int itemId in Items.ItemIdToItemType.Keys) {
                Type itemType = Items.ItemIdToItemType[itemId];
                HashSet<int> map = testMap[itemType];
                Assert.Contains(itemId, map);
            };
        }
    }
}
