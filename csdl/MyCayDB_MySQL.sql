-- =====================================================
-- CƠ SỞ DỮ LIỆU MỲ CAY SASIN - MySQL Version
-- Tạo mới: 17/12/2024
-- =====================================================

-- Tạo Database
CREATE DATABASE IF NOT EXISTS MyCayDB CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE MyCayDB;

-- Xóa các bảng cũ nếu tồn tại (theo thứ tự phụ thuộc)
DROP TABLE IF EXISTS ChiTietDonHang;
DROP TABLE IF EXISTS GioHang;
DROP TABLE IF EXISTS DonHang;
DROP TABLE IF EXISTS BaoCao;
DROP TABLE IF EXISTS TonKho;
DROP TABLE IF EXISTS SanPham;
DROP TABLE IF EXISTS DanhMuc;
DROP TABLE IF EXISTS NhanVien;
DROP TABLE IF EXISTS QuanLyCuaHang;
DROP TABLE IF EXISTS KhachHang;
DROP TABLE IF EXISTS NguoiDungQuanTri;
DROP TABLE IF EXISTS TaiKhoan;
DROP TABLE IF EXISTS CuaHang;
DROP TABLE IF EXISTS VaiTro;

-- =====================================================
-- 1. BẢNG VaiTro (Vai trò người dùng)
-- =====================================================
CREATE TABLE VaiTro (
    MaVaiTro INT AUTO_INCREMENT PRIMARY KEY,
    TenVaiTro VARCHAR(50) NOT NULL UNIQUE,
    MoTa VARCHAR(200) NULL,
    NgayTao DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- =====================================================
-- 2. BẢNG TaiKhoan (Tài khoản đăng nhập)
-- =====================================================
CREATE TABLE TaiKhoan (
    MaTK INT AUTO_INCREMENT PRIMARY KEY,
    TenDangNhap VARCHAR(50) NOT NULL UNIQUE,
    MatKhau VARCHAR(255) NOT NULL,
    Email VARCHAR(100) NULL,
    TrangThai TINYINT(1) DEFAULT 1,
    MaVaiTro INT NOT NULL,
    NgayTao DATETIME DEFAULT CURRENT_TIMESTAMP,
    LanDangNhapCuoi DATETIME NULL,
    FOREIGN KEY (MaVaiTro) REFERENCES VaiTro(MaVaiTro)
);

-- =====================================================
-- 3. BẢNG CuaHang (Cửa hàng/Chi nhánh)
-- =====================================================
CREATE TABLE CuaHang (
    MaCH INT AUTO_INCREMENT PRIMARY KEY,
    TenCuaHang VARCHAR(100) NOT NULL,
    DiaChi VARCHAR(200) NOT NULL,
    SoDienThoai VARCHAR(15) NOT NULL,
    Email VARCHAR(100) NULL,
    NgayKhaiTruong DATE NULL,
    GioMoCua TIME DEFAULT '10:00:00',
    GioDongCua TIME DEFAULT '22:00:00',
    TrangThai TINYINT(1) DEFAULT 1,
    NgayTao DATETIME DEFAULT CURRENT_TIMESTAMP
);


-- =====================================================
-- 4. BẢNG NguoiDungQuanTri (Quản trị viên hệ thống)
-- =====================================================
CREATE TABLE NguoiDungQuanTri (
    MaQTV INT AUTO_INCREMENT PRIMARY KEY,
    HoTen VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    SDT VARCHAR(15) NULL,
    MaTK INT NOT NULL UNIQUE,
    NgayTao DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (MaTK) REFERENCES TaiKhoan(MaTK)
);

-- =====================================================
-- 5. BẢNG QuanLyCuaHang (Quản lý cửa hàng)
-- =====================================================
CREATE TABLE QuanLyCuaHang (
    MaQL INT AUTO_INCREMENT PRIMARY KEY,
    HoTen VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    SDT VARCHAR(15) NOT NULL,
    MaCH INT NOT NULL,
    MaTK INT NOT NULL UNIQUE,
    NgayBatDau DATE DEFAULT (CURRENT_DATE),
    TrangThai TINYINT(1) DEFAULT 1,
    FOREIGN KEY (MaCH) REFERENCES CuaHang(MaCH),
    FOREIGN KEY (MaTK) REFERENCES TaiKhoan(MaTK)
);

-- =====================================================
-- 6. BẢNG NhanVien (Nhân viên cửa hàng)
-- =====================================================
CREATE TABLE NhanVien (
    MaNV INT AUTO_INCREMENT PRIMARY KEY,
    HoTen VARCHAR(100) NOT NULL,
    NgaySinh DATE NULL,
    GioiTinh VARCHAR(10) NULL,
    SDT VARCHAR(15) NOT NULL,
    DiaChi VARCHAR(200) NULL,
    ChucVu VARCHAR(50) DEFAULT 'Nhân viên',
    Luong DECIMAL(18,2) NULL,
    MaCH INT NOT NULL,
    MaTK INT NULL UNIQUE,
    NgayVaoLam DATE DEFAULT (CURRENT_DATE),
    TrangThai TINYINT(1) DEFAULT 1,
    FOREIGN KEY (MaCH) REFERENCES CuaHang(MaCH),
    FOREIGN KEY (MaTK) REFERENCES TaiKhoan(MaTK)
);

-- =====================================================
-- 7. BẢNG KhachHang (Khách hàng)
-- =====================================================
CREATE TABLE KhachHang (
    MaKH INT AUTO_INCREMENT PRIMARY KEY,
    HoTen VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NULL,
    SDT VARCHAR(15) NOT NULL,
    DiaChi VARCHAR(200) NULL,
    NgaySinh DATE NULL,
    DiemTichLuy INT DEFAULT 0,
    MaTK INT NULL UNIQUE,
    NgayDangKy DATETIME DEFAULT CURRENT_TIMESTAMP,
    TrangThai TINYINT(1) DEFAULT 1,
    FOREIGN KEY (MaTK) REFERENCES TaiKhoan(MaTK)
);

-- =====================================================
-- 8. BẢNG DanhMuc (Danh mục sản phẩm)
-- =====================================================
CREATE TABLE DanhMuc (
    MaDM INT AUTO_INCREMENT PRIMARY KEY,
    TenDanhMuc VARCHAR(100) NOT NULL,
    MoTa VARCHAR(200) NULL,
    HinhAnh VARCHAR(255) NULL,
    ThuTu INT DEFAULT 0,
    TrangThai TINYINT(1) DEFAULT 1
);

-- =====================================================
-- 9. BẢNG SanPham (Sản phẩm/Món ăn)
-- =====================================================
CREATE TABLE SanPham (
    MaSP INT AUTO_INCREMENT PRIMARY KEY,
    MaSPCode VARCHAR(20) NULL UNIQUE,
    TenSP VARCHAR(150) NOT NULL,
    MoTa VARCHAR(500) NULL,
    DonGia DECIMAL(18,2) NOT NULL,
    GiaKhuyenMai DECIMAL(18,2) NULL,
    HinhAnh VARCHAR(255) NULL,
    MaDM INT NULL,
    CapDoCay INT DEFAULT 0,
    NoiBat TINYINT(1) DEFAULT 0,
    TrangThai TINYINT(1) DEFAULT 1,
    NgayTao DATETIME DEFAULT CURRENT_TIMESTAMP,
    NgayCapNhat DATETIME NULL,
    FOREIGN KEY (MaDM) REFERENCES DanhMuc(MaDM)
);

-- =====================================================
-- 10. BẢNG TonKho (Tồn kho theo cửa hàng)
-- =====================================================
CREATE TABLE TonKho (
    MaTonKho INT AUTO_INCREMENT PRIMARY KEY,
    MaSP INT NOT NULL,
    MaCH INT NOT NULL,
    SoLuong INT DEFAULT 0,
    NgayCapNhat DATETIME DEFAULT CURRENT_TIMESTAMP,
    UNIQUE KEY (MaSP, MaCH),
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP),
    FOREIGN KEY (MaCH) REFERENCES CuaHang(MaCH)
);

