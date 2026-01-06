# =====================================================
# TEST API - My Cay Sasin
# =====================================================

$baseUrl = "http://localhost:5000"
$token = ""

Write-Host "==========================================="
Write-Host "TEST API - MY CAY SASIN"
Write-Host "==========================================="
Write-Host ""

# Helper function
function Test-Api {
    param(
        [string]$Method,
        [string]$Url,
        [object]$Body = $null,
        [string]$Token = ""
    )
    
    $headers = @{ "Content-Type" = "application/json" }
    if ($Token) { $headers["Authorization"] = "Bearer $Token" }
    
    try {
        if ($Body) {
            $json = $Body | ConvertTo-Json -Depth 10
            $response = Invoke-RestMethod -Uri "$baseUrl$Url" -Method $Method -Headers $headers -Body $json -ErrorAction Stop
        } else {
            $response = Invoke-RestMethod -Uri "$baseUrl$Url" -Method $Method -Headers $headers -ErrorAction Stop
        }
        return @{ Success = $true; Data = $response }
    }
    catch {
        return @{ Success = $false; Error = $_.Exception.Message }
    }
}

# =====================================================
# 1. TEST AUTH API
# =====================================================
Write-Host "1. TEST AUTH API" -ForegroundColor Cyan
Write-Host "-------------------------------------------"

# Test Login Admin
Write-Host "   [1.1] Login Admin..."
$loginResult = Test-Api -Method "POST" -Url "/api/auth/login" -Body @{
    email = "admin"
    password = "123456"
}
if ($loginResult.Success -and $loginResult.Data.success) {
    Write-Host "   [OK] Login Admin thanh cong" -ForegroundColor Green
    $token = $loginResult.Data.data.token
    Write-Host "   Role: $($loginResult.Data.data.role)"
} else {
    Write-Host "   [FAIL] Login Admin that bai: $($loginResult.Error)" -ForegroundColor Red
}

# Test Login Manager
Write-Host "   [1.2] Login Manager..."
$loginResult2 = Test-Api -Method "POST" -Url "/api/auth/login" -Body @{
    email = "quanly1"
    password = "123456"
}
if ($loginResult2.Success -and $loginResult2.Data.success) {
    Write-Host "   [OK] Login Manager thanh cong - Role: $($loginResult2.Data.data.role)" -ForegroundColor Green
} else {
    Write-Host "   [FAIL] Login Manager that bai" -ForegroundColor Red
}

# Test Login Staff
Write-Host "   [1.3] Login Staff..."
$loginResult3 = Test-Api -Method "POST" -Url "/api/auth/login" -Body @{
    email = "nhanvien1"
    password = "123456"
}
if ($loginResult3.Success -and $loginResult3.Data.success) {
    Write-Host "   [OK] Login Staff thanh cong - Role: $($loginResult3.Data.data.role)" -ForegroundColor Green
} else {
    Write-Host "   [FAIL] Login Staff that bai" -ForegroundColor Red
}

# Test Login Customer
Write-Host "   [1.4] Login Customer..."
$loginResult4 = Test-Api -Method "POST" -Url "/api/auth/login" -Body @{
    email = "khachhang1"
    password = "123456"
}
if ($loginResult4.Success -and $loginResult4.Data.success) {
    Write-Host "   [OK] Login Customer thanh cong - Role: $($loginResult4.Data.data.role)" -ForegroundColor Green
} else {
    Write-Host "   [FAIL] Login Customer that bai" -ForegroundColor Red
}

# Test Wrong Password
Write-Host "   [1.5] Login sai mat khau..."
$loginWrong = Test-Api -Method "POST" -Url "/api/auth/login" -Body @{
    email = "admin"
    password = "wrongpassword"
}
if (-not $loginWrong.Data.success) {
    Write-Host "   [OK] Tu choi dang nhap sai mat khau" -ForegroundColor Green
} else {
    Write-Host "   [FAIL] Khong tu choi dang nhap sai" -ForegroundColor Red
}

# Test Get Profile
Write-Host "   [1.6] Get Profile..."
$profileResult = Test-Api -Method "GET" -Url "/api/auth/profile" -Token $token
if ($profileResult.Success -and $profileResult.Data.success) {
    Write-Host "   [OK] Get Profile thanh cong" -ForegroundColor Green
} else {
    Write-Host "   [FAIL] Get Profile that bai" -ForegroundColor Red
}

Write-Host ""

