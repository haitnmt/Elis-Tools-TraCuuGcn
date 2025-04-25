using System.Text.Json;
using Haihv.Elis.Tool.TraCuuGcn.Models;

namespace Haihv.Elis.Tool.TraCuuGcn.WebApp.Extensions;

public static class ResponseMessageExtension
{
    /// <summary>
    /// Xử lý phản hồi từ API để trích xuất thông báo lỗi
    /// </summary>
    /// <param name="response">Đối tượng HttpResponseMessage từ API</param>
    /// <returns>Thông báo lỗi hoặc null nếu không có lỗi</returns>
    public static async Task<string?> ParseErrorMessageAsync(this HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
            return null;
            
        var contentString = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(contentString))
            return null;
            
        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            
            var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(contentString, options);
            return errorResponse?.GetErrorMessage();
        }
        catch
        {
            // Nếu không phân tích được định dạng JSON, trả về chuỗi gốc
            return contentString;
        }
    }

}