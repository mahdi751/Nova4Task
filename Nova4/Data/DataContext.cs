using Microsoft.EntityFrameworkCore;
using Nova4.Models;

namespace Nova4.Data
{
    public class DataContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
         
        public DbSet<BitcoinPrice> BitcoinPrices { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BitcoinPrice>()
            .Property(b => b.Price)
            .HasColumnType("decimal(18, 2)");

        }

    }
}
