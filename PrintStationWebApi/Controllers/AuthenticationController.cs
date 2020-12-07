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
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using PrintStationWebApi.Authentication;
using PrintStationWebApi.Models.DataBase;
using PrintStationWebApi.Services.BL;
using PrintStationWebApi.Services.DataBase;

namespace PrintStationWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAccountService _authentication;
        private readonly IUsersRepository _usersRepository;

        public AuthenticationController(IAccountService authentication, IUsersRepository usersRepository)
        {
            _authentication = authentication;
            _usersRepository = usersRepository;
        }

        [HttpPost("/token")]
        public async Task<IActionResult> Token(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return BadRequest();

            return Ok(await _authentication.Token(username, password));
        }

        [Authorize(Roles = "2")]
        [HttpPost("/reg")]
        public async Task<IActionResult> Register(string username, string password, Role role)
        {
            
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || role == 0)
                return BadRequest();

            if (username.Equals(password))
                return BadRequest("Имя пользователя и пароль не могут совпадать");

            if(await _usersRepository.SetUserAsync(username, password, role) == 1)
                return StatusCode(201);

            return StatusCode(500);
        }
    }
}
