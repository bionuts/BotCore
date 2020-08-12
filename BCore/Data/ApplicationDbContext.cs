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
        public DbSet<BOrder> BOrders { get; set; }
        public DbSet<BSymbole> BSymboles { get; set; }
        public DbSet<BSetting> BSettings { get; set; }
    }
}
