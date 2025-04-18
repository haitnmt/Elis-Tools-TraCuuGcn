using Carter;
using Haihv.Elis.Tool.TraCuuGcn.Api.Exceptions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Services;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using ILogger = Serilog.ILogger;


namespace Haihv.Elis.Tool.TraCuuGcn.Api.Features.GiayChungNhan;

public static class GetHasUpdatePermission
{
    public record Query(string Serial) : IRequest<bool>;

    public class Handler(
        ILogger logger,
        IPermissionService permissionService,
        IHttpContextAccessor httpContextAccessor) : IRequestHandler<Query, bool>
    {
        public async Task<bool> Handle(Query request, CancellationToken cancellationToken)
        {
            // Lấy HttpContext, ném ngoại lệ nếu không có
            var httpContext = httpContextAccessor.HttpContext
                              ?? throw new InvalidOperationException("HttpContext không khả dụng");
            var serial = request.Serial.ChuanHoa();
            if (string.IsNullOrWhiteSpace(serial))
                throw new NoSerialException();
            // Lấy thông tin người dùng
            var user = httpContext.User;
            var email = user.GetEmail();
            var url = httpContext.Request.GetDisplayUrl();
            // Kiểm tra quyền cập nhật, ném ngoại lệ nếu không có quyền
            if (await permissionService.HasUpdatePermission(user, serial,
                    cancellationToken)) return true;
            logger.Warning("{Email} không có quyền cập nhật Giấy chứng nhận {Url} {Serial}", 
                email, url, serial);
            throw new UnauthorizedAccessException();
        }
    }
    /// <summary>
    /// Cấu hình endpoint cho tính năng cập nhật Giấy chứng nhận
    /// </summary>
    /// <remarks>
    /// Lớp này định nghĩa các endpoint API cho việc cập nhật thông tin Giấy chứng nhận
    /// </remarks>
    public class GiayChungNhanEndpoint : ICarterModule
    {
        /// <summary>
        /// Đăng ký các route cho tính năng cập nhật Giấy chứng nhận
        /// </summary>
        /// <param name="app">Đối tượng cấu hình endpoint</param>
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/giay-chung-nhan/has-update-permission", async (ISender sender, string serial) =>
                {
                    // Không cần try-catch ở đây vì đã có middleware xử lý exception toàn cục
                    var response = await sender.Send(new Query(serial));
                    return response ? Results.Ok("Có quyền cập nhật thông tin giấy chứng nhận") : Results.Unauthorized();
                })
                .RequireAuthorization() // Yêu cầu xác thực
                .WithTags("GiayChungNhan"); // Gắn tag cho Swagger
        }
    }
}