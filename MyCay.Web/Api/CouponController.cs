using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyCay.Infrastructure.Data;
using MyCay.Domain.Entities;

namespace MyCay.Web.Api;

[ApiController]
[Route("api/[controller]")]
public class CouponController : ControllerBase
{
    private readonly MyCayDbContext _context;

    public CouponController(MyCayDbContext context)
    {
        _context = context;
    }

    // GET: api/coupon
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var coupons = await _context.MaGiamGias
            .OrderByDescending(m => m.NgayTao)
            .Select(m => new
            {
                m.MaMGG,
                m.MaCode,
                m.MoTa,
                m.LoaiGiam,
                m.GiaTri,
                m.GiamToiDa,
                m.DonToiThieu,
                m.SoLuong,
                m.DaSuDung,
                ConLai = m.SoLuong - m.DaSuDung,
                m.NgayBatDau,
                m.NgayKetThuc,
                m.TrangThai,
                HetHan = m.NgayKetThuc < DateTime.Now
            })
            .ToListAsync();

        return Ok(coupons);
    }

    // GET: api/coupon/active - Mã đang hoạt động
    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        var now = DateTime.Now;
        var coupons = await _context.MaGiamGias
            .Where(m => m.TrangThai 
                && (m.NgayBatDau == null || m.NgayBatDau <= now)
                && (m.NgayKetThuc == null || m.NgayKetThuc >= now)
                && m.DaSuDung < m.SoLuong)
            .Select(m => new
            {
                m.MaCode,
                m.MoTa,
                m.LoaiGiam,
                m.GiaTri,
                m.GiamToiDa,
                m.DonToiThieu
            })
            .ToListAsync();

        return Ok(coupons);
    }

    // POST: api/coupon/validate - Kiểm tra mã giảm giá
    [HttpPost("validate")]
    public async Task<IActionResult> ValidateCoupon([FromBody] ValidateCouponRequest request)
    {
        var coupon = await _context.MaGiamGias
            .FirstOrDefaultAsync(m => m.MaCode.ToUpper() == request.Code.ToUpper());

        if (coupon == null)
            return BadRequest(new { valid = false, message = "Mã giảm giá không tồn tại" });

        // Kiểm tra trạng thái
        if (!coupon.TrangThai)
            return BadRequest(new { valid = false, message = "Mã giảm giá đã bị vô hiệu hóa" });

        // Kiểm tra thời gian
        var now = DateTime.Now;
        if (coupon.NgayBatDau.HasValue && coupon.NgayBatDau > now)
            return BadRequest(new { valid = false, message = "Mã giảm giá chưa có hiệu lực" });

        if (coupon.NgayKetThuc.HasValue && coupon.NgayKetThuc < now)
            return BadRequest(new { valid = false, message = "Mã giảm giá đã hết hạn" });

        // Kiểm tra số lượng
        if (coupon.DaSuDung >= coupon.SoLuong)
            return BadRequest(new { valid = false, message = "Mã giảm giá đã hết lượt sử dụng" });

        // Kiểm tra đơn tối thiểu
        if (request.OrderTotal < coupon.DonToiThieu)
            return BadRequest(new { 
                valid = false, 
                message = $"Đơn hàng tối thiểu {coupon.DonToiThieu:N0}đ để áp dụng mã này" 
            });

        // Tính số tiền giảm
        decimal discount = 0;
        string discountText = "";

        switch (coupon.LoaiGiam.ToLower())
        {
            case "percent":
                discount = request.OrderTotal * coupon.GiaTri / 100;
                if (coupon.GiamToiDa.HasValue && discount > coupon.GiamToiDa)
                    discount = coupon.GiamToiDa.Value;
                discountText = $"Giảm {coupon.GiaTri}%";
                break;

            case "fixed":
                discount = coupon.GiaTri;
                discountText = $"Giảm {coupon.GiaTri:N0}đ";
                break;

            case "freeship":
                discount = coupon.GiaTri; // Giá trị = phí ship tối đa được miễn
                discountText = "Miễn phí giao hàng";
                break;
        }

        return Ok(new
        {
            valid = true,
            couponId = coupon.MaMGG,
            code = coupon.MaCode,
            loaiGiam = coupon.LoaiGiam,
            discount = discount,
            discountText = discountText,
            message = $"Áp dụng thành công: {discountText}"
        });
    }

    // POST: api/coupon/use - Sử dụng mã (khi đặt hàng thành công)
    [HttpPost("use")]
    public async Task<IActionResult> UseCoupon([FromBody] UseCouponRequest request)
    {
        var coupon = await _context.MaGiamGias
            .FirstOrDefaultAsync(m => m.MaMGG == request.CouponId);

        if (coupon == null)
            return NotFound(new { message = "Không tìm thấy mã giảm giá" });

        coupon.DaSuDung++;
        await _context.SaveChangesAsync();

        return Ok(new { message = "Đã cập nhật số lượt sử dụng" });
    }

    // POST: api/coupon
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MaGiamGia coupon)
    {
        // Kiểm tra mã đã tồn tại
        var exists = await _context.MaGiamGias
            .AnyAsync(m => m.MaCode.ToUpper() == coupon.MaCode.ToUpper());

        if (exists)
            return BadRequest(new { message = "Mã giảm giá đã tồn tại" });

        coupon.MaCode = coupon.MaCode.ToUpper();
        _context.MaGiamGias.Add(coupon);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Tạo mã giảm giá thành công", id = coupon.MaMGG });
    }

    // PUT: api/coupon/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] MaGiamGia coupon)
    {
        var existing = await _context.MaGiamGias.FindAsync(id);
        if (existing == null)
            return NotFound(new { message = "Không tìm thấy mã giảm giá" });

        existing.MoTa = coupon.MoTa;
        existing.LoaiGiam = coupon.LoaiGiam;
        existing.GiaTri = coupon.GiaTri;
        existing.GiamToiDa = coupon.GiamToiDa;
        existing.DonToiThieu = coupon.DonToiThieu;
        existing.SoLuong = coupon.SoLuong;
        existing.NgayBatDau = coupon.NgayBatDau;
        existing.NgayKetThuc = coupon.NgayKetThuc;
        existing.TrangThai = coupon.TrangThai;

        await _context.SaveChangesAsync();
        return Ok(new { message = "Cập nhật thành công" });
    }

    // DELETE: api/coupon/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var coupon = await _context.MaGiamGias.FindAsync(id);
        if (coupon == null)
            return NotFound(new { message = "Không tìm thấy mã giảm giá" });

        coupon.TrangThai = false;
        await _context.SaveChangesAsync();
        return Ok(new { message = "Đã vô hiệu hóa mã giảm giá" });
    }
}

public class ValidateCouponRequest
{
    public string Code { get; set; } = "";
    public decimal OrderTotal { get; set; }
}

public class UseCouponRequest
{
    public int CouponId { get; set; }
}
