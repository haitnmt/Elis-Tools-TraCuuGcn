using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Backplane.StackExchangeRedis;
using ZiggyCreatures.Caching.Fusion.Serialization.SystemTextJson;

namespace Haihv.Elis.Tool.TraCuuGcn.Extensions;

/// <summary>
/// Cung cấp các phương thức mở rộng để cấu hình hệ thống cache kết hợp (hybrid caching) cho ứng dụng.
/// </summary>
/// <remarks>
/// Hybrid caching kết hợp cả memory cache (nhanh nhưng local) và distributed cache (chậm hơn nhưng được chia sẻ giữa các instance).
/// Lớp này sử dụng FusionCache để triển khai giải pháp caching hai tầng hiệu quả.
/// </remarks>
public static class HybridCachingExtensions
{

    /// <summary>
    /// Đăng ký và cấu hình hệ thống cache kết hợp cho ứng dụng.
    /// </summary>
    /// <param name="builder">Builder để cấu hình ứng dụng.</param>
    /// <param name="redisConnectionName">
    /// Tên của chuỗi kết nối đến Redis server. Nếu được cung cấp, sẽ được sử dụng làm distributed cache.
    /// Nếu không cung cấp, hệ thống sẽ tìm kiếm cấu hình từ "Aspire:CacheName" hoặc ConnectionString có tên "RedisConnectionName".
    /// </param>
    /// <remarks>
    /// Phương thức này cấu hình hệ thống cache kết hợp:
    /// - Luôn cấu hình memory cache
    /// - Nếu cung cấp chuỗi kết nối Redis, cấu hình thêm distributed cache với Redis
    /// - Kết hợp cả hai thành một hybrid cache để tối ưu hiệu suất
    /// </remarks>
    public static void AddCache(this IHostApplicationBuilder builder, string? redisConnectionName = null)
    {

        // Đăng ký memory cache - luôn được sử dụng làm cache tầng 1
        builder.Services.AddMemoryCache();
        var redisCacheName = builder.Configuration.GetRedisCacheName(redisConnectionName);
        var instanceName = builder.Configuration.GetInstanceName() ?? "DefaultCache";
        // Khởi tạo FusionCache builder
        var fusionCacheBuilder = builder.Services.AddFusionCache();
        
        fusionCacheBuilder.WithSerializer(new FusionCacheSystemTextJsonSerializer());
        // Cấu hình các tùy chọn mặc định cho FusionCache
        var defaultEntryOptions = new FusionCacheEntryOptions
        {
            Duration = TimeSpan.FromSeconds(30),
            DistributedCacheSoftTimeout = TimeSpan.FromSeconds(1),
            DistributedCacheHardTimeout = TimeSpan.FromSeconds(2),
            AllowBackgroundDistributedCacheOperations = true,
            // FAILSAFE OPTIONS
            IsFailSafeEnabled = true,
            FailSafeMaxDuration = TimeSpan.FromHours(12),
            FailSafeThrottleDuration = TimeSpan.FromSeconds(30),
            // JITTERING
            JitterMaxDuration = TimeSpan.FromSeconds(2)
        };
        var cacheOptions = new FusionCacheOptions
        {
            // CUSTOM LOG LEVELS
            FactorySyntheticTimeoutsLogLevel = LogLevel.Debug,
            FactoryErrorsLogLevel = LogLevel.Error,
            // Cấu hình các tùy chọn cho Redis cache
            DistributedCacheCircuitBreakerDuration = TimeSpan.FromSeconds(2),
            // Cấu hình thời gian chờ cho Redis cache
            DistributedCacheSyntheticTimeoutsLogLevel = LogLevel.Debug,
            DistributedCacheErrorsLogLevel = LogLevel.Error,
            FailSafeActivationLogLevel = LogLevel.Debug,
            SerializationErrorsLogLevel = LogLevel.Warning,
            // Cấu hình cache key prefix
            CacheKeyPrefix = $"{instanceName}:"
        };
        // Nếu có cung cấp chuỗi kết nối Redis, cấu hình thêm distributed cache
        if (!string.IsNullOrWhiteSpace(redisCacheName))
        {
            // Đăng ký Redis distributed cache
            builder.AddRedisDistributedCache(redisCacheName);
            
            // Lấy kết nối Redis từ DI container
            var redis = builder.Services.BuildServiceProvider().GetService<IConnectionMultiplexer>();
            if (redis == null)
            {
                throw new InvalidOperationException("Không thể khởi tạo Redis cache");
            }

            // Cấu hình FusionCache với Redis
            fusionCacheBuilder.WithDistributedCache(new RedisCache(
                    new RedisCacheOptions
                    {
                        Configuration = redis.Configuration,
                    }))
                .WithBackplane(
                    new RedisBackplane(new RedisBackplaneOptions
                        { Configuration = redis.Configuration })
                );
            defaultEntryOptions.DistributedCacheDuration = TimeSpan.FromDays(7);
            
        }
        else
        {
            Console.WriteLine("Sử dụng Memory cache!");
            builder.Services.AddDistributedMemoryCache();
            var distributed = builder.Services.BuildServiceProvider().GetService<IDistributedCache>();
            if (distributed == null)
            {
                throw new InvalidOperationException("Không thể khởi tạo DistributedCache");
            }
            fusionCacheBuilder.WithDistributedCache(distributed);
            defaultEntryOptions.DistributedCacheDuration = TimeSpan.FromHours(12);
        }
        // Cấu hình các tùy chọn mặc định cho FusionCache
        fusionCacheBuilder.DefaultEntryOptions = defaultEntryOptions;
        
        // Cấu hình các tùy chọn cho FusionCache
        fusionCacheBuilder.WithOptions(cacheOptions);
        // Cấu hình FusionCache như một hybrid cache
        fusionCacheBuilder.AsHybridCache();
    }
}