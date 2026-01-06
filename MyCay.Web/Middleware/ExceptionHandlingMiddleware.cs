using System.Net;
using System.Text.Json;

namespace MyCay.Web.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "❌ Unhandled exception: {Message}", exception.Message);

        var response = context.Response;
        response.ContentType = "application/json";

        var (statusCode, message) = exception switch
        {
            ValidationException e => (HttpStatusCode.BadRequest, e.Message),
            NotFoundException e => (HttpStatusCode.NotFound, e.Message),
            UnauthorizedException e => (HttpStatusCode.Unauthorized, e.Message),
            ForbiddenException e => (HttpStatusCode.Forbidden, e.Message),
            BusinessException e => (HttpStatusCode.BadRequest, e.Message),
            _ => (HttpStatusCode.InternalServerError, "Đã xảy ra lỗi hệ thống. Vui lòng thử lại sau.")
        };

        response.StatusCode = (int)statusCode;

        var result = new ApiErrorResponse
        {
            Success = false,
            Message = message,
            StatusCode = (int)statusCode,
            TraceId = context.TraceIdentifier
        };

        // Include stack trace in development
        if (_env.IsDevelopment() && statusCode == HttpStatusCode.InternalServerError)
        {
            result.Detail = exception.ToString();
        }

        var json = JsonSerializer.Serialize(result, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await response.WriteAsync(json);
    }
}

public class ApiErrorResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = "";
    public int StatusCode { get; set; }
    public string? TraceId { get; set; }
    public string? Detail { get; set; }
}

// Custom Exceptions
public class ValidationException : Exception
{
    public Dictionary<string, string[]> Errors { get; }
    
    public ValidationException(string message) : base(message)
    {
        Errors = new Dictionary<string, string[]>();
    }
    
    public ValidationException(Dictionary<string, string[]> errors) 
        : base("Dữ liệu không hợp lệ")
    {
        Errors = errors;
    }
}

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
    public NotFoundException(string entity, object key) 
        : base($"Không tìm thấy {entity} với ID: {key}") { }
}

public class UnauthorizedException : Exception
{
    public UnauthorizedException(string message = "Bạn chưa đăng nhập") : base(message) { }
}

public class ForbiddenException : Exception
{
    public ForbiddenException(string message = "Bạn không có quyền thực hiện thao tác này") : base(message) { }
}

public class BusinessException : Exception
{
    public BusinessException(string message) : base(message) { }
}

// Extension method to register middleware
public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
