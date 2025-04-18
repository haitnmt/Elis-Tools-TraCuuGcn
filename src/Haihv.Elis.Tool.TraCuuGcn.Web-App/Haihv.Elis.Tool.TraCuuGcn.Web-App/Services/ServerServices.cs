using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;

namespace Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;

/// <summary>
/// Lớp trừu tượng cung cấp các dịch vụ giao tiếp với server.
/// Chứa các phương thức chung để thực hiện các yêu cầu HTTP.
/// </summary>
/// <param name="httpClient">Client HTTP để gửi yêu cầu tới server.</param>
/// <param name="httpContextAccessor">Bộ truy cập ngữ cảnh HTTP để lấy thông tin xác thực.</param>
internal abstract class ServerServices(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
{
    /// <summary>
    /// Gửi yêu cầu GET tới đường dẫn xác định và chuyển đổi kết quả trả về thành đối tượng kiểu T.
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu mong muốn của kết quả trả về.</typeparam>
    /// <param name="uri">Đường dẫn API cần gọi.</param>
    /// <returns>Đối tượng kiểu T từ phản hồi JSON, hoặc null nếu không có dữ liệu.</returns>
    /// <exception cref="InvalidOperationException">
    /// Ném ra khi không có HttpContext hoặc không tìm thấy access_token.
    /// </exception>
    /// <exception cref="HttpRequestException">
    /// Ném ra khi phản hồi HTTP không thành công.
    /// </exception>
    protected async Task<T?> GetDataAsync<T>(string uri)
    {
        // Lấy HttpContext từ accessor, ném ngoại lệ nếu không có
        var httpContext = httpContextAccessor.HttpContext ??
                          throw new InvalidOperationException("No HttpContext available from the IHttpContextAccessor!");

        // Lấy access token từ context để xác thực, ném ngoại lệ nếu không tìm thấy
        var accessToken = await httpContext.GetTokenAsync("access_token") ??
                          throw new InvalidOperationException("No access_token was saved");

        // Tạo yêu cầu HTTP GET với URI đã chỉ định
        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
        
        // Thêm header Authorization với Bearer token
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        
        // Gửi yêu cầu và nhận phản hồi
        using var response = await httpClient.SendAsync(requestMessage);

        // Đảm bảo phản hồi thành công, nếu không sẽ ném ngoại lệ
        response.EnsureSuccessStatusCode();

        // Đọc và chuyển đổi nội dung phản hồi JSON thành đối tượng kiểu T
        return await response.Content.ReadFromJsonAsync<T>();
    }
}