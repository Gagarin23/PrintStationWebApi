using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PrintStationWebApi.Logger;

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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(MyLoggerFactory);
            optionsBuilder.LogTo(Console.WriteLine);
        }

        private static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => //todo: не работает, исправить.
        {
            builder.AddFilter((category, level) => category == DbLoggerCategory.Database.Command.Name)
                .AddProvider(new FileLoggerProvider("DbLogger.txt"));
        });
    }
}

