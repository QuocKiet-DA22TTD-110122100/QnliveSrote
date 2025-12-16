using Microsoft.AspNetCore.Mvc;

namespace MyCay.Web.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        // Demo accounts
        private static readonly Dictionary<string, UserAccount> _accounts = new()
        {
            ["admin@mycaysasin.vn"] = new UserAccount { Email = "admin@mycaysasin.vn", Password = "123456", Name = "Nguyễn Văn Admin", Role = "admin" },
            ["admin"] = new UserAccount { Email = "admin@mycaysasin.vn", Password = "123456", Name = "Nguyễn Văn Admin", Role = "admin" },
            ["quanly1@mycaysasin.vn"] = new UserAccount { Email = "quanly1@mycaysasin.vn", Password = "123456", Name = "Trần Thị Quản Lý", Role = "manager" },
            ["quanly1"] = new UserAccount { Email = "quanly1@mycaysasin.vn", Password = "123456", Name = "Trần Thị Quản Lý", Role = "manager" },
            ["nhanvien1@mycaysasin.vn"] = new UserAccount { Email = "nhanvien1@mycaysasin.vn", Password = "123456", Name = "Lê Văn Nhân Viên", Role = "staff" },
            ["nhanvien1"] = new UserAccount { Email = "nhanvien1@mycaysasin.vn", Password = "123456", Name = "Lê Văn Nhân Viên", Role = "staff" },
            ["khach1@gmail.com"] = new UserAccount { Email = "khach1@gmail.com", Password = "123456", Name = "Nguyễn Thị Mai", Role = "customer" }
        };

        private static List<UserAccount> _registeredUsers = new();

        // POST: api/auth/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                return BadRequest(new { success = false, message = "Vui lòng nhập email và mật khẩu" });

            // Check demo accounts
            if (_accounts.TryGetValue(request.Email, out var account) && account.Password == request.Password)
            {
                return Ok(new
                {
                    success = true,
                    message = "Đăng nhập thành công",
                    data = new
                    {
                        email = account.Email,
                        name = account.Name,
                        role = account.Role,
                        token = GenerateToken(account)
                    }
                });
            }

            // Check registered users
            var user = _registeredUsers.FirstOrDefault(u => u.Email == request.Email && u.Password == request.Password);
            if (user != null)
            {
                return Ok(new
                {
                    success = true,
                    message = "Đăng nhập thành công",
                    data = new
                    {
                        email = user.Email,
                        name = user.Name,
                        role = user.Role,
                        token = GenerateToken(user)
                    }
                });
            }

            return Unauthorized(new { success = false, message = "Email hoặc mật khẩu không đúng" });
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.FullName))
                return BadRequest(new { success = false, message = "Vui lòng điền đầy đủ thông tin" });

            if (request.Password.Length < 6)
                return BadRequest(new { success = false, message = "Mật khẩu phải có ít nhất 6 ký tự" });

            // Check if email exists
            if (_accounts.ContainsKey(request.Email) || _registeredUsers.Any(u => u.Email == request.Email))
                return BadRequest(new { success = false, message = "Email đã được sử dụng" });

            var newUser = new UserAccount
            {
                Email = request.Email,
                Password = request.Password,
                Name = request.FullName,
                Phone = request.Phone,
                Role = "customer"
            };

            _registeredUsers.Add(newUser);

            return Ok(new
            {
                success = true,
                message = "Đăng ký thành công! Vui lòng đăng nhập.",
                data = new { email = newUser.Email, name = newUser.Name }
            });
        }

        // GET: api/auth/profile
        [HttpGet("profile")]
        public IActionResult GetProfile([FromHeader(Name = "Authorization")] string? token)
        {
            // Simple token validation (in production, use JWT)
            if (string.IsNullOrEmpty(token))
                return Unauthorized(new { success = false, message = "Chưa đăng nhập" });

            // Demo: return admin profile
            return Ok(new
            {
                success = true,
                data = new
                {
                    email = "admin@mycaysasin.vn",
                    name = "Nguyễn Văn Admin",
                    role = "admin",
                    phone = "0909000001"
                }
            });
        }

        // POST: api/auth/logout
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok(new { success = true, message = "Đăng xuất thành công" });
        }

        private string GenerateToken(UserAccount user)
        {
            // Simple token (in production, use JWT)
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{user.Email}:{user.Role}:{DateTime.Now.Ticks}"));
        }
    }

    public class UserAccount
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string Name { get; set; } = "";
        public string? Phone { get; set; }
        public string Role { get; set; } = "customer";
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
