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
    
    public partial class Humidity
    {
        public int Id { get; set; }
        public System.DateTime DateCapture { get; set; }
        public double HumidityPercent { get; set; }
        public int ReceptionEntryId { get; set; }
    
        public virtual ReceptionEntry ReceptionEntry { get; set; }
    }
}
