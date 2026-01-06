using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCay.Domain.Entities;

[Table("DanhMuc")]
public class DanhMuc
{
    [Key]
    public int MaDM { get; set; }
    
    [Required, MaxLength(100)]
    public string TenDanhMuc { get; set; } = string.Empty;
    
    [MaxLength(200)]
    public string? MoTa { get; set; }
    
    [MaxLength(255)]
    public string? HinhAnh { get; set; }
    
    public int ThuTu { get; set; } = 0;
    
    public bool TrangThai { get; set; } = true;
    
    // Navigation
    public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
}
