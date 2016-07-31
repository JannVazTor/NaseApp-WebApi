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
    }
}