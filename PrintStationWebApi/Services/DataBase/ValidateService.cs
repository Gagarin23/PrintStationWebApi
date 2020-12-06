using System;
using System.Collections.Generic;
using System.IO;
using PrintStationWebApi.Models.BL;
using PrintStationWebApi.Models.DataBase;

namespace PrintStationWebApi.Services.DataBase
{
    public interface IValidateService
    {
        IEnumerable<DataBaseBook> Validate(params InputBook[] books);
        long Parse(string barcode);
        IEnumerable<long> Parse(params string[] barcodes);
    }

    public class ValidateService : IValidateService
    {
        public IEnumerable<DataBaseBook> Validate(params InputBook[] InputBooks)
        {
            var dbBook = new DataBaseBook();
            foreach (var inputBook in InputBooks)
            {
                var barcode = Parse(inputBook.Barcode);
                if (barcode != 0) dbBook.Barcode = barcode;

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
            long.TryParse(barcode.Replace("-", string.Empty).Replace("_", string.Empty), out long id);
            if (id > 9785000000000 && id < 9785999999999)
            {
                return id;
            }

            return 0;
        }

        public IEnumerable<long> Parse(string[] barcodes)
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
