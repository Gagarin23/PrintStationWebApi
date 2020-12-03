using System;
using System.ComponentModel.DataAnnotations;

namespace DataBaseApi.Models
{
    /// <summary>
    ///     Объект книги в БД.
    /// </summary>
    public class Book : IEquatable<Book>
    {
        [Key]
        [Range(9785000000000, 9785999999999, ErrorMessage = "Неверный ISBN.")]
        public long Barcode { get; set; }

        public string BlockPath { get; set; }

        public string CoverPath { get; set; }

        public DateTime BlockCreationTime { get; set; }

        public DateTime CoverCreationTime { get; set; }

        public bool Equals(Book other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Barcode == other.Barcode;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Book) obj);
        }

        public override int GetHashCode()
        {
            return (Barcode.GetHashCode());
        }
    }
}