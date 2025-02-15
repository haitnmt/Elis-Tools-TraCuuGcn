using Haihv.Elis.Tool.TraCuuGcn.Api.Models;
using InterpolatedSql.Dapper;
using LanguageExt.Common;
using OSGeo.OGR;
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
    /// <returns></returns>
    Task<Result<FeatureCollectionModel>> GetResultAsync(long maGcnElis, CancellationToken cancellationToken = default);
}

public class GeoService(
    IConnectionElisData connectionElisData,
    IThuaDatService thuaDatService,
    ILogger logger,
    IFusionCache fusionCache) : IGeoService
{
    public async Task<Result<FeatureCollectionModel>> GetResultAsync(long maGcnElis, CancellationToken cancellationToken = default)
    {
        try
        {
            var geometry = await fusionCache.GetOrSetAsync($"GetPointInDatabaseAsync_{maGcnElis}", 
                await GetPointInDatabaseAsync(maGcnElis, cancellationToken), 
                TimeSpan.FromDays(1), 
                token: cancellationToken);
            if (geometry is null)
            {
                logger.Error("Không tìm thấy toạ độ thửa trong cơ sở dữ liệu: {MaGcnElis}", maGcnElis);
                return new Result<FeatureCollectionModel>(new Exception("Không tìm thấy toạ độ thửa trong cơ sở dữ liệu."));
            };
            return new Result<FeatureCollectionModel>(new FeatureCollectionModel([geometry]));
        }
        catch (Exception e)
        {
            logger.Error(e, "Lỗi khi lấy thông tin toạ độ thửa: {MaGcnElis}", maGcnElis);
            return new Result<FeatureCollectionModel>(e);
        }
    }
    private async Task<Geometry?> GetPointInDatabaseAsync(long maGcnElis, CancellationToken cancellationToken = default)
    {
        var connectionSqls = await connectionElisData.GetConnection(maGcnElis);
        if (connectionSqls.Count == 0) return null;
        var thuaDat  = await thuaDatService.GetThuaDatInDatabaseAsync(maGcnElis, cancellationToken);
        if (thuaDat is null) return null;
        var toBanDo = thuaDat.ToBanDo.Trim().ToLower();
        var thuaDatSo = thuaDat.ThuaDatSo.Trim().ToLower();
        var tyLe = thuaDat.TyLeBanDo;
        var maDvhc = thuaDat.MaDvhc;
        foreach (var connectionString in connectionSqls.Select(x => x.SdeConnectionString))
        {
            await using var dbConnection = connectionString.GetConnection();
            var query = dbConnection.SqlBuilder(
                $"""
                 SELECT eminx,
                        eminy,
                        emaxx,
                        emaxy
                 FROM f18 
                    INNER JOIN THUADAT ON f18.fid = THUADAT.shape
                 WHERE TRIM(LOWER(THUADAT.SOTO)) = {toBanDo} AND 
                       TRIM(LOWER(THUADAT.SOTHUA)) = {thuaDatSo} AND 
                       THUADAT.TYLE = {tyLe} AND
                       THUADAT.KVHC_ID = {maDvhc}
                 """);
            var shapes = (await query.QueryAsync(cancellationToken: cancellationToken)).ToList();
            if (shapes.Count == 0) continue;
            var shape = shapes.First();
            double eminy = 0;
            double emaxx = 0;
            double emaxy = 0;
            if (!double.TryParse(shape.eminx.ToString(), out double eminx) ||
                !double.TryParse(shape.eminy.ToString(), out eminy) ||
                !double.TryParse(shape.emaxx.ToString(), out emaxx) ||
                !double.TryParse(shape.emaxy.ToString(), out emaxy)) continue;
            var geometry = new Geometry(wkbGeometryType.wkbPoint);
            geometry.AddPoint((eminx + emaxx) / 2,  (emaxy + eminy) / 2, 0);
            return geometry;
        }
        return null;
    }
}