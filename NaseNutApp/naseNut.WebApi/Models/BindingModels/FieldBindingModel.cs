using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace naseNut.WebApi.Models.BindingModels
{
    public class AddFieldBindingModel
    {
        [Required]
        public string FieldName { get; set; }
    }
    public class AddBatchBindingModel
    {
        [Required]
        public double Hectares { get; set; }
        [Required]
        public string Batch { get; set; }
        [Required]
        public int FieldId { get; set; }
    }
    public class AddBoxBindingModel
    {
        [Required]
        public string Box { get; set; }
        [Required]
        public int BatchId { get; set; }
    }
}