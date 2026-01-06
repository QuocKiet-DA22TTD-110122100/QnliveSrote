using MyCay.Web.Services;
using MyCay.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Moq;

namespace MyCay.Tests;

public class EmailServiceTests
{
    private readonly IEmailService _emailService;
    private readonly Mock<ILogger<EmailService>> _loggerMock;

    public EmailServiceTests()
    {
        var settings = new Dictionary<string, string?>
        {
            {"Email:SmtpHost", "smtp.gmail.com"},
            {"Email:SmtpPort", "587"},
            {"Email:FromName", "Mỳ Cay Sasin Test"}
        };
        var config = new ConfigurationBuilder().AddInMemoryCollection(settings).Build();
        
        _loggerMock = new Mock<ILogger<EmailService>>();
        var envMock = new Mock<IWebHostEnvironment>();
        envMock.Setup(e => e.EnvironmentName).Returns("Development");
        
        _emailService = new EmailService(config, _loggerMock.Object, envMock.Object);
    }

    [Fact]
    public async Task SendOrderConfirmationAsync_InDevMode_ReturnsTrue()
    {
        // Arrange
        var order = new DonHang
        {
            MaDH = 1,
            MaDHCode = "DH001",
            TongTien = 150000,
            DiaChiGiao = "123 Test Street",
            ChiTietDonHangs = new List<ChiTietDonHang>
            {
                new() { TenSP = "Mỳ cay cấp 3", SoLuong = 2, DonGia = 50000 },
                new() { TenSP = "Trà đào", SoLuong = 1, DonGia = 25000 }
            }
        };

        // Act
        var result = await _emailService.SendOrderConfirmationAsync(order, "test@test.com", "Nguyễn Văn A");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task SendOrderStatusUpdateAsync_InDevMode_ReturnsTrue()
    {
        // Arrange
        var order = new DonHang { MaDH = 1, MaDHCode = "DH001" };

        // Act
        var result = await _emailService.SendOrderStatusUpdateAsync(order, "test@test.com", "confirmed");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task SendWelcomeEmailAsync_InDevMode_ReturnsTrue()
    {
        // Act
        var result = await _emailService.SendWelcomeEmailAsync("newuser@test.com", "Trần Thị B");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task SendPasswordResetAsync_InDevMode_ReturnsTrue()
    {
        // Act
        var result = await _emailService.SendPasswordResetAsync("user@test.com", "reset-token-123");

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData("confirmed")]
    [InlineData("preparing")]
    [InlineData("shipping")]
    [InlineData("delivered")]
    [InlineData("cancelled")]
    public async Task SendOrderStatusUpdateAsync_WithDifferentStatuses_ReturnsTrue(string status)
    {
        // Arrange
        var order = new DonHang { MaDH = 1, MaDHCode = "DH001" };

        // Act
        var result = await _emailService.SendOrderStatusUpdateAsync(order, "test@test.com", status);

        // Assert
        Assert.True(result);
    }
}
