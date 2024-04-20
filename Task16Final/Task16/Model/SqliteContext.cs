using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Task16.Model
{
    public class SqliteContext : DbContext
    {
        public SqliteContext() => Database.EnsureCreated();
        public DbSet<Client> Clients {get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlite("Data Source=CyberStore.db");
        }
    }
}
