using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Settings;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using InterpolatedSql.Dapper;
using LanguageExt;
using LanguageExt.Common;
using ZiggyCreatures.Caching.Fusion;
using ILogger = Serilog.ILogger;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Services;

public sealed class GiayChungNhanService(
    IConnectionElisData connectionElisData,
    ILogger logger,
    IFusionCache fusionCache) :
    IGiayChungNhanService
{
    /// <summary>
    /// Lấy thông tin Giấy chứng nhận theo số serial.
    /// </summary>
    /// <param name="serial">Số serial của Giấy chứng nhận.</param>
    /// <param name="maVach">Mã vạch của Giấy chứng nhận.</param>
    /// <param name="cancellationToken">Token hủy bỏ tác vụ không bắt buộc.</param>
    /// <returns>Kết quả chứa danh sách thông tin Giấy chứng nhận hoặc lỗi nếu không tìm thấy.</returns>
    /// <exception cref="ValueIsNullException">Ném ra ngoại lệ nếu không tìm thấy thông tin Giấy chứng nhận.</exception>
    public async Task<Result<GiayChungNhan>> GetResultAsync(string? serial = null, long maVach = 0,
        CancellationToken cancellationToken = default)
    {
        serial = serial.ChuanHoa();
        if (string.IsNullOrWhiteSpace(serial) && maVach <= 0)
            return new Result<GiayChungNhan>(new ValueIsNullException("Không tìm thấy thông tin Giấy chứng nhận!"));
        try
        {
            var cacheKey = CacheSettings.KeyGiayChungNhan(!string.IsNullOrWhiteSpace(serial) ? serial : CacheSettings.KeySerial(maVach.ToString()));
            List<string> tags = [];
            return await fusionCache.GetOrSetAsync(cacheKey,
                       async cancel =>
                       {
                           var giayChungNhan = await GetAsync(serial, maVach, cancel);
                           var serialInData = giayChungNhan?.Serial.ChuanHoa();
                           if (!string.IsNullOrWhiteSpace(serialInData))
                           {
                               tags = [serialInData];
                           }

                           return giayChungNhan;
                       },
                       tags: tags,
                       token: cancellationToken) ?? 
                   new Result<GiayChungNhan>(new ValueIsNullException("Không tìm thấy thông tin Giấy chứng nhận!"));
        }
        catch (Exception e)
        {
            logger.Error(e, "Lỗi khi lấy thông tin Giấy chứng nhận: {Serial} {MaVach}", serial, maVach);
            return new Result<GiayChungNhan>(e);
        }

    }
    
    /// <summary>
    /// Lấy thông tin Giấy chứng nhận theo số serial.
    /// </summary>
    /// <param name="serial">Số serial của Giấy chứng nhận.</param>
    /// <param name="maVach">Mã vạch của Giấy chứng nhận.</param>
    /// <param name="cancellationToken">Token hủy bỏ tác vụ không bắt buộc.</param>
    /// <returns>Kết quả chứa danh sách thông tin Giấy chứng nhận</returns>
    public async Task<GiayChungNhan?> GetAsync(string? serial = null, long maVach = 0,
        CancellationToken cancellationToken = default)
    {
        serial = serial.ChuanHoa()?.ToLower();
        if (string.IsNullOrWhiteSpace(serial) && maVach <= 0) return null;
        var maVachString = maVach > 0 ? maVach.ToString("0000000000000") : "-111";
        try
        {
            var giayChungNhan = string.IsNullOrWhiteSpace(serial) ? null : 
                await fusionCache.GetOrDefaultAsync<GiayChungNhan>(CacheSettings.KeyGiayChungNhan(serial),
                    token: cancellationToken);
            if (giayChungNhan is not null) return giayChungNhan;
            var connectionElis = await connectionElisData.GetAllConnection(serial);
            foreach (var (connectionName, _, elisConnectionString, _, _) in connectionElis)
            {
                await using var dbConnection = elisConnectionString.GetConnection();
                var query = dbConnection.SqlBuilder(
                    $"""
                                 SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
                                 SELECT MaGCN AS MaGcn, 
                                        MaDangKy AS MaDangKy, 
                                        MaHinhThucSH AS MaHinhThucSh,
                                        DienTichRieng,
                                        DienTichChung,
                                        SoSerial AS Serial, 
                                        CASE WHEN NgayKy < '1990-01-01' 
                                            THEN NgayVaoSo 
                                            ELSE NgayKy 
                                        END AS NgayKy, 
                                        NguoiKy, 
                                        SoVaoSo,
                                        MaHoSoDVC,
                                        MaDonViInGCN,
                                        MaVach
                                 FROM GCNQSDD
                                 WHERE 
                                     (SoSerial IS NOT NULL AND LEN(SoSerial) > 0 AND (LOWER(LTRIM(RTRIM(SoSerial))) = {serial})) OR 
                                     (MaVach = {maVachString})
                            """);
                giayChungNhan = await query.QueryFirstOrDefaultAsync<GiayChungNhan?>(cancellationToken: cancellationToken);
                if (giayChungNhan is null) continue;
                serial = giayChungNhan.Serial.ChuanHoa();
                if (string.IsNullOrWhiteSpace(serial)) continue;
                _ = fusionCache.SetAsync(CacheSettings.KeyGiayChungNhan(serial), giayChungNhan,
                    tags: [serial],
                    token: cancellationToken).AsTask();
                _ = fusionCache.SetAsync(CacheSettings.KeySerial(giayChungNhan.MaVach), serial,
                    tags: [serial],
                    token: cancellationToken).AsTask();
                _ = fusionCache.SetCacheConnectionName(connectionName, serial, cancellationToken);
                return giayChungNhan;
            }
        
        }
        catch (Exception e)
        {
            logger.Error(e, "Lỗi khi lấy thông tin Giấy chứng nhận: {Serial} {MaVach}", serial, maVach);
            throw;
        }

        return null;
    }
    
}