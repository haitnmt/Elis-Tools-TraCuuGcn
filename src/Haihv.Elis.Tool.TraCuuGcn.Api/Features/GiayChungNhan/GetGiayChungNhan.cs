using Carter;
using Haihv.Elis.Tool.TraCuuGcn.Api.Exceptions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Services;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using ILogger = Serilog.ILogger;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Features.GiayChungNhan;

public static class GetGiayChungNhan
{
    public record Query(string Serial) : IRequest<TraCuuGcn.Models.GiayChungNhan>;
    
    public class Handler(ILogger logger,
        IHttpContextAccessor httpContextAccessor,
        IGiayChungNhanService giayChungNhanService) : IRequestHandler<Query, TraCuuGcn.Models.GiayChungNhan>
    {
        public async Task<TraCuuGcn.Models.GiayChungNhan> Handle(Query request, CancellationToken cancellationToken)
        {
            var httpContext = httpContextAccessor.HttpContext
                              ?? throw new InvalidOperationException("HttpContext không khả dụng");
            var ipAddress = httpContext.GetIpAddress();
            var url = httpContext.Request.GetDisplayUrl();
            var serial = request.Serial;
            if (string.IsNullOrWhiteSpace(serial))
                throw new NoSerialException();
            var result = await giayChungNhanService.GetResultAsync(serial, cancellationToken: cancellationToken);
            return result.Match(giayChungNhan =>
            {
                logger.Information("Lấy thông tin Giấy Chứng Nhận thành công: {Url}{ClientIp}{Serial}",
                    url,
                    ipAddress,
                    serial);
                return giayChungNhan;
            },
            ex =>
            {
                logger.Error(ex, "Lỗi khi lấy thông tin Giấy Chứng Nhận: {Url}{ClientIp}{Serial}", 
                    url, ipAddress, serial);
                throw ex;
            });
        }
    }
    public class GiayChungNhanEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/giay-chung-nhan/", async (ISender sender, string serial) =>
                {
                    // Không cần try-catch ở đây vì đã có middleware xử lý exception toàn cục
                    var response = await sender.Send(new Query(serial));
                    return Results.Ok(response);
                })
                .RequireAuthorization()
                .WithTags("GiayChungNhan");
        }
    }
}