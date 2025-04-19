using Carter;
using Haihv.Elis.Tool.TraCuuGcn.Api.Exceptions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Services;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Caching.Hybrid;
using ILogger = Serilog.ILogger;
namespace Haihv.Elis.Tool.TraCuuGcn.Api.Features.Cache;

public static class DeleteCache
{
    public record Query(string Serial) : IRequest<bool>;
    public class Handler(ILogger logger,
        HybridCache hybridCache,
        IHttpContextAccessor httpContextAccessor,
        IPermissionService permissionService) : IRequestHandler<Query, bool>
    {
        public async Task<bool> Handle(Query request, CancellationToken cancellationToken)
        {
            var serial = request.Serial.ChuanHoa();
            if (string.IsNullOrWhiteSpace(serial))
                throw new NoSerialException();
            var httpContext = httpContextAccessor.HttpContext
                              ?? throw new InvalidOperationException("HttpContext không khả dụng");
            var user = httpContext.User;
            if (!permissionService.IsLocalUser(user))
                throw new UnauthorizedAccessException();
            var url = httpContext.Request.GetDisplayUrl();
            var email = user.GetEmail();
            try
            {
                // Xóa cache và ghi log
                await hybridCache.RemoveByTagAsync(serial, cancellationToken);
                logger.Information("{Email} xóa cache thành công: {Url} {Serial}",
                    email, url, serial);
                return true;
            }
            catch (Exception e)
            {
                logger.Error(e, "Lỗi khi xóa cache: {Url} {Email} {Serial}",
                    url, email, serial);
                return false;
            }
        }
    }
    public class CacheEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/cache", async (ISender sender, string serial) =>
                {
                    // Không cần try-catch ở đây vì đã có middleware xử lý exception toàn cục
                    var response = await sender.Send(new Query(serial));
                    return response ? Results.Ok("Xóa Cache thành công!") : Results.BadRequest("Lỗi khi xóa cache");
                })
                .RequireAuthorization()
                .WithTags("Cache");
        }
    }
}