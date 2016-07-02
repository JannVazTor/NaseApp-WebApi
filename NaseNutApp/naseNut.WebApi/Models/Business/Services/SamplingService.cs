using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using naseNut.WebApi.Models.BindingModels;
using naseNut.WebApi.Models.Business.Repositories;
using naseNut.WebApi.Models.Entities;

namespace naseNut.WebApi.Models.Business.Services
{
    public class SamplingService : IService<Sampling>
    {
        public bool Save(Sampling sampling)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var samplingRepository = new SamplingRepository(db);
                    samplingRepository.Insert(sampling);
                    return db.SaveChanges() >= 1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(Sampling sampling)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var samplingRepository = new SamplingRepository(db);
                    samplingRepository.Delete(sampling);
                    return db.SaveChanges() >= 1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Sampling GetById(int id)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var samplingRepository = new SamplingRepository(db);
                    var sampling = samplingRepository.GetById(id);
                    return sampling;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(AddSamplingBindingModel model)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    
                    var sampling = db.Sampling.Find(model.Id);
                    if (sampling != null)
                    {
                        sampling.DateCapture = model.DateCapture;
                        sampling.HumidityPercent= model.HumidityPercent;
                        sampling.Performance = model.Performance;
                        sampling.SampleWeight = model.SampleWeight;
                        sampling.TotalWeightOfEdibleNuts = model.TotalWeightOfEdibleNuts;
                        sampling.WalnutNumber = model.WalnutNumber;
                       
                        var samplingRepository = new SamplingRepository(db);
                        samplingRepository.Update(sampling);
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