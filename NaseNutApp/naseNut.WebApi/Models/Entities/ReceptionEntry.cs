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
    
    public partial class ReceptionEntry
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ReceptionEntry()
        {
            this.Humidities = new HashSet<Humidity>();
            this.Samplings = new HashSet<Sampling>();
            this.Receptions = new HashSet<Reception>();
            this.NutTypes = new HashSet<NutType>();
        }
    
        public int Id { get; set; }
        public DateTime DateEntry { get; set; }
        public int VarietyId { get; set; }
        public int ProducerId { get; set; }
        public int CylinderId { get; set; }
        public DateTime? DateIssue { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Humidity> Humidities { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sampling> Samplings { get; set; }
        public virtual Cylinder Cylinder { get; set; }
        public virtual Producer Producer { get; set; }
        public virtual Variety Variety { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reception> Receptions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NutType> NutTypes { get; set; }
    }
}
