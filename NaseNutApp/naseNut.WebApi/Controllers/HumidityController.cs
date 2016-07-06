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
    [RoutePrefix("api/humidity")]
    public class HumidityController : BaseApiController
    {
        private NaseNEntities _db = new NaseNEntities();
        [HttpPost]
        [Route("saveHumidity")]
        public IHttpActionResult SaveHumidity(AddHumidityBindingModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var cylinderService = new CylinderService();
                var cylinderId = cylinderService.GetId(model.CylinderName);
                var humidityService = new HumidityService();
                var humidity = new Humidity
                {
                    DateCapture = DateTime.Now,
                    HumidityPercent = model.HumidityPercent,
                    CylinderId = cylinderId,
                    ReceptionId = model.ReceptionId
                };
                var saved = humidityService.Save(humidity);
                return saved ? (IHttpActionResult)Ok() : BadRequest();
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
        [Route("getAll")]
        public IHttpActionResult GetAllHumidities()
        {
            try
            {
                var humidities = _db.Humidities.ToList();
                return humidities.Count != 0 ? (IHttpActionResult)Ok(TheModelFactory.CreateC(humidities)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener los registros de humedad." + "\n" + "Detalles del Error: " + ex));
            }
        }
    }
}
