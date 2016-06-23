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
    [RoutePrefix("api/grill")]
    public class GrillController: BaseApiController
    {
        [HttpPost]
        public IHttpActionResult SaveGrill(AddGrillBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var grillService = new GrillService();
                var grill = new Grill
                {
                    DateCapture = model.DateCapture, 
                    ReceptionId = model.ReceptionId,
                    Size = model.Size,
                    Sacks = model.Sacks,
                    Kilos = model.Kilos,
                    Quality = model.Quality,
                    Variety = model.Variety,
                    Producer = model.Producer,
                    FieldName = model.FieldName,
                    Cylinder = model.CylinderName,
                    NutAmount = model.NutAmount,
                    SelectPercentage = model.SelectPercentage,
                    TotalPercentage = model.TotalPercentage,
                    Humidity = model.Humidity,
                    Status = Convert.ToBoolean(GrillStatus.Entry)
                };
                var saved = grillService.Save(grill);
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
        public IHttpActionResult GetAllGrills()
        {
            try
            {
                var grillService = new GrillService();
                var grill = grillService.GetAll();
                return grill != null ? (IHttpActionResult)Ok(TheModelFactory.Create(grill)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener el registro." + "\n" + "Detalles del Error: " + ex));
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteGrill(int id)
        {
            try
            {
                var grillService = new GrillService();
                var grill = grillService.GetById(id);
                if (grill == null) return NotFound();
                var deleted = grillService.Delete(grill);
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