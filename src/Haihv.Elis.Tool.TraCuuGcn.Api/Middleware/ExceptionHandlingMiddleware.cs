using System.Net;
using System.Text.Json;
using Haihv.Elis.Tool.TraCuuGcn.Api.Exceptions;
using ILogger = Serilog.ILogger;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Middleware;

/// <summary>
/// Middleware xử lý exception toàn cục cho ứng dụng.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    /// <summary>
    /// Khởi tạo một instance mới của <see cref="ExceptionHandlingMiddleware"/>.
    /// </summary>
    /// <param name="next">Request delegate tiếp theo trong pipeline.</param>
    /// <param name="logger">Logger để ghi log exception.</param>
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Xử lý request và bắt exception.
    /// </summary>
    /// <param name="context">HTTP context.</param>
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
        var statusCode = HttpStatusCode.InternalServerError;
        var errorCode = "INTERNAL_SERVER_ERROR";
        var message = "Đã xảy ra lỗi trong quá trình xử lý yêu cầu.";

        // Xử lý các loại exception khác nhau
        switch (exception)
        {
            case TraCuuGcnException traCuuGcnException:
                statusCode = traCuuGcnException.StatusCode;
                errorCode = traCuuGcnException.ErrorCode;
                message = traCuuGcnException.Message;
                
                // Log với mức độ phù hợp dựa trên status code
                if ((int)statusCode >= 500)
                {
                    _logger.Error(exception, "Lỗi server: {ErrorCode} - {Message}", errorCode, message);
                }
                else if ((int)statusCode >= 400)
                {
                    _logger.Warning("Lỗi client: {ErrorCode} - {Message}", errorCode, message);
                }
                break;
                
            case UnauthorizedAccessException:
                statusCode = HttpStatusCode.Unauthorized;
                errorCode = "UNAUTHORIZED";
                message = "Bạn không có quyền truy cập tài nguyên này.";
                _logger.Warning(exception, "Truy cập trái phép: {Message}", message);
                break;
                
            default:
                // Log lỗi nội bộ
                _logger.Error(exception, "Lỗi không xác định: {Message}", exception.Message);
                break;
        }

        // Thiết lập response
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            error = new
            {
                code = errorCode,
                message
            }
        };

        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(jsonResponse);
    }
}