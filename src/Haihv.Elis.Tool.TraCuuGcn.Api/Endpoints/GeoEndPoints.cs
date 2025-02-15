using Haihv.Elis.Tool.TraCuuGcn.Api.Authenticate;
using Haihv.Elis.Tool.TraCuuGcn.Api.Services;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Endpoints;

public static class GeoEndPoints
{   
    /// <summary>
    /// Định nghĩa endpoint để lấy thông tin chủ sử dụng.
    /// </summary>
    /// <param name="app">Ứng dụng web.</param>
    public static void MapGeoEndPoints(this WebApplication app)
    {
        app.MapGet("/geo/toaDoThua", GetToaDoThua)
            .WithName("GetToaDoThua")
            .RequireAuthorization();
    }

    private static async Task GetToaDoThua([FromQuery] long maGcnElis,
        HttpContext httpContext, ILogger logger, IGeoService geoService)
    {
        // Lấy thông tin người dùng theo token từ HttpClient
        var user = httpContext.User;
        var soDinhDanh = user.GetSoDinhDanh();
        var result = await geoService.GetResultAsync(maGcnElis);
        await Task.FromResult(result.Match(
            toaDoThua =>
            {
                logger.Information("Lấy thông tin toạ độ thửa thành công: {MaGcnElis} {SoDinhDanh}",
                    maGcnElis, soDinhDanh);
                return Results.Ok(toaDoThua);
            },
            ex =>
            {
                logger.Error(ex, "Lỗi khi lấy thông tin toạ độ thửa: {MaGcnElis} {SoDinhDanh}",
                    maGcnElis, soDinhDanh);
                return Results.BadRequest(ex.Message);
            }));
    }
}