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
    [RoutePrefix("api/sampling")]
    public class SamplingController : BaseApiController
    {
        private NaseNEntities _db = new NaseNEntities();
        [HttpPost]
        [Route("grill")]
        public IHttpActionResult SaveGrillSampling(AddGrillSamplingBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var samplingService = new SamplingService();
                var sampling = new Sampling
                {
                    DateCapture = model.DateCapture.ConvertToDate(),
                    SampleWeight = model.SampleWeight,
                    HumidityPercent = model.HumidityPercent,
                    WalnutNumber = model.WalnutNumber,
                    Performance = model.Performance,
                    TotalWeightOfEdibleNuts = model.TotalWeightOfEdibleNuts,
                    GrillId = model.GrillId
                };
                var saved = samplingService.Save(sampling);
                return saved ? (IHttpActionResult)Ok() : BadRequest();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar guardar el registro." + "\n" + "Detalles del Error: " + ex));
            }
        }
        [HttpPost]
        [Route("receptionEntry")]
        public IHttpActionResult SaveReceptionEntrySampling(AddReceptionEntrySamplingBindingModel model)
        {
            if (!ModelState.IsValid || model == null || (model.NutTypes == null || model.NutTypes.Count == 0))
            {
                return BadRequest();
            }
            try
            {
                var samplingService = new SamplingService();
                var nutTypes = new List<NutType>();
                var sampling = new Sampling
                {
                    DateCapture = model.DateCapture.ConvertToDate(),
                    SampleWeight = model.SampleWeight,
                    HumidityPercent = model.HumidityPercent,
                    WalnutNumber = model.WalnutNumber,
                    Performance = model.Performance,
                    TotalWeightOfEdibleNuts = model.TotalWeightOfEdibleNuts,
                    ReceptionEntryId = model.ReceptionEntryId
                };
                foreach (var item in model.NutTypes)
                {
                    nutTypes.Add(new NutType
                    {
                        NutType1 = item.NutType,
                        Sacks = item.Sacks,
                        Kilos = item.Kilos,
                        ReceptionEntryId = model.ReceptionEntryId
                    }); 
                }
                var saved = samplingService.Save(sampling, nutTypes, model.ReceptionEntryId);
                return saved ? (IHttpActionResult)Ok() : BadRequest();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar guardar el registro." + "\n" + "Detalles del Error: " + ex));
            }
        }
        [HttpGet]
        [Route("Grills")]
        public IHttpActionResult GetAllGrillSamplings()
        {
            try
            {
                var samplings = _db.Samplings.Where(g => g.GrillId != null).ToList();
                return samplings.Count != 0 ? (IHttpActionResult)Ok(TheModelFactory.CreateSampling(samplings)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener el registro." + "\n" + "Detalles del Error: " + ex));
            }
        }
        [HttpGet]
        [Route("Receptions")]
        public IHttpActionResult GetAllReceptionSamplings()
        {
            try
            {
                var samplings = _db.Samplings.Where(g => g.ReceptionEntryId != null).ToList();
                return samplings.Count != 0 ? (IHttpActionResult)Ok(TheModelFactory.CreateReception(samplings)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener el registro." + "\n" + "Detalles del Error: " + ex));
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Deletesampling(int id)
        {
            try
            {
                var samplingService = new SamplingService();
                var cylinderService = new CylinderService();
                var sampling = samplingService.GetById(id);
                if (sampling == null) return NotFound();
                var deleted = samplingService.Delete(id);
                return deleted ? (IHttpActionResult)Ok() : Conflict();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar eliminar el registro." + "\n" + "Detalles del Error: " + ex));
            }
        }

        [HttpPut]
        public IHttpActionResult Updatesampling(UpdateGrillSamplingBindingModel model)
        {
            try
            {
                var samplingService = new SamplingService();
                var update = samplingService.Update(model);
                return update ? (IHttpActionResult)Ok() : InternalServerError();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar eliminar el registro." + "\n" + "Detalles del Error: " + ex));
            }
        }
    }
}