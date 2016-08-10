using naseNut.WebApi.Models.Business.Repositories;
using naseNut.WebApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace naseNut.WebApi.Models.Business.Services
{
    public class BoxService
    {
        public bool Save(Box box)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var boxRepository = new BoxRepository(db);
                    if (db.Boxes.Any(b => b.Id == box.Id)) {
                        boxRepository.Update(box);
                    }
                    else{
                        boxRepository.Insert(box);
                    }
                    return db.SaveChanges() >= 1;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool Delete(Box box)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var boxRepository = new BoxRepository(db);
                    boxRepository.Delete(box);
                    return db.SaveChanges() >= 1;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Box GetById(int id)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var boxRepository = new BoxRepository(db);
                    return boxRepository.SearchOne(p => p.Id == id);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public Box GetByBox(string boxName)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var boxRepository = new BoxRepository(db);
                    return boxRepository.SearchOne(p => p.Box1 == boxName);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<Box> GetAll()
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var boxRepository = new BoxRepository(db);
                    return boxRepository.GetAll();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}