using Carter;
using MediatR;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Features.AppSetting;

/// <summary>
/// Cung cấp chức năng lấy thông tin phiên bản của ứng dụng.
/// </summary>
public static class GetVersionAsync
{
    /// <summary>
    /// Query để yêu cầu thông tin phiên bản ứng dụng.
    /// Không cần tham số đầu vào.
    /// </summary>
    public record Query : IRequest<string>;

    /// <summary>
    /// Xử lý yêu cầu lấy thông tin phiên bản ứng dụng.
    /// </summary>
    public class Handler : IRequestHandler<Query, string>
    {
        /// <summary>
        /// Xử lý yêu cầu lấy phiên bản ứng dụng.
        /// </summary>
        /// <param name="request">Yêu cầu lấy phiên bản.</param>
        /// <param name="cancellationToken">Token hủy bỏ.</param>
        /// <returns>Chuỗi biểu thị phiên bản hiện tại của ứng dụng.</returns>
        public Task<string> Handle(Query request, CancellationToken cancellationToken)
        {
            // Lấy phiên bản từ assembly đang thực thi
            return Task.FromResult(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version!.ToString());
        }
    }

    /// <summary>
    /// Định nghĩa API endpoint để lấy thông tin phiên bản của ứng dụng.
    /// </summary>
    public class AppSettingEndpoint : ICarterModule
    {
        /// <summary>
        /// Thiết lập các route cho API endpoint.
        /// </summary>
        /// <param name="app">Builder để đăng ký các endpoint.</param>
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/version", async (ISender sender) =>
                {
                    // Không cần try-catch ở đây vì đã có middleware xử lý exception toàn cục
                    var response = await sender.Send(new Query());
                    return Results.Ok(response);
                })
                .WithTags("Setting");
        }
    }
}