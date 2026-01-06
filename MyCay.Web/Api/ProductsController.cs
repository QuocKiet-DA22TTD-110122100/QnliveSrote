using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyCay.Infrastructure.Data;

namespace MyCay.Web.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly MyCayDbContext _context;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(MyCayDbContext context, ILogger<ProductsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/products
        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] int? categoryId, [FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] bool includeInactive = false)
        {
            try
            {
                var query = _context.SanPhams.Include(s => s.DanhMuc).AsQueryable();
                
                // Chỉ lọc sản phẩm active nếu không yêu cầu includeInactive
                if (!includeInactive)
                    query = query.Where(s => s.TrangThai == true);

                if (categoryId.HasValue)
                    query = query.Where(s => s.MaDM == categoryId.Value);

                if (!string.IsNullOrEmpty(search))
                    query = query.Where(s => s.TenSP.Contains(search) || (s.MoTa != null && s.MoTa.Contains(search)));

                var total = await query.CountAsync();
                var items = await query.Skip((page - 1) * pageSize).Take(pageSize)
                    .Select(s => new ProductDto
                    {
                        Id = s.MaSP,
                        Code = s.MaSPCode,
                        Name = s.TenSP,
                        Description = s.MoTa,
                        Price = (int)s.DonGia,
                        SalePrice = s.GiaKhuyenMai.HasValue ? (int?)s.GiaKhuyenMai.Value : null,
                        Image = s.HinhAnh,
                        CategoryId = s.MaDM ?? 0,
                        CategoryName = s.DanhMuc != null ? s.DanhMuc.TenDanhMuc : null,
                        SpicyLevel = s.CapDoCay,
                        IsFeatured = s.NoiBat,
                        IsActive = s.TrangThai
                    }).ToListAsync();

                return Ok(new { success = true, data = items, pagination = new { page, pageSize, total, totalPages = (int)Math.Ceiling((double)total / pageSize) } });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi lấy danh sách sản phẩm");
                return StatusCode(500, new { success = false, message = "Lỗi hệ thống" });
            }
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _context.SanPhams.Include(s => s.DanhMuc)
                .Where(s => s.MaSP == id)
                .Select(s => new ProductDto
                {
                    Id = s.MaSP, Code = s.MaSPCode, Name = s.TenSP, Description = s.MoTa,
                    Price = (int)s.DonGia, SalePrice = s.GiaKhuyenMai.HasValue ? (int?)s.GiaKhuyenMai.Value : null,
                    Image = s.HinhAnh, CategoryId = s.MaDM ?? 0, CategoryName = s.DanhMuc != null ? s.DanhMuc.TenDanhMuc : null,
                    SpicyLevel = s.CapDoCay, IsFeatured = s.NoiBat
                }).FirstOrDefaultAsync();

            if (product == null)
                return NotFound(new { success = false, message = "Không tìm thấy sản phẩm" });

            return Ok(new { success = true, data = product });
        }


        // GET: api/products/featured
        [HttpGet("featured")]
        public async Task<IActionResult> GetFeaturedProducts([FromQuery] int limit = 6)
        {
            var products = await _context.SanPhams.Include(s => s.DanhMuc)
                .Where(s => s.TrangThai == true && s.NoiBat == true)
                .Take(limit)
                .Select(s => new ProductDto
                {
                    Id = s.MaSP, Code = s.MaSPCode, Name = s.TenSP, Description = s.MoTa,
                    Price = (int)s.DonGia, SalePrice = s.GiaKhuyenMai.HasValue ? (int?)s.GiaKhuyenMai.Value : null,
                    Image = s.HinhAnh, CategoryId = s.MaDM ?? 0, CategoryName = s.DanhMuc != null ? s.DanhMuc.TenDanhMuc : null,
                    SpicyLevel = s.CapDoCay, IsFeatured = s.NoiBat
                }).ToListAsync();

            return Ok(new { success = true, data = products });
        }

        // GET: api/products/categories
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.DanhMucs
                .Where(d => d.TrangThai == true)
                .OrderBy(d => d.ThuTu)
                .Select(d => new
                {
                    id = d.MaDM,
                    name = d.TenDanhMuc,
                    description = d.MoTa,
                    image = d.HinhAnh,
                    count = _context.SanPhams.Count(s => s.MaDM == d.MaDM && s.TrangThai == true)
                }).ToListAsync();

            return Ok(new { success = true, data = categories });
        }

        // POST: api/products (Admin)
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request)
        {
            try
            {
                var sanPham = new MyCay.Domain.Entities.SanPham
                {
                    MaSPCode = request.Code,
                    TenSP = request.Name,
                    MoTa = request.Description,
                    DonGia = request.Price,
                    GiaKhuyenMai = request.SalePrice,
                    HinhAnh = request.Image,
                    MaDM = request.CategoryId,
                    CapDoCay = request.SpicyLevel,
                    NoiBat = request.IsFeatured,
                    TrangThai = true,
                    NgayTao = DateTime.Now
                };

                _context.SanPhams.Add(sanPham);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Thêm sản phẩm thành công", data = new { id = sanPham.MaSP } });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi thêm sản phẩm");
                return StatusCode(500, new { success = false, message = "Lỗi hệ thống" });
            }
        }

        // PUT: api/products/{id} (Admin)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] CreateProductRequest request)
        {
            var sanPham = await _context.SanPhams.FindAsync(id);
            if (sanPham == null)
                return NotFound(new { success = false, message = "Không tìm thấy sản phẩm" });

            sanPham.MaSPCode = request.Code;
            sanPham.TenSP = request.Name;
            sanPham.MoTa = request.Description;
            sanPham.DonGia = request.Price;
            sanPham.GiaKhuyenMai = request.SalePrice;
            sanPham.HinhAnh = request.Image;
            sanPham.MaDM = request.CategoryId;
            sanPham.CapDoCay = request.SpicyLevel;
            sanPham.NoiBat = request.IsFeatured;
            sanPham.NgayCapNhat = DateTime.Now;

            await _context.SaveChangesAsync();
            return Ok(new { success = true, message = "Cập nhật sản phẩm thành công" });
        }

        // DELETE: api/products/{id} (Admin)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var sanPham = await _context.SanPhams.FindAsync(id);
            if (sanPham == null)
                return NotFound(new { success = false, message = "Không tìm thấy sản phẩm" });

            sanPham.TrangThai = false; // Soft delete
            await _context.SaveChangesAsync();
            return Ok(new { success = true, message = "Xóa sản phẩm thành công" });
        }
    }

    public class ProductDto
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public int Price { get; set; }
        public int? SalePrice { get; set; }
        private string? _image;
        public string? Image 
        { 
            get => _image;
            set => _image = FormatImageUrl(value);
        }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int SpicyLevel { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsActive { get; set; } = true;
        
        private static string? FormatImageUrl(string? img)
        {
            if (string.IsNullOrEmpty(img)) return "/images/products/MenuItem_MI0001.webp";
            if (img.StartsWith("http") || img.StartsWith("/images/") || img.StartsWith("/")) return img;
            return "/images/products/" + img;
        }
    }

    public class CreateProductRequest
    {
        public string? Code { get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal? SalePrice { get; set; }
        public string? Image { get; set; }
        public int? CategoryId { get; set; }
        public int SpicyLevel { get; set; }
        public bool IsFeatured { get; set; }
    }
}
