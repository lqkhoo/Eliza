using MessagePack;

namespace Eliza.Model.Event
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class IntArray
    {
        public int[] datas;
    }

}
