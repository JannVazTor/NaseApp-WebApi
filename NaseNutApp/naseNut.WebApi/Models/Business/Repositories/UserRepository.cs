using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using naseNut.WebApi.Models.Entities;

namespace naseNut.WebApi.Models.Business.Repositories
{
    public class UserRepository:Repository<AspNetUser>
    {
        protected NaseNEntities _context;
        public UserRepository(NaseNEntities context) : base(context)
        {
            _context = context;
        }

        public List<AspNetUser> GetAllWithProperties()
        {
            return _context.AspNetUsers.ToList();
        }
    }
}