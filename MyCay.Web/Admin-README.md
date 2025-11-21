# Trang Admin — Quản lý tài khoản

Đây là giao diện quản trị mẫu (frontend-only) để quản lý tài khoản, được thiết kế bằng Razor Pages (Area: `Admin`).

Truy cập:
- Chạy ứng dụng: `dotnet run --project MyCay.Web` (từ thư mục gốc giải pháp)
- Mở trình duyệt vào: `http://localhost:5000/` (root sẽ chuyển đến `/Admin/Accounts`)

Ghi chú:
- Dữ liệu hiện tại là dữ liệu mẫu trong `Index.cshtml.cs`. Thêm truy cập DB và API trong tương lai.
- Các hành động Tạo/Sửa/Xóa hiện chỉ cập nhật giao diện phía client (JS). Bạn có thể nối vào API hoặc Razor handlers theo nhu cầu.
