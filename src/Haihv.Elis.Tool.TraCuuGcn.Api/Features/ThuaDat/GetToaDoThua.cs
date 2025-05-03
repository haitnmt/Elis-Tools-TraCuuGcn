using Carter;
using Haihv.Elis.Tool.TraCuuGcn.Api.Exceptions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Models;
using Haihv.Elis.Tool.TraCuuGcn.Api.Services;
using Haihv.Elis.Tool.TraCuuGcn.Api.Uri;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using OSGeo.OGR;
using ILogger = Serilog.ILogger;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Features.ThuaDat;

/// <summary>
/// Cung cấp các tính năng lấy thông tin tọa độ thửa đất dựa trên số phát hành Giấy chứng nhận.
/// </summary>
public static class GetToaDoThua
{
    /// <summary>
    /// Đại diện cho một yêu cầu lấy thông tin tọa độ thửa đất.
    /// </summary>
    /// <param name="Serial">Số phát hành của Giấy chứng nhận.</param>
    /// <param name="SoDinhDanh">Số định danh của chủ sử dụng (tùy chọn).</param>
    public record Query(string Serial, string? SoDinhDanh) : IRequest<Response>;
    
    /// <summary>
    /// Đại diện cho kết quả trả về khi lấy thông tin tọa độ thửa đất.
    /// </summary>
    /// <param name="ThuaDats">Tập hợp các đối tượng thửa đất.</param>
    /// <param name="NetNhas">Tập hợp các đối tượng nét nhà.</param>
    /// <param name="LongDuongs">Tập hợp các đối tượng lòng đường.</param>
    /// <param name="TamThuaDats">Tập hợp các đối tượng tâm thửa đất.</param>
    public record Response(FeatureCollectionModel ThuaDats, FeatureCollectionModel NetNhas,
        FeatureCollectionModel LongDuongs, FeatureCollectionModel TamThuaDats);
    
    /// <summary>
    /// Xử lý yêu cầu lấy thông tin tọa độ thửa đất.
    /// </summary>
    public class Handler(
        ILogger logger,
        IHttpContextAccessor httpContextAccessor,
        IPermissionService permissionService,
        IGeoService geoService) : IRequestHandler<Query, Response>
    {
        /// <summary>
        /// Xử lý yêu cầu lấy thông tin tọa độ thửa đất và trả về dữ liệu không gian địa lý.
        /// </summary>
        /// <param name="request">Yêu cầu lấy thông tin tọa độ thửa đất.</param>
        /// <param name="cancellationToken">Token hủy bỏ tác vụ.</param>
        /// <returns>Dữ liệu không gian địa lý của thửa đất.</returns>
        /// <exception cref="NoSerialException">Khi không có số phát hành.</exception>
        /// <exception cref="InvalidOperationException">Khi HttpContext không khả dụng.</exception>
        /// <exception cref="UnauthorizedAccessException">Khi người dùng không có quyền đọc thông tin.</exception>
        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            // Chuẩn hóa số phát hành
            var serial = request.Serial.ChuanHoa();
            if (string.IsNullOrWhiteSpace(serial))
                throw new NoSerialException();
                
            // Lấy thông tin HttpContext để kiểm tra quyền và ghi log
            var httpContext = httpContextAccessor.HttpContext
                              ?? throw new InvalidOperationException("HttpContext không khả dụng");
                              
            // Lấy thông tin người dùng
            var user = httpContext.User;
            
            // Kiểm tra quyền đọc dữ liệu
            if (!await permissionService.HasReadPermission(user, serial, request.SoDinhDanh, cancellationToken))
                throw new UnauthorizedAccessException();
                
            // Lấy thông tin email và URL để ghi log
            var email = user.GetEmail();
            var url = httpContext.Request.GetDisplayUrl();
            var isLocal =permissionService.IsLocalUser(user);
            
            // Lấy thông tin tọa độ thửa đất từ service
            var result = await geoService.GetResultAsync(serial, cancellationToken);
            
            // Xử lý kết quả trả về
            return result.Match(
                // Xử lý khi lấy thông tin thành công
                coordinates =>
                {
                    logger.Information("{Email} Lấy thông tin toạ độ thửa thành công {Url} {IsLocal}",
                        email,
                        url,
                        isLocal);
                        
                    // Tạo geometry phù hợp với số lượng điểm
                    Geometry geometry;
                    if (coordinates.Count == 1)
                    {
                        // Nếu chỉ có một điểm, sử dụng wkbPoint
                        geometry = new Geometry(wkbGeometryType.wkbPoint);
                        geometry.AddPoint(coordinates[0].X, coordinates[0].Y, 0);
                    }
                    else
                    {
                        // Nếu có nhiều điểm, sử dụng wkbMultiPoint
                        geometry = new Geometry(wkbGeometryType.wkbMultiPoint);
                        foreach (var coordinate in coordinates)
                        {
                            geometry.AddPoint(coordinate.X, coordinate.Y, 0);
                        }
                    }

                    // Trả về kết quả với thông tin tâm thửa đất
                    return new Response(
                        new FeatureCollectionModel(), // Thửa đất trống
                        new FeatureCollectionModel(), // Nét nhà trống
                        new FeatureCollectionModel(), // Lòng đường trống
                        new FeatureCollectionModel([geometry])); // Tâm thửa đất với geometry đã tạo
                },
                // Xử lý khi có lỗi
                ex =>
                {
                    logger.Error(ex, "{Email} Lỗi khi lấy thông tin toạ độ thửa: {Url} {Message}",
                        email,
                        url,
                        ex.Message);
                    throw ex;
                });
        }
    }
    
    /// <summary>
    /// Định nghĩa các endpoint API cho tính năng lấy thông tin tọa độ thửa đất.
    /// </summary>
    public class ThuaDatEndpoint : ICarterModule
    {
        /// <summary>
        /// Cấu hình các route cho tính năng lấy thông tin tọa độ thửa đất.
        /// </summary>
        /// <param name="app">Đối tượng xây dựng endpoint.</param>
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet(ThuaDatUri.GetToaDo, async (ISender sender, string serial, string? soDinhDanh = null) =>
                {
                    // Không cần try-catch ở đây vì đã có middleware xử lý exception toàn cục
                    var response = await sender.Send(new Query(serial, soDinhDanh));
                    return Results.Ok(response);
                })
                .RequireAuthorization() // Yêu cầu xác thực người dùng
                .WithTags("ThuaDat"); // Gắn tag để nhóm API trong Swagger
        }
    }
}