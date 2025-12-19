-- =====================================================
-- DỮ LIỆU MẪU MÃ GIẢM GIÁ (MySQL)
-- =====================================================

USE MyCayDB;

-- Tạo bảng MaGiamGia nếu chưa có
CREATE TABLE IF NOT EXISTS MaGiamGia (
    MaMGG INT AUTO_INCREMENT PRIMARY KEY,
    MaCode VARCHAR(50) NOT NULL UNIQUE,
    MoTa VARCHAR(255),
    LoaiGiam VARCHAR(20) DEFAULT 'percent',
    GiaTri DECIMAL(18,2) NOT NULL,
    GiamToiDa DECIMAL(18,2),
    DonToiThieu DECIMAL(18,2) DEFAULT 0,
    SoLuong INT DEFAULT 100,
    DaSuDung INT DEFAULT 0,
    NgayBatDau DATETIME,
    NgayKetThuc DATETIME,
    TrangThai TINYINT(1) DEFAULT 1,
    NgayTao DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Xóa dữ liệu cũ
DELETE FROM MaGiamGia;

-- Thêm mã giảm giá mẫu
INSERT INTO MaGiamGia (MaCode, MoTa, LoaiGiam, GiaTri, GiamToiDa, DonToiThieu, SoLuong, NgayBatDau, NgayKetThuc) VALUES
('SASIN10', 'Giảm 10% cho đơn từ 100k', 'percent', 10, 50000, 100000, 1000, '2024-01-01', '2025-12-31'),
('SASIN20', 'Giảm 20% cho đơn từ 200k', 'percent', 20, 100000, 200000, 500, '2024-01-01', '2025-12-31'),
('SASIN30', 'Giảm 30% cho đơn từ 300k', 'percent', 30, 150000, 300000, 200, '2024-01-01', '2025-12-31'),
('FREESHIP', 'Miễn phí ship đơn từ 150k', 'freeship', 30000, NULL, 150000, 1000, '2024-01-01', '2025-12-31'),
('NEWUSER', 'Giảm 30k cho khách mới', 'fixed', 30000, NULL, 100000, 500, '2024-01-01', '2025-12-31'),
('WELCOME50', 'Giảm 50k đơn đầu tiên', 'fixed', 50000, NULL, 200000, 300, '2024-01-01', '2025-12-31'),
('XMAS2024', 'Giảm 25% mừng Giáng sinh', 'percent', 25, 100000, 150000, 200, '2024-12-20', '2024-12-26'),
('NEWYEAR25', 'Giảm 25% mừng năm mới', 'percent', 25, 100000, 150000, 300, '2024-12-28', '2025-01-05'),
('TETAM2025', 'Giảm 15% Tết Âm lịch', 'percent', 15, 80000, 100000, 500, '2025-01-25', '2025-02-10'),
('COMBO99', 'Giảm 99k combo từ 250k', 'fixed', 99000, NULL, 250000, 100, '2024-01-01', '2025-12-31');

-- Hiển thị kết quả
SELECT MaCode, MoTa, LoaiGiam, GiaTri, DonToiThieu, SoLuong, TrangThai FROM MaGiamGia;
