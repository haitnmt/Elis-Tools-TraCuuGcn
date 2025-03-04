using Haihv.Elis.Tool.TraCuuGcn.Api.Authenticate;
using Haihv.Elis.Tool.TraCuuGcn.Api.Settings;
using Microsoft.AspNetCore.Mvc;
using ZiggyCreatures.Caching.Fusion;
using ILogger = Serilog.ILogger;
namespace Haihv.Elis.Tool.TraCuuGcn.Api.Endpoints;

public static class AdminEndPoints
{
    private const string UrlClearCache = "/admin/clear-cache";
    /// <summary>
    /// Định nghĩa các endpoint cho Admin.
    /// </summary>
    /// <param name="app">Ứng dụng web.</param>
    public static void MapAdminEndPoints(this WebApplication app)
    {
        app.MapPost(UrlClearCache, PostClearCache)
            .WithName("PostClearCache")
            .RequireAuthorization();
    }

    private static async Task<IResult> PostClearCache(HttpContext context,
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
        var userAdmins = configuration.GetSection("UserAdmins").Get<string[]>();
        if (string.IsNullOrWhiteSpace(maDinhDanh) && !userAdmins.Contains(maDinhDanh))
        {
            logger.Warning("Người dùng không được phép xóa cache: {Url}{MaDinhDanh}", 
                UrlClearCache, maDinhDanh);
            return Results.Unauthorized();
        }
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