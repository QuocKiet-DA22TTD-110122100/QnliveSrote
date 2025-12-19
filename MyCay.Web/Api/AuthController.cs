using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyCay.Infrastructure.Data;
using MyCay.Web.Services;
using System.Security.Cryptography;
using System.Text;

namespace MyCay.Web.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly MyCayDbContext _context;
        private readonly ILogger<AuthController> _logger;
        private readonly IJwtService _jwtService;

        public AuthController(MyCayDbContext context, ILogger<AuthController> logger, IJwtService jwtService)
        {
            _context = context;
            _logger = logger;
            _jwtService = jwtService;
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                return BadRequest(new { success = false, message = "Vui lòng nhập tên đăng nhập và mật khẩu" });

            try
            {
                // Hash password với MD5 (giống trong database)
                var hashedPassword = GetMD5Hash(request.Password);

                // Tìm tài khoản theo TenDangNhap hoặc Email
                var taiKhoan = await _context.TaiKhoans
                    .Include(t => t.VaiTro)
                    .FirstOrDefaultAsync(t => 
                        (t.TenDangNhap == request.Email || t.Email == request.Email) 
                        && t.MatKhau == hashedPassword 
                        && t.TrangThai == true);

                if (taiKhoan == null)
                {
                    _logger.LogWarning("Đăng nhập thất bại: {Email}", request.Email);
                    return Unauthorized(new { success = false, message = "Tên đăng nhập hoặc mật khẩu không đúng" });
                }

                // Cập nhật lần đăng nhập cuối
                taiKhoan.LanDangNhapCuoi = DateTime.Now;
                await _context.SaveChangesAsync();

                // Lấy thông tin người dùng
                string hoTen = taiKhoan.TenDangNhap;
                string role = taiKhoan.VaiTro?.TenVaiTro ?? "KhachHang";

                // Tìm thông tin chi tiết theo vai trò
                if (role == "KhachHang")
                {
                    var khachHang = await _context.KhachHangs.FirstOrDefaultAsync(k => k.MaTK == taiKhoan.MaTK);
                    if (khachHang != null) hoTen = khachHang.HoTen;
                }
                else if (role == "NhanVien")
                {
                    var nhanVien = await _context.NhanViens.FirstOrDefaultAsync(n => n.MaTK == taiKhoan.MaTK);
                    if (nhanVien != null) hoTen = nhanVien.HoTen;
                }

                // Chuẩn hóa role
                var normalizedRole = NormalizeRole(role);
                var email = taiKhoan.Email ?? taiKhoan.TenDangNhap;
                
                // Tạo JWT token
                var token = _jwtService.GenerateToken(taiKhoan.MaTK, email, normalizedRole, hoTen);
                
                _logger.LogInformation("Đăng nhập thành công: {Email}, Role: {Role} -> {NormalizedRole}", request.Email, role, normalizedRole);

                return Ok(new
                {
                    success = true,
                    message = "Đăng nhập thành công",
                    data = new
                    {
                        id = taiKhoan.MaTK,
                        email = email,
                        name = hoTen,
                        role = normalizedRole,
                        token = token
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi đăng nhập");
                return StatusCode(500, new { success = false, message = "Lỗi hệ thống, vui lòng thử lại" });
            }
        }


        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.FullName))
                return BadRequest(new { success = false, message = "Vui lòng điền đầy đủ thông tin" });

            if (request.Password.Length < 6)
                return BadRequest(new { success = false, message = "Mật khẩu phải có ít nhất 6 ký tự" });

            try
            {
                // Kiểm tra email/username đã tồn tại
                var exists = await _context.TaiKhoans.AnyAsync(t => t.TenDangNhap == request.Email || t.Email == request.Email);
                if (exists)
                    return BadRequest(new { success = false, message = "Email đã được sử dụng" });

                // Lấy vai trò KhachHang
                var vaiTroKhachHang = await _context.VaiTros.FirstOrDefaultAsync(v => v.TenVaiTro == "KhachHang");
                if (vaiTroKhachHang == null)
                    return StatusCode(500, new { success = false, message = "Lỗi cấu hình hệ thống" });

                // Tạo tài khoản mới
                var taiKhoan = new MyCay.Domain.Entities.TaiKhoan
                {
                    TenDangNhap = request.Email,
                    MatKhau = GetMD5Hash(request.Password),
                    Email = request.Email,
                    MaVaiTro = vaiTroKhachHang.MaVaiTro,
                    TrangThai = true,
                    NgayTao = DateTime.Now
                };

                _context.TaiKhoans.Add(taiKhoan);
                await _context.SaveChangesAsync();

                // Tạo khách hàng
                var khachHang = new MyCay.Domain.Entities.KhachHang
                {
                    HoTen = request.FullName,
                    Email = request.Email,
                    SDT = request.Phone ?? "",
                    MaTK = taiKhoan.MaTK,
                    DiemTichLuy = 0,
                    TrangThai = true,
                    NgayDangKy = DateTime.Now
                };

                _context.KhachHangs.Add(khachHang);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Đăng ký thành công: {Email}", request.Email);

                return Ok(new
                {
                    success = true,
                    message = "Đăng ký thành công! Vui lòng đăng nhập.",
                    data = new { email = request.Email, name = request.FullName }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi đăng ký");
                return StatusCode(500, new { success = false, message = "Lỗi hệ thống, vui lòng thử lại" });
            }
        }

        // GET: api/auth/profile
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile([FromHeader(Name = "Authorization")] string? token)
        {
            if (string.IsNullOrEmpty(token))
                return Unauthorized(new { success = false, message = "Chưa đăng nhập" });

            try
            {
                // Sử dụng JWT Service để parse token
                var tokenData = _jwtService.ParseToken(token);
                if (tokenData == null)
                    return Unauthorized(new { success = false, message = "Token không hợp lệ hoặc đã hết hạn" });

                var taiKhoan = await _context.TaiKhoans
                    .Include(t => t.VaiTro)
                    .FirstOrDefaultAsync(t => t.MaTK == tokenData.Value.userId);

                if (taiKhoan == null)
                    return Unauthorized(new { success = false, message = "Tài khoản không tồn tại" });

                string hoTen = taiKhoan.TenDangNhap;
                string? sdt = null;

                var khachHang = await _context.KhachHangs.FirstOrDefaultAsync(k => k.MaTK == taiKhoan.MaTK);
                if (khachHang != null)
                {
                    hoTen = khachHang.HoTen;
                    sdt = khachHang.SDT;
                }

                return Ok(new
                {
                    success = true,
                    data = new
                    {
                        id = taiKhoan.MaTK,
                        email = taiKhoan.Email ?? taiKhoan.TenDangNhap,
                        name = hoTen,
                        role = NormalizeRole(taiKhoan.VaiTro?.TenVaiTro ?? "KhachHang"),
                        phone = sdt
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi lấy profile");
                return StatusCode(500, new { success = false, message = "Lỗi hệ thống" });
            }
        }
        
        // POST: api/auth/refresh - Làm mới token
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromHeader(Name = "Authorization")] string? token)
        {
            if (string.IsNullOrEmpty(token))
                return Unauthorized(new { success = false, message = "Token không hợp lệ" });

            var tokenData = _jwtService.ParseToken(token);
            if (tokenData == null)
                return Unauthorized(new { success = false, message = "Token không hợp lệ hoặc đã hết hạn" });

            var taiKhoan = await _context.TaiKhoans
                .Include(t => t.VaiTro)
                .FirstOrDefaultAsync(t => t.MaTK == tokenData.Value.userId);

            if (taiKhoan == null || !taiKhoan.TrangThai)
                return Unauthorized(new { success = false, message = "Tài khoản không tồn tại hoặc đã bị khóa" });

            string hoTen = taiKhoan.TenDangNhap;
            var khachHang = await _context.KhachHangs.FirstOrDefaultAsync(k => k.MaTK == taiKhoan.MaTK);
            if (khachHang != null) hoTen = khachHang.HoTen;

            var normalizedRole = NormalizeRole(taiKhoan.VaiTro?.TenVaiTro ?? "KhachHang");
            var newToken = _jwtService.GenerateToken(taiKhoan.MaTK, taiKhoan.Email ?? taiKhoan.TenDangNhap, normalizedRole, hoTen);

            return Ok(new
            {
                success = true,
                message = "Token đã được làm mới",
                data = new { token = newToken }
            });
        }

        // POST: api/auth/logout
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok(new { success = true, message = "Đăng xuất thành công" });
        }

        private string GetMD5Hash(string input)
        {
            using var md5 = MD5.Create();
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = md5.ComputeHash(inputBytes);
            return Convert.ToHexString(hashBytes).ToLower();
        }
        
        /// <summary>
        /// Chuẩn hóa role từ database sang frontend
        /// Database: QuanTriVien, QuanLy, NhanVien, KhachHang
        /// Frontend: admin, manager, staff, customer
        /// </summary>
        private static string NormalizeRole(string? role)
        {
            return role?.ToLower() switch
            {
                "quantrivien" => "admin",
                "quanly" => "manager",
                "nhanvien" => "staff",
                "khachhang" => "customer",
                _ => role?.ToLower() ?? "customer"
            };
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }

    public class RegisterRequest
    {
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string? Phone { get; set; }
        public string Password { get; set; } = "";
    }
}
