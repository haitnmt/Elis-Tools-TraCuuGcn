using Haihv.Elis.Tool.TraCuuGcn.Api.Authenticate;
using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Services;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using Microsoft.AspNetCore.Http.Extensions;
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
    /// <param name="checkIpService">
    /// Dịch vụ kiểm tra số lần đăng nhập sai của IP.
    /// </param>
    /// <param name="httpContext">
    /// Ngữ cảnh HTTP hiện tại.
    /// </param>
    /// <returns>Kết quả xác thực dưới dạng <see cref="IResult"/>.</returns>
    private static async Task<IResult> PostAuthChuSuDungAsync(
        [FromBody] AuthChuSuDung authChuSuDung,
        ILogger logger,
        IAuthenticationService authenticationService,
        ICheckIpService checkIpService,
        HttpContext httpContext)
    {
        if (string.IsNullOrWhiteSpace(authChuSuDung.SoDinhDanh) || string.IsNullOrWhiteSpace(authChuSuDung.HoVaTen))
        {
            return Results.BadRequest(new Response<AccessToken>("Số định danh và Họ tên không được để trống!"));
        }
        var url = httpContext.Request.GetDisplayUrl();
        var (ipAddress, isPrivate) = httpContext.GetIpInfo();
        var (count, exprSecond) = isPrivate ? (0, 0) : await checkIpService.CheckLockAsync(ipAddress);
        if (exprSecond > 0)
        {
            return Results.BadRequest(new Response<AccessToken>($"Bạn đã đăng nhập sai quá nhiều, thử lại sau {exprSecond} giây!"));
        }
        var result = await authenticationService.AuthChuSuDungAsync(authChuSuDung);
        return await Task.FromResult(result.Match(
            token =>
            {
                if (!isPrivate)
                {
                    checkIpService.ClearLockAsync(ipAddress);
                }

                logger.Information("Xác thực chủ sử dụng thành công: {Url}{MaDinhDanh}{ClientIp}",
                    url,
                    authChuSuDung.SoDinhDanh,
                    ipAddress);
                return Results.Ok(new Response<AccessToken>(token));
            },
            ex =>
            {
                logger.Error(ex, "Lỗi khi xác thực chủ sử dụng: {Url}{MaDinhDanh}{ClientIp}",
                    url,
                    authChuSuDung.SoDinhDanh,
                    ipAddress);
                if (isPrivate)
                {
                    return Results.BadRequest(new Response<AccessToken>(ex.Message));
                }
                checkIpService.SetLockAsync(ipAddress);
                return Results.BadRequest(new Response<AccessToken>(
                    $"{ex.Message} {(count < 3 ? $"Bạn còn {3 - count} lần thử" : "")}"));
            }));
    }
}