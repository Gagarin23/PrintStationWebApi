using Microsoft.Extensions.Caching.Memory;
using PrintStationWebApi.Models.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrintStationWebApi.Services.Cache
{
    public interface ICacheService
    {
        Task AddRangeAsync(IEnumerable<DataBaseBook> books);
        void AddRange(IEnumerable<DataBaseBook> books);
        void Add(DataBaseBook books);
        DataBaseBook GetBook(long barcode);
        public IEnumerable<DataBaseBook> GetBooks(IEnumerable<long> barcodes);
    }

    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task AddRangeAsync(IEnumerable<DataBaseBook> books)
        {
            if (books == null || !books.Any())
                throw new ArgumentNullException(nameof(books));

            await Task.Run(() => AddRange(books));
        }

        public void AddRange(IEnumerable<DataBaseBook> books)
        {
            if (books == null || !books.Any())
                throw new ArgumentNullException(nameof(books));

            foreach (var book in books)
            {
                _cache.Set(book.Barcode, book, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7)
                });
            }
        }

        public void Add(DataBaseBook book)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book));

            _cache.Set(book.Barcode, book, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7)
            });
        }

        public DataBaseBook GetBook(long barcode)
        {
            if (barcode == 0)
                throw new ArgumentException(nameof(barcode));

            return _cache.Get<DataBaseBook>(barcode);
        }

        public IEnumerable<DataBaseBook> GetBooks(IEnumerable<long> barcodes)
        {
            if (barcodes == null || !barcodes.Any())
                throw new ArgumentNullException(nameof(barcodes));

            foreach (var barcode in barcodes)
            {
                if (_cache.TryGetValue(barcode, out DataBaseBook dbBook))
                    yield return dbBook;
            }
        }
    }
}
