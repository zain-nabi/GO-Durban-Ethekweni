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
    
    public partial class tbl_AssociationRegion
    {
        public int AssociationRegionID { get; set; }
        public Nullable<int> AssociationID { get; set; }
        public Nullable<int> RegionID { get; set; }
    
        public virtual Association Association { get; set; }
        public virtual Region Region { get; set; }
    }
}