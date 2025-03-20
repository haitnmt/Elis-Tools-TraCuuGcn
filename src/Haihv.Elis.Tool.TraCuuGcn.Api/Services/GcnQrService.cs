﻿using System.Data;
using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Settings;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using Haihv.Elis.Tool.TraCuuGcn.Models.Extensions;
using InterpolatedSql.Dapper;
using LanguageExt;
using LanguageExt.Common;
using ZiggyCreatures.Caching.Fusion;
using ILogger = Serilog.ILogger;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Services;

public interface IGcnQrService
{
    /// <summary>
    /// Lấy thông tin Mã QR không đồng bộ.
    /// </summary>
    /// <param name="maQr">Mã QR cần truy vấn.</param>
    /// <param name="hashQr">Mã QR đã được băm.</param>
    /// <param name="serial"> Số Serial của Giấy chứng nhận.</param>
    /// <param name="cancellationToken">Token để hủy bỏ thao tác không đồng bộ.</param>
    /// <returns>Kết quả chứa thông tin Mã QR nếu tìm thấy, ngược lại trả về ngoại lệ.</returns>
    ValueTask<Result<MaQrInfo>> GetResultAsync(string? maQr = null, string? hashQr = null, string? serial = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy thông tin Mã QR không đồng bộ.
    /// </summary>
    /// <param name="maQr">Mã QR cần truy vấn.</param>
    /// <param name="hashQr">Mã QR đã được băm.</param>
    /// <param name="serial"> Số Serial của Giấy chứng nhận.</param>
    /// <param name="cancellationToken">Token để hủy bỏ thao tác không đồng bộ.</param>
    /// <returns>Kết quả chứa thông tin Mã QR nếu tìm thấy</returns>
    /// <exception cref="Exception">Ném ra ngoại lệ nếu có lỗi xảy ra trong quá trình truy vấn cơ sở dữ liệu.</exception>
    ValueTask<MaQrInfo?> GetAsync(string? maQr = null, string? hashQr = null, string? serial = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy thông tin tên đơn vị dựa trên mã đơn vị từ cơ sở dữ liệu.
    /// </summary>
    /// <param name="maDonVi">Mã định danh của đơn vị.</param>
    /// <param name="cancellationToken">Token để hủy bỏ thao tác không đồng bộ.</param>
    /// <returns>Tên đơn vị nếu tìm thấy, ngược lại trả về null.</returns>
    /// <exception cref="Exception">Ném ra ngoại lệ nếu có lỗi xảy ra trong quá trình truy vấn cơ sở dữ liệu.</exception>
    Task<string?> GetTenDonViInDataBaseAsync(string? maDonVi, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Xóa thông tin Mã QR khỏi CSDL
    /// </summary>
    /// <param name="serial">
    /// Số Serial của Giấy chứng nhận.
    /// </param>
    /// <param name="cancellationToken">
    /// Token để hủy bỏ thao tác không đồng bộ.
    /// </param>
    /// <returns>
    /// Kết quả xóa thông tin Mã QR khỏi CSDL.
    /// </returns>
    Task<Result<bool>> DeleteMaQrAsync(string? serial = null, CancellationToken cancellationToken = default);
}

public sealed class GcnQrService(IConnectionElisData connectionElisData, ILogger logger, IFusionCache fusionCache) : IGcnQrService
{
    private readonly List<ConnectionSql> _connectionElis = connectionElisData.ConnectionElis;

    /// <summary>
    /// Lấy thông tin Mã QR không đồng bộ.
    /// </summary>
    /// <param name="maQr">Mã QR cần truy vấn.</param>
    /// <param name="hashQr">Mã QR đã được băm.</param>
    /// <param name="serial"> Số Serial của Giấy chứng nhận.</param>
    /// <param name="cancellationToken">Token để hủy bỏ thao tác không đồng bộ.</param>
    /// <returns>Kết quả chứa thông tin Mã QR nếu tìm thấy, ngược lại trả về ngoại lệ.</returns>
    public async ValueTask<Result<MaQrInfo>> GetResultAsync(string? maQr= null, string? hashQr = null, string? serial = null, CancellationToken cancellationToken = default)
    {
        try
        {
            return await GetAsync(maQr, hashQr, serial, cancellationToken) ?? 
                   new Result<MaQrInfo>(new ValueIsNullException("Không tìm thấy thông tin Mã QR!"));
        }
        catch (Exception e)
        {
            logger.Error(e, "Lỗi khi lấy thông tin Mã QR: {MaQr}", maQr);
            return new Result<MaQrInfo>(e);
        }

    }
    
    /// <summary>
    /// Lấy thông tin Mã QR từ cơ sở dữ liệu.
    /// </summary>
    /// <param name="maQr">Mã QR cần truy vấn.</param>
    /// <param name="hashQr">Mã QR đã được băm.</param>
    /// <param name="serial"> Số Serial của Giấy chứng nhận.</param>
    /// <param name="cancellationToken">Token để hủy bỏ thao tác không đồng bộ.</param>
    /// <returns>Tuple chứa mã QR đã băm và thông tin Mã QR nếu tìm thấy, ngược lại trả về null.</returns>
    /// <exception cref="Exception">Ném ra ngoại lệ nếu có lỗi xảy ra trong quá trình truy vấn cơ sở dữ liệu.</exception>
    public async ValueTask<MaQrInfo?> GetAsync(string? maQr = null, string? hashQr = null, string? serial = null, 
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(maQr) && string.IsNullOrWhiteSpace(hashQr) && string.IsNullOrWhiteSpace(serial)) return null;
        try
        {
            serial = serial?.ChuanHoa();
            var maQrInfo = string.IsNullOrWhiteSpace(serial) ? null :
                await fusionCache.GetOrDefaultAsync<MaQrInfo>(CacheSettings.KeyMaQr(serial), 
                    token: cancellationToken);
            if (maQrInfo is not null) return maQrInfo;
            var connectionElis = await connectionElisData.GetAllConnection(serial);
            hashQr = hashQr?.Trim().ToLower();
            maQr = maQr?.ChuanHoa();
            foreach (var connection in connectionElis)
            {
                await using var dbConnection = connection.ElisConnectionString.GetConnection();
                var query = dbConnection.SqlBuilder(
                    $"""
                             SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
                             SELECT GuidID AS Id,
                                    MaGCN AS MaGcn,
                                    MaQR AS MaQr,
                                    MaHoaQR AS HashQr,
                                    HieuLuc
                             FROM GCNQR
                             WHERE (UPPER(MaQR) = {maQr} OR LOWER(MaHoaQR) = {hashQr} OR MaGCN LIKE N'%|{serial}|%') AND MaGCN > 0
                             """);
                var qrInData = await query.QueryFirstOrDefaultAsync<dynamic?>(cancellationToken: cancellationToken);
                if (qrInData is null) continue;
                maQr = qrInData.MaQr;
                maQrInfo = maQr.ToMaQr();
                if (!string.IsNullOrWhiteSpace(maQrInfo.MaDonVi))
                {
                    maQrInfo.TenDonVi = await fusionCache.GetOrSetAsync(CacheSettings.KeyDonViInGcn(maQrInfo.MaDonVi),
                        cancel => GetTenDonViInDataBaseAsync(maQrInfo.MaDonVi, cancel),
                        token: cancellationToken);
                }
                maQrInfo.MaQrId = qrInData.Id;
                maQrInfo.MaGcnElis = qrInData.MaGcn;
                maQrInfo.HieuLuc = qrInData.HieuLuc.ToString() != "0";
                serial = maQrInfo.SerialNumber?.ChuanHoa();
                if (string.IsNullOrWhiteSpace(serial)) return maQrInfo;
                _ = fusionCache.SetAsync(CacheSettings.KeyMaQr(serial), maQrInfo,
                    TimeSpan.FromDays(1), 
                    tags: [maQrInfo.MaGcnElis.ToString()],
                    token: cancellationToken).AsTask();
                _ = fusionCache.SetAsync(CacheSettings.ElisConnectionName(serial), connection.Name,
                    TimeSpan.FromDays(1),
                    tags: [maQrInfo.MaGcnElis.ToString()],
                    token: cancellationToken).AsTask();
                return maQrInfo;
            }
            if (string.IsNullOrWhiteSpace(maQr)) return null;
            maQrInfo = maQr.ToMaQr();
            maQrInfo.MaGcnElis = 0;
            if (!string.IsNullOrWhiteSpace(maQrInfo.MaDonVi))
            {
                maQrInfo.TenDonVi = await fusionCache.GetOrSetAsync(CacheSettings.KeyDonViInGcn(maQrInfo.MaDonVi),
                    cancel => GetTenDonViInDataBaseAsync(maQrInfo.MaDonVi, cancel),
                    token: cancellationToken);
            }
            return maQrInfo;
        }
        catch (Exception exception)
        {
            logger.Error(exception, "Lỗi khi lấy thông tin Mã QR: {MaQr}", maQr);
            return null;
        }
    }
    
    /// <summary>
    /// Lấy thông tin tên đơn vị dựa trên mã đơn vị từ cơ sở dữ liệu.
    /// </summary>
    /// <param name="maDonVi">Mã định danh của đơn vị.</param>
    /// <param name="cancellationToken">Token để hủy bỏ thao tác không đồng bộ.</param>
    /// <returns>Tên đơn vị nếu tìm thấy, ngược lại trả về null.</returns>
    /// <exception cref="Exception">Ném ra ngoại lệ nếu có lỗi xảy ra trong quá trình truy vấn cơ sở dữ liệu.</exception>
    public async Task<string?> GetTenDonViInDataBaseAsync(string? maDonVi, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(maDonVi)) return null;
        try
        {
            foreach (var connectionString in _connectionElis.Select(x => x.ElisConnectionString))
            {
                await using var dbConnection = connectionString.GetConnection();
                var query = dbConnection.SqlBuilder($"SELECT Ten FROM DonViInGCN WHERE MaDinhDanh = {maDonVi}");
                return await query.QueryFirstOrDefaultAsync<string?>(cancellationToken: cancellationToken);
            }
        }
        catch (Exception exception)
        {
            logger.Error(exception, "Lỗi khi lấy thông tin Tên đơn vị: {MaDonVi}", maDonVi);
        }
        return null;
    }
    /// <summary>
    /// Xóa thông tin Mã QR khỏi CSDL
    /// </summary>
    /// <param name="serial">
    /// Số Serial của Giấy chứng nhận.
    /// </param>
    /// <param name="cancellationToken">
    /// Token để hủy bỏ thao tác không đồng bộ.
    /// </param>
    /// <returns>
    /// Kết quả xóa thông tin Mã QR khỏi CSDL.
    /// </returns>
    public async Task<Result<bool>> DeleteMaQrAsync(string? serial = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(serial)) 
            return new Result<bool>(new ArgumentNullException(nameof(serial)));
        try
        {
            var count = 0;
            var connectionSql = await connectionElisData.GetConnectionAsync(serial);
            if (connectionSql is null)
            {
                logger.Warning("Không tìm thấy thông tin kết nối cơ sở dữ liệu: {Serial}", serial);
                return new Result<bool>(new ValueIsNullException("Không tìm thấy thông tin kết nối cơ sở dữ liệu!"));
            }
            await using var dbConnection = connectionSql.ElisConnectionString.GetConnection();
            var serialLike = $"%|{serial.ChuanHoa()}||%";
            var selectQuery = dbConnection.SqlBuilder(
                $"""
                            SELECT DISTINCT 
                                GuidID AS GuidId,
                                MaGCN AS MaGcn
                            FROM GCNQR
                            WHERE MaQR LIKE {serialLike}
                        """);
            var qrInfos = (await selectQuery.QueryAsync<QrInfo>(cancellationToken: cancellationToken)).ToList();
            foreach (var (guidId, maGcn) in qrInfos)
            {
                var query = dbConnection.SqlBuilder(
                    $"""
                     DELETE FROM GCNQR WHERE GuidID = {guidId};
                     UPDATE [GCNQSDD] 
                         SET [MaHoSoDVC] = NULL, [MaDonViInGCN] = NULL, [SoSerial] = NULL, [DaCapGCN] = NULL
                         WHERE (MaGCN = {maGcn});
                     UPDATE [TS_ChuSuDung_TaiSan] 
                         SET [DaCapGCN] = NULL, [soSerial] = NULL, [namCap] = NULL
                         WHERE [soSerial]= {serial};
                     """
                );
                if (dbConnection.State == ConnectionState.Closed)
                    await dbConnection.OpenAsync(cancellationToken);
                var transaction = dbConnection.BeginTransaction();
                try
                {
                    count += await query.ExecuteAsync(transaction: transaction, cancellationToken: cancellationToken);
                    await transaction.CommitAsync(cancellationToken: cancellationToken);
                }
                catch (Exception e)
                {
                    // Rollback transaction
                    await transaction.RollbackAsync(cancellationToken);
                    logger.Error(e, "Lỗi khi xóa thông tin Mã QR: {Serial}", serial);
                    return new Result<bool>(e);
                }
            }

            if (count == 0)
            {
                logger.Warning("Không có thông tin Mã QR để xóa: {Serial}", serial);
                return new Result<bool>(new ArgumentNullException(nameof(serial)));
            }
            // Xóa cache
            _ = fusionCache.RemoveByTagAsync(serial, token: cancellationToken).AsTask();
            logger.Information("Xóa thông tin Mã QR thành công: {Serial}", serial);
            return true;
        } 
        catch (Exception e)
        {
            logger.Error(e, "Lỗi khi xóa thông tin Mã QR: {Serial}", serial);
            return new Result<bool>(e);
        }
    }
    private record QrInfo(Guid GuidId, long MaGcn);
}