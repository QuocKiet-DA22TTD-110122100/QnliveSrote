using Microsoft.AspNetCore.Mvc;

namespace MyCay.Web.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        // GET: api/reports/dashboard
        [HttpGet("dashboard")]
        public IActionResult GetDashboardStats()
        {
            return Ok(new
            {
                success = true,
                data = new
                {
                    todayOrders = 24,
                    todayRevenue = 3450000,
                    totalProducts = 100,
                    totalCustomers = 156,
                    pendingOrders = 3,
                    revenueGrowth = 15.5,
                    orderGrowth = 12.3
                }
            });
        }

        // GET: api/reports/revenue
        [HttpGet("revenue")]
        public IActionResult GetRevenueReport([FromQuery] string period = "week")
        {
            var data = period switch
            {
                "today" => new { labels = new[] { "8h", "10h", "12h", "14h", "16h", "18h", "20h" }, values = new[] { 150000, 450000, 850000, 320000, 280000, 720000, 680000 } },
                "week" => new { labels = new[] { "T2", "T3", "T4", "T5", "T6", "T7", "CN" }, values = new[] { 2800000, 3200000, 2900000, 3500000, 4100000, 5200000, 3400000 } },
                "month" => new { labels = new[] { "Tuần 1", "Tuần 2", "Tuần 3", "Tuần 4" }, values = new[] { 18500000, 21200000, 19800000, 25100000 } },
                _ => new { labels = new[] { "T1", "T2", "T3", "T4", "T5", "T6", "T7", "T8", "T9", "T10", "T11", "T12" }, values = new[] { 65000000, 72000000, 68000000, 75000000, 82000000, 78000000, 85000000, 92000000, 88000000, 95000000, 102000000, 98000000 } }
            };

            return Ok(new
            {
                success = true,
                data = new
                {
                    period,
                    labels = data.labels,
                    values = data.values,
                    total = data.values.Sum(),
                    average = data.values.Average()
                }
            });
        }

        // GET: api/reports/top-products
        [HttpGet("top-products")]
        public IActionResult GetTopProducts([FromQuery] int limit = 5)
        {
            var products = new[]
            {
                new { rank = 1, name = "Mì Thập Cẩm No Nê", sold = 89, revenue = 6853000 },
                new { rank = 2, name = "Mì Thập Cẩm", sold = 76, revenue = 5244000 },
                new { rank = 3, name = "Combo Vui Vẻ", sold = 65, revenue = 4485000 },
                new { rank = 4, name = "Tokbokki Phô Mai Sasin", sold = 58, revenue = 3422000 },
                new { rank = 5, name = "Mì Hải Sản", sold = 52, revenue = 3224000 },
                new { rank = 6, name = "Mì Bò Mỹ", sold = 48, revenue = 2832000 },
                new { rank = 7, name = "Kimbap Sasin", sold = 45, revenue = 1575000 },
                new { rank = 8, name = "Lẩu Sincay Hải Sản", sold = 32, revenue = 6368000 },
                new { rank = 9, name = "Phô Mai Que", sold = 28, revenue = 1092000 },
                new { rank = 10, name = "Nước Gạo Hàn Quốc", sold = 25, revenue = 875000 }
            };

            return Ok(new { success = true, data = products.Take(limit) });
        }

        // GET: api/reports/category-revenue
        [HttpGet("category-revenue")]
        public IActionResult GetCategoryRevenue()
        {
            var categories = new[]
            {
                new { name = "Mì Cay", revenue = 12500000, percent = 45 },
                new { name = "Combo", revenue = 5500000, percent = 20 },
                new { name = "Lẩu Hàn Quốc", revenue = 4200000, percent = 15 },
                new { name = "Khai Vị", revenue = 2800000, percent = 10 },
                new { name = "Giải Khát", revenue = 1900000, percent = 7 },
                new { name = "Khác", revenue = 800000, percent = 3 }
            };

            return Ok(new { success = true, data = categories });
        }

        // GET: api/reports/store-performance
        [HttpGet("store-performance")]
        public IActionResult GetStorePerformance()
        {
            var stores = new[]
            {
                new { id = 1, name = "Chi nhánh Quận 1", orders = 68, revenue = 12500000, growth = 18 },
                new { id = 2, name = "Chi nhánh Quận 3", orders = 52, revenue = 8200000, growth = 12 },
                new { id = 3, name = "Chi nhánh Quận 7", orders = 36, revenue = 4400000, growth = 25 }
            };

            return Ok(new { success = true, data = stores });
        }

        // GET: api/reports/peak-hours
        [HttpGet("peak-hours")]
        public IActionResult GetPeakHours()
        {
            var hours = new[]
            {
                new { time = "11:00 - 13:00", orders = 45, percent = 28 },
                new { time = "18:00 - 20:00", orders = 52, percent = 33 },
                new { time = "20:00 - 21:00", orders = 28, percent = 18 },
                new { time = "13:00 - 15:00", orders = 18, percent = 12 },
                new { time = "Khác", orders = 13, percent = 9 }
            };

            return Ok(new { success = true, data = hours });
        }
    }
}
