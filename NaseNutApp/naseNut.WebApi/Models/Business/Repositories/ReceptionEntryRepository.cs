using naseNut.WebApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace naseNut.WebApi.Models.Business.Repositories
{
    public class ReceptionEntryRepository : Repository<ReceptionEntry>
    {
        public ReceptionEntryRepository(NaseNEntities context) : base(context)
        {
        }
    }
}