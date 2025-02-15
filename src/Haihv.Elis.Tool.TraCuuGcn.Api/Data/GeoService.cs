using System.Drawing;
using InterpolatedSql.Dapper;
using ZiggyCreatures.Caching.Fusion;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Data;

public class GeoService(
    IConnectionElisData connectionElisData,
    IThuaDatService thuaDatService,
    ILogger logger,
    IFusionCache fusionCache)
{
    private async Task<Point?> GetPointInDatabaseAsync(long maGcnElis, CancellationToken cancellationToken = default)
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
                 WHERE TRIM(LOWER(THUADAT.SOTO)) = {thuaDat.ToBanDo} AND 
                       TRIM(LOWER(THUADAT.SOTHUA)) = {thuaDat.ThuaDatSo} AND 
                       THUADAT.TYLE = {thuaDat.TyLeBanDo} AND
                       THUADAT.KVHC_ID = {thuaDat.MaDvhc}
                 """);
            var shapes = (await query.QueryAsync(cancellationToken: cancellationToken)).ToList();
            if (shapes.Count == 0) continue;
            var shape = shapes.First();
            if (!double.TryParse(shape.eminx.ToString(), out double eminx) ||
                !double.TryParse(shape.eminy.ToString(), out double eminy) ||
                !double.TryParse(shape.emaxx.ToString(), out double emaxx) ||
                !double.TryParse(shape.emaxy.ToString(), out double emaxy)) continue;
            var x = (emaxx + eminx) / 2;
            var y = (emaxy + eminy) / 2;
            return new Point(x, y);
        }
        return null;
    }
}
public record Point(double X, double Y);