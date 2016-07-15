using naseNut.WebApi.Models.BindingModels;
using naseNut.WebApi.Models.Business.Services;
using naseNut.WebApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace naseNut.WebApi.Controllers
{
    [RoutePrefix("api/receptionEntry")]
    public class ReceptionEntryController : BaseApiController
    {
        NaseNEntities _db = new NaseNEntities();
        [HttpPost]
        public IHttpActionResult SaveReceptionEntry(AddReceptionEntryBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                return BadRequest();
            }
            try
            {
                var receptionEntryService = new ReceptionEntryService();
                var receptions = model.Receptions.Select(m => new Reception
                {
                    ReceivedFromField = m.ReceivedFromField,
                    FieldName = m.FieldName,
                    CarRegistration = m.CarRegistration,
                    EntryDate = m.EntryDate == "" ? DateTime.Now : m.EntryDate.ConvertToDate(),
                    IssueDate = m.IssueDate,
                    HeatHoursDtrying = m.HeatHoursDrying,
                    HumidityPercent = m.HumidityPercent,
                    Observations = m.Observations,
                    Folio = m.Folio
                }).ToList();
                var saved = receptionEntryService.Save(receptions, model.CylinderId, model.VarietyId, model.ProducerId);
                return saved ? (IHttpActionResult)Ok() : BadRequest();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar guardar la recepcion." + "\n" + "Detalles del Error: " + ex));
            }
        }
        [HttpGet]
        public IHttpActionResult GetAllReceptionEntries() {
            try
            {
                var receptionEntries = _db.ReceptionEntries.Where(r => !r.Cylinder.Active).ToList();
                return receptionEntries != null ? (IHttpActionResult)Ok(TheModelFactory.Create(receptionEntries)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener las recepciones." + "\n" + "Detalles del Error: " + ex));
            }
        }
    }
}