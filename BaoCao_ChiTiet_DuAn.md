# BÁO CÁO CHI TIẾT DỰ ÁN HỆ THỐNG QUẢN LÝ BÁN HÀNG TRỰC TUYẾN MỲ CAY SASIN

## CHƯƠNG 4: KẾT QUẢ NGHIÊN CỨU VÀ HIỆN THỰC HÓA

### 4.1. Kiến trúc Hệ thống Thực tế

#### 4.1.1. Sơ đồ Kiến trúc Tổng thể (Deployment Architecture)

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                           CLIENT LAYER                                       │
├─────────────────────────────────────────────────────────────────────────────┤
│  ┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐          │
│  │   Web Browser   │    │   Mobile App    │    │  Admin Panel    │          │
│  │  (Khách hàng)   │    │   (Tương lai)   │    │  (ASP.NET MVC)  │          │
│  │  HTML/CSS/JS    │    │                 │    │  Razor Pages    │          │
│  └────────┬────────┘    └────────┬────────┘    └────────┬────────┘          │
│           │                      │                      │                    │
│           └──────────────────────┼──────────────────────┘                    │
│                                  │                                           │
│                          HTTP/HTTPS Requests                                 │
└──────────────────────────────────┼───────────────────────────────────────────┘
                                   │
                                   ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│                        APPLICATION LAYER                                     │
