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
    
    public partial class RoleAccess
    {
        public int RoleAccessID { get; set; }
        public string ModuleListID { get; set; }
        public string RoleName { get; set; }
        public bool IsRead { get; set; }
        public Nullable<System.DateTime> AssignDate { get; set; }
        public string AssignUser { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
    }
}