-- =====================================================
-- 11. BẢNG GioHang (Giỏ hàng)
-- =====================================================
CREATE TABLE GioHang (
    MaGH INT AUTO_INCREMENT PRIMARY KEY,
    MaKH INT NULL,
    SessionID VARCHAR(100) NULL,
    MaSP INT NOT NULL,
    SoLuong INT NOT NULL DEFAULT 1,
    CapDoCay INT DEFAULT 0,
    LoaiNuocDung VARCHAR(50) NULL,
    GhiChu VARCHAR(200) NULL,
    NgayThem DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH),
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP)
);

-- =====================================================
-- 12. BẢNG DonHang (Đơn hàng)
-- =====================================================
CREATE TABLE DonHang (
    MaDH INT AUTO_INCREMENT PRIMARY KEY,
    MaDHCode VARCHAR(20) NULL UNIQUE,
    MaKH INT NULL,
    TenKhach VARCHAR(100) NULL,
    SDTKhach VARCHAR(15) NULL,
    DiaChiGiao VARCHAR(200) NULL,
    NgayDat DATETIME DEFAULT CURRENT_TIMESTAMP,
    NgayGiao DATETIME NULL,
    TamTinh DECIMAL(18,2) DEFAULT 0,
    PhiGiaoHang DECIMAL(18,2) DEFAULT 15000,
    GiamGia DECIMAL(18,2) DEFAULT 0,
    TongTien DECIMAL(18,2) DEFAULT 0,
    PhuongThucThanhToan VARCHAR(50) DEFAULT 'Tiền mặt',
    TrangThaiThanhToan VARCHAR(50) DEFAULT 'Chưa thanh toán',
    TrangThai VARCHAR(50) DEFAULT 'Chờ xác nhận',
    GhiChu VARCHAR(500) NULL,
    MaCH INT NULL,
    MaNV INT NULL,
    NgayCapNhat DATETIME NULL,
    FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH),
    FOREIGN KEY (MaCH) REFERENCES CuaHang(MaCH),
    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV)
);

