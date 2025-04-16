using Haihv.Elis.Tool.TraCuuGcn.Models;

namespace Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;

internal sealed class ServerDataServices(HttpClient httpClient) : IDataServices
{
    // Constants
    private const string UrlSearch = "/elis/search";
    private const string UrlThuaDat = "/elis/thua-dat-public";
    private const string UrlClearCache = "/elis/delete-cache";

    public async Task<GiayChungNhanInfo?> GetGiayChungNhanInfoAsync(string query)
    {
        var url = $"{UrlSearch}?query={query}";
        return await httpClient.GetFromJsonAsync<GiayChungNhanInfo>(url);
    }
    
}