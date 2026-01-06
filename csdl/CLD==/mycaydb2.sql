-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Máy chủ: 127.0.0.1
-- Thời gian đã tạo: Th12 23, 2025 lúc 03:59 AM
-- Phiên bản máy phục vụ: 10.4.32-MariaDB
-- Phiên bản PHP: 8.0.30

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Cơ sở dữ liệu: `mycaydb`
--

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `baocao`
--

CREATE TABLE `baocao` (
  `MaBC` int(11) NOT NULL,
  `MaCH` int(11) NOT NULL,
  `MaQL` int(11) DEFAULT NULL,
  `LoaiBaoCao` varchar(50) NOT NULL,
  `TuNgay` date NOT NULL,
  `DenNgay` date NOT NULL,
  `TongDonHang` int(11) DEFAULT 0,
  `TongDoanhThu` decimal(18,2) DEFAULT 0.00,
  `TongChiPhi` decimal(18,2) DEFAULT 0.00,
  `LoiNhuan` decimal(18,2) DEFAULT 0.00,
  `GhiChu` varchar(500) DEFAULT NULL,
  `NgayTao` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `chinhanh`
--

CREATE TABLE `chinhanh` (
  `MaCN` int(11) NOT NULL,
  `TenChiNhanh` varchar(100) NOT NULL,
  `DiaChi` varchar(255) DEFAULT NULL,
  `QuanHuyen` varchar(100) DEFAULT NULL,
  `ThanhPho` varchar(100) DEFAULT NULL,
  `SoDienThoai` varchar(20) DEFAULT NULL,
  `Email` varchar(100) DEFAULT NULL,
  `GioMoCua` varchar(50) DEFAULT '10:00',
  `GioDongCua` varchar(50) DEFAULT '22:00',
  `TrangThai` tinyint(1) DEFAULT 1,
  `NgayTao` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `chinhanh`
--

INSERT INTO `chinhanh` (`MaCN`, `TenChiNhanh`, `DiaChi`, `QuanHuyen`, `ThanhPho`, `SoDienThoai`, `Email`, `GioMoCua`, `GioDongCua`, `TrangThai`, `NgayTao`) VALUES
(1, 'Mỳ Cay Sasin - Quận 1', '123 Nguyễn Huệ', 'Quận 1', 'TP.HCM', '0901234567', 'q1@mycaysasin.vn', '10:00', '22:00', 1, '2025-12-20 10:12:16'),
(2, 'Mỳ Cay Sasin - Quận 3', '456 Võ Văn Tần', 'Quận 3', 'TP.HCM', '0901234568', 'q3@mycaysasin.vn', '10:00', '22:00', 1, '2025-12-20 10:12:16'),
(3, 'Mỳ Cay Sasin - Quận 7', '789 Nguyễn Thị Thập', 'Quận 7', 'TP.HCM', '0901234569', 'q7@mycaysasin.vn', '10:00', '22:00', 1, '2025-12-20 10:12:16'),
(4, 'Mỳ Cay Sasin - Thủ Đức', '321 Võ Văn Ngân', 'TP. Thủ Đức', 'TP.HCM', '0901234570', 'thuduc@mycaysasin.vn', '10:00', '22:00', 1, '2025-12-20 10:12:16'),
(5, 'Mỳ Cay Sasin - Bình Thạnh', '654 Xô Viết Nghệ Tĩnh', 'Bình Thạnh', 'TP.HCM', '0901234571', 'binhthanh@mycaysasin.vn', '10:00', '22:00', 1, '2025-12-20 10:12:16');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `chitietdonhang`
--

CREATE TABLE `chitietdonhang` (
  `MaCTDH` int(11) NOT NULL,
  `MaDH` int(11) NOT NULL,
  `MaSP` int(11) NOT NULL,
  `TenSP` varchar(150) NOT NULL,
  `SoLuong` int(11) NOT NULL DEFAULT 1,
  `DonGia` decimal(18,2) NOT NULL,
  `CapDoCay` int(11) DEFAULT 0,
  `LoaiNuocDung` varchar(50) DEFAULT NULL,
  `GhiChu` varchar(200) DEFAULT NULL,
  `ThanhTien` decimal(18,2) GENERATED ALWAYS AS (`SoLuong` * `DonGia`) STORED
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `chitietdonhang`
--

INSERT INTO `chitietdonhang` (`MaCTDH`, `MaDH`, `MaSP`, `TenSP`, `SoLuong`, `DonGia`, `CapDoCay`, `LoaiNuocDung`, `GhiChu`) VALUES
(3, 3, 1, 'Mi Cay Cap 3', 2, 50000.00, 3, 'Nuoc dung truyen thong', NULL),
(4, 4, 32, 'Cá Viên Thêm', 1, 15000.00, 0, NULL, NULL),
(5, 4, 23, 'Tokbok-cheese Bò Mỹ', 1, 62000.00, 0, NULL, NULL),
(6, 4, 15, 'Mì Xào Hải Sản', 1, 69000.00, 4, 'Kim Chi', NULL),
(7, 4, 6, 'Mì Bò Trứng (Kim chi/ Soyum/ Sincay)', 2, 65000.00, 3, 'Soyum', NULL),
(8, 5, 1, 'Mon 1', 1, 50000.00, 0, NULL, NULL),
(9, 6, 2, 'Mon 2', 1, 60000.00, 0, NULL, NULL),
(10, 7, 5, 'Mì Bò Mỹ (Kim chi/ Soyum/ Sincay)', 2, 59000.00, 6, 'Soyum', NULL);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `congthuc`
--

CREATE TABLE `congthuc` (
  `MaCT` int(11) NOT NULL,
  `MaSP` int(11) NOT NULL,
  `MaNVL` int(11) NOT NULL,
  `SoLuong` decimal(18,3) DEFAULT 0.000,
  `GhiChu` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `cuahang`
--

CREATE TABLE `cuahang` (
  `MaCH` int(11) NOT NULL,
  `TenCuaHang` varchar(100) NOT NULL,
  `DiaChi` varchar(200) NOT NULL,
  `SoDienThoai` varchar(15) NOT NULL,
  `Email` varchar(100) DEFAULT NULL,
  `NgayKhaiTruong` date DEFAULT NULL,
  `GioMoCua` time DEFAULT '10:00:00',
  `GioDongCua` time DEFAULT '22:00:00',
  `TrangThai` tinyint(1) DEFAULT 1,
  `NgayTao` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `cuahang`
--

INSERT INTO `cuahang` (`MaCH`, `TenCuaHang`, `DiaChi`, `SoDienThoai`, `Email`, `NgayKhaiTruong`, `GioMoCua`, `GioDongCua`, `TrangThai`, `NgayTao`) VALUES
(1, 'Mỳ Cay Sasin - Quận 1', '123 Nguyễn Huệ, Phường Bến Nghé, Quận 1, TP.HCM', '0901234567', 'q1@mycaysasin.vn', '2023-01-15', '10:00:00', '22:00:00', 1, '2025-12-20 10:11:37'),
(2, 'Mỳ Cay Sasin - Quận 3', '456 Võ Văn Tần, Phường 5, Quận 3, TP.HCM', '0901234568', 'q3@mycaysasin.vn', '2023-06-01', '10:00:00', '22:00:00', 1, '2025-12-20 10:11:37'),
(3, 'Mỳ Cay Sasin - Quận 7', '789 Nguyễn Thị Thập, Phường Tân Phú, Quận 7, TP.HCM', '0901234569', 'q7@mycaysasin.vn', '2024-01-10', '10:00:00', '22:00:00', 1, '2025-12-20 10:11:37');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `danhgia`
--

CREATE TABLE `danhgia` (
  `MaDG` int(11) NOT NULL,
  `MaKH` int(11) DEFAULT NULL,
  `MaSP` int(11) DEFAULT NULL,
  `MaDH` int(11) DEFAULT NULL,
  `TenKhach` varchar(100) NOT NULL,
  `SDT` varchar(15) DEFAULT NULL,
  `Email` varchar(100) DEFAULT NULL,
  `SoSao` int(11) NOT NULL DEFAULT 5,
  `NoiDung` varchar(1000) NOT NULL,
  `HinhAnh` varchar(500) DEFAULT NULL,
  `NgayDanhGia` datetime DEFAULT current_timestamp(),
  `PhanHoi` varchar(1000) DEFAULT NULL,
  `MaNVPhanHoi` int(11) DEFAULT NULL,
  `NgayPhanHoi` datetime DEFAULT NULL,
  `DaXem` tinyint(1) DEFAULT 0,
  `HienThi` tinyint(1) DEFAULT 1,
  `DaDuyet` tinyint(1) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `danhgia`
--

INSERT INTO `danhgia` (`MaDG`, `MaKH`, `MaSP`, `MaDH`, `TenKhach`, `SDT`, `Email`, `SoSao`, `NoiDung`, `HinhAnh`, `NgayDanhGia`, `PhanHoi`, `MaNVPhanHoi`, `NgayPhanHoi`, `DaXem`, `HienThi`, `DaDuyet`) VALUES
(1, 1, NULL, NULL, 'Nguyễn Văn A', NULL, NULL, 5, 'Mì cay rất ngon, nước dùng đậm đà. Sẽ quay lại lần sau!', NULL, '2025-12-23 09:57:06', 'Cảm ơn bạn đã ủng hộ Mỳ Cay Sasin! Hẹn gặp lại bạn lần sau nhé! 🍜', NULL, '2025-12-23 09:57:06', 0, 1, 1),
(2, NULL, NULL, NULL, 'Trần Thị B', NULL, NULL, 4, 'Đồ ăn ngon, giao hàng nhanh. Chỉ tiếc là hơi ít rau.', NULL, '2025-12-23 09:57:06', 'Cảm ơn góp ý của bạn! Chúng tôi sẽ cải thiện phần rau củ trong thời gian tới. 🥬', NULL, '2025-12-23 09:57:07', 0, 1, 1),
(3, NULL, NULL, NULL, 'Lê Văn C', NULL, NULL, 5, 'Tokbokki phô mai siêu ngon, phô mai kéo sợi cực đã!', NULL, '2025-12-23 09:57:06', NULL, NULL, NULL, 0, 1, 1),
(4, NULL, NULL, NULL, 'Phạm Thị D', NULL, NULL, 5, 'Lần đầu ăn mì cay Sasin, cấp 5 vừa miệng. Highly recommend!', NULL, '2025-12-23 09:57:06', NULL, NULL, NULL, 0, 1, 1),
(5, NULL, NULL, NULL, 'Hoàng Văn E', NULL, NULL, 4, 'Combo 2 người rất hời, đủ no cho 2 người ăn.', NULL, '2025-12-23 09:57:06', NULL, NULL, NULL, 0, 1, 1);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `danhmuc`
--

CREATE TABLE `danhmuc` (
  `MaDM` int(11) NOT NULL,
  `TenDanhMuc` varchar(100) NOT NULL,
  `MoTa` varchar(200) DEFAULT NULL,
  `HinhAnh` varchar(255) DEFAULT NULL,
  `ThuTu` int(11) DEFAULT 0,
  `TrangThai` tinyint(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `danhmuc`
--

INSERT INTO `danhmuc` (`MaDM`, `TenDanhMuc`, `MoTa`, `HinhAnh`, `ThuTu`, `TrangThai`) VALUES
(1, 'Mì Cay', 'Các loại mì cay đặc trưng Hàn Quốc', 'MenuItemGroup_MG00005.webp', 1, 1),
(2, 'Mì Tương Đen', 'Mì trộn tương đen Hàn Quốc', 'MenuItemGroup_MG00006.webp', 2, 1),
(3, 'Mì Xào', 'Các loại mì xào', NULL, 3, 1),
(4, 'Món Khác', 'Cơm, tokbokki và các món khác', 'MenuItemGroup_MG00007.webp', 4, 1),
(5, 'Món Thêm Mì', 'Topping thêm cho mì', NULL, 5, 1),
(6, 'Combo', 'Các combo tiết kiệm', 'MenuItemGroup_MG00003.webp', 6, 1),
(7, 'Lẩu Hàn Quốc', 'Các loại lẩu Hàn Quốc', NULL, 7, 1),
(8, 'Món Thêm Lẩu', 'Topping thêm cho lẩu', NULL, 8, 1),
(9, 'Khai Vị', 'Món khai vị, ăn vặt', 'MenuItemGroup_MG00010.webp', 9, 1),
(10, 'Giải Khát', 'Đồ uống, nước giải khát', 'MenuItemGroup_MG00018.webp', 10, 1);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `donhang`
--

CREATE TABLE `donhang` (
  `MaDH` int(11) NOT NULL,
  `MaDHCode` varchar(20) DEFAULT NULL,
  `MaKH` int(11) DEFAULT NULL,
  `TenKhach` varchar(100) DEFAULT NULL,
  `SDTKhach` varchar(15) DEFAULT NULL,
  `DiaChiGiao` varchar(200) DEFAULT NULL,
  `NgayDat` datetime DEFAULT current_timestamp(),
  `NgayGiao` datetime DEFAULT NULL,
  `TamTinh` decimal(18,2) DEFAULT 0.00,
  `PhiGiaoHang` decimal(18,2) DEFAULT 15000.00,
  `GiamGia` decimal(18,2) DEFAULT 0.00,
  `TongTien` decimal(18,2) DEFAULT 0.00,
  `PhuongThucThanhToan` varchar(50) DEFAULT 'Tiền mặt',
  `TrangThaiThanhToan` varchar(50) DEFAULT 'Chưa thanh toán',
  `TrangThai` varchar(50) DEFAULT 'Chờ xác nhận',
  `GhiChu` varchar(500) DEFAULT NULL,
  `MaCH` int(11) DEFAULT NULL,
  `MaNV` int(11) DEFAULT NULL,
  `NgayCapNhat` datetime DEFAULT NULL,
  `MaCN` int(11) DEFAULT NULL,
  `MaMGG` int(11) DEFAULT NULL,
  `MaGiamGiaCode` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `donhang`
--

INSERT INTO `donhang` (`MaDH`, `MaDHCode`, `MaKH`, `TenKhach`, `SDTKhach`, `DiaChiGiao`, `NgayDat`, `NgayGiao`, `TamTinh`, `PhiGiaoHang`, `GiamGia`, `TongTien`, `PhuongThucThanhToan`, `TrangThaiThanhToan`, `TrangThai`, `GhiChu`, `MaCH`, `MaNV`, `NgayCapNhat`, `MaCN`, `MaMGG`, `MaGiamGiaCode`) VALUES
(1, 'DH20251220105039', 4, 'Nguyễn Thị Mai', '0986784200', 'khóm 4 phường 5', '2025-12-20 10:50:39', NULL, 59000.00, 15000.00, 0.00, 74000.00, 'Tiền mặt (COD)', 'Chưa thanh toán', 'Đang giao', 'dung gio', NULL, NULL, '2025-12-22 21:23:31', NULL, NULL, NULL),
(2, 'DH20251220105129', 4, 'Nguyễn Thị Mai', '0986789891', 'khom 3 phuong 5', '2025-12-20 10:51:29', NULL, 130000.00, 0.00, 13000.00, 117000.00, 'Tiền mặt (COD)', 'Chưa thanh toán', 'Đang chuẩn bị', '', NULL, NULL, '2025-12-22 21:04:57', NULL, NULL, NULL),
(3, 'DH20251222210928', 1, 'Test User', '0901234567', '123 Test Street', '2025-12-22 21:09:28', NULL, 100000.00, 15000.00, 0.00, 115000.00, 'Tien mat', 'Chưa thanh toán', 'Chờ xác nhận', 'Test order', NULL, NULL, NULL, NULL, NULL, NULL),
(4, 'DH202512222125118018', 4, 'Nguyễn Thị Mai', '0986784565', 'Khóm 3 phường 5', '2025-12-22 21:25:11', NULL, 276000.00, 0.00, 0.00, 276000.00, 'Tiền mặt (COD)', 'Chưa thanh toán', 'Chờ xác nhận', 'đối', NULL, NULL, NULL, NULL, NULL, NULL),
(5, 'DH202512222126128339', 1, 'Test 1', '0901111111', 'Address 1', '2025-12-22 21:26:12', NULL, 50000.00, 15000.00, 0.00, 65000.00, 'Tien mat', 'Chưa thanh toán', 'Chờ xác nhận', NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(6, 'DH202512222126122826', 2, 'Test 2', '0902222222', 'Address 2', '2025-12-22 21:26:12', NULL, 60000.00, 15000.00, 0.00, 75000.00, 'Tien mat', 'Chưa thanh toán', 'Chờ xác nhận', NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(7, 'DH202512222150535176', 4, 'Nguyễn Thị Mai', '0551889988', 'khom 3', '2025-12-22 21:50:53', NULL, 118000.00, 0.00, 0.00, 118000.00, 'Tiền mặt (COD)', 'Chưa thanh toán', 'Chờ xác nhận', '', NULL, NULL, NULL, NULL, NULL, NULL);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `giohang`
--

CREATE TABLE `giohang` (
  `MaGH` int(11) NOT NULL,
  `MaKH` int(11) DEFAULT NULL,
  `SessionID` varchar(100) DEFAULT NULL,
  `MaSP` int(11) NOT NULL,
  `SoLuong` int(11) NOT NULL DEFAULT 1,
  `CapDoCay` int(11) DEFAULT 0,
  `LoaiNuocDung` varchar(50) DEFAULT NULL,
  `GhiChu` varchar(200) DEFAULT NULL,
  `NgayThem` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `giohang`
--

INSERT INTO `giohang` (`MaGH`, `MaKH`, `SessionID`, `MaSP`, `SoLuong`, `CapDoCay`, `LoaiNuocDung`, `GhiChu`, `NgayThem`) VALUES
(2, 4, 'sess_1765970881148_5g4fhjr54', 32, 1, 0, NULL, NULL, '2025-12-22 20:46:28'),
(3, 4, 'sess_1765970881148_5g4fhjr54', 23, 1, 0, NULL, NULL, '2025-12-22 20:46:34'),
(4, 4, 'sess_1765970881148_5g4fhjr54', 15, 1, 4, 'Kim Chi', NULL, '2025-12-22 20:46:49'),
(5, 4, 'sess_1765970881148_5g4fhjr54', 6, 2, 3, 'Soyum', NULL, '2025-12-22 21:24:39'),
(6, 4, 'sess_1766415019413_skq16y64s', 5, 2, 6, 'Soyum', NULL, '2025-12-22 21:50:19');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `khachhang`
--

CREATE TABLE `khachhang` (
  `MaKH` int(11) NOT NULL,
  `HoTen` varchar(100) NOT NULL,
  `Email` varchar(100) DEFAULT NULL,
  `SDT` varchar(15) NOT NULL,
  `DiaChi` varchar(200) DEFAULT NULL,
  `NgaySinh` date DEFAULT NULL,
  `DiemTichLuy` int(11) DEFAULT 0,
  `MaTK` int(11) DEFAULT NULL,
  `NgayDangKy` datetime DEFAULT current_timestamp(),
  `TrangThai` tinyint(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `khachhang`
--

INSERT INTO `khachhang` (`MaKH`, `HoTen`, `Email`, `SDT`, `DiaChi`, `NgaySinh`, `DiemTichLuy`, `MaTK`, `NgayDangKy`, `TrangThai`) VALUES
(1, 'Nguyễn Thị Mai', 'khach1@gmail.com', '0988888881', '123 Lê Lợi, Quận 1', '1995-06-15', 150, 4, '2025-12-20 10:11:37', 1),
(2, 'Trần Văn Hùng', 'hung.tran@gmail.com', '0988888882', '456 Hai Bà Trưng, Quận 3', '1990-12-20', 280, 11, '2025-12-20 10:11:37', 1),
(3, 'Lê Thị Hương', 'huong.le@gmail.com', '0988888883', '789 Nguyễn Trãi, Quận 5', '1998-03-08', 50, 12, '2025-12-20 10:11:37', 1),
(4, 'Phạm Minh Tuấn', 'khach4@gmail.com', '0988888884', '321 Điện Biên Phủ, Quận Bình Thạnh', '1992-04-18', 100, 13, '2025-12-20 10:12:35', 1),
(6, 'Mai Này ', 'Mai@gmail.com', '0986351480', NULL, NULL, 0, 23, '2025-12-23 09:08:40', 1);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `magiamgia`
--

CREATE TABLE `magiamgia` (
  `MaMGG` int(11) NOT NULL,
  `MaCode` varchar(50) NOT NULL,
  `MoTa` varchar(255) DEFAULT NULL,
  `LoaiGiam` varchar(20) DEFAULT 'percent',
  `GiaTri` decimal(18,2) DEFAULT 0.00,
  `GiamToiDa` decimal(18,2) DEFAULT NULL,
  `DonToiThieu` decimal(18,2) DEFAULT 0.00,
  `SoLuong` int(11) DEFAULT 100,
  `DaSuDung` int(11) DEFAULT 0,
  `NgayBatDau` datetime DEFAULT NULL,
  `NgayKetThuc` datetime DEFAULT NULL,
  `TrangThai` tinyint(1) DEFAULT 1,
  `NgayTao` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `magiamgia`
--

INSERT INTO `magiamgia` (`MaMGG`, `MaCode`, `MoTa`, `LoaiGiam`, `GiaTri`, `GiamToiDa`, `DonToiThieu`, `SoLuong`, `DaSuDung`, `NgayBatDau`, `NgayKetThuc`, `TrangThai`, `NgayTao`) VALUES
(1, 'SASIN10', 'Giảm 10% cho đơn từ 100k', 'percent', 10.00, 50000.00, 100000.00, 1000, 0, '2024-01-01 00:00:00', '2025-12-31 00:00:00', 1, '2025-12-20 10:12:16'),
(2, 'SASIN20', 'Giảm 20% cho đơn từ 200k', 'percent', 20.00, 100000.00, 200000.00, 500, 0, '2024-01-01 00:00:00', '2025-12-31 00:00:00', 1, '2025-12-20 10:12:16'),
(3, 'FREESHIP', 'Miễn phí ship đơn từ 150k', 'freeship', 30000.00, NULL, 150000.00, 2000, 0, '2024-01-01 00:00:00', '2025-12-31 00:00:00', 1, '2025-12-20 10:12:16'),
(4, 'NEWUSER', 'Giảm 30k cho khách mới', 'fixed', 30000.00, NULL, 50000.00, 5000, 0, '2024-01-01 00:00:00', '2025-12-31 00:00:00', 1, '2025-12-20 10:12:16'),
(5, 'COMBO50', 'Giảm 50k cho combo', 'fixed', 50000.00, NULL, 300000.00, 200, 0, '2024-01-01 00:00:00', '2025-12-31 00:00:00', 1, '2025-12-20 10:12:16');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `nguoidungquantri`
--

CREATE TABLE `nguoidungquantri` (
  `MaQTV` int(11) NOT NULL,
  `HoTen` varchar(100) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `SDT` varchar(15) DEFAULT NULL,
  `MaTK` int(11) NOT NULL,
  `NgayTao` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `nguoidungquantri`
--

INSERT INTO `nguoidungquantri` (`MaQTV`, `HoTen`, `Email`, `SDT`, `MaTK`, `NgayTao`) VALUES
(1, 'Nguyễn Văn Admin', 'admin@mycaysasin.vn', '0909000001', 1, '2025-12-20 10:11:37'),
(2, 'Trần Văn Admin 2', 'admin2@mycaysasin.vn', '0909000010', 5, '2025-12-20 10:12:34');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `nguyenvatlieu`
--

CREATE TABLE `nguyenvatlieu` (
  `MaNVL` int(11) NOT NULL,
  `TenNVL` varchar(100) NOT NULL,
  `DonViTinh` varchar(20) DEFAULT NULL,
  `GiaNhap` decimal(18,2) DEFAULT 0.00,
  `SoLuongToiThieu` int(11) DEFAULT 10,
  `NhomNVL` varchar(50) DEFAULT NULL,
  `TrangThai` tinyint(1) DEFAULT 1,
  `NgayTao` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `nguyenvatlieu`
--

INSERT INTO `nguyenvatlieu` (`MaNVL`, `TenNVL`, `DonViTinh`, `GiaNhap`, `SoLuongToiThieu`, `NhomNVL`, `TrangThai`, `NgayTao`) VALUES
(1, 'Mì cay Sasin', 'gói', 5000.00, 100, 'Mì', 1, '2025-12-20 10:12:16'),
(2, 'Mì tương đen', 'gói', 5500.00, 50, 'Mì', 1, '2025-12-20 10:12:16'),
(3, 'Tokbokki', 'kg', 45000.00, 10, 'Mì', 1, '2025-12-20 10:12:16'),
(4, 'Thịt bò Mỹ', 'kg', 280000.00, 5, 'Thịt', 1, '2025-12-20 10:12:16'),
(5, 'Thịt heo cuộn', 'kg', 150000.00, 5, 'Thịt', 1, '2025-12-20 10:12:16'),
(6, 'Đùi gà', 'kg', 85000.00, 10, 'Thịt', 1, '2025-12-20 10:12:16'),
(7, 'Xúc xích', 'kg', 95000.00, 5, 'Thịt', 1, '2025-12-20 10:12:16'),
(8, 'Tôm', 'kg', 180000.00, 5, 'Hải sản', 1, '2025-12-20 10:12:16'),
(9, 'Mực', 'kg', 160000.00, 5, 'Hải sản', 1, '2025-12-20 10:12:16'),
(10, 'Cá viên', 'kg', 75000.00, 10, 'Hải sản', 1, '2025-12-20 10:12:16'),
(11, 'Chả cá Hàn Quốc', 'kg', 120000.00, 5, 'Hải sản', 1, '2025-12-20 10:12:16'),
(12, 'Thanh cua', 'kg', 95000.00, 5, 'Hải sản', 1, '2025-12-20 10:12:16'),
(13, 'Kim chi', 'kg', 65000.00, 10, 'Rau củ', 1, '2025-12-20 10:12:16'),
(14, 'Nấm kim châm', 'kg', 55000.00, 5, 'Rau củ', 1, '2025-12-20 10:12:16'),
(15, 'Súp lơ xanh', 'kg', 35000.00, 5, 'Rau củ', 1, '2025-12-20 10:12:16'),
(16, 'Bắp cải tím', 'kg', 25000.00, 5, 'Rau củ', 1, '2025-12-20 10:12:16'),
(17, 'Hành tây', 'kg', 20000.00, 10, 'Rau củ', 1, '2025-12-20 10:12:16'),
(18, 'Nước dùng Kim Chi', 'lít', 25000.00, 20, 'Gia vị', 1, '2025-12-20 10:12:16'),
(19, 'Nước dùng Soyum', 'lít', 28000.00, 20, 'Gia vị', 1, '2025-12-20 10:12:16'),
(20, 'Nước dùng Sincay', 'lít', 30000.00, 20, 'Gia vị', 1, '2025-12-20 10:12:16'),
(21, 'Tương đen', 'lít', 45000.00, 10, 'Gia vị', 1, '2025-12-20 10:12:16'),
(22, 'Phô mai', 'kg', 180000.00, 5, 'Gia vị', 1, '2025-12-20 10:12:16'),
(23, 'Trứng gà', 'quả', 3500.00, 100, 'Khác', 1, '2025-12-20 10:12:16'),
(24, 'Mandu', 'kg', 85000.00, 5, 'Khác', 1, '2025-12-20 10:12:16');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `nhanvien`
--

CREATE TABLE `nhanvien` (
  `MaNV` int(11) NOT NULL,
  `HoTen` varchar(100) NOT NULL,
  `NgaySinh` date DEFAULT NULL,
  `GioiTinh` varchar(10) DEFAULT NULL,
  `SDT` varchar(15) NOT NULL,
  `DiaChi` varchar(200) DEFAULT NULL,
  `ChucVu` varchar(50) DEFAULT 'Nhân viên',
  `Luong` decimal(18,2) DEFAULT NULL,
  `MaCH` int(11) NOT NULL,
  `MaTK` int(11) DEFAULT NULL,
  `NgayVaoLam` date DEFAULT curdate(),
  `TrangThai` tinyint(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `nhanvien`
--

INSERT INTO `nhanvien` (`MaNV`, `HoTen`, `NgaySinh`, `GioiTinh`, `SDT`, `DiaChi`, `ChucVu`, `Luong`, `MaCH`, `MaTK`, `NgayVaoLam`, `TrangThai`) VALUES
(1, 'Lê Văn Nhân Viên', '1998-05-15', 'Nam', '0909000003', 'Quận Bình Thạnh', 'Nhân viên phục vụ', 8000000.00, 1, 3, '2025-12-20', 1),
(2, 'Phạm Thị Hoa', '2000-08-20', 'Nữ', '0909000004', 'Quận 1', 'Thu ngân', 8500000.00, 1, 8, '2025-12-20', 1),
(3, 'Nguyễn Văn Bếp', '1995-03-10', 'Nam', '0909000005', 'Quận 3', 'Đầu bếp', 12000000.00, 1, NULL, '2025-12-20', 1),
(4, 'Hoàng Văn Minh', '1997-07-20', 'Nam', '0909000013', 'Quận 3', 'Nhân viên phục vụ', 8000000.00, 2, 9, '2025-12-20', 1),
(5, 'Trần Thị Lan', '1999-11-05', 'Nữ', '0909000014', 'Quận 7', 'Thu ngân', 8500000.00, 3, 10, '2025-12-20', 1);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `quanlycuahang`
--

CREATE TABLE `quanlycuahang` (
  `MaQL` int(11) NOT NULL,
  `HoTen` varchar(100) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `SDT` varchar(15) NOT NULL,
  `MaCH` int(11) NOT NULL,
  `MaTK` int(11) NOT NULL,
  `NgayBatDau` date DEFAULT curdate(),
  `TrangThai` tinyint(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `quanlycuahang`
--

INSERT INTO `quanlycuahang` (`MaQL`, `HoTen`, `Email`, `SDT`, `MaCH`, `MaTK`, `NgayBatDau`, `TrangThai`) VALUES
(1, 'Trần Thị Quản Lý', 'quanly1@mycaysasin.vn', '0909000002', 1, 2, '2025-12-20', 1),
(2, 'Nguyễn Thị Quản Lý 2', 'quanly2@mycaysasin.vn', '0909000011', 2, 6, '2025-12-20', 1),
(3, 'Lê Văn Quản Lý 3', 'quanly3@mycaysasin.vn', '0909000012', 3, 7, '2025-12-20', 1);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `sanpham`
--

CREATE TABLE `sanpham` (
  `MaSP` int(11) NOT NULL,
  `MaSPCode` varchar(20) DEFAULT NULL,
  `TenSP` varchar(150) NOT NULL,
  `MoTa` varchar(500) DEFAULT NULL,
  `DonGia` decimal(18,2) NOT NULL,
  `GiaKhuyenMai` decimal(18,2) DEFAULT NULL,
  `HinhAnh` varchar(255) DEFAULT NULL,
  `MaDM` int(11) DEFAULT NULL,
  `CapDoCay` int(11) DEFAULT 0,
  `NoiBat` tinyint(1) DEFAULT 0,
  `TrangThai` tinyint(1) DEFAULT 1,
  `NgayTao` datetime DEFAULT current_timestamp(),
  `NgayCapNhat` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `sanpham`
--

INSERT INTO `sanpham` (`MaSP`, `MaSPCode`, `TenSP`, `MoTa`, `DonGia`, `GiaKhuyenMai`, `HinhAnh`, `MaDM`, `CapDoCay`, `NoiBat`, `TrangThai`, `NgayTao`, `NgayCapNhat`) VALUES
(1, 'M00012', 'Mì Thập Cẩm No Nê (Kim Chi/ Soyum/ Sincay)', 'Mì cay Sasin, thịt heo, tôm, cá viên, trứng ngâm tương, thanh cua, chả cá Hàn Quốc, kim chi, nấm, súp lơ, bắp cải tím, ngò gai', 77000.00, NULL, 'MenuItem_M00012.webp', 1, 3, 1, 1, '2025-12-20 11:01:25', NULL),
(2, 'MI0008', 'Mì Thập Cẩm (Kim chi/ Soyum/ Sincay)', 'Mì cay Sasin, Thịt bò, tôm, mực, chả cá Hàn Quốc, cá viên, kim chi, súp lơ, nấm, bắp cải tím, ngò gai', 69000.00, NULL, 'MenuItem_MI0008.webp', 1, 3, 1, 1, '2025-12-20 11:01:25', NULL),
(3, 'M00018', 'Mì Hải Sản (Kim chi/ Soyum/ Sincay)', 'Mì cay Sasin, tôm, mực, chả cá Hàn Quốc, cá viên, kim chi, súp lơ, ngò gai, nấm, bắp cải tím.', 62000.00, NULL, 'MenuItem_M00018.webp', 1, 3, 1, 1, '2025-12-20 11:01:25', NULL),
(4, 'MI0005', 'Mì Hải Sản Thanh Cua (Kim chi/ Soyum/ Sincay)', 'Mì cay Sasin, Tôm, thanh cua, chả cá Hàn Quốc, cá viên, kim chi, súp lơ, nấm, bắp cải tím, ngò gai', 62000.00, NULL, 'MenuItem_MI0005.webp', 1, 3, 0, 1, '2025-12-20 11:01:25', NULL),
(5, 'M00021', 'Mì Bò Mỹ (Kim chi/ Soyum/ Sincay)', 'Mì cay Sasin, thịt bò, xúc xích, cá viên, kim chi, súp lơ, nấm, bắp cải tím, ngò gai', 59000.00, NULL, 'MenuItem_M00021.webp', 1, 3, 0, 1, '2025-12-20 11:01:25', NULL),
(6, 'M00022', 'Mì Bò Trứng (Kim chi/ Soyum/ Sincay)', 'Mì cay Sasin, thịt bò, trứng lòng đào, xúc xích, cá viên, kim chi, súp lơ, nấm, bắp cải tím, ngò gai', 65000.00, NULL, 'MenuItem_M00022.webp', 1, 3, 0, 1, '2025-12-20 11:01:25', NULL),
(7, 'M00109', 'Mì Đùi Gà (Kim chi/ Soyum/ Sincay)', 'Mì cay Sasin, đùi gà, cá viên, kim chi, súp lơ, nấm, bắp cải tím, ngò gai, chả cá Hàn Quốc', 59000.00, NULL, 'MenuItem_M00109.webp', 1, 3, 0, 1, '2025-12-20 11:01:25', NULL),
(8, 'MI0004', 'Mì Kim Chi Cá', 'Mì cay Sasin, phi lê cá, nấm, cá viên, kim chi, súp lơ, bắp cải tím, ngò gai', 49000.00, NULL, 'MenuItem_MI0004.webp', 1, 3, 0, 1, '2025-12-20 11:01:25', NULL),
(9, 'M00027', 'Mì Kim Chi Gogi', 'Mì cay Sasin, thịt heo, xúc xích, kim chi, cá viên, kim chi, súp lơ, nấm, bắp cải tím, ngò gai', 49000.00, NULL, 'MenuItem_M00027.webp', 1, 3, 0, 1, '2025-12-20 11:01:25', NULL),
(10, 'M00011', 'Mì Kim Chi Xúc Xích Cá Viên', 'Mì cay Sasin, xúc xích, kim chi, nấm, cá viên, súp lơ, bắp cải tím, chả cá Hàn Quốc, ngò gai', 39000.00, NULL, 'MenuItem_M00011.webp', 1, 3, 0, 1, '2025-12-20 11:01:25', NULL),
(11, 'M00015', 'Mì Trộn Tương Đen Heo Cuộn', 'Mì cay Sasin, heo cuộn, cá viên, cà rốt, ớt chuông, hành tây, hành baro', 69000.00, NULL, 'MenuItem_M00015.webp', 2, 0, 0, 1, '2025-12-20 11:01:25', NULL),
(12, 'M00016', 'Mì Trộn Tương Đen Bò Mỹ', 'Mì cay Sasin, thịt bò, cá viên, chả cá Hàn Quốc, hành tây, ớt chuông, cà rốt, hành baro, mè', 65000.00, NULL, 'MenuItem_M00016.webp', 2, 0, 0, 1, '2025-12-20 11:01:25', NULL),
(13, 'M00014', 'Mì Trộn Tương Đen Gà', 'Mì cay Sasin, gà, cá viên, hành tây, ớt chuông, cà rốt, hành baro, mè', 59000.00, NULL, 'MenuItem_M00014.webp', 2, 0, 0, 1, '2025-12-20 11:01:25', NULL),
(14, 'M00013', 'Mì Trộn Tương Đen Mandu', 'Mì cay Sasin, mandu, cá viên, hành tây, ớt chuông, cà rốt, hành baro, mè', 55000.00, NULL, 'MenuItem_M00013.webp', 2, 0, 0, 1, '2025-12-20 11:01:25', NULL),
(15, 'M00130', 'Mì Xào Hải Sản', 'Mì cay Sasin, tôm, mực, chả cá HQ, cá viên, ớt chuông, hành tây, cải bó xôi, nấm, mè', 69000.00, NULL, 'MenuItem_M00130.webp', 3, 0, 0, 1, '2025-12-20 11:01:25', NULL),
(16, 'M00131', 'Mì Xào Sasin', 'Mì cay Sasin, thịt heo, xúc xích, cá viên, súp lơ, ớt chuông, hành tây, cải bó xôi, nấm', 65000.00, NULL, 'MenuItem_M00131.webp', 3, 0, 0, 1, '2025-12-20 11:01:25', NULL),
(17, 'M00132', 'Mì Trộn Xốt Phô Mai', 'Mì cay Sasin, gà, phô mai, xốt kem, cà rốt, hành baro', 62000.00, NULL, 'MenuItem_M00132.webp', 3, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(18, 'M00129', 'Miến Trộn Ngũ Sắc Hàn Quốc', 'Miến, thịt bò, xúc xích, nấm, ớt chuông, cà rốt, hành tây, hành baro, cải bó xôi, chả cá HQ, mè', 65000.00, NULL, 'MenuItem_M00129.webp', 4, 2, 0, 1, '2025-12-20 11:01:26', NULL),
(19, 'MI0002', 'Mì Tương Hàn Mandu', 'Mì cay Sasin, manudu, xúc xích, súp lơ,cải thảo, nấm, hành baro', 52000.00, NULL, 'MenuItem_MI0002.webp', 4, 2, 0, 1, '2025-12-20 11:01:26', NULL),
(20, 'MI0001', 'Mì Tương Hàn Thịt Heo Cuộn', 'Mì cay Sasin, heo cuộn, cải thảo, trứng ngâm tương, súp lơ, nấm, hành baro', 65000.00, NULL, 'MenuItem_MI0001.webp', 4, 2, 0, 1, '2025-12-20 11:01:26', NULL),
(21, 'M00028', 'Cơm Trộn Thịt Bò Mỹ', 'Cơm, thịt bò, trứng, nấm, kim chi, rong biển, cà rốt, cải bó xôi, mè, ngò gai', 62000.00, NULL, 'MenuItem_M00028.webp', 4, 2, 0, 1, '2025-12-20 11:01:26', NULL),
(22, 'M00020', 'Cơm và Canh Kim Chi', 'Cơm, thịt heo, chả cá Hàn Quốc, cá viên, kim chi, nấm, súp lơ, ớt chuông, hành tây, ngò gai', 62000.00, NULL, 'MenuItem_M00020.webp', 4, 2, 0, 1, '2025-12-20 11:01:26', NULL),
(23, 'MI0003', 'Tokbok-cheese Bò Mỹ', 'Tokbokki, thịt bò, xúc xích, cá viên, phô mai, hành baro, mè', 62000.00, NULL, 'MenuItem_MI0003.webp', 4, 2, 0, 1, '2025-12-20 11:01:26', NULL),
(24, 'MI0006', 'Tokbokki Phô Mai Sasin', 'Tokbokki, xúc xích, chả cá Hàn Quốc, bắp cải, nấm, hành baro, phô mai', 59000.00, NULL, 'MenuItem_MI0006.webp', 4, 2, 1, 1, '2025-12-20 11:01:26', NULL),
(25, 'M00030', 'Trứng Ngâm Tương', '마약 계란', 12000.00, NULL, 'MenuItem_M00030.webp', 5, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(26, 'M00139', 'Bông Cải Xanh', '브로콜리', 15000.00, NULL, 'MenuItem_M00139.webp', 5, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(27, 'M00034', 'Bắp Cải Tím', '적채', 15000.00, NULL, 'MenuItem_M00034.webp', 5, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(28, 'M00113', 'Nấm Kim Châm Thêm', '에노키타케', 15000.00, NULL, 'MenuItem_M00113.webp', 5, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(29, 'M00035', 'Mực', '오징어', 15000.00, NULL, 'MenuItem_M00035.webp', 5, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(30, 'M00032', 'Tôm Thêm', '새우', 15000.00, NULL, 'MenuItem_M00032.webp', 5, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(31, 'M00029', 'Thịt Heo Cuộn', '차슈', 15000.00, NULL, 'MenuItem_M00029.webp', 5, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(32, 'M00033', 'Cá Viên Thêm', '어육 완자', 15000.00, NULL, 'MenuItem_M00033.webp', 5, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(33, 'M00036', 'Xúc xích', '소시지', 15000.00, NULL, 'MenuItem_M00036.webp', 5, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(34, 'M00138', 'Bắp Bò', '소사태', 19000.00, NULL, 'MenuItem_M00138.webp', 5, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(35, 'M00031', 'Bò Thêm', '소고기', 19000.00, NULL, 'MenuItem_M00031.webp', 5, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(36, 'M00140', 'Combo Xiên Que', '계피 꼬치 콤보', 12000.00, NULL, 'MenuItem_M00140.webp', 5, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(37, 'M00112', 'Mì Nấu Thêm', '라면', 19000.00, NULL, 'MenuItem_M00112.webp', 5, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(38, 'M00141', 'Combo Vui Vẻ (1 người)', '1 Món tự chọn, món áp dụng: Mì cá/ Mì đùi gà/ Mì gogi/ Mì xúc xích cá viên/ Mì bò Mỹ 1 Ly Coca-cola/ Sprite size L', 69000.00, NULL, 'MenuItem_M00141.webp', 6, 0, 1, 1, '2025-12-20 11:01:26', NULL),
(39, 'M00142', 'Combo Gây Mê (1 người)', '1 Món tự chọn, món áp dụng: Miến xào/ Mì xào Sasin/ Mì xào hải sản/ Cơm và canh kim chi 1 Ly Coca-cola/ Sprite size L', 79000.00, NULL, 'MenuItem_M00142.webp', 6, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(40, 'M00143', 'Combo Bạn Thân (2 người)', '2 Món tự chọn thuộc nhóm mì cay, không bao gồm mì thập cẩm no nê 1 phân khai vị tự. chọn: Bánh bạch tuộc/ Phô mai viên/ Khoai tây chiên/ Phô mai que/ Kimbap Sasin (6 cuộn)', 159000.00, NULL, 'MenuItem_M00143.webp', 6, 0, 1, 1, '2025-12-20 11:01:26', NULL),
(41, 'M00144', 'Combo No Căng (2 người)', '2 Món tự chọn thuộc nhóm mì cay không bao gồm mì thập cẩm no nê 1 Tokbokki phô mai Sasin', 179000.00, NULL, 'MenuItem_M00144.webp', 6, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(42, 'M00145', 'Combo Gia Đình (3 người)', '2 Món tự chọn thuộc nhóm mì cay, không bao gồm mì thập cẩm no nê. 1 Món bất kỳ trong nhóm: Mì tương đen/ Mì xào/ Món chính khác 1 Phần khai vị tự chọn: Bánh bạch tuộc/ Phô mai viên/ Khoai Tây Chiên/ P', 219000.00, NULL, 'MenuItem_M00145.webp', 6, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(43, 'M00146', 'Combo Lẩu 2 Người', 'Lẩu (Hải sản/ Bò Mỹ) 1 Phần khai vị tự chọn: Bánh bạch tuộc/ Phô mai viên/ Khoai tây. chiên/ Phô mai que/ Kimbap Sasin (6 cuộn)', 225000.00, NULL, 'MenuItem_M00146.webp', 6, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(44, 'M00117', 'Lẩu Sincay Hải Sản (2 Người)', 'Mì cay Sasin, thịt bò, bò viên, cá viên, chả cá Hàn Quốc, chả cá sợi, kim chi, nấm kim châm, súp lơ, bắp cải tím', 199000.00, NULL, 'MenuItem_M00117.webp', 7, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(45, 'M00037', 'Lẩu Sincay Bò Mỹ (2 Người)', 'Mì cay Sasin, thịt bò, bò viên, cá viên, chả cá Hàn Quốc, chả cá sợi, kim chi, nấm kim châm, súp lơ, bắp cải tím.', 209000.00, NULL, 'MenuItem_M00037.webp', 7, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(46, 'M00136', 'Lẩu Kim Chi Bò Mỹ (2 Người)', 'Mì cay Sasin, thịt bò, bò viên, cá viên, chả cá Hàn Quốc, chả cá sợi, kim chi, nấm kim châm, súp lơ, bắp cải tím', 209000.00, NULL, 'MenuItem_M00136.webp', 7, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(47, 'M00038', 'Lẩu Kim Chi Hải Sản (2 Người)', 'Mì cay Sasin, tôm, mực, cá viên, kim chi, nấm kim châm, súp lơ, bắp cải tím', 209000.00, NULL, 'MenuItem_M00038.webp', 7, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(48, 'M00116', 'Lẩu Soyum Hải Sản (2 Người)', 'Mì cay Sasin, tôm, mực, cá viên, kim chi, nấm kim châm, súp lơ, bắp cải tím', 209000.00, NULL, 'MenuItem_M00116.webp', 7, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(49, 'M00119', 'Lẩu Soyum Bò Mỹ (2 Người)', 'Mì cay Sasin, thịt bò, bò viên, cá viên, chả cá Hàn Quốc, chả cá sợi, kim chi, nấm kim châm, súp lơ, bắp cải tím', 209000.00, NULL, 'MenuItem_M00119.webp', 7, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(50, 'M00068', 'Trứng Gà (1 Quả)', '계란 1개', 9000.00, NULL, 'MenuItem_M00068.webp', 8, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(51, 'M00065', 'Mì Gói', '라면', 12000.00, NULL, 'MenuItem_M00065.webp', 8, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(52, 'M00060', 'Nấm Kim Châm Thêm', '에노키타케', 25000.00, NULL, 'MenuItem_M00060.webp', 8, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(53, 'M00063', 'Cải Thảo', '배추', 25000.00, NULL, 'MenuItem_M00063.webp', 8, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(54, 'M00062', 'Bông Cải Xanh', '브로콜리', 25000.00, NULL, 'MenuItem_M00062.webp', 8, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(55, 'M00048', 'Bắp Cải Tím Thêm', '적채', 25000.00, NULL, 'MenuItem_M00048.webp', 8, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(56, 'M00056', 'Cá Viên Thêm', '어육 완자', 25000.00, NULL, 'MenuItem_M00056.webp', 8, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(57, 'M00049', 'Cá Thêm', '물고기', 25000.00, NULL, 'MenuItem_M00049.webp', 8, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(58, 'M00057', 'Mực', '오징어', 25000.00, NULL, 'MenuItem_M00057.webp', 8, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(59, 'M00052', 'Tôm Thêm', '새우', 25000.00, NULL, 'MenuItem_M00052.webp', 8, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(60, 'M00066', 'Chả Cá Hàn Quốc', '어묵', 25000.00, NULL, 'MenuItem_M00066.webp', 8, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(61, 'M00058', 'Xúc Xích Thêm', '소시지', 25000.00, NULL, 'MenuItem_M00058.webp', 8, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(62, 'M00053', 'Thanh Cua', '게맛살', 25000.00, NULL, 'MenuItem_M00053.webp', 8, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(63, 'M00137', 'Bắp Bò', '소사태', 25000.00, NULL, 'MenuItem_M00137.webp', 8, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(64, 'M00054', 'Bò Thêm', '소고기', 25000.00, NULL, 'MenuItem_M00054.webp', 8, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(65, 'M00059', 'Mandu', '만두', 25000.00, NULL, 'MenuItem_M00059.webp', 8, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(66, 'M00051', 'Tokbokki Phô Mai', '치즈 떡볶이', 25000.00, NULL, 'MenuItem_M00051.webp', 8, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(67, 'M00070', 'Khoai Tây Chiên', '감자 튀김', 32000.00, NULL, 'MenuItem_M00070.webp', 9, 2, 0, 1, '2025-12-20 11:01:26', NULL),
(68, 'M00128', 'Gà Viên Chiên Giòn (6 viên)', 'Gà Viên Chiên Giòn (6 viên)', 32000.00, NULL, 'MenuItem_M00128.webp', 9, 2, 0, 1, '2025-12-20 11:01:26', NULL),
(69, 'M00076', 'Phô Mai Que', '모짜렐라 스틱', 39000.00, NULL, 'MenuItem_M00076.webp', 9, 2, 0, 1, '2025-12-20 11:01:26', NULL),
(70, 'M00072', 'Rong Biển Cuộn Fillet Cá Chiên', '어묵김말이 튀김', 39000.00, NULL, 'MenuItem_M00072.webp', 9, 2, 0, 1, '2025-12-20 11:01:26', NULL),
(71, 'M00073', 'Phô Mai Viên', '치즈볼', 29000.00, NULL, 'MenuItem_M00073.webp', 9, 2, 0, 1, '2025-12-20 11:01:26', NULL),
(72, 'M00135', 'Combo khai vị phô mai', 'Phô mai que, phô mai viên, viên thanh cua phô mai', 49000.00, NULL, 'MenuItem_M00135.webp', 9, 2, 0, 1, '2025-12-20 11:01:26', NULL),
(73, 'M00134', 'Kimbap Sasin', '12 cuộn', 59000.00, NULL, 'MenuItem_M00134.webp', 9, 2, 0, 1, '2025-12-20 11:01:26', NULL),
(74, 'M00133', 'Kimbap Sasin', '6 cuộn', 35000.00, NULL, 'MenuItem_M00133.webp', 9, 2, 0, 1, '2025-12-20 11:01:26', NULL),
(75, 'M00069', 'Kimbap Chiên', '바삭한 김밥튀김', 45000.00, NULL, 'MenuItem_M00069.webp', 9, 2, 0, 1, '2025-12-20 11:01:26', NULL),
(76, 'M00071', 'Mandu Chiên Xốt Cay', '칠리 소스를 곁들인 튀김 만두', 35000.00, NULL, 'MenuItem_M00071.webp', 9, 2, 0, 1, '2025-12-20 11:01:26', NULL),
(77, 'M00074', 'Bánh Bạch Tuộc', '타코야키', 39000.00, NULL, 'MenuItem_M00074.webp', 9, 2, 0, 1, '2025-12-20 11:01:26', NULL),
(78, 'M00127', 'Chân Gà Xốt Hàn', 'Chân Gà Xốt Hàn', 49000.00, NULL, 'MenuItem_M00127.webp', 9, 2, 0, 1, '2025-12-20 11:01:26', NULL),
(79, 'M00079', 'Sụn Gà Bắp Chiên Giòn', '버삭한 옥수수 닭 오돌뼈', 45000.00, NULL, 'MenuItem_M00079.webp', 9, 2, 0, 1, '2025-12-20 11:01:26', NULL),
(80, 'M00078', 'Đùi Gà Giòn', '닭 다리 후라이드', 39000.00, NULL, 'MenuItem_M00078.webp', 9, 2, 0, 1, '2025-12-20 11:01:26', NULL),
(81, 'M00080', 'Viên Thanh Cua Phô Mai', '크랩스틱 치즈볼', 45000.00, NULL, 'MenuItem_M00080.webp', 9, 2, 0, 1, '2025-12-20 11:01:26', NULL),
(82, 'M00075', 'Xiên Bánh Cá Hầm', '어묵', 42000.00, NULL, 'MenuItem_M00075.webp', 9, 2, 0, 1, '2025-12-20 11:01:26', NULL),
(83, 'M00077', 'Salad Xốt Mè Rang', '+ Gà Fillet 12.000 VNĐ', 35000.00, NULL, 'MenuItem_M00077.webp', 9, 2, 0, 1, '2025-12-20 11:01:26', NULL),
(84, 'M00085', 'Nước Gạo Hàn Quốc', '달콤한 쌀 음료', 35000.00, NULL, 'MenuItem_M00085.webp', 10, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(85, 'M00097', 'Nước Gạo Hoa Anh Đào', '사쿠라 찹쌀 음료', 35000.00, NULL, 'MenuItem_M00097.webp', 10, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(86, 'M00082', 'Soda Dâu Dưa Lưới', '딸기 메론 소다', 35000.00, NULL, 'MenuItem_M00082.webp', 10, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(87, 'M00083', 'Soda Dừa Dứa Đác Thơm', '파인애플 코코넛 소다와 사탕야자 씨앗', 35000.00, NULL, 'MenuItem_M00083.webp', 10, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(88, 'M00084', 'Soda Thơm Lừng', '멜론 파인애플 소다', 35000.00, NULL, 'MenuItem_M00084.webp', 10, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(89, 'M00115', 'Sting', 'Sting lon', 29000.00, NULL, 'MenuItem_M00115.webp', 10, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(90, 'M00087', 'Trà Dâu Đào', '딸기 히비스커스 홍차', 29000.00, NULL, 'MenuItem_M00087.webp', 10, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(91, 'M00086', 'Trà Đào Sasin', '복숭아 홍차', 29000.00, NULL, 'MenuItem_M00086.webp', 10, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(92, 'M00089', 'Trà Sữa Trân Châu Sasin', '밀크 티', 29000.00, NULL, 'MenuItem_M00089.webp', 10, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(93, 'M00088', 'Trà Sữa Matcha Trân Châu Sasin', '말차 밀크티', 29000.00, NULL, 'MenuItem_M00088.webp', 10, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(94, 'M00104', 'Sprite Size R', 'Sprite size R', 23000.00, NULL, 'MenuItem_M00104.webp', 10, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(95, 'M00102', 'Coca Cola Size R', 'Coca Cola Size R', 23000.00, NULL, 'MenuItem_M00102.webp', 10, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(96, 'M00105', 'Sprite Size L', 'Sprite size L', 27000.00, NULL, 'MenuItem_M00105.webp', 10, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(97, 'M00103', 'Coca Cola Size L', 'Coca cola size L', 27000.00, NULL, 'MenuItem_M00103.webp', 10, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(98, 'M00106', 'Coca Cola', 'Coca cola', 29000.00, NULL, 'MenuItem_M00106.webp', 10, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(99, 'M00107', 'Sprite', 'Sprite', 29000.00, NULL, 'MenuItem_M00107.webp', 10, 0, 0, 1, '2025-12-20 11:01:26', NULL),
(100, 'M00108', 'Samurai Dâu', 'Samurai dâu', 29000.00, NULL, 'MenuItem_M00108.webp', 10, 0, 0, 1, '2025-12-20 11:01:26', NULL);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `taikhoan`
--

CREATE TABLE `taikhoan` (
  `MaTK` int(11) NOT NULL,
  `TenDangNhap` varchar(50) NOT NULL,
  `MatKhau` varchar(255) NOT NULL,
  `Email` varchar(100) DEFAULT NULL,
  `TrangThai` tinyint(1) DEFAULT 1,
  `MaVaiTro` int(11) NOT NULL,
  `NgayTao` datetime DEFAULT current_timestamp(),
  `LanDangNhapCuoi` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `taikhoan`
--

INSERT INTO `taikhoan` (`MaTK`, `TenDangNhap`, `MatKhau`, `Email`, `TrangThai`, `MaVaiTro`, `NgayTao`, `LanDangNhapCuoi`) VALUES
(1, 'admin', 'e10adc3949ba59abbe56e057f20f883e', 'admin@mycaysasin.vn', 1, 1, '2025-12-20 10:11:37', '2025-12-22 20:52:04'),
(2, 'quanly1', 'e10adc3949ba59abbe56e057f20f883e', 'quanly1@mycaysasin.vn', 1, 2, '2025-12-20 10:11:37', NULL),
(3, 'nhanvien1', 'e10adc3949ba59abbe56e057f20f883e', 'nhanvien1@mycaysasin.vn', 1, 3, '2025-12-20 10:11:37', '2025-12-22 21:00:31'),
(4, 'khachhang1', 'e10adc3949ba59abbe56e057f20f883e', 'khach1@gmail.com', 1, 4, '2025-12-20 10:11:37', '2025-12-22 21:50:00'),
(5, 'admin2', 'e10adc3949ba59abbe56e057f20f883e', 'admin2@mycaysasin.vn', 1, 1, '2025-12-20 10:12:34', NULL),
(6, 'quanly2', 'e10adc3949ba59abbe56e057f20f883e', 'quanly2@mycaysasin.vn', 1, 2, '2025-12-20 10:12:34', NULL),
(7, 'quanly3', 'e10adc3949ba59abbe56e057f20f883e', 'quanly3@mycaysasin.vn', 1, 2, '2025-12-20 10:12:35', NULL),
(8, 'nhanvien2', 'e10adc3949ba59abbe56e057f20f883e', 'nhanvien2@mycaysasin.vn', 1, 3, '2025-12-20 10:12:35', NULL),
(9, 'nhanvien3', 'e10adc3949ba59abbe56e057f20f883e', 'nhanvien3@mycaysasin.vn', 1, 3, '2025-12-20 10:12:35', NULL),
(10, 'nhanvien4', 'e10adc3949ba59abbe56e057f20f883e', 'nhanvien4@mycaysasin.vn', 1, 3, '2025-12-20 10:12:35', NULL),
(11, 'khachhang2', 'e10adc3949ba59abbe56e057f20f883e', 'khach2@gmail.com', 1, 4, '2025-12-20 10:12:35', '2025-12-23 09:07:56'),
(12, 'khachhang3', 'e10adc3949ba59abbe56e057f20f883e', 'khach3@gmail.com', 1, 4, '2025-12-20 10:12:35', NULL),
(13, 'khachhang4', 'e10adc3949ba59abbe56e057f20f883e', 'khach4@gmail.com', 1, 4, '2025-12-20 10:12:35', NULL),
(23, 'Mai@gmail.com', 'e10adc3949ba59abbe56e057f20f883e', 'Mai@gmail.com', 1, 4, '2025-12-23 09:08:40', '2025-12-23 09:26:42');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `tonkho`
--

CREATE TABLE `tonkho` (
  `MaTK` int(11) NOT NULL,
  `MaCN` int(11) NOT NULL,
  `MaNVL` int(11) NOT NULL,
  `SoLuong` decimal(18,2) DEFAULT 0.00,
  `NgayCapNhat` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `tonkho`
--

INSERT INTO `tonkho` (`MaTK`, `MaCN`, `MaNVL`, `SoLuong`, `NgayCapNhat`) VALUES
(1, 1, 1, 200.00, '2025-12-20 10:12:16'),
(2, 1, 2, 100.00, '2025-12-20 10:12:16'),
(3, 1, 3, 20.00, '2025-12-20 10:12:16'),
(4, 1, 4, 15.00, '2025-12-20 10:12:16'),
(5, 1, 5, 12.00, '2025-12-20 10:12:16'),
(6, 1, 6, 25.00, '2025-12-20 10:12:16'),
(7, 1, 7, 18.00, '2025-12-20 10:12:16'),
(8, 1, 8, 10.00, '2025-12-20 10:12:16'),
(9, 1, 9, 8.00, '2025-12-20 10:12:16'),
(10, 1, 10, 30.00, '2025-12-20 10:12:16'),
(11, 1, 11, 15.00, '2025-12-20 10:12:16'),
(12, 1, 12, 12.00, '2025-12-20 10:12:16'),
(13, 1, 13, 25.00, '2025-12-20 10:12:16'),
(14, 1, 14, 15.00, '2025-12-20 10:12:16'),
(15, 1, 15, 10.00, '2025-12-20 10:12:16'),
(16, 1, 16, 8.00, '2025-12-20 10:12:16'),
(17, 1, 17, 20.00, '2025-12-20 10:12:16'),
(18, 1, 18, 50.00, '2025-12-20 10:12:16'),
(19, 1, 19, 50.00, '2025-12-20 10:12:16'),
(20, 1, 20, 50.00, '2025-12-20 10:12:16'),
(21, 1, 21, 30.00, '2025-12-20 10:12:16'),
(22, 1, 22, 10.00, '2025-12-20 10:12:16'),
(23, 1, 23, 200.00, '2025-12-20 10:12:16'),
(24, 1, 24, 15.00, '2025-12-20 10:12:16');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `vaitro`
--

CREATE TABLE `vaitro` (
  `MaVaiTro` int(11) NOT NULL,
  `TenVaiTro` varchar(50) NOT NULL,
  `MoTa` varchar(200) DEFAULT NULL,
  `NgayTao` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `vaitro`
--

INSERT INTO `vaitro` (`MaVaiTro`, `TenVaiTro`, `MoTa`, `NgayTao`) VALUES
(1, 'QuanTriVien', 'Quản trị viên hệ thống - toàn quyền', '2025-12-20 10:11:37'),
(2, 'QuanLy', 'Quản lý cửa hàng - quản lý sản phẩm, đơn hàng, báo cáo', '2025-12-20 10:11:37'),
(3, 'NhanVien', 'Nhân viên - xem và cập nhật trạng thái đơn hàng', '2025-12-20 10:11:37'),
(4, 'KhachHang', 'Khách hàng - xem sản phẩm, đặt hàng', '2025-12-20 10:11:37');

--
-- Chỉ mục cho các bảng đã đổ
--

--
-- Chỉ mục cho bảng `baocao`
--
ALTER TABLE `baocao`
  ADD PRIMARY KEY (`MaBC`),
  ADD KEY `MaCH` (`MaCH`),
  ADD KEY `MaQL` (`MaQL`);

--
-- Chỉ mục cho bảng `chinhanh`
--
ALTER TABLE `chinhanh`
  ADD PRIMARY KEY (`MaCN`);

--
-- Chỉ mục cho bảng `chitietdonhang`
--
ALTER TABLE `chitietdonhang`
  ADD PRIMARY KEY (`MaCTDH`),
  ADD KEY `MaDH` (`MaDH`),
  ADD KEY `MaSP` (`MaSP`);

--
-- Chỉ mục cho bảng `congthuc`
--
ALTER TABLE `congthuc`
  ADD PRIMARY KEY (`MaCT`),
  ADD KEY `MaSP` (`MaSP`),
  ADD KEY `MaNVL` (`MaNVL`);

--
-- Chỉ mục cho bảng `cuahang`
--
ALTER TABLE `cuahang`
  ADD PRIMARY KEY (`MaCH`);

--
-- Chỉ mục cho bảng `danhgia`
--
ALTER TABLE `danhgia`
  ADD PRIMARY KEY (`MaDG`),
  ADD KEY `MaKH` (`MaKH`),
  ADD KEY `MaSP` (`MaSP`),
  ADD KEY `MaDH` (`MaDH`);

--
-- Chỉ mục cho bảng `danhmuc`
--
ALTER TABLE `danhmuc`
  ADD PRIMARY KEY (`MaDM`);

--
-- Chỉ mục cho bảng `donhang`
--
ALTER TABLE `donhang`
  ADD PRIMARY KEY (`MaDH`),
  ADD UNIQUE KEY `MaDHCode` (`MaDHCode`),
  ADD KEY `MaKH` (`MaKH`),
  ADD KEY `MaCH` (`MaCH`),
  ADD KEY `MaNV` (`MaNV`);

--
-- Chỉ mục cho bảng `giohang`
--
ALTER TABLE `giohang`
  ADD PRIMARY KEY (`MaGH`),
  ADD KEY `MaKH` (`MaKH`),
  ADD KEY `MaSP` (`MaSP`);

--
-- Chỉ mục cho bảng `khachhang`
--
ALTER TABLE `khachhang`
  ADD PRIMARY KEY (`MaKH`),
  ADD UNIQUE KEY `MaTK` (`MaTK`);

--
-- Chỉ mục cho bảng `magiamgia`
--
ALTER TABLE `magiamgia`
  ADD PRIMARY KEY (`MaMGG`),
  ADD UNIQUE KEY `MaCode` (`MaCode`);

--
-- Chỉ mục cho bảng `nguoidungquantri`
--
ALTER TABLE `nguoidungquantri`
  ADD PRIMARY KEY (`MaQTV`),
  ADD UNIQUE KEY `Email` (`Email`),
  ADD UNIQUE KEY `MaTK` (`MaTK`);

--
-- Chỉ mục cho bảng `nguyenvatlieu`
--
ALTER TABLE `nguyenvatlieu`
  ADD PRIMARY KEY (`MaNVL`);

--
-- Chỉ mục cho bảng `nhanvien`
--
ALTER TABLE `nhanvien`
  ADD PRIMARY KEY (`MaNV`),
  ADD UNIQUE KEY `MaTK` (`MaTK`),
  ADD KEY `MaCH` (`MaCH`);

--
-- Chỉ mục cho bảng `quanlycuahang`
--
ALTER TABLE `quanlycuahang`
  ADD PRIMARY KEY (`MaQL`),
  ADD UNIQUE KEY `Email` (`Email`),
  ADD UNIQUE KEY `MaTK` (`MaTK`),
  ADD KEY `MaCH` (`MaCH`);

--
-- Chỉ mục cho bảng `sanpham`
--
ALTER TABLE `sanpham`
  ADD PRIMARY KEY (`MaSP`),
  ADD UNIQUE KEY `MaSPCode` (`MaSPCode`),
  ADD KEY `MaDM` (`MaDM`);

--
-- Chỉ mục cho bảng `taikhoan`
--
ALTER TABLE `taikhoan`
  ADD PRIMARY KEY (`MaTK`),
  ADD UNIQUE KEY `TenDangNhap` (`TenDangNhap`),
  ADD KEY `MaVaiTro` (`MaVaiTro`);

--
-- Chỉ mục cho bảng `tonkho`
--
ALTER TABLE `tonkho`
  ADD PRIMARY KEY (`MaTK`),
  ADD UNIQUE KEY `uk_chinhanh_nvl` (`MaCN`,`MaNVL`),
  ADD KEY `MaNVL` (`MaNVL`);

--
-- Chỉ mục cho bảng `vaitro`
--
ALTER TABLE `vaitro`
  ADD PRIMARY KEY (`MaVaiTro`),
  ADD UNIQUE KEY `TenVaiTro` (`TenVaiTro`);

--
-- AUTO_INCREMENT cho các bảng đã đổ
--

--
-- AUTO_INCREMENT cho bảng `baocao`
--
ALTER TABLE `baocao`
  MODIFY `MaBC` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT cho bảng `chinhanh`
--
ALTER TABLE `chinhanh`
  MODIFY `MaCN` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT cho bảng `chitietdonhang`
--
ALTER TABLE `chitietdonhang`
  MODIFY `MaCTDH` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT cho bảng `congthuc`
--
ALTER TABLE `congthuc`
  MODIFY `MaCT` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT cho bảng `cuahang`
--
ALTER TABLE `cuahang`
  MODIFY `MaCH` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT cho bảng `danhgia`
--
ALTER TABLE `danhgia`
  MODIFY `MaDG` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT cho bảng `danhmuc`
--
ALTER TABLE `danhmuc`
  MODIFY `MaDM` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT cho bảng `donhang`
--
ALTER TABLE `donhang`
  MODIFY `MaDH` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=18;

--
-- AUTO_INCREMENT cho bảng `giohang`
--
ALTER TABLE `giohang`
  MODIFY `MaGH` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT cho bảng `khachhang`
--
ALTER TABLE `khachhang`
  MODIFY `MaKH` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT cho bảng `magiamgia`
--
ALTER TABLE `magiamgia`
  MODIFY `MaMGG` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT cho bảng `nguoidungquantri`
--
ALTER TABLE `nguoidungquantri`
  MODIFY `MaQTV` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT cho bảng `nguyenvatlieu`
--
ALTER TABLE `nguyenvatlieu`
  MODIFY `MaNVL` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=25;

--
-- AUTO_INCREMENT cho bảng `nhanvien`
--
ALTER TABLE `nhanvien`
  MODIFY `MaNV` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT cho bảng `quanlycuahang`
--
ALTER TABLE `quanlycuahang`
  MODIFY `MaQL` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT cho bảng `sanpham`
--
ALTER TABLE `sanpham`
  MODIFY `MaSP` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=101;

--
-- AUTO_INCREMENT cho bảng `taikhoan`
--
ALTER TABLE `taikhoan`
  MODIFY `MaTK` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=24;

--
-- AUTO_INCREMENT cho bảng `tonkho`
--
ALTER TABLE `tonkho`
  MODIFY `MaTK` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=25;

--
-- AUTO_INCREMENT cho bảng `vaitro`
--
ALTER TABLE `vaitro`
  MODIFY `MaVaiTro` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- Các ràng buộc cho các bảng đã đổ
--

--
-- Các ràng buộc cho bảng `baocao`
--
ALTER TABLE `baocao`
  ADD CONSTRAINT `baocao_ibfk_1` FOREIGN KEY (`MaCH`) REFERENCES `cuahang` (`MaCH`),
  ADD CONSTRAINT `baocao_ibfk_2` FOREIGN KEY (`MaQL`) REFERENCES `quanlycuahang` (`MaQL`);

--
-- Các ràng buộc cho bảng `chitietdonhang`
--
ALTER TABLE `chitietdonhang`
  ADD CONSTRAINT `chitietdonhang_ibfk_1` FOREIGN KEY (`MaDH`) REFERENCES `donhang` (`MaDH`) ON DELETE CASCADE,
  ADD CONSTRAINT `chitietdonhang_ibfk_2` FOREIGN KEY (`MaSP`) REFERENCES `sanpham` (`MaSP`);

--
-- Các ràng buộc cho bảng `congthuc`
--
ALTER TABLE `congthuc`
  ADD CONSTRAINT `congthuc_ibfk_1` FOREIGN KEY (`MaSP`) REFERENCES `sanpham` (`MaSP`),
  ADD CONSTRAINT `congthuc_ibfk_2` FOREIGN KEY (`MaNVL`) REFERENCES `nguyenvatlieu` (`MaNVL`);

--
-- Các ràng buộc cho bảng `danhgia`
--
ALTER TABLE `danhgia`
  ADD CONSTRAINT `danhgia_ibfk_1` FOREIGN KEY (`MaKH`) REFERENCES `khachhang` (`MaKH`) ON DELETE SET NULL,
  ADD CONSTRAINT `danhgia_ibfk_2` FOREIGN KEY (`MaSP`) REFERENCES `sanpham` (`MaSP`) ON DELETE SET NULL,
  ADD CONSTRAINT `danhgia_ibfk_3` FOREIGN KEY (`MaDH`) REFERENCES `donhang` (`MaDH`) ON DELETE SET NULL;

--
-- Các ràng buộc cho bảng `donhang`
--
ALTER TABLE `donhang`
  ADD CONSTRAINT `donhang_ibfk_1` FOREIGN KEY (`MaKH`) REFERENCES `khachhang` (`MaKH`),
  ADD CONSTRAINT `donhang_ibfk_2` FOREIGN KEY (`MaCH`) REFERENCES `cuahang` (`MaCH`),
  ADD CONSTRAINT `donhang_ibfk_3` FOREIGN KEY (`MaNV`) REFERENCES `nhanvien` (`MaNV`);

--
-- Các ràng buộc cho bảng `giohang`
--
ALTER TABLE `giohang`
  ADD CONSTRAINT `giohang_ibfk_1` FOREIGN KEY (`MaKH`) REFERENCES `khachhang` (`MaKH`),
  ADD CONSTRAINT `giohang_ibfk_2` FOREIGN KEY (`MaSP`) REFERENCES `sanpham` (`MaSP`);

--
-- Các ràng buộc cho bảng `khachhang`
--
ALTER TABLE `khachhang`
  ADD CONSTRAINT `khachhang_ibfk_1` FOREIGN KEY (`MaTK`) REFERENCES `taikhoan` (`MaTK`);

--
-- Các ràng buộc cho bảng `nguoidungquantri`
--
ALTER TABLE `nguoidungquantri`
  ADD CONSTRAINT `nguoidungquantri_ibfk_1` FOREIGN KEY (`MaTK`) REFERENCES `taikhoan` (`MaTK`);

--
-- Các ràng buộc cho bảng `nhanvien`
--
ALTER TABLE `nhanvien`
  ADD CONSTRAINT `nhanvien_ibfk_1` FOREIGN KEY (`MaCH`) REFERENCES `cuahang` (`MaCH`),
  ADD CONSTRAINT `nhanvien_ibfk_2` FOREIGN KEY (`MaTK`) REFERENCES `taikhoan` (`MaTK`);

--
-- Các ràng buộc cho bảng `quanlycuahang`
--
ALTER TABLE `quanlycuahang`
  ADD CONSTRAINT `quanlycuahang_ibfk_1` FOREIGN KEY (`MaCH`) REFERENCES `cuahang` (`MaCH`),
  ADD CONSTRAINT `quanlycuahang_ibfk_2` FOREIGN KEY (`MaTK`) REFERENCES `taikhoan` (`MaTK`);

--
-- Các ràng buộc cho bảng `sanpham`
--
ALTER TABLE `sanpham`
  ADD CONSTRAINT `sanpham_ibfk_1` FOREIGN KEY (`MaDM`) REFERENCES `danhmuc` (`MaDM`);

--
-- Các ràng buộc cho bảng `taikhoan`
--
ALTER TABLE `taikhoan`
  ADD CONSTRAINT `taikhoan_ibfk_1` FOREIGN KEY (`MaVaiTro`) REFERENCES `vaitro` (`MaVaiTro`);

--
-- Các ràng buộc cho bảng `tonkho`
--
ALTER TABLE `tonkho`
  ADD CONSTRAINT `tonkho_ibfk_1` FOREIGN KEY (`MaCN`) REFERENCES `chinhanh` (`MaCN`),
  ADD CONSTRAINT `tonkho_ibfk_2` FOREIGN KEY (`MaNVL`) REFERENCES `nguyenvatlieu` (`MaNVL`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
