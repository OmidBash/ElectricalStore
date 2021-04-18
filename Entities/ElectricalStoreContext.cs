using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class ElectricalStoreContext : IdentityDbContext
    {
        public ElectricalStoreContext(DbContextOptions<ElectricalStoreContext> opts) : base(opts) { }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<CategoryProductJunction> CategoryProductJunction { get; set; }

        public DbSet<Feature> Features { get; set; }

        public DbSet<FeatureType> FeatureTypes { get; set; }

        public DbSet<ProductFeatureJunction> ProductFeatureJunction { get; set; }

        public DbSet<ProductImage> ProductImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("Store");

            #region Feature
            modelBuilder.Entity<Feature>()
                .HasOne<FeatureType>(f => f.FeatureType)
                .WithMany(ft => ft.Features)
                .HasForeignKey(ft => ft.FeatureTypeId)
                .HasConstraintName("FK_FeatureType_Feature")
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region ProductImage
            modelBuilder.Entity<ProductImage>()
                .HasOne<Product>(pi => pi.Product)
                .WithMany(p => p.ProductImage)
                .HasForeignKey(pi => pi.ProductId)
                .HasConstraintName("FK_Product_Image")
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region CategoryProductJunction
            modelBuilder.Entity<CategoryProductJunction>()
                .HasKey(cp => new { cp.CategoryId, cp.ProductId });

            modelBuilder.Entity<CategoryProductJunction>()
                .HasOne(cp => cp.Category)
                .WithMany(c => c.CategoryProduct)
                .HasForeignKey(cp => cp.CategoryId)
                .HasConstraintName("FK_Category_Product")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CategoryProductJunction>()
                .HasOne(cp => cp.Product)
                .WithMany(p => p.CategoryProduct)
                .HasForeignKey(cp => cp.ProductId)
                .HasConstraintName("FK_Product_Category")
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region ProductFeatureJunction
            modelBuilder.Entity<ProductFeatureJunction>()
                .HasKey(pf => new { pf.ProductId, pf.FeatureId });

            modelBuilder.Entity<ProductFeatureJunction>()
                .HasOne(pf => pf.Product)
                .WithMany(p => p.ProductFeature)
                .HasForeignKey(pf => pf.ProductId)
                .HasConstraintName("FK_Product_Feature")
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductFeatureJunction>()
                .HasOne(pf => pf.Feature)
                .WithMany(f => f.ProductFeature)
                .HasForeignKey(pf => pf.FeatureId)
                .HasConstraintName("FK_Feature_Product")
                .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}