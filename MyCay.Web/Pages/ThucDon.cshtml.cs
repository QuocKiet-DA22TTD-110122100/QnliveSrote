using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace MyCay.Web.Pages;

public class ThucDonModel : PageModel
{
    public List<ProductViewModel> Products { get; set; } = new();

    public void OnGet()
    {
        var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "products.json");
        
        if (System.IO.File.Exists(jsonPath))
        {
            var json = System.IO.File.ReadAllText(jsonPath);
            var products = JsonSerializer.Deserialize<List<ProductData>>(json, new JsonSerializerOptions 
            { 
                PropertyNameCaseInsensitive = true 
            });

            if (products != null)
            {
                Products = products.Select(p => new ProductViewModel
                {
                    Code = p.Code ?? "",
                    Name = p.Name ?? "",
                    Description = TruncateDescription(p.Description ?? "", 80),
                    Price = FormatPrice(p.Price ?? ""),
                    PriceNumber = ExtractPriceNumber(p.Price ?? ""),
                    Category = p.Category ?? "",
                    CategorySlug = GetCategorySlug(p.Category ?? ""),
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

    private string FormatPrice(string price)
    {
        // Convert "77,000VN&#x110;" to "77.000đ"
        var num = ExtractPriceNumber(price);
        return $"{num:N0}đ".Replace(",", ".");
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
            "mi cay" => "mi-cay",
            "mi tuong den" => "mi-tuong-den",
            "mi xao" => "mi-xao",
            "mon khac" => "mon-khac",
            "mon them mi" => "mon-them",
            "combo" => "combo",
            "lau han quoc" => "lau",
            "mon them lau" => "mon-them",
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
