using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PrintStationWebApi.Logger;
using PrintStationWebApi.Models;
using PrintStationWebApi.Models.DataBase;

namespace PrintStationWebApi.Services
{
    public interface IValidateService
    {
        IEnumerable<DataBaseBook> Validate(params InputBook[] books);
        IEnumerable<long> Parse(params string[] barcodes);
    }

    public class ValidateService : IValidateService
    {
        public IEnumerable<DataBaseBook> Validate(params InputBook[] books)
        {
            var dbBook = new DataBaseBook();
            foreach (var inputBook in books)
            {
                dbBook.Barcode = inputBook.Barcode;

                if (Path.HasExtension(inputBook.BlockPath) 
                    && Path.GetExtension(inputBook.BlockPath).Equals(".pdf")
                    && inputBook.BlockCreationTime != DateTime.MinValue)
                {
                    dbBook.BlockPath = inputBook.BlockPath;
                    dbBook.BlockCreationTime = inputBook.BlockCreationTime;
                }

                if (Path.HasExtension(inputBook.CoverPath) 
                    && Path.GetExtension(inputBook.CoverPath).Equals(".pdf")
                    && inputBook.CoverCreationTime != DateTime.MinValue)
                {
                    dbBook.CoverPath = inputBook.CoverPath;
                    dbBook.CoverCreationTime = inputBook.CoverCreationTime;
                }

                yield return dbBook;
            }
        }

        public IEnumerable<long> Parse(params string[] barcodes)
        {
            foreach (var barcode in barcodes)
            {
                long.TryParse(barcode.Replace("-", string.Empty).Replace("_", string.Empty), out long id);
                if (id > 9785000000000 && id < 9785999999999)
                {
                    yield return id;
                }
            }
        }
    }
}
