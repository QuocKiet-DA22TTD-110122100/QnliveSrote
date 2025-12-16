-- =====================================================
-- CƠ SỞ DỮ LIỆU MỲ CAY SASIN
-- Tạo mới: 16/12/2024
-- =====================================================

-- Tạo Database
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'MyCayDB')
BEGIN
    CREATE DATABASE MyCayDB;
END
GO

USE MyCayDB;
GO

-- Xóa các bảng cũ nếu tồn tại (theo thứ tự phụ thuộc)
IF OBJECT_ID('ChiTietDonHang', 'U') IS NOT NULL DROP TABLE ChiTietDonHang;
IF OBJECT_ID('GioHang', 'U') IS NOT NULL DROP TABLE GioHang;
IF OBJECT_ID('DonHang', 'U') IS NOT NULL DROP TABLE DonHang;
IF OBJECT_ID('BaoCao', 'U') IS NOT NULL DROP TABLE BaoCao;
IF OBJECT_ID('TonKho', 'U') IS NOT NULL DROP TABLE TonKho;
IF OBJECT_ID('SanPham', 'U') IS NOT NULL DROP TABLE SanPham;
IF OBJECT_ID('DanhMuc', 'U') IS NOT NULL DROP TABLE DanhMuc;
IF OBJECT_ID('NhanVien', 'U') IS NOT NULL DROP TABLE NhanVien;
IF OBJECT_ID('QuanLyCuaHang', 'U') IS NOT NULL DROP TABLE QuanLyCuaHang;
IF OBJECT_ID('KhachHang', 'U') IS NOT NULL DROP TABLE KhachHang;
IF OBJECT_ID('NguoiDungQuanTri', 'U') IS NOT NULL DROP TABLE NguoiDungQuanTri;
IF OBJECT_ID('TaiKhoan', 'U') IS NOT NULL DROP TABLE TaiKhoan;
IF OBJECT_ID('CuaHang', 'U') IS NOT NULL DROP TABLE CuaHang;
IF OBJECT_ID('VaiTro', 'U') IS NOT NULL DROP TABLE VaiTro;
GO

-- =====================================================
-- 1. BẢNG VaiTro (Vai trò người dùng)
-- =====================================================
CREATE TABLE VaiTro (
    MaVaiTro INT IDENTITY(1,1) PRIMARY KEY,
    TenVaiTro NVARCHAR(50) NOT NULL UNIQUE,
    MoTa NVARCHAR(200) NULL,
    NgayTao DATETIME DEFAULT GETDATE()
);
GO

-- =====================================================
-- 2. BẢNG TaiKhoan (Tài khoản đăng nhập)
-- =====================================================
CREATE TABLE TaiKhoan (
    MaTK INT IDENTITY(1,1) PRIMARY KEY,
    TenDangNhap NVARCHAR(50) NOT NULL UNIQUE,
    MatKhau NVARCHAR(255) NOT NULL,
    Email NVARCHAR(100) NULL,
    TrangThai BIT DEFAULT 1, -- 1: Hoạt động, 0: Khóa
    MaVaiTro INT NOT NULL,
    NgayTao DATETIME DEFAULT GETDATE(),
    LanDangNhapCuoi DATETIME NULL,
    FOREIGN KEY (MaVaiTro) REFERENCES VaiTro(MaVaiTro)
);
GO


-- =====================================================
-- 3. BẢNG CuaHang (Cửa hàng/Chi nhánh)
-- =====================================================
CREATE TABLE CuaHang (
    MaCH INT IDENTITY(1,1) PRIMARY KEY,
    TenCuaHang NVARCHAR(100) NOT NULL,
    DiaChi NVARCHAR(200) NOT NULL,
    SoDienThoai VARCHAR(15) NOT NULL,
    Email NVARCHAR(100) NULL,
    NgayKhaiTruong DATE NULL,
    GioMoCua TIME DEFAULT '10:00:00',
    GioDongCua TIME DEFAULT '22:00:00',
    TrangThai BIT DEFAULT 1,
    NgayTao DATETIME DEFAULT GETDATE()
);
GO

-- =====================================================
-- 4. BẢNG NguoiDungQuanTri (Quản trị viên hệ thống)
-- =====================================================
CREATE TABLE NguoiDungQuanTri (
    MaQTV INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    SDT VARCHAR(15) NULL,
    MaTK INT NOT NULL UNIQUE,
    NgayTao DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (MaTK) REFERENCES TaiKhoan(MaTK)
);
GO

-- =====================================================
-- 5. BẢNG QuanLyCuaHang (Quản lý cửa hàng)
-- =====================================================
CREATE TABLE QuanLyCuaHang (
    MaQL INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    SDT VARCHAR(15) NOT NULL,
    MaCH INT NOT NULL,
    MaTK INT NOT NULL UNIQUE,
    NgayBatDau DATE DEFAULT GETDATE(),
    TrangThai BIT DEFAULT 1,
    FOREIGN KEY (MaCH) REFERENCES CuaHang(MaCH),
    FOREIGN KEY (MaTK) REFERENCES TaiKhoan(MaTK)
);
GO

