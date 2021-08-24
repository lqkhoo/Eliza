﻿using MessagePack;

namespace Eliza.Model.Item
{
    [MessagePackObject]
    public class SynthesisItemData : NotAmountItemData
    {
        [Key(2)]
        public int[] SourceItems;
    }
}