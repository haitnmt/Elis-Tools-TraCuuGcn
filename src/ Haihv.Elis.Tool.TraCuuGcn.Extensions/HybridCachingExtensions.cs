using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ZiggyCreatures.Caching.Fusion;
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
    private const string RedisConnectionName = "RedisConnectionName";

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
        var cacheName = builder.Configuration.GetCacheName(redisConnectionName);
        // Khởi tạo FusionCache builder
        var fusionCacheBuilder = builder.Services.AddFusionCache();
        
        fusionCacheBuilder.WithSerializer(new FusionCacheSystemTextJsonSerializer());
        // Nếu có cung cấp chuỗi kết nối Redis, cấu hình thêm distributed cache
        if (!string.IsNullOrWhiteSpace(cacheName))
        {
            // Đăng ký Redis distributed cache
            builder.AddRedisDistributedCache(cacheName);
            // Resolve IDistributedCache thông qua Dependency Injection và cấu hình cho FusionCache
            fusionCacheBuilder.WithDistributedCache(provider =>
                provider.GetRequiredService<IDistributedCache>());
        }

        var defaultEntryOptions = new FusionCacheEntryOptions
                {
                    Duration = TimeSpan.FromSeconds(30),
                    DistributedCacheDuration = TimeSpan.FromDays(1),
                    // FAILSAFE OPTIONS
                    IsFailSafeEnabled = true,
                    FailSafeMaxDuration = TimeSpan.FromHours(12),
                    FailSafeThrottleDuration = TimeSpan.FromSeconds(30),
                    
                    // DISTRIBUTED CACHE OPTIONS
                    DistributedCacheSoftTimeout = TimeSpan.FromSeconds(1),
                    DistributedCacheHardTimeout = TimeSpan.FromSeconds(2),
                    AllowBackgroundDistributedCacheOperations = true,
                    // JITTERING
                    JitterMaxDuration = TimeSpan.FromSeconds(2)
                };
            

        fusionCacheBuilder.DefaultEntryOptions = defaultEntryOptions;
        fusionCacheBuilder.TryWithAutoSetup();
        // Hoàn tất cấu hình FusionCache như một hybrid cache
        fusionCacheBuilder.AsHybridCache();
    }

    /// <summary>
    /// Lấy tên cache từ cấu hình ứng dụng hoặc thông qua tham số đầu vào.
    /// </summary>
    /// <param name="configuration">
    /// Cấu hình ứng dụng (IConfiguration) để lấy thông tin từ các khóa được thiết lập.
    /// </param>
    /// <param name="redisConnectionName">
    /// Tên của chuỗi kết nối Redis được chỉ định. Nếu cung cấp, tên này sẽ được ưu tiên.
    /// </param>
    /// <returns>
    /// Tên cache được lấy từ input hoặc từ cấu hình ứng dụng. Kết quả có thể là null nếu không tìm thấy giá trị phù hợp.
    /// </returns>
    private static string? GetCacheName(this IConfiguration configuration, string? redisConnectionName = null)
    {
        if (!string.IsNullOrWhiteSpace(redisConnectionName))
            return redisConnectionName;
        // Configure Redis
        var cacheName = configuration["Aspire:CacheName"];
        if (string.IsNullOrWhiteSpace(cacheName) &&
            !string.IsNullOrWhiteSpace(configuration.GetConnectionString(RedisConnectionName)))
        {
            cacheName = RedisConnectionName;
        }
        return cacheName;
    }
}