-- =====================================================
-- 6. BẢNG NhanVien (Nhân viên cửa hàng)
-- =====================================================
CREATE TABLE NhanVien (
    MaNV INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    NgaySinh DATE NULL,
    GioiTinh NVARCHAR(10) NULL,
    SDT VARCHAR(15) NOT NULL,
    DiaChi NVARCHAR(200) NULL,
    ChucVu NVARCHAR(50) DEFAULT N'Nhân viên',
    Luong DECIMAL(18,2) NULL,
    MaCH INT NOT NULL,
    MaTK INT NULL UNIQUE,
    NgayVaoLam DATE DEFAULT GETDATE(),
    TrangThai BIT DEFAULT 1,
    FOREIGN KEY (MaCH) REFERENCES CuaHang(MaCH),
    FOREIGN KEY (MaTK) REFERENCES TaiKhoan(MaTK)
);
GO

-- =====================================================
-- 7. BẢNG KhachHang (Khách hàng)
-- =====================================================
CREATE TABLE KhachHang (
    MaKH INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NULL,
    SDT VARCHAR(15) NOT NULL,
    DiaChi NVARCHAR(200) NULL,
    NgaySinh DATE NULL,
    DiemTichLuy INT DEFAULT 0,
    MaTK INT NULL UNIQUE,
    NgayDangKy DATETIME DEFAULT GETDATE(),
    TrangThai BIT DEFAULT 1,
    FOREIGN KEY (MaTK) REFERENCES TaiKhoan(MaTK)
);
GO

-- =====================================================
-- 8. BẢNG DanhMuc (Danh mục sản phẩm)
-- =====================================================
CREATE TABLE DanhMuc (
    MaDM INT IDENTITY(1,1) PRIMARY KEY,
    TenDanhMuc NVARCHAR(100) NOT NULL,
    MoTa NVARCHAR(200) NULL,
    HinhAnh NVARCHAR(255) NULL,
    ThuTu INT DEFAULT 0,
    TrangThai BIT DEFAULT 1
);
GO


-- =====================================================
-- 9. BẢNG SanPham (Sản phẩm/Món ăn)
-- =====================================================
CREATE TABLE SanPham (
    MaSP INT IDENTITY(1,1) PRIMARY KEY,
    MaSPCode NVARCHAR(20) NULL UNIQUE, -- Mã code từ sasin.vn (M00012, MI0008...)
    TenSP NVARCHAR(150) NOT NULL,
    MoTa NVARCHAR(500) NULL,
    DonGia DECIMAL(18,2) NOT NULL,
    GiaKhuyenMai DECIMAL(18,2) NULL,
    HinhAnh NVARCHAR(255) NULL,
    MaDM INT NULL,
    CapDoCay INT DEFAULT 0, -- 0-7
    NoiBat BIT DEFAULT 0, -- Sản phẩm nổi bật
    TrangThai BIT DEFAULT 1,
    NgayTao DATETIME DEFAULT GETDATE(),
    NgayCapNhat DATETIME NULL,
    FOREIGN KEY (MaDM) REFERENCES DanhMuc(MaDM)
);
GO

-- =====================================================
-- 10. BẢNG TonKho (Tồn kho theo cửa hàng)
-- =====================================================
CREATE TABLE TonKho (
    MaTK INT IDENTITY(1,1) PRIMARY KEY,
    MaSP INT NOT NULL,
    MaCH INT NOT NULL,
    SoLuong INT DEFAULT 0,
    NgayCapNhat DATETIME DEFAULT GETDATE(),
    UNIQUE(MaSP, MaCH),
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP),
    FOREIGN KEY (MaCH) REFERENCES CuaHang(MaCH)
);
GO

-- =====================================================
-- 11. BẢNG GioHang (Giỏ hàng)
-- =====================================================
CREATE TABLE GioHang (
    MaGH INT IDENTITY(1,1) PRIMARY KEY,
    MaKH INT NULL, -- NULL nếu khách vãng lai
    SessionID NVARCHAR(100) NULL, -- Cho khách chưa đăng nhập
    MaSP INT NOT NULL,
    SoLuong INT NOT NULL DEFAULT 1,
    CapDoCay INT DEFAULT 0,
    GhiChu NVARCHAR(200) NULL,
    NgayThem DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH),
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP)
);
GO

-- =====================================================
-- 12. BẢNG DonHang (Đơn hàng)
-- =====================================================
CREATE TABLE DonHang (
    MaDH INT IDENTITY(1,1) PRIMARY KEY,
    MaDHCode NVARCHAR(20) NULL UNIQUE, -- Mã đơn hàng hiển thị (DH20241216001)
    MaKH INT NULL,
    TenKhach NVARCHAR(100) NULL, -- Cho khách vãng lai
    SDTKhach VARCHAR(15) NULL,
    DiaChiGiao NVARCHAR(200) NULL,
    NgayDat DATETIME DEFAULT GETDATE(),
    NgayGiao DATETIME NULL,
    TamTinh DECIMAL(18,2) DEFAULT 0,
    PhiGiaoHang DECIMAL(18,2) DEFAULT 15000,
    GiamGia DECIMAL(18,2) DEFAULT 0,
    TongTien DECIMAL(18,2) DEFAULT 0,
    PhuongThucThanhToan NVARCHAR(50) DEFAULT N'Tiền mặt', -- Tiền mặt, Chuyển khoản, Momo...
    TrangThaiThanhToan NVARCHAR(50) DEFAULT N'Chưa thanh toán',
    TrangThai NVARCHAR(50) DEFAULT N'Chờ xác nhận', -- Chờ xác nhận, Đang chuẩn bị, Đang giao, Hoàn thành, Đã hủy
    GhiChu NVARCHAR(500) NULL,
    MaCH INT NULL,
    MaNV INT NULL, -- Nhân viên xử lý
    NgayCapNhat DATETIME NULL,
    FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH),
    FOREIGN KEY (MaCH) REFERENCES CuaHang(MaCH),
    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV)
);
GO

