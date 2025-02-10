using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;

public class ZxingService(IJSRuntime jsRuntime) : IAsyncDisposable
{
    private IJSObjectReference? _zxingReader;
    private IJSObjectReference? _cameraStream;
    private bool _isInitialized;
    private bool _hasCamera;
    //private const string UrlZxingJs = "./_content/Haihv.Elis.Tool.TraCuuGcn.WebLib/zxing.min.js";
    private const string UrlZxingJs = "https://unpkg.com/@zxing/library@latest/umd/index.min.js";
    private readonly string _urlBarcodeScannerJs = "./_content/Haihv.Elis.Tool.TraCuuGcn.WebLib/barcodeScannerZxing.js" + "?v=" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
    public async Task InitializeAsync()
    {
        if (_isInitialized) return;
        await jsRuntime.InvokeAsync<IJSObjectReference>("import", UrlZxingJs);
        await jsRuntime.InvokeAsync<IJSObjectReference>("import", _urlBarcodeScannerJs);
        _hasCamera = await CheckCameraAvailability();
        _isInitialized = true;
    }

    public bool HasCamera() => _hasCamera;
    
    private async Task<bool> CheckCameraAvailability()
    {
        return await jsRuntime.InvokeAsync<bool>("checkCameraAvailability");
    }
    public async Task<string> ScanFromImage(IBrowserFile uploadedImage)
    {
        ArgumentNullException.ThrowIfNull(uploadedImage);

        await using var stream = uploadedImage.OpenReadStream();
        await using var ms = new MemoryStream();
        await stream.CopyToAsync(ms);
        var imageBytes = ms.ToArray();
        var imageDataUrl = $"data:{uploadedImage.ContentType};base64,{Convert.ToBase64String(imageBytes)}";

        return await jsRuntime.InvokeAsync<string>("scanFromImage", imageDataUrl);
    }
    
    public async Task StartCameraScan(ElementReference videoElement, DotNetObjectReference<SearchBar> dotNetObjectReference)
    {
        _zxingReader ??= await jsRuntime.InvokeAsync<IJSObjectReference>("initZxingReader");
        
        _cameraStream = await jsRuntime.InvokeAsync<IJSObjectReference>(
            "startCameraScan",
            _zxingReader,
            videoElement,
            dotNetObjectReference);

    }

    public async Task StopCameraScan()
    {
        if (_cameraStream != null)
        {
            await jsRuntime.InvokeVoidAsync("stopCameraScan", _cameraStream);
            await _cameraStream.DisposeAsync();
        }
    }
    public async ValueTask DisposeAsync()
    {
        if (_zxingReader != null)
        {
            await _zxingReader.DisposeAsync();
        }
    }
}