using Microsoft.Extensions.Caching.Hybrid;
using StackExchange.Redis;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;

public static class HybridCachingExtensions
{
    public static void AddCache(this IServiceCollection services, string? redisConnectionString = null)
    {
        const string instanceName = "TraCuuGcn:";
        //Add MemoryCache
        services.AddMemoryCache();
        //Add RedisCache
        // Clear redis cache when application start
        if (!string.IsNullOrWhiteSpace(redisConnectionString))
        {
            var redis = ConnectionMultiplexer.Connect(redisConnectionString);
            // Clear all databases in Redis server with prefix instanceName
            foreach (var endPoint in redis.GetEndPoints())
            {
                var server = redis.GetServer(endPoint);
                foreach (var key in server.Keys(pattern: $"{instanceName}*"))
                {
                    redis.GetDatabase().KeyDelete(key);
                }
            }
            services.AddStackExchangeRedisCache(
                options =>
                {
                    options.Configuration = redisConnectionString;
                    options.InstanceName = instanceName;
                });
        }

        services.AddHybridCache(
            options =>
            {
                options.MaximumPayloadBytes = 1024 * 1024;
                options.MaximumKeyLength = 1024;
                options.DefaultEntryOptions = new HybridCacheEntryOptions
                {
                    Expiration = TimeSpan.FromDays(1),
                    LocalCacheExpiration = TimeSpan.FromMinutes(5)
                };
            });

    }
}