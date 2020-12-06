using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PrintStationWebApi.Models;
using PrintStationWebApi.Models.DataBase;

namespace PrintStationWebApi.Services
{
    public interface IBooksRepository
    {
        Task<DataBaseBook[]> GetBooks(long[] barcodes);
        Task AddBooks(DataBaseBook[] books);
        Task<DataBaseBook> ChangeBookState(DataBaseBook dataBaseBook);
        Task<DataBaseBook> DeleteBook(long barcode);
    }

    public class BooksRepository : IBooksRepository
    {
        private readonly BooksContext _db;
        public BooksRepository(BooksContext db) //По хорошему надо передавать лишь один dbset, но в базе и так одна таблица.
        {
            _db = db;
        }

        public async Task<DataBaseBook[]> GetBooks(long[] barcodes)
        {
            return await _db.Books.Where(b => barcodes.Contains(b.Barcode)).ToArrayAsync();
        }

        public async Task AddBooks(DataBaseBook[] books)
        {
            var existingBooks = await _db.Books.Where(dbBook => books.Select(book => book.Barcode).Contains(dbBook.Barcode)).ToArrayAsync();
            if(existingBooks.Any())
                _db.Books.RemoveRange(existingBooks); //База мизерная, поэтому не замарачивался.

            await _db.Books.AddRangeAsync(books);
            await _db.SaveChangesAsync();
        }

        public async Task<DataBaseBook> ChangeBookState(DataBaseBook dataBaseBook)
        {
            if (await _db.Books.FindAsync(dataBaseBook.Barcode) == null)
                return null;

            _db.Update(dataBaseBook);
            await _db.SaveChangesAsync();
            return dataBaseBook;
        }

        public async Task<DataBaseBook> DeleteBook(long barcode)
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
