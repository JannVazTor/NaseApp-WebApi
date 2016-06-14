using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using naseNut.WebApi.Models.Business.Repositories;
using naseNut.WebApi.Models.Entities;

namespace naseNut.WebApi.Models.Business.Services
{
    public class ProducerService : IService<Producer>
    {
        public bool Save(Producer obj)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var producerRepository = new ProducerRepository(db);
                    producerRepository.Insert(obj);
                    return db.SaveChanges() >= 1;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool Delete(Producer obj)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var producerRepository = new ProducerRepository(db);
                    producerRepository.Delete(obj);
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