//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace REMS.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class FlatInstallmentDetail
    {
        public int InstallmentID { get; set; }
        public Nullable<int> SaleID { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public string TotalAmtInWords { get; set; }
        public Nullable<int> RecordStatus { get; set; }
        public Nullable<int> EventID { get; set; }
        public Nullable<int> InstallmentOrder { get; set; }
        public string Activity { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public string UserName { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
    
        public virtual EventMaster EventMaster { get; set; }
        public virtual SaleFlat SaleFlat { get; set; }
    }
}
