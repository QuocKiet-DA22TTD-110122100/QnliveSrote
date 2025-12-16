using Microsoft.AspNetCore.Mvc;

namespace MyCay.Web.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private static List<CustomerDto> _customers = new()
        {
            new CustomerDto { Id = 1, Name = "Nguyễn Thị Mai", Phone = "0988888881", Email = "khach1@gmail.com", Address = "123 Lê Lợi, Quận 1", Points = 150, OrderCount = 8, Status = true },
            new CustomerDto { Id = 2, Name = "Trần Văn Hùng", Phone = "0988888882", Email = "hung.tran@gmail.com", Address = "456 Hai Bà Trưng, Quận 3", Points = 280, OrderCount = 15, Status = true },
            new CustomerDto { Id = 3, Name = "Lê Thị Hương", Phone = "0988888883", Email = "huong.le@gmail.com", Address = "789 Nguyễn Trãi, Quận 5", Points = 50, OrderCount = 3, Status = true },
            new CustomerDto { Id = 4, Name = "Phạm Minh Tuấn", Phone = "0977777771", Email = "tuan.pm@gmail.com", Address = "321 Điện Biên Phủ, Quận 10", Points = 420, OrderCount = 22, Status = true },
            new CustomerDto { Id = 5, Name = "Võ Thị Lan", Phone = "0966666661", Email = "lan.vo@gmail.com", Address = "654 Cách Mạng Tháng 8, Quận Tân Bình", Points = 85, OrderCount = 5, Status = true }
        };

        // GET: api/customers
        [HttpGet]
        public IActionResult GetCustomers([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var customers = _customers.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                var searchLower = search.ToLower();
                customers = customers.Where(c =>
                    c.Name.ToLower().Contains(searchLower) ||
                    c.Phone.Contains(search) ||
                    (c.Email != null && c.Email.ToLower().Contains(searchLower))
                );
            }

            var total = customers.Count();
            var items = customers.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return Ok(new
            {
                success = true,
                data = items,
                pagination = new { page, pageSize, total }
            });
        }

        // GET: api/customers/{id}
        [HttpGet("{id}")]
        public IActionResult GetCustomer(int id)
        {
            var customer = _customers.FirstOrDefault(c => c.Id == id);
            if (customer == null)
                return NotFound(new { success = false, message = "Không tìm thấy khách hàng" });

            return Ok(new { success = true, data = customer });
        }

        // POST: api/customers
        [HttpPost]
        public IActionResult CreateCustomer([FromBody] CreateCustomerRequest request)
        {
            if (string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.Phone))
                return BadRequest(new { success = false, message = "Vui lòng điền đầy đủ thông tin" });

            var newCustomer = new CustomerDto
            {
                Id = _customers.Count + 1,
                Name = request.Name,
                Phone = request.Phone,
                Email = request.Email,
                Address = request.Address,
                Points = 0,
                OrderCount = 0,
                Status = true,
                CreatedAt = DateTime.Now
            };

            _customers.Add(newCustomer);

            return Ok(new { success = true, message = "Thêm khách hàng thành công", data = newCustomer });
        }

        // PUT: api/customers/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateCustomer(int id, [FromBody] CreateCustomerRequest request)
        {
            var customer = _customers.FirstOrDefault(c => c.Id == id);
            if (customer == null)
                return NotFound(new { success = false, message = "Không tìm thấy khách hàng" });

            customer.Name = request.Name ?? customer.Name;
            customer.Phone = request.Phone ?? customer.Phone;
            customer.Email = request.Email ?? customer.Email;
            customer.Address = request.Address ?? customer.Address;

            return Ok(new { success = true, message = "Cập nhật thành công", data = customer });
        }

        // DELETE: api/customers/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer(int id)
        {
            var customer = _customers.FirstOrDefault(c => c.Id == id);
            if (customer == null)
                return NotFound(new { success = false, message = "Không tìm thấy khách hàng" });

            _customers.Remove(customer);
            return Ok(new { success = true, message = "Đã xóa khách hàng" });
        }

        // GET: api/customers/stats
        [HttpGet("stats")]
        public IActionResult GetStats()
        {
            return Ok(new
            {
                success = true,
                data = new
                {
                    total = _customers.Count,
                    active = _customers.Count(c => c.Status),
                    vip = _customers.Count(c => c.Points >= 200),
                    newThisMonth = _customers.Count(c => c.CreatedAt?.Month == DateTime.Now.Month)
                }
            });
        }
    }

    public class CustomerDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Phone { get; set; } = "";
        public string? Email { get; set; }
        public string? Address { get; set; }
        public int Points { get; set; }
        public int OrderCount { get; set; }
        public bool Status { get; set; } = true;
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }

    public class CreateCustomerRequest
    {
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
    }
}
