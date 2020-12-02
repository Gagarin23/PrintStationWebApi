using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataBaseApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DataBaseApi.Services
{
    public class BooksService : ControllerBase, IBooksService
    {
        private readonly BooksContext _db;
        public static List<string> Statistic = new List<string>();

        public BooksService(BooksContext db)
        {
            _db = db;
        }

        public async Task<ActionResult<IEnumerable<Book>>> GetBooks(IEnumerable<string> barcodes)
        {
            if (barcodes == null)
                return BadRequest();

            var barcodeArr = barcodes as string[] ?? barcodes.ToArray();
            if (!barcodeArr.Any())
                return BadRequest();

            var books = await _db.Books.Where(b => barcodeArr.Contains(b.Barcode)).ToListAsync();

            if (!books.Any())
                return NotFound();

            return books;
        }

        public async Task<ActionResult<IEnumerable<Book>>> AddBooks(IEnumerable<Book> books)
        {
            if (books == null)
                return BadRequest();

            var bookArr = books as Book[] ?? books.ToArray();
            if (!bookArr.Any())
                return BadRequest();

            var existingBooks = await _db.Books.Where(dbBook => bookArr.Select(book => book.Barcode).Contains(dbBook.Barcode)).ToArrayAsync();
            if(existingBooks.Any())
                _db.Books.RemoveRange(existingBooks);

            await _db.Books.AddRangeAsync(bookArr);
            await _db.SaveChangesAsync();

            return new ObjectResult(bookArr);
        }

        public async Task<ActionResult<Book>> ChangeBookState(Book book)
        {
            if (book == null)
                return BadRequest();

            if (!_db.Books.Any(b => b.Barcode.Equals(book.Barcode)))
                return NotFound();

            _db.Update(book);
            await _db.SaveChangesAsync();

            return Ok(book);
        }

        public async Task<ActionResult<string>> DeleteBook(string barcode)
        {
            if (barcode == null)
                return BadRequest();

            var book = await _db.Books.FindAsync(barcode);
            if (book != null)
            {
                _db.Books.Remove(book);
                await _db.SaveChangesAsync();
            }
                
            else
                return NotFound();

            return Ok(barcode);
        }
    }
}
