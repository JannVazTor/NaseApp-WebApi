using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using naseNut.WebApi.Models.Entities;

//using naseNut.WebApi.Models.Entities;

namespace naseNut.WebApi.Models.Business.Repositories
{
    public class ProducerRepository : Repository<Producer>
    {
        public ProducerRepository(NaseNEntities context) : base(context)
        {
        }
    }
}