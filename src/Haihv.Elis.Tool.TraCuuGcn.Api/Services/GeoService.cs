using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Settings;
using LanguageExt.Common;
using Microsoft.Extensions.Caching.Hybrid;
using System.Collections.Concurrent;
using ILogger = Serilog.ILogger;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Services;

public interface IGeoService
{
    /// <summary>
    /// Lấy thông tin toạ độ thửa đất từ cơ sở dữ liệu.
    /// </summary>
    /// <param name="serial"> Serial của GCN. </param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    /// Kết quả chứa thông tin toạ độ thửa đất hoặc lỗi nếu không tìm thấy.
    /// </returns>
    Task<Result<List<Coordinates>>> GetResultAsync(string serial, CancellationToken cancellationToken = default);
    /// <summary>
    /// Lấy thông tin toạ độ thửa đất từ cơ sở dữ liệu.
    /// </summary>
    /// <param name="serial"> Serial của GCN. </param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    /// Kết quả chứa thông tin toạ độ thửa đất
    /// </returns>
    Task<List<Coordinates>> GetAsync(string serial, CancellationToken cancellationToken = default);
}

public class GeoService : IGeoService
{
    private readonly IConnectionElisData _connectionElisData;
    private readonly IThuaDatService _thuaDatService;
    private readonly ILogger _logger;
    private readonly HybridCache _hybridCache;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _apiSdeUrl;

    public GeoService(
        IConnectionElisData connectionElisData,
        IThuaDatService thuaDatService,
        ILogger logger,
        HybridCache hybridCache,
        IHttpClientFactory httpClientFactory)
    {
        _connectionElisData = connectionElisData;
        _thuaDatService = thuaDatService;
        _logger = logger;
        _hybridCache = hybridCache;
        _httpClientFactory = httpClientFactory;
        _apiSdeUrl = connectionElisData.ApiSdeUrl();

        if (!string.IsNullOrWhiteSpace(_apiSdeUrl)) return;
        var ex = new NullReferenceException("Không tìm thấy đường dẫn API SDE");
        _logger.Error(ex, "Không tìm thấy đường dẫn API SDE trong cấu hình");
        throw ex;
    }

    public async Task<Result<List<Coordinates>>> GetResultAsync(string serial, CancellationToken cancellationToken = default)
    {
        try
        {
            var coordinates = await GetAsync(serial, cancellationToken);
            if (coordinates.Count > 0) return coordinates;
            _logger.Error("Không tìm thấy toạ độ thửa trong cơ sở dữ liệu: {Serial}", serial);
            return new Result<List<Coordinates>>(new Exception("Không tìm thấy toạ độ thửa trong cơ sở dữ liệu."));

        }
        catch (Exception e)
        {
            _logger.Error(e, "Lỗi khi lấy thông tin toạ độ thửa: {Serial}", serial);
            return new Result<List<Coordinates>>(e);
        }
    }

    public async Task<List<Coordinates>> GetAsync(string serial, CancellationToken cancellationToken = default)
    {
        serial = serial.ChuanHoa() ?? "";
        if (string.IsNullOrWhiteSpace(serial)) return [];
        try
        {
            // Thêm thời gian hết hạn cho cache
            var options = new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromDays(7),
                LocalCacheExpiration = TimeSpan.FromHours(1)
            };

            return await _hybridCache.GetOrCreateAsync(
                CacheSettings.KeyToaDoThua(serial),
                async cancel => await GetPointFromApiSdeAsync(serial, cancel),
                options: options,
                tags: [serial],
                cancellationToken: cancellationToken);
        }
        catch (Exception e)
        {
            _logger.Error(e, "Lỗi khi lấy thông tin toạ độ thửa: {Serial}", serial);
            throw;
        }
    }

    private record BodyResponse(string Status, string Message, Coordinates Data);

    private async Task<List<Coordinates>> GetPointFromApiSdeAsync(string serial, CancellationToken cancellationToken = default)
    {
        var connectionSqls = await _connectionElisData.GetAllConnectionAsync(serial);
        if (connectionSqls.Count == 0) return [];

        var thuaDats = await _thuaDatService.GetThuaDatInDatabaseAsync(serial, cancellationToken);
        if (thuaDats.Count == 0) return [];

        var result = new ConcurrentBag<Coordinates>();
        var errors = new ConcurrentBag<Exception>();

        // Tạo HttpClient từ factory
        var httpClient = _httpClientFactory.CreateClient("SdeApi");
        httpClient.BaseAddress = new Uri(_apiSdeUrl);

        // Xử lý song song cho mỗi thửa đất
        var thuaDatTasks = thuaDats.Select(async thuaDat =>
        {
            var (_, _, maDvhc, thuaDatSo, toBanDo, tyLe, _, _, _, _, _, _, _) = thuaDat;
            var soTo = toBanDo.Trim().ToLower();
            var soThua = thuaDatSo.Trim().ToLower();

            // Xử lý song song cho mỗi kết nối
            var connectionTasks = connectionSqls.Select(async connection =>
            {
                var (name, _, _, database, _) = connection;
                try
                {
                    var req = new { database, SoTo = soTo, SoThua = soThua, TyLe = tyLe, KvhcId = maDvhc };
                    var response = await httpClient.PostAsJsonAsync("/api/sde/toadothuadat", req, cancellationToken);

                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.Warning("API SDE trả về mã lỗi {StatusCode} cho {Serial} {Database}",
                            (int)response.StatusCode, serial, database);
                        return;
                    }

                    var json = await response.Content.ReadFromJsonAsync<BodyResponse>(cancellationToken: cancellationToken);
                    if (json is null || json.Status != "success")
                    {
                        _logger.Warning("API SDE trả về trạng thái không thành công cho {Serial} {Database}: {Status}",
                            serial, database, json?.Status);
                        return;
                    }

                    if (!string.IsNullOrWhiteSpace(json.Message))
                    {
                        _logger.Debug("API SDE thông báo: {Message}", json.Message);
                    }

                    result.Add(json.Data);
                }
                catch (Exception e)
                {
                    _logger.Error(e, "Lỗi khi lấy vị trí thửa đất trong SDE {Serial} {Sde}",
                        serial, name);
                    errors.Add(e);
                }
            });

            await Task.WhenAll(connectionTasks);
        });

        await Task.WhenAll(thuaDatTasks);

        // Nếu không có kết quả và có lỗi, ném ra lỗi đầu tiên
        if (result.IsEmpty && !errors.IsEmpty)
        {
            throw errors.First();
        }

        return result.ToList();
    }
}

public record Coordinates(double X, double Y);
