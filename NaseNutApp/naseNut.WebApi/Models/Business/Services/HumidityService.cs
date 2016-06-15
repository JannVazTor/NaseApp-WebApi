using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using naseNut.WebApi.Models.Business.Repositories;
using naseNut.WebApi.Models.Entities;

namespace naseNut.WebApi.Models.Business.Services
{
    public class HumidityService:IService<Humidity>
    {
        public bool Save(Humidity humidity)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var humidityRepository = new HumidityRepository(db);
                    humidityRepository.Insert(humidity);
                    return db.SaveChanges() >= 1;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool Delete(Humidity humidity)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var humidityRepository = new HumidityRepository(db);
                    humidityRepository.Delete(humidity);
                    return db.SaveChanges() >= 1;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<Humidity> GetAll()
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var humidityRepository = new HumidityRepository(db);
                    return humidityRepository.GetAll();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}