-- =====================================================
-- 13. BẢNG ChiTietDonHang (Chi tiết đơn hàng)
-- =====================================================
CREATE TABLE ChiTietDonHang (
    MaCTDH INT IDENTITY(1,1) PRIMARY KEY,
    MaDH INT NOT NULL,
    MaSP INT NOT NULL,
    TenSP NVARCHAR(150) NOT NULL, -- Lưu tên tại thời điểm đặt
    SoLuong INT NOT NULL DEFAULT 1,
    DonGia DECIMAL(18,2) NOT NULL,
    CapDoCay INT DEFAULT 0,
    GhiChu NVARCHAR(200) NULL,
    ThanhTien AS (SoLuong * DonGia) PERSISTED,
    FOREIGN KEY (MaDH) REFERENCES DonHang(MaDH) ON DELETE CASCADE,
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP)
);
GO

-- =====================================================
-- 14. BẢNG BaoCao (Báo cáo doanh thu)
-- =====================================================
CREATE TABLE BaoCao (
    MaBC INT IDENTITY(1,1) PRIMARY KEY,
    MaCH INT NOT NULL,
    MaQL INT NULL,
    LoaiBaoCao NVARCHAR(50) NOT NULL, -- Ngày, Tuần, Tháng, Năm
    TuNgay DATE NOT NULL,
    DenNgay DATE NOT NULL,
    TongDonHang INT DEFAULT 0,
    TongDoanhThu DECIMAL(18,2) DEFAULT 0,
    TongChiPhi DECIMAL(18,2) DEFAULT 0,
    LoiNhuan DECIMAL(18,2) DEFAULT 0,
    GhiChu NVARCHAR(500) NULL,
    NgayTao DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (MaCH) REFERENCES CuaHang(MaCH),
    FOREIGN KEY (MaQL) REFERENCES QuanLyCuaHang(MaQL)
);
GO


-- =====================================================
-- DỮ LIỆU MẪU
-- =====================================================

-- 1. Vai trò
INSERT INTO VaiTro (TenVaiTro, MoTa) VALUES 
(N'QuanTriVien', N'Quản trị viên hệ thống - toàn quyền'),
(N'QuanLy', N'Quản lý cửa hàng - quản lý sản phẩm, đơn hàng, báo cáo'),
(N'NhanVien', N'Nhân viên - xem và cập nhật trạng thái đơn hàng'),
(N'KhachHang', N'Khách hàng - xem sản phẩm, đặt hàng');
GO

-- 2. Tài khoản mẫu (mật khẩu: 123456)
INSERT INTO TaiKhoan (TenDangNhap, MatKhau, Email, MaVaiTro) VALUES 
(N'admin', N'e10adc3949ba59abbe56e057f20f883e', N'admin@mycaysasin.vn', 1),
(N'quanly1', N'e10adc3949ba59abbe56e057f20f883e', N'quanly1@mycaysasin.vn', 2),
(N'nhanvien1', N'e10adc3949ba59abbe56e057f20f883e', N'nhanvien1@mycaysasin.vn', 3),
(N'khachhang1', N'e10adc3949ba59abbe56e057f20f883e', N'khach1@gmail.com', 4);
GO

-- 3. Cửa hàng
INSERT INTO CuaHang (TenCuaHang, DiaChi, SoDienThoai, Email, NgayKhaiTruong) VALUES 
(N'Mỳ Cay Sasin - Quận 1', N'123 Nguyễn Huệ, Phường Bến Nghé, Quận 1, TP.HCM', '0901234567', 'q1@mycaysasin.vn', '2023-01-15'),
(N'Mỳ Cay Sasin - Quận 3', N'456 Võ Văn Tần, Phường 5, Quận 3, TP.HCM', '0901234568', 'q3@mycaysasin.vn', '2023-06-01'),
(N'Mỳ Cay Sasin - Quận 7', N'789 Nguyễn Thị Thập, Phường Tân Phú, Quận 7, TP.HCM', '0901234569', 'q7@mycaysasin.vn', '2024-01-10');
GO

-- 4. Quản trị viên
INSERT INTO NguoiDungQuanTri (HoTen, Email, SDT, MaTK) VALUES 
(N'Nguyễn Văn Admin', N'admin@mycaysasin.vn', '0909000001', 1);
GO

-- 5. Quản lý cửa hàng
INSERT INTO QuanLyCuaHang (HoTen, Email, SDT, MaCH, MaTK) VALUES 
(N'Trần Thị Quản Lý', N'quanly1@mycaysasin.vn', '0909000002', 1, 2);
GO

-- 6. Nhân viên
INSERT INTO NhanVien (HoTen, NgaySinh, GioiTinh, SDT, DiaChi, ChucVu, Luong, MaCH, MaTK) VALUES 
(N'Lê Văn Nhân Viên', '1998-05-15', N'Nam', '0909000003', N'Quận Bình Thạnh', N'Nhân viên phục vụ', 8000000, 1, 3),
(N'Phạm Thị Hoa', '2000-08-20', N'Nữ', '0909000004', N'Quận 1', N'Thu ngân', 8500000, 1, NULL),
(N'Nguyễn Văn Bếp', '1995-03-10', N'Nam', '0909000005', N'Quận 3', N'Đầu bếp', 12000000, 1, NULL);
GO

