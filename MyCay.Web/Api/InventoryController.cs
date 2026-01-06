using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyCay.Infrastructure.Data;
using MyCay.Domain.Entities;

namespace MyCay.Web.Api;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly MyCayDbContext _context;

    public InventoryController(MyCayDbContext context)
    {
        _context = context;
    }

    // ========== NGUYÊN VẬT LIỆU ==========

    // GET: api/inventory/materials
    [HttpGet("materials")]
    public async Task<IActionResult> GetMaterials()
    {
        var materials = await _context.NguyenVatLieus
            .Where(n => n.TrangThai)
            .OrderBy(n => n.NhomNVL)
            .ThenBy(n => n.TenNVL)
            .ToListAsync();

        return Ok(materials);
    }

    // POST: api/inventory/materials
    [HttpPost("materials")]
    public async Task<IActionResult> CreateMaterial([FromBody] NguyenVatLieu material)
    {
        _context.NguyenVatLieus.Add(material);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Thêm nguyên vật liệu thành công", id = material.MaNVL });
    }

    // PUT: api/inventory/materials/5
    [HttpPut("materials/{id}")]
    public async Task<IActionResult> UpdateMaterial(int id, [FromBody] NguyenVatLieu material)
    {
        var existing = await _context.NguyenVatLieus.FindAsync(id);
        if (existing == null)
            return NotFound(new { message = "Không tìm thấy nguyên vật liệu" });

        existing.TenNVL = material.TenNVL;
        existing.DonViTinh = material.DonViTinh;
        existing.GiaNhap = material.GiaNhap;
        existing.SoLuongToiThieu = material.SoLuongToiThieu;
        existing.NhomNVL = material.NhomNVL;

        await _context.SaveChangesAsync();
        return Ok(new { message = "Cập nhật thành công" });
    }

    // ========== TỒN KHO ==========

    // GET: api/inventory/stock?branchId=1
    [HttpGet("stock")]
    public async Task<IActionResult> GetStock([FromQuery] int? branchId)
    {
        var query = _context.TonKhos
            .Include(t => t.ChiNhanh)
            .Include(t => t.NguyenVatLieu)
            .AsQueryable();

        if (branchId.HasValue)
            query = query.Where(t => t.MaCN == branchId);

        var stock = await query
            .Select(t => new
            {
                t.MaTK,
                t.MaCN,
                TenChiNhanh = t.ChiNhanh!.TenChiNhanh,
                t.MaNVL,
                TenNVL = t.NguyenVatLieu!.TenNVL,
                DonViTinh = t.NguyenVatLieu.DonViTinh,
                NhomNVL = t.NguyenVatLieu.NhomNVL,
                t.SoLuong,
                SoLuongToiThieu = t.NguyenVatLieu.SoLuongToiThieu,
                CanhBao = t.SoLuong < t.NguyenVatLieu.SoLuongToiThieu,
                t.NgayCapNhat
            })
            .OrderBy(t => t.TenChiNhanh)
            .ThenBy(t => t.NhomNVL)
            .ToListAsync();

        return Ok(stock);
    }

    // GET: api/inventory/alerts - Cảnh báo tồn kho thấp
    [HttpGet("alerts")]
    public async Task<IActionResult> GetStockAlerts()
    {
        var alerts = await _context.TonKhos
            .Include(t => t.ChiNhanh)
            .Include(t => t.NguyenVatLieu)
            .Where(t => t.SoLuong < t.NguyenVatLieu!.SoLuongToiThieu)
            .Select(t => new
            {
                t.MaCN,
                TenChiNhanh = t.ChiNhanh!.TenChiNhanh,
                t.MaNVL,
                TenNVL = t.NguyenVatLieu!.TenNVL,
                DonViTinh = t.NguyenVatLieu.DonViTinh,
                t.SoLuong,
                SoLuongToiThieu = t.NguyenVatLieu.SoLuongToiThieu,
                ThieuHut = t.NguyenVatLieu.SoLuongToiThieu - t.SoLuong
            })
            .ToListAsync();

        return Ok(new { count = alerts.Count, alerts });
    }

    // PUT: api/inventory/stock - Cập nhật tồn kho
    [HttpPut("stock")]
    public async Task<IActionResult> UpdateStock([FromBody] UpdateStockRequest request)
    {
        var stock = await _context.TonKhos
            .FirstOrDefaultAsync(t => t.MaCN == request.MaCN && t.MaNVL == request.MaNVL);

        if (stock == null)
        {
            // Tạo mới nếu chưa có
            stock = new TonKho
            {
                MaCN = request.MaCN,
                MaNVL = request.MaNVL,
                SoLuong = request.SoLuong
            };
            _context.TonKhos.Add(stock);
        }
        else
        {
            if (request.IsAdd)
                stock.SoLuong += request.SoLuong;
            else
                stock.SoLuong = request.SoLuong;
            
            stock.NgayCapNhat = DateTime.Now;
        }

        await _context.SaveChangesAsync();
        return Ok(new { message = "Cập nhật tồn kho thành công", soLuong = stock.SoLuong });
    }

    // ========== CÔNG THỨC ==========

    // GET: api/inventory/recipes?productId=1
    [HttpGet("recipes")]
    public async Task<IActionResult> GetRecipes([FromQuery] int? productId)
    {
        var query = _context.CongThucs
            .Include(c => c.SanPham)
            .Include(c => c.NguyenVatLieu)
            .AsQueryable();

        if (productId.HasValue)
            query = query.Where(c => c.MaSP == productId);

        var recipes = await query
            .Select(c => new
            {
                c.MaCT,
                c.MaSP,
                TenSP = c.SanPham!.TenSP,
                c.MaNVL,
                TenNVL = c.NguyenVatLieu!.TenNVL,
                DonViTinh = c.NguyenVatLieu.DonViTinh,
                c.SoLuong,
                c.GhiChu
            })
            .ToListAsync();

        return Ok(recipes);
    }

    // POST: api/inventory/recipes
    [HttpPost("recipes")]
    public async Task<IActionResult> CreateRecipe([FromBody] CongThuc recipe)
    {
        _context.CongThucs.Add(recipe);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Thêm công thức thành công" });
    }

    // POST: api/inventory/deduct - Trừ kho khi có đơn hàng
    [HttpPost("deduct")]
    public async Task<IActionResult> DeductStock([FromBody] DeductStockRequest request)
    {
        // Lấy công thức của sản phẩm
        var recipes = await _context.CongThucs
            .Where(c => c.MaSP == request.MaSP)
            .ToListAsync();

        if (!recipes.Any())
            return Ok(new { message = "Sản phẩm không có công thức, bỏ qua trừ kho" });

        foreach (var recipe in recipes)
        {
            var stock = await _context.TonKhos
                .FirstOrDefaultAsync(t => t.MaCN == request.MaCN && t.MaNVL == recipe.MaNVL);

            if (stock != null)
            {
                stock.SoLuong -= recipe.SoLuong * request.SoLuong;
                if (stock.SoLuong < 0) stock.SoLuong = 0;
                stock.NgayCapNhat = DateTime.Now;
            }
        }

        await _context.SaveChangesAsync();
        return Ok(new { message = "Đã trừ kho thành công" });
    }
}

public class UpdateStockRequest
{
    public int MaCN { get; set; }
    public int MaNVL { get; set; }
    public decimal SoLuong { get; set; }
    public bool IsAdd { get; set; } = false; // true = cộng thêm, false = set giá trị
}

public class DeductStockRequest
{
    public int MaCN { get; set; }
    public int MaSP { get; set; }
    public int SoLuong { get; set; } = 1;
}
