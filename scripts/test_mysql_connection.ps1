# Script test ket noi MySQL tu XAMPP

$hostName = "127.0.0.1"
$port = "3306"
$database = "MyCayDB"
$user = "root"
$password = ""

Write-Host "==========================================="
Write-Host "TEST KET NOI MYSQL (XAMPP)"
Write-Host "==========================================="
Write-Host "Host: $hostName"
Write-Host "Port: $port"
Write-Host "Database: $database"
Write-Host "User: $user"
Write-Host ""

# Kiem tra port MySQL co mo khong
Write-Host "Kiem tra port $port..."
$tcpClient = New-Object System.Net.Sockets.TcpClient
try {
    $tcpClient.Connect($hostName, $port)
    Write-Host "[OK] Port $port dang mo" -ForegroundColor Green
    $tcpClient.Close()
}
catch {
    Write-Host "[FAIL] Port $port khong mo - MySQL chua chay!" -ForegroundColor Red
    Write-Host ""
    Write-Host "Giai phap:"
    Write-Host "  1. Mo XAMPP Control Panel"
    Write-Host "  2. Click Start ben canh MySQL"
    Write-Host "  3. Cho MySQL chuyen sang mau xanh"
    exit 1
}

# Kiem tra mysql command co san khong
$mysqlPath = "C:\xampp\mysql\bin\mysql.exe"
if (Test-Path $mysqlPath) {
    Write-Host "[OK] Tim thay MySQL client" -ForegroundColor Green
    
    # Test ket noi
    Write-Host ""
    Write-Host "Test ket noi database..."
    
    $testResult = & $mysqlPath -h $hostName -P $port -u $user -e "SELECT 'Connected' AS Status;" 2>&1
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "[OK] Ket noi MySQL thanh cong!" -ForegroundColor Green
        
        # Kiem tra database ton tai
        Write-Host ""
        Write-Host "Kiem tra database $database..."
        
        $dbCheck = & $mysqlPath -h $hostName -P $port -u $user -e "SHOW DATABASES LIKE '$database';" 2>&1
        
        if ($dbCheck -match $database) {
            Write-Host "[OK] Database $database ton tai" -ForegroundColor Green
            
            # Dem so tai khoan
            Write-Host ""
            Write-Host "Danh sach tai khoan:"
            Write-Host "==========================================="
            & $mysqlPath -h $hostName -P $port -u $user -D $database -e "SELECT TenDangNhap, Email, MaVaiTro FROM TaiKhoan ORDER BY MaVaiTro;" 2>&1
            Write-Host "==========================================="
            Write-Host "Mat khau tat ca: 123456"
        }
        else {
            Write-Host "[WARN] Database $database chua ton tai" -ForegroundColor Yellow
            Write-Host ""
            Write-Host "Chay file SQL de tao database:"
            Write-Host "  mysql -u root < csdl/MyCayDB_MySQL.sql"
        }
    }
    else {
        Write-Host "[FAIL] Khong the ket noi MySQL" -ForegroundColor Red
        Write-Host $testResult
    }
}
else {
    Write-Host "[WARN] Khong tim thay MySQL client tai $mysqlPath" -ForegroundColor Yellow
    Write-Host "Thu dung phpMyAdmin de test ket noi: http://localhost/phpmyadmin"
}

Write-Host ""
Write-Host "==========================================="
