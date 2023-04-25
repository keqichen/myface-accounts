using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
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
        private readonly IUsersRepo _usersRepo;
        public LoginController(IUsersRepo usersRepo)
        {
            _usersRepo = usersRepo;
        }

        // [HttpGet("")]
        // public IActionResult GetUserData()
        // {
        //     string authHeader = HttpContext.Request.Headers["Authorization"];
        //     if (authHeader != null && authHeader.StartsWith("Basic"))
        //     {
        //         return Ok();
        //     }
        //     return Ok();
        // }

        [HttpPost("")]
        public async Task<IActionResult> IsValidLogin([FromHeader] string authorization)
        {
            // (string Username, string Password) details;

            // try
            // {
            //     details = AuthHelper.ExtractFromAuthHeader(authorization);
            // }
            // catch (Exception)
            // {
            //     return Unauthorized(
            //         "Authorization header was not valid. Ensure you are using basic auth, and have correctly base64-encoded your username and password.");
            // }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await HttpContext.SignInAsync("default", new ClaimsPrincipal(
            new ClaimsIdentity(
                new Claim[]
                {
                        new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
                }
            )
        ));
            return Ok();
        }
        // else
        // {
        //     return Unauthorized("Invalid login details.");
        // }
    }
}







