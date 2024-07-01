using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DAL.Models
{
    public partial class PharmEtradeDBContext : DbContext
    {
        public PharmEtradeDBContext()
        {
        }

        public PharmEtradeDBContext(DbContextOptions<PharmEtradeDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AddProduct> AddProducts { get; set; } = null!;
        public virtual DbSet<AddtoCartproduct> AddtoCartproducts { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<ProductGallery> ProductGalleries { get; set; } = null!;
        public virtual DbSet<ProductSize> ProductSizes { get; set; } = null!;
        public virtual DbSet<RoleMaster> RoleMasters { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-R2D4O65;Database=PharmEtradeDB;Integrated Security=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AddProduct>(entity =>
            {
                entity.ToTable("AddProduct");

                entity.Property(e => e.AddproductId).HasColumnName("AddproductID");

                entity.Property(e => e.BrandName).HasMaxLength(255);

                entity.Property(e => e.ExpirationDate).HasColumnType("date");

                entity.Property(e => e.Fromdate).HasColumnType("date");

                entity.Property(e => e.ImageId).HasColumnName("ImageID");

                entity.Property(e => e.LotNumber).HasMaxLength(50);

                entity.Property(e => e.Manufacturer).HasMaxLength(255);

                entity.Property(e => e.NdcorUpc)
                    .HasMaxLength(50)
                    .HasColumnName("NDCorUPC");

                entity.Property(e => e.PackCondition).HasMaxLength(50);

                entity.Property(e => e.PackType).HasMaxLength(50);

                entity.Property(e => e.PriceName).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ProductDescription)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.ProductName).HasMaxLength(255);

                entity.Property(e => e.ProductcategoryId).HasColumnName("Productcategory_id");

                entity.Property(e => e.SalePrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.SalePriceFrom).HasColumnType("date");

                entity.Property(e => e.SalePriceTo).HasColumnType("date");

                entity.Property(e => e.Strength).HasMaxLength(255);

                entity.Property(e => e.UpnmemberPrice)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("UPNmemberPrice");

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.AddProducts)
                    .HasForeignKey(d => d.ImageId)
                    .HasConstraintName("FK__AddProduc__Image__787EE5A0");

                entity.HasOne(d => d.Productcategory)
                    .WithMany(p => p.AddProducts)
                    .HasForeignKey(d => d.ProductcategoryId)
                    .HasConstraintName("FK__AddProduc__Produ__778AC167");

                entity.HasOne(d => d.Size)
                    .WithMany(p => p.AddProducts)
                    .HasForeignKey(d => d.Sizeid)
                    .HasConstraintName("FK__AddProduc__Sizei__797309D9");
            });

            modelBuilder.Entity<AddtoCartproduct>(entity =>
            {
                entity.HasKey(e => e.AddtoCartId)
                    .HasName("PK__AddtoCar__A2738A1548144F16");

                entity.ToTable("AddtoCartproduct");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.CategoryName)
                    .HasMaxLength(255)
                    .HasColumnName("category_name");
            });

            modelBuilder.Entity<ProductGallery>(entity =>
            {
                entity.HasKey(e => e.GalleryId)
                    .HasName("PK__ProductG__43D54A71F2072C5A");

                entity.ToTable("ProductGallery");

                entity.Property(e => e.GalleryId).HasColumnName("gallery_id");

                entity.Property(e => e.Caption)
                    .HasMaxLength(255)
                    .HasColumnName("caption");

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(255)
                    .HasColumnName("image_url");
            });

            modelBuilder.Entity<ProductSize>(entity =>
            {
                entity.HasKey(e => e.Sizeid)
                    .HasName("PK__ProductS__83BA0D52BE0696A4");

                entity.ToTable("ProductSize");

                entity.Property(e => e.Height)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Length)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Weight)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Width)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RoleMaster>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .HasName("PK__RoleMast__8AFACE3A2F16BA53");

                entity.ToTable("RoleMaster");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DateUpdated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasIndex(e => e.PhoneNumber, "UQ__users__A1936A6B934285B7")
                    .IsUnique();

                entity.HasIndex(e => e.PhoneNumber, "UQ__users__A1936A6BEE7D2AE7")
                    .IsUnique();

                entity.HasIndex(e => e.PhoneNumber, "UQ__users__A1936A6BF1DA909A")
                    .IsUnique();

                entity.HasIndex(e => e.Email, "UQ__users__AB6E6164B9AD0760")
                    .IsUnique();

                entity.HasIndex(e => e.Email, "UQ__users__AB6E6164CBC8F37A")
                    .IsUnique();

                entity.HasIndex(e => e.Email, "UQ__users__AB6E6164DF25D3CA")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("phone_number");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Username)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("username");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
