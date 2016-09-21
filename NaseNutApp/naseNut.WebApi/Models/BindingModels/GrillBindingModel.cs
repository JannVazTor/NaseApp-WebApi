using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace naseNut.WebApi.Models.BindingModels
{
    public class AddOrUpdateGrillBindingModel
    {
        [Required]
        public DateTime DateCapture { get; set; }
        public int Folio { get; set; }
        [Required]
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
    }
}