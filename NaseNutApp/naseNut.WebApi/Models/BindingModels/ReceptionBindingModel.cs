using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace naseNut.WebApi.Models.BindingModels
{
    public class AddReceptionBindingModel
    {
        [Required]
        public string Variety { get; set; }
        [Required]
        public double ReceivedFromField { get; set; }
        [Required]
        public string CylinderNumber { get; set; }
        [Required]
        public string FieldName { get; set; }
        [Required]
        public string CarRegistration { get; set; }
        [Required]
        public DateTime EntryDate { get; set; }
        public DateTime? IssueDate { get; set; }
        public double? HeatHoursDtrying { get; set; }
        public double? HumidityPercent { get; set; }
        [Required]
        public string Observations { get; set; }
        [Required]
        public int ProducerId { get; set; }
    }
}