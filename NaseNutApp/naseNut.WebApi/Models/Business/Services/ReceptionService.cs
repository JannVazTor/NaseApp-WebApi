﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;
using naseNut.WebApi.Models.Business.Repositories;
using naseNut.WebApi.Models.Entities;
using naseNut.WebApi.Models.BindingModels;
using System.Data.Entity;

namespace naseNut.WebApi.Models.Business.Services
{
    public class ReceptionService
    {
        public bool Delete(Reception reception, bool onlyHasOne)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    if (onlyHasOne)
                    {
                        var receptionEntryRepository = new ReceptionEntryRepository(db);
                        var receptionEntry = receptionEntryRepository.GetById(reception.ReceptionEntryId);
                        var receptionRepository = new ReceptionRepository(db);
                        receptionEntryRepository.Delete(receptionEntry);
                    }
                    else
                    {
                        var receptionRepository = new ReceptionRepository(db);
                        receptionRepository.Delete(reception);
                    }
                    return db.SaveChanges() >= 1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<Reception> GetAll()
        {
            try
            {
                // using (var db = new NaseNEntities())
                //{
                var db = new NaseNEntities();
                var receptionRepository = new ReceptionRepository(db);
                var receptions = receptionRepository.GetAllWithProperties();
                return receptions;
                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Reception GetById(int id)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var receptionRepository = new ReceptionRepository(db);

                    var reception = receptionRepository.GetById(id);
                    return reception;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Reception GetByFolio(int folio)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var receptionRepository = new ReceptionRepository(db);

                    var reception = receptionRepository.SearchOne(r => r.Folio == folio && r.ReceptionEntry.HarvestSeason.Active);
                    return reception;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool AddReceptionToGrill(int receptionId, int grillId)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var receptionRepository = new ReceptionRepository(db);
                    var grillRepository = new GrillRepository(db);
                    var reception = receptionRepository.GetById(receptionId);
                    var grill = grillRepository.GetById(grillId);
                    receptionRepository.Update(reception);
                    reception.Grills.Add(grill);
                    return db.SaveChanges() >= 1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool RemoveReceptionToGrill(int receptionId, int grillId)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var receptionRepository = new ReceptionRepository(db);
                    var grillRepository = new GrillRepository(db);
                    var reception = receptionRepository.GetById(receptionId);
                    var grill = grillRepository.GetById(grillId);
                    reception.Grills.Remove(grill);
                    return db.SaveChanges() >= 1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool Update(int id, AddOrUpdateReceptionBindingModel model)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var receptionService = new ReceptionService();
                    var receptionRepository = new ReceptionRepository(db);
                    var receptionEntryRepository = new ReceptionEntryRepository(db);
                    var reception = db.Receptions.Find(id);
                    reception.CarRegistration = model.CarRegistration;
                    reception.HeatHoursDtrying = model.HeatHoursDrying;
                    reception.Observations = model.Observations;
                    receptionRepository.Update(reception);
                    var modified = db.SaveChanges() >= 1;
                    if (!modified) return false;

                    var receptionEntry = receptionEntryRepository.SearchOne(r => r.Id == reception.ReceptionEntryId);
                    receptionEntry.EntryDate = model.EntryDate;
                    db.ReceptionEntries.Attach(receptionEntry);
                    db.Entry(receptionEntry).Property(p => p.EntryDate).IsModified = true;
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