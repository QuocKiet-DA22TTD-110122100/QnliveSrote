using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyCay.Infrastructure.Data;
using System.Diagnostics;
using System.Reflection;

namespace MyCay.Web.Api;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly MyCayDbContext _context;
    private readonly ILogger<HealthController> _logger;

    public HealthController(MyCayDbContext context, ILogger<HealthController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Basic health check - returns OK if API is running
    /// </summary>
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            status = "healthy",
            timestamp = DateTime.UtcNow,
            version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "1.0.0"
        });
    }

    /// <summary>
    /// Detailed health check - checks database connectivity
    /// </summary>
    [HttpGet("detailed")]
    public async Task<IActionResult> GetDetailed()
    {
        var checks = new List<HealthCheckResult>();
        var overallStatus = "healthy";

        // Check Database
        var dbCheck = await CheckDatabaseAsync();
        checks.Add(dbCheck);
        if (dbCheck.Status != "healthy") overallStatus = "unhealthy";

        // Check Memory
        var memoryCheck = CheckMemory();
        checks.Add(memoryCheck);
        if (memoryCheck.Status == "unhealthy") overallStatus = "unhealthy";

        return Ok(new
        {
            status = overallStatus,
            timestamp = DateTime.UtcNow,
            version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "1.0.0",
            uptime = GetUptime(),
            checks
        });
    }

    /// <summary>
    /// Liveness probe - for Kubernetes/Docker
    /// </summary>
    [HttpGet("live")]
    public IActionResult Live()
    {
        return Ok(new { status = "alive" });
    }

    /// <summary>
    /// Readiness probe - checks if app is ready to serve traffic
    /// </summary>
    [HttpGet("ready")]
    public async Task<IActionResult> Ready()
    {
        try
        {
            // Quick database check
            await _context.Database.ExecuteSqlRawAsync("SELECT 1");
            return Ok(new { status = "ready" });
        }
        catch
        {
            return StatusCode(503, new { status = "not_ready", reason = "Database unavailable" });
        }
    }

    private async Task<HealthCheckResult> CheckDatabaseAsync()
    {
        var sw = Stopwatch.StartNew();
        try
        {
            await _context.Database.ExecuteSqlRawAsync("SELECT 1");
            sw.Stop();
            
            return new HealthCheckResult
            {
                Name = "database",
                Status = "healthy",
                ResponseTime = $"{sw.ElapsedMilliseconds}ms",
                Details = new { provider = "MySQL", connected = true }
            };
        }
        catch (Exception ex)
        {
            sw.Stop();
            _logger.LogError(ex, "Database health check failed");
            
            return new HealthCheckResult
            {
                Name = "database",
                Status = "unhealthy",
                ResponseTime = $"{sw.ElapsedMilliseconds}ms",
                Details = new { provider = "MySQL", connected = false, error = ex.Message }
            };
        }
    }

    private HealthCheckResult CheckMemory()
    {
        var process = Process.GetCurrentProcess();
        var memoryMB = process.WorkingSet64 / 1024 / 1024;
        var status = memoryMB > 500 ? "degraded" : "healthy";
        
        return new HealthCheckResult
        {
            Name = "memory",
            Status = status,
            Details = new
            {
                workingSetMB = memoryMB,
                gcTotalMemoryMB = GC.GetTotalMemory(false) / 1024 / 1024
            }
        };
    }

    private string GetUptime()
    {
        var uptime = DateTime.UtcNow - Process.GetCurrentProcess().StartTime.ToUniversalTime();
        return $"{uptime.Days}d {uptime.Hours}h {uptime.Minutes}m {uptime.Seconds}s";
    }
}

public class HealthCheckResult
{
    public string Name { get; set; } = "";
    public string Status { get; set; } = "healthy";
    public string? ResponseTime { get; set; }
    public object? Details { get; set; }
}
