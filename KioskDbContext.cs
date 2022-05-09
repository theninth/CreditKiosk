using CreditKiosk.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace CreditKiosk
{
    public class KioskDbContext : DbContext
    {
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Deposit> Deposits { get; set; }

        public DbSet<Person> Persons { get; set; }

        public DbSet<ProductGroup> ProductGroups { get; set; }

        public string DbPath { get; }

        public KioskDbContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "pos.db");
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductGroup>().HasData(new ProductGroup { Id = 1, Name = "Övrigt" });
            modelBuilder.Entity<ProductGroup>().HasData(new ProductGroup { Id = 2, Name = "Glass" });

            modelBuilder.Entity<Person>().HasData(new Person { Id = 1, FirstName = "Testperson", LastName = "1" });
            modelBuilder.Entity<Person>().HasData(new Person { Id = 2, FirstName = "Testperson", LastName = "2"});
        }
    }
}
