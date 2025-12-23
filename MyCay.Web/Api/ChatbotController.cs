using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using MyCay.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MyCay.Web.Api;

[ApiController]
[Route("api/[controller]")]
public class ChatbotController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly MyCayDbContext _context;
    private readonly HttpClient _httpClient;

    public ChatbotController(IConfiguration config, MyCayDbContext context, IHttpClientFactory httpClientFactory)
    {
        _config = config;
        _context = context;
        _httpClient = httpClientFactory.CreateClient();
    }

    [HttpPost("chat")]
    public async Task<IActionResult> Chat([FromBody] ChatRequest request)
    {
        try
        {
            // Ưu tiên GitHub Models API
            var githubApiKey = _config["GitHubModels:ApiKey"];
            var model = _config["GitHubModels:Model"] ?? "gpt-4o-mini";
            
            if (!string.IsNullOrEmpty(githubApiKey) && !githubApiKey.Contains("api_key"))
            {
                return await ChatWithGitHubModels(request.Message, githubApiKey, model);
            }

            // Fallback to Gemini
            var geminiKey = _config["Gemini:ApiKey"];
            if (!string.IsNullOrEmpty(geminiKey) && geminiKey != "YOUR_GEMINI_API_KEY_HERE")
            {
                return await ChatWithGemini(request.Message, geminiKey);
            }

            // Fallback response khi không có API key
            return Ok(new { reply = GetFallbackResponse(request.Message) });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Chatbot error: {ex.Message}");
            return Ok(new { reply = "Xin lỗi, có lỗi xảy ra. Vui lòng thử lại sau! 🙏" });
        }
    }

    private async Task<IActionResult> ChatWithGitHubModels(string message, string apiKey, string model)
    {
        var systemPrompt = await BuildSystemPrompt();
        
        var url = "https://models.inference.ai.azure.com/chat/completions";
        
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        var requestBody = new
        {
            model = model,
            messages = new[]
            {
                new { role = "system", content = systemPrompt },
                new { role = "user", content = message }
            },
            temperature = 0.7,
            max_tokens = 500
        };

        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(url, content);
        var responseText = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            using var doc = JsonDocument.Parse(responseText);
            var reply = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return Ok(new { reply });
        }
        else
        {
            Console.WriteLine($"GitHub Models API error: {responseText}");
            return Ok(new { reply = GetFallbackResponse(message) });
        }
    }

    private async Task<IActionResult> ChatWithGemini(string message, string apiKey)
    {
        var systemPrompt = await BuildSystemPrompt();
        
        var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={apiKey}";

        var requestBody = new
        {
            contents = new[]
            {
                new { role = "user", parts = new[] { new { text = systemPrompt + "\n\nKhách hàng: " + message } } }
            },
            generationConfig = new
            {
                temperature = 0.7,
                maxOutputTokens = 500
            }
        };

        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(url, content);
        var responseText = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            using var doc = JsonDocument.Parse(responseText);
            var reply = doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            return Ok(new { reply });
        }
        
        return Ok(new { reply = GetFallbackResponse(message) });
    }

    private async Task<string> BuildSystemPrompt()
    {
        // Lấy dữ liệu sản phẩm theo danh mục từ database
        var productsWithCategory = await _context.SanPhams
            .Include(p => p.DanhMuc)
            .Where(p => p.TrangThai == true)
            .Select(p => new { 
                p.TenSP, 
                p.DonGia, 
                p.MoTa, 
                p.CapDoCay,
                TenDanhMuc = p.DanhMuc != null ? p.DanhMuc.TenDanhMuc : "Khác"
            })
            .ToListAsync();

        // Nhóm sản phẩm theo danh mục
        var groupedProducts = productsWithCategory
            .GroupBy(p => p.TenDanhMuc)
            .OrderBy(g => g.Key);

        var menuContext = string.Join("\n\n", groupedProducts.Select(g => 
            $"📌 {g.Key.ToUpper()}:\n" + 
            string.Join("\n", g.Select(p => $"  - {p.TenSP}: {p.DonGia:N0}đ{(p.CapDoCay > 0 ? $" (cấp cay {p.CapDoCay})" : "")}{(string.IsNullOrEmpty(p.MoTa) ? "" : $" - {p.MoTa}")}"))));

        // Lấy mã giảm giá đang hoạt động
        var coupons = await _context.MaGiamGias
            .Where(c => c.TrangThai == true && c.NgayKetThuc >= DateTime.Now)
            .Select(c => new { c.MaCode, c.LoaiGiam, c.GiaTri, c.GiamToiDa, c.DonToiThieu, c.MoTa })
            .Take(10)
            .ToListAsync();

        var couponContext = coupons.Any() 
            ? string.Join("\n", coupons.Select(c => {
                var discount = c.LoaiGiam == "percent" 
                    ? $"Giảm {c.GiaTri}%{(c.GiamToiDa.HasValue ? $" (tối đa {c.GiamToiDa:N0}đ)" : "")}"
                    : c.LoaiGiam == "freeship" ? "Miễn phí ship" : $"Giảm {c.GiaTri:N0}đ";
                return $"- {c.MaCode}: {discount} cho đơn từ {c.DonToiThieu:N0}đ";
            }))
            : "Hiện chưa có mã giảm giá";

        // Lấy chi nhánh
        var branches = await _context.ChiNhanhs
            .Where(b => b.TrangThai == true)
            .Select(b => new { b.TenChiNhanh, b.DiaChi, b.SoDienThoai })
            .ToListAsync();

        var branchContext = branches.Any()
            ? string.Join("\n", branches.Select(b => $"- {b.TenChiNhanh}: {b.DiaChi} - ĐT: {b.SoDienThoai}"))
            : "123 Đường ABC, Quận 1, TP.HCM - Hotline: 0123 456 789";

        return $@"Bạn là SASIN - trợ lý ảo thông minh và thân thiện của nhà hàng Mỳ Cay Sasin.

🏪 THÔNG TIN NHÀ HÀNG:
- Tên: Mỳ Cay Sasin - Thương hiệu mì cay Hàn Quốc
- Giờ mở cửa: 10:00 - 22:00 hàng ngày
- Chi nhánh:
{branchContext}

🌶️ CẤP ĐỘ CAY (1-10) - CHỈ ÁP DỤNG CHO MÌ CAY:
- Cấp 1-2: Không cay, phù hợp trẻ em
- Cấp 3-4: Cay nhẹ
- Cấp 5-6: Cay vừa, phổ biến nhất
- Cấp 7-8: Cay nhiều
- Cấp 9-10: Siêu cay, thử thách

🍜 LOẠI NƯỚC DÙNG MÌ (CHỈ chọn khi gọi MÌ CAY):
- Kim Chi: Vị chua cay thanh mát, đặc trưng Hàn Quốc
- Soyum: Vị béo ngậy từ đậu nành, đậm đà
- Sincay: Vị cay nồng, đậm đà nhất

⚠️ QUAN TRỌNG - PHÂN BIỆT CÁC LOẠI MÌ:
• MÌ CAY: Cần chọn nước dùng (Kim Chi/Soyum/Sincay) + cấp độ cay (1-10)
• MÌ TƯƠNG ĐEN (Jajangmyeon): KHÔNG cay, KHÔNG chọn nước dùng - vị ngọt béo từ tương đen
• MÌ XÀO: KHÔNG cay, KHÔNG chọn nước dùng - món khô, đậm đà
• MÌ PHÔ MAI: KHÔNG cay, KHÔNG chọn nước dùng - vị béo ngậy phô mai
• MÌ TƯƠNG HÀN: KHÔNG cay, KHÔNG chọn nước dùng - vị đậm đà tương Hàn

🥤 NƯỚC UỐNG GIẢI KHÁT (đồ uống):
- Các loại nước ngọt, trà, nước ép... nằm trong danh mục GIẢI KHÁT
- Dùng để uống kèm bữa ăn, giải nhiệt

📋 THỰC ĐƠN THEO DANH MỤC:
{menuContext}

🎁 MÃ GIẢM GIÁ HIỆN TẠI:
{couponContext}

📝 QUY TẮC TRẢ LỜI:
1. Thân thiện, nhiệt tình như nhân viên thật
2. Trả lời ngắn gọn (2-4 câu), có emoji
3. Gợi ý món cụ thể với giá tiền
4. Phân biệt rõ: NƯỚC DÙNG MÌ (Kim Chi/Soyum/Sincay) khác với NƯỚC UỐNG (giải khát)
5. Khi khách hỏi về nước, hỏi rõ họ muốn nước dùng mì hay nước uống
6. Nếu không biết, hướng dẫn gọi hotline
7. Kết thúc bằng câu hỏi hoặc gợi ý

🍽️ GỢI Ý MÓN ĂN KÈM (RẤT QUAN TRỌNG):
Khi khách chọn món chính, LUÔN gợi ý món ăn kèm phù hợp:

• Nếu chọn MÌ CAY → Gợi ý: 
  - Khai vị: Cánh gà chiên, Xúc xích phô mai, Há cảo chiên
  - Topping thêm: Thịt bò, Hải sản, Phô mai
  - Nước uống: Trà đào, Nước gạo Hàn Quốc (giải cay)

• Nếu chọn MÌ TƯƠNG ĐEN → Gợi ý:
  - Khai vị: Tokbokki, Kimbap
  - Nước uống: Coca, Sprite

• Nếu chọn LẨU → Gợi ý:
  - Topping lẩu: Rau củ, Nấm, Hải sản, Mì/Miến
  - Khai vị: Cơm cuộn, Há cảo
  - Nước uống: Bia, Nước ngọt

• Nếu chọn TOKBOKKI → Gợi ý:
  - Ăn kèm: Kimbap, Cánh gà
  - Nước uống: Trà sữa

• Nếu chọn KHAI VỊ → Gợi ý thêm món chính

Cách gợi ý tự nhiên:
- ""Món này ăn kèm với [món] sẽ ngon hơn đó bạn! 😋""
- ""Bạn thêm [món] để bữa ăn trọn vẹn hơn nhé!""
- ""Nhiều khách thích gọi thêm [món] khi ăn món này đó!""

❌ KHÔNG:
- Bịa thông tin không có trong menu
- Nhầm lẫn nước dùng mì với nước uống giải khát
- Trả lời ngoài phạm vi nhà hàng
- Gợi ý quá nhiều món một lúc (chỉ 1-2 món)";
    }

    private string GetFallbackResponse(string message)
    {
        var lowerMessage = message.ToLower();
        
        if (lowerMessage.Contains("menu") || lowerMessage.Contains("thực đơn") || lowerMessage.Contains("món"))
            return "Chào bạn! 😊 Bạn có thể xem thực đơn đầy đủ tại trang Thực đơn nhé. Chúng tôi có mì cay, mì tương đen, lẩu và nhiều món ngon khác! 🍜";
        
        if (lowerMessage.Contains("giá") || lowerMessage.Contains("bao nhiêu"))
            return "Giá món ăn dao động từ 35,000đ - 250,000đ tùy món. Bạn xem chi tiết tại trang Thực đơn nhé! 💰";
        
        if (lowerMessage.Contains("cay"))
            return "Chúng tôi có 10 cấp độ cay! Cấp 1-2 không cay, 5-6 cay vừa, 9-10 siêu cay. Bạn thích cấp mấy? 🌶️";
        
        if (lowerMessage.Contains("địa chỉ") || lowerMessage.Contains("ở đâu"))
            return "Bạn có thể xem địa chỉ chi nhánh tại trang Giới thiệu. Hoặc gọi hotline để được hỗ trợ nhé! 📍";
        
        if (lowerMessage.Contains("đặt") || lowerMessage.Contains("order"))
            return "Bạn có thể đặt hàng trực tiếp trên website! Thêm món vào giỏ hàng và thanh toán nhé. 🛒";
        
        if (lowerMessage.Contains("khuyến mãi") || lowerMessage.Contains("giảm giá") || lowerMessage.Contains("mã"))
            return "Xem các ưu đãi mới nhất tại trang Ưu đãi nhé! Chúng tôi thường xuyên có mã giảm giá hấp dẫn. 🎁";

        return "Xin chào! 😊 Tôi là SASIN - trợ lý của Mỳ Cay Sasin. Tôi có thể giúp bạn xem thực đơn, tư vấn món ăn, hoặc hỗ trợ đặt hàng. Bạn cần gì ạ? 🍜";
    }

    [HttpGet("suggestions")]
    public IActionResult GetSuggestions()
    {
        var suggestions = new[]
        {
            "Gợi ý món best seller",
            "Tôi muốn ăn cay vừa", 
            "Combo cho 2 người",
            "Món nào không cay?",
            "Có mã giảm giá không?"
        };
        return Ok(suggestions);
    }
}

public class ChatRequest
{
    public string Message { get; set; } = "";
}
