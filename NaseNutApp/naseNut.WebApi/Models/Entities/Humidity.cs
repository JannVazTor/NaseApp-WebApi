//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
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
        public int CylinderId { get; set; }
        public int ReceptionId { get; set; }
    
        public virtual Cylinder Cylinder { get; set; }
        public virtual Reception Reception { get; set; }
    }
}
