using System.Text.RegularExpressions;

namespace MyCay.Tests;

/// <summary>
/// Unit Tests cho Validation - Kiểm tra các quy tắc validate dữ liệu
/// </summary>
public class ValidationTests
{
    #region Phone Number Validation

    [Theory]
    [InlineData("0901234567", true)]
    [InlineData("0912345678", true)]
    [InlineData("0321234567", true)]
    [InlineData("84901234567", true)]
    [InlineData("+84901234567", true)]
    [InlineData("123456789", false)]  // Thiếu số 0 đầu
    [InlineData("09012345", false)]   // Thiếu số
    [InlineData("090123456789", false)] // Thừa số
    [InlineData("abcdefghij", false)] // Không phải số
    public void ValidatePhoneNumber_ReturnsExpectedResult(string phone, bool expected)
    {
        // Arrange
        var phonePattern = @"^(\+84|84|0)(3|5|7|8|9)[0-9]{8}$";

        // Act
        var isValid = Regex.IsMatch(phone, phonePattern);

        // Assert
        Assert.Equal(expected, isValid);
    }

    #endregion

    #region Email Validation

    [Theory]
    [InlineData("test@example.com", true)]
    [InlineData("user.name@domain.vn", true)]
    [InlineData("user+tag@gmail.com", true)]
    [InlineData("invalid-email", false)]
    [InlineData("@nodomain.com", false)]
    [InlineData("noat.com", false)]
    [InlineData("", false)]
    public void ValidateEmail_ReturnsExpectedResult(string email, bool expected)
    {
        // Arrange
        var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        // Act
        var isValid = !string.IsNullOrEmpty(email) && Regex.IsMatch(email, emailPattern);

        // Assert
        Assert.Equal(expected, isValid);
    }

    #endregion

    #region Order Code Validation

    [Theory]
    [InlineData("DH202412260001", true)]
    [InlineData("DH20241226123456", true)]
    [InlineData("DH123", false)]  // Quá ngắn
    [InlineData("AB202412260001", false)]  // Không bắt đầu bằng DH
    [InlineData("", false)]
    public void ValidateOrderCode_ReturnsExpectedResult(string orderCode, bool expected)
    {
        // Arrange - Mã đơn hàng phải bắt đầu bằng "DH" và có ít nhất 14 ký tự
        
        // Act
        var isValid = !string.IsNullOrEmpty(orderCode) 
            && orderCode.StartsWith("DH") 
            && orderCode.Length >= 14;

        // Assert
        Assert.Equal(expected, isValid);
    }

    #endregion

    #region Price Validation

    [Theory]
    [InlineData(0, false)]
    [InlineData(-1000, false)]
    [InlineData(1000, true)]
    [InlineData(55000, true)]
    [InlineData(1000000, true)]
    public void ValidatePrice_MustBePositive(decimal price, bool expected)
    {
        // Act
        var isValid = price > 0;

        // Assert
        Assert.Equal(expected, isValid);
    }

    #endregion

    #region Quantity Validation

    [Theory]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    [InlineData(1, true)]
    [InlineData(10, true)]
    [InlineData(100, false)]  // Giới hạn tối đa 99
    public void ValidateQuantity_MustBeInRange(int quantity, bool expected)
    {
        // Act - Số lượng phải từ 1 đến 99
        var isValid = quantity >= 1 && quantity <= 99;

        // Assert
        Assert.Equal(expected, isValid);
    }

    #endregion

    #region Spicy Level Validation

    [Theory]
    [InlineData(-1, false)]
    [InlineData(0, true)]
    [InlineData(1, true)]
    [InlineData(3, true)]
    [InlineData(5, true)]
    [InlineData(7, true)]
    [InlineData(8, false)]
    [InlineData(10, false)]
    public void ValidateSpicyLevel_MustBe0To7(int level, bool expected)
    {
        // Act
        var isValid = level >= 0 && level <= 7;

        // Assert
        Assert.Equal(expected, isValid);
    }

    #endregion

    #region Coupon Code Validation

    [Theory]
    [InlineData("GIAM10", true)]
    [InlineData("FREESHIP", true)]
    [InlineData("SALE50K", true)]
    [InlineData("AB", false)]  // Quá ngắn (< 3 ký tự)
    [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", false)]  // Quá dài (> 20 ký tự)
    [InlineData("giam 10", false)]  // Có khoảng trắng
    [InlineData("", false)]
    public void ValidateCouponCode_ReturnsExpectedResult(string code, bool expected)
    {
        // Arrange - Mã giảm giá: 3-20 ký tự, không có khoảng trắng
        var pattern = @"^[A-Za-z0-9]{3,20}$";

        // Act
        var isValid = !string.IsNullOrEmpty(code) && Regex.IsMatch(code, pattern);

        // Assert
        Assert.Equal(expected, isValid);
    }

    #endregion

    #region Password Validation

    [Theory]
    [InlineData("123456", true)]  // Tối thiểu 6 ký tự
    [InlineData("password123", true)]
    [InlineData("MyP@ssw0rd!", true)]
    [InlineData("12345", false)]  // Quá ngắn
    [InlineData("", false)]
    public void ValidatePassword_MinLength6(string password, bool expected)
    {
        // Act
        var isValid = !string.IsNullOrEmpty(password) && password.Length >= 6;

        // Assert
        Assert.Equal(expected, isValid);
    }

    #endregion

    #region Address Validation

    [Theory]
    [InlineData("123 Đường ABC, Phường 1, Quận 1, TP.HCM", true)]
    [InlineData("Số 45 Nguyễn Huệ", true)]
    [InlineData("AB", false)]  // Quá ngắn
    [InlineData("", false)]
    public void ValidateAddress_MinLength10(string address, bool expected)
    {
        // Act - Địa chỉ tối thiểu 10 ký tự
        var isValid = !string.IsNullOrEmpty(address) && address.Length >= 10;

        // Assert
        Assert.Equal(expected, isValid);
    }

    #endregion
}
