using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;

/// <summary>
/// Dịch vụ đồng bộ trạng thái đăng nhập giữa các tab trình duyệt
/// </summary>
public interface IStorageSyncService
{
    /// <summary>
    /// Khởi tạo dịch vụ lắng nghe sự thay đổi localStorage
    /// </summary>
    Task InitializeAsync();
    
    /// <summary>
    /// Hủy đăng ký lắng nghe sự thay đổi
    /// </summary>
    Task DisposeAsync();
}

public class StorageSyncService(
    IJSRuntime jsRuntime,
    AuthenticationStateProvider authStateProvider)
    : IStorageSyncService, IAsyncDisposable
{
    private DotNetObjectReference<StorageSyncService>? _dotNetRef;
    private IJSObjectReference? _jsModule;

    public async Task InitializeAsync()
    {
        try
        {
            _dotNetRef = DotNetObjectReference.Create(this);
            _jsModule = await jsRuntime.InvokeAsync<IJSObjectReference>("import", 
                "./_content/Haihv.Elis.Tool.TraCuuGcn.WebLib/storageSync/storageSync.js");
            await _jsModule.InvokeVoidAsync("initialize", _dotNetRef);
            
            Console.WriteLine("Đã khởi tạo dịch vụ đồng bộ localStorage giữa các tab");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi khi khởi tạo dịch vụ đồng bộ localStorage: {ex.Message}");
        }
    }

    public async Task DisposeAsync()
    {
        if (_jsModule != null)
        {
            try
            {
                await _jsModule.InvokeVoidAsync("dispose");
                await _jsModule.DisposeAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi hủy dịch vụ đồng bộ localStorage: {ex.Message}");
            }
        }
        
        _dotNetRef?.Dispose();
    }

    /// <summary>
    /// Phương thức được gọi từ JavaScript khi localStorage thay đổi
    /// </summary>
    [JSInvokable]
    public async Task OnStorageChanged(string key, string? newValue, string? oldValue)
    {
        try
        {
            Console.WriteLine($"Phát hiện thay đổi localStorage - Key: {key}, Giá trị mới: {newValue?[..Math.Min(10, newValue.Length)]}...");
            
            if (key == AuthService.AccessTokenKey)
            {
                if (string.IsNullOrEmpty(newValue))
                {
                    // Người dùng đã đăng xuất ở tab khác
                    Console.WriteLine("Phát hiện đăng xuất từ tab khác");
                    ((JwtAuthStateProvider)authStateProvider).NotifyUserChanged(JwtAuthStateProvider.AnonymousUser);
                }
                else if (newValue != oldValue)
                {
                    // Token đã thay đổi (đăng nhập mới hoặc refresh token)
                    Console.WriteLine("Phát hiện thay đổi token từ tab khác");
                    var authState = await ((JwtAuthStateProvider)authStateProvider).GetAuthenticationStateAsync();
                    ((JwtAuthStateProvider)authStateProvider).NotifyUserChanged(authState);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi khi xử lý sự thay đổi localStorage: {ex.Message}");
        }
    }

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        await DisposeAsync();
        GC.SuppressFinalize(this);
    }
}