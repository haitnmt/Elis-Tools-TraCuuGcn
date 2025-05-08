using System.Security.Claims;
using Haihv.Elis.Tool.TraCuuGcn.Api.Exceptions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Settings;
using Microsoft.Extensions.Caching.Hybrid;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Services;

/// <summary>
/// Interface định nghĩa các dịch vụ phân quyền cho hệ thống tra cứu Giấy chứng nhận
/// </summary>
/// <remarks>
/// Interface này cung cấp các phương thức để kiểm tra quyền truy cập và cập nhật 
/// của người dùng đối với Giấy chứng nhận
/// </remarks>
public interface IPermissionService
{
    /// <summary>
    /// Kiểm tra người dùng có phải là người dùng nội bộ (local) hay không
    /// </summary>
    /// <param name="user">Đối tượng ClaimsPrincipal chứa thông tin người dùng</param>
    /// <returns>True nếu là người dùng nội bộ, ngược lại là False</returns>
    bool IsLocalUser(ClaimsPrincipal user);
    
    /// <summary>
    /// Kiểm tra người dùng có quyền cập nhật Giấy chứng nhận hay không
    /// </summary>
    /// <param name="user">Đối tượng ClaimsPrincipal chứa thông tin người dùng</param>
    /// <param name="serial">Số Serial của Giấy chứng nhận cần kiểm tra quyền</param>
    /// <param name="cancellationToken">Token hủy thao tác (tùy chọn)</param>
    /// <returns>Task kết quả trả về True nếu có quyền cập nhật, ngược lại là False</returns>
    /// <exception cref="NoSerialException">Ném ra khi số Serial không hợp lệ</exception>
    /// <exception cref="NoUpdateGiayChungNhanException">Ném ra khi Giấy chứng nhận đã được ký</exception>
    Task<bool> HasUpdatePermission(ClaimsPrincipal user, string? serial, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kiểm tra người dùng có quyền cập nhật Giấy chứng nhận hay không
    /// </summary>
    /// <param name="user">Đối tượng ClaimsPrincipal chứa thông tin người dùng</param>
    /// <returns>Task kết quả trả về True nếu có quyền cập nhật, ngược lại là False</returns>
    bool HasUpdatePermission(ClaimsPrincipal user);
    
    /// <summary>
    /// Kiểm tra người dùng có quyền đọc thông tin Giấy chứng nhận hay không
    /// </summary>
    /// <param name="user">Đối tượng ClaimsPrincipal chứa thông tin người dùng</param>
    /// <param name="serial">Số Serial của Giấy chứng nhận cần kiểm tra quyền</param>
    /// <param name="soDinhDanh">Số định danh của người dùng cần kiểm tra quyền</param>
    /// <param name="cancellationToken">Token hủy thao tác (tùy chọn)</param>
    /// <returns>Task kết quả trả về True nếu có quyền đọc, ngược lại là False</returns>
    /// <exception cref="NoSerialException">Ném ra khi số Serial không hợp lệ</exception>
    Task<bool> HasReadPermission(ClaimsPrincipal user, string? serial = null, string? soDinhDanh = null, CancellationToken cancellationToken = default);
}

/// <summary>
/// Lớp triển khai dịch vụ phân quyền cho hệ thống tra cứu Giấy chứng nhận
/// </summary>
/// <remarks>
/// Lớp này cung cấp các phương thức để kiểm tra và xác thực quyền của người dùng
/// khi truy cập và thao tác với Giấy chứng nhận
/// </remarks>
/// <param name="configuration">Đối tượng cấu hình ứng dụng</param>
/// <param name="hybridCache">Dịch vụ cache hỗn hợp</param>
/// <param name="connectionElisData">Dịch vụ kết nối ELIS Data</param>
/// <param name="giayChungNhanService">Dịch vụ quản lý Giấy chứng nhận</param>
/// <param name="chuSuDungService">Dịch vụ quản lý thông tin chủ sở hữu</param>
public class PermissionService(IConfiguration configuration, 
    HybridCache hybridCache,
    IConnectionElisData connectionElisData,
    IGiayChungNhanService giayChungNhanService,
    IChuSuDungService chuSuDungService) : IPermissionService
{
    /// <summary>
    /// Tên vai trò của người dùng nội bộ, được lấy từ cấu hình ứng dụng
    /// </summary>
    private readonly string _roleLocal = configuration.GetValue<string>("OpenIdConnect:RoleLocalUser") ?? "Domain Users";

    /// <summary>
    /// Kiểm tra người dùng có phải là người dùng nội bộ (local) hay không
    /// </summary>
    /// <param name="user">Đối tượng ClaimsPrincipal chứa thông tin người dùng</param>
    /// <returns>True nếu là người dùng nội bộ, ngược lại là False</returns>
    public bool IsLocalUser(ClaimsPrincipal user)
    {
        if (user.HasPermission(_roleLocal)) return true;
        // Kiểm tra có phải là API token hay không
        var tokenName = user.FindFirstValue(ClaimTypes.Name);
        return tokenName?.Equals("ApiTokenUser", StringComparison.OrdinalIgnoreCase) == true;
    }
    
    
    /// <summary>
    /// Kiểm tra người dùng có quyền cập nhật Giấy chứng nhận hay không
    /// </summary>
    /// <param name="user">Đối tượng ClaimsPrincipal chứa thông tin người dùng</param>
    /// <param name="serial">Số Serial của Giấy chứng nhận cần kiểm tra quyền</param>
    /// <param name="cancellationToken">Token hủy thao tác (tùy chọn)</param>
    /// <returns>Task kết quả trả về True nếu có quyền cập nhật, ngược lại là False</returns>
    /// <exception cref="NoSerialException">Ném ra khi số Serial không hợp lệ</exception>
    /// <exception cref="NoUpdateGiayChungNhanException">Ném ra khi Giấy chứng nhận đã được ký</exception>
    public async Task<bool> HasUpdatePermission(ClaimsPrincipal user, string? serial, CancellationToken cancellationToken = default)
    {
        // Kiểm tra số Serial có hợp lệ hay không
        if (string.IsNullOrWhiteSpace(serial))
            throw new NoSerialException();
        
        // Lấy tên nhóm có quyền cập nhật Giấy chứng nhận
        var groupName = await connectionElisData.GetUpdateGroupName(serial, cancellationToken);
        
        // Kiểm tra tên nhóm có hợp lệ và người dùng có thuộc nhóm đó không
        if (string.IsNullOrWhiteSpace(groupName) || !user.HasPermission(groupName))
            return false;
        
        // Kiểm tra trạng thái ký của Giấy chứng nhận
        var giayChungNhan = await giayChungNhanService.GetAsync(serial, cancellationToken: cancellationToken);
        
        // Cho phép cập nhật nếu Giấy chứng nhận chưa được ký hoặc ngày ký không hợp lệ
        if (giayChungNhan is null || giayChungNhan.NgayKy <= new DateTime(1990, 1, 1)) return true;
        
        // Ném ra ngoại lệ nếu Giấy chứng nhận đã được ký
        throw new NoUpdateGiayChungNhanException(serial);
    }
    /// <summary>
    /// Kiểm tra người dùng có quyền cập nhật Giấy chứng nhận hay không
    /// </summary>
    /// <param name="user">Đối tượng ClaimsPrincipal chứa thông tin người dùng</param>
    /// <returns>Task kết quả trả về True nếu có quyền cập nhật, ngược lại là False</returns>
    public bool HasUpdatePermission(ClaimsPrincipal user)
    {
        // Lấy tên nhóm có quyền cập nhật Giấy chứng nhận
        var groupNamesList = connectionElisData.GetUpdateGroupName().ToList();
        return groupNamesList.Any(user.HasPermission);
    }
    
    /// <summary>
    /// Kiểm tra người dùng có quyền đọc thông tin Giấy chứng nhận hay không
    /// </summary>
    /// <param name="user">Đối tượng ClaimsPrincipal chứa thông tin người dùng</param>
    /// <param name="serial">Số Serial của Giấy chứng nhận cần kiểm tra quyền</param>
    /// <param name="soDinhDanh">Số định danh của người dùng cần kiểm tra quyền</param>
    /// <param name="cancellationToken">Token hủy thao tác (tùy chọn)</param>
    /// <returns>Task kết quả trả về True nếu có quyền đọc, ngược lại là False</returns>
    /// <exception cref="NoSerialException">Ném ra khi số Serial không hợp lệ</exception>
    public async Task<bool> HasReadPermission(ClaimsPrincipal user, string? serial = null, string? soDinhDanh = null, CancellationToken cancellationToken = default)
    {
        // Kiểm tra số Serial có hợp lệ hay không
        if (string.IsNullOrWhiteSpace(serial)) throw new NoSerialException();
        // Người dùng nội bộ luôn có quyền đọc
        if (IsLocalUser(user)) return true;
        var email = user.GetEmail();
        var cacheKey = CacheSettings.KeyHasReadPermission(email, serial);
        List<string> tags = [email, serial];
        return await hybridCache.GetOrCreateAsync(cacheKey,
            cancel => CheckFirstHasReadPermission( email, serial, soDinhDanh, cancel),
            tags: tags, 
            cancellationToken: cancellationToken);
    }

    private const int MaxCount = 5;

    /// <summary>
    /// Kiểm tra quyền đọc thông tin Giấy chứng nhận
    /// </summary>
    /// <param name="email">Địa chỉ email của người dùng</param>
    /// <param name="serial">Số Serial của Giấy chứng nhận.</param>
    /// <param name="soDinhDanh">Số định danh của người dùng.</param>
    /// <param name="cancellationToken">Token hủy thao tác (tùy chọn)</param>
    /// <returns>True nếu có quyền đọc, ngược lại là False</returns>
    private async ValueTask<bool> CheckFirstHasReadPermission(string email, string serial,
        string? soDinhDanh = null, CancellationToken cancellationToken = default)
    {
        // Nếu không có số định danh, không cho phép đọc
        if (string.IsNullOrWhiteSpace(soDinhDanh)) return false;
        
        // Lấy danh sách chủ sở dụng của Giấy chứng nhận
        var chuSuDungs = await chuSuDungService.GetAsync(serial, cancellationToken);
        
        // Nếu không có chủ sở hữu nào, không cho phép đọc
        if (chuSuDungs.Count == 0) return false;
        
        // Chuẩn hóa số định danh để so sánh
        soDinhDanh = soDinhDanh.Trim();
        
        // Kiểm tra số định danh có thuộc danh sách chủ sở hữu không
        if (!chuSuDungs.Any(chuSuDung =>
                chuSuDung.SoDinhDanh.Trim().Equals(soDinhDanh, StringComparison.OrdinalIgnoreCase) ||
                chuSuDung.SoDinhDanh2.Trim().Equals(soDinhDanh, StringComparison.OrdinalIgnoreCase)))
        {
            _ = GetOrSetReadLock(email, setLock: true, cancellationToken: cancellationToken);
            return false;
        }
        _ = GetOrSetReadLock(email, clearLock: true, cancellationToken: cancellationToken);
        return await CheckReadCount(email, cancellationToken);

        // Không tìm thấy số định danh trong danh sách chủ sở hữu
    }

    private async Task<bool> CheckReadCount(string email, CancellationToken cancellationToken = default)
    {
        // Lấy số lần đọc từ cache
        var key = CacheSettings.KeyReadCount(email);
        var count = await hybridCache.GetOrCreateAsync(key, _ => ValueTask.FromResult(0), cancellationToken: cancellationToken);
        // Nếu số lần đọc lớn hơn hoặc bằng MaxCount, không cho phép đọc
        if (count >= MaxCount)
            return false;
        // Tăng số lần đọc lên 1
        var cacheOption = new HybridCacheEntryOptions
        {
            Expiration = TimeSpan.FromDays(1),
        };
        await hybridCache.SetAsync(key, count + 1, 
            options: cacheOption, 
            tags: [email], 
            cancellationToken: cancellationToken);
        return true;
    }

    private async Task<bool> GetOrSetReadLock(string email, bool setLock = false, bool clearLock = false, CancellationToken cancellationToken = default)
    {
        var key = CacheSettings.KeyReadLock(email);
        var timeSpan = await hybridCache.GetOrCreateAsync(key, _ => ValueTask.FromResult(-1500), cancellationToken: cancellationToken);
        if (setLock)
            timeSpan += 300; // Thời gian khóa là 5 phút
        else if (clearLock)
            timeSpan = -1500;
        // Nếu thời gian khóa nhỏ hơn hoặc bằng 0, không cho phép đọc
        var cacheOption = new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromSeconds(Math.Abs(timeSpan))
            };
        await hybridCache.SetAsync(key, timeSpan, 
            options: cacheOption, 
            tags: [email], 
            cancellationToken: cancellationToken);
        return timeSpan > 0;
    }
}