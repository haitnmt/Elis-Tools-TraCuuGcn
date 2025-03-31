using Haihv.Elis.Tool.TraCuuGcn.Api.Settings;
using Microsoft.Extensions.Caching.Hybrid;


namespace Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;

public static class GiayChungNhanExtensions
{
    public static async Task SetCacheConnectionName(this HybridCache hybridCache, 
        string connectionName, string? serial = null,
        CancellationToken cancellationToken = default)
    {
        serial = serial.ChuanHoa();
        if (!string.IsNullOrWhiteSpace(serial))
        {
            var cacheKey = CacheSettings.ElisConnectionName(serial);
            await hybridCache.SetAsync(cacheKey,
                connectionName,
                tags: [serial],
                cancellationToken: cancellationToken);
        }
    }
}