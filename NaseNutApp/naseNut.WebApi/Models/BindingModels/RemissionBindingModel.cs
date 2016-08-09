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
        public double Quantity { get; set; }
        [Required]
        public string Butler { get; set; }
        [Required]
        public int TransportNumber { get; set; }
        [Required]
        public string Driver { get; set; }
        [Required]
        public string Elaborate { get; set; }
    }
}