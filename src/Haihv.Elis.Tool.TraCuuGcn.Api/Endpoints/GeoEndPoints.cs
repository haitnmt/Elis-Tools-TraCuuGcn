using Haihv.Elis.Tool.TraCuuGcn.Api.Authenticate;
using Haihv.Elis.Tool.TraCuuGcn.Api.Models;
using Haihv.Elis.Tool.TraCuuGcn.Api.Services;
using Microsoft.AspNetCore.Http.Extensions;
using OSGeo.OGR;
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

    private static async Task<IResult> GetToaDoThua(
        HttpContext httpContext,
        ILogger logger,
        IAuthenticationService authenticationService,
        IGeoService geoService)
    {
        var serial = httpContext.Request.Query["serial"].ToString();
        if (string.IsNullOrWhiteSpace(serial))
            return Results.BadRequest("Thiếu thông tin số Serial của Giấy chứng nhận");
        // Lấy thông tin người dùng theo token từ HttpClient
        var user = httpContext.User;
        var maDinhDanh = await authenticationService.CheckAuthenticationAsync(serial, user);
        var url = httpContext.Request.GetDisplayUrl();
        if (string.IsNullOrWhiteSpace(maDinhDanh))
        {
            logger.Error("Người dùng không được phép truy cập thông tin Tọa độ thửa đất: {Url}{MaDinhDanh}",
                url,
                maDinhDanh);
            return Results.Unauthorized();
        }
        var result = await geoService.GetResultAsync(serial);
        return result.Match(
            coordinates =>
            {
                logger.Information("Lấy thông tin toạ độ thửa thành công {Url}{MaDinhDanh}",
                    url,
                    maDinhDanh);
                // Tạo geometry phù hợp với số lượng điểm
                Geometry geometry;
                if (coordinates.Count == 1)
                {
                    // Nếu chỉ có một điểm, sử dụng wkbPoint
                    geometry = new Geometry(wkbGeometryType.wkbPoint);
                    geometry.AddPoint(coordinates[0].X, coordinates[0].Y, 0);
                }
                else
                {
                    // Nếu có nhiều điểm, sử dụng wkbMultiPoint
                    geometry = new Geometry(wkbGeometryType.wkbMultiPoint);
                    foreach (var coordinate in coordinates)
                    {
                        geometry.AddPoint(coordinate.X, coordinate.Y, 0);
                    }
                }

                return Results.Ok(new
                {
                    thuaDats = new FeatureCollectionModel(),
                    netNhas = new FeatureCollectionModel(),
                    longDuongs = new FeatureCollectionModel(),
                    tamThuaDats = new FeatureCollectionModel([geometry])
                });
            },
            ex =>
            {
                logger.Error(ex, "Lỗi khi lấy thông tin toạ độ thửa: {Url}{MaDinhDanh}",
                    url,
                    maDinhDanh);
                return Results.BadRequest(ex.Message);
            });
    }
}