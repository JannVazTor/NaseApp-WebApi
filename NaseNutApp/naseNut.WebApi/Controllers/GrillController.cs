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
using naseNut.WebApi.Models.Enum;

namespace naseNut.WebApi.Controllers
{
    [RoutePrefix("api/grill")]
    public class GrillController: BaseApiController
    {
        private NaseNEntities _db = new NaseNEntities();
        [Authorize(Roles = "admin,grillUser")]
        [HttpPost]
        public IHttpActionResult SaveGrill(AddOrUpdateGrillBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var grillService = new GrillService();
                if (model.Folio != 0 && grillService.GetByFolio(model.Folio) != null) return Conflict();
                var grill = new Grill
                {
                    DateCapture = model.DateCapture, 
                    Size = model.Size,
                    Sacks = model.Sacks,
                    Kilos = model.Kilos,
                    Quality = model.Quality,
                    VarietyId = model.VarietyId,
                    ProducerId = model.ProducerId,
                    BatchId = model.BatchId,
                    Status = true,
                    Folio = model.Folio,
                    HarvestSeasonId = _db.HarvestSeasons.FirstOrDefault(h => h.Active).Id
                };  
                var saved = grillService.Save(grill);
                return saved ? (IHttpActionResult)Ok() : BadRequest();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar guardar el registro." + "\n" + "Detalles del Error: " + ex));
            }
        }
        [Authorize(Roles = "admin,grillUser,qualityUser")]
        [HttpGet]
        public IHttpActionResult GetAllGrills()
        {
            try
            {
                var grills = _db.Grills.Where(g => g.HarvestSeason.Active).ToList();
                return grills.Count != 0 ? (IHttpActionResult)Ok(TheModelFactory.Create(grills)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener el registro." + "\n" + "Detalles del Error: " + ex));
            }
        }
        [Authorize(Roles = "admin,grillUser")]
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteGrill(int id)
        {
            try
            {
                var grillService = new GrillService();
                var grill = grillService.GetById(id);
                if (grill == null) return NotFound();
                var deleted = grillService.Delete(grill);
                return deleted ? (IHttpActionResult)Ok() : InternalServerError();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar eliminar el registro." + "\n" + "Detalles del Error: " + ex));
            }
        }
        [Authorize(Roles = "admin,grillUser")]
        [HttpPut]
        [Route("addGrillToReception/{grillId}/{receptionId}")]
        public IHttpActionResult AddReceptionToGrill(int grillId, int receptionId)
        {
            try
            {
                var receptionService = new ReceptionService();
                var grillService = new GrillService();
                var reception = receptionService.GetById(receptionId);
                var grill = grillService.GetById(grillId);
                if (reception == null || grill == null) return NotFound();
                var added = grillService.AddGrillToReception(reception.Id, grill.Id);
                if (!added) return BadRequest();
                return Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
               "Ocurrio un error al intentar relacionar los registros." + "\n" + "Detalles del Error: " + ex));
            }
        }
        [Authorize(Roles = "admin,grillUser")]
        [HttpPut]
        [Route("removeGrillToReception/{grillId}/{receptionId}")]
        public IHttpActionResult RemoveReceptionToGrill(int grillId, int receptionId)
        {
            try
            {
                var receptionService = new ReceptionService();
                var grillService = new GrillService();
                var reception = receptionService.GetById(receptionId);
                var grill = grillService.GetById(grillId);
                if (reception == null || grill == null) return NotFound();
                var removed = grillService.RemoveGrillToReception(reception.Id, grill.Id);
                if (!removed) return BadRequest();
                return Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
               "Ocurrio un error al intentar remover el registro." + "\n" + "Detalles del Error: " + ex));
            }
        }
        [Authorize(Roles = "admin,grillUser")]
        [HttpPut]
        [Route("changeStatus/{id}/{status}")]
        public IHttpActionResult UpdateStatus(int id, int status) 
        {
            try
            {
                if (status != 0 && status != 1) return BadRequest();
                var grillService = new GrillService();
                if (grillService.GetById(id) == null) return NotFound();
                var modified = grillService.UpdateStatus(id, status == 1);
                return modified ? (IHttpActionResult) Ok() : BadRequest();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar modificar el registro." + "\n" + "Detalles del Error: " + ex));
            }
        }
        [Authorize(Roles = "admin,grillUser")]
        [HttpGet]
        [Route("getAllCurrentInv")]
        public IHttpActionResult GetAllCurrentInventory()
        {
            try
            {
                var grills = _db.Grills.Where(g => g.Status && g.HarvestSeason.Active).ToList();
                return grills.Count != 0 ? (IHttpActionResult)Ok(TheModelFactory.Create(grills)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar recuperar los registro." + "\n" + "Detalles del Error: " + ex));
            }
        }
        [Authorize(Roles = "admin,grillUser")]
        [HttpPost]
        [Route("Issue")]
        public IHttpActionResult SaveIssue(SaveIssueBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var grillIssueService = new GrillIssueService();
                if (model.Remission != 0 && grillIssueService.GetByRemission(model.Remission) != null) return Conflict();
                var grillIssue = new GrillIssue
                {
                    DateCapture = model.DateCapture,
                    Truck = model.Truck,
                    Driver = model.Driver,
                    Box = model.Box,
                    Remission = model.Remission,
                    HarvestSeasonId = _db.HarvestSeasons.FirstOrDefault(h => h.Active).Id
                };
                var saved = grillIssueService.Save(grillIssue, model.GrillsIds);
                return saved ? (IHttpActionResult)Ok() : BadRequest();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar guardar el registro." + "\n" + "Detalles del Error: " + ex));
            }
        }
        [Authorize(Roles = "admin,grillUser")]
        [HttpGet]
        [Route("GetAllIssues")]
        public IHttpActionResult GetAllIssues()
        {
            try
            {
                var issues = _db.GrillIssues.Where(gi => gi.HarvestSeason.Active).ToList();
                return issues.Count != 0 ? (IHttpActionResult)Ok(TheModelFactory.Create(issues)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener los registros." + "\n" + "Detalles del Error: " + ex));
            }
        }
        [Authorize(Roles = "admin,grillUser")]
        [HttpDelete]
        [Route("grillIssue/{id}")]
        public IHttpActionResult DeleteGrillIssue(int id)
        {
            try
            {
                var grillIssueService = new GrillIssueService();
                var grillIssue = grillIssueService.GetById(id);
                if (grillIssue == null) return NotFound();
                var deleted = grillIssueService.Delete(id);
                return deleted ? (IHttpActionResult)Ok() : Conflict();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener los registros." + "\n" + "Detalles del Error: " + ex));
            }
        }
        [Authorize(Roles = "admin,grillUser")]
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult UpdateGrill(int id, AddOrUpdateGrillBindingModel model) {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var grillService = new GrillService();
                if (grillService.GetById(id) == null) return NotFound();
                var modified = grillService.Update(id, model);
                return modified ? (IHttpActionResult)Ok() : Conflict();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar actualizar la parrilla." + "\n" + "Detalles del Error: " + ex));
            }
        }
        [Authorize(Roles = "admin,grillUser")]
        [HttpPut]
        [Route("removeGrillFromGrillIssue/{id}")]
        public IHttpActionResult RemoveGrillFromGrillIssue(int id) {
            try
            {
                var grillService = new GrillService();
                if (grillService.GetById(id) == null) return NotFound();
                var removed = grillService.RemoveGrillFromGrillIssue(id);
                return removed ? (IHttpActionResult)Ok() : Conflict();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar cambiar el estado de la parrilla." + "\n" + "Detalles del Error: " + ex));
            }
        }
    }
}