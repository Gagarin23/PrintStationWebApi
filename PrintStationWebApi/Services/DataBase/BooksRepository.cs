using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PrintStationWebApi.Models.DataBase;

namespace PrintStationWebApi.Services.DataBase
{
    public interface IBookRepository
    {
        Task<DataBaseBook> GetBookAsync(long barcode);
        Task<DataBaseBook[]> GetBooksAsync(long[] barcodes);
        Task AddBooksAsync(DataBaseBook[] books);
        Task<DataBaseBook> ChangeBookStateAsync(DataBaseBook dataBaseBook);
        Task<DataBaseBook> DeleteBookAsync(long barcode);
    }

    public class BookRepository : IBookRepository
    {
        private readonly BooksContext _db;
        public BookRepository(BooksContext db) //По хорошему надо передавать лишь один dbset, но в базе и так одна таблица.
        {
            _db = db;
        }

        public async Task<DataBaseBook> GetBookAsync(long barcode)
        {
            return await _db.Books.FindAsync(barcode);
        }

        public async Task<DataBaseBook[]> GetBooksAsync(long[] barcodes)
        {
            return await _db.Books.Where(b => barcodes.Contains(b.Barcode)).ToArrayAsync();
        }

        public async Task AddBooksAsync(DataBaseBook[] books)
        {
            var existingBooks = await _db.Books.Where(dbBook => books.Select(book => book.Barcode).Contains(dbBook.Barcode)).ToArrayAsync();
            if(existingBooks.Any())
                _db.Books.RemoveRange(existingBooks); //База мизерная, поэтому не замарачивался.

            await _db.Books.AddRangeAsync(books);
            await _db.SaveChangesAsync();
        }

        public async Task<DataBaseBook> ChangeBookStateAsync(DataBaseBook dataBaseBook)
        {
            if (await _db.Books.FindAsync(dataBaseBook.Barcode) == null)
                return null;

            _db.Update(dataBaseBook);
            await _db.SaveChangesAsync();
            return dataBaseBook;
        }

        public async Task<DataBaseBook> DeleteBookAsync(long barcode)
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
