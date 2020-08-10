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
            optionsBuilder.UseSqlServer(@"Data Source=.;Initial Catalog=BcoreDB;Integrated Security=True");
        }
        public DbSet<BOrder> BOrders { get; set; }
        public DbSet<BCookie> BCookies { get; set; }
        public DbSet<BSymbole> BSymboles { get; set; }
    }
}
