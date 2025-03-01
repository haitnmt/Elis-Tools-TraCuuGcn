using Haihv.Elis.Tool.TraCuuGcn.Api.Authenticate;
using Haihv.Elis.Tool.TraCuuGcn.Api.Models;
using Haihv.Elis.Tool.TraCuuGcn.Api.Services;
using Microsoft.AspNetCore.Mvc;
using OSGeo.OGR;
using ILogger = Serilog.ILogger;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Endpoints;

public static class GeoEndPoints
{   
    private const string UrlGetToaDoThua = "/geo/toaDoThua";
    /// <summary>
    /// Định nghĩa endpoint để lấy thông tin chủ sử dụng.
    /// </summary>
    /// <param name="app">Ứng dụng web.</param>
    public static void MapGeoEndPoints(this WebApplication app)
    {
        app.MapGet(UrlGetToaDoThua, GetToaDoThua)
            .WithName("GetToaDoThua")
            .RequireAuthorization();
    }

    private static async Task<IResult> GetToaDoThua([FromQuery] long maGcnElis,
        HttpContext httpContext, 
        ILogger logger, 
        IAuthenticationService authenticationService,
        IGeoService geoService)
    {
        // Lấy thông tin người dùng theo token từ HttpClient
        var user = httpContext.User;
        var maDinhDanh = await authenticationService.CheckAuthenticationAsync(maGcnElis, user);
        if (string.IsNullOrWhiteSpace(maDinhDanh))
        {
            logger.Error("Người dùng không được phép truy cập thông tin Tọa độ thửa đất: {MaGcnElis}{Url}{MaDinhDanh}", 
                maGcnElis, 
                UrlGetToaDoThua, 
                maDinhDanh);
            return Results.Unauthorized();
        }
        var result = await geoService.GetResultAsync(maGcnElis);
        return result.Match(
            coordinates =>
            {
                logger.Information("Lấy thông tin toạ độ thửa thành công {MaGcnElis}{Url}{MaDinhDanh}",  
                    maGcnElis, 
                    UrlGetToaDoThua, 
                    maDinhDanh);
                var geometry = new Geometry(wkbGeometryType.wkbPoint);
                foreach (var coordinate in coordinates)
                {
                    geometry.AddPoint(coordinate.X, coordinate.Y, 0);
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
                logger.Error(ex, "Lỗi khi lấy thông tin toạ độ thửa: {MaGcnElis}{Url}{MaDinhDanh}",  
                    maGcnElis, 
                    UrlGetToaDoThua, 
                    maDinhDanh);
                return Results.BadRequest(ex.Message);
            });
    }
}