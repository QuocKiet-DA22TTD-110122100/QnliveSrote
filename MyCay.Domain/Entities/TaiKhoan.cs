using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCay.Domain.Entities;

[Table("TaiKhoan")]
public class TaiKhoan
{
    [Key]
    public int MaTK { get; set; }
    
    [Required, MaxLength(50)]
    public string TenDangNhap { get; set; } = string.Empty;
    
    [Required, MaxLength(255)]
    public string MatKhau { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string? Email { get; set; }
    
    public bool TrangThai { get; set; } = true;
    
    public int MaVaiTro { get; set; }
    
    public DateTime NgayTao { get; set; } = DateTime.Now;
    
    public DateTime? LanDangNhapCuoi { get; set; }
    
    // Navigation
    [ForeignKey("MaVaiTro")]
    public virtual VaiTro? VaiTro { get; set; }
    
    public virtual KhachHang? KhachHang { get; set; }
    public virtual NhanVien? NhanVien { get; set; }
}
