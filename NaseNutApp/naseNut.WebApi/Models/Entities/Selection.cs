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
    
    public partial class Selection
    {
        public int Id { get; set; }
        public System.DateTime Date { get; set; }
        public int First { get; set; }
        public int Second { get; set; }
        public int Third { get; set; }
        public int Broken { get; set; }
        public int Germinated { get; set; }
        public int Vanas { get; set; }
        public int WithNut { get; set; }
        public double NutColor { get; set; }
        public double NutPerformance { get; set; }
        public double GerminationStart { get; set; }
        public double SampleWeight { get; set; }
        public double NutsNumber { get; set; }
        public double Humidity { get; set; }
    
        public virtual Reception Reception { get; set; }
    }
}