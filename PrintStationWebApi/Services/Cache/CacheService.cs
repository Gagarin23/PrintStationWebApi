using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using PrintStationWebApi.Models.DataBase;

namespace PrintStationWebApi.Services.Cache
{
    public interface ICacheService
    {
        Task AddRangeAsync(DataBaseBook[] books);
        void AddRange(DataBaseBook[] books);
        void Add(DataBaseBook books);
        DataBaseBook GetBook(long barcode);
        public IEnumerable<DataBaseBook> GetBooks(long[] barcodes);
    }

    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task AddRangeAsync(DataBaseBook[] books)
        {
            await Task.Run(() => AddRange(books));
        }

        public void AddRange(DataBaseBook[] books)
        {
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
            _cache.Set(book.Barcode, book, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7)
            });
        }

        public DataBaseBook GetBook(long barcode)
        {
            return _cache.Get<DataBaseBook>(barcode);
        }

        public IEnumerable<DataBaseBook> GetBooks(long[] barcodes)
        {
            foreach (var barcode in barcodes)
            {
                yield return _cache.Get<DataBaseBook>(barcode);
            }
        }
    }
}
