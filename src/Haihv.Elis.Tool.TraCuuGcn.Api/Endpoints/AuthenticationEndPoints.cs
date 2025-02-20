using Haihv.Elis.Tool.TraCuuGcn.Api.Authenticate;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Endpoints;

public static class AuthenticationEndPoints
{
    /// <summary>
    /// Định nghĩa endpoint cho xác thực chủ sử dụng.
    /// </summary>
    /// <param name="app">Ứng dụng web để cấu hình endpoint.</param>
    public static void MapAuthenticationEndpoints(this WebApplication app)
    {
        app.MapPost("/elis/auth", PostAuthChuSuDungAsync)
            .WithName("PostAuthChuSuDungAsync");
    }

    /// <summary>
    /// Xử lý yêu cầu xác thực chủ sử dụng.
    /// </summary>
    /// <param name="authChuSuDung">Thông tin xác thực chủ sử dụng.</param>
    /// <param name="logger">Logger để ghi log.</param>
    /// <param name="authenticationService">Dịch vụ xác thực.</param>
    /// <returns>Kết quả xác thực dưới dạng <see cref="IResult"/>.</returns>
    private static async Task<IResult> PostAuthChuSuDungAsync(
        [FromBody] AuthChuSuDung authChuSuDung,
        ILogger logger,
        IAuthenticationService authenticationService)
    {
        var result = await authenticationService.AuthChuSuDungAsync(authChuSuDung);
        return await Task.FromResult(result.Match(
            token => 
            {
                logger.Information("Xác thực chủ sử dụng thành công: {SoDinhDanh}", 
                    authChuSuDung.SoDinhDanh);
                return Results.Ok(token);
            },
            ex =>
            {
                logger.Error(ex, "Lỗi khi xác thực chủ sử dụng: {SoDinhDanh}", 
                    authChuSuDung.SoDinhDanh);
                return Results.BadRequest(ex.Message);
            }));
    }
}