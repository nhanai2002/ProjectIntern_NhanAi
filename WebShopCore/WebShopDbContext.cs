using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopCore.Configurations;
using WebShopCore.Model;

namespace WebShopCore
{
    public class WebShopDbContext : DbContext
    {
        public WebShopDbContext(DbContextOptions<WebShopDbContext> options) : base(options)
        {
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());

            modelBuilder.Entity<Cart>().Property(u => u.CouponId).IsRequired(false); // Cho phép null
            modelBuilder.Entity<Order>().Property(u => u.FeedbackId).IsRequired(false); // Cho phép null
            modelBuilder.Entity<Image>().Property(u => u.FeedbackId).IsRequired(false);


            modelBuilder.Entity<ProductCategory>(e => {
                e.ToTable("ProductCategory");
                e.HasKey(p => new { p.CategoryId, p.ProductId });

                e.HasOne(p => p.Category)
                .WithMany(p => p.ProductCategories)
                .HasForeignKey(p => p.CategoryId)
                .HasConstraintName("FK_ProductCategory_Category");

                e.HasOne(p => p.Product)
                .WithMany(p => p.ProductCategories)
                .HasForeignKey(p => p.ProductId)
                .HasConstraintName("FK_ProductCategory_Product");
            });

        }

        public DbSet<Product> products { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<ProductCategory> productCategories { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<Role> roles { get; set; }
        public DbSet<ActionCtrl> actions { get; set; }
        public DbSet<Image> images { get; set; }
        public DbSet<ProductImage> productImages { get; set; }
        public DbSet<Coupon> coupons { get; set; }
        public DbSet<CouponHistory> couponHistories { get; set; }
        public DbSet<Cart> carts { get; set; }
        public DbSet<CartItem> cartItems { get; set; }
        public DbSet<CartHistory> cartHistories { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<OrderItem> orderItems { get; set; }

    }
}
