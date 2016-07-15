using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace naseNut.WebApi.Models.DTOs
{
    public class NutTypeDTO
    {
        public byte NutType { get; set; }
        public float Kilos { get; set; }
        public int Sacks{ get; set; }
    }
}