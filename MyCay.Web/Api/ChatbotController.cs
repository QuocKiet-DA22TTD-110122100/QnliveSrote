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
            var apiKey = _config["Gemini:ApiKey"];
            if (string.IsNullOrEmpty(apiKey) || apiKey == "YOUR_GEMINI_API_KEY_HERE")
            {
                return Ok(new { reply = "Xin chào! Tôi là trợ lý ảo của Mỳ Cay Sasin. Hiện tại hệ thống đang được cấu hình. Bạn có thể xem thực đơn tại trang Thực đơn nhé! 🍜" });
            }

            // Get menu data for context
            var products = await _context.SanPhams
                .Where(p => p.TrangThai == true)
                .Select(p => new { p.TenSP, p.DonGia, p.MoTa, p.MaDM })
                .Take(50)
                .ToListAsync();

            var menuContext = string.Join("\n", products.Select(p => $"- {p.TenSP}: {p.DonGia:N0}đ - {p.MoTa}"));

            var systemPrompt = $@"Bạn là SASIN - trợ lý ảo thông minh và thân thiện của nhà hàng Mỳ Cay Sasin.

🏪 THÔNG TIN NHÀ HÀNG:
- Tên: Mỳ Cay Sasin - Thương hiệu mì cay Hàn Quốc hàng đầu
- Địa chỉ: 123 Đường ABC, Quận 1, TP.HCM
- Hotline: 0123 456 789
- Giờ mở cửa: 10:00 - 22:00 hàng ngày (kể cả lễ tết)
- Đặc trưng: Mì cay Hàn Quốc chính hiệu với 10 cấp độ cay

🌶️ GIẢI THÍCH CẤP ĐỘ CAY:
- Cấp 1-2: Không cay, phù hợp trẻ em và người không ăn cay
- Cấp 3-4: Cay nhẹ, hơi tê tê đầu lưỡi
- Cấp 5-6: Cay vừa, phù hợp đa số người Việt
- Cấp 7-8: Cay nhiều, dành cho người thích cay
- Cấp 9-10: Siêu cay, thử thách cho dân ghiền cay

🍜 LOẠI NƯỚC DÙNG:
- Kim Chi: Vị chua cay đặc trưng, thanh mát
- Soyum: Vị đậm đà, béo ngậy từ đậu nành
- Sincay: Vị cay nồng, đậm đà nhất

📋 DANH MỤC SẢN PHẨM:
1. MÌ CAY - Món signature, chọn cấp độ cay 1-10
2. MÌ TƯƠNG ĐEN - Không cay, vị ngọt béo
3. MÌ XÀO - Khô, đậm đà
4. MÓN KHÁC - Cơm trộn, Tokbokki, Miến
5. MÓN THÊM MÌ - Topping cho mì
6. COMBO - Tiết kiệm 10-20%
7. LẨU - Cho 2 người, đầy đủ topping
8. MÓN THÊM LẨU - Topping cho lẩu
9. KHAI VỊ - Ăn vặt, chờ món
10. GIẢI KHÁT - Nước uống

🔥 MÓN BEST SELLER:
- Mì Thập Cẩm No Nê: 77,000đ - Đầy đủ topping
- Mì Hải Sản: 62,000đ - Tôm, mực, cá viên
- Tokbokki Phô Mai Sasin: 59,000đ - Ngọt cay, phô mai kéo sợi
- Combo Bạn Thân (2 người): 159,000đ - Tiết kiệm nhất

💰 KHUYẾN MÃI HIỆN TẠI:
- SASIN10: Giảm 10% đơn từ 100k
- SASIN20: Giảm 20% đơn từ 200k
- FREESHIP: Miễn phí ship đơn từ 150k

📦 THỰC ĐƠN CHI TIẾT:
{menuContext}

📝 QUY TẮC TRẢ LỜI:
1. Luôn thân thiện, nhiệt tình như nhân viên thật
2. Trả lời ngắn gọn, dễ hiểu (tối đa 3-4 câu)
3. Gợi ý món cụ thể với giá tiền
4. Hỏi thêm về khẩu vị nếu cần (cay/không cay, số người)
5. Sử dụng emoji phù hợp 🍜🌶️😋
6. Nếu không biết, hướng dẫn gọi hotline
7. Luôn kết thúc bằng câu hỏi hoặc gợi ý tiếp theo

❌ KHÔNG LÀM:
- Không bịa thông tin không có trong menu
- Không trả lời câu hỏi ngoài phạm vi nhà hàng
- Không nói xấu đối thủ";

            // Ví dụ hội thoại mẫu để AI học cách trả lời
            var exampleConversations = @"
VÍ DỤ HỘI THOẠI:

Khách: Tôi muốn ăn cay vừa
Sasin: Chào bạn! 😊 Với cấp độ cay vừa (5-6), mình gợi ý:
• Mì Hải Sản: 62,000đ - tôm, mực, cá viên 🦐
• Mì Bò Mỹ: 59,000đ - thịt bò mềm, đậm đà 🥩
Bạn thích hải sản hay thịt bò hơn ạ?

Khách: Combo cho 2 người
Sasin: Tuyệt vời! 👫 Combo 2 người hot nhất:
• Combo Bạn Thân: 159,000đ - 2 mì cay + 1 khai vị (tiết kiệm 30k!)
• Combo No Căng: 179,000đ - 2 mì cay + Tokbokki phô mai
• Combo Lẩu 2 Người: 225,000đ - lẩu + khai vị
Bạn muốn ăn mì hay lẩu ạ? 🍜

Khách: Món nào không cay?
Sasin: Có nhiều món không cay cho bạn nè! 😋
• Mì Tương Đen: 55-69k - ngọt béo, thơm mè
• Mì Xào: 62-69k - khô, đậm đà
• Tokbokki Phô Mai: 59k - ngọt cay nhẹ, phô mai kéo sợi
Hoặc chọn mì cay cấp 1-2 cũng gần như không cay đâu ạ!

Khách: Best seller là gì?
Sasin: Đây là top món được yêu thích nhất! 🔥
1. Mì Thập Cẩm No Nê: 77k - full topping, no. 1 bán chạy
2. Mì Hải Sản: 62k - tôm mực tươi ngon
3. Tokbokki Phô Mai: 59k - phô mai kéo sợi siêu ngon
Bạn muốn thử món nào? 😊";

            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={apiKey}";

            var requestBody = new
            {
                contents = new[]
                {
                    new { role = "user", parts = new[] { new { text = systemPrompt + "\n" + exampleConversations + "\n\nBây giờ hãy trả lời khách hàng:\nKhách: " + request.Message } } }
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
            else
            {
                return Ok(new { reply = "Xin lỗi, tôi đang gặp sự cố. Bạn có thể xem thực đơn hoặc liên hệ nhân viên để được hỗ trợ nhé! 🙏" });
            }
        }
        catch (Exception ex)
        {
            return Ok(new { reply = "Xin lỗi, có lỗi xảy ra. Vui lòng thử lại sau! 🙏" });
        }
    }

    [HttpGet("suggestions")]
    public async Task<IActionResult> GetSuggestions()
    {
        var suggestions = new[]
        {
            "Gợi ý món best seller",
            "Tôi muốn ăn cay vừa",
            "Combo cho 2 người",
            "Món nào không cay?",
            "Giới thiệu về lẩu"
        };
        return Ok(suggestions);
    }
}

public class ChatRequest
{
    public string Message { get; set; } = "";
}
