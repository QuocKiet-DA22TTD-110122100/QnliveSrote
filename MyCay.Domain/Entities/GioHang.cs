using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCay.Domain.Entities;

[Table("GioHang")]
public class GioHang
{
    [Key]
    public int MaGH { get; set; }
    
    public int? MaKH { get; set; }
    
    [MaxLength(100)]
    public string? SessionID { get; set; }
    
    public int MaSP { get; set; }
    
    public int SoLuong { get; set; } = 1;
    
    public int CapDoCay { get; set; } = 0;
    
    [MaxLength(50)]
    public string? LoaiNuocDung { get; set; }
    
    [MaxLength(200)]
    public string? GhiChu { get; set; }
    
    public DateTime NgayThem { get; set; } = DateTime.Now;
    
    // Navigation
    [ForeignKey("MaKH")]
    public virtual KhachHang? KhachHang { get; set; }
    
    [ForeignKey("MaSP")]
    public virtual SanPham? SanPham { get; set; }
}
