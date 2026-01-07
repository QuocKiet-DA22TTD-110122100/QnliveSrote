# HỆ THỐNG PHÂN QUYỀN - MỲ CAY SASIN

## 1. CÁC VAI TRÒ (ROLES)

| Vai trò | Mã role | Mô tả |
|---------|---------|-------|
| **Quản trị hệ thống** | `admin` | Toàn quyền hệ thống |
| **Quản lý cửa hàng** | `storemanager` | Quản lý trong phạm vi cửa hàng được gán |
| **Nhân viên xử lý đơn** | `staff` | Xử lý đơn hàng được phân công |
| **Khách hàng** | `customer` | Đặt hàng, theo dõi đơn |

---

## 2. PHÂN QUYỀN CHI TIẾT

### 2.1. ADMIN - Quản trị hệ thống

**Quyền truy cập:** Tất cả trang (`*`)

| Nhóm chức năng | Quyền | Mô tả |
|----------------|-------|-------|
| **Quản lý tài khoản** | ✅ canCreateAccount | Tạo mới tài khoản |
| | ✅ canEditAccount | Sửa thông tin tài khoản |
| | ✅ canDeleteAccount | Xóa tài khoản |
| | ✅ canLockAccount | Khóa/mở khóa tài khoản |
| | ✅ canAssignRole | Gán vai trò cho tài khoản |
| **Quản lý phân quyền** | ✅ canManageRoles | Định nghĩa vai trò |
| | ✅ canViewActivityLog | Xem nhật ký hoạt động |
| **Cấu hình hệ thống** | ✅ canConfigSystem | Cấu hình website |
| | ✅ canConfigEmail | Cấu hình SMTP |
| | ✅ canConfigShipping | Cấu hình phí ship, thuế |
| **Báo cáo** | ✅ canViewAllReports | Xem tất cả báo cáo |
| | ✅ canViewSystemStats | Xem thống kê hệ thống |
| **Quản lý cửa hàng** | ✅ canManageAllStores | Quản lý tất cả cửa hàng |
| | ✅ canCreateStore | Tạo cửa hàng mới |
| | ✅ canDeleteStore | Xóa cửa hàng |
| **Quản lý sản phẩm** | ✅ canManageProducts | Quản lý sản phẩm |
| | ✅ canCreateProduct | Thêm sản phẩm |
| | ✅ canEditProduct | Sửa sản phẩm |
| | ✅ canDeleteProduct | Xóa sản phẩm |
| **Quản lý đơn hàng** | ✅ canManageAllOrders | Quản lý tất cả đơn |
| | ✅ canAssignOrderToStaff | Phân công đơn cho nhân viên |
| | ✅ canCancelOrder | Hủy đơn hàng |
| **Quản lý khách hàng** | ✅ canManageCustomers | Quản lý khách hàng |
| | ✅ canViewCustomerDetails | Xem chi tiết khách hàng |
| **Quản lý nhân viên** | ✅ canManageStaff | Quản lý nhân viên |
| | ✅ canCreateStaff | Thêm nhân viên |
| | ✅ canDeleteStaff | Xóa nhân viên |
| **Khác** | ✅ canManageCoupons | Quản lý mã giảm giá |
| | ✅ canManageInventory | Quản lý kho |
| | ✅ canManageCategories | Quản lý danh mục |
| | ✅ canManageReviews | Quản lý đánh giá |

---

### 2.2. STORE MANAGER - Quản lý cửa hàng

**Quyền truy cập:** Dashboard, Đơn hàng, Sản phẩm, Danh mục, Khách hàng, Mã giảm giá, Đánh giá, Chi nhánh, Kho, Nhân viên, Báo cáo, Thống kê

