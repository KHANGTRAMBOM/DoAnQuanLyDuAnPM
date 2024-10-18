using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data;
using BookWebsite.Models;
using Microsoft.AspNetCore.Identity;

namespace BookWebsite.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityUser>(entity =>
            {
                entity.ToTable(name: "Users");
            });

            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Roles");
            });

            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });

            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });

            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });
        }
        public DbSet<BookWebsite.Models.Book> Book { get; set; } = default!;
        public DbSet<BookWebsite.Models.TheLoai> TheLoai { get; set; } = default!;
        public DbSet<BookWebsite.Models.ChiTietDonHang> ChiTietDonHang { get; set; } = default!;
        public DbSet<BookWebsite.Models.DanhGia> DanhGia { get; set; } = default!;
        public DbSet<BookWebsite.Models.DonHang> DonHang { get; set; } = default!;
        public DbSet<BookWebsite.Models.GioHang> GioHang { get; set; } = default!;
        public DbSet<BookWebsite.Models.GioHangItem> GioHangItem { get; set; } = default!;
        public DbSet<BookWebsite.Models.LoaiPhuongThucThanhToan> LoaiPhuongThucThanhToan { get; set; } = default!;
        public DbSet<BookWebsite.Models.ThanhToan> ThanhToan { get; set; } = default!;

      


    }
}