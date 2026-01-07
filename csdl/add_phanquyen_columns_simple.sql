-- Migration: Thêm các cột hỗ trợ phân quyền đơn hàng (MySQL/MariaDB - Simple)
-- Ngày tạo: 2026-01-07
-- Chạy từng lệnh một, bỏ qua lỗi nếu cột đã tồn tại

-- 1. Thêm cột MaNVXuLy vào bảng DonHang
ALTER TABLE DonHang ADD COLUMN MaNVXuLy INT NULL;

-- 2. Thêm cột NgayPhanCong vào bảng DonHang  
ALTER TABLE DonHang ADD COLUMN NgayPhanCong DATETIME NULL;

-- 3. Thêm cột MaCuaHang vào bảng TaiKhoan
ALTER TABLE TaiKhoan ADD COLUMN MaCuaHang INT NULL;

-- 4. Thêm Foreign Key cho MaNVXuLy -> NhanVien
ALTER TABLE DonHang ADD CONSTRAINT FK_DonHang_NhanVienXuLy 
    FOREIGN KEY (MaNVXuLy) REFERENCES NhanVien(MaNV) ON DELETE SET NULL;

-- 5. Thêm Foreign Key cho MaCuaHang -> ChiNhanh
ALTER TABLE TaiKhoan ADD CONSTRAINT FK_TaiKhoan_CuaHang 
    FOREIGN KEY (MaCuaHang) REFERENCES ChiNhanh(MaCN) ON DELETE SET NULL;

-- 6. Tạo index
CREATE INDEX IX_DonHang_MaNVXuLy ON DonHang(MaNVXuLy);
CREATE INDEX IX_DonHang_MaCN ON DonHang(MaCN);
CREATE INDEX IX_TaiKhoan_MaCuaHang ON TaiKhoan(MaCuaHang);
