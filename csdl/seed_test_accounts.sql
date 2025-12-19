-- =====================================================
-- TÀI KHOẢN TEST CHO CÁC VAI TRÒ (MySQL)
-- Mật khẩu tất cả: 123456 (MD5: e10adc3949ba59abbe56e057f20f883e)
-- =====================================================

USE MyCayDB;

-- =====================================================
-- QUẢN TRỊ VIÊN (MaVaiTro = 1)
-- =====================================================
-- Đã có: admin / 123456

-- Thêm quản trị viên 2
INSERT IGNORE INTO TaiKhoan (TenDangNhap, MatKhau, Email, MaVaiTro) 
VALUES ('admin2', 'e10adc3949ba59abbe56e057f20f883e', 'admin2@mycaysasin.vn', 1);

SET @admin2_id = (SELECT MaTK FROM TaiKhoan WHERE TenDangNhap = 'admin2');
INSERT IGNORE INTO NguoiDungQuanTri (HoTen, Email, SDT, MaTK) 
VALUES ('Trần Văn Admin 2', 'admin2@mycaysasin.vn', '0909000010', @admin2_id);

-- =====================================================
-- QUẢN LÝ CỬA HÀNG (MaVaiTro = 2)
-- =====================================================
-- Đã có: quanly1 / 123456 (Quản lý chi nhánh Quận 1)

-- Quản lý chi nhánh Quận 3
INSERT IGNORE INTO TaiKhoan (TenDangNhap, MatKhau, Email, MaVaiTro) 
VALUES ('quanly2', 'e10adc3949ba59abbe56e057f20f883e', 'quanly2@mycaysasin.vn', 2);

SET @quanly2_id = (SELECT MaTK FROM TaiKhoan WHERE TenDangNhap = 'quanly2');
INSERT IGNORE INTO QuanLyCuaHang (HoTen, Email, SDT, MaCH, MaTK) 
VALUES ('Nguyễn Thị Quản Lý 2', 'quanly2@mycaysasin.vn', '0909000011', 2, @quanly2_id);

-- Quản lý chi nhánh Quận 7
INSERT IGNORE INTO TaiKhoan (TenDangNhap, MatKhau, Email, MaVaiTro) 
VALUES ('quanly3', 'e10adc3949ba59abbe56e057f20f883e', 'quanly3@mycaysasin.vn', 2);

SET @quanly3_id = (SELECT MaTK FROM TaiKhoan WHERE TenDangNhap = 'quanly3');
INSERT IGNORE INTO QuanLyCuaHang (HoTen, Email, SDT, MaCH, MaTK) 
VALUES ('Lê Văn Quản Lý 3', 'quanly3@mycaysasin.vn', '0909000012', 3, @quanly3_id);

-- =====================================================
-- NHÂN VIÊN (MaVaiTro = 3)
-- =====================================================
-- Đã có: nhanvien1 / 123456 (Nhân viên chi nhánh Quận 1)

-- Nhân viên 2 - Chi nhánh Quận 1
INSERT IGNORE INTO TaiKhoan (TenDangNhap, MatKhau, Email, MaVaiTro) 
VALUES ('nhanvien2', 'e10adc3949ba59abbe56e057f20f883e', 'nhanvien2@mycaysasin.vn', 3);

SET @nv2_id = (SELECT MaTK FROM TaiKhoan WHERE TenDangNhap = 'nhanvien2');
UPDATE NhanVien SET MaTK = @nv2_id WHERE HoTen = 'Phạm Thị Hoa' AND MaTK IS NULL;

-- Nhân viên 3 - Chi nhánh Quận 3
INSERT IGNORE INTO TaiKhoan (TenDangNhap, MatKhau, Email, MaVaiTro) 
VALUES ('nhanvien3', 'e10adc3949ba59abbe56e057f20f883e', 'nhanvien3@mycaysasin.vn', 3);

