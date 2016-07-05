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
    
    public partial class Reception
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Reception()
        {
            this.Remissions = new HashSet<Remission>();
            this.Cylinders = new HashSet<Cylinder>();
            this.Grills = new HashSet<Grill>();
            this.Humidities = new HashSet<Humidity>();
        }
    
        public int Id { get; set; }
        public string Variety { get; set; }
        public double ReceivedFromField { get; set; }
        public string FieldName { get; set; }
        public string CarRegistration { get; set; }
        public System.DateTime EntryDate { get; set; }
        public Nullable<System.DateTime> IssueDate { get; set; }
        public Nullable<double> HeatHoursDtrying { get; set; }
        public Nullable<double> HumidityPercent { get; set; }
        public string Observations { get; set; }
        public int ProducerId { get; set; }
        public Nullable<int> GrillId { get; set; }
        public int Folio { get; set; }
    
        public virtual Producer Producer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Remission> Remissions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cylinder> Cylinders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Grill> Grills { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Humidity> Humidities { get; set; }
    }
}