-- 7. Khách hàng
INSERT INTO KhachHang (HoTen, Email, SDT, DiaChi, NgaySinh, DiemTichLuy, MaTK) VALUES 
(N'Nguyễn Thị Mai', N'khach1@gmail.com', '0988888881', N'123 Lê Lợi, Quận 1', '1995-06-15', 150, 4),
(N'Trần Văn Hùng', N'hung.tran@gmail.com', '0988888882', N'456 Hai Bà Trưng, Quận 3', '1990-12-20', 280, NULL),
(N'Lê Thị Hương', N'huong.le@gmail.com', '0988888883', N'789 Nguyễn Trãi, Quận 5', '1998-03-08', 50, NULL);
GO

-- 8. Danh mục sản phẩm
INSERT INTO DanhMuc (TenDanhMuc, MoTa, HinhAnh, ThuTu) VALUES 
(N'Mì Cay', N'Các loại mì cay đặc trưng Hàn Quốc', 'MenuItemGroup_MG00005.webp', 1),
(N'Mì Tương Đen', N'Mì trộn tương đen Hàn Quốc', 'MenuItemGroup_MG00006.webp', 2),
(N'Mì Xào', N'Các loại mì xào', NULL, 3),
(N'Món Khác', N'Cơm, tokbokki và các món khác', 'MenuItemGroup_MG00007.webp', 4),
(N'Món Thêm Mì', N'Topping thêm cho mì', NULL, 5),
(N'Combo', N'Các combo tiết kiệm', 'MenuItemGroup_MG00003.webp', 6),
(N'Lẩu Hàn Quốc', N'Các loại lẩu Hàn Quốc', NULL, 7),
(N'Món Thêm Lẩu', N'Topping thêm cho lẩu', NULL, 8),
(N'Khai Vị', N'Món khai vị, ăn vặt', 'MenuItemGroup_MG00010.webp', 9),
(N'Giải Khát', N'Đồ uống, nước giải khát', 'MenuItemGroup_MG00018.webp', 10);
GO


-- 9. Sản phẩm (100 sản phẩm từ sasin.vn)
-- Mì Cay (MaDM = 1)
INSERT INTO SanPham (MaSPCode, TenSP, MoTa, DonGia, HinhAnh, MaDM, CapDoCay, NoiBat) VALUES 
(N'M00012', N'Mì Thập Cẩm No Nê (Kim Chi/ Soyum/ Sincay)', N'Mì cay Sasin, thịt heo, tôm, cá viên, trứng ngâm tương, thanh cua, chả cá Hàn Quốc, kim chi, nấm, súp lơ, bắp cải tím, ngò gai', 77000, 'MenuItem_M00012.webp', 1, 3, 1),
(N'MI0008', N'Mì Thập Cẩm (Kim chi/ Soyum/ Sincay)', N'Mì cay Sasin, Thịt bò, tôm, mực, chả cá Hàn Quốc, cá viên, kim chi, súp lơ, nấm, bắp cải tím, ngò gai', 69000, 'MenuItem_MI0008.webp', 1, 3, 1),
(N'M00018', N'Mì Hải Sản (Kim chi/ Soyum/ Sincay)', N'Mì cay Sasin, tôm, mực, chả cá Hàn Quốc, cá viên, kim chi, súp lơ, ngò gai, nấm, bắp cải tím', 62000, 'MenuItem_M00018.webp', 1, 3, 1),
(N'MI0005', N'Mì Hải Sản Thanh Cua (Kim chi/ Soyum/ Sincay)', N'Mì cay Sasin, Tôm, thanh cua, chả cá Hàn Quốc, cá viên, kim chi, súp lơ, nấm, bắp cải tím, ngò gai', 62000, 'MenuItem_MI0005.webp', 1, 3, 0),
(N'M00021', N'Mì Bò Mỹ (Kim chi/ Soyum/ Sincay)', N'Mì cay Sasin, thịt bò, xúc xích, cá viên, kim chi, súp lơ, nấm, bắp cải tím, ngò gai', 59000, 'MenuItem_M00021.webp', 1, 3, 0),
(N'M00022', N'Mì Bò Trứng (Kim chi/ Soyum/ Sincay)', N'Mì cay Sasin, thịt bò, trứng lòng đào, xúc xích, cá viên, kim chi, súp lơ, nấm, bắp cải tím, ngò gai', 65000, 'MenuItem_M00022.webp', 1, 3, 0),
(N'M00109', N'Mì Đùi Gà (Kim chi/ Soyum/ Sincay)', N'Mì cay Sasin, đùi gà, cá viên, kim chi, súp lơ, nấm, bắp cải tím, ngò gai, chả cá Hàn Quốc', 59000, 'MenuItem_M00109.webp', 1, 3, 0),
(N'MI0004', N'Mì Kim Chi Cá', N'Mì cay Sasin, phi lê cá, nấm, cá viên, kim chi, súp lơ, bắp cải tím, ngò gai', 49000, 'MenuItem_MI0004.webp', 1, 2, 0),
(N'M00027', N'Mì Kim Chi Gogi', N'Mì cay Sasin, thịt heo, xúc xích, kim chi, cá viên, kim chi, súp lơ, nấm, bắp cải tím, ngò gai', 49000, 'MenuItem_M00027.webp', 1, 2, 0),
(N'M00011', N'Mì Kim Chi Xúc Xích Cá Viên', N'Mì cay Sasin, xúc xích, kim chi, nấm, cá viên, súp lơ, bắp cải tím, chả cá Hàn Quốc, ngò gai', 39000, 'MenuItem_M00011.webp', 1, 2, 0);
GO

