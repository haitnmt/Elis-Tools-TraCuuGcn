using Haihv.Elis.Tool.TraCuuGcn.Api.Authenticate;
using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Services;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Caching.Hybrid;
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
        mapGroup.MapPost("/update-gcn", UpdateGiayChungNhan)
            .WithName("UpdateGiayChungNhan")
            .RequireAuthorization();
    }

    private static async Task<IResult> UpdateGiayChungNhan(HttpContext context,
        ILogger logger, HybridCache hybridCache,
        IConfiguration configuration, IConnectionElisData connectionElisData,
        IHttpClientFactory httpClientFactory,
        ILogElisDataServices logElisDataServices,
        IGiayChungNhanService giayChungNhanService)
    {
        try
        {
            // Đọc và kiểm tra dữ liệu đầu vào
            var phapLyGiayChungNhan = await context.Request.ReadFromJsonAsync<PhapLyGiayChungNhan>();
            if (phapLyGiayChungNhan == null || string.IsNullOrWhiteSpace(phapLyGiayChungNhan.Serial))
            {
                return Results.BadRequest("Dữ liệu không hợp lệ hoặc thiếu số Serial!");
            }

            // Kiểm tra quyền cập nhật
            var hasUpdatePermission = await HasUpdatePermission(context, logger, configuration, connectionElisData,
                giayChungNhanService, phapLyGiayChungNhan.Serial);
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
            var url = context.Request.GetDisplayUrl();

            // Thực hiện cập nhật
            var result = await giayChungNhanService.UpdateAsync(phapLyGiayChungNhan);

            // Xử lý kết quả
            return result.Match(
                succ =>
                {
                    if (!succ) return Results.BadRequest("Lỗi khi cập nhật Giấy chứng nhận: [Không xác định]");

                    // Ghi log thành công
                    logger.Information("Cập nhật Giấy chứng nhận thành công: {Url}{MaDinhDanh}",
                        url,
                        maDinhDanh);

                    // Tạo thông điệp log
                    var message = $"""
                                   Cập nhật thông tin Giấy chứng nhận: Serial {phapLyGiayChungNhan.Serial} |
                                   Ngày ký: {phapLyGiayChungNhan.NgayKy:dd/MM/yyyy} |
                                   Người ký: {phapLyGiayChungNhan.NguoiKy} |
                                   Số vào sổ: {phapLyGiayChungNhan.SoVaoSo}
                                   """;
                    // Loại bỏ khoảng trắng và xuống dòng
                    message = message.Replace("\n", string.Empty).Replace("\r", string.Empty);

                    // Ghi log vào ELIS Data
                    logElisDataServices.WriteLogToElisDataAsync(phapLyGiayChungNhan.Serial, maDinhDanh, url, message);

                    // Xóa cache liên quan
                    _ = hybridCache.RemoveByTagAsync(phapLyGiayChungNhan.Serial).AsTask();

                    return Results.Ok("Cập nhật Giấy chứng nhận thành công!");
                },
                ex =>
                {
                    logger.Error(ex, "Lỗi khi cập nhật Giấy chứng nhận: {Url}{MaDinhDanh}",
                        url,
                        maDinhDanh);
                    return Results.BadRequest($"Lỗi khi cập nhật Giấy chứng nhận: [{ex.Message}]");
                });
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Lỗi không xác định khi cập nhật Giấy chứng nhận");
            return Results.BadRequest($"Lỗi không xác định: [{ex.Message}]");
        }
    }

    private static async Task<IResult> DeleteMaQr(HttpContext context,
        ILogger logger, IConfiguration configuration,
        IConnectionElisData connectionElisData, ILogElisDataServices logElisDataServices,
        IGiayChungNhanService giayChungNhanService,
        IGcnQrService gcnQrService)
    {
        // Lấy và chuẩn hóa serial
        var serial = context.Request.Query["serial"].ToString().Trim();
        if (string.IsNullOrWhiteSpace(serial))
        {
            logger.Warning("Thiếu thông tin số Serial của Giấy chứng nhận");
            return Results.BadRequest("Thiếu thông tin số Serial của Giấy chứng nhận!");
        }

        // Kiểm tra quyền cập nhật
        var hasUpdatePermission = await HasUpdatePermission(context, logger, configuration, connectionElisData,
            giayChungNhanService, serial);
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
        var url = context.Request.GetDisplayUrl();

        // Thực hiện xóa mã QR
        var result = await gcnQrService.DeleteMaQrAsync(serial);
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
                        $"Xóa mã QR của Giấy chứng nhận có phát hành (Serial): {serial}",
                        LogElisDataServices.LoaiTacVu.Xoa);

                    return Results.Ok($"Xóa mã QR thành công cho Giấy chứng nhận có Serial: {serial}");
                }

                logger.Warning("Xóa mã QR không thành công: {Url}{MaDinhDanh}",
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
        ILogger logger, IConfiguration configuration,
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
        IGiayChungNhanService giayChungNhanService, string? serial = null)
    {
        // Lấy thông tin số MaDinhDanh từ HttpContext
        var maDinhDanh = context.User.GetMaDinhDanh();
        // Lấy thông tin URL hiện tại
        var url = context.Request.GetDisplayUrl();

        // Kiểm tra và chuẩn hóa serial
        serial ??= context.Request.Query["serial"].ToString();
        serial = serial.Trim();

        if (string.IsNullOrWhiteSpace(serial))
        {
            logger.Warning("Thiếu thông tin số Serial của Giấy chứng nhận: {Url}{MaDinhDanh}", url, maDinhDanh);
            return (StatusCodes.Status404NotFound, "Thiếu thông tin số Serial của Giấy chứng nhận!");
        }

        // Kiểm tra xác thực LDAP
        var user = context.User;
        if (!user.IsLdap())
        {
            logger.Warning("Không có quyền cập nhật Giấy chứng nhận: {Url}{MaDinhDanh}", url, maDinhDanh);
            return (StatusCodes.Status401Unauthorized, "Không có quyền cập nhật Giấy chứng nhận!");
        }

        // Kiểm tra cấu hình AuthEndpoint
        var urlLdapApi = configuration["AuthEndpoint"];
        if (string.IsNullOrWhiteSpace(urlLdapApi))
        {
            logger.Warning("Thiếu thông tin cấu hình AuthEndpoint: {Url}{MaDinhDanh}", url, maDinhDanh);
            return (StatusCodes.Status401Unauthorized, "Không có quyền cập nhật Giấy chứng nhận!");
        }

        // Kiểm tra quyền theo nhóm
        var groupName = await connectionElisData.GetUpdateGroupName(serial);
        if (string.IsNullOrWhiteSpace(groupName) || !await context.HasUpdatePermission(urlLdapApi, groupName))
        {
            logger.Warning("Không có quyền cập nhật Giấy chứng nhận: {Url}{MaDinhDanh}", url, maDinhDanh);
            return (StatusCodes.Status401Unauthorized, "Không có quyền cập nhật Giấy chứng nhận!");
        }

        // Kiểm tra trạng thái ký của Giấy chứng nhận
        var giayChungNhan = await giayChungNhanService.GetAsync(serial);
        if (giayChungNhan is null || giayChungNhan.NgayKy <= new DateTime(1990, 1, 1))
            return (StatusCodes.Status200OK, string.Empty);

        logger.Warning("Giấy chứng nhận đã ký: {Url}{MaDinhDanh}", url, maDinhDanh);
        return (StatusCodes.Status400BadRequest, "Giấy chứng nhận đã ký!");
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
        try
        {
            // Lấy và kiểm tra serial
            var url = httpContext.Request.GetDisplayUrl();
            var serial = httpContext.Request.Query["serial"].ToString().Trim();
            if (string.IsNullOrWhiteSpace(serial))
            {
                logger.Warning("Số serial không được để trống: {Url}", url);
                return Results.BadRequest("Số serial không được để trống!");
            }

            // Lấy thông tin Giấy Chứng Nhận
            var result = await giayChungNhanService.GetResultAsync(serial);
            var ipAddr = httpContext.GetIpAddress();

            // Xử lý kết quả
            return result.Match(
                giayChungNhan =>
                {
                    logger.Information("Lấy thông tin Giấy Chứng Nhận thành công: {Url}{ClientIp}{Serial}",
                        url,
                        ipAddr,
                        serial);
                    return Results.Ok(new Response<GiayChungNhan>(giayChungNhan));
                },
                ex =>
                {
                    logger.Error(ex, "Lỗi khi lấy thông tin Giấy Chứng Nhận: {Url}{ClientIp}{Serial}",
                        url,
                        ipAddr,
                        serial);
                    return Results.BadRequest(new Response<GiayChungNhan>(ex.Message));
                });
        }
        catch (Exception ex)
        {
            var url = httpContext.Request.GetDisplayUrl();
            var ipAddr = httpContext.GetIpAddress();
            logger.Error(ex, "Lỗi không xác định khi lấy thông tin Giấy Chứng Nhận: {Url}{ClientIp}",
                url,
                ipAddr);
            return Results.BadRequest(new Response<GiayChungNhan>("Lỗi không xác định khi lấy thông tin Giấy Chứng Nhận"));
        }
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
        try
        {
            // Lấy và kiểm tra serial
            var url = httpContext.Request.GetDisplayUrl();
            var serial = httpContext.Request.Query["serial"].ToString().Trim();
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

            // Lấy thông tin Thửa Đất
            var result = await thuaDatService.GetResultAsync(serial);

            // Xử lý kết quả
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
        catch (Exception ex)
        {
            var url = httpContext.Request.GetDisplayUrl();
            logger.Error(ex, "Lỗi không xác định khi lấy thông tin Thửa Đất: {Url}", url);
            return Results.BadRequest(new Response<List<ThuaDat>>("Lỗi không xác định khi lấy thông tin Thửa Đất"));
        }
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
        try
        {
            // Lấy và kiểm tra serial
            var url = httpContext.Request.GetDisplayUrl();
            var serial = httpContext.Request.Query["serial"].ToString().Trim();
            if (string.IsNullOrWhiteSpace(serial))
            {
                logger.Warning("Số serial không được để trống: {Url}", url);
                return Results.BadRequest("Số serial không được để trống!");
            }

            // Lấy thông tin Thửa Đất
            var result = await thuaDatService.GetResultAsync(serial);
            var ipAddr = httpContext.GetIpAddress();

            // Xử lý kết quả
            return result.Match(
                thuaDats =>
                {
                    logger.Information("Lấy thông tin Thửa Đất công khai thành công: {Url}{ClientIp}{Serial}",
                        url,
                        ipAddr,
                        serial);
                    var thuaDatPublic = thuaDats.Select(x => x.ConvertToThuaDatPublic());
                    return Results.Ok(new Response<IEnumerable<ThuaDatPublic>>(thuaDatPublic));
                },
                ex =>
                {
                    logger.Error(ex, "Lỗi khi lấy thông tin Thửa Đất công khai: {Url}{ClientIp}{Serial}",
                        url,
                        ipAddr,
                        serial);
                    return Results.BadRequest(new Response<IEnumerable<ThuaDatPublic>>(ex.Message));
                });
        }
        catch (Exception ex)
        {
            var url = httpContext.Request.GetDisplayUrl();
            var ipAddr = httpContext.GetIpAddress();
            logger.Error(ex, "Lỗi không xác định khi lấy thông tin Thửa Đất công khai: {Url}{ClientIp}",
                url,
                ipAddr);
            return Results.BadRequest(new Response<IEnumerable<ThuaDatPublic>>("Lỗi không xác định khi lấy thông tin Thửa Đất công khai"));
        }
    }

    private static async Task<IResult> DeleteCache(HttpContext context,
        ILogger logger,
        HybridCache hybridCache,
        IConfiguration configuration,
        IConnectionElisData connectionElisData,
        IGiayChungNhanService giayChungNhanService)
    {
        var url = context.Request.GetDisplayUrl();
        var serial = context.Request.Query["serial"].ToString();

        // Chuẩn hóa serial
        serial = serial.ChuanHoa();
        if (string.IsNullOrWhiteSpace(serial))
        {
            logger.Warning("Thiếu thông tin số Serial của Giấy chứng nhận: {Url}", url);
            return Results.BadRequest("Thiếu thông tin số Serial của Giấy chứng nhận!");
        }

        // Kiểm tra quyền xóa cache
        var maDinhDanh = context.User.GetMaDinhDanh();
        if (string.IsNullOrWhiteSpace(maDinhDanh) || !context.User.IsLdap())
        {
            logger.Warning("Người dùng không được phép xóa cache: {Url}{MaDinhDanh}",
                url, maDinhDanh);
            return Results.Unauthorized();
        }

        try
        {
            // Xóa cache và ghi log
            await hybridCache.RemoveByTagAsync(serial);
            logger.Information("Xóa cache thành công: {Url}{MaDinhDanh}",
                url, maDinhDanh);
            return Results.Ok($"Xóa cache thành công cho Giấy chứng nhận có Serial: {serial}");
        }
        catch (Exception e)
        {
            logger.Error(e, "Lỗi khi xóa cache: {Url}{MaDinhDanh}",
                url, maDinhDanh);
            return Results.BadRequest($"Lỗi khi xóa cache: [{e.Message}]");
        }
    }
}