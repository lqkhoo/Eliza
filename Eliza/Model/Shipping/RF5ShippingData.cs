using System.Collections.Generic;
using static Eliza.Core.Serialization.ElizaFlowControlAttribute;

namespace Eliza.Model.Shipping
{
    public class RF5ShippingData
    {
        public int completedPercent;
        public long totalIncome;

        [ElizaList(IsMessagePackList = true)]
        public List<ShipmentItemRecords> ItemRecordList1;

        [ElizaList(IsMessagePackList = true)]
        public List<ShipmentItemRecords> ItemRecordList2;

        [ElizaList(IsMessagePackList = true)]
        public List<ShipmentItemRecords> ItemRecordList3;

        [ElizaList(IsMessagePackList = true)]
        public List<ShipmentItemRecords> ItemRecordList4;

        [ElizaList(IsMessagePackList = true)]
        public List<ShipmentItemRecords> ItemRecordList5;

        [ElizaList(IsMessagePackList = true)]
        public List<ShipmentItemRecords> ItemRecordList6;

        [ElizaList(IsMessagePackList = true)]
        public List<ShipmentItemRecords> ItemRecordList7;

        [ElizaList(IsMessagePackList = true)]
        public List<ShipmentItemRecords> ItemRecordList8;

        [ElizaList(IsMessagePackList = true)]
        public List<FishShipmentRecords> FishRecordList;

        [ElizaList(IsMessagePackList = true)]
        public List<SeedLevelRecords> SeedLevelRecordList;

    }
}