# =====================================================
# 2. TEST PRODUCTS API
# =====================================================
Write-Host "2. TEST PRODUCTS API" -ForegroundColor Cyan
Write-Host "-------------------------------------------"

# Get all products
Write-Host "   [2.1] Get all products..."
$productsResult = Test-Api -Method "GET" -Url "/api/products"
if ($productsResult.Success) {
    $count = $productsResult.Data.Count
    if ($count -eq $null) { $count = $productsResult.Data.data.Count }
    Write-Host "   [OK] Lay duoc $count san pham" -ForegroundColor Green
} else {
    Write-Host "   [FAIL] Lay san pham that bai" -ForegroundColor Red
}

# Get featured products
Write-Host "   [2.2] Get featured products..."
$featuredResult = Test-Api -Method "GET" -Url "/api/products/featured"
if ($featuredResult.Success) {
    Write-Host "   [OK] Lay san pham noi bat thanh cong" -ForegroundColor Green
} else {
    Write-Host "   [FAIL] Lay san pham noi bat that bai" -ForegroundColor Red
}

# Get categories
Write-Host "   [2.3] Get categories..."
$categoriesResult = Test-Api -Method "GET" -Url "/api/products/categories"
if ($categoriesResult.Success) {
    Write-Host "   [OK] Lay danh muc thanh cong" -ForegroundColor Green
} else {
    Write-Host "   [FAIL] Lay danh muc that bai" -ForegroundColor Red
}

Write-Host ""

# =====================================================
# 3. TEST CART API
# =====================================================
Write-Host "3. TEST CART API" -ForegroundColor Cyan
Write-Host "-------------------------------------------"

$sessionId = [guid]::NewGuid().ToString()

# Add to cart
Write-Host "   [3.1] Add to cart..."
$addCartResult = Test-Api -Method "POST" -Url "/api/cart" -Body @{
    sessionId = $sessionId
    productId = 1
    quantity = 2
    spicyLevel = 3
}
if ($addCartResult.Success -and $addCartResult.Data.success) {
    Write-Host "   [OK] Them vao gio hang thanh cong" -ForegroundColor Green
} else {
    Write-Host "   [FAIL] Them vao gio hang that bai" -ForegroundColor Red
}

# Get cart
Write-Host "   [3.2] Get cart..."
$getCartResult = Test-Api -Method "GET" -Url "/api/cart?sessionId=$sessionId"
if ($getCartResult.Success) {
    Write-Host "   [OK] Lay gio hang thanh cong" -ForegroundColor Green
} else {
    Write-Host "   [FAIL] Lay gio hang that bai" -ForegroundColor Red
}

# Get promo codes
Write-Host "   [3.3] Get promo codes..."
$promoResult = Test-Api -Method "GET" -Url "/api/cart/promo-codes"
if ($promoResult.Success -and $promoResult.Data.success) {
    $promoCount = $promoResult.Data.data.Count
    Write-Host "   [OK] Lay duoc $promoCount ma giam gia" -ForegroundColor Green
} else {
    Write-Host "   [FAIL] Lay ma giam gia that bai" -ForegroundColor Red
}

# Apply promo code
Write-Host "   [3.4] Apply promo code SASIN10..."
$applyPromoResult = Test-Api -Method "POST" -Url "/api/cart/apply-promo" -Body @{
    code = "SASIN10"
    subtotal = 150000
}
if ($applyPromoResult.Success -and $applyPromoResult.Data.success) {
    Write-Host "   [OK] Ap dung ma giam gia thanh cong - Giam: $($applyPromoResult.Data.data.discount)" -ForegroundColor Green
} else {
    Write-Host "   [FAIL] Ap dung ma giam gia that bai" -ForegroundColor Red
}

Write-Host ""

# =====================================================
# 4. TEST ORDERS API
# =====================================================
Write-Host "4. TEST ORDERS API" -ForegroundColor Cyan
Write-Host "-------------------------------------------"

# Create order
Write-Host "   [4.1] Create order..."
$createOrderResult = Test-Api -Method "POST" -Url "/api/orders" -Body @{
    customerName = "Test Customer"
    phone = "0901234567"
    address = "123 Test Street"
    subtotal = 150000
    shippingFee = 15000
    discount = 15000
    paymentMethod = "Tien mat"
    items = @(
        @{ productId = 1; name = "Mi Thap Cam"; price = 77000; quantity = 2; spicyLevel = 3 }
    )
}
if ($createOrderResult.Success -and $createOrderResult.Data.success) {
    $orderId = $createOrderResult.Data.data.orderId
    Write-Host "   [OK] Tao don hang thanh cong - ID: $orderId" -ForegroundColor Green
} else {
    Write-Host "   [FAIL] Tao don hang that bai" -ForegroundColor Red
    $orderId = 1
}

