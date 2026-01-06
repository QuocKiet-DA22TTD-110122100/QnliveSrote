using System.Diagnostics;

namespace MyCay.Web.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var requestId = Guid.NewGuid().ToString("N")[..8];
        
        // Add request ID to response headers for tracing
        context.Response.Headers["X-Request-Id"] = requestId;
        
        var method = context.Request.Method;
        var path = context.Request.Path;
        var query = context.Request.QueryString;
        
        _logger.LogInformation(
            "📥 [{RequestId}] {Method} {Path}{Query} - Started",
            requestId, method, path, query);

        try
        {
            await _next(context);
            
            stopwatch.Stop();
            var statusCode = context.Response.StatusCode;
            var elapsed = stopwatch.ElapsedMilliseconds;
            
            var logLevel = statusCode switch
            {
                >= 500 => LogLevel.Error,
                >= 400 => LogLevel.Warning,
                _ => LogLevel.Information
            };

            _logger.Log(logLevel,
                "📤 [{RequestId}] {Method} {Path} → {StatusCode} ({Elapsed}ms)",
                requestId, method, path, statusCode, elapsed);

            // Log slow requests
            if (elapsed > 1000)
            {
                _logger.LogWarning(
                    "⚠️ [{RequestId}] Slow request: {Method} {Path} took {Elapsed}ms",
                    requestId, method, path, elapsed);
            }
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex,
                "❌ [{RequestId}] {Method} {Path} - Exception after {Elapsed}ms: {Message}",
                requestId, method, path, stopwatch.ElapsedMilliseconds, ex.Message);
            throw;
        }
    }
}

public static class RequestLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app)
    {
        return app.UseMiddleware<RequestLoggingMiddleware>();
    }
}
