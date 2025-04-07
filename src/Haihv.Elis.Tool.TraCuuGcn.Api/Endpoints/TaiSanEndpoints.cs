using Haihv.Elis.Tool.TraCuuGcn.Api.Authenticate;
using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Services;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using Microsoft.AspNetCore.Http.Extensions;
using ILogger = Serilog.ILogger;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Endpoints;

public static class TaiSanEndpoints
{
    /// <summary>
    /// Định nghĩa endpoint để lấy thông tin tài sản.
    /// </summary>
    /// <param name="app">Ứng dụng web.</param>
    public static void MapTaiSanEndpoints(this WebApplication app)
    {
        // Định nghĩa endpoint để lấy thông tin tài sản
        app.MapGet("/elis/taisan", GetTaiSanAsync)
            .WithName("GetTaiSanAsync")
            .RequireAuthorization();
    }

    /// <summary>
    /// Phương thức xử lý yêu cầu GET thông tin tài sản.
    /// </summary>
    /// <param name="httpContext">Thông tin về HTTP context của yêu cầu.</param>
    /// <param name="logger">Logger để ghi nhật ký.</param>
    /// <param name="authenticationService">Dịch vụ xác thực.</param>
    /// <param name="thuaDatService">Dịch vụ thửa đất.</param>
    /// <param name="chuSuDungService">Dịch vụ chủ sử dụng.</param>
    /// <param name="taiSanService">Dịch vụ tài sản.</param>
    /// <returns>Kết quả trả về cho client.</returns>
    private static async Task<IResult> GetTaiSanAsync(
        HttpContext httpContext,
        ILogger logger,
        IAuthenticationService authenticationService,
        IThuaDatService thuaDatService,
        IChuSuDungService chuSuDungService,
        ITaiSanService taiSanService)
    {
        // Lấy thông tin người dùng theo token từ HttpClient
        var user = httpContext.User;
        var url = httpContext.Request.GetDisplayUrl();
        var serial = httpContext.Request.Query["serial"].ToString();
        var maDinhDanh = await authenticationService.CheckAuthenticationAsync(serial, user);
        var (ipAddress, _) = httpContext.GetIpInfo();

        if (string.IsNullOrWhiteSpace(maDinhDanh))
        {
            logger.Warning("Người dùng không được phép truy cập thông tin Tài sản: {Url}{MaDinhDanh}{ClientIp}",
                url,
                maDinhDanh,
                ipAddress);
            return Results.Unauthorized();
        }
        
        // Lấy danh sách mã thửa đất
        var dsMaThuaDatTask = thuaDatService.GetMaThuaDatAsync(serial);
        var dsMaChuSuDungTask = chuSuDungService.GetMaChuSuDungAsync(serial);
        var dsMaThuaDat = await dsMaThuaDatTask;
        if (dsMaThuaDat.Count == 0)
        {
            logger.Warning("Không tìm thấy thông tin thửa đất: {Url}{MaDinhDanh}{ClientIp}",
                url,
                maDinhDanh,
                ipAddress);
            return Results.NotFound(new Response<List<TaiSan>>("Không tìm thấy thông tin thửa đất."));
        }
        var dsMaChuSuDung = await dsMaChuSuDungTask;
        if (dsMaChuSuDung.Count == 0)
        {
            logger.Warning("Không tìm thấy thông tin chủ sử dụng: {Url}{MaDinhDanh}{ClientIp}",
                url,
                maDinhDanh,
                ipAddress);
            return Results.NotFound(new Response<List<TaiSan>>("Không tìm thấy thông tin chủ sử dụng."));
        }
        
        var result = await taiSanService.GetTaiSanAsync(serial, dsMaThuaDat, dsMaChuSuDung);

        return await Task.FromResult(result.Match(
            taiSan =>
            {
                logger.Information("Lấy thông tin tài sản thành công: {Url}{MaDinhDanh}{ClientIp}",
                    url,
                    maDinhDanh,
                    ipAddress);
                if (taiSan.Count != 0) return Results.Ok(new Response<List<TaiSan>>(taiSan));
                logger.Warning("Không tìm thấy thông tin tài sản: {Url}{MaDinhDanh}{ClientIp}",
                    url,
                    maDinhDanh,
                    ipAddress);
                return Results.NotFound(new Response<List<TaiSan>>("Không tìm thấy thông tin tài sản."));

            },
            ex =>
            {
                logger.Error(ex, "Lỗi khi lấy thông tin tài sản: {Url}{MaDinhDanh}{ClientIp}",
                    url,
                    maDinhDanh,
                    ipAddress);
                return Results.BadRequest(new Response<List<TaiSan>>(ex.Message));
            }));
    }
}