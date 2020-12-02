using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ServerCashe.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ServerCashe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CasheController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IBooksCasheService _booksCasheService;

        public CasheController(ILogger logger, IBooksCasheService booksCasheService)
        {
            _logger = logger;
            _booksCasheService = booksCasheService;
        }

        [HttpGet]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {
        }
    }
}
