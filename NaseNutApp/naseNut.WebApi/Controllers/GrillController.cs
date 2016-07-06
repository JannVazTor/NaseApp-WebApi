using System;
using System.Collections.Generic;
using System.Globalization;
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
    [RoutePrefix("api/grill")]
    public class GrillController: BaseApiController
    {
        private NaseNEntities _db = new NaseNEntities();
        [HttpPost]
        public IHttpActionResult SaveGrill(AddGrillBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var grillService = new GrillService();
                var grill = new Grill
                {
                    DateCapture = model.DateCapture.ConvertToDate(), 
                    Size = model.Size,
                    Sacks = model.Sacks,
                    Kilos = model.Kilos,
                    Quality = model.Quality,
                    Variety = model.Variety,
                    Producer = model.Producer,
                    FieldName = model.FieldName,
                    Status = Convert.ToBoolean(GrillStatus.Entry)
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

        [HttpGet]
        [Route("getAll")]
        public IHttpActionResult GetAllGrills()
        {
            try
            {
                var grills = _db.Grills.ToList();
                return grills.Count != 0 ? (IHttpActionResult)Ok(TheModelFactory.Create(grills)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener el registro." + "\n" + "Detalles del Error: " + ex));
            }
        }
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteGrill(int id)
        {
            try
            {
                var grillService = new GrillService();
                var grill = grillService.GetById(id);
                if (grill == null) return NotFound();
                if(grill.Sampling != null)
                {
                    var samplingService = new SamplingService();
                    var sampling = samplingService.GetById(id);
                    var deleteSampling = samplingService.Delete(sampling);
                }
                var deleted = grillService.Delete(grill);
                return deleted ? (IHttpActionResult)Ok() : InternalServerError();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar eliminar el registro." + "\n" + "Detalles del Error: " + ex));
            }
        }

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

        [HttpGet]
        [Route("getAllCurrentInv")]
        public IHttpActionResult GetAllCurrentInventory()
        {
            try
            {
                var grills = _db.Grills.Where(g => g.Status).ToList();
                return grills.Count != 0 ? (IHttpActionResult)Ok(TheModelFactory.Create(grills)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar recuperar los registro." + "\n" + "Detalles del Error: " + ex));
            }
        }

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
                var grillService = new GrillService();
                var grillIssue = new GrillIssue
                {
                    DateCapture = model.DateCapture.ConvertToDate(),
                    Truck = model.Truck,
                    Driver = model.Driver,
                    Box = model.Box,
                    Remission = model.Remission
                };
                var saved = grillService.SaveIssue(grillIssue, model.GrillIds);
                return saved ? (IHttpActionResult)Ok() : BadRequest();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar guardar el registro." + "\n" + "Detalles del Error: " + ex));
            }
        }

        [HttpGet]
        [Route("GetAllIssues")]
        public IHttpActionResult GetAllIssues()
        {
            try
            {
                var issues = _db.GrillIssues.ToList();
                return issues.Count != 0 ? (IHttpActionResult)Ok(TheModelFactory.Create(issues)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener los registros." + "\n" + "Detalles del Error: " + ex));
            }
        }
    }
}