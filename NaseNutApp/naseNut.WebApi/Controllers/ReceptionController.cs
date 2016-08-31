using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using naseNut.WebApi.Models.BindingModels;
using naseNut.WebApi.Models.Business.Services;
using naseNut.WebApi.Models.Entities;
using System.Data.Entity;
using System.Collections.Generic;

namespace naseNut.WebApi.Controllers
{
    [Authorize(Roles = "admin,remRecepUser")]
    [RoutePrefix("api/reception")]
    public class ReceptionController : BaseApiController
    {
        private NaseNEntities _db = new NaseNEntities();
        [HttpGet]
        [Route("getAll")]
        public IHttpActionResult GetAllReceptions()
        {
            try
            {
                var receptions = _db.Receptions.Where(r => !r.ReceptionEntry.Cylinder.Active).ToList();
                return receptions.Count != 0 ? (IHttpActionResult)Ok(TheModelFactory.Create(receptions)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener las recepciones." + "\n" + "Detalles del Error: " + ex));
            }
        }

        [HttpPut]
        [Route("addReceptionToGrill/{receptionId}/{grillId}")]
        public IHttpActionResult AddReceptionToGrill(int receptionId, int grillId )
        {
            try
            {
                var receptionService = new ReceptionService();
                var grillService = new GrillService();
                var reception = receptionService.GetById(receptionId);
                var grill = grillService.GetById(grillId);
                if (reception == null || grill == null) return NotFound();
                var added = receptionService.AddReceptionToGrill(reception.Id, grill.Id);
                if (!added) return BadRequest();
                return Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
               "Ocurrio un error al intentar relacionar los registros." + "\n" + "Detalles del Error: " + ex));
            }
        }

        [HttpPut]
        [Route("removeReceptionToGrill/{receptionId}/{grillId}")]
        public IHttpActionResult RemoveReceptionToGrill(int receptionId, int grillId)
        {
            try
            {
                var receptionService = new ReceptionService();
                var grillService = new GrillService();
                var reception = receptionService.GetById(receptionId);
                var grill = grillService.GetById(grillId);
                if (reception == null || grill == null) return NotFound();
                var removed = receptionService.RemoveReceptionToGrill(reception.Id, grill.Id);
                if (!removed) return BadRequest();
                return Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
               "Ocurrio un error al intentar remover el registro." + "\n" + "Detalles del Error: " + ex));
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteReception(int id)
        {
            try
            {
                var receptionService = new ReceptionService();
                var producer = receptionService.GetById(id);
                if (producer == null) return NotFound();
                var reception = receptionService.GetById(id);
                var deleted = receptionService.Delete(producer, _db.ReceptionEntries.First(r => r.Id == reception.ReceptionEntryId).Receptions.Count == 1);
                return deleted ? (IHttpActionResult)Ok() : InternalServerError();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar eliminar el registro." + "\n" + "Detalles del Error: " + ex));
            }
        }

        [HttpPut]
        [Route("{Id}")]
        public IHttpActionResult UpdateReception(int Id, AddOrUpdateReceptionBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var receptionService = new ReceptionService();
                if (receptionService.GetById(Id) == null) return NotFound();
                var update = receptionService.Update(Id, model);
                return update?(IHttpActionResult)Ok() : BadRequest();  
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar actgualizar la recepcion." + "\n" + "Detalles del Error: " + ex));
            }
        }

        [HttpGet]
        [Route("getReceptionsByCylinder")]
        public IHttpActionResult GetReceptionsByCylinder(int cylinderId)
        {
            try
            {
                var receptionEntryService = new ReceptionEntryService();
                var receptions = receptionEntryService.GetByCylinderId(cylinderId).ToList();
                return receptions.Count != 0 ? (IHttpActionResult)Ok(TheModelFactory.CreateReceptionId(receptions)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener las recepciones que contiene el cilindro." + "\n" + "Detalles del Error: " + ex));
            }
        }
    }
}