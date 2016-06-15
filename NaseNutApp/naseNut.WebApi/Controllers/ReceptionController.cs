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
    [RoutePrefix("api/reception")]
    public class ReceptionController : BaseApiController
    {
        [HttpPost]
        public IHttpActionResult SaveReception(AddReceptionBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var receptionService = new ReceptionService();
                var reception = new Reception
                {
                    Variety = model.Variety,
                    ReceivedFromField = model.ReceivedFromField,
                    CylinderNumber = model.CylinderNumber,
                    FieldName = model.FieldName,
                    CarRegistration = model.CarRegistration,
                    EntryDate = model.EntryDate,
                    IssueDate = model.IssueDate,
                    HeatHoursDtrying = model.HeatHoursDtrying,
                    HumidityPercent = model.HumidityPercent,
                    Observations = model.Observations,
                    ProducerId = model.ProducerId
                };
                var saved = receptionService.Save(reception);
                return saved ? (IHttpActionResult)Ok() : BadRequest();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar guardar la Recepcion." + "\n" + "Detalles del Error: " + ex));
            }
        }
    }
}