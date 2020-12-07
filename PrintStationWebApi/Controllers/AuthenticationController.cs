using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using PrintStationWebApi.Authentication;
using PrintStationWebApi.Models.DataBase;
using PrintStationWebApi.Services.BL;

namespace PrintStationWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authentication;

        public AuthenticationController(IAuthenticationService authentication)
        {
            _authentication = authentication;
        }

        [HttpPost("/token")]
        public IActionResult Token(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return BadRequest();

            var token = _authentication.Token(username, password);
            return Json(token);
        }
    }
}
