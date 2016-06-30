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
    public class ReceptionService : IService<Reception>
    {
        public bool Save(Reception reception)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var receptionRepository = new ReceptionRepository(db);
                    receptionRepository.Insert(reception);
                    return db.SaveChanges() >= 1;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

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

        public bool SaveToCylinder(Reception reception, int cylinderId)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var cylinderRepository = new CylinderRepository(db);
                    var cylinder = cylinderRepository.GetById(cylinderId);
                    cylinder.Receptions.Add(reception);
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

        public List<Reception> GetReceptionsByCylinders(int cylinderId)
        {
            try
            {
                var db = new NaseNEntities();
                var cylinderRepository = new CylinderRepository(db);
                var cylinder = cylinderRepository.GetById(cylinderId);
                var receptions = cylinder.Receptions.ToList();
                return receptions;
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
        public bool Update(int id,UpdateReceptionBindingModel model)
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
                        reception.EntryDate = model.EntryDate;
                        reception.FieldName = model.FieldName;
                        reception.HeatHoursDtrying = model.HeatHoursDrying;
                        reception.HumidityPercent = model.HumidityPercent;
                        reception.IssueDate = model.IssueDate;
                        reception.ProducerId = model.ProducerId;
                        reception.ReceivedFromField = model.ReceivedFromField;
                        reception.Variety = model.Variety;

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