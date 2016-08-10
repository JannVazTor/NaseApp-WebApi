using naseNut.WebApi.Models.Business.Repositories;
using naseNut.WebApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace naseNut.WebApi.Models.Business.Services
{
    public class FieldService : IService<Field>
    {
        public bool Save(Field field)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var fieldRepository = new FieldRepository(db);
                    fieldRepository.Insert(field);
                    return db.SaveChanges() >= 1;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool Delete(Field field)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var fieldRepository = new FieldRepository(db);
                    fieldRepository.Delete(field);
                    return db.SaveChanges() >= 1;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Field GetById(int id)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var fieldRepository = new FieldRepository(db);
                    return fieldRepository.SearchOne(p => p.Id == id);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public Field GetByFieldName(string fieldName)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var fieldRepository = new FieldRepository(db);
                    return fieldRepository.SearchOne(p => p.FieldName == fieldName);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<Field> GetAll()
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var fieldRepository = new FieldRepository(db);
                    return fieldRepository.GetAll();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}