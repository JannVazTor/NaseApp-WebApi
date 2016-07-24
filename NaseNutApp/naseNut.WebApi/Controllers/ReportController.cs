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
    [RoutePrefix("api/report")]
    public class ReportController : BaseApiController
    {
        private NaseNEntities _db = new NaseNEntities();

        [HttpGet]
        [Route("producer/{id}")]
        public IHttpActionResult GetReportProducer(int id)
        {
            try
            {
                var producers = _db.ReceptionEntries.Where(r => r.ProducerId == id).ToList();
                return producers.Count != 0 ? (IHttpActionResult)Ok(TheModelFactory.CreateReport(producers)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener el reporte de productores." + "\n" + "Detalles del Error: " + ex));
            }
        }
        [HttpGet]
        [Route("reportingProcess")]
        public IHttpActionResult GetReportingProcess() {
            try
            {
                var varieties = _db.Varieties.ToList();
                return varieties.Count != 0 ? (IHttpActionResult)Ok(TheModelFactory.CreateReport(varieties)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener el reporte de proceso." + "\n" + "Detalles del Error: " + ex));
            }
        }

        [HttpGet]
        [Route("dailyProcessReport")]
        public IHttpActionResult GetDailyProcessReport(DateTime date)
        {
            try
            {
                var grills = _db.Grills.ToList();
                var samplings = _db.Samplings.ToList();
                var receptions = _db.Receptions.ToList();
                return grills.Count != 0 ? (IHttpActionResult) Ok(TheModelFactory.CreateReport(grills, receptions, samplings, date)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener el reporte diario de proceso." + "\n" + "Detalles del Error: " + ex));
            }
        }
    }
}