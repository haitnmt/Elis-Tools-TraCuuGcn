using Carter;
using Haihv.Elis.Tool.TraCuuGcn.Api.Exceptions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Services;
using Haihv.Elis.Tool.TraCuuGcn.Api.Uri;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using ILogger = Serilog.ILogger;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Features.TaiSan;

/// <summary>
/// Cung cấp các chức năng để truy vấn thông tin tài sản dựa trên số serial của Giấy chứng nhận
/// </summary>
public static class GetTaiSan
{
    /// <summary>
    /// Đại diện cho yêu cầu truy vấn thông tin tài sản
    /// </summary>
    /// <param name="Serial">Số serial của Giấy chứng nhận</param>
    /// <param name="SoDinhDanh">Số định danh của người dùng (tùy chọn) để xác thực quyền truy cập</param>
    public record Query(string Serial, string? SoDinhDanh) : IRequest<IEnumerable<TraCuuGcn.Models.TaiSan>>;

    /// <summary>
    /// Xử lý yêu cầu truy vấn thông tin tài sản
    /// </summary>
    public class Handler(
        ILogger logger,
        IPermissionService permissionService,
        IHttpContextAccessor httpContextAccessor,
        IThuaDatService thuaDatService,
        IChuSuDungService chuSuDungService,
        ITaiSanService taiSanService) : IRequestHandler<Query, IEnumerable<TraCuuGcn.Models.TaiSan>>
    {
        /// <summary>
        /// Xử lý yêu cầu lấy thông tin tài sản dựa trên số serial và số định danh
        /// </summary>
        /// <param name="request">Yêu cầu chứa số serial và số định danh</param>
        /// <param name="cancellationToken">Token hủy bỏ thao tác</param>
        /// <returns>Danh sách thông tin tài sản</returns>
        /// <exception cref="NoSerialException">Khi số serial không được cung cấp hoặc không hợp lệ</exception>
        /// <exception cref="InvalidOperationException">Khi HttpContext không khả dụng</exception>
        /// <exception cref="UnauthorizedAccessException">Khi người dùng không có quyền truy cập</exception>
        /// <exception cref="ThuaDatNotFoundException">Khi không tìm thấy thông tin thửa đất với số serial đã cho</exception>
        /// <exception cref="ChuSuDungNotFoundException">Khi không tìm thấy thông tin chủ sử dụng với số serial đã cho</exception>
        /// <exception cref="TaiSanNotFoundException">Khi không tìm thấy thông tin tài sản với số serial đã cho</exception>
        public async Task<IEnumerable<TraCuuGcn.Models.TaiSan>> Handle(Query request, CancellationToken cancellationToken)
        {
            // Chuẩn hóa số serial
            var serial = request.Serial.ChuanHoa();
            if (string.IsNullOrWhiteSpace(serial))
                throw new NoSerialException();

            // Lấy HttpContext và kiểm tra
            var httpContext = httpContextAccessor.HttpContext
                              ?? throw new InvalidOperationException("HttpContext không khả dụng");
            var user = httpContext.User;

            // Kiểm tra quyền truy cập
            if (!await permissionService.HasReadPermission(user, serial, request.SoDinhDanh, cancellationToken))
                throw new UnauthorizedAccessException();

            // Lấy thông tin người dùng và URL hiện tại
            var isLocal = permissionService.IsLocalUser(user);
            var email = user.GetEmail(isLocal);
            var url = httpContext.Request.GetDisplayUrl();

            // Lấy danh sách mã thửa đất
            var dsMaThuaDatTask = thuaDatService.GetMaThuaDatAsync(serial, cancellationToken);

            // Lấy danh sách mã Chủ sử dụng
            var dsMaChuSuDungTask = chuSuDungService.GetMaChuSuDungAsync(serial, cancellationToken);

            // Chờ và kiểm tra kết quả thửa đất
            var dsMaThuaDat = await dsMaThuaDatTask;
            if (dsMaThuaDat.Count == 0)
            {
                logger.Warning("{Email} Không tìm thấy thông tin thửa đất: {Url} {IsLocal}",
                    email,
                    url,
                    isLocal);
                throw new TaiSanNotFoundException(serial);
            }

            // Chờ và kiểm tra kết quả chủ sử dụng
            var dsMaChuSuDung = await dsMaChuSuDungTask;
            if (dsMaChuSuDung.Count == 0)
            {
                logger.Warning("{Email} Không tìm thấy thông tin chủ sử dụng: {Url} {IsLocal}",
                    email,
                    url,
                    isLocal);
                throw new TaiSanNotFoundException(serial);
            }

            // Lấy thông tin tài sản dựa trên mã thửa đất và mã chủ sử dụng
            var result = await taiSanService.GetTaiSanAsync(serial, dsMaThuaDat, dsMaChuSuDung);

            // Xử lý kết quả trả về
            return await Task.FromResult(result.Match(
                taiSan =>
                {
                    // Nếu có tài sản, trả về danh sách
                    if (taiSan.Count != 0)
                    {
                        logger.Information("{Email} lấy thông tin tài sản thành công: {Url} {IsLocal}",
                            email,
                            url,
                            isLocal);
                        return taiSan;
                    }

                    // Nếu không có tài sản, ghi log và ném ngoại lệ
                    logger.Warning("{Email} Không tìm thấy thông tin tài sản: {Url} {IsLocal}",
                        email,
                        url,
                        isLocal);
                    throw new TaiSanNotFoundException(serial);
                },
                ex =>
                {
                    // Xử lý lỗi khi lấy thông tin tài sản
                    logger.Error(ex, "{Email} Lỗi khi lấy thông tin tài sản: {Url} {Message}",
                        email,
                        url,
                        ex.Message);
                    throw ex;
                }));
        }
    }

    /// <summary>
    /// Định nghĩa endpoint API để truy vấn thông tin tài sản
    /// </summary>
    public class TaiSanEndpoint : ICarterModule
    {
        /// <summary>
        /// Cấu hình các route cho API tài sản
        /// </summary>
        /// <param name="app">IEndpointRouteBuilder để đăng ký route</param>
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet(TaiSanUri.GetTaiSan, async (ISender sender, string serial, string? soDinhDanh = null) =>
                {
                    // Không cần try-catch ở đây vì đã có middleware xử lý exception toàn cục
                    var response = await sender.Send(new Query(serial, soDinhDanh));
                    return Results.Ok(response);
                })
                .RequireAuthorization("BearerOrApiToken") // Yêu cầu xác thực bằng JWT hoặc API Token
                .WithTags("TaiSan");
        }
    }
}