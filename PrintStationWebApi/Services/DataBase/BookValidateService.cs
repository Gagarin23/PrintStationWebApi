using PrintStationWebApi.Models.DataBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PrintStationWebApi.Services.DataBase
{
    public interface IBookValidateService
    {
        IEnumerable<DataBaseBook> Validate(params InputBook[] inputBooks);
        long Parse(string barcode);
        IEnumerable<long> Parse(IEnumerable<string> barcodes);
    }

    public class BookValidateService : IBookValidateService
    {
        public IEnumerable<DataBaseBook> Validate(params InputBook[] inputBooks)
        {
            if (inputBooks == null || inputBooks.Length < 1)
                throw new ArgumentNullException(nameof(inputBooks));

            foreach (var inputBook in inputBooks)
            {
                var dbBook = new DataBaseBook();
                var barcode = Parse(inputBook.Barcode);
                if (barcode != 0)
                    dbBook.Barcode = barcode;

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

        public long Parse(string barcode)
        {
            if (string.IsNullOrEmpty(barcode))
                throw new ArgumentNullException(nameof(barcode));

            long.TryParse(barcode.Replace("-", string.Empty).Replace("_", string.Empty), out long id);
            if (id > 9785000000000 && id < 9785999999999)
            {
                return id;
            }

            return 0;
        }

        public IEnumerable<long> Parse(IEnumerable<string> barcodes)
        {
            if (barcodes == null || !barcodes.Any())
                throw new ArgumentNullException(nameof(barcodes));

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
