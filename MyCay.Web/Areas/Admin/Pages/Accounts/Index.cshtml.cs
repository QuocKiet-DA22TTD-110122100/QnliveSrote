using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyCay.Web.Areas.Admin.Pages.Accounts
{
    public class IndexModel : PageModel
    {
        private readonly IWebHostEnvironment _env;
        private string DataFile => Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "data", "accounts.json");

        public List<AccountView> Accounts { get; set; } = new();

        public IndexModel(IWebHostEnvironment env)
        {
            _env = env;
        }

        public void OnGet()
        {
            if (System.IO.File.Exists(DataFile))
            {
                var json = System.IO.File.ReadAllText(DataFile);
                Accounts = JsonSerializer.Deserialize<List<AccountView>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
            }
            else
            {
                // Sample data for UI demo. Replace with real data access.
                Accounts = new List<AccountView>
                {
                    new AccountView { FullName = "Nguyễn Văn A", Email = "a@example.com", Role = "Admin", IsActive = true },
                    new AccountView { FullName = "Trần Thị B", Email = "b@example.com", Role = "Nhân viên", IsActive = true },
                    new AccountView { FullName = "Lê Văn C", Email = "c@example.com", Role = "Nhân viên", IsActive = false },
                };
            }
        }

        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> OnPostSaveAsync()
        {
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();
            var list = JsonSerializer.Deserialize<List<AccountView>>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (list == null) return BadRequest();

            var dir = Path.GetDirectoryName(DataFile)!;
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            await System.IO.File.WriteAllTextAsync(DataFile, JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true }));
            return new JsonResult(new { success = true });
        }
    }

    public class AccountView
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
