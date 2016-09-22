using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace naseNut.WebApi.Models.BindingModels
{
    public class AddSamplingBindingModel
    {
        public int? ReceptionEntryId { get; set; }
        public int? GrillId { get; set; }
        [Required]
        public DateTime DateCapture { get; set; }
        [Required]
        public double SampleWeight { get; set; }
        [Required]
        public double HumidityPercent { get; set; }
        [Required]
        public int WalnutNumber { get; set; }
        public double Performance { get; set; }
        [Required]
        public double TotalWeightOfEdibleNuts { get; set; }
    }
    public class AddNutTypeBindingModel
    {
        [Required]
        public int ReceptionEntryId { get; set; }
        [Required]
        public List<NutTypeBindingModel> NutTypes { get; set; }
        [Required]
        public List<NutSizeProcessResultBindingModel> NutSizeProcessResult { get; set; }
    }
    public class NutTypeBindingModel
    {
        public byte NutType { get; set; }
        public float Kilos { get; set; }
        public int Sacks { get; set; }
    }
    public class NutSizeProcessResultBindingModel
    {
        public int NutSize { get; set; }
        public int Sacks { get; set; }
    }
    public class UpdateGrillSamplingBindingModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public DateTime DateCapture { get; set; }
        [Required]
        public double SampleWeight { get; set; }
        [Required]
        public double HumidityPercent { get; set; }
        [Required]
        public int WalnutNumber { get; set; }
        public double Performance { get; set; }
        [Required]
        public double TotalWeightOfEdibleNuts { get; set; }
    }
}