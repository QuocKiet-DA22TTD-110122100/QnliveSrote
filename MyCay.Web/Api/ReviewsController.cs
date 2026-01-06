using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyCay.Infrastructure.Data;
using MyCay.Domain.Entities;

namespace MyCay.Web.Api;

[ApiController]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly MyCayDbContext _context;

    public ReviewsController(MyCayDbContext context)
    {
        _context = context;
    }

    // GET: api/reviews - Lấy danh sách đánh giá công khai
    [HttpGet]
    public async Task<IActionResult> GetReviews([FromQuery] int? productId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var query = _context.DanhGias
            .Where(d => d.HienThi && d.DaDuyet)
            .AsQueryable();

        if (productId.HasValue)
            query = query.Where(d => d.MaSP == productId);

        var total = await query.CountAsync();
        var reviews = await query
            .OrderByDescending(d => d.NgayDanhGia)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(d => new ReviewDto
            {
                Id = d.MaDG,
                CustomerName = d.TenKhach,
                Rating = d.SoSao,
                Content = d.NoiDung,
                Image = d.HinhAnh,
                CreatedAt = d.NgayDanhGia,
                ProductId = d.MaSP,
                ProductName = d.SanPham != null ? d.SanPham.TenSP : null,
                AdminReply = d.PhanHoi,
                ReplyAt = d.NgayPhanHoi
            })
            .ToListAsync();

        // Tính thống kê
        var stats = await _context.DanhGias
            .Where(d => d.HienThi && d.DaDuyet)
            .GroupBy(d => 1)
            .Select(g => new
            {
                Total = g.Count(),
                Average = g.Average(d => d.SoSao),
                Star5 = g.Count(d => d.SoSao == 5),
                Star4 = g.Count(d => d.SoSao == 4),
                Star3 = g.Count(d => d.SoSao == 3),
                Star2 = g.Count(d => d.SoSao == 2),
                Star1 = g.Count(d => d.SoSao == 1)
            })
            .FirstOrDefaultAsync();

        return Ok(new
        {
            success = true,
            data = reviews,
            stats = stats ?? new { Total = 0, Average = 0.0, Star5 = 0, Star4 = 0, Star3 = 0, Star2 = 0, Star1 = 0 },
            pagination = new { page, pageSize, total }
        });
    }

    // POST: api/reviews - Khách hàng gửi đánh giá
    [HttpPost]
    public async Task<IActionResult> CreateReview([FromBody] CreateReviewRequest request)
    {
        if (string.IsNullOrEmpty(request.CustomerName) || string.IsNullOrEmpty(request.Content))
            return BadRequest(new { success = false, message = "Vui lòng điền đầy đủ thông tin" });

        if (request.Rating < 1 || request.Rating > 5)
            return BadRequest(new { success = false, message = "Số sao phải từ 1-5" });

        var review = new DanhGia
        {
            MaKH = request.CustomerId,
            MaSP = request.ProductId,
            MaDH = request.OrderId,
            TenKhach = request.CustomerName,
            SDT = request.Phone,
            Email = request.Email,
            SoSao = request.Rating,
            NoiDung = request.Content,
            HinhAnh = request.Image,
            NgayDanhGia = DateTime.Now,
            DaDuyet = false, // Chờ admin duyệt
            HienThi = true
        };

        _context.DanhGias.Add(review);
        await _context.SaveChangesAsync();

        return Ok(new { success = true, message = "Cảm ơn bạn đã đánh giá! Đánh giá sẽ hiển thị sau khi được duyệt." });
    }

    // GET: api/reviews/admin - Admin xem tất cả đánh giá
    [HttpGet("admin")]
    public async Task<IActionResult> GetAllReviews([FromQuery] string? status, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var query = _context.DanhGias.AsQueryable();

        if (status == "pending")
            query = query.Where(d => !d.DaDuyet);
        else if (status == "approved")
            query = query.Where(d => d.DaDuyet);
        else if (status == "replied")
            query = query.Where(d => d.PhanHoi != null);
        else if (status == "unreplied")
            query = query.Where(d => d.PhanHoi == null);

        var total = await query.CountAsync();
        var reviews = await query
            .OrderByDescending(d => d.NgayDanhGia)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(d => new
            {
                d.MaDG,
                d.TenKhach,
                d.SDT,
                d.Email,
                d.SoSao,
                d.NoiDung,
                d.HinhAnh,
                d.NgayDanhGia,
                d.PhanHoi,
                d.NgayPhanHoi,
                d.DaXem,
                d.DaDuyet,
                d.HienThi,
                ProductName = d.SanPham != null ? d.SanPham.TenSP : null,
                OrderCode = d.DonHang != null ? d.DonHang.MaDHCode : null
            })
            .ToListAsync();

        // Thống kê cho admin
        var stats = new
        {
            Total = await _context.DanhGias.CountAsync(),
            Pending = await _context.DanhGias.CountAsync(d => !d.DaDuyet),
            Unreplied = await _context.DanhGias.CountAsync(d => d.PhanHoi == null && d.DaDuyet),
            Today = await _context.DanhGias.CountAsync(d => d.NgayDanhGia.Date == DateTime.Today)
        };

        return Ok(new { success = true, data = reviews, stats, pagination = new { page, pageSize, total } });
    }

    // PUT: api/reviews/{id}/approve - Duyệt đánh giá
    [HttpPut("{id}/approve")]
    public async Task<IActionResult> ApproveReview(int id)
    {
        var review = await _context.DanhGias.FindAsync(id);
        if (review == null)
            return NotFound(new { success = false, message = "Không tìm thấy đánh giá" });

        review.DaDuyet = true;
        review.DaXem = true;
        await _context.SaveChangesAsync();

        return Ok(new { success = true, message = "Đã duyệt đánh giá" });
    }

    // PUT: api/reviews/{id}/reply - Admin phản hồi
    [HttpPut("{id}/reply")]
    public async Task<IActionResult> ReplyReview(int id, [FromBody] ReplyRequest request)
    {
        var review = await _context.DanhGias.FindAsync(id);
        if (review == null)
            return NotFound(new { success = false, message = "Không tìm thấy đánh giá" });

        review.PhanHoi = request.Reply;
        review.NgayPhanHoi = DateTime.Now;
        review.MaNVPhanHoi = request.StaffId;
        review.DaXem = true;
        if (!review.DaDuyet) review.DaDuyet = true; // Tự động duyệt khi phản hồi

        await _context.SaveChangesAsync();

        return Ok(new { success = true, message = "Đã phản hồi đánh giá" });
    }

    // PUT: api/reviews/{id}/toggle - Ẩn/hiện đánh giá
    [HttpPut("{id}/toggle")]
    public async Task<IActionResult> ToggleReview(int id)
    {
        var review = await _context.DanhGias.FindAsync(id);
        if (review == null)
            return NotFound(new { success = false, message = "Không tìm thấy đánh giá" });

        review.HienThi = !review.HienThi;
        await _context.SaveChangesAsync();

        return Ok(new { success = true, message = review.HienThi ? "Đã hiện đánh giá" : "Đã ẩn đánh giá" });
    }

    // DELETE: api/reviews/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReview(int id)
    {
        var review = await _context.DanhGias.FindAsync(id);
        if (review == null)
            return NotFound(new { success = false, message = "Không tìm thấy đánh giá" });

        _context.DanhGias.Remove(review);
        await _context.SaveChangesAsync();

        return Ok(new { success = true, message = "Đã xóa đánh giá" });
    }
}

public class ReviewDto
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = "";
    public int Rating { get; set; }
    public string Content { get; set; } = "";
    public string? Image { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? ProductId { get; set; }
    public string? ProductName { get; set; }
    public string? AdminReply { get; set; }
    public DateTime? ReplyAt { get; set; }
}

public class CreateReviewRequest
{
    public int? CustomerId { get; set; }
    public int? ProductId { get; set; }
    public int? OrderId { get; set; }
    public string CustomerName { get; set; } = "";
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public int Rating { get; set; } = 5;
    public string Content { get; set; } = "";
    public string? Image { get; set; }
}

public class ReplyRequest
{
    public string Reply { get; set; } = "";
    public int? StaffId { get; set; }
}
