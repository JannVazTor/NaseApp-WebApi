using naseNut.WebApi.Models.BindingModels;
using naseNut.WebApi.Models.Business.Repositories;
using naseNut.WebApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace naseNut.WebApi.Models.Business.Services
{
    public class BatchService
    {
        public bool Save(Batch batch, List<NutInBatchBindingModel> nutInBatchModel)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var batchRepository = new BatchRepository(db);
                    if (batchRepository.Search(b => b.Batch1.ToLower() == batch.Batch1 && b.FieldId == batch.FieldId).Any())
                    {
                        var batchE = db.Batches.FirstOrDefault(b => b.Batch1.ToLower() == batch.Batch1 && b.FieldId == batch.FieldId);
                        batchE.NutInBatches.ToList().ForEach(n => db.NutInBatches.Remove(n));
                        db.SaveChanges();

                        batchE.Hectares = batch.Hectares;
                        db.Batches.Attach(batchE);
                        db.Entry(batchE).Property(p => p.Hectares).IsModified = true;
                        db.SaveChanges();

                        var nutInBatchRepositoryU = new NutInBatchRepository(db);
                        var nutInBatchU = nutInBatchModel.Select(n => new NutInBatch
                        {
                            BatchId = batchE.Id,
                            NutPercentage = n.NutPercentage,
                            VarietyId = n.VarietyId
                        }).ToList();
                        nutInBatchU.ForEach(n => nutInBatchRepositoryU.Insert(n));

                        return db.SaveChanges() >= 1;
                    }
                        var nutInBatchRepositoryA = new NutInBatchRepository(db);
                        batchRepository.Insert(batch);
                        var saved = db.SaveChanges() >= 1;
                        if (!saved) return false;
                        var nutInBatchA = nutInBatchModel.Select(n => new NutInBatch
                        {
                            BatchId = batch.Id,
                            NutPercentage = n.NutPercentage,
                            VarietyId = n.VarietyId
                        }).ToList();
                        nutInBatchA.ForEach(n => nutInBatchRepositoryA.Insert(n));
                        return db.SaveChanges() >= 1;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool Delete(Batch batch)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var batchRepository = new BatchRepository(db);
                    batchRepository.Delete(batch);
                    return db.SaveChanges() >= 1;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Batch GetById(int id)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var batchRepository = new BatchRepository(db);
                    return batchRepository.SearchOne(p => p.Id == id);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public Batch GetByBatchName(string batchName)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var batchRepository = new BatchRepository(db);
                    return batchRepository.SearchOne(p => p.Batch1 == batchName);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<Batch> GetAll()
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var batchRepository = new BatchRepository(db);
                    return batchRepository.GetAll();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsBatchInField(string batch, int fieldId) {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var batchRepository = new BatchRepository(db);
                    return batchRepository.Search(b => b.Batch1.ToLower() == batch && b.FieldId == fieldId).Any();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}