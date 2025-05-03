using System.Net.Sockets;

namespace Haihv.Elis.Tool.TraCuuGcn.WebApp.Extensions;

/// <summary>
/// Các phương thức mở rộng cho HttpContext để truy xuất thông tin IP.
/// </summary>
public static class HttpContextExtensions
{
    /// <summary>
    /// Lấy thông tin IP của người dùng và xác định xem đó có phải là địa chỉ IP riêng tư hay không.
    /// </summary>
    /// <param name="httpContext">Context HTTP hiện tại</param>
    /// <returns>Tuple chứa địa chỉ IP dưới dạng chuỗi và cờ boolean cho biết IP có phải là địa chỉ riêng tư không</returns>
    public static (string IpAddress, bool IsPrivate) GetIpInfo(this HttpContext httpContext)
    {
        var ipAddress = httpContext.GetIpAddress();

        var isPrivate = false;
        // Kiểm tra xem đây có phải là IP riêng tư và đánh dấu nếu được yêu cầu
        if (string.IsNullOrWhiteSpace(ipAddress)) return (ipAddress, isPrivate);
        if (!System.Net.IPAddress.TryParse(ipAddress, out var parsedIp)) return (ipAddress, isPrivate);
        var bytes = parsedIp.GetAddressBytes();

        isPrivate = parsedIp.AddressFamily switch
        {
            // Kiểm tra các dải IP riêng tư cho IPv4
            // Kiểm tra các dải IP riêng tư IPv4: 10.0.0.0/8, 172.16.0.0/12, 192.168.0.0/16, 127.0.0.0/8
            AddressFamily.InterNetwork => bytes[0] == 10 ||
                                       (bytes[0] == 172 && bytes[1] >= 16 && bytes[1] <= 31) ||
                                       (bytes[0] == 192 && bytes[1] == 168) ||
                                       bytes[0] == 127,

            // Đối với IPv6, kiểm tra xem có phải là địa chỉ loopback hoặc link-local
            AddressFamily.InterNetworkV6 => parsedIp.IsIPv6LinkLocal ||
                                       parsedIp.IsIPv6SiteLocal ||
                                       parsedIp.Equals(System.Net.IPAddress.IPv6Loopback),
            // Tất cả các loại địa chỉ khác không được coi là riêng tư
            _ => false
        };
        return (ipAddress, isPrivate);
    }

    /// <summary>
    /// Lấy địa chỉ IP của người dùng từ các header HTTP khác nhau.
    /// </summary>
    /// <param name="httpContext">Context HTTP hiện tại</param>
    /// <returns>Địa chỉ IP dưới dạng chuỗi hoặc "Unknown" nếu không tìm thấy</returns>
    /// <remarks>
    /// Phương thức này kiểm tra các header theo thứ tự ưu tiên:
    /// 1. CF-Connecting-IP (Cloudflare)
    /// 2. True-Client-IP (Cloudflare thay thế)
    /// 3. X-Original-For (HAProxy)
    /// 4. X-Forwarded-For (Header proxy thông thường)
    /// 5. X-Real-IP (Nginx)
    /// 6. REMOTE_ADDR (Dự phòng)
    /// Nếu không tìm thấy IP trong các header, sẽ sử dụng RemoteIpAddress từ kết nối.
    /// </remarks>
    public static string GetIpAddress(this HttpContext httpContext)
    {
        var ipAddress = string.Empty;

        // Sắp xếp các header theo thứ tự ưu tiên chuỗi proxy (từ proxy cuối cùng đến đầu tiên)
        var headerKeys = new[]
        {
            "CF-Connecting-IP",   // Ưu tiên cao nhất - IP khách hàng nguyên gốc từ Cloudflare
            "True-Client-IP",     // Header thay thế của Cloudflare
            "X-Original-For",     // HAProxy
            "X-Forwarded-For",    // Header proxy thông thường (sẽ chứa chuỗi các IP)
            "X-Real-IP",          // Nginx
            "REMOTE_ADDR"         // Dự phòng
        };

        foreach (var headerKey in headerKeys)
        {
            if (!httpContext.Request.Headers.TryGetValue(headerKey, out var headerValue)) continue;
            ipAddress = headerValue.FirstOrDefault()?.Split(',')[0].Trim();
            if (!string.IsNullOrWhiteSpace(ipAddress))
                break;
        }

        // Sử dụng RemoteIpAddress nếu không tìm thấy header proxy
        if (string.IsNullOrWhiteSpace(ipAddress) && httpContext.Connection.RemoteIpAddress != null)
        {
            ipAddress = httpContext.Connection.RemoteIpAddress.ToString();
        }
        return string.IsNullOrWhiteSpace(ipAddress) ? "Unknown" : ipAddress;
    }
}