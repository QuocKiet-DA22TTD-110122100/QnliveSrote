-- Fix lỗi đặt hàng: Thêm các cột còn thiếu vào bảng DonHang
-- Chạy file này trong MySQL Workbench hoặc phpMyAdmin

USE MyCayDB;

-- Thêm cột MaCN (Chi nhánh xử lý đơn)
ALTER TABLE DonHang ADD COLUMN IF NOT EXISTS MaCN INT NULL;

-- Thêm cột MaMGG (Mã giảm giá đã áp dụng)
ALTER TABLE DonHang ADD COLUMN IF NOT EXISTS MaMGG INT NULL;

-- Thêm cột MaGiamGiaCode (Lưu mã code để hiển thị)
ALTER TABLE DonHang ADD COLUMN IF NOT EXISTS MaGiamGiaCode VARCHAR(50) NULL;

-- Thêm foreign key (tùy chọn)
-- ALTER TABLE DonHang ADD CONSTRAINT FK_DonHang_ChiNhanh FOREIGN KEY (MaCN) REFERENCES ChiNhanh(MaCN);
-- ALTER TABLE DonHang ADD CONSTRAINT FK_DonHang_MaGiamGia FOREIGN KEY (MaMGG) REFERENCES MaGiamGia(MaMGG);

SELECT 'Đã thêm các cột MaCN, MaMGG, MaGiamGiaCode vào bảng DonHang!' AS Result;
