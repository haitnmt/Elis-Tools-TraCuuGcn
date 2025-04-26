using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

namespace Haihv.Elis.Tool.TraCuuGcn.Extensions;

public static class DataProtectionExtensions
{
    public static void AddDataProtection(this IHostApplicationBuilder builder, string? applicationName = null, string? dataProtectionKey = null)
    {
        var service = builder.Services;
        var redis = service.BuildServiceProvider().GetService<IConnectionMultiplexer>();
        if (redis == null)
        {
            return;
        }
        dataProtectionKey ??= "DataProtection";
        applicationName ??= builder.Configuration.GetInstanceName() ?? "DataProtection";
        var key = $"{applicationName}:{dataProtectionKey}";
        // Add services Data Protection sử dụng Redis
        builder.Services.AddDataProtection()
            .SetApplicationName(applicationName)
            .PersistKeysToStackExchangeRedis(redis,key); // Key trong Redis để lưu trữ keys
    }
}