using Haihv.Elis.Tool.TraCuuGcn.Api.Settings;
using ZiggyCreatures.Caching.Fusion;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;

public static class GiayChungNhanExtensions
{
    public static Task SetCacheConnectionName(this IFusionCache fusionCache, 
        long maGcn, string connectionName, 
        CancellationToken cancellationToken = default)
    {
        var cacheKey = CacheSettings.ConnectionName(maGcn);
        return fusionCache.SetAsync(cacheKey, 
            connectionName, 
            TimeSpan.FromDays(60),
            token: cancellationToken).AsTask();
    }
}