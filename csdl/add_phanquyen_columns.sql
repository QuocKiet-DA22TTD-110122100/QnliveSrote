-- Migration: Thêm các cột hỗ trợ phân quyền đơn hàng (MySQL/MariaDB)
-- Ngày tạo: 2026-01-07

-- 1. Thêm cột MaNVXuLy vào bảng DonHang (nhân viên được phân công xử lý)
ALTER TABLE DonHang ADD COLUMN IF NOT EXISTS MaNVXuLy INT NULL;

-- 2. Thêm cột NgayPhanCong vào bảng DonHang
ALTER TABLE DonHang ADD COLUMN IF NOT EXISTS NgayPhanCong DATETIME NULL;

-- 3. Thêm cột MaCuaHang vào bảng TaiKhoan (liên kết tài khoản với cửa hàng)
ALTER TABLE TaiKhoan ADD COLUMN IF NOT EXISTS MaCuaHang INT NULL;

-- 4. Thêm Foreign Key cho MaNVXuLy -> NhanVien (nếu chưa có)
-- Kiểm tra và xóa FK cũ nếu tồn tại trước khi thêm mới
SET @fk_exists = (SELECT COUNT(*) FROM information_schema.TABLE_CONSTRAINTS 
    WHERE CONSTRAINT_SCHEMA = DATABASE() 
    AND TABLE_NAME = 'DonHang' 
    AND CONSTRAINT_NAME = 'FK_DonHang_NhanVienXuLy');

SET @sql = IF(@fk_exists = 0, 
    'ALTER TABLE DonHang ADD CONSTRAINT FK_DonHang_NhanVienXuLy FOREIGN KEY (MaNVXuLy) REFERENCES NhanVien(MaNV) ON DELETE SET NULL',
    'SELECT 1');
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- 5. Thêm Foreign Key cho MaCuaHang -> ChiNhanh (nếu chưa có)
SET @fk_exists2 = (SELECT COUNT(*) FROM information_schema.TABLE_CONSTRAINTS 
    WHERE CONSTRAINT_SCHEMA = DATABASE() 
    AND TABLE_NAME = 'TaiKhoan' 
    AND CONSTRAINT_NAME = 'FK_TaiKhoan_CuaHang');

SET @sql2 = IF(@fk_exists2 = 0, 
    'ALTER TABLE TaiKhoan ADD CONSTRAINT FK_TaiKhoan_CuaHang FOREIGN KEY (MaCuaHang) REFERENCES ChiNhanh(MaCN) ON DELETE SET NULL',
    'SELECT 1');
PREPARE stmt2 FROM @sql2;
EXECUTE stmt2;
DEALLOCATE PREPARE stmt2;

-- 6. Tạo index để tối ưu query filter
CREATE INDEX IF NOT EXISTS IX_DonHang_MaNVXuLy ON DonHang(MaNVXuLy);
CREATE INDEX IF NOT EXISTS IX_DonHang_MaCN ON DonHang(MaCN);
CREATE INDEX IF NOT EXISTS IX_TaiKhoan_MaCuaHang ON TaiKhoan(MaCuaHang);

-- Hoàn tất
SELECT 'Migration hoàn tất!' AS Result;
