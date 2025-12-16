using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCay.Domain.Entities;

[Table("VaiTro")]
public class VaiTro
{
    [Key]
    public int MaVaiTro { get; set; }
    
    [Required, MaxLength(50)]
    public string TenVaiTro { get; set; } = string.Empty;
    
    [MaxLength(200)]
    public string? MoTa { get; set; }
    
    public DateTime NgayTao { get; set; } = DateTime.Now;
    
    // Navigation
    public virtual ICollection<TaiKhoan> TaiKhoans { get; set; } = new List<TaiKhoan>();
}
