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
    
    public partial class NutSize
    {
        public int Id { get; set; }
        public int Small { get; set; }
        public int MediumStart { get; set; }
        public int MediumEnd { get; set; }
        public int LargeStart { get; set; }
        public int LargeEnd { get; set; }
    
        public virtual Variety Variety { get; set; }
    }
}
