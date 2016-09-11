using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using naseNut.WebApi.Models.BindingModels;
using naseNut.WebApi.Models.Business.Services;
using naseNut.WebApi.Models.Entities;
using Microsoft.AspNet.Identity;

namespace naseNut.WebApi.Controllers
{
    [RoutePrefix("api/cylinder")]
    public class CylinderController:BaseApiController
    {
        private NaseNEntities _db = new NaseNEntities();
        [Authorize(Roles = "admin")]
        [HttpPost]
        public IHttpActionResult Save(AddCylinderBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var cylinderService = new CylinderService();
                if (cylinderService.GetByCylinderName(model.CylinderName) != null) return Conflict();
                var cylinder = new Cylinder
                {
                    CylinderName = model.CylinderName,
                    Active = true,
                    HarvestSeasonId = _db.HarvestSeasons.FirstOrDefault(h => h.Active).Id
                };
                var saved = cylinderService.Save(cylinder);
                return saved ? (IHttpActionResult)Ok() : Conflict();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar guardar el registro." + "\n" + "Detalles del Error: " + ex));
            }
        }
        [Authorize(Roles = "admin")]
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            try
            {
                var cylinder = _db.Cylinders.Where(c => c.HarvestSeason.Active).ToList();
                return cylinder.Count != 0 ? (IHttpActionResult)Ok(TheModelFactory.Create(cylinder)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener el registro." + "\n" + "Detalles del Error: " + ex));
            }
        }
        [Authorize(Roles = "admin,remRecepUser")]
        [HttpGet]
        [Route("GetAllActive")]
        public IHttpActionResult GetAllActive()
        {
            try
            {
                var cylinder = _db.Cylinders.Where(c => c.Active && c.HarvestSeason.Active).ToList();
                return cylinder != null ? (IHttpActionResult)Ok(TheModelFactory.Create(cylinder)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener el registro." + "\n" + "Detalles del Error: " + ex));
            }
        }
        [Authorize(Roles = "admin")]
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var cylinderService = new CylinderService();
                var cylinder = cylinderService.GetById(id);
                if (cylinder == null) return NotFound();
                var deleted = cylinderService.Delete(cylinder);
                return deleted ? (IHttpActionResult)Ok() : Conflict();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar eliminar el registro." + "\n" + "Detalles del Error: " + ex));
            }
        }
        
        [HttpPut]
        [Route("changeState/{id}/{state}")]
        public IHttpActionResult ChangeCylinderState(int id, int state) {
            try
            {
                var cylinderService = new CylinderService();
                if (cylinderService.GetById(id) == null) return NotFound();
                var modified = cylinderService.ChangeCylinderState(id, state == 1);
                return modified ? (IHttpActionResult)Ok() : Conflict();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener el registro." + "\n" + "Detalles del Error: " + ex));
            }
        }
    }
}