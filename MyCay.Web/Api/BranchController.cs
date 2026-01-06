using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyCay.Infrastructure.Data;
using MyCay.Domain.Entities;

namespace MyCay.Web.Api;

[ApiController]
[Route("api/[controller]")]
public class BranchController : ControllerBase
{
    private readonly MyCayDbContext _context;
    private readonly ILogger<BranchController> _logger;

    public BranchController(MyCayDbContext context, ILogger<BranchController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: api/branch
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var branches = await _context.ChiNhanhs
                .Where(c => c.TrangThai)
                .Select(c => new
                {
                    c.MaCN,
                    c.TenChiNhanh,
                    c.DiaChi,
                    c.QuanHuyen,
                    c.ThanhPho,
                    c.SoDienThoai,
                    c.Email,
                    c.GioMoCua,
                    c.GioDongCua,
                    SoNhanVien = c.NhanViens.Count(n => n.TrangThai)
                })
                .ToListAsync();

            return Ok(new { success = true, data = branches });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi lấy danh sách chi nhánh");
            return StatusCode(500, new { success = false, message = "Lỗi kết nối database" });
        }
    }

    // GET: api/branch/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var branch = await _context.ChiNhanhs
                .Include(c => c.NhanViens.Where(n => n.TrangThai))
                .Include(c => c.TonKhos)
                    .ThenInclude(t => t.NguyenVatLieu)
                .FirstOrDefaultAsync(c => c.MaCN == id);

            if (branch == null)
                return NotFound(new { success = false, message = "Không tìm thấy chi nhánh" });

            return Ok(new
            {
                success = true,
                data = new
                {
                    branch.MaCN,
                    branch.TenChiNhanh,
                    branch.DiaChi,
                    branch.QuanHuyen,
                    branch.ThanhPho,
                    branch.SoDienThoai,
                    branch.Email,
                    branch.GioMoCua,
                    branch.GioDongCua,
                    branch.TrangThai,
                    NhanViens = branch.NhanViens.Select(n => new { n.MaNV, n.HoTen, n.ChucVu }),
                    TonKho = branch.TonKhos.Select(t => new
                    {
                        t.MaNVL,
                        TenNVL = t.NguyenVatLieu?.TenNVL,
                        DonViTinh = t.NguyenVatLieu?.DonViTinh,
                        t.SoLuong,
                        SoLuongToiThieu = t.NguyenVatLieu?.SoLuongToiThieu,
                        CanhBao = t.SoLuong < (t.NguyenVatLieu?.SoLuongToiThieu ?? 0)
                    })
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi lấy chi nhánh {Id}", id);
            return StatusCode(500, new { success = false, message = "Lỗi hệ thống" });
        }
    }

    // POST: api/branch
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ChiNhanh branch)
    {
        try
        {
            _context.ChiNhanhs.Add(branch);
            await _context.SaveChangesAsync();
            return Ok(new { success = true, message = "Tạo chi nhánh thành công", id = branch.MaCN });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi tạo chi nhánh");
            return StatusCode(500, new { success = false, message = "Lỗi hệ thống" });
        }
    }

    // PUT: api/branch/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ChiNhanh branch)
    {
        try
        {
            var existing = await _context.ChiNhanhs.FindAsync(id);
            if (existing == null)
                return NotFound(new { success = false, message = "Không tìm thấy chi nhánh" });

            existing.TenChiNhanh = branch.TenChiNhanh;
            existing.DiaChi = branch.DiaChi;
            existing.QuanHuyen = branch.QuanHuyen;
            existing.ThanhPho = branch.ThanhPho;
            existing.SoDienThoai = branch.SoDienThoai;
            existing.Email = branch.Email;
            existing.GioMoCua = branch.GioMoCua;
            existing.GioDongCua = branch.GioDongCua;
            existing.TrangThai = branch.TrangThai;

            await _context.SaveChangesAsync();
            return Ok(new { success = true, message = "Cập nhật chi nhánh thành công" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi cập nhật chi nhánh {Id}", id);
            return StatusCode(500, new { success = false, message = "Lỗi hệ thống" });
        }
    }

    // DELETE: api/branch/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var branch = await _context.ChiNhanhs.FindAsync(id);
            if (branch == null)
                return NotFound(new { success = false, message = "Không tìm thấy chi nhánh" });

            branch.TrangThai = false;
            await _context.SaveChangesAsync();
            return Ok(new { success = true, message = "Đã vô hiệu hóa chi nhánh" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi xóa chi nhánh {Id}", id);
            return StatusCode(500, new { success = false, message = "Lỗi hệ thống" });
        }
    }
}
