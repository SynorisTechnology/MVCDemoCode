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
    
    public partial class vw_SaleFlat
    {
        public int SaleID { get; set; }
        public Nullable<int> FlatID { get; set; }
        public string Aggrement { get; set; }
        public Nullable<System.DateTime> SaleDate { get; set; }
        public Nullable<decimal> SaleRate { get; set; }
        public string SaleRateInWords { get; set; }
        public Nullable<decimal> ServiceTaxPer { get; set; }
        public Nullable<decimal> ServiceTaxAmount { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public string TotalAmtInWords { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public int FloorID { get; set; }
        public string FlatType { get; set; }
        public string FlatName { get; set; }
        public string FlatNo { get; set; }
        public Nullable<decimal> FlatSize { get; set; }
        public string FlatSizeUnit { get; set; }
        public Nullable<decimal> SalePrice { get; set; }
        public string Expr1 { get; set; }
        public int Expr2 { get; set; }
        public Nullable<int> TowerID { get; set; }
        public Nullable<int> FloorNo { get; set; }
        public string FloorName { get; set; }
        public int Expr3 { get; set; }
        public Nullable<int> ProjectID { get; set; }
        public Nullable<int> ProjectTypeID { get; set; }
        public string TowerName { get; set; }
        public string TowerNo { get; set; }
        public string Block { get; set; }
    }
}
