using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyCay.Infrastructure.Data;
using MyCay.Web.Services;

namespace MyCay.Web.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly MyCayDbContext _context;
        private readonly ILogger<OrdersController> _logger;
        private readonly IEmailService _emailService;

        public OrdersController(MyCayDbContext context, ILogger<OrdersController> logger, IEmailService emailService)
        {
            _context = context;
            _logger = logger;
            _emailService = emailService;
        }

        // GET: api/orders
        [HttpGet]
        public async Task<IActionResult> GetOrders(
            [FromQuery] string? status, 
            [FromQuery] int? assignedTo, // Filter theo nhân viên được phân công
            [FromQuery] int? storeId, // Filter theo cửa hàng/chi nhánh
            [FromQuery] bool? unassigned, // Lọc đơn chưa phân công
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 10)
        {
            var query = _context.DonHangs
                .Include(d => d.KhachHang)
                .Include(d => d.NhanVienXuLy)
                .Include(d => d.ChiNhanh)
                .AsQueryable();

            // Filter theo trạng thái
            if (!string.IsNullOrEmpty(status))
                query = query.Where(d => d.TrangThai == status);

            // Filter theo nhân viên được phân công
            if (assignedTo.HasValue)
                query = query.Where(d => d.MaNVXuLy == assignedTo.Value);

            // Filter theo cửa hàng/chi nhánh
            if (storeId.HasValue)
                query = query.Where(d => d.MaCN == storeId.Value);

            // Filter đơn chưa phân công
            if (unassigned == true)
                query = query.Where(d => d.MaNVXuLy == null);

            var total = await query.CountAsync();
            var items = await query.OrderByDescending(d => d.NgayDat)
                .Skip((page - 1) * pageSize).Take(pageSize)
                .Select(d => new OrderDto
                {
                    Id = d.MaDH,
                    Code = d.MaDHCode ?? $"DH{d.MaDH:D6}",
                    CustomerName = d.TenKhach ?? (d.KhachHang != null ? d.KhachHang.HoTen : "Khách vãng lai"),
                    Phone = d.SDTKhach ?? "",
                    Address = d.DiaChiGiao,
                    Subtotal = d.TamTinh,
                    ShippingFee = d.PhiGiaoHang,
                    Discount = d.GiamGia,
                    Total = d.TongTien,
                    PaymentMethod = d.PhuongThucThanhToan ?? "Tiền mặt",
                    PaymentStatus = d.TrangThaiThanhToan ?? "Chưa thanh toán",
                    Note = d.GhiChu,
                    Status = d.TrangThai ?? "Chờ xác nhận",
                    CreatedAt = d.NgayDat ?? DateTime.Now,
                    AssignedToId = d.MaNVXuLy,
                    AssignedToName = d.NhanVienXuLy != null ? d.NhanVienXuLy.HoTen : null,
                    StoreId = d.MaCN,
                    StoreName = d.ChiNhanh != null ? d.ChiNhanh.TenChiNhanh : null,
                    AssignedAt = d.NgayPhanCong
                }).ToListAsync();

            return Ok(new { success = true, data = items, pagination = new { page, pageSize, total } });
        }

        // GET: api/orders/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _context.DonHangs.Include(d => d.KhachHang)
                .Where(d => d.MaDH == id)
                .Select(d => new OrderDto
                {
                    Id = d.MaDH, Code = d.MaDHCode ?? $"DH{d.MaDH:D6}",
                    CustomerName = d.TenKhach ?? (d.KhachHang != null ? d.KhachHang.HoTen : "Khách vãng lai"),
                    Phone = d.SDTKhach ?? "", Address = d.DiaChiGiao,
                    Subtotal = d.TamTinh, ShippingFee = d.PhiGiaoHang, Discount = d.GiamGia, Total = d.TongTien,
                    PaymentMethod = d.PhuongThucThanhToan ?? "Tiền mặt", PaymentStatus = d.TrangThaiThanhToan ?? "Chưa thanh toán",
                    Note = d.GhiChu, Status = d.TrangThai ?? "Chờ xác nhận", CreatedAt = d.NgayDat ?? DateTime.Now
                }).FirstOrDefaultAsync();

            if (order == null)
                return NotFound(new { success = false, message = "Không tìm thấy đơn hàng" });

            // Lấy chi tiết đơn hàng với hình ảnh sản phẩm
            order.Items = await _context.ChiTietDonHangs
                .Where(c => c.MaDH == id)
                .Join(_context.SanPhams, c => c.MaSP, p => p.MaSP, (c, p) => new { c, p })
                .Select(x => new OrderItemDto
                {
                    ProductId = x.c.MaSP, 
                    Name = x.c.TenSP, 
                    Price = (int)x.c.DonGia,
                    Quantity = x.c.SoLuong, 
                    SpicyLevel = x.c.CapDoCay, 
                    BrothType = x.c.LoaiNuocDung,
                    Image = x.p.HinhAnh
                }).ToListAsync();

            // Fallback nếu không join được (sản phẩm đã bị xóa)
            if (order.Items == null || order.Items.Count == 0)
            {
                order.Items = await _context.ChiTietDonHangs.Where(c => c.MaDH == id)
                    .Select(c => new OrderItemDto
                    {
                        ProductId = c.MaSP, Name = c.TenSP, Price = (int)c.DonGia,
                        Quantity = c.SoLuong, SpicyLevel = c.CapDoCay, BrothType = c.LoaiNuocDung
                    }).ToListAsync();
            }

            return Ok(new { success = true, data = order });
        }


        // POST: api/orders
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            if (string.IsNullOrEmpty(request.CustomerName) || string.IsNullOrEmpty(request.Phone))
                return BadRequest(new { success = false, message = "Vui lòng điền đầy đủ thông tin" });

            try
            {
                // Tạo mã đơn hàng duy nhất: DH + timestamp + 4 số ngẫu nhiên
                var random = new Random();
                var randomSuffix = random.Next(1000, 9999);
                var orderCode = $"DH{DateTime.Now:yyyyMMddHHmmss}{randomSuffix}";
                
                // Kiểm tra trùng mã (hiếm khi xảy ra nhưng đảm bảo an toàn)
                while (await _context.DonHangs.AnyAsync(d => d.MaDHCode == orderCode))
                {
                    randomSuffix = random.Next(1000, 9999);
                    orderCode = $"DH{DateTime.Now:yyyyMMddHHmmss}{randomSuffix}";
                }
                
                // Kiểm tra CustomerId có tồn tại trong database không
                int? validCustomerId = null;
                if (request.CustomerId.HasValue && request.CustomerId.Value > 0)
                {
                    var customerExists = await _context.KhachHangs.AnyAsync(k => k.MaKH == request.CustomerId.Value);
                    if (customerExists)
                    {
                        validCustomerId = request.CustomerId.Value;
                    }
                }
                
                var donHang = new MyCay.Domain.Entities.DonHang
                {
                    MaDHCode = orderCode,
                    MaKH = validCustomerId, // Null nếu khách vãng lai hoặc CustomerId không hợp lệ
                    TenKhach = request.CustomerName,
                    SDTKhach = request.Phone,
                    DiaChiGiao = request.Address,
                    TamTinh = request.Subtotal,
                    PhiGiaoHang = request.ShippingFee,
                    GiamGia = request.Discount,
                    TongTien = request.Subtotal + request.ShippingFee - request.Discount,
                    PhuongThucThanhToan = request.PaymentMethod ?? "Tiền mặt",
                    TrangThaiThanhToan = "Chưa thanh toán",
                    TrangThai = "Chờ xác nhận",
                    GhiChu = request.Note,
                    NgayDat = DateTime.Now
                };

                _context.DonHangs.Add(donHang);
                await _context.SaveChangesAsync();

                // Thêm chi tiết đơn hàng
                if (request.Items != null && request.Items.Count > 0)
                {
                    _logger.LogInformation("Saving {Count} items for order {OrderCode}", request.Items.Count, orderCode);
                    foreach (var item in request.Items)
                    {
                        _logger.LogInformation("Item: {Name}, ProductId: {ProductId}, Qty: {Qty}", item.Name, item.ProductId, item.Quantity);
                        var chiTiet = new MyCay.Domain.Entities.ChiTietDonHang
                        {
                            MaDH = donHang.MaDH,
                            MaSP = item.ProductId,
                            TenSP = item.Name ?? "Sản phẩm",
                            SoLuong = item.Quantity,
                            DonGia = item.Price,
                            CapDoCay = item.SpicyLevel,
                            LoaiNuocDung = item.BrothType,
                            GhiChu = item.Note
                        };
                        _context.ChiTietDonHangs.Add(chiTiet);
                    }
                    await _context.SaveChangesAsync();
                }

                _logger.LogInformation("Đơn hàng mới: {OrderCode}", orderCode);
                
                // Gửi email xác nhận đơn hàng (async, không block response)
                if (!string.IsNullOrEmpty(request.Email))
                {
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            // Load lại đơn hàng với chi tiết để gửi email
                            var orderForEmail = await _context.DonHangs
                                .Include(d => d.ChiTietDonHangs)
                                .FirstOrDefaultAsync(d => d.MaDH == donHang.MaDH);
                            
                            if (orderForEmail != null)
                            {
                                await _emailService.SendOrderConfirmationAsync(orderForEmail, request.Email, request.CustomerName);
                            }
                        }
                        catch (Exception emailEx)
                        {
                            _logger.LogError(emailEx, "Failed to send order confirmation email");
                        }
                    });
                }
                
                return Ok(new { success = true, message = "Đặt hàng thành công!", data = new { orderId = donHang.MaDH, orderCode } });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi tạo đơn hàng");
                return StatusCode(500, new { success = false, message = "Lỗi hệ thống" });
            }
        }

        // PUT: api/orders/{id}/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateStatusRequest request)
        {
            var order = await _context.DonHangs.Include(d => d.KhachHang).FirstOrDefaultAsync(d => d.MaDH == id);
            if (order == null)
                return NotFound(new { success = false, message = "Không tìm thấy đơn hàng" });

            var oldStatus = order.TrangThai;
            order.TrangThai = request.Status;
            order.NgayCapNhat = DateTime.Now;
            if (request.Status == "Hoàn thành") order.TrangThaiThanhToan = "Đã thanh toán";
            await _context.SaveChangesAsync();

            // Gửi email thông báo cập nhật trạng thái
            if (oldStatus != request.Status && order.KhachHang?.Email != null)
            {
                _ = _emailService.SendOrderStatusUpdateAsync(order, order.KhachHang.Email, request.Status);
            }

            return Ok(new { success = true, message = "Cập nhật trạng thái thành công" });
        }

        // PUT: api/orders/{id}/assign - Phân công đơn hàng cho nhân viên
        [HttpPut("{id}/assign")]
        public async Task<IActionResult> AssignOrder(int id, [FromBody] AssignOrderRequest request)
        {
            var order = await _context.DonHangs.FindAsync(id);
            if (order == null)
                return NotFound(new { success = false, message = "Không tìm thấy đơn hàng" });

            // Kiểm tra nhân viên tồn tại
            if (request.StaffId.HasValue)
            {
                var staff = await _context.NhanViens.FindAsync(request.StaffId.Value);
                if (staff == null)
                    return BadRequest(new { success = false, message = "Không tìm thấy nhân viên" });
                
                // Nếu có storeId, kiểm tra nhân viên thuộc cửa hàng đó
                if (request.StoreId.HasValue && staff.MaCN != request.StoreId.Value)
                    return BadRequest(new { success = false, message = "Nhân viên không thuộc cửa hàng này" });
            }

            // Cập nhật phân công
            order.MaNVXuLy = request.StaffId;
            order.MaCN = request.StoreId ?? order.MaCN;
            order.NgayPhanCong = request.StaffId.HasValue ? DateTime.Now : null;
            order.NgayCapNhat = DateTime.Now;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Đơn hàng {OrderId} được phân công cho nhân viên {StaffId}", id, request.StaffId);

            return Ok(new { 
                success = true, 
                message = request.StaffId.HasValue 
                    ? "Phân công đơn hàng thành công" 
                    : "Đã hủy phân công đơn hàng"
            });
        }

        // GET: api/orders/staff - Lấy danh sách nhân viên có thể phân công
        [HttpGet("staff")]
        public async Task<IActionResult> GetAvailableStaff([FromQuery] int? storeId)
        {
            var query = _context.NhanViens
                .Where(nv => nv.TrangThai == true)
                .AsQueryable();

            if (storeId.HasValue)
                query = query.Where(nv => nv.MaCN == storeId.Value);

            var staff = await query
                .Select(nv => new
                {
                    id = nv.MaNV,
                    name = nv.HoTen,
                    position = nv.ChucVu,
                    storeId = nv.MaCN,
                    phone = nv.SDT
                })
                .ToListAsync();

            return Ok(new { success = true, data = staff });
        }

        // GET: api/orders/stores - Lấy danh sách cửa hàng/chi nhánh
        [HttpGet("stores")]
        public async Task<IActionResult> GetStores()
        {
            var stores = await _context.ChiNhanhs
                .Select(cn => new
                {
                    id = cn.MaCN,
                    name = cn.TenChiNhanh,
                    address = cn.DiaChi,
                    phone = cn.SoDienThoai
                })
                .ToListAsync();

            return Ok(new { success = true, data = stores });
        }

        // DELETE: api/orders/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var order = await _context.DonHangs.FindAsync(id);
            if (order == null)
                return NotFound(new { success = false, message = "Không tìm thấy đơn hàng" });

            order.TrangThai = "Đã hủy";
            order.NgayCapNhat = DateTime.Now;
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Đã hủy đơn hàng" });
        }

        // GET: api/orders/search?code=DH... (Tra cứu đơn hàng bằng mã)
        [HttpGet("search")]
        public async Task<IActionResult> SearchOrder([FromQuery] string code)
        {
            if (string.IsNullOrEmpty(code))
                return BadRequest(new { success = false, message = "Vui lòng nhập mã đơn hàng" });

            var order = await _context.DonHangs.Include(d => d.KhachHang)
                .Where(d => d.MaDHCode == code || d.MaDH.ToString() == code)
                .Select(d => new OrderDto
                {
                    Id = d.MaDH,
                    Code = d.MaDHCode ?? $"DH{d.MaDH:D6}",
                    CustomerName = d.TenKhach ?? (d.KhachHang != null ? d.KhachHang.HoTen : "Khách vãng lai"),
                    Phone = d.SDTKhach ?? "",
                    Address = d.DiaChiGiao,
                    Subtotal = d.TamTinh,
                    ShippingFee = d.PhiGiaoHang,
                    Discount = d.GiamGia,
                    Total = d.TongTien,
                    PaymentMethod = d.PhuongThucThanhToan ?? "Tiền mặt",
                    PaymentStatus = d.TrangThaiThanhToan ?? "Chưa thanh toán",
                    Note = d.GhiChu,
                    Status = d.TrangThai ?? "Chờ xác nhận",
                    CreatedAt = d.NgayDat ?? DateTime.Now
                }).FirstOrDefaultAsync();

            if (order == null)
                return NotFound(new { success = false, message = "Không tìm thấy đơn hàng" });

            // Lấy chi tiết đơn hàng với hình ảnh
            order.Items = await _context.ChiTietDonHangs
                .Where(c => c.MaDH == order.Id)
                .Join(_context.SanPhams, c => c.MaSP, p => p.MaSP, (c, p) => new { c, p })
                .Select(x => new OrderItemDto
                {
                    ProductId = x.c.MaSP,
                    Name = x.c.TenSP,
                    Price = (int)x.c.DonGia,
                    Quantity = x.c.SoLuong,
                    SpicyLevel = x.c.CapDoCay,
                    BrothType = x.c.LoaiNuocDung,
                    Image = x.p.HinhAnh
                }).ToListAsync();

            // Fallback nếu không join được
            if (order.Items == null || order.Items.Count == 0)
            {
                order.Items = await _context.ChiTietDonHangs.Where(c => c.MaDH == order.Id)
                    .Select(c => new OrderItemDto
                    {
                        ProductId = c.MaSP,
                        Name = c.TenSP,
                        Price = (int)c.DonGia,
                        Quantity = c.SoLuong,
                        SpicyLevel = c.CapDoCay,
                        BrothType = c.LoaiNuocDung
                    }).ToListAsync();
            }

            return Ok(new { success = true, data = order });
        }

        // GET: api/orders/stats
        [HttpGet("stats")]
        public async Task<IActionResult> GetOrderStats()
        {
            var today = DateTime.Today;
            return Ok(new
            {
                success = true,
                data = new
                {
                    pending = await _context.DonHangs.CountAsync(d => d.TrangThai == "Chờ xác nhận"),
                    preparing = await _context.DonHangs.CountAsync(d => d.TrangThai == "Đang chuẩn bị"),
                    shipping = await _context.DonHangs.CountAsync(d => d.TrangThai == "Đang giao"),
                    completed = await _context.DonHangs.CountAsync(d => d.TrangThai == "Hoàn thành"),
                    cancelled = await _context.DonHangs.CountAsync(d => d.TrangThai == "Đã hủy"),
                    totalRevenue = await _context.DonHangs.Where(d => d.TrangThai == "Hoàn thành").SumAsync(d => d.TongTien),
                    todayOrders = await _context.DonHangs.CountAsync(d => d.NgayDat != null && d.NgayDat.Value.Date == today)
                }
            });
        }

        // GET: api/orders/my (Đơn hàng của khách hàng)
        [HttpGet("my")]
        public async Task<IActionResult> GetMyOrders([FromQuery] int customerId)
        {
            var orders = await _context.DonHangs.Where(d => d.MaKH == customerId)
                .OrderByDescending(d => d.NgayDat)
                .Select(d => new OrderDto
                {
                    Id = d.MaDH, Code = d.MaDHCode ?? $"DH{d.MaDH:D6}",
                    CustomerName = d.TenKhach ?? "", Phone = d.SDTKhach ?? "", Address = d.DiaChiGiao,
                    Total = d.TongTien, Status = d.TrangThai ?? "Chờ xác nhận", CreatedAt = d.NgayDat ?? DateTime.Now
                }).ToListAsync();

            return Ok(new { success = true, data = orders });
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
        public string PaymentStatus { get; set; } = "Chưa thanh toán";
        public string? Note { get; set; }
        public string Status { get; set; } = "Chờ xác nhận";
        public List<OrderItemDto>? Items { get; set; }
        public DateTime CreatedAt { get; set; }
        
        // Thông tin phân công
        public int? AssignedToId { get; set; }
        public string? AssignedToName { get; set; }
        public int? StoreId { get; set; }
        public string? StoreName { get; set; }
        public DateTime? AssignedAt { get; set; }
    }

    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = "";
        public int Price { get; set; }
        public int Quantity { get; set; }
        public int SpicyLevel { get; set; }
        public string? BrothType { get; set; }
        public string? Note { get; set; }
        public string? Image { get; set; }
    }

    public class CreateOrderRequest
    {
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; } = "";
        public string Phone { get; set; } = "";
        public string? Email { get; set; }
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

    public class AssignOrderRequest
    {
        public int? StaffId { get; set; } // Null để hủy phân công
        public int? StoreId { get; set; } // Cửa hàng/chi nhánh
    }
}
