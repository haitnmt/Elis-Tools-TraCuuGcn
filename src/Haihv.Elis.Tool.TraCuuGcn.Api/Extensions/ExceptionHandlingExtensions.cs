using Haihv.Elis.Tool.TraCuuGcn.Api.Middleware;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;

/// <summary>
/// Extension methods để cấu hình xử lý exception.
/// </summary>
public static class ExceptionHandlingExtensions
{
    /// <summary>
    /// Thêm middleware xử lý exception toàn cục vào pipeline.
    /// </summary>
    /// <param name="app">Application builder.</param>
    /// <returns>Application builder đã được cấu hình.</returns>
    public static void UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}