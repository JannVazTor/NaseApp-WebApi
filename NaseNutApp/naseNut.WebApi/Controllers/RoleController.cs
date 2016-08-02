using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using naseNut.WebApi.Models;
using naseNut.WebApi.Models.BindingModels;
using naseNut.WebApi.Models.Business.Services;
using naseNut.WebApi.Models.Entities;

namespace naseNut.WebApi.Controllers
{
    [Authorize(Roles = "admin")]
    [RoutePrefix("api/roles")]
    public class RoleController : BaseApiController
    {
        [HttpPost]
        [Route("addUserToRole")]
        public IHttpActionResult AddUserToRole(AddRoleToUserBindingModel model)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                    var user = userManager.FindByName(model.UserName);
                    var result = userManager.AddToRole(user.Id, model.RoleName);
                    return (result.Succeeded) ? (IHttpActionResult) Ok() : InternalServerError();
                }
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                    "Ocurrio un error al intentar asignar el rol al usuario." + "\n" + "Detalles del Error: " + ex));
            }
        }

        [HttpGet]
        [Route("getAll")]
        public IHttpActionResult GetAllRoles()
        {
            try
            {
                var roleService = new RoleService();
                var roles = roleService.GetAll();
                return roles != null ? (IHttpActionResult)Ok(TheModelFactory.Create(roles)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener los roles." + "\n" + "Detalles del Error: " + ex));
            }
        }

        [HttpDelete]
        [Route("{roleId}")]
        public IHttpActionResult DeleteRole(string roleId)
        {
            if (!ModelState.IsValid) return BadRequest();
            var roleService = new RoleService();
            var role = roleService.GetById(roleId);
            var deleted = roleService.Delete(role);
            if (!deleted) return InternalServerError();
            return Ok();
        }

        [HttpPost]
        public IHttpActionResult SaveRol(RoleBindingModel model)
        {
            if (!ModelState.IsValid) return BadRequest();
            var roleService = new RoleService();
            var saved = roleService.Save(new AspNetRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.Name
            });
            if (!saved) return InternalServerError();
            return Ok();
        }
    }
}
