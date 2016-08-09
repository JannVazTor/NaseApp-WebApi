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
    
    public partial class Sampling
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sampling()
        {
            this.NutTypes = new HashSet<NutType>();
        }
    
        public int Id { get; set; }
        public System.DateTime DateCapture { get; set; }
        public double SampleWeight { get; set; }
        public double HumidityPercent { get; set; }
        public int WalnutNumber { get; set; }
        public double Performance { get; set; }
        public double TotalWeightOfEdibleNuts { get; set; }
        public Nullable<int> GrillId { get; set; }
        public Nullable<int> ReceptionEntryId { get; set; }
    
        public virtual ReceptionEntry ReceptionEntry { get; set; }
        public virtual Grill Grill { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NutType> NutTypes { get; set; }
    }
}
