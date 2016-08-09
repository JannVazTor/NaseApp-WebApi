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

namespace naseNut.WebApi.Controllers
{
    [Authorize(Roles = "admin")]
    [RoutePrefix("api/cylinder")]
    public class CylinderController:BaseApiController
    {
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
                var cylinder = new Cylinder
                {
                    CylinderName = model.CylinderName,
                    Active = true
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

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            try
            {
                var cylinderService = new CylinderService();
                var cylinder = cylinderService.GetAll();
                return cylinder != null ? (IHttpActionResult)Ok(TheModelFactory.Create(cylinder)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener el registro." + "\n" + "Detalles del Error: " + ex));
            }
        }
        [HttpGet]
        [Route("GetAllActive")]
        public IHttpActionResult GetAllActive()
        {
            try
            {
                var cylinderService = new CylinderService();
                var cylinder = cylinderService.GetAllActive();
                return cylinder != null ? (IHttpActionResult)Ok(TheModelFactory.Create(cylinder)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener el registro." + "\n" + "Detalles del Error: " + ex));
            }
        }

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