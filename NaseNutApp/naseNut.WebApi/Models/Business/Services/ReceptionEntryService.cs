using naseNut.WebApi.Models.Business.Repositories;
using naseNut.WebApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace naseNut.WebApi.Models.Business.Services
{
    public class ReceptionEntryService
    {
        public bool Save(List<Reception> receptions, int CylinderId)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var receptionEntry = new ReceptionEntry { Id = CylinderId, DateEntry = DateTime.Now };
                    foreach (var reception in receptions)
                    {
                        receptionEntry.Receptions.Add(reception);
                    }
                    db.ReceptionEntries.Add(receptionEntry);
                    return db.SaveChanges() >= 1;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool Delete(ReceptionEntry receptionEntry)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var receptionEntryRepository = new ReceptionEntryRepository(db);
                    receptionEntryRepository.Delete(receptionEntry);
                    return db.SaveChanges() >= 1;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public ReceptionEntry GetById(int id)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var receptionEntryRepository = new ReceptionEntryRepository(db);
                    var receptionEntry = receptionEntryRepository.GetById(id);
                    return receptionEntry;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ReceptionEntry> GetAll() {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var receptionEntryRepository = new ReceptionEntryRepository(db);
                    var receptionEntries = receptionEntryRepository.GetAll();
                    return receptionEntries;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}