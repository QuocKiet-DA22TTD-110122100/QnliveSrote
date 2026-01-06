-- Script cập nhật đường dẫn hình ảnh cho tất cả sản phẩm
-- Đảm bảo tất cả sản phẩm có hình ảnh đúng định dạng

SET NAMES utf8mb4;

-- Cập nhật hình ảnh dựa trên MaSPCode
-- Format: MenuItem_{MaSPCode}.webp

UPDATE SanPham SET HinhAnh = CONCAT('MenuItem_', MaSPCode, '.webp') WHERE HinhAnh IS NULL OR HinhAnh = '';

-- Kiểm tra các sản phẩm có hình ảnh
SELECT MaSP, MaSPCode, TenSP, HinhAnh FROM SanPham ORDER BY MaSP;

-- Thống kê
SELECT 
    COUNT(*) as TongSP,
    SUM(CASE WHEN HinhAnh IS NOT NULL AND HinhAnh != '' THEN 1 ELSE 0 END) as CoHinhAnh,
    SUM(CASE WHEN HinhAnh IS NULL OR HinhAnh = '' THEN 1 ELSE 0 END) as KhongCoHinhAnh
FROM SanPham;
