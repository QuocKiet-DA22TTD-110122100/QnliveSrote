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
    
    // Quản lý Chi nhánh
    public DbSet<ChiNhanh> ChiNhanhs { get; set; }
    
    // Quản lý Kho/Nguyên vật liệu
    public DbSet<NguyenVatLieu> NguyenVatLieus { get; set; }
    public DbSet<TonKho> TonKhos { get; set; }
    public DbSet<CongThuc> CongThucs { get; set; }
    
    // Mã giảm giá
    public DbSet<MaGiamGia> MaGiamGias { get; set; }

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
        
        // MaGiamGia - unique MaCode
        modelBuilder.Entity<MaGiamGia>()
            .HasIndex(m => m.MaCode)
            .IsUnique();
        
        // TonKho - unique combination of MaCN + MaNVL
        modelBuilder.Entity<TonKho>()
            .HasIndex(t => new { t.MaCN, t.MaNVL })
            .IsUnique();
    }
}
