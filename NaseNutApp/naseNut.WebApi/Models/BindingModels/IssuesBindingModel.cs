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
        public DateTime DateCapture { get; set; }
        [Required]
        public string Truck { get; set; }
        [Required]
        public string Driver { get; set; }
        [Required]
        public string Box { get; set; }
        [Required]
        public List<int> GrillsIds { get; set; }
    }
}