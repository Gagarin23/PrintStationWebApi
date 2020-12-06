using System.Collections.Generic;
using System.Linq;
using PrintStationWebApi.Models.BL;

namespace PrintStationWebApi.Services.BL
{
    public interface IBookSortingService
    {
        List<T[]> Sort<T>(T[] books) where T : Book;
    }

    public class BookSortingService : IBookSortingService
    {
        public List<T[]> Sort<T>(T[] books) where T : Book
        {
            var uniqBookParameters = books.Distinct().ToList();
            var queuesForPrinting = new List<T[]>();

            foreach (var uniqBookParameter in uniqBookParameters)
                queuesForPrinting.Add(books.Where(b => b.Equals(uniqBookParameter)).ToArray());

            return queuesForPrinting;
        }
    }
}
