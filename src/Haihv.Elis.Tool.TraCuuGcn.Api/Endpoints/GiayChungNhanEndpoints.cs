using Haihv.Elis.Tool.TraCuuGcn.Api.Authenticate;
using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Services;
using Haihv.Elis.Tool.TraCuuGcn.Api.Settings;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;
using ZiggyCreatures.Caching.Fusion;
using ILogger = Serilog.ILogger;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Endpoints;

public static class GiayChungNhanEndpoints
{
    private const string UrlGroup = "/elis";
    private const string UrlGetGiayChungNhan = $"{UrlGroup}/gcn";
    private const string UrlGetThuaDat = $"{UrlGroup}/thua-dat";
    private const string UrlGetThuaDatPublic = $"{UrlGroup}/thua-dat-public";
    private const string UrlClearCache = $"{UrlGroup}/delete-cache";
    private const string UrlPermissionsCanUpdate= $"{UrlGroup}/permissions/can-update";
    private const string UrlDeleteMaQr = $"{UrlGroup}/delete-qr";
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
        
        app.MapDelete(UrlClearCache, DeleteCache)
            .WithName("DeleteCache")
            .RequireAuthorization();
        app.MapGet(UrlPermissionsCanUpdate, GetHasUpdatePermission)
            .WithName("HasUpdatePermission")
            .RequireAuthorization();
        app.MapDelete(UrlDeleteMaQr, DeleteMaQr)
            .WithName("DeleteMaQr")
            .RequireAuthorization();
    }

    private static async Task<IResult> DeleteMaQr(HttpContext context,
        ILogger logger, IFusionCache fusionCache, IConfiguration configuration, 
        IConnectionElisData connectionElisData, IGiayChungNhanService giayChungNhanService,
        IGcnQrService gcnQrService,
        [FromQuery] string? serial = null)
    {
        var urlLdapApi = configuration["AuthEndpoint"];
        
        if (string.IsNullOrWhiteSpace(urlLdapApi))
        {
            logger.Warning("Thiếu thông tin cấu hình AuthEndpoint: {Url}{Serial}", UrlPermissionsCanUpdate, serial);
            return Results.BadRequest("Thiếu thông tin cấu hình xác thực người dùng");
        }
        var resultGiayChungNhan = await HasUpdateGiayChungNhan(logger, giayChungNhanService, serial);
        return await resultGiayChungNhan.Match<Task<IResult>>(async _ =>
        {
            var maDinhDanh = context.User.GetMaDinhDanh();
            if (string.IsNullOrWhiteSpace(maDinhDanh))
            {
                logger.Warning("Không tìm thấy thông tin mã định danh của người dùng: {Url}{Serial}", UrlPermissionsCanUpdate, serial);
                return Results.Unauthorized();
            }
            var user = context.User;
            if (!user.IsLdap())
            {
                logger.Warning("Không có quyền cập nhật GCN: {Url}{Serial}", UrlPermissionsCanUpdate, serial);
                return Results.Unauthorized();
            }

            var groupName = await connectionElisData.GetUpdateGroupName(serial!);

            if (string.IsNullOrWhiteSpace(groupName))
            {
                logger.Warning("Không tìm thấy thông tin nhóm có quyền cập nhật GCN: {Url}{Serial}",  
                    UrlPermissionsCanUpdate, serial);
                return Results.Unauthorized();
            }

            if (!await context.HasUpdatePermission(urlLdapApi, groupName))
            {
                logger.Warning("Không có quyền cập nhật GCN: {Url}{Serial}", UrlPermissionsCanUpdate, serial);
                return Results.Unauthorized();
            }
            var result = await gcnQrService.DeleteMaQrAsync(serial);
            return result.Match(
                _ =>
                {
                    logger.Information("Xóa mã QR thành công: {Url}{MaDinhDanh}{Serial}",
                        UrlDeleteMaQr,
                        maDinhDanh,
                        serial);
                    return Results.Ok();
                },
                ex =>
                {
                    logger.Error(ex, "Xóa mã QR thất bại: {Url}{MaDinhDanh}{Serial}",
                        UrlDeleteMaQr,
                        maDinhDanh,
                        serial);
                    return Results.BadRequest(ex.Message);
                });
        }, ex =>
        {
            logger.Warning(ex, ex.Message + "{Url}{Serial}", UrlPermissionsCanUpdate, serial);
            return Task.FromResult(Results.NotFound(ex.Message));
        });
        

    }

    private static async Task<IResult> GetHasUpdatePermission(HttpContext context,
        ILogger logger, IFusionCache fusionCache, IConfiguration configuration,
        IConnectionElisData connectionElisData, IGiayChungNhanService giayChungNhanService,
        [FromQuery] string? serial = null)
    {
        var urlLdapApi = configuration["AuthEndpoint"];

        if (string.IsNullOrWhiteSpace(urlLdapApi))
        {
            logger.Warning("Thiếu thông tin cấu hình AuthEndpoint: {Url}{Serial}", UrlPermissionsCanUpdate, serial);
            return Results.BadRequest("Thiếu thông tin cấu hình xác thực người dùng");
        }

        var resultGiayChungNhan = await HasUpdateGiayChungNhan(logger, giayChungNhanService, serial);
        return await resultGiayChungNhan.Match<Task<IResult>>(async _ =>
        {
            var user = context.User;
            if (!user.IsLdap())
            {
                logger.Warning("Không có quyền cập nhật GCN: {Url}{Serial}", UrlPermissionsCanUpdate, serial);
                return Results.Unauthorized();
            }

            var groupName = await connectionElisData.GetUpdateGroupName(serial!);

            if (!string.IsNullOrWhiteSpace(groupName))
                return await context.HasUpdatePermission(urlLdapApi, groupName)
                    ? Results.Ok(true)
                    : Results.Unauthorized();

            logger.Warning("Không tìm thấy thông tin nhóm có quyền cập nhật GCN: {Url}{Serial}",
                UrlPermissionsCanUpdate, serial);
            return Results.Unauthorized();
        }, ex =>
        {
            logger.Warning(ex, ex.Message + "{Url}{Serial}", UrlPermissionsCanUpdate, serial);
            return Task.FromResult(Results.NotFound(ex.Message));
        });
    }

    private static async Task<Result<bool>> HasUpdateGiayChungNhan(ILogger logger, IGiayChungNhanService giayChungNhanService,
        string? serial)
    {
        if (string.IsNullOrWhiteSpace(serial))
        {
            logger.Warning("Thiếu thông tin số Serial của GCN: {Url}", UrlPermissionsCanUpdate);
            return new Result<bool>(new ArgumentException("Thiếu thông tin số Serial của GCN"));
        }

        var giayChungNhan = await giayChungNhanService.GetAsync(serial);
        if (giayChungNhan is null)
        {
            logger.Warning("Không tìm thấy thông tin GCN: {Url}{Serial}", UrlPermissionsCanUpdate, serial);
            return new Result<bool>(new ArgumentException("Không tìm thấy thông tin GCN"));
        }

        if (giayChungNhan.NgayKy <= new DateTime(1990, 1, 1) &&
            string.IsNullOrWhiteSpace(giayChungNhan.SoVaoSo)) return true;
        
        logger.Warning("GCN đã vào sổ không được xoá: {Url}{Serial}", UrlPermissionsCanUpdate, serial);
        return new Result<bool>(new ArgumentException("GCN đã vào sổ không được xoá"));
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
        return result.Match(
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
            });
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
        return result.Match(
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
            });
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
        return result.Match(
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
            });
    }
    private static async Task<IResult> DeleteCache(HttpContext context,
        ILogger logger,
        IFusionCache fusionCache,
        IConfiguration configuration,
        [FromQuery] string? serial)
    {
        if (string.IsNullOrWhiteSpace(serial))
        {
            logger.Warning("Thiếu thông tin số Serial của GCN: {Url}", UrlClearCache);
            return Results.BadRequest("Thiếu thông tin số Serial của GCN");
        }
        var maDinhDanh = context.User.GetMaDinhDanh();
        try
        {
            await fusionCache.RemoveByTagAsync(serial.ChuanHoa());
            logger.Information("Xóa cache thành công: {Url}{Serial}{MaDinhDanh}",
                UrlClearCache,serial, maDinhDanh);
            return Results.Ok("Xóa cache thành công");
        }
        catch (Exception e)
        {
            logger.Error(e, "Lỗi khi xóa cache: {Url}{Serial}{MaDinhDanh}",
                UrlClearCache, serial, maDinhDanh);
            return Results.BadRequest("Lỗi khi xóa cache");
        }
    }
}