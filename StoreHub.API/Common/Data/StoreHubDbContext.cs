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
        public DbSet<Category> Categories { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Sale> Sales { get; set; }

        public DbSet<Seller> Sellers { get; set; }
        public DbSet<SellerMapping> SellerMappings { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map User entity to "user" table
            modelBuilder.Entity<User>().ToTable("user");
            modelBuilder.Entity<Seller>().ToTable("seller");
            modelBuilder.Entity<Product>().ToTable("product");
            modelBuilder.Entity<Category>().ToTable("category");
            modelBuilder.Entity<Promotion>().ToTable("promotion");
            modelBuilder.Entity<Sale>().ToTable("sale");
            modelBuilder.Entity<SellerMapping>().ToTable("sellermapping");


            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Product)
                .WithMany()
                .HasForeignKey(s => s.ProductId);
            
            modelBuilder.Entity<Seller>()
                .HasOne(s => s.User)
                .WithOne(u => u.Seller)
                .HasForeignKey<Seller>(s => s.UserId);

            modelBuilder.Entity<SellerMapping>()
                .HasKey(sm => sm.SellerMappingId);

            //modelBuilder.Entity<SellerMapping>()
            //    .HasOne(sm => sm.Seller)
            //    .WithMany()
            //    .HasForeignKey(sm => sm.SellerId);

            //modelBuilder.Entity<SellerMapping>()
            //    .HasOne(sm => sm.Product)
            //    .WithMany(p => p.SellerMappings)
            //    .HasForeignKey(sm => sm.ProductId);

            //modelBuilder.Entity<ProductMapping>().ToTable("productmapping");
        }
    }
}
