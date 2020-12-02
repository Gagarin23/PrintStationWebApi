using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataBaseApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ServerCashe.Services
{
    public interface IBooksCasheService
    {
        Task<ActionResult<IEnumerable<Book>>> AddBooksToCashe(IEnumerable<Book> books);
        Task<ActionResult<IEnumerable<Book>>> GetBooksFromCashe(IEnumerable<string> barcodes);
    }
}
