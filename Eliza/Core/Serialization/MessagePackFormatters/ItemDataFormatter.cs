﻿using Eliza.Model.Item;
using MessagePack;
using MessagePack.Formatters;
using System.Collections.Generic;

namespace Eliza.Core.Serialization.MessagePackFormatters
{
    public class ItemDataFormatter : IMessagePackFormatter<ItemData>
    {

        public ItemData Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            IFormatterResolver resolver = options.Resolver;
            options.Security.DepthStep(ref reader);

            if (reader.TryReadNil()) {
                return null;
            } else {
                //Key, Value
                var d = reader.ReadArrayHeader();
                var id = reader.ReadByte();
                reader.ReadArrayHeader();
                switch (id) {
                    //AmountItemData
                    case 0: {
                            var itemData = new AmountItemData();
                            var list = new List<int>();

                            itemData.ItemId = reader.ReadInt32();

                            //AmountItemDataBase
                            var length = reader.ReadArrayHeader();

                            for (int i = 0; i < length; i++) {
                                list.Add(reader.ReadInt32());
                            }

                            itemData.LevelAmount = list;

                            return itemData;
                        }
                    //SeedItemData
                    case 1: {
                            var itemData = new SeedItemData();
                            var list = new List<int>();

                            itemData.ItemId = reader.ReadInt32();

                            //AmountItemData
                            var length = reader.ReadArrayHeader();

                            for (int i = 0; i < length; i++) {
                                list.Add(reader.ReadInt32());
                            }

                            itemData.LevelAmount = list;

                            return itemData;
                        }
                    //FoodItemData
                    case 2: {
                            var itemData = new FoodItemData();

                            itemData.ItemId = reader.ReadInt32();

                            //NotAmountItemData
                            itemData.Level = reader.ReadInt32();

                            //SynthesisItemData
                            var length = reader.ReadArrayHeader();
                            var list = new int[length];

                            for (int i = 0; i < length; i++) {
                                list[i] = reader.ReadInt32();
                            }

                            itemData.SourceItems = list;

                            //FoodItemData
                            itemData.IsArrange = reader.ReadBoolean();

                            return itemData;
                        }
                    //EquipItemData
                    case 3: {
                            var itemData = new EquipItemData();

                            itemData.ItemId = reader.ReadInt32();

                            //NotAmountData
                            itemData.Level = reader.ReadInt32();

                            //SynthesisItemData
                            var length = reader.ReadArrayHeader();
                            var list = new int[length];

                            for (int i = 0; i < length; i++) {
                                list[i] = reader.ReadInt32();
                            }

                            itemData.SourceItems = list;

                            //EquipItemData
                            length = reader.ReadArrayHeader();
                            var addedItems = new int[length];

                            for (int i = 0; i < length; i++) {
                                addedItems[i] = reader.ReadInt32();
                            }

                            itemData.AddedItems = addedItems;

                            length = reader.ReadArrayHeader();
                            var arrangeItems = new int[length];

                            for (int i = 0; i < length; i++) {
                                arrangeItems[i] = reader.ReadInt32();
                            }

                            itemData.ArrangeItems = arrangeItems;

                            itemData.ArrangeOverride = reader.ReadInt32();
                            itemData.BaseLevel = reader.ReadInt32();
                            itemData.SozaiLevel = reader.ReadInt32();
                            itemData.DualWorkSmithBonusType = reader.ReadInt32();
                            itemData.DualWorkLoveLevel = reader.ReadInt32();
                            itemData.DualWorkActor = reader.ReadInt32();
                            itemData.DualWorkParam = reader.ReadInt32();

                            return itemData;
                        }
                    //RuneAbilityItemData
                    case 4: {
                            var itemData = new RuneAbilityItemData();

                            itemData.ItemId = reader.ReadInt32();

                            //NotAmountItemData
                            itemData.Level = reader.ReadInt32();

                            return itemData;
                        }
                    //FishItemData
                    case 5: {
                            var itemData = new FishItemData();

                            itemData.ItemId = reader.ReadInt32();

                            //NotAmountItemData
                            itemData.Level = reader.ReadInt32();

                            //FishItemData
                            itemData.Size = reader.ReadInt32();

                            return itemData;
                        }
                    //PotToolItemData
                    case 6: {
                            var itemData = new PotToolItemData();

                            itemData.ItemId = reader.ReadInt32();

                            //EquipItemData

                            //NotAmountData
                            itemData.Level = reader.ReadInt32();

                            //SynthesisItemData
                            var length = reader.ReadArrayHeader();
                            var list = new int[length];

                            for (int i = 0; i < length; i++) {
                                list[i] = reader.ReadInt32();
                            }

                            itemData.SourceItems = list;

                            //EquipItemDataBase
                            length = reader.ReadArrayHeader();
                            var addedItems = new int[length];

                            for (int i = 0; i < length; i++) {
                                addedItems[i] = reader.ReadInt32();
                            }

                            itemData.AddedItems = addedItems;

                            length = reader.ReadArrayHeader();
                            var arrangeItems = new int[length];

                            for (int i = 0; i < length; i++) {
                                arrangeItems[i] = reader.ReadInt32();
                            }

                            itemData.ArrangeItems = arrangeItems;

                            itemData.ArrangeOverride = reader.ReadInt32();
                            itemData.BaseLevel = reader.ReadInt32();
                            itemData.SozaiLevel = reader.ReadInt32();
                            itemData.DualWorkSmithBonusType = reader.ReadInt32();
                            itemData.DualWorkLoveLevel = reader.ReadInt32();
                            itemData.DualWorkActor = reader.ReadInt32();
                            itemData.DualWorkParam = reader.ReadInt32();

                            //PotToolItemData
                            itemData.Capacity = reader.ReadInt32();

                            return itemData;
                        }
                    default: return null;
                }
            }
        }

