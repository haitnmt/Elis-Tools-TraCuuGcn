using Microsoft.JSInterop;

namespace Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;

public class LeafletMapService(IJSRuntime jsRuntime) : IAsyncDisposable
{
    private const string MapContainerId = "map";
    private const string VersionLeaflet = "1.9.4";
    private const string UrlLibraryLeaflet = "https://unpkg.com/leaflet";
    private readonly string _urlLeafletMapJs = "./_content/Haihv.Elis.Tool.TraCuuGcn.WebLib/leafletMap.js" + "?v=" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

    public async Task InitializeAsync(string? mapContainerId = null)
    {
        mapContainerId ??= MapContainerId;
        try
        {
            // Import CSS cho Leaflet
            // Add CSS link to head
            await jsRuntime.InvokeVoidAsync("eval", $"""
                                                     
                                                                     var link = document.createElement('link');
                                                                     link.rel = 'stylesheet';
                                                                     link.href = '{UrlLibraryLeaflet}@{VersionLeaflet}/dist/leaflet.css';
                                                                     document.head.appendChild(link);
                                                                 
                                                     """);
        
            // Import JS cho Leaflet
            await jsRuntime.InvokeAsync<IJSObjectReference>("import", $"{UrlLibraryLeaflet}@{VersionLeaflet}/dist/leaflet.js");
        
            // Import custom JS
            await jsRuntime.InvokeAsync<IJSObjectReference>("import", _urlLeafletMapJs);
        
            // Đợi một chút để đảm bảo DOM đã load
            await Task.Delay(100);
        
            // Khởi tạo bản đồ
            await jsRuntime.InvokeVoidAsync("leafletMapInterop.initializeMap", mapContainerId);
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task UpdateMapAsync(string? geoJsonData)
    {
        await jsRuntime.InvokeVoidAsync("leafletMapInterop.updateMap", geoJsonData);
    }
    
    public async Task ShareToGoogleMaps()
    {
        await jsRuntime.InvokeVoidAsync("leafletMapInterop.shareToGoogleMaps");
    }
    
    public async ValueTask DisposeAsync()
    {
        try
        {
            await jsRuntime.InvokeVoidAsync("leafletMapInterop.disposeMap");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}