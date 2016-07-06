using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using naseNut.WebApi.Models.Entities;

namespace naseNut.WebApi.Models.Business.Repositories
{
    public class ReceptionRepository : Repository<Reception>
    {
        private NaseNEntities _context;
        public ReceptionRepository(NaseNEntities context) : base(context)
        {
            _context = context;
        }
        public List<Reception> GetAllWithProperties()
        {
            return _context.Receptions.ToList();
        }
    }
}