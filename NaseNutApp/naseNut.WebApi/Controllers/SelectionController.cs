using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using naseNut.WebApi.Models.BindingModels;
using naseNut.WebApi.Models.Business.Services;
using naseNut.WebApi.Models.Entities;
using System.Data.Entity;

namespace naseNut.WebApi.Controllers
{
    [RoutePrefix("api/selection")]
    public class SelectionController : BaseApiController
    {
        private NaseNEntities _db = new NaseNEntities();
        [HttpPost]
        [Route("saveSelection")]
        public IHttpActionResult SaveSelection(SelectionBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                return BadRequest();
            }
            try
            {
                var selectionService = new SelectionService();
                var selection = new Selection
                {
                    Date = DateTime.Now,
                    First = model.First,
                    Second = model.Second,
                    Third = model.Third,
                    Broken = model.Broken,
                    Germinated = model.Germinated,
                    Vanas = model.Vanas,
                    WithNut = model.WithNut,
                    NutColor = model.NutColor,
                    NutPerformance = model.NutPerformance,
                    GerminationStart = model.GerminationStart,
                    SampleWeight = model.SampleWeight,
                    NutsNumber = model.NutsNumber,
                    Humidity = model.Humidity
                };
                var saved = selectionService.Save(selection);
                return saved ? (IHttpActionResult)Ok() : BadRequest();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar guardar la selección." + "\n" + "Detalles del Error: " + ex));
            }
        }

        [HttpGet]
        [Route("getAll")]
        public IHttpActionResult GetAllSelections()
        {
            try
            {
                var selections = _db.Selections.ToList();
                return selections.Count != 0 ? (IHttpActionResult)Ok(TheModelFactory.Create(selections)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener las selecciones." + "\n" + "Detalles del Error: " + ex));
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteSelection(int id)
        {
            try
            {
                var selectionService = new SelectionService();
                var selection = selectionService.GetById(id);
                if(selection == null) return NotFound();
                var deleted = selectionService.Delete(selection);
                return deleted ? (IHttpActionResult) Ok() : InternalServerError();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar eliminar la selección." + "\n" + "Detalles del Error: " + ex));
            }
        }
    }
}