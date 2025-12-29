using MyCay.Domain.Entities;

namespace MyCay.Tests;

/// <summary>
/// Unit Tests cho các Entity - Kiểm tra khởi tạo và thuộc tính
/// </summary>
public class EntityTests
{
    #region SanPham Tests

    [Fact]
    public void SanPham_Create_WithValidData_Success()
    {
        // Arrange & Act
        var sanPham = new SanPham
        {
            MaSP = 1,
            MaSPCode = "SP001",
            TenSP = "Mì Cay Hải Sản",
            MoTa = "Mì cay với hải sản tươi ngon",
            DonGia = 55000,
            CapDoCay = 3,
            TrangThai = true
        };

        // Assert
        Assert.Equal(1, sanPham.MaSP);
        Assert.Equal("SP001", sanPham.MaSPCode);
        Assert.Equal("Mì Cay Hải Sản", sanPham.TenSP);
        Assert.Equal(55000, sanPham.DonGia);
        Assert.Equal(3, sanPham.CapDoCay);
        Assert.True(sanPham.TrangThai);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(5)]
    [InlineData(7)]
    public void SanPham_CapDoCay_ValidRange(int capDoCay)
    {
        // Arrange & Act
        var sanPham = new SanPham { CapDoCay = capDoCay };

        // Assert
        Assert.Equal(capDoCay, sanPham.CapDoCay);
        Assert.InRange(sanPham.CapDoCay, 0, 7);
    }

    #endregion

    #region DonHang Tests

    [Fact]
    public void DonHang_Create_WithValidData_Success()
    {
        // Arrange & Act
        var donHang = new DonHang
        {
            MaDH = 1,
            MaDHCode = "DH202412260001",
            TenKhach = "Nguyễn Văn A",
            SDTKhach = "0901234567",
            DiaChiGiao = "123 Đường ABC, Quận 1",
            TongTien = 150000,
            TrangThai = "Chờ xác nhận",
            NgayDat = DateTime.Now
        };

        // Assert
        Assert.Equal("DH202412260001", donHang.MaDHCode);
        Assert.Equal("Nguyễn Văn A", donHang.TenKhach);
        Assert.Equal(150000, donHang.TongTien);
        Assert.Equal("Chờ xác nhận", donHang.TrangThai);
    }

    [Theory]
    [InlineData("Chờ xác nhận")]
    [InlineData("Đang chuẩn bị")]
    [InlineData("Đang giao")]
    [InlineData("Hoàn thành")]
    [InlineData("Đã hủy")]
    public void DonHang_TrangThai_ValidValues(string trangThai)
    {
        // Arrange & Act
        var donHang = new DonHang { TrangThai = trangThai };

        // Assert
        Assert.Equal(trangThai, donHang.TrangThai);
    }

    #endregion

    #region KhachHang Tests

    [Fact]
    public void KhachHang_Create_WithValidData_Success()
    {
        // Arrange & Act
        var khachHang = new KhachHang
        {
            MaKH = 1,
            HoTen = "Trần Thị B",
            Email = "tranthib@email.com",
            SDT = "0912345678",
            DiaChi = "456 Đường XYZ",
            DiemTichLuy = 100,
            TrangThai = true
        };

        // Assert
        Assert.Equal("Trần Thị B", khachHang.HoTen);
        Assert.Equal("tranthib@email.com", khachHang.Email);
        Assert.Equal(100, khachHang.DiemTichLuy);
    }

    [Fact]
    public void KhachHang_DiemTichLuy_DefaultIsZero()
    {
        // Arrange & Act
        var khachHang = new KhachHang();

        // Assert
        Assert.Equal(0, khachHang.DiemTichLuy);
    }

    #endregion

    #region MaGiamGia Tests

    [Fact]
    public void MaGiamGia_Create_PercentType_Success()
    {
        // Arrange & Act
        var maGiamGia = new MaGiamGia
        {
            MaMGG = 1,
            MaCode = "GIAM10",
            MoTa = "Giảm 10%",
            LoaiGiam = "percent",
            GiaTri = 10,
            GiamToiDa = 50000,
            DonToiThieu = 100000,
            TrangThai = true
        };

        // Assert
        Assert.Equal("GIAM10", maGiamGia.MaCode);
        Assert.Equal("percent", maGiamGia.LoaiGiam);
        Assert.Equal(10, maGiamGia.GiaTri);
    }

    [Fact]
    public void MaGiamGia_Create_FixedType_Success()
    {
        // Arrange & Act
        var maGiamGia = new MaGiamGia
        {
            MaCode = "GIAM20K",
            LoaiGiam = "fixed",
            GiaTri = 20000
        };

        // Assert
        Assert.Equal("fixed", maGiamGia.LoaiGiam);
        Assert.Equal(20000, maGiamGia.GiaTri);
    }

    [Fact]
    public void MaGiamGia_IsValid_WhenInDateRange()
    {
        // Arrange
        var maGiamGia = new MaGiamGia
        {
            NgayBatDau = DateTime.Now.AddDays(-1),
            NgayKetThuc = DateTime.Now.AddDays(7),
            TrangThai = true,
            SoLuong = 100,
            DaSuDung = 50
        };

        // Act
        var isValid = maGiamGia.TrangThai 
            && maGiamGia.NgayBatDau <= DateTime.Now 
            && maGiamGia.NgayKetThuc >= DateTime.Now
            && maGiamGia.DaSuDung < maGiamGia.SoLuong;

        // Assert
        Assert.True(isValid);
    }

    #endregion

    #region ChiTietDonHang Tests

    [Fact]
    public void ChiTietDonHang_Create_WithValidData_Success()
    {
        // Arrange & Act
        var chiTiet = new ChiTietDonHang
        {
            MaCTDH = 1,
            MaDH = 1,
            MaSP = 1,
            TenSP = "Mì Cay Level 5",
            SoLuong = 2,
            DonGia = 55000,
            CapDoCay = 5
        };

        // Assert
        Assert.Equal("Mì Cay Level 5", chiTiet.TenSP);
        Assert.Equal(2, chiTiet.SoLuong);
        Assert.Equal(55000, chiTiet.DonGia);
        Assert.Equal(5, chiTiet.CapDoCay);
    }

    [Fact]
    public void ChiTietDonHang_ThanhTien_CalculatedCorrectly()
    {
        // Arrange
        var chiTiet = new ChiTietDonHang
        {
            SoLuong = 3,
            DonGia = 50000
        };

        // Act - ThanhTien được tính trong database, ở đây test logic
        var expectedThanhTien = chiTiet.SoLuong * chiTiet.DonGia;

        // Assert
        Assert.Equal(150000, expectedThanhTien);
    }

    #endregion

    #region GioHang Tests

    [Fact]
    public void GioHang_Create_WithSessionId_Success()
    {
        // Arrange & Act
        var gioHang = new GioHang
        {
            MaGH = 1,
            SessionID = "sess_123456789",
            MaSP = 1,
            SoLuong = 2,
            CapDoCay = 3,
            NgayThem = DateTime.Now
        };

        // Assert
        Assert.Equal("sess_123456789", gioHang.SessionID);
        Assert.Equal(2, gioHang.SoLuong);
        Assert.Equal(3, gioHang.CapDoCay);
    }

    [Fact]
    public void GioHang_Create_WithCustomerId_Success()
    {
        // Arrange & Act
        var gioHang = new GioHang
        {
            MaKH = 1,
            MaSP = 5,
            SoLuong = 1,
            CapDoCay = 7
        };

        // Assert
        Assert.Equal(1, gioHang.MaKH);
        Assert.Equal(7, gioHang.CapDoCay);
    }

    #endregion
}
