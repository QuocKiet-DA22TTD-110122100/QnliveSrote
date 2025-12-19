using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCay.Domain.Entities;

[Table("ChiNhanh")]
public class ChiNhanh
{
    [Key]
    public int MaCN { get; set; }
    
    [Required, MaxLength(100)]
    public string TenChiNhanh { get; set; } = string.Empty;
    
    [MaxLength(255)]
    public string? DiaChi { get; set; }
    
    [MaxLength(100)]
    public string? QuanHuyen { get; set; }
    
    [MaxLength(100)]
    public string? ThanhPho { get; set; }
    
    [MaxLength(20)]
    public string? SoDienThoai { get; set; }
    
    [MaxLength(100)]
    public string? Email { get; set; }
    
    [MaxLength(50)]
    public string? GioMoCua { get; set; } // VD: "10:00"
    
    [MaxLength(50)]
    public string? GioDongCua { get; set; } // VD: "22:00"
    
    public bool TrangThai { get; set; } = true;
    
    public DateTime NgayTao { get; set; } = DateTime.Now;
    
    // Navigation
    public virtual ICollection<NhanVien> NhanViens { get; set; } = new List<NhanVien>();
    public virtual ICollection<TonKho> TonKhos { get; set; } = new List<TonKho>();
    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
}
