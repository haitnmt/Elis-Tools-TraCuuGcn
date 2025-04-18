using System.Security.Claims;
using Haihv.Elis.Tool.TraCuuGcn.Api.Exceptions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;

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
/// <param name="connectionElisData">Dịch vụ kết nối ELIS Data</param>
/// <param name="giayChungNhanService">Dịch vụ quản lý Giấy chứng nhận</param>
/// <param name="chuSuDungService">Dịch vụ quản lý thông tin chủ sở hữu</param>
public class PermissionService(IConfiguration configuration, 
    IConnectionElisData connectionElisData,
    IGiayChungNhanService giayChungNhanService,
    IChuSuDungService chuSuDungService) : IPermissionService
{
    /// <summary>
    /// Tên vai trò của người dùng nội bộ, được lấy từ cấu hình ứng dụng
    /// </summary>
    private readonly string _roleLocal = configuration["OpenIdConnect.RoleLocalUser"] ?? "Domain Users";

    /// <summary>
    /// Kiểm tra người dùng có phải là người dùng nội bộ (local) hay không
    /// </summary>
    /// <param name="user">Đối tượng ClaimsPrincipal chứa thông tin người dùng</param>
    /// <returns>True nếu là người dùng nội bộ, ngược lại là False</returns>
    public bool IsLocalUser(ClaimsPrincipal user)
        => user.HasPermission(_roleLocal);
    
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
        
        // Nếu không có số định danh, không cho phép đọc
        if (string.IsNullOrWhiteSpace(soDinhDanh)) return false;
        
        // Lấy danh sách chủ sở hữu của Giấy chứng nhận
        var chuSuDungs = await chuSuDungService.GetInDatabaseAsync(serial, cancellationToken);
        
        // Chuẩn hóa số định danh để so sánh
        soDinhDanh = soDinhDanh.Trim();
        
        // Kiểm tra số định danh có thuộc danh sách chủ sở hữu không
        foreach (var chuSuDung in chuSuDungs)
        {   
            // So sánh số định danh chính
            if (chuSuDung.SoDinhDanh.Trim().Equals(soDinhDanh, StringComparison.OrdinalIgnoreCase))
                return true;
                
            // Kiểm tra nếu số định danh có trong số định danh phụ
            if (chuSuDung.SoDinhDanh2.Trim().Contains(soDinhDanh, StringComparison.OrdinalIgnoreCase))
                return true;
        }
        
        // Không tìm thấy số định danh trong danh sách chủ sở hữu
        return false;
    }
    
}