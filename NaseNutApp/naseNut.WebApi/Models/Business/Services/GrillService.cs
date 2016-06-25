using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using naseNut.WebApi.Models.Business.Repositories;
using naseNut.WebApi.Models.Entities;

namespace naseNut.WebApi.Models.Business.Services
{
    public class GrillService : IService<Grill>
    {
        public bool Save(Grill grill)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var grillRepository = new GrillRepository(db);
                    grillRepository.Insert(grill);
                    return db.SaveChanges() >= 1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(Grill grill)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var grillRepository = new GrillRepository(db);
                    grillRepository.Delete(grill);
                    return db.SaveChanges() >= 1;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<Grill> GetAll()
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var grillRepository = new GrillRepository(db);
                    var grills = grillRepository.GetAll();
                    return grills;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Grill GetById(int id)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var grillRepository = new GrillRepository(db);
                    var grill = grillRepository.GetById(id);
                    return grill;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool AddGrillToReception(int receptionId, int grillId)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var receptionRepository = new ReceptionRepository(db);
                    var grillRepository = new GrillRepository(db);
                    var reception = receptionRepository.GetById(receptionId);
                    var grill = grillRepository.GetById(grillId);
                    grillRepository.Update(grill);
                    grill.Receptions.Add(reception);
                    return db.SaveChanges() >= 1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool RemoveGrillToReception(int receptionId, int grillId)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var receptionRepository = new ReceptionRepository(db);
                    var grillRepository = new GrillRepository(db);
                    var reception = receptionRepository.GetById(receptionId);
                    var grill = grillRepository.GetById(grillId);
                    grill.Receptions.Remove(reception);
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