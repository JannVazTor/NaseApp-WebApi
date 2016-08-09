using naseNut.WebApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace naseNut.WebApi.Models.Business.Repositories
{
    public class BoxRepository : Repository<Box>
    {
        public BoxRepository(NaseNEntities context) : base(context)
        {
        }
    }
}