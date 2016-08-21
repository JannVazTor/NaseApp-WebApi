using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace naseNut.WebApi.Controllers
{
    [RoutePrefix("test")]
    public class TestController : BaseApiController
    {
        [AllowAnonymous]
        [HttpGet]
        public string Test() {
            return "Connection Established!";
        }
    }
}