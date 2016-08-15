using naseNut.WebApi.Models.Business.Repositories;
using naseNut.WebApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace naseNut.WebApi.Models.Business.Services
{
    public class NutTypeService
    {
        //Save sampling for ReceptionEntry
        public bool Save(List<NutType> nutTypes, int receptionEntryId)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var nutTypeRepository = new NutTypeRepository(db);
                    var receptionEntryRepository = new ReceptionEntryRepository(db);
                    var receptionEntry = receptionEntryRepository.GetById(receptionEntryId);
                    if (receptionEntry.Samplings.Any())
                    {
                        nutTypes.ForEach(n => n.SamplingId = receptionEntry.Samplings.First().Id);
                    }
                    receptionEntry.DateIssue = DateTime.Now;
                    db.ReceptionEntries.Attach(receptionEntry);
                    db.Entry(receptionEntry).Property(p => p.DateIssue).IsModified = true;

                    nutTypes.ForEach(n => nutTypeRepository.Insert(n));
                    return db.SaveChanges() >= 1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}