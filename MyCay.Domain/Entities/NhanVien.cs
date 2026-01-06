using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCay.Domain.Entities;

[Table("NhanVien")]
public class NhanVien
{
    [Key]
    public int MaNV { get; set; }
    
    [Required, MaxLength(100)]
    public string HoTen { get; set; } = string.Empty;
    
    public DateTime? NgaySinh { get; set; }
    
    [MaxLength(10)]
    public string? GioiTinh { get; set; }
    
    [Required, MaxLength(15)]
    public string SDT { get; set; } = string.Empty;
    
    [MaxLength(200)]
    public string? DiaChi { get; set; }
    
    [MaxLength(50)]
    public string ChucVu { get; set; } = "Nhân viên";
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal? Luong { get; set; }
    
    public int MaCH { get; set; }
    
    public int? MaTK { get; set; }
    
    public int? MaCN { get; set; } // Chi nhánh làm việc
    
    public DateTime NgayVaoLam { get; set; } = DateTime.Now;
    
    public bool TrangThai { get; set; } = true;
    
    // Navigation
    [ForeignKey("MaTK")]
    public virtual TaiKhoan? TaiKhoan { get; set; }
    
    [ForeignKey("MaCN")]
    public virtual ChiNhanh? ChiNhanh { get; set; }
}
