using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MyCay.Web.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public ProductsController(IWebHostEnvironment env)
        {
            _env = env;
        }

        // GET: api/products
        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] int? categoryId, [FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var products = await LoadProductsAsync();

            // Filter by category
            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value).ToList();
            }

            // Search
            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p => 
                    p.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    (p.Description?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false)
                ).ToList();
            }

            var total = products.Count;
            var items = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return Ok(new
            {
                success = true,
                data = items,
                pagination = new
                {
                    page,
                    pageSize,
                    total,
                    totalPages = (int)Math.Ceiling((double)total / pageSize)
                }
            });
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var products = await LoadProductsAsync();
            var product = products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound(new { success = false, message = "Không tìm thấy sản phẩm" });

            return Ok(new { success = true, data = product });
        }

        // GET: api/products/featured
        [HttpGet("featured")]
        public async Task<IActionResult> GetFeaturedProducts([FromQuery] int limit = 6)
        {
            var products = await LoadProductsAsync();
            var featured = products.Take(limit).ToList();

            return Ok(new { success = true, data = featured });
        }

        // GET: api/products/categories
        [HttpGet("categories")]
        public IActionResult GetCategories()
        {
            var categories = new[]
            {
                new { id = 1, name = "Mì Cay", icon = "🍜", count = 10 },
                new { id = 2, name = "Mì Tương Đen", icon = "🥢", count = 4 },
                new { id = 3, name = "Mì Xào", icon = "🍝", count = 3 },
                new { id = 4, name = "Món Khác", icon = "🍚", count = 7 },
                new { id = 5, name = "Món Thêm Mì", icon = "🥚", count = 13 },
                new { id = 6, name = "Combo", icon = "🎁", count = 6 },
                new { id = 7, name = "Lẩu Hàn Quốc", icon = "🍲", count = 6 },
                new { id = 8, name = "Món Thêm Lẩu", icon = "🥬", count = 17 },
                new { id = 9, name = "Khai Vị", icon = "🍟", count = 17 },
                new { id = 10, name = "Giải Khát", icon = "🥤", count = 17 }
            };

            return Ok(new { success = true, data = categories });
        }

        private async Task<List<ProductDto>> LoadProductsAsync()
        {
            var filePath = Path.Combine(_env.WebRootPath, "data", "products.json");
            if (!System.IO.File.Exists(filePath))
                return new List<ProductDto>();

            var json = await System.IO.File.ReadAllTextAsync(filePath);
            var products = JsonSerializer.Deserialize<List<JsonProduct>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<JsonProduct>();

            return products.Select((p, i) => new ProductDto
            {
                Id = i + 1,
                Code = p.Code,
                Name = p.Name,
                Description = p.Description,
                Price = p.GetPriceAsInt(),
                Image = p.Image,
                CategoryId = GetCategoryId(p.Name)
            }).ToList();
        }

        private int GetCategoryId(string name)
        {
            if (name.Contains("Lẩu")) return 7;
            if (name.Contains("Combo")) return 6;
            if (name.Contains("Trộn Tương Đen")) return 2;
            if (name.Contains("Xào")) return 3;
            if (name.Contains("Cơm") || name.Contains("Tokbok") || name.Contains("Miến")) return 4;
            if (name.Contains("Nước") || name.Contains("Soda") || name.Contains("Trà") || name.Contains("Coca") || name.Contains("Sprite") || name.Contains("Sting")) return 10;
            if (name.Contains("Khoai") || name.Contains("Phô Mai Que") || name.Contains("Kimbap") || name.Contains("Gà Viên") || name.Contains("Mandu Chiên") || name.Contains("Salad")) return 9;
            return 1;
        }
    }

    public class JsonProduct
    {
        public string? Code { get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public string? Price { get; set; }
        public string? Category { get; set; }
        public string? Image { get; set; }
        
        public int GetPriceAsInt()
        {
            if (string.IsNullOrEmpty(Price)) return 0;
            // Parse "77,000VNĐ" -> 77000
            var numStr = new string(Price.Where(c => char.IsDigit(c)).ToArray());
            return int.TryParse(numStr, out var result) ? result : 0;
        }
    }

    public class ProductDto
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public int Price { get; set; }
        public string? Image { get; set; }
        public int CategoryId { get; set; }
    }
}