-- Mì Tương Đen (MaDM = 2)
INSERT INTO SanPham (MaSPCode, TenSP, MoTa, DonGia, HinhAnh, MaDM, CapDoCay, NoiBat) VALUES 
(N'M00015', N'Mì Trộn Tương Đen Heo Cuộn', N'Mì cay Sasin, heo cuộn, cá viên, cà rốt, ớt chuông, hành tây, hành baro', 69000, 'MenuItem_M00015.webp', 2, 0, 0),
(N'M00016', N'Mì Trộn Tương Đen Bò Mỹ', N'Mì cay Sasin, thịt bò, cá viên, chả cá Hàn Quốc, hành tây, ớt chuông, cà rốt, hành baro, mè', 65000, 'MenuItem_M00016.webp', 2, 0, 0),
(N'M00014', N'Mì Trộn Tương Đen Gà', N'Mì cay Sasin, gà, cá viên, hành tây, ớt chuông, cà rốt, hành baro, mè', 59000, 'MenuItem_M00014.webp', 2, 0, 0),
(N'M00013', N'Mì Trộn Tương Đen Mandu', N'Mì cay Sasin, mandu, cá viên, hành tây, ớt chuông, cà rốt, hành baro, mè', 55000, 'MenuItem_M00013.webp', 2, 0, 0);
GO

-- Mì Xào (MaDM = 3)
INSERT INTO SanPham (MaSPCode, TenSP, MoTa, DonGia, HinhAnh, MaDM, CapDoCay, NoiBat) VALUES 
(N'M00130', N'Mì Xào Hải Sản', N'Mì cay Sasin, tôm, mực, chả cá HQ, cá viên, ớt chuông, hành tây, cải bó xôi, nấm, mè', 69000, 'MenuItem_M00130.webp', 3, 1, 0),
(N'M00131', N'Mì Xào Sasin', N'Mì cay Sasin, thịt heo, xúc xích, cá viên, súp lơ, ớt chuông, hành tây, cải bó xôi, nấm', 65000, 'MenuItem_M00131.webp', 3, 1, 0),
(N'M00132', N'Mì Trộn Xốt Phô Mai', N'Mì cay Sasin, gà, phô mai, xốt kem, cà rốt, hành baro', 62000, 'MenuItem_M00132.webp', 3, 0, 0);
GO

-- Món Khác (MaDM = 4)
INSERT INTO SanPham (MaSPCode, TenSP, MoTa, DonGia, HinhAnh, MaDM, CapDoCay, NoiBat) VALUES 
(N'M00129', N'Miến Trộn Ngũ Sắc Hàn Quốc', N'Miến, thịt bò, xúc xích, nấm, ớt chuông, cà rốt, hành tây, hành baro, cải bó xôi, chả cá HQ, mè', 65000, 'MenuItem_M00129.webp', 4, 0, 0),
(N'MI0002', N'Mì Tương Hàn Mandu', N'Mì cay Sasin, manudu, xúc xích, súp lơ, cải thảo, nấm, hành baro', 52000, 'MenuItem_MI0002.webp', 4, 1, 0),
(N'MI0001', N'Mì Tương Hàn Thịt Heo Cuộn', N'Mì cay Sasin, heo cuộn, cải thảo, trứng ngâm tương, súp lơ, nấm, hành baro', 65000, 'MenuItem_MI0001.webp', 4, 1, 0),
(N'M00028', N'Cơm Trộn Thịt Bò Mỹ', N'Cơm, thịt bò, trứng, nấm, kim chi, rong biển, cà rốt, cải bó xôi, mè, ngò gai', 62000, 'MenuItem_M00028.webp', 4, 0, 0),
(N'M00020', N'Cơm và Canh Kim Chi', N'Cơm, thịt heo, chả cá Hàn Quốc, cá viên, kim chi, nấm, súp lơ, ớt chuông, hành tây, ngò gai', 62000, 'MenuItem_M00020.webp', 4, 2, 0),
(N'MI0003', N'Tokbok-cheese Bò Mỹ', N'Tokbokki, thịt bò, xúc xích, cá viên, phô mai, hành baro, mè', 62000, 'MenuItem_MI0003.webp', 4, 2, 0),
(N'MI0006', N'Tokbokki Phô Mai Sasin', N'Tokbokki, xúc xích, chả cá Hàn Quốc, bắp cải, nấm, hành baro, phô mai', 59000, 'MenuItem_MI0006.webp', 4, 2, 1);
GO