-- =====================================================
-- 13. BẢNG ChiTietDonHang (Chi tiết đơn hàng)
-- =====================================================
CREATE TABLE ChiTietDonHang (
    MaCTDH INT AUTO_INCREMENT PRIMARY KEY,
    MaDH INT NOT NULL,
    MaSP INT NOT NULL,
    TenSP VARCHAR(150) NOT NULL,
    SoLuong INT NOT NULL DEFAULT 1,
    DonGia DECIMAL(18,2) NOT NULL,
    CapDoCay INT DEFAULT 0,
    LoaiNuocDung VARCHAR(50) NULL,
    GhiChu VARCHAR(200) NULL,
    ThanhTien DECIMAL(18,2) GENERATED ALWAYS AS (SoLuong * DonGia) STORED,
    FOREIGN KEY (MaDH) REFERENCES DonHang(MaDH) ON DELETE CASCADE,
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP)
);

-- =====================================================
-- 14. BẢNG BaoCao (Báo cáo doanh thu)
-- =====================================================
CREATE TABLE BaoCao (
    MaBC INT AUTO_INCREMENT PRIMARY KEY,
    MaCH INT NOT NULL,
    MaQL INT NULL,
    LoaiBaoCao VARCHAR(50) NOT NULL,
    TuNgay DATE NOT NULL,
    DenNgay DATE NOT NULL,
    TongDonHang INT DEFAULT 0,
    TongDoanhThu DECIMAL(18,2) DEFAULT 0,
    TongChiPhi DECIMAL(18,2) DEFAULT 0,
    LoiNhuan DECIMAL(18,2) DEFAULT 0,
    GhiChu VARCHAR(500) NULL,
    NgayTao DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (MaCH) REFERENCES CuaHang(MaCH),
    FOREIGN KEY (MaQL) REFERENCES QuanLyCuaHang(MaQL)
);


-- =====================================================
-- DỮ LIỆU MẪU
-- =====================================================

-- 1. Vai trò
INSERT INTO VaiTro (TenVaiTro, MoTa) VALUES 
('QuanTriVien', 'Quản trị viên hệ thống - toàn quyền'),
('QuanLy', 'Quản lý cửa hàng - quản lý sản phẩm, đơn hàng, báo cáo'),
('NhanVien', 'Nhân viên - xem và cập nhật trạng thái đơn hàng'),
('KhachHang', 'Khách hàng - xem sản phẩm, đặt hàng');