        public void Serialize(ref MessagePackWriter writer, ItemData value, MessagePackSerializerOptions options)
        {
            IFormatterResolver resolver = options.Resolver;
            if (value == null) {
                writer.WriteNil();
            } else {
                writer.WriteArrayHeader(2);
                // It's reversed due to for example seedItemData deriving amountItemData
                switch (value) {
                    //6
                    case PotToolItemData itemData: {
                            //Key
                            writer.WriteInt32(6); // These int32 writes are the whole point here.
                            //Items
                            writer.WriteArrayHeader(13);

                            writer.Write(itemData.ItemId);

                            //NotAmountData
                            writer.Write(itemData.Level);

                            //SynthesisItemData
                            writer.WriteArrayHeader(itemData.SourceItems.Length);
                            foreach (var item in itemData.SourceItems) {
                                writer.Write(item);
                            }

                            //EquipItemDataBase
                            writer.WriteArrayHeader(itemData.AddedItems.Length);
                            foreach (var item in itemData.AddedItems) {
                                writer.Write(item);
                            }

                            writer.WriteArrayHeader(itemData.ArrangeItems.Length);
                            foreach (var item in itemData.ArrangeItems) {
                                writer.Write(item);
                            }

                            writer.Write(itemData.ArrangeOverride);
                            writer.Write(itemData.BaseLevel);
                            writer.Write(itemData.SozaiLevel);
                            writer.Write(itemData.DualWorkSmithBonusType);
                            writer.Write(itemData.DualWorkLoveLevel);
                            writer.Write(itemData.DualWorkActor);
                            writer.Write(itemData.DualWorkParam);

                            //PotToolItemDataBase
                            writer.Write(itemData.Capacity);
                            break;
                        }
                    //5
                    case FishItemData itemData: {
                            //Key
                            writer.WriteInt32(5);
                            //Items
                            writer.WriteArrayHeader(3);

                            writer.Write(itemData.ItemId);

                            //NotAmountData
                            writer.Write(itemData.Level);

                            //FishItemDataBase
                            writer.Write(itemData.Size);
                            break;
                        }
                    //4
                    case RuneAbilityItemData itemData: {
                            //Key
                            writer.WriteInt32(4);
                            //Items
                            writer.WriteArrayHeader(2);

                            writer.Write(itemData.ItemId);

                            //NotAmountData
                            writer.Write(itemData.Level);
                            break;
                        }
                    //3
                    case EquipItemData itemData: {
                            //Key
                            writer.WriteInt32(3);
                            //Items
                            writer.WriteArrayHeader(12);

                            writer.Write(itemData.ItemId);

                            //NotAmountData
                            writer.Write(itemData.Level);

                            //SynthesisItemData
                            writer.WriteArrayHeader(itemData.SourceItems.Length);
                            foreach (var item in itemData.SourceItems) {
                                writer.Write(item);
                            }

                            //EquipItemDataBase
                            writer.WriteArrayHeader(itemData.AddedItems.Length);
                            foreach (var item in itemData.AddedItems) {
                                writer.Write(item);
                            }

                            writer.WriteArrayHeader(itemData.ArrangeItems.Length);
                            foreach (var item in itemData.ArrangeItems) {
                                writer.Write(item);
                            }

                            writer.Write(itemData.ArrangeOverride);
                            writer.Write(itemData.BaseLevel);
                            writer.Write(itemData.SozaiLevel);
                            writer.Write(itemData.DualWorkSmithBonusType);
                            writer.Write(itemData.DualWorkLoveLevel);
                            writer.Write(itemData.DualWorkActor);
                            writer.Write(itemData.DualWorkParam);
                            break;
                        }
                    //2
                    case FoodItemData itemData: {
                            //Key
                            writer.WriteInt32(2);
                            //Items
                            writer.WriteArrayHeader(4);

                            writer.Write(itemData.ItemId);

                            //NotAmountItemData
                            writer.Write(itemData.Level);

                            //SynthesisItemData
                            writer.WriteArrayHeader(itemData.SourceItems.Length);
                            foreach (var item in itemData.SourceItems) {
                                writer.Write(item);
                            }

                            //FoodItemData
                            writer.Write(itemData.IsArrange);
                            break;
                        }
                    //1
                    case SeedItemData itemData: {
                            //Key
                            writer.WriteInt32(1);
                            //Items
                            writer.WriteArrayHeader(2);

                            writer.Write(itemData.ItemId);
                            writer.WriteArrayHeader(itemData.LevelAmount.Count);

                            foreach (var item in itemData.LevelAmount) {
                                writer.Write(item);
                            }
                            break;
                        }
                    // 0
                    case AmountItemData itemData: {
                            //Key
                            writer.WriteInt32(0);
                            //Items
                            writer.WriteArrayHeader(2);

                            writer.Write(itemData.ItemId);
                            writer.WriteArrayHeader(itemData.LevelAmount.Count);
                            foreach (var item in itemData.LevelAmount) {
                                writer.Write(item);
                            }
                            break;
                        }
                }
            }
        }
    }
}
