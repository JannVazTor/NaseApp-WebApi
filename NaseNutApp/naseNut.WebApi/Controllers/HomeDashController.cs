using naseNut.WebApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace naseNut.WebApi.Controllers
{
    [RoutePrefix("api/homeDash")]
    public class HomeDashController:BaseApiController
    {
        private NaseNEntities _db = new NaseNEntities();
        [HttpGet]
        [Route("productionVariety")]
        public IHttpActionResult ProductionByVariety() {
            try
            {
                var varieties = _db.Varieties.ToList();
                return varieties.Count != 0 ? (IHttpActionResult)Ok(TheModelFactory.CreateDash(varieties)) : Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        [Route("accumulatedNutProducer")]
        public IHttpActionResult AccumulatedNutProducer()
        {
            try
            {
                var producers = _db.Producers.ToList();
                return producers.Count != 0 ? (IHttpActionResult)Ok(TheModelFactory.CreateDash(producers)) : Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        [Route("grillIssuesAndInventory")]
        public IHttpActionResult GrillIssuesAndInventory()
        {
            try
            {
                var grills = _db.Grills.ToList();
                return grills.Count != 0 ? (IHttpActionResult)Ok(TheModelFactory.CreateDash(grills)) : Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        [Route("cylinderOccupiedHours")]
        public IHttpActionResult CylinderOccupiedHours()
        {
            try
            {
                var cylinders = _db.Cylinders.Where(c=> !c.Active && c.ReceptionEntries.Any(r => r.IssueDate == null)).ToList();
                return cylinders.Count != 0 ? (IHttpActionResult)Ok(TheModelFactory.CreateDash(cylinders)) : Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        [Route("averageNumberOfNuts")]
        public IHttpActionResult AverageNumberOfNuts()
        {
            try
            {
                var varieties = _db.Varieties.ToList();
                return varieties.Count != 0 ? (IHttpActionResult)Ok(TheModelFactory.CreateDashBarWithNumber(varieties)) : Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        [Route("percentageOfFirstAndSecond")]
        public IHttpActionResult PercentageOfFirstAndSecond()
        {
            try
            {
                var varieties = _db.Varieties.ToList();
                return varieties.Count != 0 ? (IHttpActionResult)Ok(TheModelFactory.CreateDashBarWithNumberPercentage(varieties)) : Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}