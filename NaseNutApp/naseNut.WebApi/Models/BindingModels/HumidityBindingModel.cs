using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace naseNut.WebApi.Models.BindingModels
{
    public class AddHumidityBindingModel
    {
        [Required]
        public double HumidityPercent { get; set; }
        [Required]
        public int CylinderId { get; set; }
        [Required]
        public int ReceptionId { get; set; }
    }
}