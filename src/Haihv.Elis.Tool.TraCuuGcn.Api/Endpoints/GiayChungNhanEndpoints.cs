using Haihv.Elis.Tool.TraCuuGcn.Api.Authenticate;
using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Services;
using Haihv.Elis.Tool.TraCuuGcn.Api.Settings;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Endpoints;

public static class GiayChungNhanEndpoints
{
    private const string UrlGetGiayChungNhan = "/elis/gcn";
    private const string UrlGetThuaDat = "/elis/thua-dat";
    private const string UrlGetThuaDatPublic = "/elis/thua-dat-public";
    /// <summary>
    /// Định nghĩa các endpoint cho Giấy Chứng Nhận.
    /// </summary>
    /// <param name="app">Ứng dụng web.</param>
    public static void MapGiayChungNhanEndpoints(this WebApplication app)
    {
        app.MapGet(UrlGetGiayChungNhan, GetGiayChungNhanAsync)
            .WithName("GetGiayChungNhan");

        app.MapGet(UrlGetThuaDat, GetThuaDatAsync)
            .WithName("GetThuaDatAsync")
            .RequireAuthorization();

        app.MapGet(UrlGetThuaDatPublic, GetThuaDatPublicAsync)
            .WithName("GetThuaDatPublicAsync");
    }

    /// <summary>
    /// Lấy thông tin Giấy Chứng Nhận theo số serial.
    /// </summary>
    /// <param name="serial">Số serial của Giấy Chứng Nhận.</param>
    /// <param name="logger">Logger để ghi log.</param>
    /// <param name="giayChungNhanService">Dịch vụ Giấy Chứng Nhận.</param>
    /// <param name="httpContext">
    /// Ngữ cảnh HTTP hiện tại.
    /// </param>
    /// <returns>Kết quả truy vấn Giấy Chứng Nhận.</returns>
    private static async Task<IResult> GetGiayChungNhanAsync([FromQuery] string? serial, ILogger logger,
        IGiayChungNhanService giayChungNhanService, HttpContext httpContext)
    {
        if (string.IsNullOrWhiteSpace(serial))
        {
            logger.Warning("Số serial không được để trống: {Url}", UrlGetGiayChungNhan);
            return Results.BadRequest("Số serial không được để trống");
        }
        serial = serial.ChuanHoa();
        var result = await giayChungNhanService.GetResultAsync(serial);
        var ipAddr = httpContext.GetIpAddress();
        return await Task.FromResult(result.Match(
            giayChungNhan =>
            {
                logger.Information("Lấy thông tin Giấy Chứng Nhận thành công: {Serial}{Url}{ClientIp}", 
                    serial, 
                    UrlGetGiayChungNhan, 
                    ipAddr);
                return Results.Ok(new Response<GiayChungNhan>(giayChungNhan));
            },
            ex =>
            {
                logger.Error(ex, "Lỗi khi lấy thông tin Giấy Chứng Nhận: {Serial}{Url}{ClientIp}",
                    serial, 
                    UrlGetGiayChungNhan, 
                    ipAddr); 
                return Results.BadRequest(ex.Message);
            }));
    }

    /// <summary>
    /// Lấy thông tin Thửa Đất theo Giấy Chứng Nhận.
    /// </summary>
    /// <param name="serial">Số serial của Giấy Chứng Nhận.</param>
    /// <param name="httpContext">Ngữ cảnh HTTP hiện tại.</param>
    /// <param name="logger">Logger để ghi log.</param>
    /// <param name="authenticationService">Dịch vụ xác thực.</param>
    /// <param name="thuaDatService">Dịch vụ Giấy Chứng Nhận.</param>
    /// <returns>Kết quả truy vấn Thửa Đất.</returns>
    private static async Task<IResult> GetThuaDatAsync([FromQuery] string? serial,
        HttpContext httpContext,
        ILogger logger,
        IAuthenticationService authenticationService,
        IThuaDatService thuaDatService)
    {
        if (string.IsNullOrWhiteSpace(serial))
        {
            logger.Warning("Số serial không được để trống: {Url}", UrlGetThuaDat);
            return Results.BadRequest("Số serial không được để trống");
        }
        serial = serial.ChuanHoa();
        // Lấy thông tin người dùng theo token từ HttpClient
        var user = httpContext.User;
        var maDinhDanh = await authenticationService.CheckAuthenticationAsync(serial, user);
        if (string.IsNullOrWhiteSpace(maDinhDanh))
        {
            logger.Warning("Người dùng không được phép truy cập thông tin Thửa Đất: {Serial}{Url}{MaDinhDanh}", 
                serial, 
                UrlGetThuaDat, 
                maDinhDanh);
            return Results.Unauthorized();
        }
        var result = await thuaDatService.GetResultAsync(serial);
        return await Task.FromResult(result.Match(
            thuaDats =>
            {
                logger.Information("Lấy thông tin Thửa Đất thành công: {Serial}{Url}{MaDinhDanh}",
                    serial, 
                    UrlGetThuaDat, 
                    maDinhDanh);
                return Results.Ok(new Response<List<ThuaDat>>(thuaDats));
            },
            ex =>
            {
                logger.Error(ex, "Lỗi khi lấy thông tin Thửa Đất: {Serial}{Url}{MaDinhDanh}", 
                    serial, 
                    UrlGetThuaDat, 
                    maDinhDanh);
                return Results.BadRequest(new Response<List<ThuaDat>>(ex.Message));
            }));
    }

    /// <summary>
    /// Lấy thông tin Thửa Đất công khai theo Giấy Chứng Nhận.
    /// </summary>
    /// <param name="serial">Số serial của Giấy Chứng Nhận.</param>
    /// <param name="logger">Logger để ghi log.</param>
    /// <param name="httpContext">
    /// Ngữ cảnh HTTP hiện tại.
    /// </param>
    /// <param name="thuaDatService">Dịch vụ Giấy Chứng Nhận.</param>
    /// <returns>Kết quả truy vấn Thửa Đất công khai.</returns>
    private static async Task<IResult> GetThuaDatPublicAsync([FromQuery] string? serial,
        ILogger logger,
        HttpContext httpContext,
        IThuaDatService thuaDatService)
    {
        if (string.IsNullOrWhiteSpace(serial))
        {
            logger.Warning("Số serial không được để trống: {Url}", UrlGetThuaDatPublic);
            return Results.BadRequest("Số serial không được để trống");
        }
        serial = serial.ChuanHoa();
        var result = await thuaDatService.GetResultAsync(serial);
        var ipAddr = httpContext.GetIpAddress();
        return await Task.FromResult(result.Match(
            thuaDats =>
            {
                logger.Information("Lấy thông tin Thửa Đất công khai thành công: {Serial}{Url}{ClientIp}", 
                    serial, 
                    UrlGetThuaDatPublic, 
                    ipAddr);
                var thuaDatPublic = thuaDats.Select(x => x.ConvertToThuaDatPublic());
                return Results.Ok(new Response<IEnumerable<ThuaDatPublic>>(thuaDatPublic));
            },
            ex =>
            {
                logger.Error(ex, "Lỗi khi lấy thông tin Thửa Đất công khai: {Serial}{Url}{ClientIp}",  
                    serial,
                    UrlGetThuaDatPublic, 
                    ipAddr);
                return Results.BadRequest(new Response<IEnumerable<ThuaDatPublic>>(ex.Message));
            }));
    }
}