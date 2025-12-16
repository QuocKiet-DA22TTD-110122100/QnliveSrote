using Microsoft.EntityFrameworkCore;
using MyCay.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Enable detailed logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

// Add MySQL Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<MyCayDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Add services for Razor Pages
builder.Services.AddRazorPages();

// Add services for API Controllers
builder.Services.AddControllers();

// Add CORS for API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add Session support
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Serve static files (for CSS/JS)
app.UseStaticFiles();

app.UseCors("AllowAll");
app.UseSession();

// Add request logging middleware
app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("📥 {Method} {Path}", context.Request.Method, context.Request.Path);
    await next();
    logger.LogInformation("📤 {Method} {Path} → {StatusCode}", context.Request.Method, context.Request.Path, context.Response.StatusCode);
});

app.MapRazorPages();
app.MapControllers();

app.Run();
