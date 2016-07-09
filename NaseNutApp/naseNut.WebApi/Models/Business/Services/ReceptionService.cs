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
        public bool Save(List<Reception> receptions, int CylinderId, int VarietyId, int ProducerId)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var receptionEntry = new ReceptionEntry { Id = CylinderId, DateEntry = DateTime.Now, VarietyId = VarietyId , ProducerId = ProducerId};
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

        public List<Reception> GetReceptionsByCylinders(int cylinderId)
        {
            try
            {
                var db = new NaseNEntities();
                var cylinderRepository = new CylinderRepository(db);
                var cylinder = cylinderRepository.GetById(cylinderId);
                var receptions = cylinder.ReceptionEntry.Receptions.ToList();
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
                        reception.ReceivedFromField = model.ReceivedFromField;

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