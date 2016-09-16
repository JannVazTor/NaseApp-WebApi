using naseNut.WebApi.Models.BindingModels;
using naseNut.WebApi.Models.Business.Services;
using naseNut.WebApi.Models.Entities;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace naseNut.WebApi.Controllers
{
    [Authorize(Roles = "admin")]
    [RoutePrefix("api/harvestSeason")]
    public class HarvestSeasonController : BaseApiController
    {
        [HttpPost]
        public IHttpActionResult Save(SaveHarvestSeasonBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var harvestSeasonService = new HarvestSeasonService();
                var userService = new UserService();
                if (harvestSeasonService.GetByName(model.Name) != null) return Conflict();
                var harvestSeason = new HarvestSeason
                {
                    Name = model.Name,
                    Active = true,
                    EntryDate = DateTime.Now,
                    UserId = userService.GetByName(model.UserName).Id,
                    Description = model.Description,
                    IssueDate = model.IssueDate
                };
                var saved = harvestSeasonService.Save(harvestSeason);
                return saved ? (IHttpActionResult)Ok() : Conflict();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar guardar el registro." + "\n" + "Detalles del Error: " + ex));
            }
        }
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            try
            {
                var harvestSeasonService = new HarvestSeasonService();
                var harvestSeasons = harvestSeasonService.GetAll();
                return harvestSeasons.Count != 0 ? (IHttpActionResult)Ok(TheModelFactory.Create(harvestSeasons)) : Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener el registro." + "\n" + "Detalles del Error: " + ex));
            }
        }
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var harvestSeasonService = new HarvestSeasonService();
                var harvestSeason = harvestSeasonService.GetById(id);
                if (harvestSeason == null) return NotFound();
                var deleted = harvestSeasonService.Delete(harvestSeason);
                return deleted ? (IHttpActionResult)Ok() : Conflict();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar eliminar el registro." + "\n" + "Detalles del Error: " + ex));
            }
        }
        [HttpPut]
        [Route("changeState/{id}/{state}")]
        public IHttpActionResult ChangeCylinderState(int id, int state)
        {
            try
            {
                var harvestSeasonService = new HarvestSeasonService();
                if (harvestSeasonService.GetById(id) == null) return NotFound();
                var modified = harvestSeasonService.ChangeState(id, state == 1);
                return modified ? (IHttpActionResult)Ok() : Conflict();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar obtener el registro." + "\n" + "Detalles del Error: " + ex));
            }
        }
        [Authorize(Roles = "admin")]
        [HttpPut]
        public IHttpActionResult Update(UpdateHarvestSeasonBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var harvestSeasonService = new HarvestSeasonService();
                if (harvestSeasonService.GetById(model.Id) == null) return NotFound();
                var modified = harvestSeasonService.Update(model);
                return modified ? (IHttpActionResult)Ok() : Conflict();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "Ocurrio un error al intentar actualizar la parrilla." + "\n" + "Detalles del Error: " + ex));
            }
        }
    }
}