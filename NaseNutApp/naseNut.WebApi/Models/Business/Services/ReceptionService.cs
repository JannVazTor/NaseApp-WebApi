﻿using System;
using System.Collections.Generic;
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
    }
}