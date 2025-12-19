using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MyCay.Infrastructure.Data;
using MyCay.Web.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Enable detailed logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

// Add MySQL Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<MyCayDbContext>(options =>
    options.UseMySql(connectionString, new MariaDbServerVersion(new Version(10, 6, 0))));

// Add JWT Service
builder.Services.AddSingleton<IJwtService, JwtService>();

// Add JWT Authentication
var jwtKey = builder.Configuration["Jwt:SecretKey"] ?? "MyCaySasin_DefaultKey_2024_32Chars!";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "MyCaySasin",
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"] ?? "MyCaySasinApp",
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

// Add services for Razor Pages
builder.Services.AddRazorPages();

// Add services for API Controllers
builder.Services.AddControllers();

// Add HttpClient for Gemini API
builder.Services.AddHttpClient();

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
app.UseAuthentication();
app.UseAuthorization();
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
