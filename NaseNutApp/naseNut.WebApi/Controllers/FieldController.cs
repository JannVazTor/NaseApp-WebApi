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
    [Authorize(Roles ="admin")]
    [RoutePrefix("api/field")]
    public class FieldController:BaseApiController
    {
        private NaseNEntities _db = new NaseNEntities();
        [HttpPost]
        public IHttpActionResult SaveField(AddFieldBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var fieldService = new FieldService();
                var field = new Field
                {
                    FieldName = model.FieldName,
                    Hectares = model.Hectares
                };
                var saved = fieldService.Save(field);
                return saved ? (IHttpActionResult)Ok() : BadRequest();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar guardar el campo." + "\n" + "Detalles del Error: " + ex));
            }
        }

        [HttpGet]
        public IHttpActionResult GetAllFields()
        {
            try
            {
                var fieldService = new FieldService();
                var fields = fieldService.GetAll();
                return fields != null ? (IHttpActionResult)Ok(TheModelFactory.Create(fields)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener los campos." + "\n" + "Detalles del Error: " + ex));
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteField(int id)
        {
            try
            {
                var fieldService = new FieldService();
                var field = fieldService.GetById(id);
                if (field == null) return NotFound();
                var deleted = fieldService.Delete(field);
                return deleted ? (IHttpActionResult)Ok() : InternalServerError();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar eliminar al campo." + "\n" + "Detalles del Error: " + ex));
            }
        }
    }
}