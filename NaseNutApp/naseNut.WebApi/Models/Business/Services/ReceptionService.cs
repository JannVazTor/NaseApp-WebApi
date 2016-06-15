using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            catch (Exception)
            {

                throw;
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
            catch (Exception)
            {

                throw;
            }
        }
    }
}