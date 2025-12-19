using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyCay.Infrastructure.Data;

namespace MyCay.Web.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly MyCayDbContext _context;
        private readonly ILogger<CartController> _logger;

        public CartController(MyCayDbContext context, ILogger<CartController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/cart - Lấy giỏ hàng của khách
        [HttpGet]
        public async Task<IActionResult> GetCart([FromQuery] int? customerId, [FromQuery] string? sessionId)
        {
            var query = _context.GioHangs.Include(g => g.SanPham).AsQueryable();

            if (customerId.HasValue)
                query = query.Where(g => g.MaKH == customerId.Value);
            else if (!string.IsNullOrEmpty(sessionId))
                query = query.Where(g => g.SessionID == sessionId);
            else
                return Ok(new { success = true, data = new List<object>() });

            var items = await query.Select(g => new
            {
                id = g.MaGH,
                productId = g.MaSP,
                name = g.SanPham != null ? g.SanPham.TenSP : "",
                price = g.SanPham != null ? (int)g.SanPham.DonGia : 0,
                image = g.SanPham != null ? g.SanPham.HinhAnh : null,
                quantity = g.SoLuong,
                spicyLevel = g.CapDoCay,
                brothType = g.LoaiNuocDung,
                note = g.GhiChu
            }).ToListAsync();

            return Ok(new { success = true, data = items });
        }

        // POST: api/cart - Thêm vào giỏ hàng
        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            try
            {
                // Kiểm tra sản phẩm tồn tại
                var sanPham = await _context.SanPhams.FindAsync(request.ProductId);
                if (sanPham == null)
                    return NotFound(new { success = false, message = "Sản phẩm không tồn tại" });

                // Kiểm tra đã có trong giỏ chưa
                var existing = await _context.GioHangs.FirstOrDefaultAsync(g =>
                    g.MaSP == request.ProductId &&
                    ((request.CustomerId.HasValue && g.MaKH == request.CustomerId) ||
                     (!string.IsNullOrEmpty(request.SessionId) && g.SessionID == request.SessionId)) &&
                    g.CapDoCay == request.SpicyLevel &&
                    g.LoaiNuocDung == request.BrothType);

                if (existing != null)
                {
                    existing.SoLuong += request.Quantity;
                }
                else
                {
                    var gioHang = new MyCay.Domain.Entities.GioHang
                    {
                        MaKH = request.CustomerId,
                        SessionID = request.SessionId,
                        MaSP = request.ProductId,
                        SoLuong = request.Quantity,
                        CapDoCay = request.SpicyLevel,
                        LoaiNuocDung = request.BrothType,
                        GhiChu = request.Note,
                        NgayThem = DateTime.Now
                    };
                    _context.GioHangs.Add(gioHang);
                }

                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Đã thêm vào giỏ hàng" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi thêm giỏ hàng");
                return StatusCode(500, new { success = false, message = "Lỗi hệ thống" });
            }
        }


        // PUT: api/cart/{id} - Cập nhật số lượng
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCartItem(int id, [FromBody] UpdateCartRequest request)
        {
            var item = await _context.GioHangs.FindAsync(id);
            if (item == null)
                return NotFound(new { success = false, message = "Không tìm thấy sản phẩm trong giỏ" });

            if (request.Quantity <= 0)
            {
                _context.GioHangs.Remove(item);
            }
            else
            {
                item.SoLuong = request.Quantity;
            }

            await _context.SaveChangesAsync();
            return Ok(new { success = true, message = "Đã cập nhật giỏ hàng" });
        }

        // DELETE: api/cart/{id} - Xóa khỏi giỏ hàng
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var item = await _context.GioHangs.FindAsync(id);
            if (item == null)
                return NotFound(new { success = false, message = "Không tìm thấy sản phẩm trong giỏ" });

            _context.GioHangs.Remove(item);
            await _context.SaveChangesAsync();
            return Ok(new { success = true, message = "Đã xóa khỏi giỏ hàng" });
        }

        // DELETE: api/cart/clear - Xóa toàn bộ giỏ hàng
        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart([FromQuery] int? customerId, [FromQuery] string? sessionId)
        {
            var query = _context.GioHangs.AsQueryable();

            if (customerId.HasValue)
                query = query.Where(g => g.MaKH == customerId.Value);
            else if (!string.IsNullOrEmpty(sessionId))
                query = query.Where(g => g.SessionID == sessionId);
            else
                return BadRequest(new { success = false, message = "Thiếu thông tin khách hàng" });

            _context.GioHangs.RemoveRange(await query.ToListAsync());
            await _context.SaveChangesAsync();
            return Ok(new { success = true, message = "Đã xóa giỏ hàng" });
        }

        // POST: api/cart/apply-promo - Áp dụng mã giảm giá từ database
        [HttpPost("apply-promo")]
        public async Task<IActionResult> ApplyPromoCode([FromBody] ApplyPromoRequest request)
        {
            if (string.IsNullOrEmpty(request.Code))
                return BadRequest(new { success = false, message = "Vui lòng nhập mã giảm giá" });

            var code = request.Code.ToUpper();
            var coupon = await _context.MaGiamGias
                .FirstOrDefaultAsync(m => m.MaCode.ToUpper() == code);

            if (coupon == null)
                return BadRequest(new { success = false, message = "Mã giảm giá không hợp lệ" });

            // Kiểm tra trạng thái
            if (!coupon.TrangThai)
                return BadRequest(new { success = false, message = "Mã giảm giá đã bị vô hiệu hóa" });

            // Kiểm tra thời gian
            var now = DateTime.Now;
            if (coupon.NgayBatDau.HasValue && coupon.NgayBatDau > now)
                return BadRequest(new { success = false, message = "Mã giảm giá chưa có hiệu lực" });

            if (coupon.NgayKetThuc.HasValue && coupon.NgayKetThuc < now)
                return BadRequest(new { success = false, message = "Mã giảm giá đã hết hạn" });

            // Kiểm tra số lượng
            if (coupon.DaSuDung >= coupon.SoLuong)
                return BadRequest(new { success = false, message = "Mã giảm giá đã hết lượt sử dụng" });

            // Kiểm tra đơn tối thiểu
            if (request.Subtotal < coupon.DonToiThieu)
                return BadRequest(new { success = false, message = $"Đơn hàng tối thiểu {coupon.DonToiThieu:N0}đ để áp dụng mã này" });

            decimal discount = 0;
            decimal shippingDiscount = 0;

            switch (coupon.LoaiGiam.ToLower())
            {
                case "percent":
                    discount = request.Subtotal * coupon.GiaTri / 100;
                    if (coupon.GiamToiDa.HasValue && discount > coupon.GiamToiDa)
                        discount = coupon.GiamToiDa.Value;
                    break;
                case "fixed":
                    discount = coupon.GiaTri;
                    break;
                case "freeship":
                    shippingDiscount = coupon.GiaTri;
                    break;
            }

            return Ok(new
            {
                success = true,
                message = $"Áp dụng mã {code} thành công!",
                data = new
                {
                    couponId = coupon.MaMGG,
                    code = coupon.MaCode,
                    description = coupon.MoTa,
                    discount,
                    shippingDiscount,
                    type = coupon.LoaiGiam
                }
            });
        }

        // GET: api/cart/promo-codes - Lấy danh sách mã giảm giá đang hoạt động
        [HttpGet("promo-codes")]
        public async Task<IActionResult> GetPromoCodes()
        {
            var now = DateTime.Now;
            var codes = await _context.MaGiamGias
                .Where(m => m.TrangThai
                    && (m.NgayBatDau == null || m.NgayBatDau <= now)
                    && (m.NgayKetThuc == null || m.NgayKetThuc >= now)
                    && m.DaSuDung < m.SoLuong)
                .Select(m => new
                {
                    code = m.MaCode,
                    description = m.MoTa,
                    type = m.LoaiGiam,
                    value = m.GiaTri,
                    maxDiscount = m.GiamToiDa,
                    minOrder = m.DonToiThieu
                })
                .ToListAsync();

            return Ok(new { success = true, data = codes });
        }

        // POST: api/cart/calculate
        [HttpPost("calculate")]
        public IActionResult CalculateCart([FromBody] CalculateCartRequest request)
        {
            decimal subtotal = request.Items?.Sum(i => i.Price * i.Quantity) ?? 0;
            decimal shippingFee = subtotal >= 100000 ? 0 : 15000;
            decimal discount = request.Discount;
            decimal total = subtotal + shippingFee - discount;

            return Ok(new
            {
                success = true,
                data = new { subtotal, shippingFee, discount, total = Math.Max(0, total), itemCount = request.Items?.Sum(i => i.Quantity) ?? 0 }
            });
        }
    }

    public class AddToCartRequest
    {
        public int? CustomerId { get; set; }
        public string? SessionId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1;
        public int SpicyLevel { get; set; }
        public string? BrothType { get; set; }
        public string? Note { get; set; }
    }

    public class UpdateCartRequest
    {
        public int Quantity { get; set; }
    }

    public class ApplyPromoRequest
    {
        public string Code { get; set; } = "";
        public decimal Subtotal { get; set; }
    }

    public class CalculateCartRequest
    {
        public List<CartItemDto>? Items { get; set; }
        public decimal Discount { get; set; }
    }

    public class CartItemDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = "";
        public int Price { get; set; }
        public int Quantity { get; set; }
    }
}
