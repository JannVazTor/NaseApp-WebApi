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

        public bool Delete(int id, bool isProcessResult)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    if (isProcessResult)
                    {
                        var receptionEntryRepository = new ReceptionEntryRepository(db);
                        var receptionEntry = receptionEntryRepository.GetById(id);
                        receptionEntry.IssueDate = null;
                        db.ReceptionEntries.Attach(receptionEntry);
                        db.Entry(receptionEntry).Property(p => p.IssueDate).IsModified = true;

                        receptionEntry.NutTypes.ToList().ForEach(n => db.NutTypes.Remove(n));
                        receptionEntry.Samplings.ToList().ForEach(n => db.Samplings.Remove(n));
                    }
                    else {
                        var samplingRepository = new SamplingRepository(db);
                        var sampling = samplingRepository.GetById(id);
                        samplingRepository.Delete(sampling);
                    }
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

        public bool Update(UpdateGrillSamplingBindingModel model, bool isProcessResult)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var samplingRepository = new SamplingRepository(db);
                    if (isProcessResult)
                    {
                        var receptionEntryRepository = new ReceptionEntryRepository(db);
                        var receptionEntry = receptionEntryRepository.GetById(model.Id);
                        var sampling = receptionEntry.Samplings.OrderByDescending(d => d.DateCapture).First();
                        sampling.DateCapture = model.DateCapture;
                        sampling.HumidityPercent = model.HumidityPercent;
                        sampling.Performance = model.Performance;
                        sampling.SampleWeight = model.SampleWeight;
                        sampling.TotalWeightOfEdibleNuts = model.TotalWeightOfEdibleNuts;
                        sampling.WalnutNumber = model.WalnutNumber;
                        samplingRepository.Update(sampling);
                    }
                    else {
                        var sampling = samplingRepository.GetById(model.Id);
                            sampling.DateCapture = model.DateCapture;
                            sampling.HumidityPercent = model.HumidityPercent;
                            sampling.Performance = model.Performance;
                            sampling.SampleWeight = model.SampleWeight;
                            sampling.TotalWeightOfEdibleNuts = model.TotalWeightOfEdibleNuts;
                            sampling.WalnutNumber = model.WalnutNumber;
                            samplingRepository.Update(sampling);
                    }
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