-- 2. Tài khoản mẫu (mật khẩu: 123456 - MD5 hash)
INSERT INTO TaiKhoan (TenDangNhap, MatKhau, Email, MaVaiTro) VALUES 
('admin', 'e10adc3949ba59abbe56e057f20f883e', 'admin@mycaysasin.vn', 1),
('quanly1', 'e10adc3949ba59abbe56e057f20f883e', 'quanly1@mycaysasin.vn', 2),
('nhanvien1', 'e10adc3949ba59abbe56e057f20f883e', 'nhanvien1@mycaysasin.vn', 3),
('khachhang1', 'e10adc3949ba59abbe56e057f20f883e', 'khach1@gmail.com', 4);

-- 3. Cửa hàng
INSERT INTO CuaHang (TenCuaHang, DiaChi, SoDienThoai, Email, NgayKhaiTruong) VALUES 
('Mỳ Cay Sasin - Quận 1', '123 Nguyễn Huệ, Phường Bến Nghé, Quận 1, TP.HCM', '0901234567', 'q1@mycaysasin.vn', '2023-01-15'),
('Mỳ Cay Sasin - Quận 3', '456 Võ Văn Tần, Phường 5, Quận 3, TP.HCM', '0901234568', 'q3@mycaysasin.vn', '2023-06-01'),
('Mỳ Cay Sasin - Quận 7', '789 Nguyễn Thị Thập, Phường Tân Phú, Quận 7, TP.HCM', '0901234569', 'q7@mycaysasin.vn', '2024-01-10');

-- 4. Quản trị viên
INSERT INTO NguoiDungQuanTri (HoTen, Email, SDT, MaTK) VALUES 
('Nguyễn Văn Admin', 'admin@mycaysasin.vn', '0909000001', 1);

-- 5. Quản lý cửa hàng
INSERT INTO QuanLyCuaHang (HoTen, Email, SDT, MaCH, MaTK) VALUES 
('Trần Thị Quản Lý', 'quanly1@mycaysasin.vn', '0909000002', 1, 2);

-- 6. Nhân viên
INSERT INTO NhanVien (HoTen, NgaySinh, GioiTinh, SDT, DiaChi, ChucVu, Luong, MaCH, MaTK) VALUES 
('Lê Văn Nhân Viên', '1998-05-15', 'Nam', '0909000003', 'Quận Bình Thạnh', 'Nhân viên phục vụ', 8000000, 1, 3),
('Phạm Thị Hoa', '2000-08-20', 'Nữ', '0909000004', 'Quận 1', 'Thu ngân', 8500000, 1, NULL),
('Nguyễn Văn Bếp', '1995-03-10', 'Nam', '0909000005', 'Quận 3', 'Đầu bếp', 12000000, 1, NULL);

-- 7. Khách hàng
INSERT INTO KhachHang (HoTen, Email, SDT, DiaChi, NgaySinh, DiemTichLuy, MaTK) VALUES 
('Nguyễn Thị Mai', 'khach1@gmail.com', '0988888881', '123 Lê Lợi, Quận 1', '1995-06-15', 150, 4),
('Trần Văn Hùng', 'hung.tran@gmail.com', '0988888882', '456 Hai Bà Trưng, Quận 3', '1990-12-20', 280, NULL),
('Lê Thị Hương', 'huong.le@gmail.com', '0988888883', '789 Nguyễn Trãi, Quận 5', '1998-03-08', 50, NULL);

-- 8. Danh mục sản phẩm
INSERT INTO DanhMuc (TenDanhMuc, MoTa, HinhAnh, ThuTu) VALUES 
('Mì Cay', 'Các loại mì cay đặc trưng Hàn Quốc', 'MenuItemGroup_MG00005.webp', 1),
('Mì Tương Đen', 'Mì trộn tương đen Hàn Quốc', 'MenuItemGroup_MG00006.webp', 2),
('Mì Xào', 'Các loại mì xào', NULL, 3),
('Món Khác', 'Cơm, tokbokki và các món khác', 'MenuItemGroup_MG00007.webp', 4),
('Món Thêm Mì', 'Topping thêm cho mì', NULL, 5),
('Combo', 'Các combo tiết kiệm', 'MenuItemGroup_MG00003.webp', 6),
('Lẩu Hàn Quốc', 'Các loại lẩu Hàn Quốc', NULL, 7),
('Món Thêm Lẩu', 'Topping thêm cho lẩu', NULL, 8),
('Khai Vị', 'Món khai vị, ăn vặt', 'MenuItemGroup_MG00010.webp', 9),
('Giải Khát', 'Đồ uống, nước giải khát', 'MenuItemGroup_MG00018.webp', 10);

