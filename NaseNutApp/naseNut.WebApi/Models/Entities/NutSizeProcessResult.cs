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
    
    public partial class NutSizeProcessResult
    {
        public int Id { get; set; }
        public Nullable<int> NutSize { get; set; }
        public Nullable<int> Sacks { get; set; }
        public Nullable<int> NutTypeId { get; set; }
    
        public virtual NutType NutType { get; set; }
    }
}