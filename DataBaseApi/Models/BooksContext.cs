using Microsoft.EntityFrameworkCore;

namespace DataBaseApi.Models
{
    public sealed class BooksContext : DbContext
    {
        public DbSet<Book> Books { get; set; }

        public BooksContext(DbContextOptions<BooksContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
