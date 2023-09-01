using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ApiProxy.Logic.Database
{
    public class MyDbContext : DbContext
    {
        public static MyDbContext GetMyDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();
            var options = optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)).Options;
            return new MyDbContext(options);
        }
        public DbSet<Merchant> Merchants { get; set; } = null!;
        public DbSet<Invoice> Invoices { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Merchant>().HasAlternateKey(m => m.MerchantId);
        }
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
