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
        public bool Save(Producer producer)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var producerRepository = new ProducerRepository(db);
                    producerRepository.Insert(producer);
                    return db.SaveChanges() >= 1;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool Delete(Producer producer)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var producerRepository = new ProducerRepository(db);
                    producerRepository.Delete(producer);
                    return db.SaveChanges() >= 1;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Producer GetById(int id)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var producerRepository = new ProducerRepository(db);
                    return producerRepository.SearchOne(p => p.Id == id);
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
        public Producer GetByProducerName(string producer)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var producerRepository = new ProducerRepository(db);
                    return producerRepository.SearchOne(p => p.ProducerName.ToLower() == producer.ToLower() && p.HarvestSeason.Active);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public List<Producer> GetAll()
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var producerRepository = new ProducerRepository(db);
                    return producerRepository.GetAll();
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
    }
}