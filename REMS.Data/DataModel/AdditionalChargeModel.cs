﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.DataModel
{
    public class AdditionalChargeModel
    {
        public int AdditionalChargeID { get; set; }
        public Nullable<int> TowerID { get; set; }
        public string Name { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string Unit { get; set; }
        public string ChargeType { get; set; }
        public Nullable<System.DateTime> CrDate { get; set; }
        public string CrBy { get; set; }
    }
}