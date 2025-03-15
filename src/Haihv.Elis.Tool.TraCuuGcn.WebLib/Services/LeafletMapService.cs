using Microsoft.JSInterop;

namespace Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;

public class LeafletMapService(IJSRuntime jsRuntime) : IAsyncDisposable
{
    private const string MapContainerId = "map";
    private readonly string _urlLeafletMapJs = "./_content/Haihv.Elis.Tool.TraCuuGcn.WebLib/leaflet/leafletMap.js"
                                               + "?v=" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
    private readonly string _urlLibraryLeaflet = "./_content/Haihv.Elis.Tool.TraCuuGcn.WebLib/leaflet/leaflet-src.js"
                                                + "?v=" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

    private const string UrlCssLeaflet = "./_content/Haihv.Elis.Tool.TraCuuGcn.WebLib/leaflet/leaflet.css";

    public async Task InitializeAsync(string? mapContainerId = null)
    {
        mapContainerId ??= MapContainerId;
        try
        {
            // Import CSS cho Leaflet
            // Add CSS link to head using IJSRuntime
            await jsRuntime.InvokeVoidAsync("document.head.insertAdjacentHTML", 
                "beforeend", 
                $"<link rel='stylesheet' href='{UrlCssLeaflet}' />");

            // Import JS cho Leaflet
            await jsRuntime.InvokeAsync<IJSObjectReference>("import", _urlLibraryLeaflet);

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