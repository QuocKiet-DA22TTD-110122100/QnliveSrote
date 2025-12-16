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

        // Promo codes (có thể chuyển vào database sau)
        private static readonly Dictionary<string, PromoCode> _promoCodes = new()
        {
            ["SASIN10"] = new PromoCode { Code = "SASIN10", Type = "percent", Value = 10, MinOrder = 100000, Description = "Giảm 10% đơn từ 100k" },
            ["SASIN20"] = new PromoCode { Code = "SASIN20", Type = "percent", Value = 20, MinOrder = 200000, Description = "Giảm 20% đơn từ 200k" },
            ["FREESHIP"] = new PromoCode { Code = "FREESHIP", Type = "shipping", Value = 15000, MinOrder = 150000, Description = "Miễn phí ship đơn từ 150k" },
            ["NEWUSER"] = new PromoCode { Code = "NEWUSER", Type = "fixed", Value = 30000, MinOrder = 100000, Description = "Giảm 30k cho khách mới" }
        };

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

        // POST: api/cart/apply-promo
        [HttpPost("apply-promo")]
        public IActionResult ApplyPromoCode([FromBody] ApplyPromoRequest request)
        {
            if (string.IsNullOrEmpty(request.Code))
                return BadRequest(new { success = false, message = "Vui lòng nhập mã giảm giá" });

            var code = request.Code.ToUpper();
            if (!_promoCodes.TryGetValue(code, out var promo))
                return BadRequest(new { success = false, message = "Mã giảm giá không hợp lệ" });

            if (request.Subtotal < promo.MinOrder)
                return BadRequest(new { success = false, message = $"Đơn hàng tối thiểu {promo.MinOrder:N0}đ để áp dụng mã này" });

            decimal discount = 0;
            decimal shippingDiscount = 0;

            switch (promo.Type)
            {
                case "percent": discount = request.Subtotal * promo.Value / 100; break;
                case "fixed": discount = promo.Value; break;
                case "shipping": shippingDiscount = promo.Value; break;
            }

            return Ok(new
            {
                success = true,
                message = $"Áp dụng mã {code} thành công!",
                data = new { code = promo.Code, description = promo.Description, discount, shippingDiscount, type = promo.Type }
            });
        }

        // GET: api/cart/promo-codes
        [HttpGet("promo-codes")]
        public IActionResult GetPromoCodes()
        {
            var codes = _promoCodes.Values.Select(p => new
            {
                code = p.Code, description = p.Description, type = p.Type, value = p.Value, minOrder = p.MinOrder
            });
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

    public class PromoCode
    {
        public string Code { get; set; } = "";
        public string Type { get; set; } = "percent";
        public decimal Value { get; set; }
        public decimal MinOrder { get; set; }
        public string Description { get; set; } = "";
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
