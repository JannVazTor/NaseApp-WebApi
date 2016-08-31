﻿using naseNut.WebApi.Models.BindingModels;
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
                    var nutInBatchRepository = new NutInBatchRepository(db);
                    batchRepository.Insert(batch);
                    var saved = db.SaveChanges()>=1;
                    if (!saved) return false;
                    var nutInBatch = nutInBatchModel.Select(n => new NutInBatch {
                        BatchId = batch.Id,
                        NutPercentage = n.NutPercentage,
                        VarietyId = n.VarietyId
                    }).ToList();
                    nutInBatch.ForEach(n => nutInBatchRepository.Insert(n));
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
    }
}