using Haihv.Elis.Tool.TraCuuGcn.Api.Settings;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using ZiggyCreatures.Caching.Fusion;
using ILogger = Serilog.ILogger;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Data;

/// <summary>
/// Interface cho dữ liệu kết nối ELIS.
/// </summary>
public interface IConnectionElisData
{
    /// <summary>
    /// Danh sách các kết nối ELIS.
    /// </summary>
    List<ConnectionSql> ConnectionElis { get; }

    // /// <summary>
    // /// Danh sách các chuỗi kết nối.
    // /// </summary>
    // List<string> ConnectionStrings { get; }

    /// <summary>
    /// Lấy chuỗi kết nối từ tên kết nối.
    /// </summary>
    /// <param name="name">Tên kết nối.</param>
    /// <returns>Chuỗi kết nối.</returns>
    string GetElisConnectionString(string name);

    /// <summary>
    /// Lấy chuỗi kết nối SDE từ tên kết nối.
    /// </summary>
    /// <param name="name">Tên kết nối.</param>
    /// <returns>Chuỗi kết nối.</returns>
    string GetSdeConnectionString(string name);

    /// <summary>
    /// Lấy danh sách chuỗi kết nối dựa trên mã GCN.
    /// </summary>
    /// <param name="maGcn"></param>
    /// <returns>Danh sách chuỗi kết nối.</returns>
    ValueTask<List<ConnectionSql>> GetConnection(long maGcn);
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
    private const string KeyElisConnectionString = "ElisConnectionString";
    private const string KeySdeConnectionString = "SdeConnectionString";

    /// <summary>
    /// Danh sách các kết nối ELIS.
    /// </summary>
    public List<ConnectionSql> ConnectionElis =>
        memoryCache.GetOrCreate(CacheSettings.ElisConnections, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
            return GetConnection();
        }) ?? [];

    // /// <summary>
    // /// Danh sách các chuỗi kết nối.
    // /// </summary>
    // public List<string> ConnectionStrings => ConnectionElis.Select(x => x.ElisConnectionString).ToList();
    /// <summary>
    /// Danh sách các chuỗi kết nối.
    /// </summary>
    public List<string> SdeConnectionStrings => ConnectionElis.Select(x => x.SdeConnectionString).ToList();

    /// <summary>
    /// Lấy danh sách các kết nối từ cấu hình.
    /// </summary>
    /// <returns>Danh sách các kết nối ELIS.</returns>
    private List<ConnectionSql> GetConnection()
    {
        var section = configuration.GetSection(SectionName);
        var data = section.GetSection(SectionData).GetChildren().ToList();
        List<ConnectionSql> result = [];
        foreach (var configurationSection in data)
        {
            var name = configurationSection[KeyName] ?? string.Empty;
            var maDvhc = configurationSection[KeyMaDvhc] ?? string.Empty;
            var elisConnectionString = configurationSection[KeyElisConnectionString] ?? string.Empty;
            var sdeConnectionString = configurationSection[KeySdeConnectionString] ?? string.Empty;
            if (string.IsNullOrWhiteSpace(elisConnectionString)) continue;
            using var connection = new SqlConnection(elisConnectionString);
            try
            {
                connection.Open();
                result.Add(new ConnectionSql(name, int.TryParse(maDvhc, out var maDvhcInt) ? maDvhcInt : 0,
                    elisConnectionString, sdeConnectionString));
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

    public async ValueTask<List<ConnectionSql>> GetConnection(long maGcn)
    {
        if (maGcn <= 0) return ConnectionElis;
        var connectionName = await fusionCache.GetOrDefaultAsync<string>(
            CacheSettings.ElisConnectionName(maGcn));
        return string.IsNullOrWhiteSpace(connectionName)
            ? ConnectionElis
            : ConnectionElis.Where(x => x.Name == connectionName).ToList();
    }

    public string GetElisConnectionString(string name)
        => ConnectionElis.FirstOrDefault(x => x.Name == name)?.ElisConnectionString ?? string.Empty;

    public string GetSdeConnectionString(string name)
        => ConnectionElis.FirstOrDefault(x => x.Name == name)?.SdeConnectionString ?? string.Empty;
}

/// <summary>
/// Bản ghi ConnectionElis đại diện cho một kết nối ELIS.
/// </summary>
/// <param name="Name">Tên kết nối.</param>
/// <param name="MaDvhc">Mã đơn vị hành chính.</param>
/// <param name="ElisConnectionString">Chuỗi kết nối CSDL.</param>
/// <param name="SdeConnectionString">Chuỗi kết nối SDE.</param>
public record ConnectionSql(string Name, int MaDvhc, string ElisConnectionString, string SdeConnectionString);