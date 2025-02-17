using InterpolatedSql.Dapper;
using LanguageExt.Common;
using ZiggyCreatures.Caching.Fusion;
#pragma warning disable CS0618
using System.Data.SqlClient;
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
    public async Task<Result<Coordinates>> GetResultAsync(long maGcnElis, CancellationToken cancellationToken = default)
    {
        try
        {
            var coordinates = await fusionCache.GetOrSetAsync($"ToaDoThua:{maGcnElis}", 
                await GetPointInDatabaseAsync(maGcnElis, cancellationToken), 
                TimeSpan.FromDays(1), 
                token: cancellationToken);
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
                await GetPointInDatabaseAsync(maGcnElis, cancellationToken), 
                TimeSpan.FromDays(1), 
                token: cancellationToken);
            if (coordinates is not null) return coordinates;
        }
        catch (Exception e)
        {
            logger.Error(e, "Lỗi khi lấy thông tin toạ độ thửa: {MaGcnElis}", maGcnElis);
        }
        return null;
    }
    
    private async Task<Coordinates?> GetPointInDatabaseAsync(long maGcnElis, CancellationToken cancellationToken = default)
    {
        var connectionSqls = await connectionElisData.GetConnection(maGcnElis);
        if (connectionSqls.Count == 0) return null;
        var thuaDat  = await thuaDatService.GetThuaDatInDatabaseAsync(maGcnElis, cancellationToken);
        if (thuaDat is null) return null;
        var toBanDo = thuaDat.ToBanDo.Trim().ToLower();
        var thuaDatSo = thuaDat.ThuaDatSo.Trim().ToLower();
        var tyLe = thuaDat.TyLeBanDo;
        var maDvhc = thuaDat.MaDvhc;
        foreach (var (name, _, _, connectionString) in connectionSqls)
        {
            try
            {
                var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString)
                {
                    TrustServerCertificate = true,
                    Encrypt = false
                };
                await using var dbConnection = new SqlConnection(connectionStringBuilder.ConnectionString);
                await dbConnection.OpenAsync(cancellationToken);
                var query = dbConnection.SqlBuilder(
                    $"""
                     SELECT eminx,
                            eminy,
                            emaxx,
                            emaxy
                     FROM f18 
                        INNER JOIN THUADAT ON f18.fid = THUADAT.shape
                     WHERE LOWER(THUADAT.SOTO) = {toBanDo} AND 
                           LOWER(THUADAT.SOTHUA) = {thuaDatSo} AND 
                           THUADAT.TYLE = {tyLe} AND
                           THUADAT.KVHC_ID = {maDvhc}
                     """);
                var shape = await query.QueryFirstOrDefaultAsync(cancellationToken: cancellationToken);
                if (shape == null) continue;
                double eminy = 0;
                double emaxx = 0;
                double emaxy = 0;
                if (!double.TryParse(shape.eminx.ToString(), out double eminx) ||
                    !double.TryParse(shape.eminy.ToString(), out eminy) ||
                    !double.TryParse(shape.emaxx.ToString(), out emaxx) ||
                    !double.TryParse(shape.emaxy.ToString(), out emaxy)) continue;
                return new Coordinates((eminx + emaxx) / 2, (emaxy + eminy) / 2);
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

public record Coordinates(double X, double Y);
