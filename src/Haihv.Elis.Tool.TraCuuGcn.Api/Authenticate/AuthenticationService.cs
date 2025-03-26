using System.Globalization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Services;
using Haihv.Elis.Tool.TraCuuGcn.Api.Settings;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.Net.Http.Headers;
using ZiggyCreatures.Caching.Fusion;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Authenticate;

public interface IAuthenticationService
{
    /// <summary>
    /// Kiểm tra xác thực.
    /// </summary>
    /// <param name="serial"> Số Serial của GCN.</param>
    /// <param name="claimsPrincipal">Thông tin xác thực của người dùng.</param>
    /// <param name="cancellationToken">Token hủy bỏ.</param>
    /// <returns>Trả về Số định danh nếu thành công hoặc null nếu không thành công.</returns>
    ValueTask<string?> CheckAuthenticationAsync(string? serial, ClaimsPrincipal? claimsPrincipal = null,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Xác thực chủ sử dụng.
    /// </summary>
    /// <param name="authChuSuDung">Thông tin xác thực chủ sử dụng.</param>
    /// <returns>Trả về thông tin token nếu thành công hoặc lỗi nếu không thành công.</returns>
    ValueTask<Result<AccessToken>> AuthChuSuDungAsync(AuthChuSuDung? authChuSuDung);
    
}

public sealed class AuthenticationService(
    IChuSuDungService chuSuDungService,
    IFusionCache fusionCache,
    TokenProvider tokenProvider) : IAuthenticationService
{
    /// <summary>
    /// Kiểm tra xác thực.
    /// </summary>
    /// <param name="serial"> Số Serial của GCN.</param>
    /// <param name="claimsPrincipal">Thông tin xác thực của người dùng.</param>
    /// <param name="cancellationToken">Token hủy bỏ.</param>
    /// <returns>Trả về Số định danh nếu thành công hoặc null nếu không thành công.</returns>
    public async ValueTask<string?> CheckAuthenticationAsync(string? serial, ClaimsPrincipal? claimsPrincipal = null,
        CancellationToken cancellationToken = default)
    {
        if (claimsPrincipal is null || string.IsNullOrWhiteSpace(serial)) return null;
        var typeIdentity = claimsPrincipal.GetIdentityType();
        var maDinhDanh = claimsPrincipal.GetMaDinhDanh();
        if (typeIdentity?.ToLower() == "ldap")
        {
            return maDinhDanh;
        }
        if (string.IsNullOrWhiteSpace(maDinhDanh)) return null;
        serial = serial.ChuanHoa();
        if (string.IsNullOrWhiteSpace(serial)) return null;
        var tenChuSuDung = await fusionCache.GetOrDefaultAsync<string>(CacheSettings.KeyAuthentication(serial, maDinhDanh),
            token: cancellationToken);
        if (!string.IsNullOrWhiteSpace(tenChuSuDung) && CompareVietnameseStrings(tenChuSuDung, claimsPrincipal.GetHoVaTen()))
        {
            return maDinhDanh;
        }
        var chuSuDungResult =
            await chuSuDungService.GetResultAuthChuSuDungAsync(serial, maDinhDanh, cancellationToken);
        return chuSuDungResult.Match(
            csd => 
                !CompareVietnameseStrings(csd.HoVaTen, claimsPrincipal.GetHoVaTen()) ? null : maDinhDanh,
            _ => null);
    }
    
    /// <summary>
    /// Xác thực chủ sử dụng.
    /// </summary>
    /// <param name="authChuSuDung">Thông tin xác thực chủ sử dụng.</param>
    /// <returns>Trả về thông tin token nếu thành công hoặc lỗi nếu không thành công.</returns>
    public async ValueTask<Result<AccessToken>> AuthChuSuDungAsync(AuthChuSuDung? authChuSuDung)
    {
        if (authChuSuDung is null)
            return new Result<AccessToken>(new ValueIsNullException("Thông tin xác thực không hợp lệ!"));
        var soDinhDanh = authChuSuDung.SoDinhDanh;
        var serial = authChuSuDung.Serial.ChuanHoa();
        var hoTen = authChuSuDung.HoVaTen;
        if (string.IsNullOrWhiteSpace(soDinhDanh) || string.IsNullOrWhiteSpace(serial) ||
            string.IsNullOrWhiteSpace(hoTen))
            return new Result<AccessToken>(new ValueIsNullException("Thông tin xác thực không hợp lệ!"));
        var chuSuDung = await chuSuDungService.GetResultAuthChuSuDungAsync(serial, soDinhDanh);
        return chuSuDung.Match(
            csd =>
            {
                if (!CompareVietnameseStrings(csd.HoVaTen, hoTen))
                    return new Result<AccessToken>(new ValueIsNullException("Thông tin xác thực không hợp lệ!"));
                var accessToken = tokenProvider.GenerateToken(authChuSuDung);
                return new Result<AccessToken>(accessToken);
            },
            _ => new Result<AccessToken>(new ValueIsNullException("Không tìm thấy chủ sử dụng!")));
    }

    /// <summary>
    /// So sánh hai chuỗi tiếng Việt không phân biệt hoa thường và dấu.
    /// </summary>
    /// <param name="str1">Chuỗi thứ nhất.</param>
    /// <param name="str2">Chuỗi thứ hai.</param>
    /// <returns>Trả về true nếu hai chuỗi giống nhau, ngược lại trả về false.</returns>
    private static bool CompareVietnameseStrings(string? str1, string? str2)
    {
        if (string.IsNullOrWhiteSpace(str1) || string.IsNullOrWhiteSpace(str2)) return false;
        str1 = RemoveDiacritics(str1);
        str2 = RemoveDiacritics(str2);
        // So sánh không phân biệt hoa thường
        return string.Equals(str1, str2, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Loại bỏ các dấu tiếng Việt và ký tự đặc biệt khỏi chuỗi.
    /// </summary>
    /// <param name="input">Chuỗi đầu vào cần loại bỏ dấu.</param>
    /// <returns>Chuỗi đã được loại bỏ dấu.</returns>
    private static string RemoveDiacritics(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        // Chuyển chuỗi về dạng chuẩn NFD
        var normalized = input.Normalize(NormalizationForm.FormD);

        // Loại bỏ các ký tự không phải ký tự cơ bản (dấu) và ký tự không phải chữ, số
        return new string(normalized
            .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark &&
                        char.IsLetterOrDigit(c))
            .ToArray());
    }
}

public static class AuthenticationExtensions
{
    /// <summary>
    /// Kiểm tra xác thực LDAP.
    /// </summary>
    /// <param name="claimsPrincipal"></param>
    /// <returns>
    /// Trả về true nếu xác thực LDAP, ngược lại trả về false.
    /// </returns>
    public static bool IsLdap(this ClaimsPrincipal claimsPrincipal)
        =>claimsPrincipal.GetIdentityType()?.ToLower() == "ldap";

    public static async Task<bool> HasUpdatePermission(this HttpContext httpContext, string urlLdapApi, string groupName)
    {
        if (!httpContext.User.IsLdap())
            return false;
        // Lấy Token Access trong Header
        var token = httpContext.Request.Headers[HeaderNames.Authorization].ToString();
        if (string.IsNullOrWhiteSpace(token))
            return false;
        // Kiểm tra quyền từ Api xác thực:
        var httpClient = new HttpClient
        {
            BaseAddress = new Uri(urlLdapApi),
        };
        // Bổ sung Token vào Header
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("Bearer ",""));
        var response = await httpClient.GetAsync($"user/checkGroup?groupName={groupName}");
        return response.IsSuccessStatusCode;
    }
}