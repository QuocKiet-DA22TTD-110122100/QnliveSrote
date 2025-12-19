using MySqlConnector;

namespace MyCay.Web;

/// <summary>
/// Helper class để test kết nối MySQL
/// Chạy: dotnet run --project MyCay.Web -- --test-db
/// </summary>
public static class TestDbConnection
{
    public static async Task<bool> TestConnectionAsync(string connectionString)
    {
        Console.WriteLine("===========================================");
        Console.WriteLine("TEST KẾT NỐI MYSQL");
        Console.WriteLine("===========================================");
        Console.WriteLine($"Connection String: {connectionString}");
        Console.WriteLine();

        try
        {
            using var connection = new MySqlConnection(connectionString);
            Console.WriteLine("Đang kết nối...");
            await connection.OpenAsync();
            
            Console.WriteLine("✓ Kết nối thành công!");
            Console.WriteLine($"  Server Version: {connection.ServerVersion}");
            Console.WriteLine($"  Database: {connection.Database}");
            
            // Test query
            using var cmd = new MySqlCommand("SELECT COUNT(*) FROM TaiKhoan", connection);
            var count = await cmd.ExecuteScalarAsync();
            Console.WriteLine($"  Số tài khoản: {count}");
            
            // Liệt kê tài khoản
            Console.WriteLine();
            Console.WriteLine("Danh sách tài khoản:");
            Console.WriteLine("-------------------------------------------");
            
            using var cmd2 = new MySqlCommand(@"
                SELECT t.TenDangNhap, t.Email, v.TenVaiTro 
                FROM TaiKhoan t 
                JOIN VaiTro v ON t.MaVaiTro = v.MaVaiTro 
                ORDER BY t.MaVaiTro, t.TenDangNhap", connection);
            
            using var reader = await cmd2.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                Console.WriteLine($"  {reader["TenDangNhap"],-15} | {reader["TenVaiTro"],-15} | {reader["Email"]}");
            }
            
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("Mật khẩu tất cả: 123456");
            Console.WriteLine("===========================================");
            
            return true;
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"✗ Lỗi MySQL: {ex.Message}");
            Console.WriteLine();
            Console.WriteLine("Kiểm tra:");
            Console.WriteLine("  1. XAMPP MySQL đã chạy chưa?");
            Console.WriteLine("  2. Port 3306 có bị block không?");
            Console.WriteLine("  3. Database MyCayDB đã tạo chưa?");
            Console.WriteLine("  4. User/Password có đúng không?");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Lỗi: {ex.Message}");
            return false;
        }
    }
}
