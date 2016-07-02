using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using naseNut.WebApi.Models.Business.Repositories;
using naseNut.WebApi.Models.Entities;
using naseNut.WebApi.Models.BindingModels;

namespace naseNut.WebApi.Models.Business.Services
{
    public class RemissionService:IService<Remission>
    {
        public bool Save(Remission remission)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var remissionRepository = new RemissionRepository(db);
                    remissionRepository.Insert(remission);
                    return db.SaveChanges() >= 1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(Remission remission)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var remissionRepository = new RemissionRepository(db);
                    remissionRepository.Delete(remission);
                    return db.SaveChanges() >= 1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Remission GetById(int id)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var remissionRepository = new RemissionRepository(db);
                    var remission = remissionRepository.GetById(id);
                    return remission;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<Remission> GetAll()
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var remissionRepository = new RemissionRepository(db);
                    return remissionRepository.GetAll();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(int id,UpdateRemissionBindingModel model)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var remissionRepository = new RemissionRepository(db);
                    var remission = db.Remission.Find(id);

                    remission.Batch = model.Batch;
                    remission.Butler = model.Butler;
                    remission.Cultivation = model.Cultivation;
                    remission.DateCapture = model.DateCapture;
                    remission.Driver = model.Driver;
                    remission.Elaborate = model.Elaborate;
                    remission.Quantity = model.Quantity;
                    remission.TransportNumber = model.TransportNumber;

                    remissionRepository.Update(remission);
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