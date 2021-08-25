﻿using MessagePack;
using Eliza.Core.Serialization.MessagePackFormatters;

namespace Eliza.Model.Farm
{

    [MessagePackObject]
    [MessagePackFormatter(typeof(FarmSizeLevelFormatter))]
    public class FarmSizeLevel
    {
        [Key(0)]
        public int Level;
    }
}
