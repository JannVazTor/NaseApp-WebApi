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
        public List<AspNetRole> GetAll()
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var roleRepository = new RoleRepository(db);
                    var role = roleRepository.GetAll();
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

        public void CreateRoles() {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var roleList = new List<AspNetRole> {
                        new AspNetRole {
                            Id = "592690d4-f3ce-4e57-b2c8-78d0394b08bd",
                            Name = "admin"
                        },
                        new AspNetRole {
                            Id = "479627c7-12a3-4778-ab3f-1ec2bd048ce1",
                            Name = "humidityUser"
                        },
                        new AspNetRole {
                            Id = "ba05c2a8-4c32-44af-a26a-a3eb9bd8240f",
                            Name = "grillUser"
                        },
                        new AspNetRole {
                            Id = "cc74bc28-2050-4be5-8ca9-da71c55d1403",
                            Name = "qualityUser"
                        },
                        new AspNetRole {
                            Id = "4f76500a-ba89-47d5-8a48-ed4b880ada40",
                            Name = "remRecepUser"
                        },
                        new AspNetRole {
                            Id = "b64a6b00-4480-42b8-9b10-40435164494a",
                            Name = "producerUser"
                        }
                    };
                    var roleRepository = new RoleRepository(db);
                    foreach (var role in roleList)
                    {
                        if (!db.AspNetRoles.Any(r => r.Id == role.Id)) {
                            roleRepository.Insert(role);
                        }
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}