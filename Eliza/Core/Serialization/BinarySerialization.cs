using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

using Eliza.Model;
using Eliza.Model.Battle;
using Eliza.Model.Build;
using Eliza.Model.Event;
using Eliza.Model.Farm;
using Eliza.Model.FarmSupportMonster;
using Eliza.Model.Fishing;
using Eliza.Model.Furniture;
using Eliza.Model.Item;
using Eliza.Model.ItemFlag;
using Eliza.Model.Lpocket;
using Eliza.Model.Making;
using Eliza.Model.Name;
using Eliza.Model.Npc;
using Eliza.Model.Order;
using Eliza.Model.Player;
using Eliza.Model.PoliceBatch;
using Eliza.Model.Save;
using Eliza.Model.Shipping;
using Eliza.Model.Stamp;
using Eliza.Model.Status;
using Eliza.Model.World;

namespace Eliza.Core.Serialization
{

    class BinarySerialization
    {
        public readonly Stream BaseStream;

        protected HashSet<Type> _DebugTypeSet = new() {
            typeof(RF5SaveDataHeader),
            typeof(SaveFlagStorage),
            typeof(RF5WorldData),
            typeof(RF5PlayerData),
            typeof(RF5EventData),
            typeof(RF5NPCData),
            typeof(RF5FishingData),
            typeof(RF5StampData),
            typeof(RF5FurnitureData),
            typeof(RF5BattleData),
            typeof(RF5ItemData),
            typeof(RF5FarmSupportMonsterData),
            typeof(RF5FarmData),
            typeof(RF5StatusData),
            typeof(RF5OrderData),
            typeof(RF5MakingData),
            typeof(RF5PoliceBatchData),
            typeof(RF5ItemFlagData),
            typeof(RF5BuildData),
            typeof(RF5ShippingData),
            typeof(RF5LpocketData),
            typeof(RF5NameData),
            typeof(RF5SaveDataFooter)
        };


        public BinarySerialization(Stream baseStream)
        {
            this.BaseStream = baseStream;
        }

        protected static bool IsList(Type type)
        {
            return typeof(IList).IsAssignableFrom(type);
        }

        protected static bool IsDictionary(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>);
        }

        // This returns all public fields of a class as a lazy enumerable,
        // starting from the base object. They're not ordered in the alphabetical sense.
        protected static IEnumerable<FieldInfo> GetFieldsOrdered(Type objectType)
        {

            Stack<Type> typeStack = new();

            do {
                typeStack.Push(objectType);
                objectType = objectType.BaseType;
            } while (objectType != null);

            while (typeStack.Count > 0) {
                objectType = typeStack.Pop();
                foreach (FieldInfo fieldInfo in objectType.GetFields()) {
                    // yield return fieldInfo;
                    if (! fieldInfo.IsDefined(typeof(CompilerGeneratedAttribute))) {
                        yield return fieldInfo;
                    }
                    
                }
            }

        }

    }
}
