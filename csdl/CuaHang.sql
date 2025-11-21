
USE QuanLyDuAn;
GO

/* =====================================================
   1. BẢNG VaiTro
===================================================== */
CREATE TABLE VaiTro (
    MaVaiTro INT IDENTITY(1,1) PRIMARY KEY,
    TenVaiTro NVARCHAR(50) NOT NULL,
    MoTa NVARCHAR(200) NULL
);

 /* =====================================================
    2. BẢNG TaiKhoan
===================================================== */
CREATE TABLE TaiKhoan (
    MaTK INT IDENTITY(1,1) PRIMARY KEY,
    TenDangNhap NVARCHAR(50) NOT NULL UNIQUE,
    MatKhau NVARCHAR(255) NOT NULL,
    TrangThai BIT DEFAULT(1),
    MaVaiTro INT,
    NgayTao DATETIME DEFAULT(GETDATE()),
    FOREIGN KEY (MaVaiTro) REFERENCES VaiTro(MaVaiTro)
);

/* =====================================================
   3. BẢNG CuaHang
===================================================== */
CREATE TABLE CuaHang (
    MaCH INT IDENTITY(1,1) PRIMARY KEY,
    TenCuaHang NVARCHAR(100) NOT NULL,
    DiaChi NVARCHAR(200) NOT NULL,
    SoDienThoai VARCHAR(15) NOT NULL,
    NgayKhaiTruong DATETIME NULL,
    TrangThai BIT DEFAULT(1)
);

/* =====================================================
   4. BẢNG QuanLyCuaHang
===================================================== */
CREATE TABLE QuanLyCuaHang (
    MaQL INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    SDT VARCHAR(15) NOT NULL,
    MaCH INT,
    MaTK INT,
    FOREIGN KEY (MaCH) REFERENCES CuaHang(MaCH),
    FOREIGN KEY (MaTK) REFERENCES TaiKhoan(MaTK)
);

/* =====================================================
   5. BẢNG NhanVien
===================================================== */
CREATE TABLE NhanVien (
    MaNV INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    NgaySinh DATE NULL,
    GioiTinh NVARCHAR(10) NULL,
    SDT VARCHAR(15) NOT NULL,
    MaCH INT,
    MaTK INT,
    NgayVaoLam DATETIME DEFAULT(GETDATE()),
    FOREIGN KEY (MaCH) REFERENCES CuaHang(MaCH),
    FOREIGN KEY (MaTK) REFERENCES TaiKhoan(MaTK)
);

/* =====================================================
   6. BẢNG KhachHang
===================================================== */
CREATE TABLE KhachHang (
    MaKH INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    SDT VARCHAR(15) NOT NULL,
    DiaChi NVARCHAR(200) NULL,
    MaTK INT NULL,
    NgayDangKy DATETIME DEFAULT(GETDATE()),
    FOREIGN KEY (MaTK) REFERENCES TaiKhoan(MaTK)
);

/* =====================================================
   7. BẢNG SanPham
===================================================== */
CREATE TABLE SanPham (
    MaSP INT IDENTITY(1,1) PRIMARY KEY,
    TenSP NVARCHAR(100) NOT NULL,
    MoTa NVARCHAR(200) NULL,
    DonGia DECIMAL(18,2) NOT NULL,
    HinhAnh NVARCHAR(255) NULL,
    TrangThai BIT DEFAULT(1)
);

/* =====================================================
   8. BẢNG DonHang
===================================================== */
CREATE TABLE DonHang (
    MaDH INT IDENTITY(1,1) PRIMARY KEY,
    MaKH INT,
    NgayDat DATETIME DEFAULT(GETDATE()),
    TongTien DECIMAL(18,2) DEFAULT(0),
    TrangThai NVARCHAR(50) DEFAULT(N'Đang xử lý'),
    MaCH INT,
    MaNV INT,
    FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH),
    FOREIGN KEY (MaCH) REFERENCES CuaHang(MaCH),
    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV)
);

/* =====================================================
   9. BẢNG ChiTietDonHang
===================================================== */
CREATE TABLE ChiTietDonHang (
    MaDH INT,
    MaSP INT,
    SoLuong INT NOT NULL,
    DonGia DECIMAL(18,2) NOT NULL,
    ThanhTien AS (SoLuong * DonGia) PERSISTED,
    PRIMARY KEY (MaDH, MaSP),
    FOREIGN KEY (MaDH) REFERENCES DonHang(MaDH),
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP)
);

/* =====================================================
   🔟 BẢNG BaoCao
===================================================== */
CREATE TABLE BaoCao (
    MaBC INT IDENTITY(1,1) PRIMARY KEY,
    MaCH INT,
    MaQL INT,
    ThoiGianBaoCao DATETIME DEFAULT(GETDATE()),
    DoanhThu DECIMAL(18,2) NOT NULL,
    GhiChu NVARCHAR(200) NULL,
    FOREIGN KEY (MaCH) REFERENCES CuaHang(MaCH),
    FOREIGN KEY (MaQL) REFERENCES QuanLyCuaHang(MaQL)
);
