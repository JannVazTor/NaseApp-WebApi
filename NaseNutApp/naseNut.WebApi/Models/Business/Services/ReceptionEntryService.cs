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
        public bool Save(List<Reception> receptions, int CylinderId, int VarietyId, int ProducerId)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var receptionEntry = new ReceptionEntry {
                        EntryDate = DateTime.Now,
                        VarietyId = VarietyId,
                        ProducerId = ProducerId,
                        CylinderId = CylinderId,
                        Active = true 
                    };
                    foreach (var reception in receptions)
                    {
                        receptionEntry.Receptions.Add(reception);
                    }
                    var cylinder = db.Cylinders.Where(c => c.Id == CylinderId).FirstOrDefault();
                    cylinder.Active = false;
                    db.Cylinders.Attach(cylinder);
                    db.Entry(cylinder).Property(p => p.Active).IsModified = true;
                    db.ReceptionEntries.Add(receptionEntry);
                    return db.SaveChanges() >= 1;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ReceptionEntry> GetByCylinderId(int cylinderId)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var receptionEntryRepository = new ReceptionEntryRepository(db);
                    var receptionEntries = receptionEntryRepository.Search(r => r.CylinderId == cylinderId);
                    return receptionEntries;
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