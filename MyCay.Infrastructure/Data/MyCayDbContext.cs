using Microsoft.EntityFrameworkCore;
using MyCay.Domain.Entities;

namespace MyCay.Infrastructure.Data;

public class MyCayDbContext : DbContext
{
    public MyCayDbContext(DbContextOptions<MyCayDbContext> options) : base(options)
    {
    }

    public DbSet<VaiTro> VaiTros { get; set; }
    public DbSet<TaiKhoan> TaiKhoans { get; set; }
    public DbSet<KhachHang> KhachHangs { get; set; }
    public DbSet<NhanVien> NhanViens { get; set; }
    public DbSet<DanhMuc> DanhMucs { get; set; }
    public DbSet<SanPham> SanPhams { get; set; }
    public DbSet<DonHang> DonHangs { get; set; }
    public DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }
    public DbSet<GioHang> GioHangs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // TaiKhoan - unique TenDangNhap
        modelBuilder.Entity<TaiKhoan>()
            .HasIndex(t => t.TenDangNhap)
            .IsUnique();

        // SanPham - unique MaSPCode
        modelBuilder.Entity<SanPham>()
            .HasIndex(s => s.MaSPCode)
            .IsUnique();

        // DonHang - unique MaDHCode
        modelBuilder.Entity<DonHang>()
            .HasIndex(d => d.MaDHCode)
            .IsUnique();

        // ChiTietDonHang - ThanhTien is computed column in database
        modelBuilder.Entity<ChiTietDonHang>()
            .Property(c => c.ThanhTien)
            .ValueGeneratedOnAddOrUpdate();
    }
}
