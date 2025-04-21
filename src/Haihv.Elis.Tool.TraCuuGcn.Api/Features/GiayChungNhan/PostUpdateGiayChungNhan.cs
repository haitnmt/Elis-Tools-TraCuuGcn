using Carter;
using Haihv.Elis.Tool.TraCuuGcn.Api.Exceptions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Services;
using Haihv.Elis.Tool.TraCuuGcn.Api.Uri;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Caching.Hybrid;
using ILogger = Serilog.ILogger;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Features.GiayChungNhan;

/// <summary>
/// Tính năng cập nhật thông tin pháp lý của Giấy chứng nhận
/// </summary>
/// <remarks>
/// Tính năng này cho phép cập nhật các thông tin pháp lý như ngày ký, người ký, số vào sổ
/// của Giấy chứng nhận trong hệ thống ELIS
/// </remarks>
public static class PostUpdateGiayChungNhan
{
    /// <summary>
    /// Lệnh cập nhật thông tin pháp lý Giấy chứng nhận
    /// </summary>
    /// <param name="PhapLyGiayChungNhan">Thông tin pháp lý cần cập nhật cho Giấy chứng nhận</param>
    public record Command(PhapLyGiayChungNhan PhapLyGiayChungNhan) : IRequest<bool>;
    
    /// <summary>
    /// Xử lý lệnh cập nhật thông tin pháp lý Giấy chứng nhận
    /// </summary>
    /// <remarks>
    /// Lớp này thực hiện việc kiểm tra quyền, cập nhật thông tin và ghi log cho quá trình cập nhật
    /// </remarks>
    /// <param name="logger">Dịch vụ ghi log</param>
    /// <param name="permissionService">Dịch vụ kiểm tra quyền</param>
    /// <param name="httpContextAccessor">Đối tượng truy cập HttpContext</param>
    /// <param name="giayChungNhanService">Dịch vụ quản lý Giấy chứng nhận</param>
    public class Handler(
        ILogger logger,
        HybridCache hybridCache,
        IPermissionService permissionService,
        IHttpContextAccessor httpContextAccessor,
        IGiayChungNhanService giayChungNhanService,
        ILogElisDataServices logElisDataServices) : IRequestHandler<Command, bool>
    {
        /// <summary>
        /// Xử lý yêu cầu cập nhật thông tin pháp lý Giấy chứng nhận
        /// </summary>
        /// <param name="request">Lệnh cập nhật chứa thông tin pháp lý Giấy chứng nhận</param>
        /// <param name="cancellationToken">Token hủy thao tác</param>
        /// <returns>True nếu cập nhật thành công, ngược lại False</returns>
        /// <exception cref="InvalidOperationException">Ném ra khi HttpContext không khả dụng</exception>
        /// <exception cref="UnauthorizedAccessException">Ném ra khi người dùng không có quyền cập nhật</exception>
        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            // Lấy HttpContext, ném ngoại lệ nếu không có
            var httpContext = httpContextAccessor.HttpContext
                              ?? throw new InvalidOperationException("HttpContext không khả dụng");
            var serial = request.PhapLyGiayChungNhan.Serial.ChuanHoa();
            if (string.IsNullOrWhiteSpace(serial))
                throw new NoSerialException();
            // Lấy thông tin người dùng
            var user = httpContext.User;
            var email = user.GetEmail();
            // Kiểm tra quyền cập nhật, ném ngoại lệ nếu không có quyền
            if (!await permissionService.HasUpdatePermission(user, request.PhapLyGiayChungNhan.Serial, cancellationToken))
                throw new UnauthorizedAccessException();
            
            var url = httpContext.Request.GetDisplayUrl();
            
            // Thực hiện cập nhật thông tin pháp lý Giấy chứng nhận
            var result = await giayChungNhanService.UpdateAsync(request.PhapLyGiayChungNhan, cancellationToken);
            
            // Xử lý kết quả cập nhật
            return result.Match(
                succ =>
                {
                    // Ghi log thành công
                    logger.Information("{Email} cập nhật thông tin Giấy chứng nhận thành công: {Url}{Serial}",
                    email,
                        url,
                        serial);
                    // Tạo thông điệp log
                    var message = $"""
                                   Cập nhật thông tin Giấy chứng nhận: Serial {request.PhapLyGiayChungNhan.Serial} |
                                   Ngày ký: {request.PhapLyGiayChungNhan.NgayKy:dd/MM/yyyy} |
                                   Người ký: {request.PhapLyGiayChungNhan.NguoiKy} |
                                   Số vào sổ: {request.PhapLyGiayChungNhan.SoVaoSo}
                                   """;
                    // Loại bỏ khoảng trắng và xuống dòng
                    message = message.Replace("\n", string.Empty).Replace("\r", string.Empty);

                    // Ghi log vào ELIS Data
                    logElisDataServices.WriteLogToElisDataAsync(request.PhapLyGiayChungNhan.Serial, 
                        email, url, message,
                        cancellationToken: cancellationToken);

                    // Xóa cache liên quan
                    _ = hybridCache.RemoveByTagAsync(serial, cancellationToken).AsTask();
                    return succ;
                },
                ex =>
                {
                    // Ghi log lỗi và ném lại ngoại lệ
                    logger.Error(ex, "Lỗi khi cập nhật thông tin Giấy chứng nhận: {Url}{Email}{Serial}",
                        url,
                        email,
                        serial);
                    throw ex;
                });
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
            app.MapPost(GiayChungNhanUri.UpdateGiayChungNhan, async (ISender sender, PhapLyGiayChungNhan phapLyGiayChungNhan) =>
                {
                    // Không cần try-catch ở đây vì đã có middleware xử lý exception toàn cục
                    var response = await sender.Send(new Command(phapLyGiayChungNhan));
                    return response ? Results.Ok("Cập nhật thành công") : Results.NotFound();
                })
                .RequireAuthorization() // Yêu cầu xác thực
                .WithTags("GiayChungNhan"); // Gắn tag cho Swagger
        }
    }
}