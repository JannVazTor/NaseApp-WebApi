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
    [Authorize(Roles = "admin,qualityUser")]
    [RoutePrefix("api/processResult")]
    public class ProcessResultController : BaseApiController
    {
        [HttpPost]
        public IHttpActionResult SaveReceptionEntrySampling(AddNutTypeBindingModel model)
        {
            if (!ModelState.IsValid || model == null || model.NutTypes.Count == 0 || model.NutSizeProcessResult.Count == 0)
            {
                return BadRequest();
            }
            try
            {
                var nutTypeService = new NutTypeService();
                var nutTypes = model.NutTypes.Select(n => new NutType {
                    NutType1 = n.NutType,
                    Sacks = n.Sacks,
                    Kilos = n.Kilos,
                    ReceptionEntryId = model.ReceptionEntryId
                }).ToList();
                var nutSizeProcessResult = model.NutSizeProcessResult.Select(n => new NutSizeProcessResult {
                    NutSize = n.NutSize,
                    Sacks = n.Sacks
                }).ToList();
                var saved = nutTypeService.Save(nutTypes, model.ReceptionEntryId, nutSizeProcessResult);
                return saved ? (IHttpActionResult)Ok() : BadRequest();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar guardar el registro." + "\n" + "Detalles del Error: " + ex));
            }
        }
    }
}