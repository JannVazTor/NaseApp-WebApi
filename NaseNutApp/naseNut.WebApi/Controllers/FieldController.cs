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
    [RoutePrefix("api/field")]
    public class FieldController:BaseApiController
    {
        private NaseNEntities _db = new NaseNEntities();
        [Authorize(Roles = "admin")]
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
                if (fieldService.GetByFieldName(model.FieldName) != null) return Conflict(); 
                var field = new Field
                {
                    FieldName = model.FieldName,
                    HarvestSeasonId = _db.HarvestSeasons.FirstOrDefault(h => h.Active).Id
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
        [Authorize(Roles = "admin")]
        [HttpPost]
        [Route("batch")]
        public IHttpActionResult SaveBatch(AddBatchBindingModel model)
        {
            if (!ModelState.IsValid || !model.NutInBatch.Any() || model == null)
            {
                return BadRequest();
            }
            try
            {
                var batchService = new BatchService();
                var batch = new Batch
                {
                    Batch1 = model.Batch,
                    Hectares = model.Hectares,
                    FieldId = model.FieldId
                };
                var saved = batchService.Save(batch, model.NutInBatch);
                return saved ? (IHttpActionResult)Ok() : BadRequest();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar guardar el lote." + "\n" + "Detalles del Error: " + ex));
            }
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        [Route("box")]
        public IHttpActionResult SaveBox(AddBoxBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var boxService = new BoxService();
                var box = new Box
                {
                    Id = model.BatchId,
                    Box1 = model.Box
                };
                var saved = boxService.Save(box);
                return saved ? (IHttpActionResult)Ok() : BadRequest();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar guardar el lote." + "\n" + "Detalles del Error: " + ex));
            }
        }
        [Authorize(Roles = "admin,remRecepUser")]
        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult GetAll()
        {
            try
            {
                var fieldService = new FieldService();
                var batches = _db.Batches.Where(b => b.Field.HarvestSeason.Active).ToList();
                return batches != null ? (IHttpActionResult)Ok(TheModelFactory.Create(batches, false)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener los campos." + "\n" + "Detalles del Error: " + ex));
            }
        }
        [Authorize(Roles = "admin,grillUser,remRecepUser")]
        [HttpGet]
        [Route("fields")]
        public IHttpActionResult GetFields()
        {
            try
            {
                var fieldService = new FieldService();
                var fields = _db.Fields.Where(f => f.HarvestSeason.Active).ToList();
                return fields != null ? (IHttpActionResult)Ok(TheModelFactory.Create(fields)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener los campos." + "\n" + "Detalles del Error: " + ex));
            }
        }
        [Authorize(Roles = "admin,grillUser")]
        [HttpGet]
        [Route("batches")]
        public IHttpActionResult GetBatches()
        {
            try
            {
                var fieldService = new FieldService();
                var batches = _db.Batches.Where(b => b.Field.HarvestSeason.Active).ToList();
                return batches != null ? (IHttpActionResult)Ok(TheModelFactory.Create(batches, true)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener las Huertas/Lotes." + "\n" + "Detalles del Error: " + ex));
            }
        }
        [Authorize(Roles = "admin,remRecepUser,grillUser")]
        [HttpGet]
        [Route("batchesInField/{fieldId}")]
        public IHttpActionResult GetBatchesInField(int fieldId)
        {
            try
            {
                var fieldService = new FieldService();
                if (fieldService.GetById(fieldId) == null) return NotFound();
                var batches = _db.Batches.Where(b => b.Field.Id == fieldId).ToList();
                return batches != null ? (IHttpActionResult)Ok(TheModelFactory.Create(batches, true)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener las Huertas/Lotes." + "\n" + "Detalles del Error: " + ex));
            }
        }
        [Authorize(Roles = "admin")]
        [HttpGet]
        [Route("boxes")]
        public IHttpActionResult GetBoxes()
        {
            try
            {
                var fieldService = new FieldService();
                var boxes = _db.Boxes.Where(b => b.Batch.Field.HarvestSeason.Active).ToList();
                return boxes != null ? (IHttpActionResult)Ok(TheModelFactory.Create(boxes)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener los cuadros." + "\n" + "Detalles del Error: " + ex));
            }
        }
        [Authorize(Roles = "admin,remRecepUser")]
        [HttpGet]
        [Route("boxesInBatch/{batchId}")]
        public IHttpActionResult GetBoxesInBatch(int batchId)
        {
            try
            {
                var batchService = new BatchService();
                if (batchService.GetById(batchId) == null) return NotFound();
                var boxes = _db.Boxes.Where(b => b.Batch.Id == batchId).ToList();
                return boxes != null ? (IHttpActionResult)Ok(TheModelFactory.Create(boxes)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener los cuadros." + "\n" + "Detalles del Error: " + ex));
            }
        }
        [Authorize(Roles = "admin")]
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
                return deleted ? (IHttpActionResult)Ok() : Conflict();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar eliminar al campo." + "\n" + "Detalles del Error: " + ex));
            }
        }
    }
}