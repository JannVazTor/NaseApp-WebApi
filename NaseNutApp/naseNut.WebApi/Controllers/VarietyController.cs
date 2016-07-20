using naseNut.WebApi.Models.BindingModels;
using naseNut.WebApi.Models.Business.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using naseNut.WebApi.Models.Entities;

namespace naseNut.WebApi.Controllers
{
    [RoutePrefix("api/variety")]
    public class VarietyController : BaseApiController
    {
        private NaseNEntities _db = new NaseNEntities();
        [HttpPost]
        public IHttpActionResult SaveVariety(AddVarietyBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var varietyService = new VarietyService();
                var variety = new Variety
                {
                    Variety1 = model.VarietyName,
                    NutSize = new NutSize {
                        Small = model.Small,
                        MediumStart = model.MediumStart,
                        MediumEnd = model.MediumEnd,
                        LargeStart = model.LargeStart,
                        LargeEnd = model.LargeEnd
                    }
                };
                var saved = varietyService.Save(variety);
                return saved ? (IHttpActionResult)Ok() : BadRequest();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar guardar la variedad." + "\n" + "Detalles del Error: " + ex));
            }
        }

        [HttpGet]
        public IHttpActionResult GetAllVarieties()
        {
            try
            {
                var varieties = _db.Varieties.ToList();
                return varieties != null ? (IHttpActionResult)Ok(TheModelFactory.Create(varieties)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener las variedades." + "\n" + "Detalles del Error: " + ex));
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteVariety(int id)
        {
            try
            {
                var varietyService = new VarietyService();
                var variety = varietyService.GetById(id);
                if (variety == null) return NotFound();
                var deleted = varietyService.Delete(variety);
                return deleted ? (IHttpActionResult)Ok() : InternalServerError();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar eliminar la variedad." + "\n" + "Detalles del Error: " + ex));
            }
        }
    }
}