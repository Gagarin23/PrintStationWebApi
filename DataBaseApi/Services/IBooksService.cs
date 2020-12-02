using System.Collections.Generic;
using System.Threading.Tasks;
using DataBaseApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DataBaseApi.Services
{
    public interface IBooksService
    {
        Task<ActionResult<IEnumerable<Book>>> GetBooks(IEnumerable<string> barcodes);
        Task<ActionResult<IEnumerable<Book>>> AddBooks(IEnumerable<Book> books);
        Task<ActionResult<Book>> ChangeBookState(Book book);
        Task<ActionResult<Book>> DeleteBook(string barcode);
    }
}
