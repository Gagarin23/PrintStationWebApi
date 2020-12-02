using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataBaseApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace ServerCashe.Services
{
    public class BooksCasheService : ControllerBase, IBooksCasheService
    {
        private readonly IMemoryCache _cache;

        public BooksCasheService(IMemoryCache cache)
        {
            _cache = cache;
        }
        public async Task<ActionResult<IEnumerable<Book>>> AddBooksToCashe(IEnumerable<Book> books)
        {
            if (books == null)
                return BadRequest();

            var bookArr = books as Book[] ?? books.ToArray();
            if (!bookArr.Any())
                return BadRequest();

            foreach (var book in bookArr)
            {
                _cache.Set(book.Barcode, book, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7)
                });
            }

            return new ObjectResult(books);
        }

        public async Task<ActionResult<IEnumerable<Book>>> GetBooksFromCashe(IEnumerable<string> barcodes)
        {
            if (barcodes == null)
                return BadRequest();

            var barcodeArr = barcodes as string[] ?? barcodes.ToArray();
            if (!barcodeArr.Any())
                return BadRequest();

            var result = GetBooks(barcodeArr);
            if (result == null)
                return NotFound();

            return Ok();
        }

        private IEnumerable<Book> GetBooks(params string[] barcodes)
        {
            foreach (var barcode in barcodes)
            {
                _cache.TryGetValue(barcode, out Book book);
                if(book != null)
                    yield return book;
            }
        }
    }
}
