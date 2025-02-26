using Haihv.Elis.Tool.TraCuuGcn.Api.Authenticate;
using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Services;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Endpoints;

public static class ChuSuDungEndpoints
{
    private const string UrlGetChuSuDung = "/elis/chuSuDung";
    /// <summary>
    /// Định nghĩa endpoint để lấy thông tin chủ sử dụng.
    /// </summary>
    /// <param name="app">Ứng dụng web.</param>
    public static void MapChuSuDungEndpoints(this WebApplication app)
    {
        app.MapGet(UrlGetChuSuDung, GetChuSuDung)
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
        var ipAddr = httpContext.GetIpAddress();
        if (string.IsNullOrWhiteSpace(maDinhDanh))
        {
            logger.Warning("Người dùng không được phép truy cập thông tin Chủ sử dụng: {MaDinhDanh}{Url}{ClientIp}", 
                UrlGetChuSuDung, 
                ipAddr, 
                maDinhDanh);
            return Results.Unauthorized();
        }
        var result = await chuSuDungService.GetResultAsync(maGcnElis);
        return await Task.FromResult(result.Match(
            chuSuDung =>
            {
                logger.Information("Lấy thông tin chủ sử dụng thành công: {MaDinhDanh}{Url}{ClientIp}",
                    maDinhDanh, 
                    UrlGetChuSuDung, 
                    ipAddr);
                if (chuSuDung.Count == 0)
                {
                    logger.Warning("Không tìm thấy thông tin chủ sử dụng: {MaDinhDanh}{Url}{ClientIp}", 
                        maDinhDanh, 
                        UrlGetChuSuDung, 
                        ipAddr);
                    return Results.NotFound(new Response<List<ChuSuDungInfo>>("Không tìm thấy thông tin chủ sử dụng."));
                }

                if (user.IsLdapAsync())
                {
                    return Results.Ok(new Response<List<ChuSuDungInfo>>(chuSuDung));
                }
                return Results.Ok(new Response<List<ChuSuDungInfo>>
                    (chuSuDung.Where(x => (x.ChuSuDung.GiayTo?.EndsWith(maDinhDanh) ?? false) || 
                                          (x.ChuSuDungQuanHe?.GiayTo?.EndsWith(maDinhDanh) ?? false)).ToList()));
            },
            ex =>
            {
                logger.Error(ex, "Lỗi khi lấy thông tin chủ sử dụng: {MaDinhDanh}{Url}{ClientIp}", 
                    maDinhDanh, 
                    UrlGetChuSuDung, 
                    ipAddr);
                return Results.BadRequest(new Response<ChuSuDungInfo>(ex.Message));
            }));
    }
}