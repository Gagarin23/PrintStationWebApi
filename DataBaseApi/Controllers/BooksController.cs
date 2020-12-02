using System.Collections.Generic;
using System.Threading.Tasks;
using DataBaseApi.Models;
using DataBaseApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DataBaseApi.Controllers
{
    public class BooksController
    {
        private readonly ILogger _logger;
        private readonly IBooksService _booksService;

        public BooksController(ILogger logger, IBooksService booksService)
        {
            _logger = logger;
            _booksService = booksService;
        }

        [HttpGet]
        public Task<ActionResult<IEnumerable<Book>>> FindBooks(IEnumerable<string> barcodes) =>
            _booksService.GetBooks(barcodes);

        [HttpPost]
        public Task<ActionResult<IEnumerable<Book>>> AddBooks(IEnumerable<Book> books) =>
            _booksService.AddBooks(books);

        [HttpPatch]
        public Task<ActionResult<Book>> ChangeBookState(Book book) =>
            _booksService.ChangeBookState(book);

        [HttpDelete]
        public Task<ActionResult<string>> DeleteBook(string barcode) =>
            _booksService.DeleteBook(barcode);
    }
}
