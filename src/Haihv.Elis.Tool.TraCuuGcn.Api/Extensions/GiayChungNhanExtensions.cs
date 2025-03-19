using Haihv.Elis.Tool.TraCuuGcn.Api.Settings;
using ZiggyCreatures.Caching.Fusion;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;

public static class GiayChungNhanExtensions
{
    public static async Task SetCacheConnectionName(this IFusionCache fusionCache, 
        string connectionName, string? serial = null,
        CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(serial))
        {
            var cacheKey = CacheSettings.ElisConnectionName(serial.ChuanHoa());
            await fusionCache.SetAsync(cacheKey,
                connectionName,
                tags: [serial],
                token: cancellationToken);
        }
    }

    public static async Task SetUpdateGroupName(this IFusionCache fusionCache,
        string serial, string groupName,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = CacheSettings.KeyUpdateGroupName(serial.ChuanHoa());
        await fusionCache.SetAsync(cacheKey,
            groupName,
            tags: [serial],
            token: cancellationToken);
    }
}