using Eliza.Model.Item;
using MessagePack;

namespace Eliza.Model.Status
{
    [MessagePackObject]
    public class HumanEquip
    {
        [Key(0)]
        public ItemData[] EquipItems;
        //[IgnoreMember]
        //private EquipSlotType _CurrentWeaponSlot;
    }
}
