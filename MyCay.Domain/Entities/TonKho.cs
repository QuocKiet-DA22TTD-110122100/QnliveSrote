using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCay.Domain.Entities;

[Table("TonKho")]
public class TonKho
{
    [Key]
    public int MaTK { get; set; }
    
    public int MaCN { get; set; }
    
    public int MaNVL { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal SoLuong { get; set; }
    
    public DateTime NgayCapNhat { get; set; } = DateTime.Now;
    
    // Navigation
    [ForeignKey("MaCN")]
    public virtual ChiNhanh? ChiNhanh { get; set; }
    
    [ForeignKey("MaNVL")]
    public virtual NguyenVatLieu? NguyenVatLieu { get; set; }
}
