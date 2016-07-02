using System;
using System.ComponentModel.DataAnnotations;

namespace naseNut.WebApi.Models.BindingModels
{
    public class AddReceptionBindingModel
    {
        [Required]
        public string Variety { get; set; }
        [Required]
        public int Folio { get; set; }
        [Required]
        public double ReceivedFromField { get; set; }
        [Required]
        public int CylinderId { get; set; }
        [Required]
        public string FieldName { get; set; }
        [Required]
        public string CarRegistration { get; set; }
        public DateTime? EntryDate { get; set; } = DateTime.Now;
        public DateTime? IssueDate { get; set; }
        public double? HeatHoursDrying { get; set; }
        public double? HumidityPercent { get; set; }
        [Required]
        public string Observations { get; set; }
        [Required]
        public int ProducerId { get; set; }
    }
    public class UpdateReceptionBindingModel
    {
        [Required]
        public string Variety { get; set; } 
        [Required]
        public double ReceivedFromField { get; set; }
        [Required]
        public string FieldName { get; set; }
        [Required]
        public string CarRegistration { get; set; }
        public DateTime EntryDate { get; set; } = DateTime.Now;
        public DateTime? IssueDate { get; set; }
        public double? HeatHoursDrying { get; set; }
        public double? HumidityPercent { get; set; }
        [Required]
        public string Observations { get; set; }
        [Required]
        public int ProducerId { get; set; }
    }
}