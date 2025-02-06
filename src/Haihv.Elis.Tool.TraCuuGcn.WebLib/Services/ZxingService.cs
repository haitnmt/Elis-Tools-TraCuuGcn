using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace Haihv.Elis.Tool.TraCuuGcn.WebLib.wwwroot;

public class ZxingService(IJSRuntime jsRuntime) : IAsyncDisposable
{
    private IJSObjectReference? _zxingReader;
    private IJSObjectReference? _cameraStream;
    private bool _isInitialized;
    private bool _hasCamera;

    public async Task InitializeAsync()
    {
        if (_isInitialized) return;
        await jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Haihv.Elis.Tool.TraCuuGcn.WebLib/zxing.min.js");
        await jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Haihv.Elis.Tool.TraCuuGcn.WebLib/barcodeScanner.js");
        _zxingReader = await jsRuntime.InvokeAsync<IJSObjectReference>("eval", "new window.ZXing.BrowserMultiFormatReader()");
        _hasCamera = await CheckCameraAvailability();
        _isInitialized = true;
    }

    public bool HasCamera() => _hasCamera;
    
    public IJSObjectReference? GetZxingReader() => _zxingReader;

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

        return await jsRuntime.InvokeAsync<string>("scanFromImage", _zxingReader, imageDataUrl);
    }
    
    public async Task StartCameraScan(ElementReference videoElement, ElementReference _canvasElement, DotNetObjectReference<SearchBar> dotNetObjectReference)
    {
        _cameraStream = await jsRuntime.InvokeAsync<IJSObjectReference>(
            "startCameraScan",
            _zxingReader,
            videoElement,
            _canvasElement,
            dotNetObjectReference);
    }

    public async Task StopCameraScan()
    {
        if (_cameraStream != null)
        {
            await jsRuntime.InvokeVoidAsync("stopCameraScan", _zxingReader);
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