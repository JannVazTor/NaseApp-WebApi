using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace naseNut.WebApi.Models.BindingModels
{
    public class AddVarietyBindingModel
    {
        [Required]
        public string VarietyName { get; set; }
        [Required]
        public int Small { get; set; }
        [Required]
        public int MediumStart { get; set; }
        [Required]
        public int MediumEnd { get; set; }
        [Required]
        public int LargeStart { get; set; }
        [Required]
        public int LargeEnd { get; set; }
    }
}