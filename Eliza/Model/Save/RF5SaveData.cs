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
using Eliza.Model.Shipping;
using Eliza.Model.Stamp;
using Eliza.Model.Status;
using Eliza.Model.World;

namespace Eliza.Model.Save
{
    public class RF5SaveData
    {
        public int slotNo;
        public SaveFlagStorage saveFlag;
        public RF5WorldData worldData;
        public RF5PlayerData playerData;
        public RF5EventData eventData;
        public RF5NPCData npcData;
        public RF5FishingData fishingData;
        public RF5StampData stampData;
        public RF5FurnitureData furnitureData;
        public RF5BattleData battleData;
        public RF5ItemData itemData;
        public RF5FarmSupportMonsterData supportMonsterData;
        public RF5FarmData farmData;
        public RF5StatusData statusData;
        public RF5OrderData orderData;
        public RF5MakingData makingData;
        public RF5PoliceBatchData policeBatchData;
        public RF5ItemFlagData itemFlagData;
        public RF5BuildData buildData;
        public RF5ShippingData shippingData;
        public RF5LpocketData lpocketData;
        public RF5NameData nameData;
    }

}