| Nhóm chức năng | Quyền | Mô tả |
|----------------|-------|-------|
| **Quản lý tài khoản** | ✅ canCreateAccount | Chỉ tạo tài khoản nhân viên |
| | ✅ canEditAccount | Chỉ sửa nhân viên cửa hàng mình |
| | ❌ canDeleteAccount | Không được xóa |
| | ✅ canLockAccount | Khóa nhân viên cửa hàng mình |
| | ❌ canAssignRole | Không được gán role admin |
| **Quản lý phân quyền** | ❌ canManageRoles | Không có quyền |
| | ✅ canViewActivityLog | Xem log cửa hàng mình |
| **Cấu hình hệ thống** | ❌ Tất cả | Không có quyền |
| **Báo cáo** | ❌ canViewAllReports | Không xem báo cáo toàn hệ thống |
| | ✅ canViewStoreReports | Chỉ xem báo cáo cửa hàng mình |
| **Quản lý cửa hàng** | ❌ canManageAllStores | Không quản lý tất cả |
| | ✅ canManageOwnStore | Quản lý cửa hàng được gán |
| **Quản lý sản phẩm** | ✅ Tất cả | Trong phạm vi cửa hàng |
| **Quản lý đơn hàng** | ✅ canManageStoreOrders | Đơn hàng cửa hàng mình |
| | ✅ canAssignOrderToStaff | Phân công đơn cho nhân viên |
| | ✅ canApproveOrder | Duyệt/xác nhận đơn |
| | ✅ canCancelOrder | Hủy đơn có vấn đề |
| **Quản lý nhân viên** | ✅ canManageStaff | Nhân viên cửa hàng mình |
| | ✅ canCreateStaff | Thêm nhân viên |
| | ✅ canDeleteStaff | Xóa nhân viên |
| | ✅ canAssignStaffToOrder | Phân công nhân viên |
| | ✅ canViewStaffPerformance | Xem hiệu suất nhân viên |
| **Khác** | ✅ canManageCoupons | Quản lý mã giảm giá |
| | ✅ canManageInventory | Quản lý kho |
| | ✅ canManageCategories | Danh mục con cửa hàng |
| | ✅ canManageReviews | Duyệt đánh giá |

---

### 2.3. STAFF - Nhân viên xử lý đơn

**Quyền truy cập:** Dashboard, Đơn hàng (được phân công), Sản phẩm (chỉ xem), Khách hàng (xem thông tin)

| Nhóm chức năng | Quyền | Mô tả |
|----------------|-------|-------|
| **Quản lý tài khoản** | ❌ Tất cả | Không có quyền |
| **Quản lý phân quyền** | ❌ Tất cả | Không có quyền |
| **Cấu hình hệ thống** | ❌ Tất cả | Không có quyền |
| **Báo cáo** | ❌ Tất cả | Không có quyền |
| **Quản lý cửa hàng** | ❌ Tất cả | Không có quyền |
| **Quản lý sản phẩm** | ❌ canManageProducts | Không được quản lý |
| | ✅ canViewProducts | Chỉ xem sản phẩm |
| **Quản lý đơn hàng** | ✅ canManageAssignedOrders | Xử lý đơn được giao |
| | ✅ canConfirmOrder | Xác nhận đơn |
| | ✅ canPrepareOrder | Chuẩn bị hàng |
| | ✅ canUpdateOrderStatus | Cập nhật trạng thái |
| | ✅ canAddOrderNote | Ghi chú sự cố |
| | ❌ canCancelOrder | Không được hủy đơn |
| | ❌ canAssignOrderToStaff | Không được phân công |
| **Quản lý khách hàng** | ❌ canManageCustomers | Không được quản lý |
| | ✅ canViewCustomerDetails | Xem thông tin để liên hệ |
| **Quản lý nhân viên** | ❌ Tất cả | Không có quyền |
| **Khác** | ❌ canManageCoupons | Không có quyền |
| | ❌ canManageInventory | Không có quyền |
| | ✅ canUpdateInventory | Trừ tồn kho khi chuẩn bị |
| | ✅ canPrintPackingSlip | In phiếu gói hàng |

---

### 2.4. CUSTOMER - Khách hàng

**Quyền truy cập:** Không vào được Admin Area

