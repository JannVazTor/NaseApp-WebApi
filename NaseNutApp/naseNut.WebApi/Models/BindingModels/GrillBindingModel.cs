using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace naseNut.WebApi.Models.BindingModels
{
    public class AddGrillBindingModel
    {
        [Required]
        public DateTime DateCapture { get; set; }
        [Required]
        public int ReceptionId { get; set; }
        public int Size { get; set; }
        [Required]
        public int Sacks { get; set; }
        [Required]
        public double Kilos { get; set; }
        [Required]
        public int Quality { get; set; }
        [Required]
        public string Variety { get; set; }
        [Required]
        public string Producer { get; set; }
        [Required]
        public string FieldName { get; set; }
        [Required]
        public string CylinderName { get; set; }
        public int NutAmount { get; set; }
        public double SelectPercentage { get; set; }
        public double TotalPercentage { get; set; }
        public double Humidity { get; set; }
    }
}