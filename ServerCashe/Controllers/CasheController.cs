using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataBaseApi.Models;
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
        public Task<ActionResult<IEnumerable<Book>>> Get(IEnumerable<string> barcodes) =>
            _booksCasheService.GetBooksFromCashe(barcodes);

        [HttpPost]
        public Task<ActionResult<IEnumerable<Book>>> Post(IEnumerable<Book> books) =>
            _booksCasheService.AddBooksToCashe(books);
    }
}
