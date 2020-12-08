using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrintStationWebApi.Models.DataBase;
using PrintStationWebApi.Services.DataBase;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrintStationWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DbBookController : ControllerBase
    {
        private readonly IBookValidateService _bookValidateService;
        private readonly IBookRepository _bookRepository;

        public DbBookController(IBookValidateService bookValidateService, IBookRepository bookRepository)
        {
            _bookValidateService = bookValidateService;
            _bookRepository = bookRepository;
        }

        [Authorize]
        [HttpGet("/search")]
        public async Task<ActionResult<IEnumerable<InputBook>>> FindBooks([FromQuery] params string[] barcodes)
        {
            if (barcodes == null || barcodes.Length < 1)
                return BadRequest();

            var ids = _bookValidateService.Parse(barcodes).ToArray();
            if (ids.Length < 1)
                return NotFound();

            var books = await _bookRepository.GetBooksAsync(ids);
            if (books.Count < 1)
                return NotFound();

            return Ok(books);
        }

        [Authorize]
        [HttpPost("/add")]
        public async Task<ActionResult<IEnumerable<InputBook>>> AddBooks(params InputBook[] books)
        {
            if (books == null || books.Length < 1)
                return BadRequest();

            var dbBooks = _bookValidateService.Validate(books).ToArray();
            await _bookRepository.AddBooksAsync(dbBooks);
            return StatusCode(201);
        }

        [Authorize(Roles = "admin")]
        [HttpPatch("/change")]
        public async Task<ActionResult<InputBook>> ChangeBookState(InputBook book)
        {
            if (book == null)
                return BadRequest();

            var dbBook = _bookValidateService.Validate(book).FirstOrDefault();
            dbBook = await _bookRepository.ChangeBookStateAsync(dbBook);
            if (dbBook != null)
                return Ok();
            return NotFound();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("/delete")]
        public async Task<ActionResult<long>> DeleteBook(string barcode)
        {
            if (barcode == null)
                return BadRequest();

            var id = _bookValidateService.Parse(barcode);
            var book = await _bookRepository.DeleteBookAsync(id);
            if (book != null)
                return Ok();
            return NotFound();
        }
    }
}
