using System;

namespace PrintStationWebApi.Models.BL
{
    public class Block : Book, IEquatable<Block>
    {
        public string SheetFormat { get; set; }

        public bool Equals(Block other)
        {
            if (other == null)
                return false;

            if (BookFormat == other.BookFormat
                && BookMount == other.BookMount
                && SheetFormat == other.SheetFormat)
                return true;

            return false;
        }

        public override bool Equals(Book other)
        {
            return Equals(other as Block);
        }

        public override int GetHashCode()
        {
            var hashBookFormat = BookFormat == null ? 0 : BookFormat.GetHashCode();
            var hashBookMount = BookMount == null ? 0 : BookMount.GetHashCode();
            var hashSheetFormat = SheetFormat == null ? 0 : SheetFormat.GetHashCode();

            return hashBookFormat ^ hashBookMount ^ hashSheetFormat;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}