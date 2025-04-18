using Haihv.Elis.Tool.TraCuuGcn.Api.Exceptions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Services;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Caching.Hybrid;
using ILogger = Serilog.ILogger;


namespace Haihv.Elis.Tool.TraCuuGcn.Api.Features.GiayChungNhan;

public static class DeleteMaQr
{
    public record Query(string Serial) : IRequest<bool>;
    public class Handler(
        ILogger logger,
        HybridCache hybridCache,
        IPermissionService permissionService,
        IHttpContextAccessor httpContextAccessor,
        ILogElisDataServices logElisDataServices,
        IGcnQrService gcnQrService) : IRequestHandler<Query, bool>
    {
        public async Task<bool> Handle(Query request, CancellationToken cancellationToken)
        {
            var serial = request.Serial.ChuanHoa();
            if (string.IsNullOrWhiteSpace(serial))
                throw new NoSerialException();
            // Lấy HttpContext, ném ngoại lệ nếu không có
            var httpContext = httpContextAccessor.HttpContext
                              ?? throw new InvalidOperationException("HttpContext không khả dụng");
            // Lấy thông tin người dùng
            var user = httpContext.User;
            var email = user.GetEmail();
            // Kiểm tra quyền cập nhật, ném ngoại lệ nếu không có quyền
            if (!await permissionService.HasUpdatePermission(user, serial, cancellationToken))
                throw new UnauthorizedAccessException();
            
            var url = httpContext.Request.GetDisplayUrl();
            // Thực hiện xóa mã QR
            var result = await gcnQrService.DeleteMaQrAsync(serial, cancellationToken);
            return result.Match(
                succ =>
                {
                    if (succ)
                    {
                        logger.Information("{Email}Xóa mã QR thành công: {Url} {Serial}",
                            email,
                            url,
                            serial);
                        // Ghi log vào ELIS Data
                        logElisDataServices.WriteLogToElisDataAsync(serial, email, url,
                            $"Xóa mã QR của Giấy chứng nhận có phát hành (Serial): {serial}",
                            LogElisDataServices.LoaiTacVu.Xoa, cancellationToken);
                        // Xóa cache liên quan
                        _ = hybridCache.RemoveByTagAsync(serial, cancellationToken).AsTask();
                        return true;
                    }

                    logger.Warning("Xóa mã QR không thành công: {Url}{MaDinhDanh}",
                        url,
                        email);
                    return false;
                },
                ex =>
                {
                    logger.Error(ex, "Lỗi khi xóa mã QR: {Url}{MaDinhDanh}",
                        url,
                        email);
                    return false;
                });
        }
    }
}