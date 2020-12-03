using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataBaseApi.Models;
using DataBaseApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DataBaseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBooksService _booksService;

        public BooksController(IBooksService booksService)
        {
            _booksService = booksService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> FindBooks([FromQuery] params long[] barcodes)
        {
            if (barcodes == null)
                return BadRequest();

            if (barcodes.Length < 1)
                return BadRequest();

            var books = await _booksService.GetBooks(barcodes);
            return Ok(books);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<Book>>> AddBooks(params Book[] books)
        {
            if (books == null)
                return BadRequest();

            if (books.Length < 1)
                return BadRequest();

            await _booksService.AddBooks(books);
            return StatusCode(201);
        }

        [HttpPatch]
        public async Task<ActionResult<Book>> ChangeBookState(Book book)
        {
            if (book == null)
                return BadRequest();

            book = await _booksService.ChangeBookState(book);
            if(book != null)
                return Ok();
            return NotFound();
        }

        [HttpDelete]
        public async Task<ActionResult<long>> DeleteBook(long barcode)
        {
            if (barcode == 0)
                return BadRequest();

            var book = await _booksService.DeleteBook(barcode);
            if(book != null)
                return Ok();
            return NotFound();
        }
    }
}
