﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using naseNut.WebApi.Models.BindingModels;
using naseNut.WebApi.Models.Business.Services;
using naseNut.WebApi.Models.Entities;

namespace naseNut.WebApi.Controllers
{
    [RoutePrefix("api/producer")]
    public class ProducerController : BaseApiController
    {
        NaseNEntities _db = new NaseNEntities();
        [Authorize(Roles = "admin")]
        [HttpPost]
        public IHttpActionResult SaveProducer(AddProducerBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var producerService = new ProducerService();
                if (producerService.GetByProducerName(model.ProducerName) != null) return Conflict();
                var producer = new Producer
                {
                    ProducerName = model.ProducerName,
                    HarvestSeasonId = _db.HarvestSeasons.FirstOrDefault(h => h.Active).Id
                };
                var saved = producerService.Save(producer);
                return saved ? (IHttpActionResult) Ok() : BadRequest();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar guardar al Productor." + "\n" + "Detalles del Error: " + ex));
            }
        }
        [Authorize(Roles = "admin,grillUser,remRecepUser")]
        [HttpGet]
        [Route("getAll")]
        public IHttpActionResult GetAllProducers()
        {
            try
            {
                var producerService = new ProducerService();
                var producers = _db.Producers.Where(p => p.HarvestSeason.Active).ToList();
                return producers != null ? (IHttpActionResult) Ok(TheModelFactory.Create(producers)) : Ok(); 
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener los productores." + "\n" + "Detalles del Error: " + ex));
            }
        }
        [Authorize(Roles = "admin")]
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteProducer(int id)
        {
            try
            {
                var producerService = new ProducerService();
                var producer = producerService.GetById(id);
                if (producer == null) return NotFound();
                var deleted = producerService.Delete(producer);
                return deleted ? (IHttpActionResult) Ok() : InternalServerError();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar eliminar al productor." + "\n" + "Detalles del Error: " + ex));
            }
        }
    }
}
