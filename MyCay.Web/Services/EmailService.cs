using System.Net;
using System.Net.Mail;
using MyCay.Domain.Entities;

namespace MyCay.Web.Services;

public interface IEmailService
{
    Task<bool> SendOrderConfirmationAsync(DonHang order, string customerEmail, string customerName);
    Task<bool> SendOrderStatusUpdateAsync(DonHang order, string customerEmail, string newStatus);
    Task<bool> SendWelcomeEmailAsync(string email, string name);
    Task<bool> SendPasswordResetAsync(string email, string resetToken);
}

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;
    private readonly ILogger<EmailService> _logger;
    private readonly bool _isDevelopment;

    public EmailService(IConfiguration config, ILogger<EmailService> logger, IWebHostEnvironment env)
    {
        _config = config;
        _logger = logger;
        _isDevelopment = env.IsDevelopment();
    }

    public async Task<bool> SendOrderConfirmationAsync(DonHang order, string customerEmail, string customerName)
    {
        var subject = $"[Mỳ Cay Sasin] Xác nhận đơn hàng #{order.MaDHCode ?? $"DH{order.MaDH}"}";
        var body = BuildOrderConfirmationBody(order, customerName);
        return await SendEmailAsync(customerEmail, subject, body);
    }

    public async Task<bool> SendOrderStatusUpdateAsync(DonHang order, string customerEmail, string newStatus)
    {
        var statusText = newStatus switch
        {
            "confirmed" => "Đã xác nhận",
            "preparing" => "Đang chuẩn bị",
            "shipping" => "Đang giao hàng",
            "delivered" => "Đã giao hàng",
            "cancelled" => "Đã hủy",
            _ => newStatus
        };

        var subject = $"[Mỳ Cay Sasin] Cập nhật đơn hàng #{order.MaDHCode ?? $"DH{order.MaDH}"} - {statusText}";
        var body = $@"
<html>
<body style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
    <div style='background: linear-gradient(135deg, #ff6b35, #f7931e); padding: 20px; text-align: center;'>
        <h1 style='color: white; margin: 0;'>🍜 Mỳ Cay Sasin</h1>
    </div>
    <div style='padding: 20px; background: #fff;'>
        <h2>Cập nhật trạng thái đơn hàng</h2>
        <p>Đơn hàng <strong>#{order.MaDHCode ?? $"DH{order.MaDH}"}</strong> của bạn đã được cập nhật:</p>
        <div style='background: #f8f9fa; padding: 15px; border-radius: 8px; text-align: center;'>
            <span style='font-size: 24px; color: #ff6b35; font-weight: bold;'>{statusText}</span>
        </div>
        <p style='margin-top: 20px;'>Cảm ơn bạn đã tin tưởng Mỳ Cay Sasin!</p>
    </div>
    <div style='background: #333; color: #fff; padding: 15px; text-align: center; font-size: 12px;'>
        © 2024 Mỳ Cay Sasin. Hotline: 1900-xxxx
    </div>
</body>
</html>";

        return await SendEmailAsync(customerEmail, subject, body);
    }

    public async Task<bool> SendWelcomeEmailAsync(string email, string name)
    {
        var subject = "Chào mừng bạn đến với Mỳ Cay Sasin! 🍜";
        var body = $@"
<html>
<body style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
    <div style='background: linear-gradient(135deg, #ff6b35, #f7931e); padding: 20px; text-align: center;'>
        <h1 style='color: white; margin: 0;'>🍜 Mỳ Cay Sasin</h1>
    </div>
    <div style='padding: 20px; background: #fff;'>
        <h2>Xin chào {name}! 👋</h2>
        <p>Cảm ơn bạn đã đăng ký tài khoản tại Mỳ Cay Sasin.</p>
        <p>Bạn có thể bắt đầu đặt món ngay bây giờ với nhiều ưu đãi hấp dẫn!</p>
        <div style='text-align: center; margin: 20px 0;'>
            <a href='http://localhost:5267/ThucDon' 
               style='background: #ff6b35; color: white; padding: 12px 30px; text-decoration: none; border-radius: 25px; font-weight: bold;'>
                Xem Thực Đơn
            </a>
        </div>
    </div>
    <div style='background: #333; color: #fff; padding: 15px; text-align: center; font-size: 12px;'>
        © 2024 Mỳ Cay Sasin. Hotline: 1900-xxxx
    </div>
</body>
</html>";

        return await SendEmailAsync(email, subject, body);
    }

    public async Task<bool> SendPasswordResetAsync(string email, string resetToken)
    {
        var subject = "[Mỳ Cay Sasin] Đặt lại mật khẩu";
        var resetLink = $"http://localhost:5267/ResetPassword?token={resetToken}";
        var body = $@"
<html>
<body style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
    <div style='background: linear-gradient(135deg, #ff6b35, #f7931e); padding: 20px; text-align: center;'>
        <h1 style='color: white; margin: 0;'>🍜 Mỳ Cay Sasin</h1>
    </div>
    <div style='padding: 20px; background: #fff;'>
        <h2>Đặt lại mật khẩu</h2>
        <p>Bạn đã yêu cầu đặt lại mật khẩu. Nhấn vào nút bên dưới để tiếp tục:</p>
        <div style='text-align: center; margin: 20px 0;'>
            <a href='{resetLink}' 
               style='background: #ff6b35; color: white; padding: 12px 30px; text-decoration: none; border-radius: 25px; font-weight: bold;'>
                Đặt lại mật khẩu
            </a>
        </div>
        <p style='color: #666; font-size: 12px;'>Link này sẽ hết hạn sau 1 giờ. Nếu bạn không yêu cầu đặt lại mật khẩu, vui lòng bỏ qua email này.</p>
    </div>
    <div style='background: #333; color: #fff; padding: 15px; text-align: center; font-size: 12px;'>
        © 2024 Mỳ Cay Sasin. Hotline: 1900-xxxx
    </div>
</body>
</html>";

        return await SendEmailAsync(email, subject, body);
    }

    private string BuildOrderConfirmationBody(DonHang order, string customerName)
    {
        var itemsHtml = "";
        decimal total = 0;

        if (order.ChiTietDonHangs != null)
        {
            foreach (var item in order.ChiTietDonHangs)
            {
                var itemTotal = item.DonGia * item.SoLuong;
                total += itemTotal;
                itemsHtml += $@"
                <tr>
                    <td style='padding: 10px; border-bottom: 1px solid #eee;'>{item.TenSP ?? "Món ăn"}</td>
                    <td style='padding: 10px; border-bottom: 1px solid #eee; text-align: center;'>{item.SoLuong}</td>
                    <td style='padding: 10px; border-bottom: 1px solid #eee; text-align: right;'>{item.DonGia:N0}đ</td>
                    <td style='padding: 10px; border-bottom: 1px solid #eee; text-align: right;'>{itemTotal:N0}đ</td>
                </tr>";
            }
        }

        return $@"
<html>
<body style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
    <div style='background: linear-gradient(135deg, #ff6b35, #f7931e); padding: 20px; text-align: center;'>
        <h1 style='color: white; margin: 0;'>🍜 Mỳ Cay Sasin</h1>
    </div>
    <div style='padding: 20px; background: #fff;'>
        <h2>Xác nhận đơn hàng #{order.MaDHCode ?? $"DH{order.MaDH}"}</h2>
        <p>Xin chào <strong>{customerName}</strong>,</p>
        <p>Cảm ơn bạn đã đặt hàng tại Mỳ Cay Sasin! Đơn hàng của bạn đã được tiếp nhận.</p>
        
        <h3 style='color: #ff6b35;'>Chi tiết đơn hàng</h3>
        <table style='width: 100%; border-collapse: collapse;'>
            <thead>
                <tr style='background: #f8f9fa;'>
                    <th style='padding: 10px; text-align: left;'>Món</th>
                    <th style='padding: 10px; text-align: center;'>SL</th>
                    <th style='padding: 10px; text-align: right;'>Đơn giá</th>
                    <th style='padding: 10px; text-align: right;'>Thành tiền</th>
                </tr>
            </thead>
            <tbody>
                {itemsHtml}
            </tbody>
            <tfoot>
                <tr style='font-weight: bold; background: #fff3e0;'>
                    <td colspan='3' style='padding: 10px; text-align: right;'>Tổng cộng:</td>
                    <td style='padding: 10px; text-align: right; color: #ff6b35;'>{order.TongTien:N0}đ</td>
                </tr>
            </tfoot>
        </table>

        <h3 style='color: #ff6b35; margin-top: 20px;'>Thông tin giao hàng</h3>
        <p><strong>Địa chỉ:</strong> {order.DiaChiGiao}</p>
        <p><strong>Ghi chú:</strong> {order.GhiChu ?? "Không có"}</p>
        
        <p style='margin-top: 20px;'>Chúng tôi sẽ liên hệ với bạn sớm nhất!</p>
    </div>
    <div style='background: #333; color: #fff; padding: 15px; text-align: center; font-size: 12px;'>
        © 2024 Mỳ Cay Sasin. Hotline: 1900-xxxx
    </div>
</body>
</html>";
    }

    private async Task<bool> SendEmailAsync(string to, string subject, string body)
    {
        try
        {
            // In development mode, just log the email
            if (_isDevelopment)
            {
                _logger.LogInformation("📧 [DEV MODE] Email would be sent:");
                _logger.LogInformation("   To: {To}", to);
                _logger.LogInformation("   Subject: {Subject}", subject);
                _logger.LogInformation("   Body length: {Length} chars", body.Length);
                return true;
            }

            // Production: Send via SMTP
            var smtpHost = _config["Email:SmtpHost"] ?? "smtp.gmail.com";
            var smtpPort = int.Parse(_config["Email:SmtpPort"] ?? "587");
            var smtpUser = _config["Email:SmtpUser"];
            var smtpPass = _config["Email:SmtpPassword"];
            var fromEmail = _config["Email:FromEmail"] ?? smtpUser;
            var fromName = _config["Email:FromName"] ?? "Mỳ Cay Sasin";

            if (string.IsNullOrEmpty(smtpUser) || string.IsNullOrEmpty(smtpPass))
            {
                _logger.LogWarning("⚠️ SMTP credentials not configured. Email not sent.");
                return false;
            }

            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUser, smtpPass),
                EnableSsl = true
            };

            var message = new MailMessage
            {
                From = new MailAddress(fromEmail!, fromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            message.To.Add(to);

            await client.SendMailAsync(message);
            _logger.LogInformation("✅ Email sent to {To}: {Subject}", to, subject);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Failed to send email to {To}", to);
            return false;
        }
    }
}
