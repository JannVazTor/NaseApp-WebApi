using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using naseNut.WebApi.Models.Business.Repositories;
using naseNut.WebApi.Models.Entities;

namespace naseNut.WebApi.Models.Business.Services
{
    public class UserService
    {
        public bool Delete(AspNetUser user)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var userRepository = new UserRepository(db);
                    userRepository.Delete(user);
                    return db.SaveChanges() >= 1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public AspNetUser GetById(string id)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var userRepository = new UserRepository(db);
                    var user = userRepository.GetById(id);
                    return user;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}