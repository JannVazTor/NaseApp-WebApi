using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using naseNut.WebApi.Models.Entities;

namespace naseNut.WebApi.Models.Business.Repositories
{
    public class SamplingRepository : Repository<Sampling>
    {
        public SamplingRepository(NaseNEntities context) : base(context)
        {
        }
    }
}