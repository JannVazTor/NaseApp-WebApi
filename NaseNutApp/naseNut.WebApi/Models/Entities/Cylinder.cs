//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace naseNut.WebApi.Models.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Cylinder
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cylinder()
        {
            this.ReceptionEntries = new HashSet<ReceptionEntry>();
        }
    
        public int Id { get; set; }
        public string CylinderName { get; set; }
        public bool Active { get; set; }
        public int HarvestSeasonId { get; set; }
    
        public virtual HarvestSeason HarvestSeason { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReceptionEntry> ReceptionEntries { get; set; }
    }
}
