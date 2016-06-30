using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using naseNut.WebApi.Models.Entities;

namespace naseNut.WebApi.Models.Business.Repositories
{
    public class HumidityViewRepository : Repository<TotalHumidityView>
    {
        public HumidityViewRepository(NaseNEntities context) : base(context)
        {

        }
    }
}