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
    
    public partial class PropertyTransfer
    {
        public int PropertyTransferID { get; set; }
        public Nullable<int> SaleID { get; set; }
        public Nullable<int> OldCustomerID { get; set; }
        public Nullable<int> NewCustomerID { get; set; }
        public Nullable<System.DateTime> TransferDate { get; set; }
        public Nullable<decimal> TransferAmount { get; set; }
        public string OldPlanType { get; set; }
        public string NewPlanType { get; set; }
        public string UserName { get; set; }
    
        public virtual SaleFlat SaleFlat { get; set; }
    }
}
