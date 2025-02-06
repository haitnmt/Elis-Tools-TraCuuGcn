using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;

public class BarcodeScannerService(IJSRuntime jsRuntime) : IAsyncDisposable
{
    private bool _isInitialized;
    private bool _hasCamera;
    /// <summary>
    /// Khởi tạo dịch vụ, kiểm tra camera có sẵn hay không,
    /// và import file barcodeScannerInterop.js 
    /// (chứa các hàm quét bằng QuaggaJS/jsQR).
    /// </summary>
    public async Task InitializeAsync()
    {
        if (_isInitialized) return;

        // // Import QuaggaJS 
        // await jsRuntime.InvokeAsync<object>("import", "./_content/Haihv.Elis.Tool.TraCuuGcn.WebLib/quagga.min.js");
        //
        // // Ensure Quagga is defined
        // await jsRuntime.InvokeVoidAsync("eval", "if (typeof Quagga === 'undefined') { throw new Error('Quagga is not defined'); }");

        // Import jsQR 
        await jsRuntime.InvokeAsync<object>("import", "./_content/Haihv.Elis.Tool.TraCuuGcn.WebLib/jsQR.js");

        // Import file JavaScript chứa hàm quét
        await jsRuntime.InvokeAsync<object>("import", "./_content/Haihv.Elis.Tool.TraCuuGcn.WebLib/barcodeScannerInterop.js");

        // Kiểm tra có camera hay không
        _hasCamera = await jsRuntime.InvokeAsync<bool>("checkCameraAvailability");
        _isInitialized = true;
    }

    /// <summary>
    /// Trả về true nếu tìm thấy camera trên thiết bị.
    /// </summary>
    public bool HasCamera() => _hasCamera;

    /// <summary>
    /// Quét ảnh tải lên (IBrowserFile) để tìm QR Code trước, nếu không thấy thì kiểm tra Code128.
    /// Sử dụng jsQR cho QR Code, QuaggaJS cho Code128. 
    /// </summary>
    public async Task<string> ScanFromImage(IBrowserFile uploadedImage)
    {
        ArgumentNullException.ThrowIfNull(uploadedImage);

        await using var stream = uploadedImage.OpenReadStream();
        await using var ms = new MemoryStream();
        await stream.CopyToAsync(ms);
        var imageBytes = ms.ToArray();
        var imageDataUrl = $"data:{uploadedImage.ContentType};base64,{Convert.ToBase64String(imageBytes)}";

        // Thử quét QR Code (jsQR)
        return await jsRuntime.InvokeAsync<string>("scanImageForQr", imageDataUrl);
        try
        {
            return await jsRuntime.InvokeAsync<string>("scanImageForQr", imageDataUrl);
        }
        catch
        {
            // Nếu không thấy QR Code, thử Code128 (QuaggaJS)
            return await jsRuntime.InvokeAsync<string>("scanImageForCode128", imageDataUrl);
        }

    }

    /// <summary>
    /// Bắt đầu quét Code128 bằng camera (QuaggaJS).
    /// Kết quả quét trả về qua callback C# "HandleScanResult"
    /// trong DotNetObjectReference (ví dụ: SearchBar).
    /// 
    /// Chú ý: Nếu muốn quét QR Code bằng camera,
    /// dùng hàm startCameraScanQr bên phía JavaScript.
    /// </summary>
    public async Task StartCameraScanCode128(ElementReference videoElement, DotNetObjectReference<SearchBar> dotNetObjectReference)
    {
        // Gọi hàm quét Code128 trong barcodeScannerInterop.js
        await jsRuntime.InvokeVoidAsync(
            "startCameraScanCode128",
            videoElement.Id,
            dotNetObjectReference);
    }

    /// <summary>
    /// Bắt đầu quét QR Code bằng camera (jsQR).
    /// Kết quả trả về qua callback "HandleScanResult".
    /// </summary>
    public async Task StartCameraScanQr(ElementReference videoElement, DotNetObjectReference<SearchBar> dotNetObjectReference)
    {
        // Gọi hàm quét QR Code trong barcodeScannerInterop.js
        await jsRuntime.InvokeVoidAsync(
            "startCameraScanQr",
            videoElement.Id,
            dotNetObjectReference);
    }

    /// <summary>
    /// Dừng quét Code128 (QuaggaJS).
    /// Nếu đang quét QR Code (jsQR), việc dừng
    /// sẽ do track.stop() tự xử lý khi tìm thấy mã.
    /// </summary>
    public async Task StopCameraScanCode128()
    {
        await jsRuntime.InvokeVoidAsync("stopCameraScanCode128");
    }
    /// <summary>
    /// Dừng quét QR Code (jsQR).
    /// </summary>
    public async Task StopCameraScanQr()
    {
        await jsRuntime.InvokeVoidAsync("stopCameraScanQr");
    }

    /// <summary>
    /// Thực thi IAsyncDisposable.
    /// </summary>
    public ValueTask DisposeAsync()
    {
        // Không giữ tham chiếu JS nào, nên không cần giải phóng thêm.
        return ValueTask.CompletedTask;
    }
}