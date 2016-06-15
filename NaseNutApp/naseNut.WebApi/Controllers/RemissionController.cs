using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using naseNut.WebApi.Models.Business.Services;
using naseNut.WebApi.Models.Entities;
using naseNut.WebApi.Models.BindingModels;

namespace naseNut.WebApi.Controllers
{
    [RoutePrefix("api/remission")]
    public class RemissionController : BaseApiController
    {
        [HttpPost]
        public IHttpActionResult SaveRemission(AddRemissionBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var remissionService = new RemissionService();
                var remission = new Remission
                {
                    Cultivation = model.Cultivation,
                    Batch = model.Batch,
                    Quantity = model.Quantity,
                    Butler = model.Butler,
                    TransportNumber = model.TransportNumber,
                    Driver = model.Driver,
                    Elaborate = model.Elaborate,
                    ReceptionId = model.ReceptionId
                };
                var saved = remissionService.Save(remission);
                return saved ? (IHttpActionResult)Ok() : BadRequest();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar guardar la Remision." + "\n" + "Detalles del Error: " + ex));
            }
        }

        [HttpGet]
        [Route("getAll")]
        public IHttpActionResult GetAllRemissions()
        {
            try
            {
                var remissionService = new RemissionService();
                var remissions = remissionService.GetAll();
                return remissions != null ? (IHttpActionResult) Ok(TheModelFactory.Create(remissions)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener las remisiones." + "\n" + "Detalles del Error: " + ex));
            }
        }
    }
}
