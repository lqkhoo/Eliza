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

    public class RF5SaveDataV106
    {
        public int slotNo;
        public SaveFlagStorage saveFlag;
        public RF5WorldData worldData;
        public RF5PlayerData playerData;
        public RF5EventDataV106 eventData;
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

        public RF5SaveData AdaptTo()
        {
            RF5SaveData rf5SaveData = new()
            {
                slotNo = this.slotNo,
                saveFlag = this.saveFlag,
                worldData = this.worldData,
                playerData = this.playerData,
                eventData = this.eventData.AdaptTo(),
                npcData = this.npcData,
                fishingData = this.fishingData,
                stampData = this.stampData,
                furnitureData = this.furnitureData,
                battleData = this.battleData,
                itemData = this.itemData,
                supportMonsterData = this.supportMonsterData,
                farmData = this.farmData,
                statusData = this.statusData,
                orderData = this.orderData,
                makingData = this.makingData,
                policeBatchData = this.policeBatchData,
                itemFlagData = this.itemFlagData,
                buildData = this.buildData,
                shippingData = this.shippingData,
                lpocketData = this.lpocketData,
                nameData = this.nameData
            };
            return rf5SaveData;
        }
        public RF5SaveDataV106 AdaptFrom(RF5SaveData rf5SaveData)
        {
            this.slotNo = rf5SaveData.slotNo;
            this.saveFlag = rf5SaveData.saveFlag;
            this.worldData = rf5SaveData.worldData;
            this.playerData = rf5SaveData.playerData;
            this.eventData = new RF5EventDataV106().AdaptFrom(rf5SaveData.eventData);
            this.npcData = rf5SaveData.npcData;
            this.fishingData = rf5SaveData.fishingData;
            this.stampData = rf5SaveData.stampData;
            this.furnitureData = rf5SaveData.furnitureData;
            this.battleData = rf5SaveData.battleData;
            this.itemData = rf5SaveData.itemData;
            this.supportMonsterData = rf5SaveData.supportMonsterData;
            this.farmData = rf5SaveData.farmData;
            this.statusData = rf5SaveData.statusData;
            this.orderData = rf5SaveData.orderData;
            this.makingData = rf5SaveData.makingData;
            this.policeBatchData = rf5SaveData.policeBatchData;
            this.itemFlagData = rf5SaveData.itemFlagData;
            this.buildData = rf5SaveData.buildData;
            this.shippingData = rf5SaveData.shippingData;
            this.lpocketData = rf5SaveData.lpocketData;
            this.nameData = rf5SaveData.nameData;
            return this;
        }

    }

    public class RF5SaveDataV102
    {
        public int slotNo;
        public SaveFlagStorage saveFlag;
        public RF5WorldData worldData;
        public RF5PlayerData playerData;
        public RF5EventDataV106 eventData;
        public RF5NPCData npcData;
        public RF5FishingData fishingData;
        public RF5StampData stampData;
        public RF5FurnitureData furnitureData;
        public RF5BattleData battleData;
        public RF5ItemData itemData;
        public RF5FarmSupportMonsterData supportMonsterData;
        public RF5FarmData farmData;
        public RF5StatusDataV102 statusData;
        public RF5OrderData orderData;
        public RF5MakingData makingData;
        public RF5PoliceBatchData policeBatchData;
        public RF5ItemFlagData itemFlagData;
        public RF5BuildData buildData;
        public RF5ShippingData shippingData;
        public RF5LpocketData lpocketData;
        public RF5NameData nameData;

        public RF5SaveData AdaptTo()
        {
            RF5SaveData rf5SaveData = new()
            {
                slotNo = this.slotNo,
                saveFlag = this.saveFlag,
                worldData = this.worldData,
                playerData = this.playerData,
                eventData = this.eventData.AdaptTo(),
                npcData = this.npcData,
                fishingData = this.fishingData,
                stampData = this.stampData,
                furnitureData = this.furnitureData,
                battleData = this.battleData,
                itemData = this.itemData,
                supportMonsterData = this.supportMonsterData,
                farmData = this.farmData,
                statusData = this.statusData.AdaptTo(),
                orderData = this.orderData,
                makingData = this.makingData,
                policeBatchData = this.policeBatchData,
                itemFlagData = this.itemFlagData,
                buildData = this.buildData,
                shippingData = this.shippingData,
                lpocketData = this.lpocketData,
                nameData = this.nameData
            };
            return rf5SaveData;
        }
        public RF5SaveDataV102 AdaptFrom(RF5SaveData rf5SaveData)
        {
            this.slotNo = rf5SaveData.slotNo;
            this.saveFlag = rf5SaveData.saveFlag;
            this.worldData = rf5SaveData.worldData;
            this.playerData = rf5SaveData.playerData;
            this.eventData = new RF5EventDataV106().AdaptFrom(rf5SaveData.eventData);
            this.npcData = rf5SaveData.npcData;
            this.fishingData = rf5SaveData.fishingData;
            this.stampData = rf5SaveData.stampData;
            this.furnitureData = rf5SaveData.furnitureData;
            this.battleData = rf5SaveData.battleData;
            this.itemData = rf5SaveData.itemData;
            this.supportMonsterData = rf5SaveData.supportMonsterData;
            this.farmData = rf5SaveData.farmData;
            this.statusData = new RF5StatusDataV102().AdaptFrom(rf5SaveData.statusData);
            this.orderData = rf5SaveData.orderData;
            this.makingData = rf5SaveData.makingData;
            this.policeBatchData = rf5SaveData.policeBatchData;
            this.itemFlagData = rf5SaveData.itemFlagData;
            this.buildData = rf5SaveData.buildData;
            this.shippingData = rf5SaveData.shippingData;
            this.lpocketData = rf5SaveData.lpocketData;
            this.nameData = rf5SaveData.nameData;
            return this;
        }

    }

}
