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
        public bool Save(List<NutType> nutTypes, int receptionEntryId, List<NutSizeProcessResult> nutSizeProcessResult)
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
                    receptionEntry.IssueDate = DateTime.Now;
                    db.ReceptionEntries.Attach(receptionEntry);
                    db.Entry(receptionEntry).Property(p => p.IssueDate).IsModified = true;

                    foreach (var n in nutTypes)
                    {
                        if (n.NutType1 == 1)
                        {
                            foreach (var np in nutSizeProcessResult)
                            {
                                n.NutSizeProcessResults.Add(np);
                            }
                            db.NutTypes.Add(n);
                        }
                        db.NutTypes.Add(n);
                    }
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