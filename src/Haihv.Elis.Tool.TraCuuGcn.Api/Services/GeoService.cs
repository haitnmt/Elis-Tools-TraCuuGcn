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
    Task<Result<Coordinates>> GetResultAsync(long maGcnElis, CancellationToken cancellationToken = default);
    /// <summary>
    /// Lấy thông tin toạ độ thửa đất từ cơ sở dữ liệu.
    /// </summary>
    /// <param name="maGcnElis">Mã GCN của Giấy chứng nhận. </param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    /// Kết quả chứa thông tin toạ độ thửa đất
    /// </returns>
    Task<Coordinates?> GetAsync(long maGcnElis, CancellationToken cancellationToken = default);
}

public class GeoService(
    IConnectionElisData connectionElisData,
    IThuaDatService thuaDatService,
    ILogger logger,
    IFusionCache fusionCache) : IGeoService
{

    private readonly string _apiSdeUrl = connectionElisData.ApiSdeUrl();
    public async Task<Result<Coordinates>> GetResultAsync(long maGcnElis, CancellationToken cancellationToken = default)
    {
        try
        {
            var coordinates = await GetAsync(maGcnElis, cancellationToken);
            if (coordinates is not null) return coordinates;
            logger.Error("Không tìm thấy toạ độ thửa trong cơ sở dữ liệu: {MaGcnElis}", maGcnElis);
            return new Result<Coordinates>(new Exception("Không tìm thấy toạ độ thửa trong cơ sở dữ liệu."));

        }
        catch (Exception e)
        {
            logger.Error(e, "Lỗi khi lấy thông tin toạ độ thửa: {MaGcnElis}", maGcnElis);
            return new Result<Coordinates>(e);
        }
    }
    public async Task<Coordinates?> GetAsync(long maGcnElis, CancellationToken cancellationToken = default)
    {
        try
        {
            var coordinates = await fusionCache.GetOrSetAsync($"ToaDoThua:{maGcnElis}", 
                await GetPointFromApiSdeAsync(maGcnElis, cancellationToken),
                token: cancellationToken);
            if (coordinates is not null) return coordinates;
        }
        catch (Exception e)
        {
            logger.Error(e, "Lỗi khi lấy thông tin toạ độ thửa: {MaGcnElis}", maGcnElis);
            throw;
        }
        return null;
    }
    
    private sealed record BodyResponse(string Status, string Message, Coordinates Data);
    private async Task<Coordinates?> GetPointFromApiSdeAsync(long maGcnElis, CancellationToken cancellationToken = default)
    {
        var connectionSqls = await connectionElisData.GetConnection(maGcnElis);
        if (connectionSqls.Count == 0) return null;
        var thuaDat  = await thuaDatService.GetThuaDatInDatabaseAsync(maGcnElis, cancellationToken);
        if (thuaDat is null) return null;
        var soTo = thuaDat.ToBanDo.Trim().ToLower();
        var soThua = thuaDat.ThuaDatSo.Trim().ToLower();
        var tyLe = thuaDat.TyLeBanDo;
        var kvhcId = thuaDat.MaDvhc;
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
                var req = new {database, SoTo = soTo, SoThua = soThua, TyLe = tyLe, KvhcId = kvhcId};
                var response = await httpClient.PostAsJsonAsync("/api/sde/toadothuadat", req, cancellationToken);
                if (!response.IsSuccessStatusCode) continue;
                var json = await response.Content.ReadFromJsonAsync<BodyResponse>(cancellationToken: cancellationToken);
                if (json is null || json.Status != "success") continue;
                return json.Data;
            }
            catch (Exception e)
            {
                logger.Error(e, "Lỗi khi lấy vị trí thửa đất trong SDE {MaGcnElis}, {SdeName}",
                    maGcnElis, name);
                throw;
            }
        }
        return null;
    }
}

public sealed class Coordinates
{
    public double X { get; init; }
    public double Y { get; init; }
}
