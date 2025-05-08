using Carter;
using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Services;
using Haihv.Elis.Tool.TraCuuGcn.Api.Uri;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using ILogger = Serilog.ILogger;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Features.ChuSuDung;

public static class GetHasReadPermission
{
    public record Query(string Serial, string SoDinhDanh) : IRequest<bool>;

    public class Handler(
        ILogger logger,
        IPermissionService permissionService,
        IHttpContextAccessor httpContextAccessor) : IRequestHandler<Query, bool>
    {
        public async Task<bool> Handle(Query request, CancellationToken cancellationToken)
        {
            var httpContext = httpContextAccessor.HttpContext
                              ?? throw new InvalidOperationException("HttpContext không khả dụng");
            var user = httpContext.User;
            var isLocal = permissionService.IsLocalUser(user);
            if (!string.IsNullOrWhiteSpace(request.Serial) && isLocal)
            {
                return true;
            }
            var email = user.GetEmail(isLocal);
            var url = httpContext.Request.GetDisplayUrl();
            var serial = request.Serial;
            if (!await permissionService.HasReadPermission(user, serial, request.SoDinhDanh, cancellationToken))
            {
                logger.Error("{Email} Xác minh thông tin theo Số định danh không thành công thành công: {Url}",
                    email,
                    url);
                throw new UnauthorizedAccessException();
            }
            logger.Information("{Email} Xác minh thông tin theo Số định danh thành công thành công: {Url}",
                email,
                url);
            return true;
        }
    }
    public class ChuSuDungEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet(ChuSuDungUri.GetHasReadPermission, async (ISender sender, string serial, string soDinhDanh) =>
                {
                    // Không cần try-catch ở đây vì đã có middleware xử lý exception toàn cục
                    var response = await sender.Send(new Query(serial, soDinhDanh));
                    return response ? Results.Ok() : Results.Unauthorized();
                })
                .RequireAuthorization("BearerOrApiToken") // Yêu cầu xác thực bằng JWT hoặc API Token
                .WithTags("ChuSuDung");
        }
    }
}