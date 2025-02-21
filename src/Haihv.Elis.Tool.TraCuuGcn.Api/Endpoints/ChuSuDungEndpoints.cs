using Haihv.Elis.Tool.TraCuuGcn.Api.Authenticate;
using Haihv.Elis.Tool.TraCuuGcn.Api.Services;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Endpoints;

public static class ChuSuDungEndpoints
{
    /// <summary>
    /// Định nghĩa endpoint để lấy thông tin chủ sử dụng.
    /// </summary>
    /// <param name="app">Ứng dụng web.</param>
    public static void MapChuSuDungEndpoints(this WebApplication app)
    {
        app.MapGet("/elis/chuSuDung", GetChuSuDung)
            .WithName("GetChuSuDung")
            .RequireAuthorization();
    }

    /// <summary>
    /// Lấy thông tin chủ sử dụng theo số định danh.
    /// </summary>
    /// <param name="maGcnElis">Mã GCN của Giấy chứng nhận.</param>
    /// <param name="httpContext">Ngữ cảnh HTTP hiện tại.</param>
    /// <param name="logger">Logger.</param>
    /// <param name="authenticationService">Dịch vụ xác thực.</param>
    /// <param name="chuSuDungService">Dịch vụ chủ sử dụng.</param>
    /// <returns>Kết quả truy vấn chủ sử dụng.</returns>
    private static async Task<IResult> GetChuSuDung(
        [FromQuery] long maGcnElis,
        HttpContext httpContext,
        ILogger logger,
        IAuthenticationService authenticationService,
        IChuSuDungService chuSuDungService)
    {
        // Lấy thông tin người dùng theo token từ HttpClient
        var user = httpContext.User;
        var maDinhDanh = await authenticationService.CheckAuthenticationAsync(maGcnElis, user);
        if (string.IsNullOrWhiteSpace(maDinhDanh))
        {
            logger.Warning("Người dùng không được phép truy cập thông tin Chủ sử dụng. {MaGcnElis} {User}",
                maGcnElis, user.Claims);
            return Results.Unauthorized();
        }
        var result = await chuSuDungService.GetResultAsync(maGcnElis);
        return await Task.FromResult(result.Match(
            chuSuDung =>
            {
                logger.Information("Lấy thông tin chủ sử dụng thành công: {MaGcnElis} {MaDinhDanh}", 
                    maGcnElis, maDinhDanh);
                if (chuSuDung.Count == 0)
                {
                    logger.Warning("Không tìm thấy thông tin chủ sử dụng: {MaGcnElis} {MaDinhDanh}", 
                        maGcnElis, maDinhDanh);
                    return Results.NotFound(new Response<List<ChuSuDungInfo>>("Không tìm thấy thông tin chủ sử dụng."));
                }

                if (user.IsLdapAsync())
                {
                    return Results.Ok(new Response<List<ChuSuDungInfo>>(chuSuDung));
                }
                return Results.Ok(new Response<List<ChuSuDungInfo>>
                    (chuSuDung.Where(x => x.ChuSuDung.GiayTo == maDinhDanh || 
                                          x.ChuSuDungQuanHe?.GiayTo == maDinhDanh).ToList()));
            },
            ex =>
            {
                logger.Error(ex, "Lỗi khi lấy thông tin chủ sử dụng: {MaGcnElis} {MaDinhDanh}", 
                    maGcnElis, maDinhDanh);
                return Results.BadRequest(new Response<ChuSuDungInfo>(ex.Message));
            }));
    }
}