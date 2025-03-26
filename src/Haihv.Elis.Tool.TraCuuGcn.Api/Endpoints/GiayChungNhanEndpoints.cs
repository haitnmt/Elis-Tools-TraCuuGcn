using Haihv.Elis.Tool.TraCuuGcn.Api.Authenticate;
using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Services;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using Microsoft.AspNetCore.Http.Extensions;
using ZiggyCreatures.Caching.Fusion;
using ILogger = Serilog.ILogger;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Endpoints;

public static class GiayChungNhanEndpoints
{
    /// <summary>
    /// Định nghĩa các endpoint cho Giấy Chứng Nhận.
    /// </summary>
    /// <param name="app">Ứng dụng web.</param>
    public static void MapGiayChungNhanEndpoints(this WebApplication app)
    {
        var mapGroup = app.MapGroup("/elis");
        mapGroup.MapGet("/gcn", GetGiayChungNhanAsync)
            .WithName("GetGiayChungNhan");

        mapGroup.MapGet("/thua-dat", GetThuaDatAsync)
            .WithName("GetThuaDatAsync")
            .RequireAuthorization();

        mapGroup.MapGet("/thua-dat-public", GetThuaDatPublicAsync)
            .WithName("GetThuaDatPublicAsync");

        mapGroup.MapDelete("/delete-cache", DeleteCache)
            .WithName("DeleteCache")
            .RequireAuthorization();
        mapGroup.MapGet("/permissions/can-update", GetHasUpdatePermission)
            .WithName("HasUpdatePermission")
            .RequireAuthorization();
        mapGroup.MapDelete("/delete-qr", DeleteMaQr)
            .WithName("DeleteMaQr")
            .RequireAuthorization();
    }

    private static async Task<IResult> DeleteMaQr(HttpContext context,
        ILogger logger, IFusionCache fusionCache, IConfiguration configuration,
        IConnectionElisData connectionElisData, ILogElisDataServices logElisDataServices,
        IGiayChungNhanService giayChungNhanService,
        IGcnQrService gcnQrService)
    {
        var serial = context.Request.Query["serial"].ToString();
        var hasUpdatePermission = await HasUpdatePermission(context, logger, configuration, connectionElisData,
            giayChungNhanService);
        if (hasUpdatePermission.StatusCodes != StatusCodes.Status200OK)
        {
            return hasUpdatePermission.StatusCodes switch
            {
                StatusCodes.Status404NotFound => Results.NotFound(hasUpdatePermission.Message),
                StatusCodes.Status401Unauthorized => Results.Unauthorized(),
                _ => Results.BadRequest(hasUpdatePermission.Message)
            };
        }
        var maDinhDanh = context.User.GetMaDinhDanh();
        var result = await gcnQrService.DeleteMaQrAsync(serial);
        var url = context.Request.GetDisplayUrl();
        return result.Match(
            succ =>
            {
                if (succ)
                {
                    logger.Information("Xóa mã QR thành công: {Url}{MaDinhDanh}",
                        url,
                        maDinhDanh);
                    // Ghi log vào ELIS Data
                    logElisDataServices.WriteLogToElisDataAsync(serial, maDinhDanh, url,
                        $"Xóa mã QR của Giấy chứng nhận có phát hành (Serial): {serial}", LogElisDataServices.LoaiTacVu.Xoa);

                    return Results.Ok("Xóa mã QR thành công!");
                }

                logger.Information("Xóa mã QR không thành công: {Url}{MaDinhDanh}",
                    url,
                    maDinhDanh);
                return Results.BadRequest("Lỗi khi xóa mã QR: [Không xác định]");
            },
            ex =>
            {
                logger.Error(ex, "Lỗi khi xóa mã QR: {Url}{MaDinhDanh}",
                    url,
                    maDinhDanh);
                return Results.BadRequest($"Lỗi khi xóa mã QR: [{ex.Message}]");
            });
    }

    private static async Task<IResult> GetHasUpdatePermission(HttpContext context,
        ILogger logger, IFusionCache fusionCache, IConfiguration configuration,
        IConnectionElisData connectionElisData, IGiayChungNhanService giayChungNhanService)
    {
        var result = await HasUpdatePermission(context,
            logger,
            configuration,
            connectionElisData,
            giayChungNhanService);
        return result.StatusCodes switch
        {
            StatusCodes.Status404NotFound => Results.NotFound(result.Message),
            StatusCodes.Status401Unauthorized => Results.Unauthorized(),
            StatusCodes.Status200OK => Results.Ok(true),
            _ => Results.BadRequest(result.Message)
        };
    }

