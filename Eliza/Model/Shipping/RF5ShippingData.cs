using System.Collections.Generic;
using static Eliza.Core.Serialization.ElizaAttribute;

namespace Eliza.Model.Shipping
{
    public class RF5ShippingData
    {
        public int completedPercent;
        public long totalIncome;
        [ElizaMessagePackList]
        //List<ShipmentItemRecords>[]
        public List<ShipmentItemRecords> ItemRecordList1;
        [ElizaMessagePackList]
        public List<ShipmentItemRecords> ItemRecordList2;
        [ElizaMessagePackList]
        public List<ShipmentItemRecords> ItemRecordList3;
        [ElizaMessagePackList]
        public List<ShipmentItemRecords> ItemRecordList4;
        [ElizaMessagePackList]
        public List<ShipmentItemRecords> ItemRecordList5;
        [ElizaMessagePackList]
        public List<ShipmentItemRecords> ItemRecordList6;
        [ElizaMessagePackList]
        public List<ShipmentItemRecords> ItemRecordList7;
        [ElizaMessagePackList]
        public List<ShipmentItemRecords> ItemRecordList8;
        [ElizaMessagePackList]
        public List<FishShipmentRecords> FishRecordList;
        [ElizaMessagePackList]
        public List<SeedLevelRecords> SeedLevelRecordList;

    }
}