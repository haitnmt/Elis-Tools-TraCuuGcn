﻿using Haihv.Elis.Tool.TraCuuGcn.Web_Api.Settings;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using ZiggyCreatures.Caching.Fusion;
using ILogger = Serilog.ILogger;

namespace Haihv.Elis.Tool.TraCuuGcn.Web_Api.Data;

/// <summary>
/// Interface cho dữ liệu kết nối ELIS.
/// </summary>
public interface IConnectionElisData
{
    /// <summary>
    /// Danh sách các kết nối ELIS.
    /// </summary>
    List<ConnectionElis> ConnectionElis { get; }

    /// <summary>
    /// Danh sách các chuỗi kết nối.
    /// </summary>
    List<string> ConnectionStrings { get; }
    
    /// <summary>
    /// Lấy chuỗi kết nối từ tên kết nối.
    /// </summary>
    /// <param name="name">Tên kết nối.</param>
    /// <returns>Chuỗi kết nối.</returns>
    string GetConnectionString(string name);
    /// <summary>
    /// Lấy chuỗi kết nối từ tên kết nối.
    /// </summary>
    /// <param name="maGcn">Mã GCN.</param>
    /// <returns>Chuỗi kết nối.</returns>
    ValueTask<string?> GetConnectionString(long maGcn);
    /// <summary>
    /// Lấy danh sách chuỗi kết nối dựa trên mã GCN.
    /// </summary>
    /// <param name="maGcn"></param>
    /// <returns>Danh sách chuỗi kết nối.</returns>
    ValueTask<List<ConnectionElis>> GetConnectionElis(long maGcn);
}

/// <summary>
/// Lớp ConnectionElisData quản lý kết nối và cấu hình ELIS.
/// </summary>
/// <param name="configuration">Cấu hình ứng dụng.</param>
/// <param name="logger">Logger để ghi log.</param>
/// <param name="memoryCache">Bộ nhớ đệm để lưu trữ tạm thời.</param>
public sealed class ConnectionElisData(
    IConfiguration configuration,
    ILogger logger,
    IMemoryCache memoryCache,
    IFusionCache fusionCache) : IConnectionElisData
{
    
    private const string SectionName = "ElisSql";
    private const string SectionData = "Databases";
    private const string KeyName = "Name";
    private const string KeyMaDvhc = "MaDvhc";
    private const string KeyConnectionString = "ConnectionString";

    /// <summary>
    /// Danh sách các kết nối ELIS.
    /// </summary>
    public List<ConnectionElis> ConnectionElis =>
        memoryCache.GetOrCreate(CacheSettings.ElisConnections, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
            return GetConnection();
        }) ?? [];

    /// <summary>
    /// Danh sách các chuỗi kết nối.
    /// </summary>
    public List<string> ConnectionStrings => ConnectionElis.Select(x => x.ConnectionString).ToList();
    
    /// <summary>
    /// Lấy danh sách các kết nối từ cấu hình.
    /// </summary>
    /// <returns>Danh sách các kết nối ELIS.</returns>
    private List<ConnectionElis> GetConnection()
    {
        var section = configuration.GetSection(SectionName);
        var data = section.GetSection(SectionData).GetChildren().ToList();
        List<ConnectionElis> result = [];
        foreach (var configurationSection in data)
        {
            var name = configurationSection[KeyName] ?? string.Empty;
            var maDvhc = configurationSection[KeyMaDvhc] ?? string.Empty;
            var connectionString = configurationSection[KeyConnectionString] ?? string.Empty;
            if (string.IsNullOrWhiteSpace(connectionString)) continue;
            using var connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                result.Add(new ConnectionElis(name, int.TryParse(maDvhc, out var maDvhcInt) ? maDvhcInt : 0,
                    connectionString));
                connection.Close();
            }
            catch (Exception e)
            {
                logger.Error(e, "Lỗi khi kiểm tra kết nối ELIS, kết nối {Name}.", name);
            }
        }

        if (result.Count == 0)
        {
            logger.Error("Không tìm thấy cấu hình kết nối ELIS.");
        }

        return result;
    }

    public async ValueTask<string?> GetConnectionString(long maGcn)
    {
        if (maGcn <= 0) return null;
        var connectionName = await fusionCache.GetOrDefaultAsync<string>(
            CacheSettings.ConnectionName(maGcn));
        return string.IsNullOrWhiteSpace(connectionName) ? null : GetConnectionString(connectionName);
    }

    public async ValueTask<List<ConnectionElis>> GetConnectionElis(long maGcn)
    {
        if (maGcn <= 0) return ConnectionElis;
        var connectionName = await fusionCache.GetOrDefaultAsync<string>(
            CacheSettings.ConnectionName(maGcn));
        return string.IsNullOrWhiteSpace(connectionName) ? ConnectionElis : ConnectionElis.Where(x => x.Name == connectionName).ToList();
    }
    public string GetConnectionString(string name) 
        => ConnectionElis.FirstOrDefault(x => x.Name == name)?.ConnectionString ?? string.Empty;
}

/// <summary>
/// Bản ghi ConnectionElis đại diện cho một kết nối ELIS.
/// </summary>
/// <param name="Name">Tên kết nối.</param>
/// <param name="MaDvhc">Mã đơn vị hành chính.</param>
/// <param name="ConnectionString">Chuỗi kết nối.</param>
public record ConnectionElis(string Name, int MaDvhc, string ConnectionString);