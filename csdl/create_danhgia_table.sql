-- Tạo bảng Đánh giá
CREATE TABLE IF NOT EXISTS DanhGia (
    MaDG INT AUTO_INCREMENT PRIMARY KEY,
    MaKH INT NULL,
    MaSP INT NULL,
    MaDH INT NULL,
    TenKhach VARCHAR(100) NOT NULL,
    SDT VARCHAR(15) NULL,
    Email VARCHAR(100) NULL,
    SoSao INT NOT NULL DEFAULT 5,
    NoiDung VARCHAR(1000) NOT NULL,
    HinhAnh VARCHAR(500) NULL,
    NgayDanhGia DATETIME DEFAULT CURRENT_TIMESTAMP,
    PhanHoi VARCHAR(1000) NULL,
    MaNVPhanHoi INT NULL,
    NgayPhanHoi DATETIME NULL,
    DaXem BOOLEAN DEFAULT FALSE,
    HienThi BOOLEAN DEFAULT TRUE,
    DaDuyet BOOLEAN DEFAULT FALSE,
    FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH) ON DELETE SET NULL,
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP) ON DELETE SET NULL,
    FOREIGN KEY (MaDH) REFERENCES DonHang(MaDH) ON DELETE SET NULL
);

-- Thêm dữ liệu mẫu
INSERT INTO DanhGia (MaKH, TenKhach, SoSao, NoiDung, DaDuyet, HienThi) VALUES
(1, 'Nguyễn Văn A', 5, 'Mì cay rất ngon, nước dùng đậm đà. Sẽ quay lại lần sau!', TRUE, TRUE),
(NULL, 'Trần Thị B', 4, 'Đồ ăn ngon, giao hàng nhanh. Chỉ tiếc là hơi ít rau.', TRUE, TRUE),
(NULL, 'Lê Văn C', 5, 'Tokbokki phô mai siêu ngon, phô mai kéo sợi cực đã!', TRUE, TRUE),
(NULL, 'Phạm Thị D', 5, 'Lần đầu ăn mì cay Sasin, cấp 5 vừa miệng. Highly recommend!', TRUE, TRUE),
(NULL, 'Hoàng Văn E', 4, 'Combo 2 người rất hời, đủ no cho 2 người ăn.', TRUE, TRUE);

-- Thêm phản hồi mẫu từ admin
UPDATE DanhGia SET PhanHoi = 'Cảm ơn bạn đã ủng hộ Mỳ Cay Sasin! Hẹn gặp lại bạn lần sau nhé! 🍜', NgayPhanHoi = NOW() WHERE MaDG = 1;
UPDATE DanhGia SET PhanHoi = 'Cảm ơn góp ý của bạn! Chúng tôi sẽ cải thiện phần rau củ trong thời gian tới. 🥬', NgayPhanHoi = NOW() WHERE MaDG = 2;
