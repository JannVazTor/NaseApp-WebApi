using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace naseNut.WebApi.Models.BindingModels
{
    public class AddRemissionBindingModel
    {
        [Required]
        public string Cultivation { get; set; }
        [Required]
        public string Batch { get; set; }
        [Required]
        public double Quantity { get; set; }
        [Required]
        public string Butler { get; set; }
        [Required]
        public int TransportNumber { get; set; }
        [Required]
        public string Driver { get; set; }
        [Required]
        public string Elaborate { get; set; }
        [Required]
        public int ReceptionId { get; set; }
    }
    public class UpdateRemissionBindingModel
    {
        [Required]
        public string Cultivation { get; set; }
        [Required]
        public string Batch { get; set; }
        [Required]
        public double Quantity { get; set; }
        [Required]
        public string Butler { get; set; }
        [Required]
        public int TransportNumber { get; set; }
        [Required]
        public string Driver { get; set; }
        [Required]
        public string Elaborate { get; set; }
        [Required]
        public DateTime DateCapture { get; set; } = DateTime.Now;


    }
}