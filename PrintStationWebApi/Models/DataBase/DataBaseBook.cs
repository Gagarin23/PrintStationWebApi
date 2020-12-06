using System;
using System.ComponentModel.DataAnnotations;

namespace PrintStationWebApi.Models.DataBase
{
    /// <summary>
    ///     Объект книги в БД.
    /// </summary>
    public class DataBaseBook : IEquatable<DataBaseBook>
    {
        [Key]
        public long Barcode { get; set; }

        public string BlockPath { get; set; }

        public string CoverPath { get; set; }

        public DateTime BlockCreationTime { get; set; }

        public DateTime CoverCreationTime { get; set; }

        public bool Equals(DataBaseBook other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Barcode == other.Barcode;
        }

        public override int GetHashCode()
        {
            return (Barcode.GetHashCode());
        }
    }
}