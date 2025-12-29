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
                new { rank = 1, name = "Mì Thập Cẩm No Nê", quantity = 89, revenue = 6853000 },
                new { rank = 2, name = "Mì Thập Cẩm", quantity = 76, revenue = 5244000 },
                new { rank = 3, name = "Combo Vui Vẻ", quantity = 65, revenue = 4485000 },
                new { rank = 4, name = "Tokbokki Phô Mai Sasin", quantity = 58, revenue = 3422000 },
                new { rank = 5, name = "Mì Hải Sản", quantity = 52, revenue = 3224000 },
                new { rank = 6, name = "Mì Bò Mỹ", quantity = 48, revenue = 2832000 },
                new { rank = 7, name = "Kimbap Sasin", quantity = 45, revenue = 1575000 },
                new { rank = 8, name = "Lẩu Sincay Hải Sản", quantity = 32, revenue = 6368000 },
                new { rank = 9, name = "Phô Mai Que", quantity = 28, revenue = 1092000 },
                new { rank = 10, name = "Nước Gạo Hàn Quốc", quantity = 25, revenue = 875000 }
            };

            return Ok(new { success = true, data = products.Take(limit) });
        }

        // GET: api/reports/category-revenue
        [HttpGet("category-revenue")]
        public IActionResult GetCategoryRevenue()
        {
            return Ok(new
            {
                success = true,
                labels = new[] { "Mì Cay", "Combo", "Lẩu Hàn Quốc", "Khai Vị", "Giải Khát", "Khác" },
                values = new[] { 12500000, 5500000, 4200000, 2800000, 1900000, 800000 }
            });
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

        // =====================================================
        // THỐNG KÊ CHI TIẾT
        // =====================================================

        // GET: api/reports/stats - Thống kê tổng quan theo kỳ
        [HttpGet("stats")]
        public IActionResult GetStats([FromQuery] string period = "week")
        {
            // Mock data - trong thực tế sẽ query từ database
            var stats = period switch
            {
                "today" => new { totalRevenue = 3450000m, totalOrders = 24, avgOrderValue = 143750m, newCustomers = 5, revenueChange = 12.5, ordersChange = 8.3 },
                "week" => new { totalRevenue = 25100000m, totalOrders = 156, avgOrderValue = 160897m, newCustomers = 28, revenueChange = 15.2, ordersChange = 10.5 },
                "month" => new { totalRevenue = 84600000m, totalOrders = 512, avgOrderValue = 165234m, newCustomers = 89, revenueChange = 18.7, ordersChange = 14.2 },
                _ => new { totalRevenue = 980000000m, totalOrders = 5840, avgOrderValue = 167808m, newCustomers = 1250, revenueChange = 22.5, ordersChange = 18.9 }
            };

            return Ok(new { success = true, data = stats });
        }

        // GET: api/reports/revenue-chart - Biểu đồ doanh thu
        [HttpGet("revenue-chart")]
        public IActionResult GetRevenueChart([FromQuery] string period = "week")
        {
            var data = period switch
            {
                "today" => new { 
                    labels = new[] { "8h", "9h", "10h", "11h", "12h", "13h", "14h", "15h", "16h", "17h", "18h", "19h", "20h", "21h" }, 
                    revenue = new[] { 0, 150000, 280000, 520000, 680000, 450000, 320000, 180000, 250000, 380000, 620000, 750000, 580000, 270000 },
                    orders = new[] { 0, 1, 2, 4, 5, 3, 2, 1, 2, 3, 5, 6, 4, 2 }
                },
                "week" => new { 
                    labels = new[] { "T2", "T3", "T4", "T5", "T6", "T7", "CN" }, 
                    revenue = new[] { 2800000, 3200000, 2900000, 3500000, 4100000, 5200000, 3400000 },
                    orders = new[] { 18, 21, 19, 23, 27, 34, 22 }
                },
                "month" => new { 
                    labels = new[] { "Tuần 1", "Tuần 2", "Tuần 3", "Tuần 4" }, 
                    revenue = new[] { 18500000, 21200000, 19800000, 25100000 },
                    orders = new[] { 115, 132, 123, 156 }
                },
                _ => new { 
                    labels = new[] { "T1", "T2", "T3", "T4", "T5", "T6", "T7", "T8", "T9", "T10", "T11", "T12" }, 
                    revenue = new[] { 65000000, 72000000, 68000000, 75000000, 82000000, 78000000, 85000000, 92000000, 88000000, 95000000, 102000000, 98000000 },
                    orders = new[] { 405, 450, 425, 468, 512, 487, 531, 575, 550, 593, 637, 612 }
                }
            };

            return Ok(data);
        }

        // GET: api/reports/order-status - Phân bố đơn hàng theo trạng thái
        [HttpGet("order-status")]
        public IActionResult GetOrderStatus()
        {
            return Ok(new
            {
                labels = new[] { "Chờ xác nhận", "Đang chuẩn bị", "Đang giao", "Hoàn thành", "Đã hủy" },
                values = new[] { 12, 8, 5, 125, 6 }
            });
        }

        // GET: api/reports/hourly-orders - Đơn hàng theo giờ
        [HttpGet("hourly-orders")]
        public IActionResult GetHourlyOrders()
        {
            return Ok(new
            {
                labels = new[] { "10h", "11h", "12h", "13h", "14h", "15h", "16h", "17h", "18h", "19h", "20h", "21h" },
                values = new[] { 3, 8, 15, 12, 6, 4, 5, 9, 18, 22, 16, 8 }
            });
        }
    }
}
