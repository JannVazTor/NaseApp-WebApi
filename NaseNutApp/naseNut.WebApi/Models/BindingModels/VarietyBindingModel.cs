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
    }
}