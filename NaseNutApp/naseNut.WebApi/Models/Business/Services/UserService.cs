using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using naseNut.WebApi.Models.Business.Repositories;
using naseNut.WebApi.Models.Entities;
using naseNut.WebApi.Controllers;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Net.Http;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;

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
        public AspNetUser GetByName(string userName)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var userRepository = new UserRepository(db);
                    var user = userRepository.SearchOne(u => u.UserName == userName);
                    return user;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateAdminUser()
        {
            using (var db = new ApplicationDbContext())
            {
                if (!(db.Users.Any(u => u.UserName == "jannadmin")))
                {
                    var userStore = new UserStore<ApplicationUser>(db);
                    var userManager = new UserManager<ApplicationUser>(userStore);
                    var userToInsert = new ApplicationUser { UserName = "jannadmin", Email = "jannadmin@gmail.com" };
                    userManager.Create(userToInsert, "SuperPass19*");
                    var roleService = new RoleService();
                    var role = roleService.GetById("592690d4-f3ce-4e57-b2c8-78d0394b08bd");
                    userManager.AddToRole(userToInsert.Id, role.Name);
                }
            }
        }
    }
}