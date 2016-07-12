using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace naseNut.WebApi.Models.BindingModels
{
    public class SaveIssueBindingModel
    {
        [Required]
        public int Remission { get; set; }
        [Required]
        public string DateCapture { get; set; }
        public string Truck { get; set; }
        public string Driver { get; set; }
        public string Box { get; set; }
        [Required]
        public List<int> GrillIds { get; set; }
    }
}