using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyCay.Infrastructure.Data;
using System.Text.Json;

namespace MyCay.Web.Pages;

public class ThucDonModel : PageModel
{
    private readonly MyCayDbContext _context;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<ThucDonModel> _logger;

    public ThucDonModel(MyCayDbContext context, IWebHostEnvironment env, ILogger<ThucDonModel> logger)
    {
        _context = context;
        _env = env;
        _logger = logger;
    }

    public List<ProductViewModel> Products { get; set; } = new();

    public async Task OnGetAsync()
    {
        try
        {
            // Thử lấy từ database
            var dbProducts = await _context.SanPhams
                .Include(s => s.DanhMuc)
                .Where(s => s.TrangThai == true)
                .OrderBy(s => s.MaDM)
                .ThenBy(s => s.TenSP)
                .ToListAsync();

            if (dbProducts.Any())
            {
                Products = dbProducts.Select(p => new ProductViewModel
                {
                    Id = p.MaSP,
                    Code = p.MaSPCode ?? $"SP{p.MaSP:D5}",
                    Name = p.TenSP,
                    Description = TruncateDescription(p.MoTa ?? "", 80),
                    Price = FormatPrice((int)p.DonGia),
                    PriceNumber = (int)p.DonGia,
                    Category = p.DanhMuc?.TenDanhMuc ?? "",
                    CategorySlug = GetCategorySlug(p.DanhMuc?.TenDanhMuc ?? ""),
                    Image = p.HinhAnh ?? "MenuItem_M00012.webp",
                    IsBestSeller = p.NoiBat
                }).ToList();
                _logger.LogInformation("Loaded {Count} products from database", Products.Count);
                return;
            }
            
            _logger.LogInformation("No products in database, falling back to JSON");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Không thể kết nối database, sử dụng file JSON");
        }

        // Fallback: đọc từ file JSON
        try
        {
            LoadFromJson();
            _logger.LogInformation("Loaded {Count} products from JSON", Products.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi đọc file JSON");
            Products = new List<ProductViewModel>(); // Trả về list rỗng thay vì crash
        }
    }

    private void LoadFromJson()
    {
        var jsonPath = Path.Combine(_env.WebRootPath, "data", "products.json");
        
        if (System.IO.File.Exists(jsonPath))
        {
            var json = System.IO.File.ReadAllText(jsonPath);
            var products = JsonSerializer.Deserialize<List<ProductData>>(json, new JsonSerializerOptions 
            { 
                PropertyNameCaseInsensitive = true 
            });

            if (products != null)
            {
                int index = 1;
                Products = products.Select(p => new ProductViewModel
                {
                    Id = index++,
                    Code = p.Code ?? "",
                    Name = p.Name ?? "",
                    Description = TruncateDescription(p.Description ?? "", 80),
                    Price = FormatPriceFromString(p.Price ?? ""),
                    PriceNumber = ExtractPriceNumber(p.Price ?? ""),
                    Category = p.Category ?? "",
                    CategorySlug = GetCategorySlugOld(p.Category ?? ""),
                    Image = p.Image ?? "",
                    IsBestSeller = IsBestSellerProduct(p.Code ?? "")
                }).ToList();
            }
        }
    }

    private string TruncateDescription(string desc, int maxLength)
    {
        if (string.IsNullOrEmpty(desc)) return "";
        desc = desc.Replace("\r\n", " ").Replace("\n", " ").Trim();
        return desc.Length <= maxLength ? desc : desc.Substring(0, maxLength) + "...";
    }

    private string FormatPrice(int price)
    {
        return string.Format("{0:N0}đ", price).Replace(",", ".");
    }

    private string FormatPriceFromString(string price)
    {
        var num = ExtractPriceNumber(price);
        return string.Format("{0:N0}đ", num).Replace(",", ".");
    }

    private int ExtractPriceNumber(string price)
    {
        var numStr = new string(price.Where(c => char.IsDigit(c) || c == ',').ToArray());
        numStr = numStr.Replace(",", "");
        return int.TryParse(numStr, out var result) ? result : 0;
    }

    private string GetCategorySlug(string category)
    {
        return category.ToLower() switch
        {
            "mì cay" => "mi-cay",
            "mì tương đen" => "mi-tuong-den",
            "mì xào" => "mi-xao",
            "món khác" => "mon-khac",
            "món thêm mì" => "mon-them",
            "combo" => "combo",
            "lẩu hàn quốc" => "lau",
            "món thêm lẩu" => "mon-them-lau",
            "khai vị" => "khai-vi",
            "giải khát" => "giai-khat",
            _ => "khac"
        };
    }

    private string GetCategorySlugOld(string category)
    {
        return category.ToLower() switch
        {
            "mi cay" => "mi-cay",
            "mi tuong den" => "mi-tuong-den",
            "mi xao" => "mi-xao",
            "mon khac" => "mon-khac",
            "mon them mi" => "mon-them",
            "combo" => "combo",
            "lau han quoc" => "lau",
            "mon them lau" => "mon-them-lau",
            "khai vi" => "khai-vi",
            "giai khat" => "giai-khat",
            _ => "khac"
        };
    }

    private bool IsBestSellerProduct(string code)
    {
        var bestSellers = new[] { "M00012", "MI0008", "M00018", "MI0006", "M00141", "M00143" };
        return bestSellers.Contains(code);
    }
}

public class ProductData
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Price { get; set; }
    public string? Category { get; set; }
    public string? Image { get; set; }
}

public class ProductViewModel
{
    public int Id { get; set; }
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string Price { get; set; } = "";
    public int PriceNumber { get; set; }
    public string Category { get; set; } = "";
    public string CategorySlug { get; set; } = "";
    public string Image { get; set; } = "";
    public bool IsBestSeller { get; set; }
}
