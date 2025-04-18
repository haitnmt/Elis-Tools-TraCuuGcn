using Carter;
using Haihv.Elis.Tool.TraCuuGcn.Api.Exceptions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Models;
using Haihv.Elis.Tool.TraCuuGcn.Api.Services;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using OSGeo.OGR;
using ILogger = Serilog.ILogger;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Features.ThuaDat;

public static class GetToaDoThua
{
    public record Query(string Serial, string? SoDinhDanh) : IRequest<Response>;
    public record Response(FeatureCollectionModel ThuaDats, FeatureCollectionModel NetNhas,
        FeatureCollectionModel LongDuongs, FeatureCollectionModel TamThuaDats);
    
    public class Handler(
        ILogger logger,
        IHttpContextAccessor httpContextAccessor,
        IPermissionService permissionService,
        IGeoService geoService) : IRequestHandler<Query, Response>
    {
        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var serial = request.Serial.ChuanHoa();
            if (string.IsNullOrWhiteSpace(serial))
                throw new NoSerialException();
            var httpContext = httpContextAccessor.HttpContext
                              ?? throw new InvalidOperationException("HttpContext không khả dụng");
            var user = httpContext.User;
            if (!await permissionService.HasReadPermission(user, serial, request.SoDinhDanh, cancellationToken))
                throw new UnauthorizedAccessException();
            var email = user.GetEmail();
            var url = httpContext.Request.GetDisplayUrl();
            var result = await geoService.GetResultAsync(serial, cancellationToken);
            
            return result.Match(
                coordinates =>
                {
                    logger.Information("{Email} Lấy thông tin toạ độ thửa thành công {Url} {Serial}",
                        email,
                        url,
                        serial);
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

                    return new Response(
                        new FeatureCollectionModel(), 
                        new FeatureCollectionModel(),
                        new FeatureCollectionModel(),
                        new FeatureCollectionModel([geometry]));
                },
                ex =>
                {
                    logger.Error(ex, "{Email} Lỗi khi lấy thông tin toạ độ thửa: {Url} {Serial}",
                        email,
                        url,
                        serial);
                    throw ex;
                });
        }
    }
    public class ThuaDatEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/thua-dat/toa-do-thua", async (ISender sender, string serial, string? soDinhDanh = null) =>
                {
                    // Không cần try-catch ở đây vì đã có middleware xử lý exception toàn cục
                    var response = await sender.Send(new Query(serial, soDinhDanh));
                    return Results.Ok(response);
                })
                .RequireAuthorization()
                .WithTags("ThuaDat");
        }
    }
}