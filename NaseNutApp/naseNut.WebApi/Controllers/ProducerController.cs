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
    [RoutePrefix("api/producer")]
    public class ProducerController : BaseApiController
    {
        [HttpPost]
        public IHttpActionResult SaveProducer(AddProducerBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var producerService = new ProducerService();
                var producer = new Producer
                {
                    ProducerName = model.ProducerName
                };
                var saved = producerService.Save(producer);
                return saved ? (IHttpActionResult) Ok() : BadRequest();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar guardar al Productor." + "\n" + "Detalles del Error: " + ex));
            }
        }

        [HttpGet]
        [Route("getAll")]
        public IHttpActionResult GetAllProducers()
        {
            try
            {
                var producerService = new ProducerService();
                var producers = producerService.GetAll();
                return producers != null ? (IHttpActionResult) Ok(TheModelFactory.Create(producers)) : Ok(); 
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener los productores." + "\n" + "Detalles del Error: " + ex));
            }
        }
    }
}
