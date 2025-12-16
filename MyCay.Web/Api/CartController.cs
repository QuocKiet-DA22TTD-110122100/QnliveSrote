using Microsoft.AspNetCore.Mvc;

namespace MyCay.Web.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        // Promo codes
        private static readonly Dictionary<string, PromoCode> _promoCodes = new()
        {
            ["SASIN10"] = new PromoCode { Code = "SASIN10", Type = "percent", Value = 10, MinOrder = 100000, Description = "Giảm 10% đơn từ 100k" },
            ["SASIN20"] = new PromoCode { Code = "SASIN20", Type = "percent", Value = 20, MinOrder = 200000, Description = "Giảm 20% đơn từ 200k" },
            ["FREESHIP"] = new PromoCode { Code = "FREESHIP", Type = "shipping", Value = 15000, MinOrder = 150000, Description = "Miễn phí ship đơn từ 150k" },
            ["NEWUSER"] = new PromoCode { Code = "NEWUSER", Type = "fixed", Value = 30000, MinOrder = 100000, Description = "Giảm 30k cho khách mới" }
        };

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
                case "percent":
                    discount = request.Subtotal * promo.Value / 100;
                    break;
                case "fixed":
                    discount = promo.Value;
                    break;
                case "shipping":
                    shippingDiscount = promo.Value;
                    break;
            }

            return Ok(new
            {
                success = true,
                message = $"Áp dụng mã {code} thành công!",
                data = new
                {
                    code = promo.Code,
                    description = promo.Description,
                    discount,
                    shippingDiscount,
                    type = promo.Type
                }
            });
        }

        // GET: api/cart/promo-codes
        [HttpGet("promo-codes")]
        public IActionResult GetPromoCodes()
        {
            var codes = _promoCodes.Values.Select(p => new
            {
                code = p.Code,
                description = p.Description,
                type = p.Type,
                value = p.Value,
                minOrder = p.MinOrder
            });

            return Ok(new { success = true, data = codes });
        }

        // POST: api/cart/calculate
        [HttpPost("calculate")]
        public IActionResult CalculateCart([FromBody] CalculateCartRequest request)
        {
            decimal subtotal = request.Items?.Sum(i => i.Price * i.Quantity) ?? 0;
            decimal shippingFee = subtotal >= 100000 ? 0 : 15000; // Free ship từ 100k
            decimal discount = request.Discount;
            decimal total = subtotal + shippingFee - discount;

            return Ok(new
            {
                success = true,
                data = new
                {
                    subtotal,
                    shippingFee,
                    discount,
                    total = Math.Max(0, total),
                    itemCount = request.Items?.Sum(i => i.Quantity) ?? 0
                }
            });
        }
    }

    public class PromoCode
    {
        public string Code { get; set; } = "";
        public string Type { get; set; } = "percent"; // percent, fixed, shipping
        public decimal Value { get; set; }
        public decimal MinOrder { get; set; }
        public string Description { get; set; } = "";
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
