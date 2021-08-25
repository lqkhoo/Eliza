using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagePack;
using MessagePack.Formatters;
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
