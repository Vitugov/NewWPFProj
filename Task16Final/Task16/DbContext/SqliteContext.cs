using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using Task16.Model;
using WPFUsefullThings;

namespace Task16
{
    public class SqliteContext : DbContext
    {
        public SqliteContext() => Database.EnsureCreated();
        public DbSet<ProjectModel> Items { get; set; }
        public DbSet<Client> Clients {get; set; }
        public DbSet<Commodity> Commodities { get; set; }
        public DbSet<OrderRow> OrderRows { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
            . SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlite(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectModel>().UseTpcMappingStrategy();  // Используем стратегию TPC
        }
    }
}
