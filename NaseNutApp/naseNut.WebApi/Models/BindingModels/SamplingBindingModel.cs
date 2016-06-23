using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace naseNut.WebApi.Models.BindingModels
{
    public class AddSamplingBindingModel
    {
        [Required]
        public int GrillId { get; set; }
        [Required]
        public DateTime DateCapture { get; set; }
        [Required]
        public double SampleWeight { get; set; }
        [Required]
        public double HumidityPercent { get; set; }
        [Required]
        public int WalnutNumber { get; set; }
        [Required]
        public double Performance { get; set; }
        [Required]
        public double TotalWeightOfEdibleNuts { get; set; }
    }
}