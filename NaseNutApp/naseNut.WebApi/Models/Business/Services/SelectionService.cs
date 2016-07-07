using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using naseNut.WebApi.Models.Business.Repositories;
using naseNut.WebApi.Models.Entities;

namespace naseNut.WebApi.Models.Business.Services
{
    public class SelectionService:IService<Selection>
    {
        public bool Save(Selection selection)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var selectionRepository = new SelectionRepository(db);
                    selectionRepository.Insert(selection);
                    return db.SaveChanges() >= 1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(Selection selection)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var selectionRepository = new SelectionRepository(db);
                    selectionRepository.Delete(selection);
                    return db.SaveChanges() >= 1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Selection> GetAll()
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var selectionRepository = new SelectionRepository(db);
                    var selections = selectionRepository.GetAll();
                    return selections;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Selection GetById(int id)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var selectionRepository = new SelectionRepository(db);
                    return selectionRepository.SearchOne(p => p.Id == id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}