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
        public IHttpActionResult SaveSampling(AddSamplingBindingModel model)
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
                    Id = model.GrillId,
                    DateCapture = model.DateCapture,
                    SampleWeight = model.SampleWeight,
                    HumidityPercent = model.HumidityPercent,
                    WalnutNumber = model.WalnutNumber,
                    Performance = model.Performance,
                    TotalWeightOfEdibleNuts = model.TotalWeightOfEdibleNuts
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

        [HttpGet]
        [Route("getAll")]
        public IHttpActionResult GetAllSamplings()
        {
            try
            {
                var samplings = _db.Samplings.ToList();
                return samplings.Count != 0 ? (IHttpActionResult)Ok(TheModelFactory.Create(samplings)) : Ok();
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
                var deleted = samplingService.Delete(sampling);
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