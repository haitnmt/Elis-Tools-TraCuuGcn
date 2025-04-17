using Carter;
using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Services;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using ILogger = Serilog.ILogger;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Features.ThuaDat;

public static class GetThuaDatPublicAsync
{
    public record Query(string Serial) : IRequest<IEnumerable<ThuaDatPublic>>;

    public class Handler(
        ILogger logger,
        IHttpContextAccessor httpContextAccessor,
        IThuaDatService thuaDatService) : IRequestHandler<Query, IEnumerable<ThuaDatPublic>>
    {
        public async Task<IEnumerable<ThuaDatPublic>> Handle(Query request, CancellationToken cancellationToken)
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
                    logger.Information("Lấy thông tin Thửa Đất công khai thành công: {Url}{ClientIp}{Serial}",
                        url,
                        ipAddress,
                        serial);
                    var thuaDatPublic = thuaDats.Select(x => x.ConvertToThuaDatPublic());
                    return thuaDatPublic;
                },
                ex =>
                {
                    logger.Error(ex, "Lỗi khi lấy thông tin Thửa Đất công khai: {Url}{ClientIp}{Serial}",
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
            app.MapGet("/thua-dat-public/", async (ISender sender, string serial) =>
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