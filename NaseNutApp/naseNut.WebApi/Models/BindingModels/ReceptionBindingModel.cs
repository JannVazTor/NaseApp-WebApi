using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace naseNut.WebApi.Models.BindingModels
{
    public class AddOrUpdateReceptionBindingModel
    {
        [Required]
        public int Folio { get; set; }
        [Required]
        public string CarRegistration { get; set; }
        public double? HeatHoursDrying { get; set; }
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
        public List<AddOrUpdateReceptionBindingModel> Receptions { get; set; }
    }
}