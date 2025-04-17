using Carter;
using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Services;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using ILogger = Serilog.ILogger;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Features.ThuaDat;

public static class GetThuaDatAsync
{
    public record Query(string Serial) : IRequest<IEnumerable<TraCuuGcn.Models.ThuaDat>>;

    public class Handler(
        ILogger logger,
        IHttpContextAccessor httpContextAccessor,
        IThuaDatService thuaDatService) : IRequestHandler<Query, IEnumerable<TraCuuGcn.Models.ThuaDat>>
    {
        public async Task<IEnumerable<TraCuuGcn.Models.ThuaDat>> Handle(Query request, CancellationToken cancellationToken)
        {
            var httpContext = httpContextAccessor.HttpContext
                              ?? throw new InvalidOperationException("HttpContext không khả dụng");
            var ipAddress = httpContext.GetIpAddress();
            var url = httpContext.Request.GetDisplayUrl();
            var serial = request.Serial;
            // Lấy thông tin Thửa Đất
            var result = await thuaDatService.GetResultAsync(serial, cancellationToken);
            // Xử lý kết quả
            return result.Match(
                thuaDats =>
                {
                    logger.Information("Lấy thông tin Thửa Đất thành công: {Url}{ClientIp}{Serial}",
                        url,
                        ipAddress,
                        serial);
                    return thuaDats;
                },
                ex =>
                {
                    logger.Error(ex, "Lỗi khi lấy thông tin Thửa Đất: {Url}{ClientIp}{Serial}",
                        url,
                        ipAddress,
                        serial);
                    throw ex;
                });
        }
    }
    
    public class ThuaDatEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/thua-dat/", async (ISender sender, string serial) =>
                {
                    // Không cần try-catch ở đây vì đã có middleware xử lý exception toàn cục
                    var response = await sender.Send(new Query(serial));
                    return Results.Ok(response);
                })
                .RequireAuthorization()
                .WithTags("ThuaDat");
        }
    }
}