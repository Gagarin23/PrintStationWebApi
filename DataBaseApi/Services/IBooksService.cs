using System.Collections.Generic;
using System.Threading.Tasks;
using DataBaseApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DataBaseApi.Services
{
    public interface IBooksService
    {
        Task<Book[]> GetBooks(long[] barcodes);
        Task AddBooks(Book[] books);
        Task<Book> ChangeBookState(Book book);
        Task<Book> DeleteBook(long barcode);
    }
}
