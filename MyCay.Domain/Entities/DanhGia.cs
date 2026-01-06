using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCay.Domain.Entities;

[Table("DanhGia")]
public class DanhGia
{
    [Key]
    public int MaDG { get; set; }
    
    public int? MaKH { get; set; }
    
    public int? MaSP { get; set; }
    
    public int? MaDH { get; set; } // Đơn hàng liên quan (nếu có)
    
    [Required, MaxLength(100)]
    public string TenKhach { get; set; } = string.Empty;
    
    [MaxLength(15)]
    public string? SDT { get; set; }
    
    [MaxLength(100)]
    public string? Email { get; set; }
    
    [Range(1, 5)]
    public int SoSao { get; set; } = 5; // 1-5 sao
    
    [Required, MaxLength(1000)]
    public string NoiDung { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? HinhAnh { get; set; } // URL ảnh đánh giá (nếu có)
    
    public DateTime NgayDanhGia { get; set; } = DateTime.Now;
    
    // Phản hồi từ admin
    [MaxLength(1000)]
    public string? PhanHoi { get; set; }
    
    public int? MaNVPhanHoi { get; set; } // Nhân viên phản hồi
    
    public DateTime? NgayPhanHoi { get; set; }
    
    public bool DaXem { get; set; } = false;
    
    public bool HienThi { get; set; } = true; // Hiển thị công khai
    
    public bool DaDuyet { get; set; } = false; // Admin duyệt
    
    // Navigation
    [ForeignKey("MaKH")]
    public virtual KhachHang? KhachHang { get; set; }
    
    [ForeignKey("MaSP")]
    public virtual SanPham? SanPham { get; set; }
    
    [ForeignKey("MaDH")]
    public virtual DonHang? DonHang { get; set; }
}