SET @nv3_id = (SELECT MaTK FROM TaiKhoan WHERE TenDangNhap = 'nhanvien3');
INSERT IGNORE INTO NhanVien (HoTen, NgaySinh, GioiTinh, SDT, DiaChi, ChucVu, Luong, MaCH, MaTK) 
VALUES ('Hoàng Văn Minh', '1997-07-20', 'Nam', '0909000013', 'Quận 3', 'Nhân viên phục vụ', 8000000, 2, @nv3_id);

-- Nhân viên 4 - Chi nhánh Quận 7
INSERT IGNORE INTO TaiKhoan (TenDangNhap, MatKhau, Email, MaVaiTro) 
VALUES ('nhanvien4', 'e10adc3949ba59abbe56e057f20f883e', 'nhanvien4@mycaysasin.vn', 3);

SET @nv4_id = (SELECT MaTK FROM TaiKhoan WHERE TenDangNhap = 'nhanvien4');
INSERT IGNORE INTO NhanVien (HoTen, NgaySinh, GioiTinh, SDT, DiaChi, ChucVu, Luong, MaCH, MaTK) 
VALUES ('Trần Thị Lan', '1999-11-05', 'Nữ', '0909000014', 'Quận 7', 'Thu ngân', 8500000, 3, @nv4_id);

-- =====================================================
-- KHÁCH HÀNG (MaVaiTro = 4)
-- =====================================================
-- Đã có: khachhang1 / 123456

-- Khách hàng 2
INSERT IGNORE INTO TaiKhoan (TenDangNhap, MatKhau, Email, MaVaiTro) 
VALUES ('khachhang2', 'e10adc3949ba59abbe56e057f20f883e', 'khach2@gmail.com', 4);

SET @kh2_id = (SELECT MaTK FROM TaiKhoan WHERE TenDangNhap = 'khachhang2');
UPDATE KhachHang SET MaTK = @kh2_id WHERE HoTen = 'Trần Văn Hùng' AND MaTK IS NULL;

-- Khách hàng 3
INSERT IGNORE INTO TaiKhoan (TenDangNhap, MatKhau, Email, MaVaiTro) 
VALUES ('khachhang3', 'e10adc3949ba59abbe56e057f20f883e', 'khach3@gmail.com', 4);

SET @kh3_id = (SELECT MaTK FROM TaiKhoan WHERE TenDangNhap = 'khachhang3');
UPDATE KhachHang SET MaTK = @kh3_id WHERE HoTen = 'Lê Thị Hương' AND MaTK IS NULL;

-- Khách hàng 4 (mới)
INSERT IGNORE INTO TaiKhoan (TenDangNhap, MatKhau, Email, MaVaiTro) 
VALUES ('khachhang4', 'e10adc3949ba59abbe56e057f20f883e', 'khach4@gmail.com', 4);

SET @kh4_id = (SELECT MaTK FROM TaiKhoan WHERE TenDangNhap = 'khachhang4');
INSERT IGNORE INTO KhachHang (HoTen, Email, SDT, DiaChi, NgaySinh, DiemTichLuy, MaTK) 
VALUES ('Phạm Minh Tuấn', 'khach4@gmail.com', '0988888884', '321 Điện Biên Phủ, Quận Bình Thạnh', '1992-04-18', 100, @kh4_id);

-- =====================================================
-- HIỂN THỊ DANH SÁCH TÀI KHOẢN
-- =====================================================
SELECT '=====================================================';
SELECT 'DANH SÁCH TÀI KHOẢN TEST - Mật khẩu: 123456';
SELECT '=====================================================';
SELECT TenDangNhap, Email, 
    CASE MaVaiTro 
        WHEN 1 THEN 'Quản trị viên'
        WHEN 2 THEN 'Quản lý'
        WHEN 3 THEN 'Nhân viên'
        WHEN 4 THEN 'Khách hàng'
    END AS VaiTro
FROM TaiKhoan ORDER BY MaVaiTro, TenDangNhap;
