using Haihv.Elis.Tool.TraCuuGcn.Api.Settings;
using ZiggyCreatures.Caching.Fusion;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;

public static class GiayChungNhanExtensions
{
    public static async Task SetCacheConnectionName(this IFusionCache fusionCache, 
        string connectionName, string? serial = null,
        CancellationToken cancellationToken = default)
    {
        serial = serial.ChuanHoa();
        if (!string.IsNullOrWhiteSpace(serial))
        {
            var cacheKey = CacheSettings.ElisConnectionName(serial);
            await fusionCache.SetAsync(cacheKey,
                connectionName,
                tags: [serial],
                token: cancellationToken);
        }
    }
}