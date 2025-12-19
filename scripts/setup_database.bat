@echo off
chcp 65001 >nul
echo =====================================================
echo SETUP DATABASE MYCAYDB (MySQL/XAMPP)
echo =====================================================
echo.

set MYSQL_PATH=C:\xampp\mysql\bin\mysql.exe
set HOST=127.0.0.1
set PORT=3306
set USER=root
set PASSWORD=
set DATABASE=MyCayDB

echo Kiem tra MySQL...
"%MYSQL_PATH%" -h %HOST% -P %PORT% -u %USER% -e "SELECT 1;" >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo [FAIL] MySQL chua chay! Hay khoi dong XAMPP truoc.
    pause
    exit /b 1
)
echo [OK] MySQL dang chay

echo.
echo Tao database va du lieu...
echo.

REM Chạy file tạo database MySQL
if exist "csdl\MyCayDB_MySQL.sql" (
    echo Dang chay MyCayDB_MySQL.sql...
    "%MYSQL_PATH%" -h %HOST% -P %PORT% -u %USER% < "csdl\MyCayDB_MySQL.sql"
    if %ERRORLEVEL% EQU 0 (
        echo [OK] Tao database thanh cong!
    ) else (
        echo [FAIL] Loi khi tao database
    )
) else (
    echo [WARN] Khong tim thay file MyCayDB_MySQL.sql
)

REM Chạy file thêm tài khoản test
if exist "csdl\seed_test_accounts.sql" (
    echo.
    echo Dang chay seed_test_accounts.sql...
    "%MYSQL_PATH%" -h %HOST% -P %PORT% -u %USER% -D %DATABASE% < "csdl\seed_test_accounts.sql"
    if %ERRORLEVEL% EQU 0 (
        echo [OK] Them tai khoan test thanh cong!
    ) else (
        echo [FAIL] Loi khi them tai khoan
    )
)

echo.
echo =====================================================
echo HOAN TAT!
echo =====================================================
echo.
echo Tai khoan test (mat khau: 123456):
echo   - admin, admin2 (Quan tri vien)
echo   - quanly1, quanly2, quanly3 (Quan ly)
echo   - nhanvien1, nhanvien2, nhanvien3, nhanvien4 (Nhan vien)
echo   - khachhang1, khachhang2, khachhang3, khachhang4 (Khach hang)
echo.
pause
