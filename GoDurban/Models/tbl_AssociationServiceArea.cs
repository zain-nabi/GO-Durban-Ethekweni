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
    
    public partial class tbl_AssociationServiceArea
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_AssociationServiceArea()
        {
            this.tbl_OwnerServiceArea = new HashSet<tbl_OwnerServiceArea>();
        }
    
        public int AssociationServiceAreaID { get; set; }
        public Nullable<int> AssociationID { get; set; }
        public Nullable<int> RegionSAreaID { get; set; }
    
        public virtual Association Association { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_OwnerServiceArea> tbl_OwnerServiceArea { get; set; }
    }
}
