USE QuanLyDuAn;
SET NOCOUNT ON;

-- Seed roles
IF NOT EXISTS (SELECT 1 FROM VaiTro WHERE TenVaiTro = N'Admin')
    INSERT INTO VaiTro (TenVaiTro, MoTa) VALUES (N'Admin', N'Quản trị hệ thống');
IF NOT EXISTS (SELECT 1 FROM VaiTro WHERE TenVaiTro = N'Nhân viên')
    INSERT INTO VaiTro (TenVaiTro, MoTa) VALUES (N'Nhân viên', N'Nhân viên cửa hàng');
IF NOT EXISTS (SELECT 1 FROM VaiTro WHERE TenVaiTro = N'Khách hàng')
    INSERT INTO VaiTro (TenVaiTro, MoTa) VALUES (N'Khách hàng', N'Khách mua hàng');

-- Get role ids
DECLARE @AdminRole INT = (SELECT MaVaiTro FROM VaiTro WHERE TenVaiTro = N'Admin');
DECLARE @NVRole INT = (SELECT MaVaiTro FROM VaiTro WHERE TenVaiTro = N'Nhân viên');
DECLARE @KHRole INT = (SELECT MaVaiTro FROM VaiTro WHERE TenVaiTro = N'Khách hàng');

-- NOTE: mật khẩu demo được lưu dưới dạng SHA2-256 hex để tránh plaintext.
-- Ứng dụng khi đăng nhập cần hash cùng thuật toán và so sánh chuỗi hex.

-- Seed accounts (only if not exists)
IF NOT EXISTS (SELECT 1 FROM TaiKhoan WHERE TenDangNhap = N'admin')
BEGIN
    INSERT INTO TaiKhoan (TenDangNhap, MatKhau, TrangThai, MaVaiTro)
    VALUES (N'admin', master.dbo.fn_varbintohexstr(HASHBYTES('SHA2_256', 'P@ssw0rd!')), 1, @AdminRole);
END

IF NOT EXISTS (SELECT 1 FROM TaiKhoan WHERE TenDangNhap = N'staff1')
BEGIN
    INSERT INTO TaiKhoan (TenDangNhap, MatKhau, TrangThai, MaVaiTro)
    VALUES (N'staff1', master.dbo.fn_varbintohexstr(HASHBYTES('SHA2_256', 'Staff@123')), 1, @NVRole);
END

IF NOT EXISTS (SELECT 1 FROM TaiKhoan WHERE TenDangNhap = N'customer1')
BEGIN
    INSERT INTO TaiKhoan (TenDangNhap, MatKhau, TrangThai, MaVaiTro)
    VALUES (N'customer1', master.dbo.fn_varbintohexstr(HASHBYTES('SHA2_256', 'Cust@1234')), 1, @KHRole);
END

-- Seed stores
IF NOT EXISTS (SELECT 1 FROM CuaHang WHERE TenCuaHang = N'MyCay Trung Tâm')
    INSERT INTO CuaHang (TenCuaHang, DiaChi, SoDienThoai, NgayKhaiTruong, TrangThai)
    VALUES (N'MyCay Trung Tâm', N'123 Đường A, Quận 1', '0901234567', '2024-01-01', 1);

IF NOT EXISTS (SELECT 1 FROM CuaHang WHERE TenCuaHang = N'MyCay Sư Vạn Hạnh')
    INSERT INTO CuaHang (TenCuaHang, DiaChi, SoDienThoai, NgayKhaiTruong, TrangThai)
    VALUES (N'MyCay Sư Vạn Hạnh', N'456 Đường B, Quận 10', '0907654321', '2024-06-01', 1);

-- Link managers and sample staff/customers
-- Create a manager linked to admin account
DECLARE @MaCH1 INT = (SELECT TOP 1 MaCH FROM CuaHang WHERE TenCuaHang = N'MyCay Trung Tâm');
DECLARE @MaTK_Admin INT = (SELECT TOP 1 MaTK FROM TaiKhoan WHERE TenDangNhap = N'admin');