-- Món Thêm Mì (MaDM = 5)
INSERT INTO SanPham (MaSPCode, TenSP, MoTa, DonGia, HinhAnh, MaDM, CapDoCay, NoiBat) VALUES 
(N'M00030', N'Trứng Ngâm Tương', N'마약 계란', 12000, 'MenuItem_M00030.webp', 5, 0, 0),
(N'M00139', N'Bông Cải Xanh', N'브로콜리', 15000, 'MenuItem_M00139.webp', 5, 0, 0),
(N'M00034', N'Bắp Cải Tím', N'적채', 15000, 'MenuItem_M00034.webp', 5, 0, 0),
(N'M00113', N'Nấm Kim Châm Thêm', N'에노키타케', 15000, 'MenuItem_M00113.webp', 5, 0, 0),
(N'M00035', N'Mực', N'오징어', 15000, 'MenuItem_M00035.webp', 5, 0, 0),
(N'M00032', N'Tôm Thêm', N'새우', 15000, 'MenuItem_M00032.webp', 5, 0, 0),
(N'M00029', N'Thịt Heo Cuộn', N'차슈', 15000, 'MenuItem_M00029.webp', 5, 0, 0),
(N'M00033', N'Cá Viên Thêm', N'어육 완자', 15000, 'MenuItem_M00033.webp', 5, 0, 0),
(N'M00036', N'Xúc xích', N'소시지', 15000, 'MenuItem_M00036.webp', 5, 0, 0),
(N'M00138', N'Bắp Bò', N'소사태', 19000, 'MenuItem_M00138.webp', 5, 0, 0),
(N'M00031', N'Bò Thêm', N'소고기', 19000, 'MenuItem_M00031.webp', 5, 0, 0),
(N'M00140', N'Combo Xiên Que', N'계피 꼬치 콤보', 12000, 'MenuItem_M00140.webp', 5, 0, 0),
(N'M00112', N'Mì Nấu Thêm', N'라면', 19000, 'MenuItem_M00112.webp', 5, 0, 0);
GO

-- Combo (MaDM = 6)
INSERT INTO SanPham (MaSPCode, TenSP, MoTa, DonGia, HinhAnh, MaDM, CapDoCay, NoiBat) VALUES 
(N'M00141', N'Combo Vui Vẻ (1 người)', N'1 Món tự chọn (Mì cá/ Mì đùi gà/ Mì gogi/ Mì xúc xích cá viên/ Mì bò Mỹ) + 1 Ly Coca-cola/ Sprite size L', 69000, 'MenuItem_M00141.webp', 6, 3, 1),
(N'M00142', N'Combo Gây Mê (1 người)', N'1 Món tự chọn (Miến xào/ Mì xào Sasin/ Mì xào hải sản/ Cơm và canh kim chi) + 1 Ly Coca-cola/ Sprite size L', 79000, 'MenuItem_M00142.webp', 6, 3, 0),
(N'M00143', N'Combo Bạn Thân (2 người)', N'2 Món tự chọn thuộc nhóm mì cay + 1 phần khai vị tự chọn', 159000, 'MenuItem_M00143.webp', 6, 3, 1),
(N'M00144', N'Combo No Căng (2 người)', N'2 Món tự chọn thuộc nhóm mì cay + 1 Tokbokki phô mai Sasin', 179000, 'MenuItem_M00144.webp', 6, 3, 0),
(N'M00145', N'Combo Gia Đình (3 người)', N'2 Món mì cay + 1 Món mì tương đen/mì xào + 1 Phần khai vị', 219000, 'MenuItem_M00145.webp', 6, 3, 0),
(N'M00146', N'Combo Lẩu 2 Người', N'Lẩu (Hải sản/ Bò Mỹ) + 1 Phần khai vị tự chọn', 225000, 'MenuItem_M00146.webp', 6, 3, 0);
GO

-- Lẩu Hàn Quốc (MaDM = 7)
INSERT INTO SanPham (MaSPCode, TenSP, MoTa, DonGia, HinhAnh, MaDM, CapDoCay, NoiBat) VALUES 
(N'M00117', N'Lẩu Sincay Hải Sản (2 Người)', N'Mì cay Sasin, thịt bò, bò viên, cá viên, chả cá Hàn Quốc, chả cá sợi, kim chi, nấm kim châm, súp lơ, bắp cải tím', 199000, 'MenuItem_M00117.webp', 7, 4, 0),
(N'M00037', N'Lẩu Sincay Bò Mỹ (2 Người)', N'Mì cay Sasin, thịt bò, bò viên, cá viên, chả cá Hàn Quốc, chả cá sợi, kim chi, nấm kim châm, súp lơ, bắp cải tím', 209000, 'MenuItem_M00037.webp', 7, 4, 0),
(N'M00136', N'Lẩu Kim Chi Bò Mỹ (2 Người)', N'Mì cay Sasin, thịt bò, bò viên, cá viên, chả cá Hàn Quốc, chả cá sợi, kim chi, nấm kim châm, súp lơ, bắp cải tím', 209000, 'MenuItem_M00136.webp', 7, 4, 0),
(N'M00038', N'Lẩu Kim Chi Hải Sản (2 Người)', N'Mì cay Sasin, tôm, mực, cá viên, kim chi, nấm kim châm, súp lơ, bắp cải tím', 209000, 'MenuItem_M00038.webp', 7, 4, 0),
(N'M00116', N'Lẩu Soyum Hải Sản (2 Người)', N'Mì cay Sasin, tôm, mực, cá viên, kim chi, nấm kim châm, súp lơ, bắp cải tím', 209000, 'MenuItem_M00116.webp', 7, 4, 0),
(N'M00119', N'Lẩu Soyum Bò Mỹ (2 Người)', N'Mì cay Sasin, thịt bò, bò viên, cá viên, chả cá Hàn Quốc, chả cá sợi, kim chi, nấm kim châm, súp lơ, bắp cải tím', 209000, 'MenuItem_M00119.webp', 7, 4, 0);
GO

