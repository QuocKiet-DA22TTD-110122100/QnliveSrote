using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCay.Domain.Entities;

[Table("NguyenVatLieu")]
public class NguyenVatLieu
{
    [Key]
    public int MaNVL { get; set; }
    
    [Required, MaxLength(100)]
    public string TenNVL { get; set; } = string.Empty;
    
    [MaxLength(20)]
    public string? DonViTinh { get; set; } // kg, g, lít, cái...
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal GiaNhap { get; set; }
    
    public int SoLuongToiThieu { get; set; } = 10; // Ngưỡng cảnh báo
    
    [MaxLength(50)]
    public string? NhomNVL { get; set; } // Thịt, Rau, Gia vị, Mì...
    
    public bool TrangThai { get; set; } = true;
    
    public DateTime NgayTao { get; set; } = DateTime.Now;
    
    // Navigation
    public virtual ICollection<TonKho> TonKhos { get; set; } = new List<TonKho>();
    public virtual ICollection<CongThuc> CongThucs { get; set; } = new List<CongThuc>();
}
