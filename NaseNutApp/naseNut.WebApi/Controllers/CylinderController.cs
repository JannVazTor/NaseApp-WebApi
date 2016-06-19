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
    [RoutePrefix("api/cylinder")]
    public class CylinderController:BaseApiController
    {
        [HttpPost]
        public IHttpActionResult SaveProducer(AddCylinderBindingModel model)
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
                    CylinderName = model.CylinderName
                };
                var saved = cylinderService.Save(cylinder);
                return saved ? (IHttpActionResult)Ok() : BadRequest();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar guardar el registro." + "\n" + "Detalles del Error: " + ex));
            }
        }

        [HttpGet]
        [Route("getAll")]
        public IHttpActionResult GetAllProducers()
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

        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteCylinder(int id)
        {
            try
            {
                var cylinderService = new CylinderService();
                var cylinder = cylinderService.GetById(id);
                if (cylinder == null) return NotFound();
                var deleted = cylinderService.Delete(cylinder);
                return deleted ? (IHttpActionResult)Ok() : InternalServerError();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar eliminar el registro." + "\n" + "Detalles del Error: " + ex));
            }
        }
    }
}