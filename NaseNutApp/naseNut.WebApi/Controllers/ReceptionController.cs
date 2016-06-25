using System;
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
        private NaseNEntities _db = new NaseNEntities();
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
                    FieldName = model.FieldName,
                    CarRegistration = model.CarRegistration,
                    EntryDate = model.EntryDate,
                    IssueDate = model.IssueDate,
                    HeatHoursDtrying = model.HeatHoursDrying,
                    HumidityPercent = model.HumidityPercent,
                    Observations = model.Observations,
                    ProducerId = model.ProducerId,
                    Folio = model.Folio
                };
                var saved = receptionService.SaveToCylinder(reception, model.CylinderId);
                return saved ? (IHttpActionResult)Ok() : BadRequest();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar guardar la recepcion." + "\n" + "Detalles del Error: " + ex));
            }
        }

        [HttpGet]
        [Route("getAll")]
        public IHttpActionResult GetAllReceptions()
        {
            try
            {
                var receptions = _db.Receptions.ToList();
                return receptions.Count != 0 ? (IHttpActionResult)Ok(TheModelFactory.Create(receptions)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener las recepciones." + "\n" + "Detalles del Error: " + ex));
            }
        }

        [HttpPut]
        [Route("addReceptionToGrill/{receptionId}/{grillId}")]
        public IHttpActionResult AddReceptionToGrill(int receptionId, int grillId )
        {
            try
            {
                var receptionService = new ReceptionService();
                var grillService = new GrillService();
                var reception = receptionService.GetById(receptionId);
                var grill = grillService.GetById(grillId);
                if (reception == null || grill == null) return NotFound();
                var added = receptionService.AddReceptionToGrill(reception.Id, grill.Id);
                if (!added) return BadRequest();
                return Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
               "Ocurrio un error al intentar relacionar los registros." + "\n" + "Detalles del Error: " + ex));
            }
        }

        [HttpPut]
        [Route("removeReceptionToGrill/{receptionId}/{grillId}")]
        public IHttpActionResult RemoveReceptionToGrill(int receptionId, int grillId)
        {
            try
            {
                var receptionService = new ReceptionService();
                var grillService = new GrillService();
                var reception = receptionService.GetById(receptionId);
                var grill = grillService.GetById(grillId);
                if (reception == null || grill == null) return NotFound();
                var removed = receptionService.RemoveReceptionToGrill(reception.Id, grill.Id);
                if (!removed) return BadRequest();
                return Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
               "Ocurrio un error al intentar remover el registro." + "\n" + "Detalles del Error: " + ex));
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteReception(int id)
        {
            try
            {
                var receptionService = new ReceptionService();
                var producer = receptionService.GetById(id);
                if (producer == null) return NotFound();
                var deleted = receptionService.Delete(producer);
                return deleted ? (IHttpActionResult)Ok() : InternalServerError();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar eliminar el registro." + "\n" + "Detalles del Error: " + ex));
            }
        }

        [HttpPut]
        [Route("{Id}")]
        public IHttpActionResult UpdateReception(int Id,AddReceptionBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            
            try
            {
                var receptionService = new ReceptionService();
                var reception = _db.Receptions.Find(Id);
                if(reception != null)
                {
                    reception.CarRegistration = model.CarRegistration;
                    reception.EntryDate = model.EntryDate;
                    reception.FieldName = model.FieldName;
                    reception.HeatHoursDtrying = model.HeatHoursDrying;
                    reception.HumidityPercent = model.HumidityPercent;
                    reception.IssueDate = model.IssueDate;
                    reception.ProducerId = model.ProducerId;
                    reception.ReceivedFromField = model.ReceivedFromField;
                    reception.Variety = model.Variety;

                    var update = receptionService.Update(reception, model.CylinderId);
                    return update ? (IHttpActionResult)Ok() : BadRequest();
                }else
                {
                    return BadRequest();
                }
               
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar actgualizar la recepcion." + "\n" + "Detalles del Error: " + ex));
            }
        }
    }
}