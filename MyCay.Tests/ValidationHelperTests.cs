using MyCay.Web.Helpers;

namespace MyCay.Tests;

public class ValidationHelperTests
{
    #region Phone Validation
    [Theory]
    [InlineData("0912345678", true)]
    [InlineData("0123456789", true)]
    [InlineData("0987654321", true)]
    [InlineData("1234567890", false)]  // Not starting with 0
    [InlineData("091234567", false)]   // 9 digits
    [InlineData("09123456789", false)] // 11 digits
    [InlineData("", false)]
    [InlineData(null, false)]
    [InlineData("abc", false)]
    public void IsValidPhone_ReturnsExpectedResult(string? phone, bool expected)
    {
        Assert.Equal(expected, ValidationHelper.IsValidPhone(phone));
    }
    #endregion

    #region Email Validation
    [Theory]
    [InlineData("test@test.com", true)]
    [InlineData("user@domain.vn", true)]
    [InlineData("a.b@c.d", true)]
    [InlineData("invalid", false)]
    [InlineData("@test.com", false)]
    [InlineData("test@", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void IsValidEmail_ReturnsExpectedResult(string? email, bool expected)
    {
        Assert.Equal(expected, ValidationHelper.IsValidEmail(email));
    }
    #endregion

    #region Password Validation
    [Theory]
    [InlineData("123456", true)]
    [InlineData("password", true)]
    [InlineData("12345", false)]  // Too short
    [InlineData("", false)]
    [InlineData(null, false)]
    public void IsValidPassword_ReturnsExpectedResult(string? password, bool expected)
    {
        Assert.Equal(expected, ValidationHelper.IsValidPassword(password));
    }

    [Theory]
    [InlineData("Password1", true)]
    [InlineData("Abc12345", true)]
    [InlineData("password1", false)]  // No uppercase
    [InlineData("PASSWORD1", false)]  // No lowercase
    [InlineData("Password", false)]   // No number
    [InlineData("Pass1", false)]      // Too short
    public void IsStrongPassword_ReturnsExpectedResult(string? password, bool expected)
    {
        Assert.Equal(expected, ValidationHelper.IsStrongPassword(password));
    }
    #endregion

    #region Name Validation
    [Theory]
    [InlineData("Nguyễn Văn A", true)]
    [InlineData("AB", true)]
    [InlineData("A", false)]  // Too short
    [InlineData("", false)]
    [InlineData(null, false)]
    public void IsValidName_ReturnsExpectedResult(string? name, bool expected)
    {
        Assert.Equal(expected, ValidationHelper.IsValidName(name));
    }
    #endregion

    #region Address Validation
    [Theory]
    [InlineData("123 Đường ABC, Quận 1, TP.HCM", true)]
    [InlineData("Short", false)]  // Too short
    [InlineData("", false)]
    [InlineData(null, false)]
    public void IsValidAddress_ReturnsExpectedResult(string? address, bool expected)
    {
        Assert.Equal(expected, ValidationHelper.IsValidAddress(address));
    }
    #endregion

    #region Price Validation
    [Theory]
    [InlineData(0, true)]
    [InlineData(50000, true)]
    [InlineData(100000000, true)]
    [InlineData(-1, false)]
    [InlineData(100000001, false)]
    public void IsValidPrice_ReturnsExpectedResult(decimal price, bool expected)
    {
        Assert.Equal(expected, ValidationHelper.IsValidPrice(price));
    }
    #endregion

    #region Quantity Validation
    [Theory]
    [InlineData(1, true)]
    [InlineData(50, true)]
    [InlineData(100, true)]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    [InlineData(101, false)]
    public void IsValidQuantity_ReturnsExpectedResult(int quantity, bool expected)
    {
        Assert.Equal(expected, ValidationHelper.IsValidQuantity(quantity));
    }
    #endregion

    #region Spicy Level Validation
    [Theory]
    [InlineData(0, true)]
    [InlineData(3, true)]
    [InlineData(7, true)]
    [InlineData(-1, false)]
    [InlineData(8, false)]
    public void IsValidSpicyLevel_ReturnsExpectedResult(int level, bool expected)
    {
        Assert.Equal(expected, ValidationHelper.IsValidSpicyLevel(level));
    }
    #endregion

    #region Sanitize Input
    [Theory]
    [InlineData("Hello World", "Hello World")]
    [InlineData("<script>alert('xss')</script>", "scriptalert(xss)/script")]
    [InlineData("Test\"Quote'", "TestQuote")]
    [InlineData("  Trim  ", "Trim")]
    [InlineData(null, "")]
    [InlineData("", "")]
    public void SanitizeInput_ReturnsExpectedResult(string? input, string expected)
    {
        Assert.Equal(expected, ValidationHelper.SanitizeInput(input));
    }
    #endregion

    #region Registration Validation
    [Fact]
    public void ValidateRegistration_WithValidData_ReturnsTrue()
    {
        var (isValid, _) = ValidationHelper.ValidateRegistration(
            "Nguyễn Văn A", "test@test.com", "0912345678", "password123");
        Assert.True(isValid);
    }

    [Fact]
    public void ValidateRegistration_WithInvalidName_ReturnsFalse()
    {
        var (isValid, message) = ValidationHelper.ValidateRegistration(
            "A", "test@test.com", "0912345678", "password123");
        Assert.False(isValid);
        Assert.Contains("Tên", message);
    }

    [Fact]
    public void ValidateRegistration_WithInvalidEmail_ReturnsFalse()
    {
        var (isValid, message) = ValidationHelper.ValidateRegistration(
            "Nguyễn Văn A", "invalid", "0912345678", "password123");
        Assert.False(isValid);
        Assert.Contains("Email", message);
    }

    [Fact]
    public void ValidateRegistration_WithInvalidPhone_ReturnsFalse()
    {
        var (isValid, message) = ValidationHelper.ValidateRegistration(
            "Nguyễn Văn A", "test@test.com", "123", "password123");
        Assert.False(isValid);
        Assert.Contains("điện thoại", message);
    }

    [Fact]
    public void ValidateRegistration_WithInvalidPassword_ReturnsFalse()
    {
        var (isValid, message) = ValidationHelper.ValidateRegistration(
            "Nguyễn Văn A", "test@test.com", "0912345678", "12345");
        Assert.False(isValid);
        Assert.Contains("Mật khẩu", message);
    }
    #endregion
}
