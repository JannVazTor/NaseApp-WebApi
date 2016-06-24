﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;
using naseNut.WebApi.Models.Business.Repositories;
using naseNut.WebApi.Models.Entities;

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
                using (var db = new NaseNEntities())
                {
                    var receptionRepository = new ReceptionRepository(db);
                    var receptions = receptionRepository.GetAllWithProperties();
                    return receptions;
                }
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
                    db.Set<Reception>().Attach(reception);
                    db.Entry(reception).State = EntityState.Modified;
                    reception.Grills.Add(grill);;
                    return db.SaveChanges()>=1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(Reception reception, int cylinderId)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var receptionRepository = new ReceptionRepository(db);
                    var cylinderRepository = new CylinderRepository(db);

                    var cylinder = cylinderRepository.GetById(cylinderId);
                    receptionRepository.Update(reception);
                    cylinder.Receptions.Add(reception);
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