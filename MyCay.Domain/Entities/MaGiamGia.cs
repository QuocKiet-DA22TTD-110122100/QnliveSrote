using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCay.Domain.Entities;

[Table("MaGiamGia")]
public class MaGiamGia
{
    [Key]
    public int MaMGG { get; set; }
    
    [Required, MaxLength(50)]
    public string MaCode { get; set; } = string.Empty; // VD: SASIN10, FREESHIP
    
    [MaxLength(255)]
    public string? MoTa { get; set; }
    
    /// <summary>
    /// Loại giảm giá: "percent" (%), "fixed" (số tiền cố định), "freeship"
    /// </summary>
    [MaxLength(20)]
    public string LoaiGiam { get; set; } = "percent";
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal GiaTri { get; set; } // 10 = 10% hoặc 10000đ
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal? GiamToiDa { get; set; } // Giảm tối đa (cho loại percent)
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal DonToiThieu { get; set; } = 0; // Đơn tối thiểu để áp dụng
    
    public int SoLuong { get; set; } = 100; // Số lượng mã
    
    public int DaSuDung { get; set; } = 0; // Đã sử dụng
    
    public DateTime? NgayBatDau { get; set; }
    
    public DateTime? NgayKetThuc { get; set; }
    
    public bool TrangThai { get; set; } = true;
    
    public DateTime NgayTao { get; set; } = DateTime.Now;
}
