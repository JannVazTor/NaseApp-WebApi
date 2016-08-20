using naseNut.WebApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using naseNut.WebApi.Models.BindingModels;
using naseNut.WebApi.Models.Business.Services;

namespace naseNut.WebApi.Controllers
{
    [Authorize(Roles = "admin")]
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
        [Route("currentInventoryGrills")]
        public IHttpActionResult GetCurrentInvetoryReport() {
            try
            {
                var grills = _db.Grills.Where(g => g.Status).ToList();
                return grills.Count != 0 ? (IHttpActionResult)Ok(TheModelFactory.Create(grills)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener el inventario actual de proceso." + "\n" + "Detalles del Error: " + ex));
            }
        }

        [HttpGet]
        [Route("secondCurrentInventoryGrills")]
        public IHttpActionResult GetSecondCurrentInventoryReport()
        {
            try
            {
                var grills = _db.Grills.Where(g => g.Status && g.Quality == 2).ToList();

                return grills.Count != 0 ? (IHttpActionResult)Ok(TheModelFactory.Create(grills)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener el inventario actual de proceso de segunda calidad." + "\n" + "Detalles del Error: " + ex));
            }
        }
        [HttpGet]
        [Route("processInventory")]
        public IHttpActionResult GetProcessInventory() {
            try
            {
                var grills = _db.Grills.ToList();
                return grills.Count != 0 ? (IHttpActionResult)Ok(TheModelFactory.Create(grills)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener el inventario de proceso." + "\n" + "Detalles del Error: " + ex));
            }
        }
        [HttpGet]
        [Route("grillIssues")]
        public IHttpActionResult GetGrillIssuesReport() {
            try
            {
                var grillsIssues = _db.GrillIssues.ToList();
                return grillsIssues.Count != 0 ? (IHttpActionResult)Ok(TheModelFactory.CreateReport(grillsIssues)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener las salidas de parrillas." + "\n" + "Detalles del Error: " + ex));
            }
        }

        [HttpGet]
        [Route("secondGrillIssues")]
        public IHttpActionResult GetSecondGrillIssuesReport()
        {
            try
            {
                var secondGrill = _db.GrillIssues.SelectMany(x => x.Grills).Where(y => y.Quality == 2).ToList();
                var grillsIssues = _db.GrillIssues.ToList();
                return grillsIssues.Count != 0 ? (IHttpActionResult)Ok(TheModelFactory.CreateReport(secondGrill, grillsIssues)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener las salidas de parrillas." + "\n" + "Detalles del Error: " + ex));
            }
        }

        [HttpGet]
        [Route("originReport")]
        public IHttpActionResult GetOriginReportt()
        {
            try
            {
                var batches = _db.Batches.ToList();
                var varieties = _db.Varieties.ToList();
                var remissions = _db.Remissions.ToList();
                var nutTypes = _db.NutTypes.ToList();
                return batches.Count != 0 ? (IHttpActionResult)Ok(TheModelFactory.CreateReport(batches, varieties, remissions, nutTypes)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener las salidas de parrillas." + "\n" + "Detalles del Error: " + ex));
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("dailyProcess")]
        public IHttpActionResult GetDailyProcessReport(ReportBindingModel date)
        {
            //var date2 = DateTime.Now.ToString("d");
            List<ReceptionEntry> datedReceptionEntries = new List<ReceptionEntry>();
            try
            {
                var receptionEntries = _db.ReceptionEntries.ToList();
                var datedReception = from rec in receptionEntries
                    where rec.DateIssue != null && rec.DateIssue.Value.ToString("d").Equals(date.ReportDate.ToString("d")) select rec;

                datedReceptionEntries.AddRange(datedReception);
                return receptionEntries.Count != 0 ? (IHttpActionResult)Ok(TheModelFactory.CreateReport(datedReceptionEntries, date.ReportDate.ToString(CultureInfo.InvariantCulture))) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener el reporte de proceso." + "\n" + "Detalles del Error: " + ex));
            }
        }
    }
}