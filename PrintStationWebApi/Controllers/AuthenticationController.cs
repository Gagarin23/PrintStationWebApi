using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PrintStationWebApi.Authentication;
using PrintStationWebApi.Models.DataBase;
using PrintStationWebApi.Services.DataBase;
using IAuthenticationService = PrintStationWebApi.Services.BL.IAuthenticationService;

namespace PrintStationWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authentication;
        private readonly IUsersRepository _usersRepository;

        public AuthenticationController(IAuthenticationService authentication, IUsersRepository usersRepository)
        {
            _authentication = authentication;
            _usersRepository = usersRepository;
        }

        [HttpPost("/token")]
        public IActionResult Token(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return BadRequest();

            var token = _authentication.Token(username, password);
            return Json(token);
        }

        [HttpPost("/reg")]
        public IActionResult Register(string username, string password, Role role)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || role == 0)
                return BadRequest();

            _usersRepository.SetUser(username, password, role);

            return StatusCode(201);
        }
    }
}
