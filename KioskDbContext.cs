﻿using CreditKiosk.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace CreditKiosk
{
    public class KioskDbContext : DbContext
    {
        public DbSet<Purchase>? Purchases { get; set; }
        public DbSet<Deposit>? Deposits { get; set; }

        public DbSet<Person>? Persons { get; set; }

        public DbSet<ProductGroup>? ProductGroups { get; set; }

        public string DbPath { get; }

        public KioskDbContext()
        {
            string appName = (string)App.Current.Resources["AppName"];
            string appDataFolder = Helpers.DataPaths.CreateAndGetAppDataFolder();
            DbPath = Path.Join(appDataFolder, $"{appName}.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductGroup>().HasData(new ProductGroup { Id = 1, Name = "Övrigt" });
            modelBuilder.Entity<ProductGroup>().HasData(new ProductGroup { Id = 2, Name = "Glass" });

            modelBuilder.Entity<Person>().HasData(new Person { Id = 1, FirstName = "Testperson", LastName = "1" });
            modelBuilder.Entity<Person>().HasData(new Person { Id = 2, FirstName = "Testperson", LastName = "2"});

            modelBuilder.Entity<Deposit>().HasData(new Deposit { Id = 1, PersonId = 1, Amount = 150, Comment = "Startkredit" });
            modelBuilder.Entity<Deposit>().HasData(new Deposit { Id = 2, PersonId = 2, Amount = 150, Comment = "Startkredit" });
        }
    }
}
