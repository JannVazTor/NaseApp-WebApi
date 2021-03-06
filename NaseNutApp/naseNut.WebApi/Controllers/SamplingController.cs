﻿using System;
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
    [Authorize(Roles = "admin,qualityUser,grillUser")]
    [RoutePrefix("api/sampling")]
    public class SamplingController : BaseApiController
    {
        private NaseNEntities _db = new NaseNEntities();
        [HttpPost]
        public IHttpActionResult SaveGrillSampling(AddSamplingBindingModel model)
        {
            if (!ModelState.IsValid || (model.GrillId == null && model.ReceptionEntryId == null))
            {
                return BadRequest();
            }
            try
            {
                var samplingService = new SamplingService();
                var sampling = new Sampling
                {
                    DateCapture = model.DateCapture,
                    SampleWeight = model.SampleWeight,
                    HumidityPercent = model.HumidityPercent,
                    WalnutNumber = model.WalnutNumber,
                    Performance = (model.TotalWeightOfEdibleNuts / model.SampleWeight) * 100,
                    TotalWeightOfEdibleNuts = model.TotalWeightOfEdibleNuts,
                    GrillId = model.GrillId,
                    ReceptionEntryId = model.ReceptionEntryId
                };
                var saved = samplingService.Save(sampling);
                return saved ? (IHttpActionResult)Ok() : BadRequest();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar guardar el muestreo." + "\n" + "Detalles del Error: " + ex));
            }
        }

        [HttpGet]
        [Route("Grills")]
        public IHttpActionResult GetAllGrillSamplings()
        {
            try
            {
                var samplings = _db.Samplings.Where(g => g.GrillId != null && g.Grill.HarvestSeason.Active).ToList();
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
                var samplings = _db.ReceptionEntries.Where(r => (r.Samplings.Any() || r.NutTypes.Any()) && r.HarvestSeason.Active).ToList();
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
                var sampling = samplingService.GetById(id);
                if (sampling == null) return NotFound();
                var deleted = samplingService.Delete(id, false);
                return deleted ? (IHttpActionResult)Ok() : Conflict();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar eliminar el registro." + "\n" + "Detalles del Error: " + ex));
            }
        }

        [HttpDelete]
        [Route("processResult/{id}")]
        public IHttpActionResult DeleteProcessResult(int id)
        {
            try
            {
                var receptionEntryService = new ReceptionEntryService();
                var receptionEntry = receptionEntryService.GetById(id);
                if (receptionEntry == null) return NotFound();
                var samplingService = new SamplingService();
                var deleted = samplingService.Delete(id, true);
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
            if (!ModelState.IsValid || model == null)
            {
                return BadRequest();
            }
            try
            {
                var samplingService = new SamplingService();
                var update = samplingService.Update(model, false);
                return update ? (IHttpActionResult)Ok() : InternalServerError();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar eliminar el registro." + "\n" + "Detalles del Error: " + ex));
            }
        }
        [HttpPut]
        [Route("processResult")]
        public IHttpActionResult UpdateProcessResultSampling(UpdateGrillSamplingBindingModel model)
        {
            if (!ModelState.IsValid || model == null)
            {
                return BadRequest();
            }
            try
            {
                var samplingService = new SamplingService();
                var update = samplingService.Update(model, true);
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