-- 9. Sản phẩm - Mì Cay
INSERT INTO SanPham (MaSPCode, TenSP, MoTa, DonGia, HinhAnh, MaDM, CapDoCay, NoiBat) VALUES 
('M00012', 'Mì Thập Cẩm No Nê (Kim Chi/ Soyum/ Sincay)', 'Mì cay Sasin, thịt heo, tôm, cá viên, trứng ngâm tương, thanh cua, chả cá Hàn Quốc, kim chi, nấm, súp lơ, bắp cải tím, ngò gai', 77000, 'MenuItem_M00012.webp', 1, 3, 1),
('MI0008', 'Mì Thập Cẩm (Kim chi/ Soyum/ Sincay)', 'Mì cay Sasin, Thịt bò, tôm, mực, chả cá Hàn Quốc, cá viên, kim chi, súp lơ, nấm, bắp cải tím, ngò gai', 69000, 'MenuItem_MI0008.webp', 1, 3, 1),
('M00018', 'Mì Hải Sản (Kim chi/ Soyum/ Sincay)', 'Mì cay Sasin, tôm, mực, chả cá Hàn Quốc, cá viên, kim chi, súp lơ, ngò gai, nấm, bắp cải tím', 62000, 'MenuItem_M00018.webp', 1, 3, 1),
('MI0005', 'Mì Hải Sản Thanh Cua (Kim chi/ Soyum/ Sincay)', 'Mì cay Sasin, Tôm, thanh cua, chả cá Hàn Quốc, cá viên, kim chi, súp lơ, nấm, bắp cải tím, ngò gai', 62000, 'MenuItem_MI0005.webp', 1, 3, 0),
('M00021', 'Mì Bò Mỹ (Kim chi/ Soyum/ Sincay)', 'Mì cay Sasin, thịt bò, xúc xích, cá viên, kim chi, súp lơ, nấm, bắp cải tím, ngò gai', 59000, 'MenuItem_M00021.webp', 1, 3, 0),
('M00022', 'Mì Bò Trứng (Kim chi/ Soyum/ Sincay)', 'Mì cay Sasin, thịt bò, trứng lòng đào, xúc xích, cá viên, kim chi, súp lơ, nấm, bắp cải tím, ngò gai', 65000, 'MenuItem_M00022.webp', 1, 3, 0),
('M00109', 'Mì Đùi Gà (Kim chi/ Soyum/ Sincay)', 'Mì cay Sasin, đùi gà, cá viên, kim chi, súp lơ, nấm, bắp cải tím, ngò gai, chả cá Hàn Quốc', 59000, 'MenuItem_M00109.webp', 1, 3, 0),
('MI0004', 'Mì Kim Chi Cá', 'Mì cay Sasin, phi lê cá, nấm, cá viên, kim chi, súp lơ, bắp cải tím, ngò gai', 49000, 'MenuItem_MI0004.webp', 1, 2, 0),
('M00027', 'Mì Kim Chi Gogi', 'Mì cay Sasin, thịt heo, xúc xích, kim chi, cá viên, kim chi, súp lơ, nấm, bắp cải tím, ngò gai', 49000, 'MenuItem_M00027.webp', 1, 2, 0),
('M00011', 'Mì Kim Chi Xúc Xích Cá Viên', 'Mì cay Sasin, xúc xích, kim chi, nấm, cá viên, súp lơ, bắp cải tím, chả cá Hàn Quốc, ngò gai', 39000, 'MenuItem_M00011.webp', 1, 2, 0);

