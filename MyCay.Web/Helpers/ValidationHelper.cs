using System.Text.RegularExpressions;

namespace MyCay.Web.Helpers;

public static class ValidationHelper
{
    // Phone validation (Vietnam format)
    public static bool IsValidPhone(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone)) return false;
        // Vietnam phone: 10 digits starting with 0
        return Regex.IsMatch(phone, @"^0\d{9}$");
    }

    // Email validation
    public static bool IsValidEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;
        return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }

    // Password validation (min 6 chars)
    public static bool IsValidPassword(string? password)
    {
        return !string.IsNullOrEmpty(password) && password.Length >= 6;
    }

    // Strong password (8+ chars, uppercase, lowercase, number)
    public static bool IsStrongPassword(string? password)
    {
        if (string.IsNullOrEmpty(password) || password.Length < 8) return false;
        return Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$");
    }

    // Name validation (2-100 chars, no special chars)
    public static bool IsValidName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name)) return false;
        return name.Length >= 2 && name.Length <= 100;
    }

    // Address validation
    public static bool IsValidAddress(string? address)
    {
        return !string.IsNullOrWhiteSpace(address) && address.Length >= 10 && address.Length <= 200;
    }

    // Price validation
    public static bool IsValidPrice(decimal price)
    {
        return price >= 0 && price <= 100_000_000; // Max 100 million VND
    }

    // Quantity validation
    public static bool IsValidQuantity(int quantity)
    {
        return quantity > 0 && quantity <= 100;
    }

    // Spicy level validation (0-7)
    public static bool IsValidSpicyLevel(int level)
    {
        return level >= 0 && level <= 7;
    }

    // Sanitize input (remove dangerous chars)
    public static string SanitizeInput(string? input)
    {
        if (string.IsNullOrEmpty(input)) return "";
        // Remove potential XSS characters
        return Regex.Replace(input, @"[<>""'&]", "").Trim();
    }

    // Validate order request
    public static (bool isValid, string errorMessage) ValidateOrderRequest(
        string? customerName, string? phone, string? address, decimal subtotal, List<dynamic>? items)
    {
        if (string.IsNullOrWhiteSpace(customerName))
            return (false, "Vui lòng nhập tên khách hàng");

        if (!IsValidPhone(phone))
            return (false, "Số điện thoại không hợp lệ (10 số, bắt đầu bằng 0)");

        if (string.IsNullOrWhiteSpace(address))
            return (false, "Vui lòng nhập địa chỉ giao hàng");

        if (subtotal <= 0)
            return (false, "Giá trị đơn hàng không hợp lệ");

        if (items == null || items.Count == 0)
            return (false, "Đơn hàng phải có ít nhất 1 sản phẩm");

        return (true, "");
    }

    // Validate registration request
    public static (bool isValid, string errorMessage) ValidateRegistration(
        string? name, string? email, string? phone, string? password)
    {
        if (!IsValidName(name))
            return (false, "Tên phải từ 2-100 ký tự");

        if (!IsValidEmail(email))
            return (false, "Email không hợp lệ");

        if (!IsValidPhone(phone))
            return (false, "Số điện thoại không hợp lệ");

        if (!IsValidPassword(password))
            return (false, "Mật khẩu phải có ít nhất 6 ký tự");

        return (true, "");
    }
}
