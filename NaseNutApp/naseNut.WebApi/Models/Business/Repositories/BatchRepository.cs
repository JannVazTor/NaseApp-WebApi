using naseNut.WebApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace naseNut.WebApi.Models.Business.Repositories
{
    public class BatchRepository : Repository<Batch>
    {
        public BatchRepository(NaseNEntities context) : base(context)
        {
        }
    }
}