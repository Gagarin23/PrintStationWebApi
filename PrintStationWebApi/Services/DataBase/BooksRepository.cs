using Microsoft.EntityFrameworkCore;
using PrintStationWebApi.Models.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrintStationWebApi.Services.DataBase
{
    public interface IBookRepository
    {
        Task<DataBaseBook> GetBookAsync(long barcode);
        Task<List<DataBaseBook>> GetBooksAsync(IEnumerable<long> barcodes);
        Task AddBooksAsync(DataBaseBook[] books);
        Task<DataBaseBook> ChangeBookStateAsync(DataBaseBook dataBaseBook);
        Task<DataBaseBook> DeleteBookAsync(long barcode);
    }

    public class BookRepository : IBookRepository
    {
        private readonly PrintStationContext _db;

        public BookRepository(PrintStationContext db)
        {
            _db = db;
        }

        public async Task<DataBaseBook> GetBookAsync(long barcode)
        {
            if (barcode == 0)
                throw new ArgumentException(nameof(barcode));

            return await _db.Books.FindAsync(barcode);
        }

        public async Task<List<DataBaseBook>> GetBooksAsync(IEnumerable<long> barcodes)
        {
            if (barcodes == null || !barcodes.Any())
                throw new ArgumentNullException(nameof(barcodes));

            return await _db.Books.Where(b => barcodes.Contains(b.Barcode)).ToListAsync();
        }

        public async Task AddBooksAsync(DataBaseBook[] books)
        {
            if (books == null || books.Length < 1)
                throw new ArgumentNullException(nameof(books));

            var existingBooks = await _db.Books.Where(dbBook => books.Select(book => book.Barcode).Contains(dbBook.Barcode)).ToListAsync();
            if (existingBooks.Any())
                _db.Books.RemoveRange(existingBooks); //База мизерная, поэтому не замарачивался.

            await _db.Books.AddRangeAsync(books);
            await _db.SaveChangesAsync();
        }

        public async Task<DataBaseBook> ChangeBookStateAsync(DataBaseBook dataBaseBook)
        {
            if (dataBaseBook == null)
                throw new ArgumentNullException(nameof(dataBaseBook));

            if (await _db.Books.FindAsync(dataBaseBook.Barcode) == null)
                return null;

            _db.Update(dataBaseBook);
            await _db.SaveChangesAsync();
            return dataBaseBook;
        }

        public async Task<DataBaseBook> DeleteBookAsync(long barcode)
        {
            if (barcode == 0)
                throw new ArgumentException(nameof(barcode));

            var book = new DataBaseBook{Barcode = barcode};
            _db.Entry(book).State = EntityState.Deleted;
            await _db.SaveChangesAsync();
            return book;
        }
    }
}
