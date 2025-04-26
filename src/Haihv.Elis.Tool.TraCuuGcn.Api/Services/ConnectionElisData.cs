using System.Text.RegularExpressions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Settings;
using InterpolatedSql.Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Caching.Memory;
using static System.Threading.Tasks.ValueTask;
using ILogger = Serilog.ILogger;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Services;

/// <summary>
/// Interface cho dữ liệu kết nối ELIS.
/// </summary>
public interface IConnectionElisData
{
    /// <summary>
    /// Lấy đường dẫn API SDE.
    /// </summary>
    /// <value>Đường dẫn API SDE.</value>
    string ApiSdeUrl();

    /// <summary>
    /// Danh sách các kết nối ELIS.
    /// </summary>
    List<ConnectionSql> ConnectionElis { get; }

    // /// <summary>
    // /// Danh sách các chuỗi kết nối.
    // /// </summary>
    // List<string> ConnectionStrings { get; }

    // /// <summary>
    // /// Lấy chuỗi kết nối từ tên kết nối.
    // /// </summary>
    // /// <param name="name">Tên kết nối.</param>
    // /// <returns>Chuỗi kết nối.</returns>
    // string GetElisConnectionString(string name);

    /// <summary>
    /// Lấy danh sách kết nối
    /// </summary>
    /// <param name="serial"> Serial của GCNQSDD.</param>
    /// <returns>Danh sách chuỗi kết nối.</returns>
    ValueTask<List<ConnectionSql>> GetAllConnectionAsync(string? serial = null);
    /// <summary>
    /// Lấy kết nối CSDL dựa trên serial.
    /// </summary>
    /// <param name="serial"> Serial của GCNQSDD.</param>
    /// <returns>Kết nối CSDL.
    /// <see cref="ConnectionSql"/>
    /// </returns>
    ValueTask<ConnectionSql?> GetConnectionAsync(string? serial = null);
    /// <summary>
    /// Xóa cache dữ liệu dựa trên thời gian.
    /// </summary>
    /// <param name="timeSpan">Thời gian cần xóa cache.</param>
    /// <param name="cancellationToken">Token hủy.</param>
    /// <returns></returns>
    Task CacheRemoveAsync(TimeSpan timeSpan, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy thời gian chênh lệch giữa máy chủ và máy khách.
    /// </summary>
    /// <param name="cancellationToken">Token hủy.</param>
    /// <returns></returns>
    Task<int> GetTimeDifferenceAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Lấy tên nhóm có quyền cập nhật theo serial.
    /// </summary>
    /// <param name="serial">
    /// Serial của GCNQSDD.
    /// </param>
    /// <param name="cancellationToken">
    /// Token hủy.
    /// </param>
    /// <returns>
    /// Tên nhóm có quyền cập nhật.
    /// </returns>
    Task<string?> GetUpdateGroupName(string? serial, CancellationToken cancellationToken = default);
    /// <summary>
    /// Lấy danh sách tên nhóm có quyền cập nhật
    /// </summary>
    /// <returns>
    /// Danh sách tên nhóm có quyền cập nhật.
    /// </returns>
    IEnumerable<string> GetUpdateGroupName();
}

/// <summary>
/// Lớp ConnectionElisData quản lý kết nối và cấu hình ELIS.
/// </summary>
/// <param name="configuration">Cấu hình ứng dụng.</param>
/// <param name="logger">Logger để ghi log.</param>
/// <param name="memoryCache">Bộ nhớ đệm để lưu trữ tạm thời.</param>
public sealed partial class ConnectionElisData(
    IConfiguration configuration,
    ILogger logger,
    IMemoryCache memoryCache,
    HybridCache hybridCache) : IConnectionElisData
{
    private const string SectionName = "ElisSql";
    private const string SectionData = "Databases";
    private const string KeyName = "Name";
    private const string KeyMaDvhc = "MaDvhc";
    private const string KeyElisConnectionString = "ElisConnectionString";
    private const string KeySdeDatabase = "SdeDatabase";
    private const string KeyApiSde = "ApiSde";
    private const string KeyUpdateGroupName = "UpdateGroupName";


    /// <summary>
    /// Lấy đường dẫn API SDE.
    /// </summary>
    /// <value>Đường dẫn API SDE.</value>
    public string ApiSdeUrl() => configuration[$"{SectionName}:{KeyApiSde}"] ?? string.Empty;

    /// <summary>
    /// Danh sách các kết nối ELIS.
    /// </summary>
    public List<ConnectionSql> ConnectionElis =>
        memoryCache.GetOrCreate(CacheSettings.ElisConnections, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
            return GetAllConnection();
        }) ?? [];


