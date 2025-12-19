using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCay.Domain.Entities;

/// <summary>
/// Công thức/Định lượng nguyên vật liệu cho mỗi sản phẩm
/// </summary>
[Table("CongThuc")]
public class CongThuc
{
    [Key]
    public int MaCT { get; set; }
    
    public int MaSP { get; set; }
    
    public int MaNVL { get; set; }
    
    [Column(TypeName = "decimal(18,3)")]
    public decimal SoLuong { get; set; } // Số lượng NVL cần cho 1 sản phẩm
    
    [MaxLength(255)]
    public string? GhiChu { get; set; }
    
    // Navigation
    [ForeignKey("MaSP")]
    public virtual SanPham? SanPham { get; set; }
    
    [ForeignKey("MaNVL")]
    public virtual NguyenVatLieu? NguyenVatLieu { get; set; }
}
