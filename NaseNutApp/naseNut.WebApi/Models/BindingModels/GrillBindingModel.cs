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
        public System.DateTime DateCapture { get; set; }
        [Required]
        public int ReceptionId { get; set; }
        [Required]
        public int Size { get; set; }
        [Required]
        public int Sacks { get; set; }
        [Required]
        public double Kilos { get; set; }
    }
}