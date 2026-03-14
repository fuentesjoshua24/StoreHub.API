using Microsoft.EntityFrameworkCore;
using StoreHub.API.Common.Model;
using System.Reflection.Emit;

namespace StoreHub.API.Common.Data
{
    public class StoreHubDbContext : DbContext
    {
        public StoreHubDbContext(DbContextOptions<StoreHubDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map User entity to "user" table
            modelBuilder.Entity<User>().ToTable("user");
            modelBuilder.Entity<Product>().ToTable("product");
        }
    }
}
