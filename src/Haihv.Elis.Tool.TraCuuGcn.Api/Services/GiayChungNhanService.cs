using Haihv.Elis.Tool.TraCuuGcn.Api.Exceptions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Settings;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using InterpolatedSql.Dapper;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.Extensions.Caching.Hybrid;
using ILogger = Serilog.ILogger;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Services;

public sealed class GiayChungNhanService(
    IConnectionElisData connectionElisData,
    ILogger logger,
    HybridCache hybridCache) :
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
        if (string.IsNullOrWhiteSpace(serial) && maVach <= 0)
            return new Result<GiayChungNhan>(new ValueIsNullException("Không tìm thấy thông tin Giấy chứng nhận!"));
        try
        {
            serial = string.IsNullOrWhiteSpace(serial) ? CacheSettings.KeySerial(maVach.ToString()) : serial.ChuanHoa();
            if (string.IsNullOrWhiteSpace(serial))
                return new Result<GiayChungNhan>(new ValueIsNullException("Không tìm thấy thông tin Giấy chứng nhận!"));
            var cacheKey = CacheSettings.KeyGiayChungNhan(serial);
            List<string> tags = [];
            return await hybridCache.GetOrCreateAsync(cacheKey,
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
                       cancellationToken: cancellationToken) ?? 
                   new Result<GiayChungNhan>(string.IsNullOrWhiteSpace(serial) ?
                       new GiayChungNhanNotFoundException(maVach) :
                       new GiayChungNhanNotFoundException(serial));
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
        serial = serial.ChuanHoa();
        if (string.IsNullOrWhiteSpace(serial) && maVach <= 0) return null;
        var maVachString = maVach > 0 ? maVach.ToString("0000000000000") : "-111";
        try
        {
            var connectionElis = await connectionElisData.GetAllConnectionAsync(serial);
            foreach (var (connectionName, _, elisConnectionString, _, _) in connectionElis)
            {
                var serialLowerCase = serial.ChuanHoaLowerCase();
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
                                 NgayVaoSo,
                                 MaHoSoDVC,
                                 MaDonViInGCN,
                                 MaVach
                          FROM GCNQSDD
                          WHERE 
                              (SoSerial IS NOT NULL AND LEN(SoSerial) > 0 AND (LOWER(LTRIM(RTRIM(SoSerial))) = {serialLowerCase})) OR 
                              (MaVach = {maVachString})
                     """);
                var giayChungNhan = await query.QueryFirstOrDefaultAsync<GiayChungNhan?>(cancellationToken: cancellationToken);
                if (giayChungNhan is null) continue;
                serial = giayChungNhan.Serial.ChuanHoa();
                if (string.IsNullOrWhiteSpace(serial)) continue;
                List<string> tags = [serial];
                _ = hybridCache.SetAsync(CacheSettings.KeyGiayChungNhan(serial), giayChungNhan,
                    tags: tags,
                    cancellationToken: cancellationToken).AsTask();
                _ = hybridCache.SetAsync(CacheSettings.KeySerial(giayChungNhan.MaVach), serial,
                    tags: tags,
                    cancellationToken: cancellationToken).AsTask();
                _ = hybridCache.SetCacheConnectionName(connectionName, serial, cancellationToken);
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
    
    public async Task<Result<bool>> UpdateAsync(PhapLyGiayChungNhan? phapLyGiayChungNhan, CancellationToken cancellationToken = default)
    {
        if (phapLyGiayChungNhan is null) 
            return new Result<bool>(new ArgumentNullException(nameof(phapLyGiayChungNhan), "Không tìm thấy thông tin Giấy chứng nhận!"));
        var serial = phapLyGiayChungNhan.Serial.ChuanHoa();
        if (string.IsNullOrWhiteSpace(serial) || phapLyGiayChungNhan.MaGcn <= 0)
            return new Result<bool>(new NullReferenceException("Không tìm thấy thông tin Serial hoặc Mã GCN!"));
        try
        {
            var connectionElis = await connectionElisData.GetConnectionAsync(serial);
            if (connectionElis is null) 
                return new Result<bool>(new NullReferenceException("Không tìm thấy thông tin kết nối cơ sở dữ liệu!"));
            await using var dbConnection = connectionElis.ElisConnectionString.GetConnection();
            var query = dbConnection.SqlBuilder(
                $"""
                      UPDATE GCNQSDD
                      SET NgayKy = {phapLyGiayChungNhan.NgayKy},
                          NguoiKy = {phapLyGiayChungNhan.NguoiKy},
                          SoVaoSo = {phapLyGiayChungNhan.SoVaoSo},
                          NgayVaoSo = {phapLyGiayChungNhan.NgayVaoSo}
                      WHERE UPPER(SoSerial) = {serial}
                 """);
            var count = await query.ExecuteAsync(cancellationToken: cancellationToken);
            if (count <= 0) 
                return new Result<bool>(new NullReferenceException("Không có Giấy chứng nhận nào được cập nhật!")); 
            _ = hybridCache.SetAsync(CacheSettings.KeyGiayChungNhan(serial), phapLyGiayChungNhan,
                tags: [serial],
                cancellationToken: cancellationToken).AsTask();
            return true;
        }
        catch (Exception e)
        {
            logger.Error(e, "Lỗi khi cập nhật thông tin Giấy chứng nhận: {Serial}", serial);
            return new Result<bool>(e);
        }
    }
    
}