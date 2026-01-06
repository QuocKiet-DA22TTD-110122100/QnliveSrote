using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCay.Domain.Entities;

[Table("KhachHang")]
public class KhachHang
{
    [Key]
    public int MaKH { get; set; }
    
    [Required, MaxLength(100)]
    public string HoTen { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string? Email { get; set; }
    
    [Required, MaxLength(15)]
    public string SDT { get; set; } = string.Empty;
    
    [MaxLength(200)]
    public string? DiaChi { get; set; }
    
    public DateTime? NgaySinh { get; set; }
    
    public int DiemTichLuy { get; set; } = 0;
    
    public int? MaTK { get; set; }
    
    public DateTime NgayDangKy { get; set; } = DateTime.Now;
    
    public bool TrangThai { get; set; } = true;
    
    // Navigation
    [ForeignKey("MaTK")]
    public virtual TaiKhoan? TaiKhoan { get; set; }
    
    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
    public virtual ICollection<GioHang> GioHangs { get; set; } = new List<GioHang>();
}
