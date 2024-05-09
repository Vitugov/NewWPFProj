using Microsoft.EntityFrameworkCore;
using WPFUsefullThings;

namespace Task16.Model
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
            optionsBuilder
                .UseSqlite("Data Source=CyberStore.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectModel>().UseTpcMappingStrategy();  // Используем стратегию TPC
        }
    }
}
