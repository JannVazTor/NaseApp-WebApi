using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace naseNut.WebApi.Models.BindingModels
{
    public class AddReceptionBindingModel
    {
        [Required]
        public int Folio { get; set; }
        [Required]
        public double ReceivedFromField { get; set; }
        [Required]
        public int FieldId { get; set; }
        [Required]
        public string CarRegistration { get; set; }
        public string EntryDate { get; set; }
        public DateTime? IssueDate { get; set; }
        public double? HeatHoursDrying { get; set; }
        public double? HumidityPercent { get; set; }
        [Required]
        public string Observations { get; set; }
    }
    public class AddReceptionEntryBindingModel
    {
        [Required]
        public int CylinderId { get; set; }
        [Required]
        public int VarietyId { get; set; }

        [Required]
        public int ProducerId { get; set; }
        [Required]
        public List<AddReceptionBindingModel> Receptions { get; set; }
    }
    public class UpdateReceptionBindingModel
    {
        [Required]
        public double ReceivedFromField { get; set; }
        [Required]
        public int FieldId { get; set; }
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