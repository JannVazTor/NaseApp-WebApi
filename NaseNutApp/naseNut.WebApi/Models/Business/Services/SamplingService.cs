using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using naseNut.WebApi.Models.BindingModels;
using naseNut.WebApi.Models.Business.Repositories;
using naseNut.WebApi.Models.Entities;

namespace naseNut.WebApi.Models.Business.Services
{
    public class SamplingService
    {
        //Save sampling for ReceptionEntry
        public bool Save(Sampling sampling, List<NutType> nutTypes, int receptionEntryId)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var samplingRepository = new SamplingRepository(db);
                    var nutTypeRepository = new NutTypeRepository(db);
                    var cylinderRepository = new CylinderRepository(db);
                    var receptionEntryRepository = new ReceptionEntryRepository(db);

                    var receptionEntry = receptionEntryRepository.GetById(receptionEntryId);
                    var cylinder = cylinderRepository.GetById(receptionEntry.Cylinder.Id);

                    receptionEntry.DateIssue = DateTime.Now;
                    db.ReceptionEntries.Attach(receptionEntry);
                    db.Entry(receptionEntry).Property(p => p.DateIssue).IsModified = true;

                    foreach (var item in nutTypes)
                    {
                        nutTypeRepository.Insert(item);
                    }
                    samplingRepository.Insert(sampling);
                    return db.SaveChanges() >= 1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Save sampling for Grill
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

        public bool Delete(int id)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var samplingRepository = new SamplingRepository(db);
                    var cylinderRepository = new CylinderRepository(db);
                    var receptionEntryRepository = new ReceptionEntryRepository(db);
                    var sampling = samplingRepository.GetById(id);
                    var cylinder = cylinderRepository.GetById(sampling.ReceptionEntry.Cylinder.Id);
                    var receptionEntry = receptionEntryRepository.GetById(sampling.ReceptionEntry.Id);

                    receptionEntry.DateIssue = null;
                    db.ReceptionEntries.Attach(receptionEntry);
                    db.Entry(receptionEntry).Property(p => p.DateIssue).IsModified = true;

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

        public bool Update(UpdateGrillSamplingBindingModel model)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    
                    var sampling = db.Samplings.Find(model.Id);
                    if (sampling != null)
                    {
                        sampling.DateCapture = model.DateCapture.ConvertToDate();
                        sampling.HumidityPercent= model.HumidityPercent;
                        sampling.Performance = (model.TotalWeightOfEdibleNuts / model.SampleWeight) * 100;
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