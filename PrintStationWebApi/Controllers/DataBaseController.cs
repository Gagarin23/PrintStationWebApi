using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PrintStationWebApi.Models;
using PrintStationWebApi.Models.DataBase;
using PrintStationWebApi.Services;

namespace PrintStationWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataBaseController : ControllerBase
    {
        private readonly IValidateService _validateService;
        private readonly IBooksRepository _booksRepository;

        public DataBaseController(IValidateService validateService, IBooksRepository booksRepository)
        {
            _validateService = validateService;
            _booksRepository = booksRepository;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InputBook>>> FindBooks([FromQuery] params string[] barcodes)
        {
            if (barcodes == null || barcodes.Length < 1)
                return BadRequest();

            var ids = _validateService.Parse(barcodes).ToArray();
            var books = await _booksRepository.GetBooks(ids);
            return Ok(books);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<InputBook>>> AddBooks(params InputBook[] books)
        {
            if (books == null || books.Length < 1)
                return BadRequest();

            var dbBooks = _validateService.Validate(books).ToArray();
            await _booksRepository.AddBooks(dbBooks);
            return StatusCode(201);
        }

        [HttpPatch]
        public async Task<ActionResult<InputBook>> ChangeBookState(InputBook book)
        {
            if (book == null)
                return BadRequest();

            var dbBook = _validateService.Validate(book).FirstOrDefault();
            dbBook = await _booksRepository.ChangeBookState(dbBook);
            if(dbBook != null)
                return Ok();
            return NotFound();
        }

        [HttpDelete]
        public async Task<ActionResult<long>> DeleteBook(string barcode)
        {
            if (barcode == null)
                return BadRequest();

            var id = _validateService.Parse(barcode).FirstOrDefault();
            var book = await _booksRepository.DeleteBook(id);
            if(book != null)
                return Ok();
            return NotFound();
        }
    }
}