IF NOT EXISTS (SELECT 1 FROM QuanLyCuaHang WHERE Email = N'manager@mycay.com')
BEGIN
    INSERT INTO QuanLyCuaHang (HoTen, Email, SDT, MaCH, MaTK)
    VALUES (N'Nguyễn Quản Lý', N'manager@mycay.com', '0912345678', @MaCH1, @MaTK_Admin);
END

-- Sample employee linked to staff1
DECLARE @MaTK_Staff1 INT = (SELECT TOP 1 MaTK FROM TaiKhoan WHERE TenDangNhap = N'staff1');
IF NOT EXISTS (SELECT 1 FROM NhanVien WHERE SDT = N'0933333333')
BEGIN
    INSERT INTO NhanVien (HoTen, NgaySinh, GioiTinh, SDT, MaCH, MaTK)
    VALUES (N'Trần Nhân Viên', '1995-05-05', N'Nam', '0933333333', @MaCH1, @MaTK_Staff1);
END

-- Sample customer linked to customer1
DECLARE @MaTK_Cust1 INT = (SELECT TOP 1 MaTK FROM TaiKhoan WHERE TenDangNhap = N'customer1');
IF NOT EXISTS (SELECT 1 FROM KhachHang WHERE Email = N'customer1@example.com')
BEGIN
    INSERT INTO KhachHang (HoTen, Email, SDT, DiaChi, MaTK)
    VALUES (N'Khách Hàng Một', N'customer1@example.com', '0988888888', N'789 Đường C, Quận 3', @MaTK_Cust1);
END

-- Sample products
IF NOT EXISTS (SELECT 1 FROM SanPham WHERE TenSP = N'Mỳ Cay Truyền Thống')
    INSERT INTO SanPham (TenSP, MoTa, DonGia, HinhAnh, TrangThai)
    VALUES (N'Mỳ Cay Truyền Thống', N'Mỳ cay vị đặc trưng', 45000.00, NULL, 1);

IF NOT EXISTS (SELECT 1 FROM SanPham WHERE TenSP = N'Mỳ Cay Hải Sản')
    INSERT INTO SanPham (TenSP, MoTa, DonGia, HinhAnh, TrangThai)
    VALUES (N'Mỳ Cay Hải Sản', N'Mỳ cay kèm hải sản tươi', 65000.00, NULL, 1);

-- Sample order for customer
DECLARE @MaKH1 INT = (SELECT TOP 1 MaKH FROM KhachHang WHERE Email = N'customer1@example.com');
IF @MaKH1 IS NOT NULL AND NOT EXISTS (SELECT 1 FROM DonHang WHERE MaKH = @MaKH1)
BEGIN
    INSERT INTO DonHang (MaKH, TongTien, TrangThai, MaCH, MaNV)
    VALUES (@MaKH1, 110000.00, N'Hoàn thành', @MaCH1, (SELECT TOP 1 MaNV FROM NhanVien WHERE MaTK = @MaTK_Staff1));
    DECLARE @MaDH_New INT = SCOPE_IDENTITY();
    DECLARE @MaSP1 INT = (SELECT TOP 1 MaSP FROM SanPham WHERE TenSP = N'Mỳ Cay Truyền Thống');
    DECLARE @MaSP2 INT = (SELECT TOP 1 MaSP FROM SanPham WHERE TenSP = N'Mỳ Cay Hải Sản');
    IF @MaSP1 IS NOT NULL
        INSERT INTO ChiTietDonHang (MaDH, MaSP, SoLuong, DonGia)
        VALUES (@MaDH_New, @MaSP1, 1, (SELECT DonGia FROM SanPham WHERE MaSP = @MaSP1));
    IF @MaSP2 IS NOT NULL
        INSERT INTO ChiTietDonHang (MaDH, MaSP, SoLuong, DonGia)
        VALUES (@MaDH_New, @MaSP2, 1, (SELECT DonGia FROM SanPham WHERE MaSP = @MaSP2));
END

PRINT 'Seed data applied.';
