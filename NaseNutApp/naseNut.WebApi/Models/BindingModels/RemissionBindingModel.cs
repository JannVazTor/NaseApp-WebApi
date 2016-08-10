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
        public DateTime DateCapture { get; set; }
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
        public int Folio { get; set; }
        [Required]
        public int FieldId { get; set; }
        [Required]
        public int BatchId { get; set; }
        [Required]
        public int BoxId { get; set; }
        [Required]
        public int RemissionFolio { get; set; }
    }
    public class UpdateRemissionBindingModel
    {
        [Required]
        public string DateCapture { get; set; }
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
        public int FieldId { get; set; }
        [Required]
        public int BatchId { get; set; }
        [Required]
        public int BoxId { get; set; }
    }
}