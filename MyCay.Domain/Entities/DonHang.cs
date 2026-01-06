using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCay.Domain.Entities;

[Table("DonHang")]
public class DonHang
{
    [Key]
    public int MaDH { get; set; }
    
    [MaxLength(20)]
    public string? MaDHCode { get; set; }
    
    public int? MaKH { get; set; }
    
    [MaxLength(100)]
    public string? TenKhach { get; set; }
    
    [MaxLength(15)]
    public string? SDTKhach { get; set; }
    
    [MaxLength(200)]
    public string? DiaChiGiao { get; set; }
    
    public DateTime? NgayDat { get; set; } = DateTime.Now;
    
    public DateTime? NgayGiao { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal TamTinh { get; set; } = 0;
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal PhiGiaoHang { get; set; } = 15000;
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal GiamGia { get; set; } = 0;
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal TongTien { get; set; } = 0;
    
    [MaxLength(50)]
    public string PhuongThucThanhToan { get; set; } = "Tiền mặt";
    
    [MaxLength(50)]
    public string TrangThaiThanhToan { get; set; } = "Chưa thanh toán";
    
    [MaxLength(50)]
    public string TrangThai { get; set; } = "Chờ xác nhận";
    
    [MaxLength(500)]
    public string? GhiChu { get; set; }
    
    public int? MaCH { get; set; }
    public int? MaNV { get; set; }
    public int? MaCN { get; set; } // Chi nhánh xử lý đơn
    public int? MaMGG { get; set; } // Mã giảm giá đã áp dụng
    
    [MaxLength(50)]
    public string? MaGiamGiaCode { get; set; } // Lưu mã code để hiển thị
    
    public DateTime? NgayCapNhat { get; set; }
    
    // Navigation
    [ForeignKey("MaKH")]
    public virtual KhachHang? KhachHang { get; set; }
    
    [ForeignKey("MaCN")]
    public virtual ChiNhanh? ChiNhanh { get; set; }
    
    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();
}
