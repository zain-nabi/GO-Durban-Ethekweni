//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GoDurban.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_UserPage
    {
        public int UserPageID { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<bool> Association { get; set; }
        public Nullable<bool> AssociationL { get; set; }
        public Nullable<bool> AssociationR { get; set; }
        public Nullable<bool> AssociationSA { get; set; }
        public Nullable<bool> Owner { get; set; }
        public Nullable<bool> OwnerA { get; set; }
        public Nullable<bool> OwnerSA { get; set; }
        public Nullable<bool> Region { get; set; }
        public Nullable<bool> RegionL { get; set; }
        public Nullable<bool> RegionSA { get; set; }
        public Nullable<bool> Driver { get; set; }
        public Nullable<bool> Vehicle { get; set; }
    
        public virtual tbl_User tbl_User { get; set; }
    }
}
