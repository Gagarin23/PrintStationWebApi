using System;
using System.ComponentModel.DataAnnotations;

namespace PrintStationWebApi.Models.BL
{
    public abstract class Book : IEquatable<Book>
    {
        [Required]
        public string Barcode { get; set; }

        public string Name { get; set; }

        [Required]
        public short NumberOfCopies { get; set; }

        [Required]
        public string BookFormat { get; set; }

        [Required]
        public string BookMount { get; set; }

        [Required]
        public byte Imposition { get; set; }

        public double PrintСoefficient { get; set; }

        public string FullPath { get; set; }

        public virtual bool Equals(Book other)
        {
            if (other == null)
                return false;

            if (Barcode == other.Barcode)
                return true;

            return false;
        }
        public override int GetHashCode()
        {
            return Barcode == null ? 0 : Barcode.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Barcode}: {Name}";
        }
    }
}