-- Món Thêm Lẩu (MaDM = 8)
INSERT INTO SanPham (MaSPCode, TenSP, MoTa, DonGia, HinhAnh, MaDM, CapDoCay, NoiBat) VALUES 
(N'M00068', N'Trứng Gà (1 Quả)', N'계란 1개', 9000, 'MenuItem_M00068.webp', 8, 0, 0),
(N'M00065', N'Mì Gói', N'라면', 12000, 'MenuItem_M00065.webp', 8, 0, 0),
(N'M00060', N'Nấm Kim Châm Thêm Lẩu', N'에노키타케', 25000, 'MenuItem_M00060.webp', 8, 0, 0),
(N'M00063', N'Cải Thảo', N'배추', 25000, 'MenuItem_M00063.webp', 8, 0, 0),
(N'M00062', N'Bông Cải Xanh Lẩu', N'브로콜리', 25000, 'MenuItem_M00062.webp', 8, 0, 0),
(N'M00048', N'Bắp Cải Tím Thêm', N'적채', 25000, 'MenuItem_M00048.webp', 8, 0, 0),
(N'M00056', N'Cá Viên Thêm Lẩu', N'어육 완자', 25000, 'MenuItem_M00056.webp', 8, 0, 0),
(N'M00049', N'Cá Thêm', N'물고기', 25000, 'MenuItem_M00049.webp', 8, 0, 0),
(N'M00057', N'Mực Lẩu', N'오징어', 25000, 'MenuItem_M00057.webp', 8, 0, 0),
(N'M00052', N'Tôm Thêm Lẩu', N'새우', 25000, 'MenuItem_M00052.webp', 8, 0, 0),
(N'M00066', N'Chả Cá Hàn Quốc', N'어묵', 25000, 'MenuItem_M00066.webp', 8, 0, 0),
(N'M00058', N'Xúc Xích Thêm', N'소시지', 25000, 'MenuItem_M00058.webp', 8, 0, 0),
(N'M00053', N'Thanh Cua', N'게맛살', 25000, 'MenuItem_M00053.webp', 8, 0, 0),
(N'M00137', N'Bắp Bò Lẩu', N'소사태', 25000, 'MenuItem_M00137.webp', 8, 0, 0),
(N'M00054', N'Bò Thêm Lẩu', N'소고기', 25000, 'MenuItem_M00054.webp', 8, 0, 0),
(N'M00059', N'Mandu', N'만두', 25000, 'MenuItem_M00059.webp', 8, 0, 0),
(N'M00051', N'Tokbokki Phô Mai', N'치즈 떡볶이', 25000, 'MenuItem_M00051.webp', 8, 0, 0);
GO


-- Khai Vị (MaDM = 9)
INSERT INTO SanPham (MaSPCode, TenSP, MoTa, DonGia, HinhAnh, MaDM, CapDoCay, NoiBat) VALUES 
(N'M00070', N'Khoai Tây Chiên', N'감자 튀김', 32000, 'MenuItem_M00070.webp', 9, 0, 0),
(N'M00128', N'Gà Viên Chiên Giòn (6 viên)', N'Gà Viên Chiên Giòn (6 viên)', 32000, 'MenuItem_M00128.webp', 9, 0, 0),
(N'M00076', N'Phô Mai Que', N'모짜렐라 스틱', 39000, 'MenuItem_M00076.webp', 9, 0, 0),
(N'M00072', N'Rong Biển Cuộn Fillet Cá Chiên', N'어묵김말이 튀김', 39000, 'MenuItem_M00072.webp', 9, 0, 0),
(N'M00073', N'Phô Mai Viên', N'치즈볼', 29000, 'MenuItem_M00073.webp', 9, 0, 0),
(N'M00135', N'Combo khai vị phô mai', N'Phô mai que, phô mai viên, viên thanh cua phô mai', 49000, 'MenuItem_M00135.webp', 9, 0, 0),
(N'M00134', N'Kimbap Sasin (12 cuộn)', N'12 cuộn', 59000, 'MenuItem_M00134.webp', 9, 0, 0),
(N'M00133', N'Kimbap Sasin (6 cuộn)', N'6 cuộn', 35000, 'MenuItem_M00133.webp', 9, 0, 0),
(N'M00069', N'Kimbap Chiên', N'바삭한 김밥튀김', 45000, 'MenuItem_M00069.webp', 9, 0, 0),
(N'M00071', N'Mandu Chiên Xốt Cay', N'칠리 소스를 곁들인 튀김 만두', 35000, 'MenuItem_M00071.webp', 9, 1, 0),
(N'M00074', N'Bánh Bạch Tuộc', N'타코야키', 39000, 'MenuItem_M00074.webp', 9, 0, 0),
(N'M00127', N'Chân Gà Xốt Hàn', N'Chân Gà Xốt Hàn', 49000, 'MenuItem_M00127.webp', 9, 2, 0),
(N'M00079', N'Sụn Gà Bắp Chiên Giòn', N'버삭한 옥수수 닭 오돌뼈', 45000, 'MenuItem_M00079.webp', 9, 0, 0),
(N'M00078', N'Đùi Gà Giòn', N'닭 다리 후라이드', 39000, 'MenuItem_M00078.webp', 9, 0, 0),
(N'M00080', N'Viên Thanh Cua Phô Mai', N'크랩스틱 치즈볼', 45000, 'MenuItem_M00080.webp', 9, 0, 0),
(N'M00075', N'Xiên Bánh Cá Hầm', N'어묵', 42000, 'MenuItem_M00075.webp', 9, 0, 0),
(N'M00077', N'Salad Xốt Mè Rang', N'+ Gà Fillet 12.000 VNĐ', 35000, 'MenuItem_M00077.webp', 9, 0, 0);
GO

