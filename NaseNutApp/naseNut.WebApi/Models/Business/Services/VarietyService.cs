﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using naseNut.WebApi.Models.Business.Repositories;
using naseNut.WebApi.Models.Entities;

namespace naseNut.WebApi.Models.Business.Services
{
    public class VarietyService : IService<Variety>
    {
        public bool Save(Variety variety)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    db.Varieties.Add(variety);
                    return db.SaveChanges() >= 1;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool Delete(Variety variety)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var varietyRepository = new VarietyRepository(db);
                    var nutSizeRepository = new NutSizeRepository(db);
                    var nutSize = nutSizeRepository.GetById(variety.Id);
                    nutSizeRepository.Delete(nutSize);
                    varietyRepository.Delete(variety);
                    return db.SaveChanges() >= 1;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<Variety> GetAll()
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var varietyRepository = new VarietyRepository(db);
                    return varietyRepository.GetAll();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Variety GetById(int id)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var varietyRepository = new VarietyRepository(db);
                    return varietyRepository.SearchOne(p => p.Id == id);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public Variety GetByVarietyName(string varietyName)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var varietyRepository = new VarietyRepository(db);
                    return varietyRepository.SearchOne(v => v.Variety1.ToLower() == varietyName.ToLower() && v.HarvestSeason.Active);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}