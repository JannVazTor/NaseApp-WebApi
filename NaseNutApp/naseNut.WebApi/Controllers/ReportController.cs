﻿using naseNut.WebApi.Models.Entities;
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
    }
}