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
                    FieldName = model.FieldName
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
        [HttpPost]
        [Route("batch")]
        public IHttpActionResult SaveBatch(AddBatchBindingModel model)
        {
            if (!ModelState.IsValid)
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
                var saved = batchService.Save(batch);
                return saved ? (IHttpActionResult)Ok() : BadRequest();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar guardar el lote." + "\n" + "Detalles del Error: " + ex));
            }
        }
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

        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult GetAll()
        {
            try
            {
                var fieldService = new FieldService();
                var batches = _db.Batches.ToList();
                return batches != null ? (IHttpActionResult)Ok(TheModelFactory.Create(batches, false)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener los campos." + "\n" + "Detalles del Error: " + ex));
            }
        }

        [HttpGet]
        [Route("fields")]
        public IHttpActionResult GetFields()
        {
            try
            {
                var fieldService = new FieldService();
                var fields = _db.Fields.ToList();
                return fields != null ? (IHttpActionResult)Ok(TheModelFactory.Create(fields)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener los campos." + "\n" + "Detalles del Error: " + ex));
            }
        }

        [HttpGet]
        [Route("batches")]
        public IHttpActionResult GetBatches()
        {
            try
            {
                var fieldService = new FieldService();
                var batches = _db.Batches.ToList();
                return batches != null ? (IHttpActionResult)Ok(TheModelFactory.Create(batches, true)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener las Huertas/Lotes." + "\n" + "Detalles del Error: " + ex));
            }
        }
        [HttpGet]
        [Route("boxes")]
        public IHttpActionResult GetBoxes()
        {
            try
            {
                var fieldService = new FieldService();
                var boxes = _db.Boxes.ToList();
                return boxes != null ? (IHttpActionResult)Ok(TheModelFactory.Create(boxes)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener los cuadros." + "\n" + "Detalles del Error: " + ex));
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