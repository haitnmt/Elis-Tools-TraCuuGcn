using Microsoft.JSInterop;

namespace Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;

public class LeafletMapService(IJSRuntime jsRuntime) : IAsyncDisposable
{
    private bool _isInitialized;
    private const string UrlLibraryLeaflet = "https://unpkg.com/leaflet@1.7.1/dist/leaflet.js";
    private readonly string _urlLeafletMapJs = "./_content/Haihv.Elis.Tool.TraCuuGcn.WebLib/leafletMap.js" + "?v=" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
    public async Task InitializeAsync()
    {
        if (_isInitialized) return;
        await jsRuntime.InvokeAsync<IJSObjectReference>("import", UrlLibraryLeaflet);
        await jsRuntime.InvokeAsync<IJSObjectReference>("import", _urlLeafletMapJs);
        await jsRuntime.InvokeVoidAsync("leafletMapInterop.initializeMap");
        _isInitialized = true;
    }
    
    public async Task UpdateMapAsync(string geoJson)
    {
        if (!_isInitialized) await InitializeAsync();
        await jsRuntime.InvokeVoidAsync("leafletMapInterop.updateMap", geoJson);
    }
    
    public async ValueTask DisposeAsync()
    {
        if (_isInitialized)
        {
            await jsRuntime.InvokeVoidAsync("leafletMapInterop.disposeMap");
            _isInitialized = false;
        }
    }
}