-- Giải Khát (MaDM = 10)
INSERT INTO SanPham (MaSPCode, TenSP, MoTa, DonGia, HinhAnh, MaDM, CapDoCay, NoiBat) VALUES 
(N'M00085', N'Nước Gạo Hàn Quốc', N'달콤한 쌀 음료', 35000, 'MenuItem_M00085.webp', 10, 0, 0),
(N'M00097', N'Nước Gạo Hoa Anh Đào', N'사쿠라 찹쌀 음료', 35000, 'MenuItem_M00097.webp', 10, 0, 0),
(N'M00082', N'Soda Dâu Dưa Lưới', N'딸기 메론 소다', 35000, 'MenuItem_M00082.webp', 10, 0, 0),
(N'M00083', N'Soda Dừa Dứa Đác Thơm', N'파인애플 코코넛 소다와 사탕야자 씨앗', 35000, 'MenuItem_M00083.webp', 10, 0, 0),
(N'M00084', N'Soda Thơm Lừng', N'멜론 파인애플 소다', 35000, 'MenuItem_M00084.webp', 10, 0, 0),
(N'M00115', N'Sting', N'Sting lon', 29000, 'MenuItem_M00115.webp', 10, 0, 0),
(N'M00087', N'Trà Dâu Đào', N'딸기 히비스커스 홍차', 29000, 'MenuItem_M00087.webp', 10, 0, 0),
(N'M00086', N'Trà Đào Sasin', N'복숭아 홍차', 29000, 'MenuItem_M00086.webp', 10, 0, 0),
(N'M00089', N'Trà Sữa Trân Châu Sasin', N'밀크 티', 29000, 'MenuItem_M00089.webp', 10, 0, 0),
(N'M00088', N'Trà Sữa Matcha Trân Châu Sasin', N'말차 밀크티', 29000, 'MenuItem_M00088.webp', 10, 0, 0),
(N'M00104', N'Sprite Size R', N'Sprite size R', 23000, 'MenuItem_M00104.webp', 10, 0, 0),
(N'M00102', N'Coca Cola Size R', N'Coca Cola Size R', 23000, 'MenuItem_M00102.webp', 10, 0, 0),
(N'M00105', N'Sprite Size L', N'Sprite size L', 27000, 'MenuItem_M00105.webp', 10, 0, 0),
(N'M00103', N'Coca Cola Size L', N'Coca cola size L', 27000, 'MenuItem_M00103.webp', 10, 0, 0),
(N'M00106', N'Coca Cola', N'Coca cola', 29000, 'MenuItem_M00106.webp', 10, 0, 0),
(N'M00107', N'Sprite', N'Sprite', 29000, 'MenuItem_M00107.webp', 10, 0, 0),
(N'M00108', N'Samurai Dâu', N'Samurai dâu', 29000, 'MenuItem_M00108.webp', 10, 0, 0);
GO

-- 10. Đơn hàng mẫu
INSERT INTO DonHang (MaDHCode, MaKH, TenKhach, SDTKhach, DiaChiGiao, TamTinh, PhiGiaoHang, GiamGia, TongTien, TrangThai, MaCH, MaNV) VALUES 
(N'DH20241216001', 1, N'Nguyễn Thị Mai', '0988888881', N'123 Lê Lợi, Quận 1', 146000, 0, 0, 146000, N'Hoàn thành', 1, 1),
(N'DH20241216002', 2, N'Trần Văn Hùng', '0988888882', N'456 Hai Bà Trưng, Quận 3', 238000, 15000, 0, 253000, N'Đang giao', 1, 2),
(N'DH20241216003', NULL, N'Khách vãng lai', '0999999999', N'789 CMT8, Quận 10', 69000, 15000, 10000, 74000, N'Chờ xác nhận', 1, NULL);
GO

-- 11. Chi tiết đơn hàng mẫu
INSERT INTO ChiTietDonHang (MaDH, MaSP, TenSP, SoLuong, DonGia, CapDoCay, GhiChu) VALUES 
(1, 1, N'Mì Thập Cẩm No Nê', 1, 77000, 3, N'Cay vừa'),
(1, 24, N'Tokbokki Phô Mai Sasin', 1, 59000, 2, NULL),
(2, 2, N'Mì Thập Cẩm', 2, 69000, 5, N'Cay nhiều'),
(2, 41, N'Combo Vui Vẻ', 1, 69000, 3, NULL),
(3, 41, N'Combo Vui Vẻ', 1, 69000, 3, N'Không hành');
GO

-- 12. Báo cáo mẫu
INSERT INTO BaoCao (MaCH, MaQL, LoaiBaoCao, TuNgay, DenNgay, TongDonHang, TongDoanhThu, TongChiPhi, LoiNhuan) VALUES 
(1, 1, N'Ngày', '2024-12-16', '2024-12-16', 3, 473000, 150000, 323000);
GO

PRINT N'Tạo cơ sở dữ liệu MyCayDB thành công!';
PRINT N'Tổng số sản phẩm: 100';
PRINT N'Tài khoản mẫu: admin/123456, quanly1/123456, nhanvien1/123456, khachhang1/123456';
GO
