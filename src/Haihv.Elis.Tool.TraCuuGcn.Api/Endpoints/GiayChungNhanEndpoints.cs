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

        mapGroup.MapDelete("/delete-cache", DeleteCache)
            .WithName("DeleteCache")
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