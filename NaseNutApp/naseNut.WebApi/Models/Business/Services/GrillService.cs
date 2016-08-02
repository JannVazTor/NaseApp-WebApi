using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using naseNut.WebApi.Models.BindingModels;
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

        public bool SaveIssue(GrillIssue grillIssue)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var grillIssueRepository = new GrillIssueRepository(db);
                    grillIssueRepository.Insert(grillIssue);
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
        public bool UpdateStatus(int id, bool status)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var grillRepository = new GrillRepository(db);
                    var grill = grillRepository.GetById(id);
                    grill.Status = status;
                    db.Grills.Attach(grill);
                    db.Entry(grill).Property(p => p.Status).IsModified = true;
                    return db.SaveChanges() >= 1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool Update(int id, AddOrUpdateGrillBindingModel model)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var grill = db.Grills.Find(id);
                    if (grill == null) return false;
                    grill.DateCapture = model.DateCapture.ConvertToDate(); 
                    grill.FieldId = model.FieldId;
                    grill.Kilos = model.Kilos;
                    grill.ProducerId = model.ProducerId;
                    grill.Quality = model.Quality;
                    grill.Sacks = model.Sacks;
                    grill.Size = model.Size;
                    grill.VarietyId = model.VarietyId;
                    var grillRepository = new GrillRepository(db);
                    grillRepository.Update(grill);
                    return db.SaveChanges() >= 1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveIssue(GrillIssue grillIssue, List<int> grillsId)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var grillIssueRepository = new GrillIssueRepository(db);
                    grillIssueRepository.Insert(grillIssue);
                    var saved = db.SaveChanges() >= 1;
                    if (!saved) return false;
                    var grills = db.Grills.Where(g => grillsId.Any(id => id == g.Id)).ToList();

                    grillIssueRepository.Update(grillIssue);
                    foreach (var grill in grills)
                    {
                        grillIssue.Grills.Add(grill);
                    }
                    var added = db.SaveChanges() >= 1;
                    if (!added) return false;
                    foreach (var grill in grills)
                    {
                        UpdateStatus(grill.Id, false);
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}