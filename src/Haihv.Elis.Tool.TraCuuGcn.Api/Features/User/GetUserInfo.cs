using Carter;
using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using ILogger = Serilog.ILogger;
using MediatR;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Features.User;

public static class GetUserInfo
{
    public record Query : IRequest<UserInfo>;

    public class Handler(
        ILogger logger,
        IHttpContextAccessor httpContextAccessor) : IRequestHandler<Query, UserInfo>
    {
        public Task<UserInfo> Handle(Query request, CancellationToken cancellationToken)
        {
            var httpContext = httpContextAccessor.HttpContext
                              ?? throw new InvalidOperationException("HttpContext không khả dụng");
            var user = httpContext.User;
            var userInfo = user.GetUserInfo();
            logger.Information("Lấy thông tin người sử dụng thành công {email}", userInfo.Email);
            return Task.FromResult(userInfo);
        }
    }
    public class UserEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/user/info", async (ISender sender) =>
                {
                    // Không cần try-catch ở đây vì đã có middleware xử lý exception toàn cục
                    var response = await sender.Send(new Query());
                    return Results.Ok(response);
                })
                .RequireAuthorization()
                .WithTags("User");
        }
    }
}