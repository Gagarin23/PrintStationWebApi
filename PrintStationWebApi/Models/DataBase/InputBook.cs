using System;
using System.ComponentModel.DataAnnotations;

namespace PrintStationWebApi.Models.DataBase
{
    public class InputBook
    {
        public string Barcode { get; set; }

        public string BlockPath { get; set; }

        public string CoverPath { get; set; }

        public DateTime BlockCreationTime { get; set; }

        public DateTime CoverCreationTime { get; set; }
    }
}
