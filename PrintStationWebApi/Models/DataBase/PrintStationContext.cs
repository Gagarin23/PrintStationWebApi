using Microsoft.EntityFrameworkCore;

namespace PrintStationWebApi.Models.DataBase
{
    public sealed class PrintStationContext : DbContext
    {
        public DbSet<DataBaseBook> Books { get; set; }
        public DbSet<User> Users { get; set; }

        public PrintStationContext(DbContextOptions<PrintStationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
