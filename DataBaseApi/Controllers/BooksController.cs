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

            var books = _booksService.GetBooks(barcodes);
            return new ObjectResult(books.Result);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<Book>>> AddBooks(params Book[] books)
        {
            if (books == null)
                return BadRequest();

            if (books.Length < 1)
                return BadRequest();

            var isComplite = await _booksService.AddBooks(books);
            if (isComplite)
                return Ok();
            
            return StatusCode(500);
        }

        [HttpPatch]
        public async Task<ActionResult<Book>> ChangeBookState(Book book)
        {
            if (book == null)
                return BadRequest();

            var isComplite = await _booksService.ChangeBookState(book);
            if (isComplite)
                return Ok();
            
            return StatusCode(500);
        }

        [HttpDelete]
        public async Task<ActionResult<long>> DeleteBook(long barcode)
        {
            if (barcode == 0)
                return BadRequest();

            var isComplite = await _booksService.DeleteBook(barcode);
            if (isComplite)
                return Ok();
            
            return StatusCode(500);
        }
    }
}
