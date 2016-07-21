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
                var grillIssues = _db.GrillIssues.ToList();
                return grillIssues.Count != 0 ? (IHttpActionResult)Ok(TheModelFactory.Create(grillIssues)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener las salidas de parrillas." + "\n" + "Detalles del Error: " + ex));
            }
        } 
    }
}