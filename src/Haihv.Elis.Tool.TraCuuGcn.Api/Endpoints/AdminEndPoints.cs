using Haihv.Elis.Tool.TraCuuGcn.Api.Authenticate;
using Haihv.Elis.Tool.TraCuuGcn.Api.Settings;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using ZiggyCreatures.Caching.Fusion;
using ILogger = Serilog.ILogger;
namespace Haihv.Elis.Tool.TraCuuGcn.Api.Endpoints;

public static class AdminEndPoints
{
    /// <summary>
    /// Định nghĩa các endpoint cho Admin.
    /// </summary>
    /// <param name="app">Ứng dụng web.</param>
    public static void MapAdminEndPoints(this WebApplication app)
    {
        var mapGroup = app.MapGroup("/local");
        mapGroup.MapDelete("/delete-cache", DeleteCacheBySerial)
            .WithName("DeleteCacheBySerial")
            .RequireAuthorization();
    }

    private static async Task<IResult> DeleteCacheBySerial(HttpContext context,
        ILogger logger,
        IFusionCache fusionCache,
        IConfiguration configuration,
        [FromQuery] string? serial)
    {
        var url = context.Request.GetDisplayUrl();
        if (string.IsNullOrWhiteSpace(serial))
        {
            logger.Warning("Thiếu thông tin số Serial của GCN: {Url}", url);
            return Results.BadRequest("Thiếu thông tin số Serial của GCN");
        }
        var maDinhDanh = context.User.GetMaDinhDanh();
        var isLdapUser = context.User.IsLdap();
        if (string.IsNullOrWhiteSpace(maDinhDanh) || !isLdapUser)
        {
            logger.Warning("Người dùng không được phép xóa cache: {Url}{MaDinhDanh}", 
                url, maDinhDanh);
            return Results.Unauthorized();
        }
        try
        {
            await fusionCache.RemoveByTagAsync(serial.ChuanHoa());
            logger.Information("Xóa cache thành công: {Url}{Serial}{MaDinhDanh}",
                 url,serial, maDinhDanh);
            return Results.Ok("Xóa cache thành công");
        }
        catch (Exception e)
        {
            logger.Error(e, "Lỗi khi xóa cache: {Url}{Serial}{MaDinhDanh}",
                url, serial, maDinhDanh);
            return Results.BadRequest("Lỗi khi xóa cache");
        }
    }
}