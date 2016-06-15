using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using naseNut.WebApi.Models.Business.Repositories;
using naseNut.WebApi.Models.Entities;

namespace naseNut.WebApi.Models.Business.Services
{
    public class RoleService:IService<AspNetRole>
    {
        public bool Save(AspNetRole role)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var roleRepository = new RoleRepository(db);
                    db.AspNetRoles.Attach(role);
                    roleRepository.Insert(role);
                    return db.SaveChanges() >= 1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(AspNetRole role)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var roleRepository = new RoleRepository(db);
                    roleRepository.Delete(role);
                    return db.SaveChanges() >= 1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public AspNetRole GetById(string roleId)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var roleRepository = new RoleRepository(db);
                    var role = roleRepository.GetById(roleId);
                    return role;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public AspNetRole GetByName(string roleName)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var roleRepository = new RoleRepository(db);
                    return roleRepository.SearchOne(r => r.Name == roleName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}