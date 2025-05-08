using Carter;
using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Services;
using Haihv.Elis.Tool.TraCuuGcn.Api.Uri;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using ILogger = Serilog.ILogger;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Features.ChuSuDung;

public static class GetChuSuDung
{
    public record Query(string Serial, string? SoDinhDanh) : IRequest<IEnumerable<ChuSuDungInfo>>;
    public class Handler(
        ILogger logger,
        IPermissionService permissionService,
        IHttpContextAccessor httpContextAccessor,
        IChuSuDungService chuSuDungService) : IRequestHandler<Query, IEnumerable<ChuSuDungInfo>>
    {
        public async Task<IEnumerable<ChuSuDungInfo>> Handle(Query request, CancellationToken cancellationToken)
        {
            var httpContext = httpContextAccessor.HttpContext
                              ?? throw new InvalidOperationException("HttpContext không khả dụng");
            var user = httpContext.User;
            if (!await permissionService.HasReadPermission(user, request.Serial, request.SoDinhDanh, cancellationToken))
                throw new UnauthorizedAccessException();

            var url = httpContext.Request.GetDisplayUrl();

            var isLocal = permissionService.IsLocalUser(user);
            var email = user.GetEmail(isLocal);
            var serial = request.Serial;
            // Lấy thông tin Thửa Đất
            var result = await chuSuDungService.GetResultAsync(serial, cancellationToken);
            // Xử lý kết quả
            return result.Match(
                chuSuDung =>
                {
                    logger.Information("{Email} lấy thông tin chủ sử dụng thành công: {Url} {IsLocal}",
                        email,
                        url,
                        isLocal);
                    return chuSuDung;
                },
                ex =>
                {
                    logger.Error(ex, "{Email} lấy thông tin chủ sử dụng không thành công: {Url} {Message}",
                        email,
                        url,
                        ex.Message);
                    throw ex;
                });
        }
    }
    public class ChuSuDungEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet(ChuSuDungUri.GetChuSuDung, async (ISender sender, string serial, string? soDinhDanh = null) =>
                {
                    // Không cần try-catch ở đây vì đã có middleware xử lý exception toàn cục
                    var response = await sender.Send(new Query(serial, soDinhDanh));
                    return Results.Ok(response);
                })
                .RequireAuthorization("BearerOrApiToken") // Yêu cầu xác thực bằng JWT hoặc API Token
                .WithTags("ChuSuDung");
        }
    }
}