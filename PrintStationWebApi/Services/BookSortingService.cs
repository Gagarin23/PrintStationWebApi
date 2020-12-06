using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrintStationWebApi.Models.BL;

namespace PrintStationWebApi.Services
{
    public interface IBookSortingService<T> where T : Book
    {
        List<T[]> Sort(T[] books);
    }

    public class BookSortingService<T> : IBookSortingService<T> where T : Book
    {
        public List<T[]> Sort(T[] books)
        {
            var uniqBookParameters = books.Distinct().ToList();
            var queuesForPrinting = new List<T[]>();

            foreach (var uniqBookParameter in uniqBookParameters)
                queuesForPrinting.Add(books.Where(b => b.Equals(uniqBookParameter)).ToArray());

            return queuesForPrinting;
        }
    }
}
