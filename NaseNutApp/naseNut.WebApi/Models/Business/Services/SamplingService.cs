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
        public bool Save(Sampling sampling)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var samplingRepository = new SamplingRepository(db);
                    if (sampling.ReceptionEntryId != null)
                    {
                        var receptionEntry = db.ReceptionEntries.Where(r => r.Id == sampling.ReceptionEntryId).First();
                        var nutTypes = receptionEntry.NutTypes.ToList();
                        foreach (var item in nutTypes)
                        {
                            item.SamplingId = sampling.Id;
                            db.NutTypes.Attach(item);
                            db.Entry(item).Property(p => p.SamplingId).IsModified = true;
                        }
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

        public bool Delete(int id)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var samplingRepository = new SamplingRepository(db);
                    var sampling = samplingRepository.GetById(id);

                    if (sampling.ReceptionEntryId != null)
                    {
                        var receptionEntryRepository = new ReceptionEntryRepository(db);
                        var receptionEntry = receptionEntryRepository.GetById(sampling.ReceptionEntry.Id);

                        receptionEntry.IssueDate = null;
                        db.ReceptionEntries.Attach(receptionEntry);
                        db.Entry(receptionEntry).Property(p => p.IssueDate).IsModified = true;

                        receptionEntry.NutTypes.ToList().ForEach(n => db.NutTypes.Remove(n));
                    }
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