using MessagePack;

namespace Eliza.Model.Farm
{
    [MessagePackObject]
    public class FriendMonsterIDAndHouseID : KeyAndValue<int, uint> { }
}