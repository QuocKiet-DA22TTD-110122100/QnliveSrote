-- =====================================================
-- PHẦN 1: TẠO BẢNG (Chạy trước)
-- =====================================================
SET NAMES utf8mb4;

-- 1. Bảng Chi nhánh
DROP TABLE IF EXISTS TonKho;
DROP TABLE IF EXISTS CongThuc;
DROP TABLE IF EXISTS ChiNhanh;
DROP TABLE IF EXISTS NguyenVatLieu;
DROP TABLE IF EXISTS MaGiamGia;

CREATE TABLE ChiNhanh (
    MaCN INT AUTO_INCREMENT PRIMARY KEY,
    TenChiNhanh VARCHAR(100) NOT NULL,
    DiaChi VARCHAR(255),
    QuanHuyen VARCHAR(100),
    ThanhPho VARCHAR(100),
    SoDienThoai VARCHAR(20),
    Email VARCHAR(100),
    GioMoCua VARCHAR(50) DEFAULT '10:00',
    GioDongCua VARCHAR(50) DEFAULT '22:00',
    TrangThai TINYINT(1) DEFAULT 1,
    NgayTao DATETIME DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- 2. Bảng Nguyên vật liệu
CREATE TABLE NguyenVatLieu (
    MaNVL INT AUTO_INCREMENT PRIMARY KEY,
    TenNVL VARCHAR(100) NOT NULL,
    DonViTinh VARCHAR(20),
    GiaNhap DECIMAL(18,2) DEFAULT 0,
    SoLuongToiThieu INT DEFAULT 10,
    NhomNVL VARCHAR(50),
    TrangThai TINYINT(1) DEFAULT 1,
    NgayTao DATETIME DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- 3. Bảng Tồn kho (theo chi nhánh)
CREATE TABLE TonKho (
    MaTK INT AUTO_INCREMENT PRIMARY KEY,
    MaCN INT NOT NULL,
    MaNVL INT NOT NULL,
    SoLuong DECIMAL(18,2) DEFAULT 0,
    NgayCapNhat DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (MaCN) REFERENCES ChiNhanh(MaCN),
    FOREIGN KEY (MaNVL) REFERENCES NguyenVatLieu(MaNVL),
    UNIQUE KEY uk_chinhanh_nvl (MaCN, MaNVL)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- 4. Bảng Công thức (định lượng NVL cho sản phẩm)
CREATE TABLE CongThuc (
    MaCT INT AUTO_INCREMENT PRIMARY KEY,
    MaSP INT NOT NULL,
    MaNVL INT NOT NULL,
    SoLuong DECIMAL(18,3) DEFAULT 0,
    GhiChu VARCHAR(255),
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP),
    FOREIGN KEY (MaNVL) REFERENCES NguyenVatLieu(MaNVL)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- 5. Bảng Mã giảm giá
CREATE TABLE MaGiamGia (
    MaMGG INT AUTO_INCREMENT PRIMARY KEY,
    MaCode VARCHAR(50) NOT NULL UNIQUE,
    MoTa VARCHAR(255),
    LoaiGiam VARCHAR(20) DEFAULT 'percent',
    GiaTri DECIMAL(18,2) DEFAULT 0,
    GiamToiDa DECIMAL(18,2),
    DonToiThieu DECIMAL(18,2) DEFAULT 0,
    SoLuong INT DEFAULT 100,
    DaSuDung INT DEFAULT 0,
    NgayBatDau DATETIME,
    NgayKetThuc DATETIME,
    TrangThai TINYINT(1) DEFAULT 1,
    NgayTao DATETIME DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

SELECT 'Đã tạo xong 5 bảng mới!' AS Result;

-- =====================================================
-- PHẦN 2: THÊM CỘT VÀO BẢNG CŨ (Chạy sau phần 1)
-- =====================================================

-- Thêm cột vào bảng DonHang (bỏ qua nếu đã có)
-- ALTER TABLE DonHang ADD COLUMN MaCN INT;
-- ALTER TABLE DonHang ADD COLUMN MaMGG INT;
-- ALTER TABLE DonHang ADD COLUMN MaGiamGiaCode VARCHAR(50);

-- Thêm cột vào bảng NhanVien (bỏ qua nếu đã có)
-- ALTER TABLE NhanVien ADD COLUMN MaCN INT;

-- =====================================================
-- PHẦN 3: DỮ LIỆU MẪU (Chạy sau phần 1 và 2)
-- =====================================================

-- Chi nhánh mẫu
INSERT INTO ChiNhanh (TenChiNhanh, DiaChi, QuanHuyen, ThanhPho, SoDienThoai, Email) VALUES
('Mỳ Cay Sasin - Quận 1', '123 Nguyễn Huệ', 'Quận 1', 'TP.HCM', '0901234567', 'q1@mycaysasin.vn'),
('Mỳ Cay Sasin - Quận 3', '456 Võ Văn Tần', 'Quận 3', 'TP.HCM', '0901234568', 'q3@mycaysasin.vn'),
('Mỳ Cay Sasin - Quận 7', '789 Nguyễn Thị Thập', 'Quận 7', 'TP.HCM', '0901234569', 'q7@mycaysasin.vn'),
('Mỳ Cay Sasin - Thủ Đức', '321 Võ Văn Ngân', 'TP. Thủ Đức', 'TP.HCM', '0901234570', 'thuduc@mycaysasin.vn'),
('Mỳ Cay Sasin - Bình Thạnh', '654 Xô Viết Nghệ Tĩnh', 'Bình Thạnh', 'TP.HCM', '0901234571', 'binhthanh@mycaysasin.vn');

-- Nguyên vật liệu mẫu
INSERT INTO NguyenVatLieu (TenNVL, DonViTinh, GiaNhap, SoLuongToiThieu, NhomNVL) VALUES
('Mì cay Sasin', 'gói', 5000, 100, 'Mì'),
('Mì tương đen', 'gói', 5500, 50, 'Mì'),
('Tokbokki', 'kg', 45000, 10, 'Mì'),
('Thịt bò Mỹ', 'kg', 280000, 5, 'Thịt'),
('Thịt heo cuộn', 'kg', 150000, 5, 'Thịt'),
('Đùi gà', 'kg', 85000, 10, 'Thịt'),
('Xúc xích', 'kg', 95000, 5, 'Thịt'),
('Tôm', 'kg', 180000, 5, 'Hải sản'),
('Mực', 'kg', 160000, 5, 'Hải sản'),
('Cá viên', 'kg', 75000, 10, 'Hải sản'),
('Chả cá Hàn Quốc', 'kg', 120000, 5, 'Hải sản'),
('Thanh cua', 'kg', 95000, 5, 'Hải sản'),
('Kim chi', 'kg', 65000, 10, 'Rau củ'),
('Nấm kim châm', 'kg', 55000, 5, 'Rau củ'),
('Súp lơ xanh', 'kg', 35000, 5, 'Rau củ'),
('Bắp cải tím', 'kg', 25000, 5, 'Rau củ'),
('Hành tây', 'kg', 20000, 10, 'Rau củ'),
('Nước dùng Kim Chi', 'lít', 25000, 20, 'Gia vị'),
('Nước dùng Soyum', 'lít', 28000, 20, 'Gia vị'),
('Nước dùng Sincay', 'lít', 30000, 20, 'Gia vị'),
('Tương đen', 'lít', 45000, 10, 'Gia vị'),
('Phô mai', 'kg', 180000, 5, 'Gia vị'),
('Trứng gà', 'quả', 3500, 100, 'Khác'),
('Mandu', 'kg', 85000, 5, 'Khác');

-- Tồn kho mẫu cho chi nhánh 1
INSERT INTO TonKho (MaCN, MaNVL, SoLuong) VALUES
(1, 1, 200), (1, 2, 100), (1, 3, 20),
(1, 4, 15), (1, 5, 12), (1, 6, 25),
(1, 7, 18), (1, 8, 10), (1, 9, 8),
(1, 10, 30), (1, 11, 15), (1, 12, 12),
(1, 13, 25), (1, 14, 15), (1, 15, 10),
(1, 16, 8), (1, 17, 20), (1, 18, 50),
(1, 19, 50), (1, 20, 50), (1, 21, 30),
(1, 22, 10), (1, 23, 200), (1, 24, 15);

-- Mã giảm giá mẫu
INSERT INTO MaGiamGia (MaCode, MoTa, LoaiGiam, GiaTri, GiamToiDa, DonToiThieu, SoLuong, NgayBatDau, NgayKetThuc) VALUES
('SASIN10', 'Giảm 10% cho đơn từ 100k', 'percent', 10, 50000, 100000, 1000, '2024-01-01', '2025-12-31'),
('SASIN20', 'Giảm 20% cho đơn từ 200k', 'percent', 20, 100000, 200000, 500, '2024-01-01', '2025-12-31'),
('FREESHIP', 'Miễn phí ship đơn từ 150k', 'freeship', 30000, NULL, 150000, 2000, '2024-01-01', '2025-12-31'),
('NEWUSER', 'Giảm 30k cho khách mới', 'fixed', 30000, NULL, 50000, 5000, '2024-01-01', '2025-12-31'),
('COMBO50', 'Giảm 50k cho combo', 'fixed', 50000, NULL, 300000, 200, '2024-01-01', '2025-12-31');

SELECT 'Đã tạo xong tất cả bảng và dữ liệu mẫu!' AS Result;
