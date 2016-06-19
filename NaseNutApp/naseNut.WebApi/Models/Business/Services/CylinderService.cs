using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using naseNut.WebApi.Models.Business.Repositories;
using naseNut.WebApi.Models.Entities;

namespace naseNut.WebApi.Models.Business.Services
{
    public class CylinderService:IService<Cylinder>
    {
        public bool Save(Cylinder cylinder)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var cylinderRepository = new CylinderRepository(db);
                    cylinderRepository.Insert(cylinder);
                    return db.SaveChanges()>=1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(Cylinder cylinder)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var cylinderRepository = new CylinderRepository(db);
                    cylinderRepository.Delete(cylinder);
                    return db.SaveChanges() >= 1;
                }
            }
            catch (Exception ex)
            {
                    
                throw ex;
            }
        }
        public Cylinder GetById(int id)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var cylinderRepository = new CylinderRepository(db);
                    return cylinderRepository.SearchOne(p => p.Id == id);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<Cylinder> GetAll()
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var cylinderRepository = new CylinderRepository(db);
                    var cylinders = cylinderRepository.GetAll();
                    return cylinders;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}