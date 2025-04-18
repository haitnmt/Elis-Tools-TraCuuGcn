using Carter;
using Haihv.Elis.Tool.TraCuuGcn.Api.Exceptions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Services;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using ILogger = Serilog.ILogger;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Features.ThuaDat;

public static class GetThuaDat
{
    public record Query(string Serial, string? SoDinhDanh) : IRequest<IEnumerable<TraCuuGcn.Models.ThuaDat>>;

    public class Handler(
        ILogger logger,
        IPermissionService permissionService,
        IHttpContextAccessor httpContextAccessor,
        IThuaDatService thuaDatService) : IRequestHandler<Query, IEnumerable<TraCuuGcn.Models.ThuaDat>>
    {
        public async Task<IEnumerable<TraCuuGcn.Models.ThuaDat>> Handle(Query request, CancellationToken cancellationToken)
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
            // Lấy thông tin Thửa Đất
            var result = await thuaDatService.GetResultAsync(serial, cancellationToken);
            // Xử lý kết quả
            return result.Match(
                thuaDats =>
                {
                    logger.Information("Lấy thông tin Thửa Đất thành công: {Url}{Email}{Serial}",
                        url,
                        email,
                        serial);
                    return thuaDats;
                },
                ex =>
                {
                    logger.Error(ex, "Lỗi khi lấy thông tin Thửa Đất: {Url}{Email}{Serial}",
                        url,
                        email,
                        serial);
                    throw ex;
                });
        }
    }
    
    public class ThuaDatEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/thua-dat/", async (ISender sender, string serial, string? soDinhDanh = null) =>
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