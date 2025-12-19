-- Fix missing product images - Run in phpMyAdmin
-- Map missing images to existing similar images

-- M00013 - Mì Trộn Tương Đen Mandu -> use M00014 (similar)
UPDATE SanPham SET HinhAnh = 'MenuItem_M00014.webp' WHERE MaSPCode = 'M00013' AND NOT EXISTS (SELECT 1 FROM (SELECT 1) t WHERE 1=0);

-- M00031 - Bò Thêm -> use M00138 (Bắp Bò - similar)
UPDATE SanPham SET HinhAnh = 'MenuItem_M00138.webp' WHERE MaSPCode = 'M00031';

-- M00069 - Kimbap Chiên -> use M00133 (Kimbap Sasin)
UPDATE SanPham SET HinhAnh = 'MenuItem_M00133.webp' WHERE MaSPCode = 'M00069';

-- M00075 - Xiên Bánh Cá Hầm -> use M00066 (Chả Cá Hàn Quốc)
UPDATE SanPham SET HinhAnh = 'MenuItem_M00066.webp' WHERE MaSPCode = 'M00075';

-- M00115 - Sting -> use M00106 (Coca Cola)
UPDATE SanPham SET HinhAnh = 'MenuItem_M00106.webp' WHERE MaSPCode = 'M00115';

-- M00137 - Bắp Bò (món thêm lẩu) -> use M00138
UPDATE SanPham SET HinhAnh = 'MenuItem_M00138.webp' WHERE MaSPCode = 'M00137';

-- Verify updates
SELECT MaSPCode, TenSP, HinhAnh FROM SanPham WHERE MaSPCode IN ('M00013', 'M00031', 'M00069', 'M00075', 'M00115', 'M00137');
