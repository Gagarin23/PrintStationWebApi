using System;
using System.ComponentModel.DataAnnotations;

namespace PrintStationWebApi.Models.DataBase
{
    public class InputBook
    {
        [Range(9785000000000, 9785999999999, ErrorMessage = "Неверный ISBN.")]
        public long Barcode { get; set; }

        public string BlockPath { get; set; }

        public string CoverPath { get; set; }

        public DateTime BlockCreationTime { get; set; }

        public DateTime CoverCreationTime { get; set; }
    }
}
