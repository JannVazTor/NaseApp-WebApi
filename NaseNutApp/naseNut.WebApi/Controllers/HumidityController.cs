using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using naseNut.WebApi.Models.BindingModels;
using naseNut.WebApi.Models.Business.Services;
using naseNut.WebApi.Models.Entities;

namespace naseNut.WebApi.Controllers
{
    [RoutePrefix("api/humidity")]
    public class HumidityController : BaseApiController
    {
        [HttpPost]
        public IHttpActionResult SaveHumidity(AddHumidityBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var humidityService = new HumidityService();
                var humidity = new Humidity
                {
                    HumidityPercent = model.HumidityPercent,
                    CylinderReceptionId = model.CylinderReceptionId
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

        [HttpGet]
        [Route("getAll")]
        public IHttpActionResult GetAllHumidity()
        {
            try
            {
                var humidityService = new HumidityService();
                var humidity = humidityService.GetAll();
                return humidity != null ? (IHttpActionResult)Ok(TheModelFactory.Create(humidity)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener los registros de humedad." + "\n" + "Detalles del Error: " + ex));
            }
        }
    }
}
