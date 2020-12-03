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

        public async Task<bool> AddBooks(Book[] books)
        {
            var existingBooks = await _db.Books.Where(dbBook => books.Select(book => book.Barcode).Contains(dbBook.Barcode)).ToArrayAsync();
            if(existingBooks.Any())
                _db.Books.RemoveRange(existingBooks);

            _db.Books.AddRange(books);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ChangeBookState(Book book)
        {
            if (await _db.Books.FindAsync(book.Barcode) == null)
                return false;

            _db.Update(book);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteBook(long barcode)
        {
            var book = await _db.Books.FindAsync(barcode);
            if (book != null)
            {
                _db.Books.Remove(book);
                await _db.SaveChangesAsync();
            }
                
            else
                return false;

            return true;
        }
    }
}
