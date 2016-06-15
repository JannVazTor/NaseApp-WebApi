using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using naseNut.WebApi.Models.Entities;

namespace naseNut.WebApi.Models.Business.Repositories
{
    public class RoleRepository:Repository<AspNetRole>
    {
        public RoleRepository(NaseNEntities context) : base(context)
        {
        }
    }
}