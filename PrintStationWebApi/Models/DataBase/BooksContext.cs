using Microsoft.EntityFrameworkCore;

namespace PrintStationWebApi.Models.DataBase
{
    public sealed class BooksContext : DbContext
    {
        public DbSet<DataBaseBook> Books { get; set; }

        public BooksContext(DbContextOptions<BooksContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
