using System.Security.Claims;
using System.Text.Json;
using Haihv.Elis.Tool.TraCuuGcn.Models;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;

/// <summary>
/// Lớp mở rộng cung cấp các phương thức bổ sung cho đối tượng ClaimsPrincipal
/// </summary>
/// <remarks>
/// Lớp này cung cấp các phương thức tiện ích để truy xuất thông tin người dùng và quyền
/// từ đối tượng ClaimsPrincipal trong hệ thống xác thực
/// </remarks>
public static class ClaimsPrincipalExtensions
{
    /// <summary>
    /// Lấy danh sách vai trò (roles) của người dùng từ claims
    /// </summary>
    /// <param name="user">Đối tượng ClaimsPrincipal của người dùng</param>
    /// <returns>Danh sách chuỗi chứa các vai trò của người dùng</returns>
    private static List<string> GetUserRoles(this ClaimsPrincipal user) 
    {
        // Lấy giá trị claim realm_access chứa thông tin về roles
        var roles = user.FindFirstValue("realm_access");
        if (roles == null) return [];
    
        // Chuyển đổi chuỗi JSON thành đối tượng
        var rolesObject = JsonSerializer.Deserialize<JsonElement>(roles);
        
        // Nếu có thuộc tính "roles", lấy các giá trị từ mảng
        if (rolesObject.TryGetProperty("roles", out var rolesArray))
        {
            return rolesArray.EnumerateArray()
                .Select(r => r.GetString())
                .Where(r => r != null)
                .ToList()!;
        }
    
        return [];
    }
    
    /// <summary>
    /// Trích xuất và tạo đối tượng UserInfo từ thông tin claims của người dùng
    /// </summary>
    /// <param name="user">Đối tượng ClaimsPrincipal của người dùng</param>
    /// <returns>Đối tượng UserInfo chứa thông tin người dùng</returns>
    public static UserInfo GetUserInfo(this ClaimsPrincipal user)
    {
        return new UserInfo
        {
            DisplayName = user.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname") ?? string.Empty,
            PreferredUsername = user.FindFirstValue("preferred_username") ?? string.Empty,
            Email = user.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress") ?? string.Empty,
            EmailVerified = user.FindFirstValue("email_verified") == "true",
            Name = user.FindFirstValue("name") ?? string.Empty,
            Surname = user.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname") ?? string.Empty,
            Roles = user.GetUserRoles()
        };
    }
    
    public static string GetEmail(this ClaimsPrincipal user, bool isLocalUser = true)
    {
        var email = user.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
        if (string.IsNullOrWhiteSpace(email)) return "Khách";
        // Người dùng nội bộ với email có đuôi là defaultDomain trả về thông tin đầy đủ
        if (isLocalUser) return email;
        var emailParts = email.Split('@');
        if (emailParts.Length < 2) return email;
        var nameEmailPart = emailParts[0];
        var domainEmailPart = emailParts[1];
        
        // Chỉ giữ lại 3 ký tự đầu tiên của nameEmailPart và thêm vào "***"
        if (nameEmailPart.Length > 3)
        {
            nameEmailPart = nameEmailPart[..3] + "***";
        }
        
        // Loại bỏ 3 ký tự đầu của domainEmailPart và thêm vào "***"
        if (domainEmailPart.Length > 3)
        {
            domainEmailPart = "***" + domainEmailPart[3..];
        }
        // trả về email đã được chuẩn hóa
        return $"{nameEmailPart}@{domainEmailPart}";
    }


    /// <summary>
    /// Kiểm tra người dùng có quyền/vai trò được chỉ định hay không
    /// </summary>
    /// <param name="user">Đối tượng ClaimsPrincipal của người dùng</param>
    /// <param name="role">Tên vai trò cần kiểm tra</param>
    /// <returns>True nếu người dùng có vai trò đó, ngược lại False</returns>
    public static bool HasPermission(this ClaimsPrincipal user, string role)
    {
        return user.GetUserRoles().Contains(role);
    }
    
}