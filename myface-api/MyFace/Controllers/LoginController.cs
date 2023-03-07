using System;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFace.Models.Request;
using MyFace.Models.Response;
using MyFace.Repositories;

namespace MyFace.Controllers
{
    //we are creating a path/endpoint here;
    [Route("/login")]
    public class LoginController : ControllerBase
    {
        [HttpGet("")]
        public IActionResult GetUserData()
        {
            string authHeader = HttpContext.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("Basic"))
            {
                return Ok();
            }
            return Ok();
        }
    }
}






