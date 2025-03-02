using Haihv.Elis.Tool.TraCuuGcn.Api.Settings;
using LanguageExt.Common;
using ZiggyCreatures.Caching.Fusion;
using ILogger = Serilog.ILogger;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Services;

public interface IGeoService
{
    /// <summary>
    /// Lấy thông tin toạ độ thửa đất từ cơ sở dữ liệu.
    /// </summary>
    /// <param name="maGcnElis">Mã GCN của Giấy chứng nhận. </param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    /// Kết quả chứa thông tin toạ độ thửa đất hoặc lỗi nếu không tìm thấy.
    /// </returns>
    Task<Result<List<Coordinates>>> GetResultAsync(long maGcnElis, CancellationToken cancellationToken = default);
    /// <summary>
    /// Lấy thông tin toạ độ thửa đất từ cơ sở dữ liệu.
    /// </summary>
    /// <param name="maGcnElis">Mã GCN của Giấy chứng nhận. </param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    /// Kết quả chứa thông tin toạ độ thửa đất
    /// </returns>
    Task<List<Coordinates>> GetAsync(long maGcnElis, CancellationToken cancellationToken = default);
}

public class GeoService(
    IConnectionElisData connectionElisData,
    IThuaDatService thuaDatService,
    ILogger logger,
    IFusionCache fusionCache) : IGeoService
{

    private readonly string _apiSdeUrl = connectionElisData.ApiSdeUrl();
    public async Task<Result<List<Coordinates>>> GetResultAsync(long maGcnElis, CancellationToken cancellationToken = default)
    {
        try
        {
            var coordinates = await GetAsync(maGcnElis, cancellationToken);
            if (coordinates.Count > 0) return coordinates;
            logger.Error("Không tìm thấy toạ độ thửa trong cơ sở dữ liệu: {MaGcnElis}", maGcnElis);
            return new Result<List<Coordinates>>(new Exception("Không tìm thấy toạ độ thửa trong cơ sở dữ liệu."));

        }
        catch (Exception e)
        {
            logger.Error(e, "Lỗi khi lấy thông tin toạ độ thửa: {MaGcnElis}", maGcnElis);
            return new Result<List<Coordinates>>(e);
        }
    }
    public async Task<List<Coordinates>> GetAsync(long maGcnElis, CancellationToken cancellationToken = default)
    {
        try
        {
            return await fusionCache.GetOrSetAsync(CacheSettings.KeyToaDoThua(maGcnElis), 
                await GetPointFromApiSdeAsync(maGcnElis, cancellationToken),
                tags: [maGcnElis.ToString()],
                token: cancellationToken);
        }
        catch (Exception e)
        {
            logger.Error(e, "Lỗi khi lấy thông tin toạ độ thửa: {MaGcnElis}", maGcnElis);
            throw;
        }
    }
    
    private sealed record BodyResponse(string Status, string Message, Coordinates Data);
    private async Task<List<Coordinates>> GetPointFromApiSdeAsync(long maGcnElis, CancellationToken cancellationToken = default)
    {
        var connectionSqls = await connectionElisData.GetConnection(maGcnElis);
        if (connectionSqls.Count == 0) return [];
        var thuaDats  = await thuaDatService.GetThuaDatInDatabaseAsync(maGcnElis, cancellationToken);
        List<Coordinates> result = [];
        foreach (var (maDvhc, thuaDatSo, toBanDo, tyLe, _, _, _, _, _, _, _) in thuaDats)
        {
            var soTo = toBanDo.Trim().ToLower();
            var soThua = thuaDatSo.Trim().ToLower();
            if (string.IsNullOrWhiteSpace(_apiSdeUrl))
            {
                var ex = new NullReferenceException("Không tìm thấy đường dẫn API SDE");
                logger.Error(ex, 
                    "Không tìm thấy đường dẫn API SDE");
                throw ex;
            }
            var httpClient = new HttpClient { BaseAddress = new Uri(_apiSdeUrl) };
            foreach (var (name, _, _, database) in connectionSqls)
            {
                try
                {
                    var req = new {database, SoTo = soTo, SoThua = soThua, TyLe = tyLe, KvhcId = maDvhc};
                    var response = await httpClient.PostAsJsonAsync("/api/sde/toadothuadat", req, cancellationToken);
                    if (!response.IsSuccessStatusCode) continue;
                    var json = await response.Content.ReadFromJsonAsync<BodyResponse>(cancellationToken: cancellationToken);
                    if (json is null || json.Status != "success") continue;
                    result.Add(json.Data);
                }
                catch (Exception e)
                {
                    logger.Error(e, "Lỗi khi lấy vị trí thửa đất trong SDE {MaGcnElis}, {SdeName}",
                        maGcnElis, name);
                    throw;
                }
            }
        }
        return result;
    }
}

public sealed class Coordinates
{
    public double X { get; init; }
    public double Y { get; init; }
}