-- Mì Tương Đen
INSERT INTO SanPham (MaSPCode, TenSP, MoTa, DonGia, HinhAnh, MaDM, CapDoCay, NoiBat) VALUES 
('M00015', 'Mì Trộn Tương Đen Heo Cuộn', 'Mì cay Sasin, heo cuộn, cá viên, cà rốt, ớt chuông, hành tây, hành baro', 69000, 'MenuItem_M00015.webp', 2, 0, 0),
('M00016', 'Mì Trộn Tương Đen Bò Mỹ', 'Mì cay Sasin, thịt bò, cá viên, chả cá Hàn Quốc, hành tây, ớt chuông, cà rốt, hành baro, mè', 65000, 'MenuItem_M00016.webp', 2, 0, 0),
('M00014', 'Mì Trộn Tương Đen Gà', 'Mì cay Sasin, gà, cá viên, hành tây, ớt chuông, cà rốt, hành baro, mè', 59000, 'MenuItem_M00014.webp', 2, 0, 0),
('M00013', 'Mì Trộn Tương Đen Mandu', 'Mì cay Sasin, mandu, cá viên, hành tây, ớt chuông, cà rốt, hành baro, mè', 55000, 'MenuItem_M00013.webp', 2, 0, 0);

-- Món Khác
INSERT INTO SanPham (MaSPCode, TenSP, MoTa, DonGia, HinhAnh, MaDM, CapDoCay, NoiBat) VALUES 
('MI0006', 'Tokbokki Phô Mai Sasin', 'Tokbokki, xúc xích, chả cá Hàn Quốc, bắp cải, nấm, hành baro, phô mai', 59000, 'MenuItem_MI0006.webp', 4, 2, 1),
('M00028', 'Cơm Trộn Thịt Bò Mỹ', 'Cơm, thịt bò, trứng, nấm, kim chi, rong biển, cà rốt, cải bó xôi, mè, ngò gai', 62000, 'MenuItem_M00028.webp', 4, 0, 0),
('M00020', 'Cơm và Canh Kim Chi', 'Cơm, thịt heo, chả cá Hàn Quốc, cá viên, kim chi, nấm, súp lơ, ớt chuông, hành tây, ngò gai', 62000, 'MenuItem_M00020.webp', 4, 2, 0);

-- Combo
INSERT INTO SanPham (MaSPCode, TenSP, MoTa, DonGia, HinhAnh, MaDM, CapDoCay, NoiBat) VALUES 
('M00141', 'Combo Vui Vẻ (1 người)', '1 Món tự chọn + 1 Ly Coca-cola/Sprite size L', 69000, 'MenuItem_M00141.webp', 6, 3, 1),
('M00143', 'Combo Bạn Thân (2 người)', '2 Món tự chọn thuộc nhóm mì cay + 1 phần khai vị tự chọn', 159000, 'MenuItem_M00143.webp', 6, 3, 1);

-- Khai Vị
INSERT INTO SanPham (MaSPCode, TenSP, MoTa, DonGia, HinhAnh, MaDM, CapDoCay, NoiBat) VALUES 
('M00070', 'Khoai Tây Chiên', 'Khoai tây chiên giòn', 32000, 'MenuItem_M00070.webp', 9, 0, 0),
('M00133', 'Kimbap Sasin (6 cuộn)', '6 cuộn cơm cuộn rong biển', 35000, 'MenuItem_M00133.webp', 9, 0, 0),
('M00076', 'Phô Mai Que', 'Phô mai que chiên giòn', 39000, 'MenuItem_M00076.webp', 9, 0, 0);

-- Giải Khát
INSERT INTO SanPham (MaSPCode, TenSP, MoTa, DonGia, HinhAnh, MaDM, CapDoCay, NoiBat) VALUES 
('M00085', 'Nước Gạo Hàn Quốc', 'Nước gạo truyền thống Hàn Quốc', 35000, 'MenuItem_M00085.webp', 10, 0, 0),
('M00102', 'Coca Cola Size R', 'Coca Cola size R', 23000, 'MenuItem_M00102.webp', 10, 0, 0),
('M00089', 'Trà Sữa Trân Châu Sasin', 'Trà sữa trân châu đặc biệt', 29000, 'MenuItem_M00089.webp', 10, 0, 0);

SELECT 'Tạo cơ sở dữ liệu MyCayDB thành công!' AS Message;
