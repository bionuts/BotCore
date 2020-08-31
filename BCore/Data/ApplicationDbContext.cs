using BotCore.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BCore.Data
{
    class ApplicationDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string path = System.IO.Path.Combine(Environment.CurrentDirectory, "db.sqlite3");
            optionsBuilder.UseSqlite($"Filename={path}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<BOrderAccounts>().HasKey(oa => new { oa.OrderID, oa.UserId });
            
            modelBuilder.Entity<BOrderAccounts>()
                .HasOne(oa => oa.BOrder)
                .WithMany(s => s.OrderAccounts)
                .HasForeignKey(oa => oa.OrderID);

            modelBuilder.Entity<BOrderAccounts>()
                .HasOne(oa => oa.BAccount)
                .WithMany(s => s.OrderAccounts)
                .HasForeignKey(oa => oa.UserId);
        }

        public DbSet<BOrder> BOrders { get; set; }
        public DbSet<BSymbole> BSymboles { get; set; }
        public DbSet<BSetting> BSettings { get; set; }
        public DbSet<BAccount> BAccounts { get; set; }
        public DbSet<BOrderAccounts> BOrderAccounts { get; set; }
    }
}
