using Microsoft.AspNetCore.Mvc;

namespace MyCay.Web.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private static List<OrderDto> _orders = new()
        {
            new OrderDto { Id = 1, Code = "DH20241216001", CustomerName = "Nguyễn Thị Mai", Phone = "0988888881", Address = "123 Lê Lợi, Quận 1", Total = 146000, Status = "Hoàn thành", CreatedAt = DateTime.Now.AddHours(-2) },
            new OrderDto { Id = 2, Code = "DH20241216002", CustomerName = "Trần Văn Hùng", Phone = "0988888882", Address = "456 Hai Bà Trưng, Quận 3", Total = 253000, Status = "Đang giao", CreatedAt = DateTime.Now.AddHours(-1) },
            new OrderDto { Id = 3, Code = "DH20241216003", CustomerName = "Khách vãng lai", Phone = "0999999999", Address = "789 CMT8, Quận 10", Total = 74000, Status = "Chờ xác nhận", CreatedAt = DateTime.Now }
        };

        // GET: api/orders
        [HttpGet]
        public IActionResult GetOrders([FromQuery] string? status, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var orders = _orders.AsQueryable();

            if (!string.IsNullOrEmpty(status))
                orders = orders.Where(o => o.Status == status);

            var total = orders.Count();
            var items = orders.OrderByDescending(o => o.CreatedAt)
                              .Skip((page - 1) * pageSize)
                              .Take(pageSize)
                              .ToList();

            return Ok(new
            {
                success = true,
                data = items,
                pagination = new { page, pageSize, total }
            });
        }

        // GET: api/orders/{id}
        [HttpGet("{id}")]
        public IActionResult GetOrder(int id)
        {
            var order = _orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
                return NotFound(new { success = false, message = "Không tìm thấy đơn hàng" });

            return Ok(new { success = true, data = order });
        }

        // POST: api/orders
        [HttpPost]
        public IActionResult CreateOrder([FromBody] CreateOrderRequest request)
        {
            if (string.IsNullOrEmpty(request.CustomerName) || string.IsNullOrEmpty(request.Phone))
                return BadRequest(new { success = false, message = "Vui lòng điền đầy đủ thông tin" });

            var newOrder = new OrderDto
            {
                Id = _orders.Count + 1,
                Code = $"DH{DateTime.Now:yyyyMMdd}{(_orders.Count + 1):D3}",
                CustomerName = request.CustomerName,
                Phone = request.Phone,
                Address = request.Address,
                Subtotal = request.Subtotal,
                ShippingFee = request.ShippingFee,
                Discount = request.Discount,
                Total = request.Subtotal + request.ShippingFee - request.Discount,
                PaymentMethod = request.PaymentMethod ?? "Tiền mặt",
                Note = request.Note,
                Status = "Chờ xác nhận",
                Items = request.Items,
                CreatedAt = DateTime.Now
            };

            _orders.Add(newOrder);

            return Ok(new
            {
                success = true,
                message = "Đặt hàng thành công!",
                data = new { orderId = newOrder.Id, orderCode = newOrder.Code }
            });
        }

        // PUT: api/orders/{id}/status
        [HttpPut("{id}/status")]
        public IActionResult UpdateOrderStatus(int id, [FromBody] UpdateStatusRequest request)
        {
            var order = _orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
                return NotFound(new { success = false, message = "Không tìm thấy đơn hàng" });

            order.Status = request.Status;
            return Ok(new { success = true, message = "Cập nhật trạng thái thành công" });
        }

        // DELETE: api/orders/{id}
        [HttpDelete("{id}")]
        public IActionResult CancelOrder(int id)
        {
            var order = _orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
                return NotFound(new { success = false, message = "Không tìm thấy đơn hàng" });

            order.Status = "Đã hủy";
            return Ok(new { success = true, message = "Đã hủy đơn hàng" });
        }

        // GET: api/orders/stats
        [HttpGet("stats")]
        public IActionResult GetOrderStats()
        {
            return Ok(new
            {
                success = true,
                data = new
                {
                    pending = _orders.Count(o => o.Status == "Chờ xác nhận"),
                    preparing = _orders.Count(o => o.Status == "Đang chuẩn bị"),
                    shipping = _orders.Count(o => o.Status == "Đang giao"),
                    completed = _orders.Count(o => o.Status == "Hoàn thành"),
                    cancelled = _orders.Count(o => o.Status == "Đã hủy"),
                    totalRevenue = _orders.Where(o => o.Status == "Hoàn thành").Sum(o => o.Total),
                    todayOrders = _orders.Count(o => o.CreatedAt.Date == DateTime.Today)
                }
            });
        }
    }

    public class OrderDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = "";
        public string CustomerName { get; set; } = "";
        public string Phone { get; set; } = "";
        public string? Address { get; set; }
        public decimal Subtotal { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public string PaymentMethod { get; set; } = "Tiền mặt";
        public string? Note { get; set; }
        public string Status { get; set; } = "Chờ xác nhận";
        public List<OrderItemDto>? Items { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = "";
        public int Price { get; set; }
        public int Quantity { get; set; }
        public int SpicyLevel { get; set; }
    }

    public class CreateOrderRequest
    {
        public string CustomerName { get; set; } = "";
        public string Phone { get; set; } = "";
        public string? Address { get; set; }
        public decimal Subtotal { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal Discount { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Note { get; set; }
        public List<OrderItemDto>? Items { get; set; }
    }

    public class UpdateStatusRequest
    {
        public string Status { get; set; } = "";
    }
}
