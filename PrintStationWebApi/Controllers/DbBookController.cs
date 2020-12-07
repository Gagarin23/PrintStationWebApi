using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PrintStationWebApi.Models;
using PrintStationWebApi.Models.DataBase;
using PrintStationWebApi.Services;
using PrintStationWebApi.Services.DataBase;

namespace PrintStationWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DbBookController : ControllerBase
    {
        private readonly IValidateService _validateService;
        private readonly IBookRepository _bookRepository;

        public DbBookController(IValidateService validateService, IBookRepository bookRepository)
        {
            _validateService = validateService;
            _bookRepository = bookRepository;
        }

        [HttpGet("/search")]
        public async Task<ActionResult<IEnumerable<InputBook>>> FindBooks([FromQuery] params string[] barcodes)
        {
            if (barcodes == null || barcodes.Length < 1)
                return BadRequest();

            var ids = _validateService.Parse(barcodes).ToArray();
            if (ids.Length < 1)
                return NotFound();

            var books = await _bookRepository.GetBooksAsync(ids);
            if (books.Count < 1)
                return NotFound();

            return Ok(books);
        }

        [HttpPost("/add")]
        public async Task<ActionResult<IEnumerable<InputBook>>> AddBooks(params InputBook[] books)
        {
            if (books == null || books.Length < 1)
                return BadRequest();

            var dbBooks = _validateService.Validate(books).ToArray();
            await _bookRepository.AddBooksAsync(dbBooks);
            return StatusCode(201);
        }

        [HttpPatch("/change")]
        public async Task<ActionResult<InputBook>> ChangeBookState(InputBook book)
        {
            if (book == null)
                return BadRequest();

            var dbBook = _validateService.Validate(book).FirstOrDefault();
            dbBook = await _bookRepository.ChangeBookStateAsync(dbBook);
            if(dbBook != null)
                return Ok();
            return NotFound();
        }

        [HttpDelete("/delete")]
        public async Task<ActionResult<long>> DeleteBook(string barcode)
        {
            if (barcode == null)
                return BadRequest();

            var id = _validateService.Parse(barcode);
            var book = await _bookRepository.DeleteBookAsync(id);
            if(book != null)
                return Ok();
            return NotFound();
        }
    }
}
