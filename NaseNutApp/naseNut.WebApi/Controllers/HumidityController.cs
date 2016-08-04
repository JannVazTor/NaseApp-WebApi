using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using naseNut.WebApi.Models.BindingModels;
using naseNut.WebApi.Models.Business.Services;
using naseNut.WebApi.Models.Entities;
using Newtonsoft.Json.Linq;

namespace naseNut.WebApi.Controllers
{
    [Authorize(Roles = "admin,humidityUser")]
    [RoutePrefix("api/humidity")]
    public class HumidityController : BaseApiController
    {
        private NaseNEntities _db = new NaseNEntities();
        [HttpPost]
        public IHttpActionResult SaveHumidity(AddHumidityBindingModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var humidity = new Humidity
                {
                   HumidityPercent = model.HumidityPercent,
                   DateCapture = DateTime.Now,
                   ReceptionEntryId = model.ReceptionEntryId
                };
                var humidityService = new HumidityService();
                var saved = humidityService.Save(humidity);
                return saved ? (IHttpActionResult)Ok() : Conflict();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar guardar el registro de humedad." + "\n" + "Detalles del Error: " + ex));
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteHumidity(int id)
        {
            try
            {
                var humidityService = new HumidityService();
                var humidity = humidityService.GetById(id);
                if (humidity == null) return NotFound();
                var deleted = humidityService.Delete(humidity);
                return deleted ? (IHttpActionResult) Ok() : InternalServerError();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar eliminar el registro." + "\n" + "Detalles del Error: " + ex));
            }
        }

        [HttpGet]
        [Route("getByReceptionEntry/{id}")]
        public IHttpActionResult GetAllHumidities(int id)
        {
            try
            {
                var humidities = _db.ReceptionEntries.Where(r => r.Id == id).FirstOrDefault();
                return humidities != null ? (IHttpActionResult)Ok(TheModelFactory.Create(humidities)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener los registros de humedad." + "\n" + "Detalles del Error: " + ex));
            }
        }
        [HttpGet]
        [Route("getLastHumiditiesSamplings")]
        public IHttpActionResult GetLastSamplings() {
            try
            {
                var lastSamplings = _db.ReceptionEntries.Where(r => r.Humidities.Any()).Select(r => r.Humidities.OrderByDescending(d => d.DateCapture).FirstOrDefault()).ToList();
                return lastSamplings.Count != 0 ? (IHttpActionResult)Ok(TheModelFactory.CreateH(lastSamplings)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener los registros de humedad." + "\n" + "Detalles del Error: " + ex));
            }
        }

        [HttpGet]
        public IHttpActionResult GetAll() {
            try
            {
                var humidities = _db.Humidities.ToList();
                return humidities.Count != 0 ? (IHttpActionResult)Ok(TheModelFactory.CreateH(humidities)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener los registros de humedad." + "\n" + "Detalles del Error: " + ex));
            }
        }
    }
}
