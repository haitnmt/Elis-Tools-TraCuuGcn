using Microsoft.Extensions.Configuration;

namespace Haihv.Elis.Tool.TraCuuGcn.Extensions;

internal static class ConfigurationExtensions
{
    private const string RedisConnectionNameKey = "RedisConnectionName";
    private const string AspireCacheNameKey = "Aspire:CacheName";
    private const string InstanceNameKey = "InstanceName";
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
    internal static string? GetRedisCacheName(this IConfiguration configuration, string? redisConnectionName = null)
    {
        if (!string.IsNullOrWhiteSpace(redisConnectionName))
            return redisConnectionName;
        // Configure Redis
        var cacheName = configuration[AspireCacheNameKey];
        if (string.IsNullOrWhiteSpace(cacheName) &&
            !string.IsNullOrWhiteSpace(configuration.GetConnectionString(RedisConnectionNameKey)))
        {
            cacheName = RedisConnectionNameKey;
        }
        return cacheName;
    }
    /// <summary>
    /// Lấy tên cache từ cấu hình ứng dụng hoặc thông qua tham số đầu vào.
    /// </summary>
    /// <param name="configuration">
    /// Cấu hình ứng dụng (IConfiguration) để lấy thông tin từ các khóa được thiết lập.
    /// </param>
    /// <returns>
    /// Tên cache được lấy từ input hoặc từ cấu hình ứng dụng. Kết quả có thể là null nếu không tìm thấy giá trị phù hợp.
    /// </returns>
    internal static string? GetInstanceName(this IConfiguration configuration)
    {
        // Configure Redis
        var instanceName = configuration[InstanceNameKey];
        return instanceName;
    }
}