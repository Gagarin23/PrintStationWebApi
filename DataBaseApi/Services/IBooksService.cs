using System.Collections.Generic;
using System.Threading.Tasks;
using DataBaseApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DataBaseApi.Services
{
    public interface IBooksService
    {
        Task<Book[]> GetBooks(long[] barcodes);
        Task<bool> AddBooks(Book[] books);
        Task<bool> ChangeBookState(Book book);
        Task<bool> DeleteBook(long barcode);
    }
}
