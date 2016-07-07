using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace naseNut.WebApi.Models.BindingModels
{
    public class SelectionBindingModel
    {
        public DateTime? Date { get; set; } = DateTime.Now;
        [Required]
        public int First { get; set; }
        [Required]
        public int Second { get; set; }
        [Required]
        public int Third { get; set; }
        [Required]
        public int Broken { get; set; }
        [Required]
        public int Germinated { get; set; }
        [Required]
        public int Vanas { get; set; }
        [Required]
        public int WithNut { get; set; }
        [Required]
        public double NutColor { get; set; }
        [Required]
        public double NutPerformance { get; set; }
        [Required]
        public double GerminationStart{ get; set; }
        [Required]
        public double SampleWeight{ get; set; }
        [Required]
        public double NutsNumber { get; set; }
        [Required]
        public double Humidity { get; set; }
    }
}