    private static async Task<(int StatusCodes, string Message)> HasUpdatePermission(HttpContext context,
        ILogger logger,
        IConfiguration configuration,
        IConnectionElisData connectionElisData,
        IGiayChungNhanService giayChungNhanService)
    {
        // Lấy thông tin số MaDinhDanh từ HttpContext
        var maDinhDanh = context.User.GetMaDinhDanh();
        // Lấy thông tin URL hiện tại
        var url = context.Request.GetDisplayUrl();
        var serial = context.Request.Query["serial"].ToString();
        if (string.IsNullOrWhiteSpace(serial))
        {
            logger.Warning("Thiếu thông tin số Serial của Giấy chứng nhận: {Url}{MaDinhDanh}", url, maDinhDanh);
            return (StatusCodes.Status404NotFound, "Thiếu thông tin số Serial của Giấy chứng nhận!");
        }

        var user = context.User;
        if (!user.IsLdap())
        {
            logger.Warning("Không có quyền cập nhật Giấy chứng nhận: {Url}{MaDinhDanh}", url, maDinhDanh);
            return (StatusCodes.Status401Unauthorized, "Không có quyền cập nhật Giấy chứng nhận!");
        }

        var urlLdapApi = configuration["AuthEndpoint"];

        if (string.IsNullOrWhiteSpace(urlLdapApi))
        {
            logger.Warning("Thiếu thông tin cấu hình AuthEndpoint: {Url}{MaDinhDanh}", url, maDinhDanh);
            return (StatusCodes.Status401Unauthorized, "Không có quyền cập nhật Giấy chứng nhận!");
        }

        var groupName = await connectionElisData.GetUpdateGroupName(serial);

        if (string.IsNullOrWhiteSpace(groupName) || !await context.HasUpdatePermission(urlLdapApi, groupName))
        {
            logger.Warning("Không có quyền cập nhật Giấy chứng nhận: {Url}{MaDinhDanh}", url, maDinhDanh);
            return (StatusCodes.Status401Unauthorized, "Không có quyền cập nhật Giấy chứng nhận!");
        }

        var giayChungNhan = await giayChungNhanService.GetAsync(serial);

        if (giayChungNhan is null || giayChungNhan.NgayKy <= new DateTime(1990, 1, 1))
            return (StatusCodes.Status200OK, string.Empty);

        logger.Warning("Giấy chứng nhận đã ký không được xoá: {Url}{MaDinhDanh}", url, maDinhDanh);
        return (StatusCodes.Status400BadRequest, "Giấy chứng nhận đã ký không được xoá!");
    }

    /// <summary>
    /// Lấy thông tin Giấy Chứng Nhận theo số serial.
    /// </summary>
    /// <param name="logger">Logger để ghi log.</param>
    /// <param name="giayChungNhanService">Dịch vụ Giấy Chứng Nhận.</param>
    /// <param name="httpContext">
    /// Ngữ cảnh HTTP hiện tại.
    /// </param>
    /// <returns>Kết quả truy vấn Giấy Chứng Nhận.</returns>
    private static async Task<IResult> GetGiayChungNhanAsync(
        ILogger logger,
        IGiayChungNhanService giayChungNhanService,
        HttpContext httpContext)
    {
        var url = httpContext.Request.GetDisplayUrl();
        var serial = httpContext.Request.Query["serial"].ToString();
        if (string.IsNullOrWhiteSpace(serial))
        {
            logger.Warning("Số serial không được để trống: {Url}", url);
            return Results.BadRequest("Số serial không được để trống!");
        }

        var result = await giayChungNhanService.GetResultAsync(serial);
        var ipAddr = httpContext.GetIpAddress();
        return result.Match(
            giayChungNhan =>
            {
                logger.Information("Lấy thông tin Giấy Chứng Nhận thành công: {Url}{ClientIp}",
                    url,
                    ipAddr);
                return Results.Ok(new Response<GiayChungNhan>(giayChungNhan));
            },
            ex =>
            {
                logger.Error(ex, "Lỗi khi lấy thông tin Giấy Chứng Nhận: {Url}{ClientIp}",
                    url,
                    ipAddr);
                return Results.BadRequest(ex.Message);
            });
    }

