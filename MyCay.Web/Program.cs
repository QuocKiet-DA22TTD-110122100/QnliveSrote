using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MyCay.Infrastructure.Data;
using MyCay.Web.Services;
using MyCay.Web.Middleware;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Enable detailed logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

// Add MySQL Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var serverVersion = new MariaDbServerVersion(new Version(10, 4, 0)); // Use 10.4 to disable RETURNING clause
builder.Services.AddDbContext<MyCayDbContext>(options =>
    options.UseMySql(connectionString, serverVersion, mysqlOptions =>
    {
        mysqlOptions.EnableRetryOnFailure(3);
    }));

// Add JWT Service
builder.Services.AddSingleton<IJwtService, JwtService>();

// Add Email Service
builder.Services.AddScoped<IEmailService, EmailService>();

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

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "MyCay API",
        Version = "v1",
        Description = "API cho hệ thống quản lý bán hàng Mỳ Cay Sasin"
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

// Enable Swagger in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyCay API v1");
        c.RoutePrefix = "swagger";
    });
}

// Global exception handling
app.UseExceptionHandling();

// Request logging with timing
app.UseRequestLogging();

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapRazorPages();
app.MapControllers();

app.Run();
