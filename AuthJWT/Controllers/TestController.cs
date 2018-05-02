using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthJWT.Controllers
{
    [Produces("application/json")]
    [Route("api/test")]
    public class TestController : Controller
    {
        [HttpGet("test1")]
        public bool Test1()
        {
            return true;
        }

        [Authorize]
        [HttpGet("test2")]
        public bool Test2()
        {
            return true;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("test3")]
        public bool Test3()
        {
            return true;
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("test4")]
        public bool Test4()
        {
            return true;
        }
    }
}