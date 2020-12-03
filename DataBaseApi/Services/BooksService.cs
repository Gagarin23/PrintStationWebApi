using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataBaseApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DataBaseApi.Services
{
    public class BooksService : IBooksService
    {
        private readonly BooksContext _db;
        public BooksService(BooksContext db)
        {
            _db = db;
        }

        public async Task<Book[]> GetBooks(long[] barcodes)
        {
            return await _db.Books.Where(b => barcodes.Contains(b.Barcode)).ToArrayAsync();
        }

        public async Task AddBooks(Book[] books)
        {
            var existingBooks = _db.Books.Where(dbBook => books.Select(book => book.Barcode).Contains(dbBook.Barcode));
            if(await existingBooks.AnyAsync())
                _db.Books.RemoveRange(existingBooks);

            await _db.Books.AddRangeAsync(books);
            await _db.SaveChangesAsync();
        }

        public async Task<Book> ChangeBookState(Book book)
        {
            if (await _db.Books.FindAsync(book.Barcode) == null)
                return null;

            _db.Update(book);
            await _db.SaveChangesAsync();
            return book;
        }

        public async Task<Book> DeleteBook(long barcode)
        {
            var book = await _db.Books.FindAsync(barcode);
            if (book != null)
            {
                _db.Books.Remove(book);
                await _db.SaveChangesAsync();
                return book;
            }

            return null;
        }
    }
}