# Get orders
Write-Host "   [4.2] Get orders..."
$ordersResult = Test-Api -Method "GET" -Url "/api/orders"
if ($ordersResult.Success) {
    Write-Host "   [OK] Lay danh sach don hang thanh cong" -ForegroundColor Green
} else {
    Write-Host "   [FAIL] Lay danh sach don hang that bai" -ForegroundColor Red
}

# Get order stats
Write-Host "   [4.3] Get order stats..."
$statsResult = Test-Api -Method "GET" -Url "/api/orders/stats"
if ($statsResult.Success -and $statsResult.Data.success) {
    Write-Host "   [OK] Lay thong ke don hang thanh cong" -ForegroundColor Green
    Write-Host "        Cho xu ly: $($statsResult.Data.data.pending)"
    Write-Host "        Hoan thanh: $($statsResult.Data.data.completed)"
} else {
    Write-Host "   [FAIL] Lay thong ke that bai" -ForegroundColor Red
}

Write-Host ""

# =====================================================
# 5. TEST CUSTOMERS API
# =====================================================
Write-Host "5. TEST CUSTOMERS API" -ForegroundColor Cyan
Write-Host "-------------------------------------------"

Write-Host "   [5.1] Get customers..."
$customersResult = Test-Api -Method "GET" -Url "/api/customers"
if ($customersResult.Success) {
    Write-Host "   [OK] Lay danh sach khach hang thanh cong" -ForegroundColor Green
} else {
    Write-Host "   [FAIL] Lay danh sach khach hang that bai" -ForegroundColor Red
}

Write-Host ""

# =====================================================
# 6. TEST COUPON API
# =====================================================
Write-Host "6. TEST COUPON API" -ForegroundColor Cyan
Write-Host "-------------------------------------------"

Write-Host "   [6.1] Get all coupons..."
$couponsResult = Test-Api -Method "GET" -Url "/api/coupon"
if ($couponsResult.Success) {
    $couponCount = $couponsResult.Data.Count
    Write-Host "   [OK] Lay duoc $couponCount ma giam gia" -ForegroundColor Green
} else {
    Write-Host "   [FAIL] Lay ma giam gia that bai" -ForegroundColor Red
}

Write-Host "   [6.2] Validate coupon..."
$validateResult = Test-Api -Method "POST" -Url "/api/coupon/validate" -Body @{
    code = "SASIN20"
    orderTotal = 250000
}
if ($validateResult.Success -and $validateResult.Data.valid) {
    Write-Host "   [OK] Validate coupon thanh cong - Giam: $($validateResult.Data.discount)" -ForegroundColor Green
} else {
    Write-Host "   [FAIL] Validate coupon that bai" -ForegroundColor Red
}

Write-Host ""

# =====================================================
# 7. TEST BRANCH API
# =====================================================
Write-Host "7. TEST BRANCH API" -ForegroundColor Cyan
Write-Host "-------------------------------------------"

Write-Host "   [7.1] Get branches..."
$branchResult = Test-Api -Method "GET" -Url "/api/branch"
if ($branchResult.Success) {
    Write-Host "   [OK] Lay danh sach chi nhanh thanh cong" -ForegroundColor Green
} else {
    Write-Host "   [FAIL] Lay chi nhanh that bai" -ForegroundColor Red
}

Write-Host ""

# =====================================================
# 8. TEST REPORTS API
# =====================================================
Write-Host "8. TEST REPORTS API" -ForegroundColor Cyan
Write-Host "-------------------------------------------"

Write-Host "   [8.1] Get dashboard stats..."
$dashboardResult = Test-Api -Method "GET" -Url "/api/reports/dashboard"
if ($dashboardResult.Success) {
    Write-Host "   [OK] Lay thong ke dashboard thanh cong" -ForegroundColor Green
} else {
    Write-Host "   [FAIL] Lay thong ke dashboard that bai" -ForegroundColor Red
}

Write-Host ""
Write-Host "==========================================="
Write-Host "HOAN TAT TEST API"
Write-Host "==========================================="
