﻿using System.Collections.Generic;
using Eliza.Core.Serialization;

namespace Eliza.Model.Lpocket
{
    public class RF5LpocketData
    {
        public bool SyncToCampEquip;
        public int QuickLastFocus;
        public int CampLastFocus;
        [ElizaList(FixedSize = 38)]
        public List<int> Unk;
        //public List<int> UIEMCategoriesCustomNo;
        //public List<int> UIEMQuckCategories;
    }
}