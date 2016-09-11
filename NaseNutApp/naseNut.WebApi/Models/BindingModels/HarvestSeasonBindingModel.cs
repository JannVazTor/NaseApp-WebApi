using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace naseNut.WebApi.Models.BindingModels
{
    public class SaveHarvestSeasonBindingModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime IssueDate { get; set; }
        [Required]
        public DateTime EntryDate { get; set; }
        [Required]
        public string UserName { get; set; }
    }
}