using System.Collections.Generic;
using static Eliza.Core.Serialization.ElizaAttribute;

namespace Eliza.Model.Lpocket
{
    public class RF5LpocketData
    {
        public bool SyncToCampEquip;
        public int QuickLastFocus;
        public int CampLastFocus;
        [ElizaSize(Fixed = 38)]
        public List<int> Unk;
        //public List<int> UIEMCategoriesCustomNo;
        //public List<int> UIEMQuckCategories;
    }
}