| Chức năng | Quyền |
|-----------|-------|
| Xem sản phẩm, tìm kiếm, lọc | ✅ |
| Xem chi tiết sản phẩm | ✅ |
| Thêm vào giỏ hàng | ✅ |
| Đăng ký / Đăng nhập | ✅ |
| Quản lý giỏ hàng | ✅ |
| Đặt hàng | ✅ |
| Xem đơn hàng của mình | ✅ |
| Yêu cầu hủy đơn | ✅ (nếu đơn cho phép) |
| Đánh giá sản phẩm | ✅ (sau khi đơn hoàn thành) |
| Quản lý thông tin cá nhân | ✅ |

---

## 3. LUỒNG XỬ LÝ ĐƠN HÀNG

```
[Khách đặt hàng]
       ↓
[Chờ xác nhận] ← Admin/StoreManager phân công cho Staff
       ↓
[Đang chuẩn bị] ← Staff xác nhận, chuẩn bị hàng
       ↓
[Đang giao] ← Staff cập nhật khi giao cho shipper
       ↓
[Hoàn thành] ← Staff cập nhật khi giao thành công
       
[Đã hủy] ← Chỉ Admin/StoreManager được hủy
```

---

## 4. CÁC TRANG ADMIN VÀ QUYỀN TRUY CẬP

| Trang | Admin | StoreManager | Staff |
|-------|-------|--------------|-------|
| /Admin (Dashboard) | ✅ | ✅ | ✅ |
| /Admin/DonHang | ✅ | ✅ (cửa hàng) | ✅ (được giao) |
| /Admin/SanPham | ✅ | ✅ | ✅ (chỉ xem) |
| /Admin/DanhMuc | ✅ | ✅ | ❌ |
| /Admin/KhachHang | ✅ | ✅ | ✅ (chỉ xem) |
| /Admin/MaGiamGia | ✅ | ✅ | ❌ |
| /Admin/DanhGia | ✅ | ✅ | ❌ |
| /Admin/ChiNhanh | ✅ | ✅ | ❌ |
| /Admin/Kho | ✅ | ✅ | ❌ |
| /Admin/NhanVien | ✅ | ✅ | ❌ |
| /Admin/CuaHang | ✅ | ❌ | ❌ |
| /Admin/TaiKhoan | ✅ | ❌ | ❌ |
| /Admin/BaoCao | ✅ | ✅ (cửa hàng) | ❌ |
| /Admin/ThongKe | ✅ | ✅ (cửa hàng) | ❌ |

---

## 5. CÁCH SỬ DỤNG TRONG CODE

### 5.1. Kiểm tra quyền trong JavaScript

```javascript
// Kiểm tra quyền cụ thể
if (checkPermission('canCancelOrder')) {
    // Hiển thị nút hủy đơn
}

// Kiểm tra vai trò
if (isAdmin()) {
    // Logic cho admin
}

if (isStoreManager()) {
    // Logic cho quản lý cửa hàng
}

if (isStaff()) {
    // Logic cho nhân viên
}

// Lấy ID cửa hàng của user
const storeId = getUserStoreId();

// Ẩn element nếu không có quyền
hideIfNoPermission('.btn-delete', 'canDeleteProduct');

// Disable element nếu không có quyền
disableIfNoPermission('.btn-cancel', 'canCancelOrder');
```

### 5.2. Ẩn menu theo quyền

```html
<!-- Chỉ Admin thấy -->
<div class="nav-section admin-only">
    ...
</div>

<!-- CSS class để ẩn -->
<a href="/Admin/BaoCao" class="nav-item permission-hidden">
    ...
</a>
```

---

## 6. DATABASE - BẢNG LIÊN QUAN

### TaiKhoan (Accounts)
- MaTK (PK)
- TenDangNhap
- MatKhau (hashed)
- Email
- VaiTro (admin/storemanager/staff/customer)
- TrangThai (active/locked)
- MaNV (FK -> NhanVien)
- MaKH (FK -> KhachHang)
- MaCuaHang (FK -> CuaHang) - Cửa hàng được gán

### NhanVien (Staff)
- MaNV (PK)
- HoTen
- SoDienThoai
- MaCuaHang (FK -> CuaHang)

### DonHang (Orders)
- MaDH (PK)
- ...
- MaNVXuLy (FK -> NhanVien) - Nhân viên được phân công
- MaCuaHang (FK -> CuaHang)

---

*Cập nhật: Tháng 1/2026*