│                     (ASP.NET Core 9.0 Web API)                              │
├─────────────────────────────────────────────────────────────────────────────┤
│  ┌─────────────────────────────────────────────────────────────────────┐    │
│  │                      MIDDLEWARE PIPELINE                             │    │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────┐ ┌──────────┐ ┌──────────┐  │    │
│  │  │  CORS    │→│  Auth    │→│ Session  │→│ Logging  │→│ Routing  │  │    │
│  │  └──────────┘ └──────────┘ └──────────┘ └──────────┘ └──────────┘  │    │
│  └─────────────────────────────────────────────────────────────────────┘    │
│                                                                              │
│  ┌─────────────────────────────────────────────────────────────────────┐    │
│  │                        API CONTROLLERS                               │    │
│  │  ┌────────────┐ ┌────────────┐ ┌────────────┐ ┌────────────┐       │    │
│  │  │ AuthCtrl   │ │ ProductCtrl│ │ OrderCtrl  │ │ CartCtrl   │       │    │
│  │  │ (Xác thực) │ │ (Sản phẩm) │ │ (Đơn hàng) │ │ (Giỏ hàng) │       │    │
│  │  └────────────┘ └────────────┘ └────────────┘ └────────────┘       │    │
│  │  ┌────────────┐ ┌────────────┐ ┌────────────┐ ┌────────────┐       │    │
│  │  │ BranchCtrl │ │ CouponCtrl │ │ ReportCtrl │ │ ReviewCtrl │       │    │
│  │  │ (Chi nhánh)│ │ (Khuyến mãi│ │ (Báo cáo)  │ │ (Đánh giá) │       │    │
│  │  └────────────┘ └────────────┘ └────────────┘ └────────────┘       │    │
│  └─────────────────────────────────────────────────────────────────────┘    │
│                                                                              │
│  ┌─────────────────────────────────────────────────────────────────────┐    │
│  │                         SERVICES                                     │    │
│  │  ┌────────────────┐    ┌────────────────┐    ┌────────────────┐     │    │
│  │  │   JwtService   │    │  HttpClient    │    │  ILogger<T>    │     │    │
│  │  │ (Token Auth)   │    │ (External API) │    │  (Logging)     │     │    │
│  │  └────────────────┘    └────────────────┘    └────────────────┘     │    │
│  └─────────────────────────────────────────────────────────────────────┘    │
└──────────────────────────────────┬───────────────────────────────────────────┘
                                   │
                                   ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│                          DATA ACCESS LAYER                                   │
│                    (Entity Framework Core + Pomelo MySQL)                    │
├─────────────────────────────────────────────────────────────────────────────┤
│  ┌─────────────────────────────────────────────────────────────────────┐    │
│  │                       MyCayDbContext                                 │    │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────┐ ┌──────────┐ ┌──────────┐  │    │
│  │  │ DbSet<>  │ │ DbSet<>  │ │ DbSet<>  │ │ DbSet<>  │ │ DbSet<>  │  │    │
│  │  │ SanPham  │ │ DonHang  │ │ KhachHang│ │ TaiKhoan │ │ ChiNhanh │  │    │
│  │  └──────────┘ └──────────┘ └──────────┘ └──────────┘ └──────────┘  │    │
│  └─────────────────────────────────────────────────────────────────────┘    │
└──────────────────────────────────┬───────────────────────────────────────────┘
                                   │
                                   ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│                           DATA LAYER                                         │
│                        MySQL Server (MariaDB 10.4)                           │
├─────────────────────────────────────────────────────────────────────────────┤
│  Database: MyCayDB                                                           │
│  ┌──────────┐ ┌──────────┐ ┌──────────┐ ┌──────────┐ ┌──────────┐          │
│  │ VaiTro   │ │ TaiKhoan │ │ KhachHang│ │ NhanVien │ │ ChiNhanh │          │
│  └──────────┘ └──────────┘ └──────────┘ └──────────┘ └──────────┘          │
│  ┌──────────┐ ┌──────────┐ ┌──────────┐ ┌──────────┐ ┌──────────┐          │
│  │ DanhMuc  │ │ SanPham  │ │ DonHang  │ │ChiTietDH │ │ GioHang  │          │
│  └──────────┘ └──────────┘ └──────────┘ └──────────┘ └──────────┘          │
│  ┌──────────┐ ┌──────────┐ ┌──────────┐ ┌──────────┐                       │
│  │MaGiamGia │ │NguyenVL  │ │ TonKho   │ │ DanhGia  │                       │
│  └──────────┘ └──────────┘ └──────────┘ └──────────┘                       │
└─────────────────────────────────────────────────────────────────────────────┘
```


### 4.2. Sơ đồ Phân cấp Chức năng (Functional Decomposition Diagram)

```
                    ┌─────────────────────────────────────┐
                    │   HỆ THỐNG QUẢN LÝ BÁN HÀNG        │
                    │       MỲ CAY SASIN                  │
                    └─────────────────┬───────────────────┘
                                      │
        ┌─────────────────────────────┼─────────────────────────────┐
        │                             │                             │
        ▼                             ▼                             ▼
┌───────────────────┐    ┌───────────────────┐    ┌───────────────────┐
│  PHÂN HỆ KHÁCH    │    │  PHÂN HỆ QUẢN TRỊ │    │  PHÂN HỆ NHÂN     │
│      HÀNG         │    │      VIÊN         │    │      VIÊN         │
└────────┬──────────┘    └────────┬──────────┘    └────────┬──────────┘
         │                        │                        │
    ┌────┴────┐              ┌────┴────┐              ┌────┴────┐
    │         │              │         │              │         │
    ▼         ▼              ▼         ▼              ▼         ▼
┌───────┐ ┌───────┐    ┌───────┐ ┌───────┐    ┌───────┐ ┌───────┐
│Đăng ký│ │Đặt    │    │Quản lý│ │Quản lý│    │Xử lý  │ │Quản lý│
│Đăng   │ │hàng   │    │Sản    │ │Đơn    │    │Đơn    │ │Kho    │
│nhập   │ │trực   │    │phẩm   │ │hàng   │    │hàng   │ │hàng   │
└───┬───┘ │tuyến  │    └───┬───┘ └───┬───┘    └───┬───┘ └───┬───┘
    │     └───┬───┘        │         │            │         │
    │         │            │         │            │         │
    ▼         ▼            ▼         ▼            ▼         ▼
┌───────────────────────────────────────────────────────────────────┐
│                    CHI TIẾT CHỨC NĂNG                             │
├───────────────────────────────────────────────────────────────────┤
│                                                                   │
│  KHÁCH HÀNG:                                                      │
│  ├── Đăng ký tài khoản (POST /api/auth/register)                 │
│  ├── Đăng nhập (POST /api/auth/login)                            │
│  ├── Xem thực đơn (GET /api/products)                            │
│  ├── Tìm kiếm sản phẩm (GET /api/products?search=)               │
│  ├── Thêm vào giỏ hàng (POST /api/cart)                          │
│  ├── Cập nhật giỏ hàng (PUT /api/cart/{id})                      │
│  ├── Áp dụng mã giảm giá (POST /api/cart/apply-promo)            │
│  ├── Đặt hàng (POST /api/orders)                                 │
│  ├── Tra cứu đơn hàng (GET /api/orders/search?code=)             │
│  ├── Xem đơn hàng của tôi (GET /api/orders/my)                   │
│  └── Đánh giá sản phẩm (POST /api/reviews)                       │
│                                                                   │
│  QUẢN TRỊ VIÊN:                                                   │
│  ├── Quản lý Sản phẩm (CRUD /api/products)                       │
│  ├── Quản lý Danh mục (GET /api/products/categories)             │
│  ├── Quản lý Đơn hàng (GET/PUT /api/orders)                      │
│  ├── Cập nhật trạng thái đơn (PUT /api/orders/{id}/status)       │
│  ├── Quản lý Khách hàng (CRUD /api/customers)                    │
│  ├── Quản lý Tài khoản (CRUD /api/auth/accounts)                 │
│  ├── Quản lý Chi nhánh (CRUD /api/branch)                        │
│  ├── Quản lý Mã giảm giá (CRUD /api/coupon)                      │
│  ├── Quản lý Kho (CRUD /api/inventory)                           │
│  ├── Xem Báo cáo Thống kê (GET /api/reports/*)                   │
│  └── Quản lý Đánh giá (GET/PUT /api/reviews)                     │
│                                                                   │
│  NHÂN VIÊN CHI NHÁNH:                                             │
│  ├── Xem đơn hàng được phân bổ                                   │
│  ├── Cập nhật trạng thái đơn hàng                                │
│  ├── Quản lý tồn kho chi nhánh                                   │
│  └── Báo cáo tồn kho                                             │
│                                                                   │
└───────────────────────────────────────────────────────────────────┘
```


### 4.3. Sơ đồ Luồng Người dùng (User Flow Diagrams)

#### 4.3.1. Luồng Đặt hàng Trực tuyến (Customer Order Flow)

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    LUỒNG ĐẶT HÀNG TRỰC TUYẾN                                │
└─────────────────────────────────────────────────────────────────────────────┘

    ┌─────────┐
    │  START  │
    └────┬────┘
         │
         ▼
    ┌─────────────┐     ┌─────────────┐
    │ Truy cập    │────▶│ Xem Thực đơn│
    │ Trang chủ   │     │ /ThucDon    │
    └─────────────┘     └──────┬──────┘
                               │
                               ▼
                        ┌─────────────┐
                        │ Chọn sản    │
                        │ phẩm        │
                        └──────┬──────┘
                               │
                               ▼
                    ┌──────────────────┐
                    │ Mở Modal chi tiết│
                    │ - Chọn cấp độ cay│
                    │ - Chọn số lượng  │
                    │ - Ghi chú        │
                    └────────┬─────────┘
                             │
                             ▼
                    ┌──────────────────┐
                    │ POST /api/cart   │◀──────────────────┐
                    │ Thêm vào giỏ hàng│                   │
                    └────────┬─────────┘                   │
                             │                             │
                             ▼                             │
                    ┌──────────────────┐                   │
                    │ Tiếp tục mua?    │───── Có ─────────┘
                    └────────┬─────────┘
                             │ Không
                             ▼
                    ┌──────────────────┐
                    │ Xem Giỏ hàng     │
                    │ GET /api/cart    │
                    └────────┬─────────┘
                             │
                             ▼
                    ┌──────────────────┐
                    │ Có mã giảm giá?  │
                    └────────┬─────────┘
                             │ Có
                             ▼
                    ┌──────────────────┐
                    │POST /api/cart/   │
                    │apply-promo       │
                    └────────┬─────────┘
                             │
                             ▼
                    ┌──────────────────┐
                    │ Nhập thông tin   │
                    │ giao hàng        │
                    │ - Họ tên         │
                    │ - SĐT            │
                    │ - Địa chỉ        │
                    └────────┬─────────┘
                             │
                             ▼
                    ┌──────────────────┐
                    │ POST /api/orders │
                    │ Tạo đơn hàng     │
                    └────────┬─────────┘
                             │
                             ▼
                    ┌──────────────────┐
                    │ Đặt hàng thành   │
                    │ công! Mã đơn:    │
                    │ DH20241225...    │
                    └────────┬─────────┘
                             │
                             ▼
                        ┌─────────┐
                        │   END   │
                        └─────────┘
```


#### 4.3.2. Luồng Xử lý Đơn hàng (Order Processing Flow)

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    LUỒNG XỬ LÝ ĐƠN HÀNG (ADMIN/STAFF)                       │
└─────────────────────────────────────────────────────────────────────────────┘

    ┌─────────────┐
    │ Đơn hàng    │
    │ mới tạo     │
    │ (Chờ xác    │
    │  nhận)      │
    └──────┬──────┘
           │
           ▼
    ┌─────────────────┐
    │ Admin/Staff     │
    │ xem danh sách   │
    │ GET /api/orders │
    └────────┬────────┘
             │
             ▼
    ┌─────────────────┐
    │ Xem chi tiết    │
    │ GET /api/orders │
    │ /{id}           │
    └────────┬────────┘
             │
             ▼
    ┌─────────────────┐     ┌─────────────────┐
    │ Xác nhận đơn?   │─No─▶│ Hủy đơn hàng    │
    └────────┬────────┘     │ DELETE /api/    │
             │ Yes          │ orders/{id}     │
             ▼              └────────┬────────┘
    ┌─────────────────┐              │
    │ PUT /api/orders │              ▼
    │ /{id}/status    │         ┌─────────┐
    │ status="Đang    │         │ Đã hủy  │
    │ chuẩn bị"       │         └─────────┘
    └────────┬────────┘
             │
             ▼
    ┌─────────────────┐
    │ Chuẩn bị món    │
    │ ăn tại chi      │
    │ nhánh           │
    └────────┬────────┘
             │
             ▼
    ┌─────────────────┐
    │ PUT /api/orders │
    │ /{id}/status    │
    │ status="Đang    │
    │ giao"           │
    └────────┬────────┘
             │
             ▼
    ┌─────────────────┐
    │ Giao hàng cho   │
    │ khách           │
    └────────┬────────┘
             │
             ▼
    ┌─────────────────┐
    │ PUT /api/orders │
    │ /{id}/status    │
    │ status="Hoàn    │
    │ thành"          │
    └────────┬────────┘
             │
             ▼
    ┌─────────────────┐
    │ Tự động cập nhật│
    │ TrangThaiThanh  │
    │ Toan = "Đã      │
    │ thanh toán"     │
    └────────┬────────┘
             │
             ▼
        ┌─────────┐
        │   END   │
        └─────────┘
```


#### 4.3.3. Luồng Xác thực và Phân quyền (Authentication Flow)

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    LUỒNG XÁC THỰC JWT (JSON Web Token)                      │
└─────────────────────────────────────────────────────────────────────────────┘

    ┌─────────────┐
    │   Client    │
    │ (Browser)   │
    └──────┬──────┘
           │
           │ POST /api/auth/login
           │ { email, password }
           ▼
    ┌─────────────────────────────────────────────────────────────────┐
    │                    AuthController.Login()                        │
    ├─────────────────────────────────────────────────────────────────┤
    │  1. Validate input (email, password required)                   │
    │  2. Hash password với MD5: GetMD5Hash(password)                 │
    │  3. Query database:                                             │
    │     _context.TaiKhoans                                          │
    │       .Include(t => t.VaiTro)                                   │
    │       .FirstOrDefaultAsync(t =>                                 │
    │         (t.TenDangNhap == email || t.Email == email)            │
    │         && t.MatKhau == hashedPassword                          │
    │         && t.TrangThai == true)                                 │
    │  4. Nếu tìm thấy:                                               │
    │     - Cập nhật LanDangNhapCuoi                                  │
    │     - Lấy thông tin KhachHang/NhanVien                          │
    │     - Chuẩn hóa role: NormalizeRole()                           │
    │     - Tạo JWT token: _jwtService.GenerateToken()                │
    └─────────────────────────────────────────────────────────────────┘
           │
           │ Response: { success: true, data: { token, user } }
           ▼
    ┌─────────────────────────────────────────────────────────────────┐
    │                    Client Storage                                │
    ├─────────────────────────────────────────────────────────────────┤
    │  localStorage.setItem('token', token)                           │
    │  localStorage.setItem('user', JSON.stringify(user))             │
    └─────────────────────────────────────────────────────────────────┘
           │
           │ Subsequent API calls with Authorization header
           │ Authorization: Bearer <token>
           ▼
    ┌─────────────────────────────────────────────────────────────────┐
    │                    JWT Middleware                                │
    ├─────────────────────────────────────────────────────────────────┤
    │  TokenValidationParameters:                                     │
    │  - ValidateIssuerSigningKey: true                               │
    │  - IssuerSigningKey: SymmetricSecurityKey(jwtKey)               │
    │  - ValidateIssuer: true (MyCaySasin)                            │
    │  - ValidateAudience: true (MyCaySasinApp)                       │
    │  - ValidateLifetime: true                                       │
    │  - ClockSkew: TimeSpan.Zero                                     │
    └─────────────────────────────────────────────────────────────────┘
```


### 4.4. Sơ đồ Luồng Dữ liệu DFD (Data Flow Diagram)

#### 4.4.1. DFD Mức 0 - Sơ đồ Ngữ cảnh (Context Diagram)

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    DFD MỨC 0 - SƠ ĐỒ NGỮ CẢNH                               │
│                    (Context Diagram)                                         │
└─────────────────────────────────────────────────────────────────────────────┘

                    ┌─────────────────────┐
                    │                     │
     Thông tin      │                     │      Thông tin
     đăng ký/       │                     │      sản phẩm
     đăng nhập      │                     │      
    ─────────────▶  │                     │  ◀─────────────
                    │                     │
     Đơn hàng,      │                     │      Cập nhật
     thanh toán     │                     │      sản phẩm
    ─────────────▶  │                     │  ─────────────▶
                    │                     │
┌──────────┐        │    HỆ THỐNG         │        ┌──────────┐
│          │        │    QUẢN LÝ          │        │          │
│  KHÁCH   │        │    BÁN HÀNG         │        │  QUẢN    │
│  HÀNG    │        │    MỲ CAY           │        │  TRỊ     │
│          │        │    SASIN            │        │  VIÊN    │
└──────────┘        │                     │        └──────────┘
                    │        0            │
     Xác nhận       │                     │      Báo cáo
     đơn hàng       │                     │      thống kê
    ◀─────────────  │                     │  ◀─────────────
                    │                     │
     Thông tin      │                     │      Quản lý
     tra cứu        │                     │      đơn hàng
    ◀─────────────  │                     │  ─────────────▶
                    │                     │
                    └─────────────────────┘
                              │
                              │
                              ▼
                    ┌─────────────────────┐
                    │                     │
                    │    CƠ SỞ DỮ LIỆU    │
                    │      MyCayDB        │
                    │                     │
                    └─────────────────────┘
```


#### 4.4.2. DFD Mức 1 - Sơ đồ Phân rã Chức năng

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    DFD MỨC 1 - PHÂN RÃ CHỨC NĂNG                            │
└─────────────────────────────────────────────────────────────────────────────┘

┌──────────┐                                                    ┌──────────┐
│  KHÁCH   │                                                    │  QUẢN    │
│  HÀNG    │                                                    │  TRỊ     │
└────┬─────┘                                                    └────┬─────┘
     │                                                               │
     │ Thông tin đăng ký                                            │
     │ Email, Password                                              │
     ▼                                                               │
┌─────────────────┐                                                  │
│                 │                                                  │
│  1.0 QUẢN LÝ   │◀──────────────────────────────────────────────────┤
│  TÀI KHOẢN     │     Quản lý tài khoản                            │
│                 │                                                  │
└────────┬────────┘                                                  │
         │                                                           │
         │ Thông tin xác thực                                       │
         ▼                                                           │
    ═══════════════                                                  │
    ║  D1: TaiKhoan ║                                                │
    ═══════════════                                                  │
         │                                                           │
         │ Token JWT                                                 │
         ▼                                                           │
┌──────────┐                                                         │
│  KHÁCH   │                                                         │
│  HÀNG    │                                                         │
└────┬─────┘                                                         │
     │                                                               │
     │ Xem thực đơn                                                  │
     │ Tìm kiếm                                                      │
     ▼                                                               │
┌─────────────────┐                                                  │
│                 │◀─────────────────────────────────────────────────┤
│  2.0 QUẢN LÝ   │     CRUD Sản phẩm                                │
│  SẢN PHẨM      │                                                  │
│                 │                                                  │
└────────┬────────┘                                                  │
         │                                                           │
         │ Thông tin sản phẩm                                       │
         ▼                                                           │
    ═══════════════                                                  │
    ║  D2: SanPham  ║                                                │
    ═══════════════                                                  │
         │                                                           │
         │ Danh sách sản phẩm                                       │
         ▼                                                           │
┌──────────┐                                                         │
│  KHÁCH   │                                                         │
│  HÀNG    │                                                         │
└────┬─────┘                                                         │
     │                                                               │
     │ Thêm vào giỏ                                                  │
     │ Cập nhật số lượng                                            │
     ▼                                                               │
┌─────────────────┐                                                  │
│                 │                                                  │
│  3.0 QUẢN LÝ   │                                                  │
│  GIỎ HÀNG      │                                                  │
│                 │                                                  │
└────────┬────────┘                                                  │
         │                                                           │
         │ Thông tin giỏ hàng                                       │
         ▼                                                           │
    ═══════════════                                                  │
    ║  D3: GioHang  ║                                                │
    ═══════════════                                                  │
         │                                                           │
         │ Chi tiết giỏ hàng                                        │
         ▼                                                           │
┌──────────┐                                                         │
│  KHÁCH   │                                                         │
│  HÀNG    │                                                         │
└────┬─────┘                                                         │
     │                                                               │
     │ Đặt hàng                                                      │
     │ Thông tin giao hàng                                          │
     ▼                                                               │
┌─────────────────┐                                                  │
│                 │◀─────────────────────────────────────────────────┤
│  4.0 XỬ LÝ     │     Xử lý đơn hàng                               │
│  ĐƠN HÀNG      │     Cập nhật trạng thái                          │
│                 │                                                  │
└────────┬────────┘                                                  │
         │                                                           │
         │ Thông tin đơn hàng                                       │
         ▼                                                           │
    ═══════════════                                                  │
    ║  D4: DonHang  ║                                                │
    ═══════════════                                                  │
         │                                                           │
         │ Xác nhận đơn hàng                                        │
         ▼                                                           │
┌──────────┐                                                         │
│  KHÁCH   │                                                         │
│  HÀNG    │                                                         │
└──────────┘                                                         │
                                                                     │
                                                                     │
┌─────────────────┐                                                  │
│                 │◀─────────────────────────────────────────────────┘
│  5.0 BÁO CÁO   │     Yêu cầu báo cáo
│  THỐNG KÊ      │
│                 │─────────────────────────────────────────────────▶
└─────────────────┘     Báo cáo doanh thu, thống kê              ┌──────────┐
         │                                                        │  QUẢN    │
         │                                                        │  TRỊ     │
         ▼                                                        └──────────┘
    ═══════════════
    ║ D4: DonHang  ║
    ═══════════════
```


#### 4.4.3. DFD Mức 2 - Chi tiết Xử lý Đơn hàng (Process 4.0)

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    DFD MỨC 2 - XỬ LÝ ĐƠN HÀNG (4.0)                         │
└─────────────────────────────────────────────────────────────────────────────┘

┌──────────┐
│  KHÁCH   │
│  HÀNG    │
└────┬─────┘
     │
     │ Thông tin đặt hàng
     │ (Tên, SĐT, Địa chỉ, Items)
     ▼
┌─────────────────┐
│                 │
│  4.1 KIỂM TRA  │
│  THÔNG TIN     │
│                 │
└────────┬────────┘
         │
         │ Thông tin hợp lệ
         ▼
┌─────────────────┐         ═══════════════
│                 │────────▶║ D2: SanPham  ║
│  4.2 TÍNH TOÁN │         ═══════════════
│  GIÁ TRỊ       │◀────────  Giá sản phẩm
│  ĐƠN HÀNG      │
│                 │         ═══════════════
│                 │────────▶║D5: MaGiamGia ║
│                 │◀────────═══════════════
└────────┬────────┘          Thông tin giảm giá
         │
         │ Tổng tiền, Giảm giá
         ▼
┌─────────────────┐
│                 │
│  4.3 TẠO MÃ    │
│  ĐƠN HÀNG      │
│                 │
└────────┬────────┘
         │
         │ Mã đơn: DH20241225xxxx
         ▼
┌─────────────────┐         ═══════════════
│                 │────────▶║ D4: DonHang  ║
│  4.4 LƯU       │         ═══════════════
│  ĐƠN HÀNG      │
│                 │         ═══════════════════
│                 │────────▶║D6: ChiTietDonHang║
└────────┬────────┘         ═══════════════════
         │
         │ Xác nhận đơn hàng
         ▼
┌──────────┐
│  KHÁCH   │
│  HÀNG    │
└──────────┘


                    ┌──────────┐
                    │  QUẢN    │
                    │  TRỊ     │
                    └────┬─────┘
                         │
                         │ Cập nhật trạng thái
                         ▼
                    ┌─────────────────┐
                    │                 │
                    │  4.5 CẬP NHẬT  │
                    │  TRẠNG THÁI    │
                    │                 │
                    └────────┬────────┘
                             │
                             │ Trạng thái mới
                             ▼
                        ═══════════════
                        ║ D4: DonHang  ║
                        ═══════════════
                             │
                             │ Thông báo
                             ▼
                    ┌──────────┐
                    │  KHÁCH   │
                    │  HÀNG    │
                    └──────────┘
```


#### 4.4.4. DFD Mức 2 - Chi tiết Quản lý Tài khoản (Process 1.0)

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    DFD MỨC 2 - QUẢN LÝ TÀI KHOẢN (1.0)                      │
└─────────────────────────────────────────────────────────────────────────────┘

┌──────────┐
│  KHÁCH   │
│  HÀNG    │
└────┬─────┘
     │
     │ Email, Password, Họ tên, SĐT
     ▼
┌─────────────────┐
│                 │
│  1.1 ĐĂNG KÝ   │
│  TÀI KHOẢN     │
│                 │
└────────┬────────┘
         │
         │ Kiểm tra email tồn tại
         ▼
    ═══════════════
    ║ D1: TaiKhoan ║
    ═══════════════
         │
         │ Email chưa tồn tại
         ▼
┌─────────────────┐
│                 │
│  1.2 MÃ HÓA    │
│  MẬT KHẨU      │
│  (MD5 Hash)    │
│                 │
└────────┬────────┘
         │
         │ Password đã hash
         ▼
┌─────────────────┐         ═══════════════
│                 │────────▶║ D1: TaiKhoan ║
│  1.3 TẠO       │         ═══════════════
│  TÀI KHOẢN     │
│                 │         ═══════════════
│                 │────────▶║ D7: KhachHang║
└────────┬────────┘         ═══════════════
         │
         │ Đăng ký thành công
         ▼
┌──────────┐
│  KHÁCH   │
│  HÀNG    │
└──────────┘


┌──────────┐
│  KHÁCH   │
│  HÀNG    │
└────┬─────┘
     │
     │ Email/Username, Password
     ▼
┌─────────────────┐
│                 │
│  1.4 XÁC THỰC  │
│  ĐĂNG NHẬP     │
│                 │
└────────┬────────┘
         │
         │ Kiểm tra thông tin
         ▼
    ═══════════════
    ║ D1: TaiKhoan ║
    ═══════════════
         │
         │ Thông tin tài khoản
         ▼
┌─────────────────┐
│                 │
│  1.5 TẠO       │
│  JWT TOKEN     │
│                 │
└────────┬────────┘
         │
         │ Token + User Info
         ▼
┌──────────┐
│  KHÁCH   │
│  HÀNG    │
└──────────┘
```


#### 4.4.5. Từ điển Dữ liệu (Data Dictionary)

| Ký hiệu | Tên | Mô tả | Thuộc tính chính |
|---------|-----|-------|------------------|
| D1 | TaiKhoan | Lưu thông tin đăng nhập | MaTK, TenDangNhap, MatKhau, Email, MaVaiTro, TrangThai |
| D2 | SanPham | Lưu thông tin sản phẩm | MaSP, TenSP, MoTa, DonGia, HinhAnh, MaDM, TrangThai |
| D3 | GioHang | Lưu giỏ hàng tạm thời | MaGH, MaKH, SessionID, MaSP, SoLuong, CapDoCay |
| D4 | DonHang | Lưu thông tin đơn hàng | MaDH, MaDHCode, MaKH, TongTien, TrangThai, NgayDat |
| D5 | MaGiamGia | Lưu mã khuyến mãi | MaMGG, MaCode, LoaiGiam, GiaTri, NgayBatDau, NgayKetThuc |
| D6 | ChiTietDonHang | Chi tiết từng món trong đơn | MaCTDH, MaDH, MaSP, SoLuong, DonGia, CapDoCay |
| D7 | KhachHang | Thông tin khách hàng | MaKH, MaTK, HoTen, Email, SDT, DiaChi, DiemTichLuy |
| D8 | DanhMuc | Danh mục sản phẩm | MaDM, TenDanhMuc, MoTa, HinhAnh, ThuTu |
| D9 | ChiNhanh | Thông tin chi nhánh | MaCN, TenChiNhanh, DiaChi, SDT, TrangThai |
| D10 | VaiTro | Phân quyền người dùng | MaVaiTro, TenVaiTro, MoTa |


### 4.5. Chi tiết Logic Xử lý và Công nghệ Sử dụng

#### 4.4.1. Cấu hình Dependency Injection (Program.cs)

```csharp
// ═══════════════════════════════════════════════════════════════════════════
// CẤU HÌNH DATABASE - Entity Framework Core với MySQL (Pomelo Provider)
// ═══════════════════════════════════════════════════════════════════════════
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var serverVersion = new MariaDbServerVersion(new Version(10, 4, 0));

builder.Services.AddDbContext<MyCayDbContext>(options =>
    options.UseMySql(connectionString, serverVersion, mysqlOptions =>
    {
        mysqlOptions.EnableRetryOnFailure(3); // Tự động retry khi mất kết nối
    }));

// ═══════════════════════════════════════════════════════════════════════════
// CẤU HÌNH JWT AUTHENTICATION
// ═══════════════════════════════════════════════════════════════════════════
builder.Services.AddSingleton<IJwtService, JwtService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateIssuer = true,
            ValidIssuer = "MyCaySasin",
            ValidateAudience = true,
            ValidAudience = "MyCaySasinApp",
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero // Token hết hạn chính xác
        };
    });

// ═══════════════════════════════════════════════════════════════════════════
// CẤU HÌNH CORS - Cho phép Frontend gọi API
// ═══════════════════════════════════════════════════════════════════════════
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// ═══════════════════════════════════════════════════════════════════════════
// CẤU HÌNH SESSION - Lưu trữ giỏ hàng tạm thời
// ═══════════════════════════════════════════════════════════════════════════
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
```


#### 4.4.2. Logic Xử lý Đơn hàng (OrdersController.cs)

```csharp
// ═══════════════════════════════════════════════════════════════════════════
// TẠO ĐƠN HÀNG MỚI - POST /api/orders
// ═══════════════════════════════════════════════════════════════════════════
[HttpPost]
public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
{
    // 1. VALIDATION - Kiểm tra dữ liệu đầu vào
    if (string.IsNullOrEmpty(request.CustomerName) || string.IsNullOrEmpty(request.Phone))
        return BadRequest(new { success = false, message = "Vui lòng điền đầy đủ thông tin" });

    try
    {
        // 2. GENERATE ORDER CODE - Tạo mã đơn hàng duy nhất
        // Format: DH + yyyyMMddHHmmss + 4 số ngẫu nhiên
        var random = new Random();
        var randomSuffix = random.Next(1000, 9999);
        var orderCode = $"DH{DateTime.Now:yyyyMMddHHmmss}{randomSuffix}";
        
        // Kiểm tra trùng mã (đảm bảo tính duy nhất)
        while (await _context.DonHangs.AnyAsync(d => d.MaDHCode == orderCode))
        {
            randomSuffix = random.Next(1000, 9999);
            orderCode = $"DH{DateTime.Now:yyyyMMddHHmmss}{randomSuffix}";
        }
        
        // 3. VALIDATE CUSTOMER - Kiểm tra khách hàng tồn tại
        int? validCustomerId = null;
        if (request.CustomerId.HasValue && request.CustomerId.Value > 0)
        {
            var customerExists = await _context.KhachHangs
                .AnyAsync(k => k.MaKH == request.CustomerId.Value);
            if (customerExists)
                validCustomerId = request.CustomerId.Value;
        }
        
        // 4. CREATE ORDER ENTITY - Tạo đối tượng đơn hàng
        var donHang = new DonHang
        {
            MaDHCode = orderCode,
            MaKH = validCustomerId,
            TenKhach = request.CustomerName,
            SDTKhach = request.Phone,
            DiaChiGiao = request.Address,
            TamTinh = request.Subtotal,
            PhiGiaoHang = request.ShippingFee,
            GiamGia = request.Discount,
            TongTien = request.Subtotal + request.ShippingFee - request.Discount,
            PhuongThucThanhToan = request.PaymentMethod ?? "Tiền mặt",
            TrangThaiThanhToan = "Chưa thanh toán",
            TrangThai = "Chờ xác nhận",
            GhiChu = request.Note,
            NgayDat = DateTime.Now
        };

        // 5. SAVE TO DATABASE - Lưu đơn hàng
        _context.DonHangs.Add(donHang);
        await _context.SaveChangesAsync();

        // 6. SAVE ORDER ITEMS - Lưu chi tiết đơn hàng
        if (request.Items != null && request.Items.Count > 0)
        {
            foreach (var item in request.Items)
            {
                var chiTiet = new ChiTietDonHang
                {
                    MaDH = donHang.MaDH,
                    MaSP = item.ProductId,
                    TenSP = item.Name ?? "Sản phẩm",
                    SoLuong = item.Quantity,
                    DonGia = item.Price,
                    CapDoCay = item.SpicyLevel,
                    LoaiNuocDung = item.BrothType,
                    GhiChu = item.Note
                };
                _context.ChiTietDonHangs.Add(chiTiet);
            }
            await _context.SaveChangesAsync();
        }

        // 7. RETURN SUCCESS RESPONSE
        return Ok(new { 
            success = true, 
            message = "Đặt hàng thành công!", 
            data = new { orderId = donHang.MaDH, orderCode } 
        });
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Lỗi tạo đơn hàng");
        return StatusCode(500, new { success = false, message = "Lỗi hệ thống" });
    }
}
```


#### 4.4.3. Logic Xác thực Người dùng (AuthController.cs)

```csharp
// ═══════════════════════════════════════════════════════════════════════════
// ĐĂNG NHẬP - POST /api/auth/login
// ═══════════════════════════════════════════════════════════════════════════
[HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginRequest request)
{
    // 1. VALIDATION
    if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
        return BadRequest(new { success = false, message = "Vui lòng nhập tên đăng nhập và mật khẩu" });

    try
    {
        // 2. HASH PASSWORD - Mã hóa mật khẩu với MD5
        var hashedPassword = GetMD5Hash(request.Password);

        // 3. QUERY DATABASE - Tìm tài khoản
        // Sử dụng Include() để eager loading VaiTro
        var taiKhoan = await _context.TaiKhoans
            .Include(t => t.VaiTro)
            .FirstOrDefaultAsync(t => 
                (t.TenDangNhap == request.Email || t.Email == request.Email) 
                && t.MatKhau == hashedPassword 
                && t.TrangThai == true);

        if (taiKhoan == null)
            return Unauthorized(new { success = false, message = "Tên đăng nhập hoặc mật khẩu không đúng" });

        // 4. UPDATE LAST LOGIN
        taiKhoan.LanDangNhapCuoi = DateTime.Now;
        await _context.SaveChangesAsync();

        // 5. GET USER DETAILS - Lấy thông tin chi tiết theo vai trò
        string hoTen = taiKhoan.TenDangNhap;
        string role = taiKhoan.VaiTro?.TenVaiTro ?? "KhachHang";

        if (role == "KhachHang")
        {
            var khachHang = await _context.KhachHangs
                .FirstOrDefaultAsync(k => k.MaTK == taiKhoan.MaTK);
            if (khachHang != null) hoTen = khachHang.HoTen;
        }
        else if (role == "NhanVien")
        {
            var nhanVien = await _context.NhanViens
                .FirstOrDefaultAsync(n => n.MaTK == taiKhoan.MaTK);
            if (nhanVien != null) hoTen = nhanVien.HoTen;
        }

        // 6. NORMALIZE ROLE - Chuẩn hóa tên vai trò
        var normalizedRole = NormalizeRole(role);
        
        // 7. GENERATE JWT TOKEN
        var token = _jwtService.GenerateToken(
            taiKhoan.MaTK, 
            taiKhoan.Email ?? taiKhoan.TenDangNhap, 
            normalizedRole, 
            hoTen
        );

        // 8. RETURN SUCCESS RESPONSE
        return Ok(new
        {
            success = true,
            message = "Đăng nhập thành công",
            data = new
            {
                id = taiKhoan.MaTK,
                email = taiKhoan.Email ?? taiKhoan.TenDangNhap,
                name = hoTen,
                role = normalizedRole,
                token = token
            }
        });
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Lỗi đăng nhập");
        return StatusCode(500, new { success = false, message = "Lỗi hệ thống" });
    }
}

// ═══════════════════════════════════════════════════════════════════════════
// HÀM CHUẨN HÓA VAI TRÒ - Chuyển đổi từ tiếng Việt sang tiếng Anh
// ═══════════════════════════════════════════════════════════════════════════
private static string NormalizeRole(string? role)
{
    return role?.ToLower() switch
    {
        "quantrivien" => "admin",
        "quanly" => "manager",
        "nhanvien" => "staff",
        "khachhang" => "customer",
        _ => role?.ToLower() ?? "customer"
    };
}

// ═══════════════════════════════════════════════════════════════════════════
// HÀM MÃ HÓA MD5 - Hash password
// ═══════════════════════════════════════════════════════════════════════════
private string GetMD5Hash(string input)
{
    using var md5 = MD5.Create();
    var inputBytes = Encoding.UTF8.GetBytes(input);
    var hashBytes = md5.ComputeHash(inputBytes);
    return Convert.ToHexString(hashBytes).ToLower();
}
```


#### 4.4.4. Logic Giỏ hàng và Mã giảm giá (CartController.cs)

```csharp
// ═══════════════════════════════════════════════════════════════════════════
// THÊM VÀO GIỎ HÀNG - POST /api/cart
// ═══════════════════════════════════════════════════════════════════════════
[HttpPost]
public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
{
    try
    {
        // 1. VALIDATE PRODUCT - Kiểm tra sản phẩm tồn tại
        var sanPham = await _context.SanPhams.FindAsync(request.ProductId);
        if (sanPham == null)
            return NotFound(new { success = false, message = "Sản phẩm không tồn tại" });

        // 2. CHECK EXISTING ITEM - Kiểm tra đã có trong giỏ chưa
        // Điều kiện: cùng sản phẩm, cùng khách hàng/session, cùng cấp độ cay, cùng loại nước dùng
        var existing = await _context.GioHangs.FirstOrDefaultAsync(g =>
            g.MaSP == request.ProductId &&
            ((request.CustomerId.HasValue && g.MaKH == request.CustomerId) ||
             (!string.IsNullOrEmpty(request.SessionId) && g.SessionID == request.SessionId)) &&
            g.CapDoCay == request.SpicyLevel &&
            g.LoaiNuocDung == request.BrothType);

        // 3. UPDATE OR CREATE
        if (existing != null)
        {
            // Nếu đã có, tăng số lượng
            existing.SoLuong += request.Quantity;
        }
        else
        {
            // Nếu chưa có, tạo mới
            var gioHang = new GioHang
            {
                MaKH = request.CustomerId,
                SessionID = request.SessionId,
                MaSP = request.ProductId,
                SoLuong = request.Quantity,
                CapDoCay = request.SpicyLevel,
                LoaiNuocDung = request.BrothType,
                GhiChu = request.Note,
                NgayThem = DateTime.Now
            };
            _context.GioHangs.Add(gioHang);
        }

        await _context.SaveChangesAsync();
        return Ok(new { success = true, message = "Đã thêm vào giỏ hàng" });
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Lỗi thêm giỏ hàng");
        return StatusCode(500, new { success = false, message = "Lỗi hệ thống" });
    }
}

// ═══════════════════════════════════════════════════════════════════════════
// ÁP DỤNG MÃ GIẢM GIÁ - POST /api/cart/apply-promo
// ═══════════════════════════════════════════════════════════════════════════
[HttpPost("apply-promo")]
public async Task<IActionResult> ApplyPromoCode([FromBody] ApplyPromoRequest request)
{
    if (string.IsNullOrEmpty(request.Code))
        return BadRequest(new { success = false, message = "Vui lòng nhập mã giảm giá" });

    var code = request.Code.ToUpper();
    
    // 1. FIND COUPON - Tìm mã giảm giá trong database
    var coupon = await _context.MaGiamGias
        .FirstOrDefaultAsync(m => m.MaCode.ToUpper() == code);

    if (coupon == null)
        return BadRequest(new { success = false, message = "Mã giảm giá không hợp lệ" });

    // 2. VALIDATE STATUS - Kiểm tra trạng thái
    if (!coupon.TrangThai)
        return BadRequest(new { success = false, message = "Mã giảm giá đã bị vô hiệu hóa" });

    // 3. VALIDATE TIME - Kiểm tra thời gian hiệu lực
    var now = DateTime.Now;
    if (coupon.NgayBatDau.HasValue && coupon.NgayBatDau > now)
        return BadRequest(new { success = false, message = "Mã giảm giá chưa có hiệu lực" });

    if (coupon.NgayKetThuc.HasValue && coupon.NgayKetThuc < now)
        return BadRequest(new { success = false, message = "Mã giảm giá đã hết hạn" });

    // 4. VALIDATE USAGE - Kiểm tra số lượng sử dụng
    if (coupon.DaSuDung >= coupon.SoLuong)
        return BadRequest(new { success = false, message = "Mã giảm giá đã hết lượt sử dụng" });

    // 5. VALIDATE MIN ORDER - Kiểm tra đơn tối thiểu
    if (request.Subtotal < coupon.DonToiThieu)
        return BadRequest(new { 
            success = false, 
            message = $"Đơn hàng tối thiểu {coupon.DonToiThieu:N0}đ để áp dụng mã này" 
        });

    // 6. CALCULATE DISCOUNT - Tính toán giảm giá
    decimal discount = 0;
    decimal shippingDiscount = 0;

    switch (coupon.LoaiGiam.ToLower())
    {
        case "percent":
            discount = request.Subtotal * coupon.GiaTri / 100;
            if (coupon.GiamToiDa.HasValue && discount > coupon.GiamToiDa)
                discount = coupon.GiamToiDa.Value;
            break;
        case "fixed":
            discount = coupon.GiaTri;
            break;
        case "freeship":
            shippingDiscount = coupon.GiaTri;
            break;
    }

    // 7. RETURN SUCCESS
    return Ok(new
    {
        success = true,
        message = $"Áp dụng mã {code} thành công!",
        data = new
        {
            couponId = coupon.MaMGG,
            code = coupon.MaCode,
            description = coupon.MoTa,
            discount,
            shippingDiscount,
            type = coupon.LoaiGiam
        }
    });
}
```


### 4.5. Mô hình Dữ liệu Thực tế (Entity Classes)

#### 4.5.1. Sơ đồ ERD Chi tiết

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    ENTITY RELATIONSHIP DIAGRAM (ERD)                         │
└─────────────────────────────────────────────────────────────────────────────┘

┌──────────────┐       ┌──────────────┐       ┌──────────────┐
│   VaiTro     │       │   TaiKhoan   │       │  KhachHang   │
├──────────────┤       ├──────────────┤       ├──────────────┤
│ MaVaiTro(PK) │◄──────│ MaVaiTro(FK) │       │ MaKH (PK)    │
│ TenVaiTro    │   1:N │ MaTK (PK)    │◄──────│ MaTK (FK)    │
│ MoTa         │       │ TenDangNhap  │   1:1 │ HoTen        │
│ NgayTao      │       │ MatKhau      │       │ Email        │
└──────────────┘       │ Email        │       │ SDT          │
                       │ TrangThai    │       │ DiaChi       │
                       │ NgayTao      │       │ DiemTichLuy  │
                       │ LanDangNhap  │       │ NgayDangKy   │
                       │ Cuoi         │       │ TrangThai    │
                       └──────────────┘       └──────┬───────┘
                                                     │
                                                     │ 1:N
                                                     ▼
┌──────────────┐       ┌──────────────┐       ┌──────────────┐
│   DanhMuc    │       │   SanPham    │       │   DonHang    │
├──────────────┤       ├──────────────┤       ├──────────────┤
│ MaDM (PK)    │◄──────│ MaDM (FK)    │       │ MaDH (PK)    │
│ TenDanhMuc   │   1:N │ MaSP (PK)    │       │ MaDHCode     │
│ MoTa         │       │ MaSPCode     │       │ MaKH (FK)    │◄─────┘
│ HinhAnh      │       │ TenSP        │       │ TenKhach     │
│ ThuTu        │       │ MoTa         │       │ SDTKhach     │
│ TrangThai    │       │ DonGia       │       │ DiaChiGiao   │
└──────────────┘       │ GiaKhuyenMai │       │ NgayDat      │
                       │ HinhAnh      │       │ TamTinh      │
                       │ CapDoCay     │       │ PhiGiaoHang  │
                       │ NoiBat       │       │ GiamGia      │
                       │ TrangThai    │       │ TongTien     │
                       │ NgayTao      │       │ TrangThai    │
                       └──────┬───────┘       │ MaCN (FK)    │
                              │               │ MaMGG (FK)   │
                              │ 1:N           └──────┬───────┘
                              ▼                      │
                       ┌──────────────┐              │ 1:N
                       │ChiTietDonHang│◄─────────────┘
                       ├──────────────┤
                       │ MaCTDH (PK)  │
                       │ MaDH (FK)    │
                       │ MaSP (FK)    │
                       │ TenSP        │
                       │ SoLuong      │
                       │ DonGia       │
                       │ CapDoCay     │
                       │ LoaiNuocDung │
                       │ ThanhTien    │
                       └──────────────┘

┌──────────────┐       ┌──────────────┐       ┌──────────────┐
│  ChiNhanh    │       │   TonKho     │       │ NguyenVatLieu│
├──────────────┤       ├──────────────┤       ├──────────────┤
│ MaCN (PK)    │◄──────│ MaCN (FK)    │       │ MaNVL (PK)   │
│ TenChiNhanh  │   1:N │ MaTonKho(PK) │       │ TenNVL       │◄──────┐
│ DiaChi       │       │ MaNVL (FK)   │───────│ DonViTinh    │   1:N │
│ QuanHuyen    │       │ SoLuong      │       │ SoLuongToi   │       │
│ ThanhPho     │       │ NgayCapNhat  │       │ Thieu        │       │
│ SoDienThoai  │       └──────────────┘       │ TrangThai    │       │
│ GioMoCua     │                              └──────────────┘       │
│ GioDongCua   │                                                     │
│ TrangThai    │       ┌──────────────┐                              │
└──────────────┘       │   CongThuc   │──────────────────────────────┘
                       ├──────────────┤
                       │ MaCT (PK)    │
                       │ MaSP (FK)    │
                       │ MaNVL (FK)   │
                       │ SoLuong      │
                       └──────────────┘

┌──────────────┐       ┌──────────────┐
│  MaGiamGia   │       │   DanhGia    │
├──────────────┤       ├──────────────┤
│ MaMGG (PK)   │       │ MaDG (PK)    │
│ MaCode       │       │ MaKH (FK)    │
│ MoTa         │       │ MaSP (FK)    │
│ LoaiGiam     │       │ MaDH (FK)    │
│ GiaTri       │       │ SoSao        │
│ GiamToiDa    │       │ NoiDung      │
│ DonToiThieu  │       │ NgayDanhGia  │
│ NgayBatDau   │       │ TrangThai    │
│ NgayKetThuc  │       └──────────────┘
│ SoLuong      │
│ DaSuDung     │
│ TrangThai    │
└──────────────┘
```


### 4.6. Bảng Tổng hợp API Endpoints

| Module | Method | Endpoint | Mô tả | Controller Method |
|--------|--------|----------|-------|-------------------|
| **Authentication** |
| | POST | `/api/auth/login` | Đăng nhập | `AuthController.Login()` |
| | POST | `/api/auth/register` | Đăng ký tài khoản | `AuthController.Register()` |
| | GET | `/api/auth/profile` | Lấy thông tin profile | `AuthController.GetProfile()` |
| | POST | `/api/auth/refresh` | Làm mới token | `AuthController.RefreshToken()` |
| | GET | `/api/auth/accounts` | Danh sách tài khoản (Admin) | `AuthController.GetAccounts()` |
| | POST | `/api/auth/accounts` | Tạo tài khoản mới | `AuthController.CreateAccount()` |
| | PUT | `/api/auth/accounts/{id}` | Cập nhật tài khoản | `AuthController.UpdateAccount()` |
| **Products** |
| | GET | `/api/products` | Danh sách sản phẩm | `ProductsController.GetProducts()` |
| | GET | `/api/products/{id}` | Chi tiết sản phẩm | `ProductsController.GetProduct()` |
| | GET | `/api/products/featured` | Sản phẩm nổi bật | `ProductsController.GetFeaturedProducts()` |
| | GET | `/api/products/categories` | Danh mục sản phẩm | `ProductsController.GetCategories()` |
| | POST | `/api/products` | Thêm sản phẩm (Admin) | `ProductsController.CreateProduct()` |
| | PUT | `/api/products/{id}` | Sửa sản phẩm | `ProductsController.UpdateProduct()` |
| | DELETE | `/api/products/{id}` | Xóa sản phẩm | `ProductsController.DeleteProduct()` |
| **Cart** |
| | GET | `/api/cart` | Lấy giỏ hàng | `CartController.GetCart()` |
| | POST | `/api/cart` | Thêm vào giỏ | `CartController.AddToCart()` |
| | PUT | `/api/cart/{id}` | Cập nhật số lượng | `CartController.UpdateCartItem()` |
| | DELETE | `/api/cart/{id}` | Xóa khỏi giỏ | `CartController.RemoveFromCart()` |
| | POST | `/api/cart/apply-promo` | Áp dụng mã giảm giá | `CartController.ApplyPromoCode()` |
| **Orders** |
| | GET | `/api/orders` | Danh sách đơn hàng | `OrdersController.GetOrders()` |
| | GET | `/api/orders/{id}` | Chi tiết đơn hàng | `OrdersController.GetOrder()` |
| | POST | `/api/orders` | Tạo đơn hàng | `OrdersController.CreateOrder()` |
| | PUT | `/api/orders/{id}/status` | Cập nhật trạng thái | `OrdersController.UpdateOrderStatus()` |
| | DELETE | `/api/orders/{id}` | Hủy đơn hàng | `OrdersController.CancelOrder()` |
| | GET | `/api/orders/search` | Tra cứu đơn hàng | `OrdersController.SearchOrder()` |
| | GET | `/api/orders/my` | Đơn hàng của tôi | `OrdersController.GetMyOrders()` |
| | GET | `/api/orders/stats` | Thống kê đơn hàng | `OrdersController.GetOrderStats()` |
| **Branch** |
| | GET | `/api/branch` | Danh sách chi nhánh | `BranchController.GetAll()` |
| | GET | `/api/branch/{id}` | Chi tiết chi nhánh | `BranchController.GetById()` |
| | POST | `/api/branch` | Thêm chi nhánh | `BranchController.Create()` |
| | PUT | `/api/branch/{id}` | Sửa chi nhánh | `BranchController.Update()` |
| **Reports** |
| | GET | `/api/reports/dashboard` | Thống kê dashboard | `ReportsController.GetDashboardStats()` |
| | GET | `/api/reports/revenue` | Báo cáo doanh thu | `ReportsController.GetRevenueReport()` |
| | GET | `/api/reports/top-products` | Top sản phẩm | `ReportsController.GetTopProducts()` |
| | GET | `/api/reports/category-revenue` | Doanh thu theo danh mục | `ReportsController.GetCategoryRevenue()` |


### 4.7. Công nghệ và Thư viện Sử dụng

| Công nghệ | Phiên bản | Mục đích sử dụng |
|-----------|-----------|------------------|
| **Backend** |
| ASP.NET Core | 9.0 | Framework chính cho Web API và Razor Pages |
| Entity Framework Core | 9.0 | ORM - Object Relational Mapping |
| Pomelo.EntityFrameworkCore.MySql | 9.0 | Provider kết nối MySQL/MariaDB |
| Microsoft.AspNetCore.Authentication.JwtBearer | 9.0 | Xác thực JWT |
| System.IdentityModel.Tokens.Jwt | 8.0 | Tạo và xác thực JWT Token |
| **Database** |
| MySQL/MariaDB | 10.4+ | Hệ quản trị CSDL quan hệ |
| **Frontend** |
| HTML5/CSS3 | - | Cấu trúc và định kiểu giao diện |
| JavaScript (ES6+) | - | Xử lý tương tác phía client |
| Chart.js | 4.x | Biểu đồ thống kê |
| Font Awesome | 6.x | Icon library |

### 4.8. Các Hàm và Phương thức Quan trọng

#### 4.8.1. Entity Framework Core Methods

| Method | Mô tả | Ví dụ sử dụng |
|--------|-------|---------------|
| `DbSet<T>.FindAsync(id)` | Tìm entity theo Primary Key | `_context.SanPhams.FindAsync(id)` |
| `DbSet<T>.Include()` | Eager Loading - Load quan hệ | `_context.DonHangs.Include(d => d.KhachHang)` |
| `DbSet<T>.FirstOrDefaultAsync()` | Lấy bản ghi đầu tiên hoặc null | `_context.TaiKhoans.FirstOrDefaultAsync(t => t.Email == email)` |
| `DbSet<T>.AnyAsync()` | Kiểm tra tồn tại | `_context.DonHangs.AnyAsync(d => d.MaDHCode == code)` |
| `DbSet<T>.CountAsync()` | Đếm số bản ghi | `_context.DonHangs.CountAsync(d => d.TrangThai == "Hoàn thành")` |
| `DbSet<T>.SumAsync()` | Tính tổng | `_context.DonHangs.SumAsync(d => d.TongTien)` |
| `DbSet<T>.Where()` | Lọc dữ liệu | `_context.SanPhams.Where(s => s.TrangThai == true)` |
| `DbSet<T>.OrderByDescending()` | Sắp xếp giảm dần | `query.OrderByDescending(d => d.NgayDat)` |
| `DbSet<T>.Skip().Take()` | Phân trang | `query.Skip((page-1)*pageSize).Take(pageSize)` |
| `DbSet<T>.Select()` | Projection - Chọn cột | `query.Select(d => new OrderDto { ... })` |
| `DbSet<T>.Add()` | Thêm entity mới | `_context.DonHangs.Add(donHang)` |
| `DbContext.SaveChangesAsync()` | Lưu thay đổi vào DB | `await _context.SaveChangesAsync()` |

#### 4.8.2. LINQ Query Methods

```csharp
// Ví dụ Query phức tạp - Lấy chi tiết đơn hàng với hình ảnh sản phẩm
order.Items = await _context.ChiTietDonHangs
    .Where(c => c.MaDH == id)                    // Lọc theo mã đơn hàng
    .Join(_context.SanPhams,                     // Join với bảng SanPham
          c => c.MaSP,                           // Khóa ngoại
          p => p.MaSP,                           // Khóa chính
          (c, p) => new { c, p })                // Kết quả join
    .Select(x => new OrderItemDto                // Projection
    {
        ProductId = x.c.MaSP,
        Name = x.c.TenSP,
        Price = (int)x.c.DonGia,
        Quantity = x.c.SoLuong,
        SpicyLevel = x.c.CapDoCay,
        BrothType = x.c.LoaiNuocDung,
        Image = x.p.HinhAnh                      // Lấy hình ảnh từ SanPham
    })
    .ToListAsync();                              // Execute query
```
