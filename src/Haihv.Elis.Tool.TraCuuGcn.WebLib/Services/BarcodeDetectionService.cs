using System.Text.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;

public class BarcodeDetectionService(IJSRuntime jsRuntime) : IAsyncDisposable
{
    private IJSObjectReference? _module;

    // Khởi tạo module JavaScript
    private readonly string _urlBarcodeDetectionJs = "./_content/Haihv.Elis.Tool.TraCuuGcn.WebLib/barcodeDetection.js" + "?v=" 
        + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
    public async Task InitializeAsync()
    {
        _module ??= await jsRuntime.InvokeAsync<IJSObjectReference>("import", _urlBarcodeDetectionJs);
    }

    // Kiểm tra xem API có được hỗ trợ không
    public async Task<bool> IsBarcodeDetectionSupportedAsync()
    {
        await InitializeAsync();
        return await jsRuntime.InvokeAsync<bool>("BarcodeDetectionService.isBarcodeDetectionSupported");
    }

    // Kiểm tra xem thiết bị có hỗ trợ camera không
    public async Task<bool> IsCameraAvailableAsync()
    {
        try
        {
            await InitializeAsync();
            return await jsRuntime.InvokeAsync<bool>("BarcodeDetectionService.isCameraAvailable");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
    
    // Lấy danh sách các định dạng được hỗ trợ
    public async Task<string[]> GetSupportedFormatsAsync()
    {
        await InitializeAsync();
        return await jsRuntime.InvokeAsync<string[]>("BarcodeDetectionService.getSupportedFormats");
    }

    // Phát hiện mã vạch từ hình ảnh
    public async Task<BarcodeDetectionResult> DetectFromImageAsync(IBrowserFile uploadedImage, string[]? formats = null)
    {
        await InitializeAsync();
        // Convert the file to data URL
        var dataUrl = await uploadedImage.ToDataUrlAsync();
        var result =
            await jsRuntime.InvokeAsync<JsonElement>("BarcodeDetectionService.detectFromImage", dataUrl,
                formats);
        return ParseResult(result);
    }

    // Phát hiện mã vạch từ video
    public async Task<BarcodeDetectionResult> DetectFromVideoAsync(string videoElementId, string[]? formats = null)
    {
        await InitializeAsync();
        var result =
            await jsRuntime.InvokeAsync<JsonElement>("BarcodeDetectionService.detectFromVideo", videoElementId,
                formats);
        return ParseResult(result);
    }

    // Bắt đầu phát hiện mã vạch trực tiếp
    public async Task<BarcodeDetectionResult> StartLiveDetectionAsync(string videoElementId, 
        DotNetObjectReference<SearchBar> dotNetObjectReference, 
        string resultCallbackName,
        string[]? formats = null)
    {
        await InitializeAsync();
        var result = await jsRuntime.InvokeAsync<JsonElement>("BarcodeDetectionService.startLiveDetection",
            videoElementId, formats, dotNetObjectReference, resultCallbackName);
        return ParseResult(result);
    }

    // Dừng phát hiện mã vạch trực tiếp
    public async Task<BarcodeDetectionResult> StopLiveDetectionAsync()
    {
        await InitializeAsync();
        var result = await jsRuntime.InvokeAsync<JsonElement>("BarcodeDetectionService.stopLiveDetection");
        return ParseResult(result);
    }

    // Chuyển đổi kết quả từ JavaScript sang C#
    private static BarcodeDetectionResult ParseResult(JsonElement result)
    {
        var success = result.GetProperty("success").GetBoolean();
        if (success)
        {
            if (!result.TryGetProperty("results", out var resultsElement))
                return new BarcodeDetectionResult { Success = true, Barcodes = [] };
            var barcodes = new List<BarcodeInfo>();
            foreach (var item in resultsElement.EnumerateArray())
            {
                var barcode = new BarcodeInfo
                {
                    Format = item.GetProperty("format").GetString(),
                    RawValue = item.GetProperty("rawValue").GetString()
                };

                if (item.TryGetProperty("boundingBox", out var boundingBoxElement))
                {
                    barcode.BoundingBox = new BoundingBox
                    {
                        X = boundingBoxElement.GetProperty("x").GetDouble(),
                        Y = boundingBoxElement.GetProperty("y").GetDouble(),
                        Width = boundingBoxElement.GetProperty("width").GetDouble(),
                        Height = boundingBoxElement.GetProperty("height").GetDouble()
                    };
                }

                if (item.TryGetProperty("cornerPoints", out JsonElement cornerPointsElement))
                {
                    barcode.CornerPoints = cornerPointsElement.EnumerateArray().Select(point => new CornerPoint
                        { X = point.GetProperty("x").GetDouble(), Y = point.GetProperty("y").GetDouble() }).ToArray();
                }

                barcodes.Add(barcode);
            }

            return new BarcodeDetectionResult { Success = true, Barcodes = barcodes.ToArray() };
        }

        var error = result.TryGetProperty("error", out var errorElement)
            ? errorElement.GetString()
            : "Lỗi không xác định";
        return new BarcodeDetectionResult { Success = false, Error = error };
    }

    public async ValueTask DisposeAsync()
    {
        if (_module != null)
        {
            await _module.DisposeAsync();
        }
    }
}

public class BarcodeDetectionResult
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public BarcodeInfo[] Barcodes { get; set; } = [];
}

public class BarcodeInfo
{
    public string? Format { get; set; }
    public string? RawValue { get; set; }
    public BoundingBox? BoundingBox { get; set; }
    public CornerPoint[] CornerPoints { get; set; } = [];
}

public class BoundingBox
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
}

public class CornerPoint
{
    public double X { get; set; }
    public double Y { get; set; }
}
public static class BrowserFileExtensions
{
    public static async Task<string> ToDataUrlAsync(this IBrowserFile file)
    {
        var buffer = new byte[file.Size];
        await file.OpenReadStream(maxAllowedSize: 10485760).ReadAsync(buffer); // 10MB max
        
        string base64String = Convert.ToBase64String(buffer);
        string contentType = file.ContentType;
        
        return $"data:{contentType};base64,{base64String}";
    }
}