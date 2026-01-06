# Sơ Đồ Hoạt Động - Hệ Thống Mỳ Cay Sasin

## Danh sách các sơ đồ hoạt động

### 1. Activity_DatHang.puml
**Quy trình đặt hàng** - Mô tả luồng từ khi khách hàng xem thực đơn đến khi đặt hàng thành công.
- Xem thực đơn → Thêm giỏ hàng → Thanh toán → Xác nhận đơn

### 2. Activity_XuLyDonHang.puml
**Xử lý đơn hàng (Admin)** - Quy trình admin xử lý đơn hàng từ khi nhận đến khi hoàn thành.
- Xác nhận đơn → Chuẩn bị → Giao hàng → Hoàn thành/Hủy

### 3. Activity_DangKyDangNhap.puml
**Đăng ký & Đăng nhập** - Luồng xác thực người dùng.
- Đăng ký tài khoản mới
- Đăng nhập với email/SĐT
- Quên mật khẩu

### 4. Activity_QuanLySanPham.puml
**Quản lý sản phẩm (Admin)** - CRUD sản phẩm trong hệ thống.
- Thêm/Sửa/Xóa sản phẩm
- Tìm kiếm và lọc

### 5. Activity_ThanhToan.puml
**Quy trình thanh toán** - Chi tiết các phương thức thanh toán.
- COD (tiền mặt khi nhận hàng)
- Chuyển khoản ngân hàng
- Ví điện tử (MoMo, ZaloPay, VNPay)

### 6. Activity_DanhGiaSanPham.puml
**Đánh giá sản phẩm** - Khách hàng đánh giá sau khi mua.
- Chọn số sao
- Viết nhận xét
- Admin duyệt đánh giá

### 7. Activity_ThongKeBaoCao.puml
**Thống kê báo cáo (Admin)** - Xem và xuất báo cáo.
- Doanh thu
- Đơn hàng
- Sản phẩm bán chạy
- Khách hàng

---

## Cách sử dụng

### Online (PlantUML Server)
1. Truy cập: https://www.plantuml.com/plantuml/uml
2. Copy nội dung file .puml
3. Paste và xem kết quả

### VS Code Extension
1. Cài extension "PlantUML"
2. Mở file .puml
3. Nhấn `Alt + D` để preview

### Export hình ảnh
```bash
# Cài Java và PlantUML
java -jar plantuml.jar Activity_DatHang.puml -tpng
```

---

## Màu sắc sử dụng

| Thành phần | Màu | Hex |
|------------|-----|-----|
| Activity Background | Cam nhạt | #FFF7ED |
| Activity Border | Cam | #F97316 |
| Diamond Background | Cam nhạt | #FFEDD5 |
| Diamond Border | Cam đậm | #EA580C |
| Arrow | Xám | #374151 |

---

## Actors trong hệ thống

1. **Khách hàng** - Người dùng cuối, đặt hàng và mua sản phẩm
2. **Admin** - Quản trị viên, quản lý hệ thống
3. **Shipper** - Nhân viên giao hàng
4. **Hệ thống** - Backend xử lý logic
5. **Cổng thanh toán** - Bên thứ 3 xử lý thanh toán

---

## Liên kết với Use Case

| Activity Diagram | Use Case liên quan |
|------------------|-------------------|
| Activity_DatHang | UC_DatHang, UC_XemThucDon, UC_QuanLyGioHang |
| Activity_XuLyDonHang | UC_QuanLyDonHang, UC_CapNhatTrangThai |
| Activity_DangKyDangNhap | UC_DangKy, UC_DangNhap, UC_QuenMatKhau |
| Activity_QuanLySanPham | UC_ThemSanPham, UC_SuaSanPham, UC_XoaSanPham |
| Activity_ThanhToan | UC_ThanhToan, UC_ApDungMaGiamGia |
| Activity_DanhGiaSanPham | UC_DanhGia, UC_DuyetDanhGia |
| Activity_ThongKeBaoCao | UC_XemThongKe, UC_XuatBaoCao |
