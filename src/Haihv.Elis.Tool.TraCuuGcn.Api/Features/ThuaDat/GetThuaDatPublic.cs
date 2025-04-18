using Carter;
using Haihv.Elis.Tool.TraCuuGcn.Api.Exceptions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Services;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using ILogger = Serilog.ILogger;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Features.ThuaDat;

public static class GetThuaDatPublic
{
    public record Query(string Serial) : IRequest<IEnumerable<ThuaDatPublic>>;

    public class Handler(
        ILogger logger,
        IHttpContextAccessor httpContextAccessor,
        IThuaDatService thuaDatService) : IRequestHandler<Query, IEnumerable<ThuaDatPublic>>
    {
        public async Task<IEnumerable<ThuaDatPublic>> Handle(Query request, CancellationToken cancellationToken)
        {
            var serial = request.Serial.ChuanHoa();
            if (string.IsNullOrWhiteSpace(serial))
                throw new NoSerialException();
            var httpContext = httpContextAccessor.HttpContext
                              ?? throw new InvalidOperationException("HttpContext không khả dụng");
            var email = httpContext.User.GetEmail();
            var url = httpContext.Request.GetDisplayUrl();
            // Lấy thông tin Thửa Đất
            var result = await thuaDatService.GetResultAsync(serial, cancellationToken);
            // Xử lý kết quả
            return result.Match(
                thuaDats =>
                {
                    logger.Information("Lấy thông tin Thửa Đất công khai thành công: {Url}{Email}{Serial}",
                        url,
                        email,
                        serial);
                    var thuaDatPublic = thuaDats.Select(x => x.ConvertToThuaDatPublic());
                    return thuaDatPublic;
                },
                ex =>
                {
                    logger.Error(ex, "Lỗi khi lấy thông tin Thửa Đất công khai: {Url}{Email}{Serial}",
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