    /// <summary>
    /// Lấy thông tin Thửa Đất theo Giấy Chứng Nhận.
    /// </summary>
    /// <param name="httpContext">Ngữ cảnh HTTP hiện tại.</param>
    /// <param name="logger">Logger để ghi log.</param>
    /// <param name="authenticationService">Dịch vụ xác thực.</param>
    /// <param name="thuaDatService">Dịch vụ Giấy Chứng Nhận.</param>
    /// <returns>Kết quả truy vấn Thửa Đất.</returns>
    private static async Task<IResult> GetThuaDatAsync(
        HttpContext httpContext,
        ILogger logger,
        IAuthenticationService authenticationService,
        IThuaDatService thuaDatService)
    {
        var url = httpContext.Request.GetDisplayUrl();
        var serial = httpContext.Request.Query["serial"].ToString();
        if (string.IsNullOrWhiteSpace(serial))
        {
            logger.Warning("Số serial không được để trống: {Url}", url);
            return Results.BadRequest("Số serial không được để trống!");
        }

        // Lấy thông tin người dùng theo token từ HttpClient
        var user = httpContext.User;
        var maDinhDanh = await authenticationService.CheckAuthenticationAsync(serial, user);
        if (string.IsNullOrWhiteSpace(maDinhDanh))
        {
            logger.Warning("Người dùng không được phép truy cập thông tin Thửa Đất: {Url}{MaDinhDanh}",
                url,
                maDinhDanh);
            return Results.Unauthorized();
        }

        var result = await thuaDatService.GetResultAsync(serial);
        return result.Match(
            thuaDats =>
            {
                logger.Information("Lấy thông tin Thửa Đất thành công: {Url}{MaDinhDanh}",
                    url,
                    maDinhDanh);
                return Results.Ok(new Response<List<ThuaDat>>(thuaDats));
            },
            ex =>
            {
                logger.Error(ex, "Lỗi khi lấy thông tin Thửa Đất: {Url}{MaDinhDanh}",
                    url,
                    maDinhDanh);
                return Results.BadRequest(new Response<List<ThuaDat>>(ex.Message));
            });
    }

    /// <summary>
    /// Lấy thông tin Thửa Đất công khai theo Giấy Chứng Nhận.
    /// </summary>
    /// <param name="logger">Logger để ghi log.</param>
    /// <param name="httpContext">
    /// Ngữ cảnh HTTP hiện tại.
    /// </param>
    /// <param name="thuaDatService">Dịch vụ Giấy Chứng Nhận.</param>
    /// <returns>Kết quả truy vấn Thửa Đất công khai.</returns>
    private static async Task<IResult> GetThuaDatPublicAsync(
        ILogger logger,
        HttpContext httpContext,
        IThuaDatService thuaDatService)
    {
        var url = httpContext.Request.GetDisplayUrl();
        var serial = httpContext.Request.Query["serial"].ToString();
        if (string.IsNullOrWhiteSpace(serial))
        {
            logger.Warning("Số serial không được để trống: {Url}", url);
            return Results.BadRequest("Số serial không được để trống");
        }

        var result = await thuaDatService.GetResultAsync(serial);
        var ipAddr = httpContext.GetIpAddress();
        return result.Match(
            thuaDats =>
            {
                logger.Information("Lấy thông tin Thửa Đất công khai thành công: {Url}{ClientIp}",
                    url,
                    ipAddr);
                var thuaDatPublic = thuaDats.Select(x => x.ConvertToThuaDatPublic());
                return Results.Ok(new Response<IEnumerable<ThuaDatPublic>>(thuaDatPublic));
            },
            ex =>
            {
                logger.Error(ex, "Lỗi khi lấy thông tin Thửa Đất công khai: {Url}{ClientIp}",
                    url,
                    ipAddr);
                return Results.BadRequest(new Response<IEnumerable<ThuaDatPublic>>(ex.Message));
            });
    }

    private static async Task<IResult> DeleteCache(HttpContext context,
        ILogger logger,
        IFusionCache fusionCache,
        IConfiguration configuration,
        IConnectionElisData connectionElisData,
        IGiayChungNhanService giayChungNhanService)
    {
        var url = context.Request.GetDisplayUrl();
        var serial = context.Request.Query["serial"].ToString();
        serial = serial.ChuanHoa();
        if (string.IsNullOrWhiteSpace(serial))
        {
            logger.Warning("Thiếu thông tin số Serial của Giấy chứng nhận: {Url}", url);
            return Results.BadRequest("Thiếu thông tin số Serial của Giấy chứng nhận!");
        }

        var maDinhDanh = context.User.GetMaDinhDanh();
        if (string.IsNullOrWhiteSpace(maDinhDanh) || !context.User.IsLdap())
        {
            logger.Warning("Người dùng không được phép xóa cache: {Url}{MaDinhDanh}",
                url, maDinhDanh);
            return Results.Unauthorized();
        }
        try
        {
            await fusionCache.RemoveByTagAsync(serial);
            logger.Information("Xóa cache thành công: {Url}{MaDinhDanh}",
                url, maDinhDanh);
            return Results.Ok("Xóa cache thành công");
        }
        catch (Exception e)
        {
            logger.Error(e, "Lỗi khi xóa cache: {Url}{MaDinhDanh}",
                url, maDinhDanh);
            return Results.BadRequest("Lỗi khi xóa cache");
        }
    }
}