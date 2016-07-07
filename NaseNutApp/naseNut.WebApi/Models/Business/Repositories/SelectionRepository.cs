using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using naseNut.WebApi.Models.Entities;

namespace naseNut.WebApi.Models.Business.Repositories
{
    public class SelectionRepository : Repository<Selection>
    {
        public SelectionRepository(NaseNEntities context) : base(context)
        {
        }
    }
}