using System;
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
        public bool Delete(Reception reception)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var receptionRepository = new ReceptionRepository(db);
                    receptionRepository.Delete(reception);
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
                    return db.SaveChanges()>=1;
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
                    var reception = db.Receptions.Find(id);
                    if (reception != null)
                    {
                        reception.CarRegistration = model.CarRegistration;
                        reception.FieldId = model.FieldId;
                        reception.HeatHoursDtrying = model.HeatHoursDrying;
                        reception.HumidityPercent = model.HumidityPercent;
                        reception.ReceivedFromField = model.ReceivedFromField;
                        reception.Observations = model.Observations;
                        var receptionRepository = new ReceptionRepository(db);
                        receptionRepository.Update(reception);
                        return db.SaveChanges() >= 1;
                    }
                    else
                    {
                        return false;
                    }    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}