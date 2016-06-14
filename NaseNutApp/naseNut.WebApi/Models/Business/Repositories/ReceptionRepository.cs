using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using naseNut.WebApi.Models.Entities;

namespace naseNut.WebApi.Models.Business.Repositories
{
    public class ReceptionRepository : Repository<Reception>
    {
        public ReceptionRepository(NaseNEntities context) : base(context)
        {
        }
    }
}