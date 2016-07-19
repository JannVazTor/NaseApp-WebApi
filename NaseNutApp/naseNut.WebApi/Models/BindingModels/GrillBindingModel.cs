using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace naseNut.WebApi.Models.BindingModels
{
    public class AddGrillBindingModel
    {
        [Required]
        public string DateCapture { get; set; }
        public int Size { get; set; }
        [Required]
        public int Sacks { get; set; }
        [Required]
        public double Kilos { get; set; }
        [Required]
        public int Quality { get; set; }
        [Required]
        public int VarietyId { get; set; }
        [Required]
        public int ProducerId { get; set; }
        [Required]
        public string FieldName { get; set; }
    }
}