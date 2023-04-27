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
using MyFace.Models.Database;
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _usersRepo.HasAccess(authorization);
                //Console.Write(authorization);
            }
            catch (Exception)
            {
                return Unauthorized("Sorry, invalid username or password.");
            }
            
            string encodedUsernamePassword = authorization.Substring("Basic ".Length).Trim();
            var username = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword)).Split(':')[0];

                await HttpContext.SignInAsync("default", new ClaimsPrincipal(
                new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
                    }
                )
            ));
                Response.Cookies.Append("username", username, new CookieOptions { Expires = DateTime.Now.AddDays(30) } );
                return Ok();
            }
        }
    }