    /// <summary>
    /// Lấy danh sách các kết nối từ cấu hình.
    /// </summary>
    /// <returns>Danh sách các kết nối ELIS.</returns>
    private List<ConnectionSql> GetAllConnection()
    {
        var section = configuration.GetSection(SectionName);
        var data = section.GetSection(SectionData).GetChildren().ToList();
        List<ConnectionSql> result = [];
        foreach (var configurationSection in data)
        {
            var name = configurationSection[KeyName] ?? string.Empty;
            var maDvhc = configurationSection[KeyMaDvhc] ?? string.Empty;
            var elisConnectionString = configurationSection[KeyElisConnectionString] ?? string.Empty;
            var sdeConnectionString = configurationSection[KeySdeDatabase] ?? string.Empty;
            var groupName = configurationSection[KeyUpdateGroupName] ?? string.Empty;
            if (string.IsNullOrWhiteSpace(elisConnectionString)) continue;
            using var connection = new SqlConnection(elisConnectionString);
            try
            {
                connection.Open();
                result.Add(new ConnectionSql(name, int.TryParse(maDvhc, out var maDvhcInt) ? maDvhcInt : 0,
                    elisConnectionString, sdeConnectionString, groupName));
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

    public async ValueTask<List<ConnectionSql>> GetAllConnectionAsync(string? serial)
    {
        serial = serial.ChuanHoa();
        if (string.IsNullOrWhiteSpace(serial)) return ConnectionElis;
        var connectionName =
            await hybridCache.GetOrCreateAsync(CacheSettings.ElisConnectionName(serial),
                _ => FromResult<string?>(null));
        return string.IsNullOrWhiteSpace(connectionName)
            ? ConnectionElis
            : ConnectionElis.Where(x => x.Name == connectionName).ToList();
    }

    public async ValueTask<ConnectionSql?> GetConnectionAsync(string? serial)
    {
        serial = serial.ChuanHoa();
        if (string.IsNullOrWhiteSpace(serial)) return null;
        var connectionName =
            await hybridCache.GetOrCreateAsync(CacheSettings.ElisConnectionName(serial),
                _ => FromResult<string?>(null));
        return string.IsNullOrWhiteSpace(connectionName)
            ? null
            : ConnectionElis.FirstOrDefault(x => x.Name == connectionName);
    }

    public string GetElisConnectionString(string name)
        => ConnectionElis.FirstOrDefault(x => x.Name == name)?.ElisConnectionString ?? string.Empty;

    /// <summary>
    /// Lấy thời gian chênh lệch giữa máy chủ và máy khách.
    /// </summary>
    /// <param name="cancellationToken">Token hủy.</param>
    /// <returns></returns>
    public async Task<int> GetTimeDifferenceAsync(CancellationToken cancellationToken = default)
    {
        var maxTimeDifference = 0;
        try
        {
            foreach (var elisConnectionString in ConnectionElis.Select(x => x.ElisConnectionString))
            {
                var connection = elisConnectionString.GetConnection();
                var query = connection.SqlBuilder(
                    $"SELECT GETUTCDATE();"
                );
                var now = DateTime.UtcNow;
                var result = await query.QueryFirstAsync<DateTime>(cancellationToken: cancellationToken);
                var timeDifference = (int)(now - result).TotalSeconds;
                if (timeDifference > maxTimeDifference) maxTimeDifference = timeDifference;
            }
        }
        catch (Exception e)
        {
            logger.Error(e, "Lỗi khi lấy thời gian chênh lệch giữa máy chủ và máy khách.");
            maxTimeDifference = 300;
        }
        return maxTimeDifference;
    }
    /// <summary>
    /// Lấy tên nhóm có quyền cập nhật theo serial.
    /// </summary>
    /// <param name="serial">
    /// Serial của GCNQSDD.
    /// </param>
    /// <param name="cancellationToken">
    /// Token hủy.
    /// </param>
    /// <returns>
    /// Tên nhóm có quyền cập nhật.
    /// </returns>
    public async Task<string?> GetUpdateGroupName(string? serial, CancellationToken cancellationToken = default)
    {
        serial = serial.ChuanHoa();
        if (string.IsNullOrWhiteSpace(serial)) return null;
        var connectionSql = await GetConnectionAsync(serial);
        return connectionSql?.UpdateGroupName;
    }

    public IEnumerable<string> GetUpdateGroupName()
    {
        return ConnectionElis.Select(x => x.UpdateGroupName);
    }

    /// <summary>
    /// Xóa cache dữ liệu dựa trên thời gian.
    /// </summary>
    /// <param name="timeSpan">Thời gian cần xóa cache.</param>
    /// <param name="cancellationToken">Token hủy.</param>
    /// <returns></returns>
    public async Task CacheRemoveAsync(TimeSpan timeSpan, CancellationToken cancellationToken = default)
    {
        if (timeSpan == TimeSpan.Zero) return;
        var dateTime = DateTime.UtcNow.Subtract(timeSpan);
        foreach (var elisConnectionString in ConnectionElis.Select(x => x.ElisConnectionString))
        {
            var connection = elisConnectionString.GetConnection();
            var query = connection.SqlBuilder(
                $"""
                         SELECT DISTINCT [OldValue], [NewValue]
                         FROM Audit
                            WHERE [DateTime] >= {dateTime} AND [TableName] = 'GCNQSDD'
                         """
            );
            var dynamicList = (await query.QueryAsync(cancellationToken: cancellationToken)).ToList();
            foreach (var dynamic in dynamicList)
            {
                var serials = new List<string>();
                var serial = GetSerialFromAuditLog(dynamic.OldValue);
                if (!string.IsNullOrWhiteSpace(serial))
                {
                    serials.Add(serial);
                }
                serial = GetSerialFromAuditLog(dynamic.NewValue);
                if (!string.IsNullOrWhiteSpace(serial)) 
                {
                    serials.Add(serial);
                }
                await hybridCache.RemoveByTagAsync(serials, cancellationToken: cancellationToken);
            }
        }
    }
    /// <summary>
    /// Lấy serial từ audit log.
    /// </summary>
    /// <param name="auditValue">Giá trị audit log. </param>
    /// <returns> Serial của GCNQSDD.</returns>
    private static string? GetSerialFromAuditLog(dynamic? auditValue)
    {
        var auditValueStr = auditValue as string;
        if (string.IsNullOrWhiteSpace(auditValueStr)) return null;
        var match = SerialRegex().Match(auditValueStr);
        return match.Success ? match.Groups[1].Value.Trim().ToUpper() : null;
    }
    
    /// <summary>
    /// Phương thức tạo đối tượng Regex để trích xuất số serial từ chuỗi.
    /// </summary>
    /// <returns>Đối tượng Regex để tìm kiếm và trích xuất mã serial có định dạng "Serial: [chuỗi số]".</returns>
    [GeneratedRegex(@"Serial: (\w+ \d+)")]
    private static partial Regex SerialRegex();
}

/// <summary>
/// Bản ghi ConnectionElis đại diện cho một kết nối ELIS.
/// </summary>
/// <param name="Name">Tên kết nối.</param>
/// <param name="MaDvhc">Mã đơn vị hành chính.</param>
/// <param name="ElisConnectionString">Chuỗi kết nối CSDL.</param>
/// <param name="SdeDatabase">Tên CSDL SDE.</param>
/// <param name="UpdateGroupName">Tên nhóm có quyền cập nhật.</param>
public record ConnectionSql(string Name, int MaDvhc, string ElisConnectionString, string SdeDatabase, string UpdateGroupName);