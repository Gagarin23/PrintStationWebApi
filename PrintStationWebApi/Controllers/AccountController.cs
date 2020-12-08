using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrintStationWebApi.Models.DataBase;
using PrintStationWebApi.Services.BL;
using PrintStationWebApi.Services.DataBase;
using System.Threading.Tasks;

namespace PrintStationWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authentication;
        private readonly IAccountService _accountService;

        public AccountController(IAuthService authentication, IAccountService accountService)
        {
            _authentication = authentication;
            _accountService = accountService;
        }

        [HttpGet("/token")]
        public async Task<ActionResult<string>> Token(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return BadRequest();

            return await _authentication.Token(username, password);
        }

        [Authorize(Roles = "admin")]
        [HttpPost("/reg")]
        public async Task<IActionResult> Register(string username, string password, string role)
        {

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(role))
                return BadRequest();

            if (username == password)
                return BadRequest("Имя пользователя и пароль не могут совпадать");

            return await _accountService.AddUserAsync(username, password, role);
        }

        [Authorize(Roles = "admin")]
        [HttpPatch("/changerole")]
        public async Task<IActionResult> SetRole(string username, string role)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(role))
                return BadRequest();

            return await _accountService.ChangeRoleAsync(username, role);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("/del")]
        public async Task<IActionResult> DeleteUser(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return BadRequest();

            return await _accountService.RemoveUserAsync(username);
        }
    }
}
