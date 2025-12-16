using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCay.Domain.Entities;

[Table("ChiTietDonHang")]
public class ChiTietDonHang
{
    [Key]
    public int MaCTDH { get; set; }
    
    public int MaDH { get; set; }
    
    public int MaSP { get; set; }
    
    [Required, MaxLength(150)]
    public string TenSP { get; set; } = string.Empty;
    
    public int SoLuong { get; set; } = 1;
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal DonGia { get; set; }
    
    public int CapDoCay { get; set; } = 0;
    
    [MaxLength(50)]
    public string? LoaiNuocDung { get; set; }
    
    [MaxLength(200)]
    public string? GhiChu { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public decimal ThanhTien { get; private set; }
    
    // Navigation
    [ForeignKey("MaDH")]
    public virtual DonHang? DonHang { get; set; }
    
    [ForeignKey("MaSP")]
    public virtual SanPham? SanPham { get; set; }
}
