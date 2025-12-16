using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCay.Domain.Entities;

[Table("SanPham")]
public class SanPham
{
    [Key]
    public int MaSP { get; set; }
    
    [MaxLength(20)]
    public string? MaSPCode { get; set; }
    
    [Required, MaxLength(150)]
    public string TenSP { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? MoTa { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal DonGia { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal? GiaKhuyenMai { get; set; }
    
    [MaxLength(255)]
    public string? HinhAnh { get; set; }
    
    public int? MaDM { get; set; }
    
    public int CapDoCay { get; set; } = 0;
    
    public bool NoiBat { get; set; } = false;
    
    public bool TrangThai { get; set; } = true;
    
    public DateTime NgayTao { get; set; } = DateTime.Now;
    
    public DateTime? NgayCapNhat { get; set; }
    
    // Navigation
    [ForeignKey("MaDM")]
    public virtual DanhMuc? DanhMuc { get; set; }
    
    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();
    public virtual ICollection<GioHang> GioHangs { get; set; } = new List<GioHang>();
}
