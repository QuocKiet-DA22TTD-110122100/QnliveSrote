var builder = WebApplication.CreateBuilder(args);

// Add services for Razor Pages
builder.Services.AddRazorPages();

var app = builder.Build();

// Serve static files (for CSS/JS)
app.UseStaticFiles();

app.MapRazorPages();

app.MapGet("/", context =>
{
	context.Response.Redirect("/Admin/Accounts");
	return System.Threading.Tasks.Task.CompletedTask;
});

app.Run();
