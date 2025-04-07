using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Settings;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using InterpolatedSql.Dapper;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.Extensions.Caching.Hybrid;
using Exception = System.Exception;
using ILogger = Serilog.ILogger;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Services;

public class ThuaDatService(
    IConnectionElisData connectionElisData,
    ILogger logger,
    HybridCache hybridCache) : IThuaDatService
{
    /// <summary>
    /// Lấy thông tin Thửa đất theo Serial của Giấy chứng nhận.
    /// </summary>
    /// <param name="serial"> Serial của Giấy chứng nhận.</param>
    /// <param name="cancellationToken">Token hủy bỏ tác vụ không bắt buộc.</param>
    /// <returns>Kết quả chứa thông tin Thửa đất hoặc lỗi nếu không tìm thấy.</returns>
    public async Task<Result<List<ThuaDat>>> GetResultAsync(string serial = "",
        CancellationToken cancellationToken = default)
    {
        serial = serial.ChuanHoa() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(serial))
        {
            return new Result<List<ThuaDat>>(new ArgumentException("Tham số truy vấn không hợp lệ!"));
        }
        var cacheKey = CacheSettings.KeyThuaDat(serial);
        try
        {
            var thuaDats = await hybridCache.GetOrCreateAsync(cacheKey,
                async cancel => await GetThuaDatInDatabaseAsync(serial, cancel),
                tags: [serial],
                cancellationToken: cancellationToken);
            return thuaDats.Count == 0 ? 
                new Result<List<ThuaDat>>(new ValueIsNullException("Không tìm thấy thông tin thửa đất!")) : 
                thuaDats;
        }
        catch (Exception exception)
        {
            return new Result<List<ThuaDat>>(exception);
        }
    }
    /// <summary>
    /// Lấy thông tin Thửa đất theo Serial của Giấy chứng nhận.
    /// </summary>
    /// <param name="serial"> Serial của Giấy chứng nhận.</param>
    /// <param name="cancellationToken">Token hủy bỏ tác vụ không bắt buộc.</param>
    /// <returns>Kết quả chứa thông tin Thửa đất hoặc lỗi nếu không tìm thấy.</returns>
    public async Task<List<ThuaDat>> GetAsync(string serial = "", CancellationToken cancellationToken = default)
    {
        serial = serial.ChuanHoa() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(serial))
        {
            return [];
        }
        var cacheKey = CacheSettings.KeyThuaDat(serial);
        try
        {
            var thuaDats = await hybridCache.GetOrCreateAsync(cacheKey,
                async cancel => await GetThuaDatInDatabaseAsync(serial, cancel),
                tags: [serial],
                cancellationToken: cancellationToken);
            return thuaDats;
        }
        catch (Exception exception)
        {
            logger.Error(exception, "Lỗi khi lấy thông tin Thửa đất theo Serial: {Serial}", serial);
            return [];
        }
    }

    public async Task<List<long>> GetMaThuaDatAsync(string serial, CancellationToken cancellationToken = default)
    {
        // Lấy danh sách thửa đất theo Serial 
        var dsThuaDat = await GetAsync(serial, cancellationToken);
        
        // Trả về danh sách mã thửa đất
         return dsThuaDat.Select(x => x.MaThuaDat).ToList();
    }

    /// <summary>
    /// Lấy thông tin Thửa đất theo Serial của Giấy chứng nhận từ cơ sở dữ liệu.
    /// </summary>
    /// <param name="serial"> Serial của Giấy chứng nhận.</param>
    /// <param name="cancellationToken"> Token hủy bỏ tác vụ không bắt buộc.</param>
    /// <returns>Kết quả chứa thông tin Thửa đất hoặc lỗi nếu không tìm thấy.</returns>
    public async Task<List<ThuaDat>> GetThuaDatInDatabaseAsync(string serial = "",
        CancellationToken cancellationToken = default)
    {
        serial = serial.ChuanHoa() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(serial)) return [];
        var connectionSqls = await connectionElisData.GetAllConnectionAsync(serial);

        List<ThuaDat> thuaDats = [];
        foreach (var connectionString in connectionSqls.Select(x => x.ElisConnectionString))
        {
            thuaDats.AddRange(await GetThuaDatInDataBaseAsync(serial, connectionString,cancellationToken: cancellationToken));
        }
        thuaDats = thuaDats.Distinct().ToList();
        return thuaDats;

    }

    private async Task<List<ThuaDat>> GetThuaDatInDataBaseAsync(string serial, string connectionString,
        CancellationToken cancellationToken = default)
    {
        serial = serial.ChuanHoa() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(serial)) return [];
        serial = serial.ToLower();
        List<ThuaDat> result = [];
        try
        {
            await using var dbConnection = connectionString.GetConnection();
            var query = dbConnection.SqlBuilder(
                $"""
                 SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
                 SELECT DISTINCT TBD.SoTo AS SoTo, 
                        TBD.TyLe AS TyLe,
                        TD.MaThuaDat AS MaThuaDat,
                        TD.ThuaDatSo AS SoThua, 
                        TBD.MaDVHC AS maDvhc,
                        TD.DiaChi AS DiaChi,
                        TBD.GhiChu AS GhiChu,
                        GCN.MaGCN AS MaGcn,
                        GCN.DienTichRieng AS DienTichRieng,
                        GCN.DienTichChung AS DienTichChung
                 FROM ThuaDat TD 
                     INNER JOIN ToBanDo TBD ON TD.MaToBanDo = TBD.MaToBanDo 
                     INNER JOIN DangKyQSDD DK ON TD.MaThuaDat = DK.MaThuaDat
                     INNER JOIN GCNQSDD GCN ON DK.MaDangKy = GCN.MaDangKy
                 WHERE LOWER(LTRIM(RTRIM(GCN.SoSerial))) = {serial}
                 """);
            var thuaDatToBanDos = (await query.QueryAsync(cancellationToken: cancellationToken))
                .ToList();
            var mucDichService = new MucDichAndHinhThucService(connectionString, logger);
            var nguonGocService = new NguonGocService(connectionString, logger);
            foreach (var thuaDatToBanDo in thuaDatToBanDos)
            {
                if (!int.TryParse(thuaDatToBanDo.maDvhc.ToString(), out int maDvhc)) return [];
                if (!long.TryParse(thuaDatToBanDo.MaThuaDat.ToString(), out long maThuaDat)) return [];
                string diaChi = thuaDatToBanDo.DiaChi;
                diaChi =
                    $"{(string.IsNullOrWhiteSpace(diaChi) ? "" : $"{diaChi}, ")}{await GetDiaChiByMaDvhcAsync(maDvhc, connectionString, cancellationToken)}";
                string soThua = thuaDatToBanDo.SoThua.ToString().Trim();
                string toBanDo = thuaDatToBanDo.SoTo.ToString().Trim();
                int.TryParse(thuaDatToBanDo.TyLe.ToString(), out int tyLe);
                string ghiChu = thuaDatToBanDo.GhiChu?.ToString() ?? string.Empty;
                var mucDichSuDung = await mucDichService.GetMucDichSuDungAsync(thuaDatToBanDo.MaGcn, cancellationToken);
                var nguonGoc = await nguonGocService.GetNguonGocSuDungAsync(thuaDatToBanDo.MaGcn, cancellationToken);
                if(result.Any(x => x.MaDvhc == maDvhc && x.ThuaDatSo == soThua && x.ToBanDo == toBanDo)) continue;
                result.Add(new ThuaDat(
                    maThuaDat,
                    thuaDatToBanDo.MaGcn,
                    maDvhc,
                    soThua.Replace(".", string.Empty).Replace(",", string.Empty),
                    toBanDo,
                    tyLe,
                    diaChi.Trim().VietHoaDauChuoi(),
                    $"{(thuaDatToBanDo.DienTichRieng + thuaDatToBanDo.DienTichChung).ToString("##.0")} m²",
                    mucDichSuDung.Item1,
                    mucDichSuDung.Item2,
                    mucDichSuDung.Item3,
                    nguonGoc,
                    ghiChu.Trim().VietHoaDauChuoi()
                ));
            }

            return result;
        }
        catch (Exception e)
        {
            logger.Error(e, "Lỗi khi lấy thông tin Thửa đất theo Serial: {Serial}", serial);
            throw;
        }
    }

    private async ValueTask<string> GetDiaChiByMaDvhcAsync(int maDvhc, string connectionString,
        CancellationToken cancellationToken = default)
    {
        if (maDvhc <= 0) return string.Empty;
        var cacheKey = CacheSettings.KeyDiaChiByMaDvhc(maDvhc);
        try
        {
            var diaChi = await hybridCache.GetOrCreateAsync(cacheKey,
                async cancel => await GetDiaChiByMaDvhcAsyncInDataAsync(maDvhc, connectionString, cancel),
                cancellationToken: cancellationToken);
            return diaChi;
        }
        catch (Exception exception)
        {
            logger.Error(exception, "Lỗi khi truy vấn dữ liệu Đơn vị hành chính từ cơ sở dữ liệu, {MaDVHC}", maDvhc);
            return string.Empty;
        }
    }

    private async Task<string> GetDiaChiByMaDvhcAsyncInDataAsync(int maDvhc, string connectionString,
        CancellationToken cancellationToken = default)
    {
        if (maDvhc <= 0) return string.Empty;
        try
        {
            await using var dbConnection = connectionString.GetConnection();
            var query = dbConnection.SqlBuilder(
                $"""
                 SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
                 SELECT DVHCXa.Ten AS TenXa, DVHCHuyen.Ten AS TenHuyen, DVHCTinh.Ten AS TenTinh
                 FROM   DVHC DVHCXa 
                     	INNER JOIN DVHC AS DVHCHuyen ON DVHCXa.MaHuyen = DVHCHuyen.MaHuyen AND DVHCHuyen.MaXa = 0
                     	INNER JOIN DVHC AS DVHCTinh ON DVHCXa.MaTinh = DVHCTinh.MaTinh AND DVHCTinh.MaHuyen = 0
                 WHERE DVHCXa.MaDVHC = {maDvhc}
                 """);
            var diaChiData = await query.QuerySingleOrDefaultAsync<dynamic?>(cancellationToken: cancellationToken);
            if (diaChiData is null) return string.Empty;
            string tenXa = diaChiData.TenXa.ToString();
            string tenHuyen = diaChiData.TenHuyen.ToString();
            string tenTinh = diaChiData.TenTinh.ToString();
            return $"{tenXa.ChuanHoaTenDonViHanhChinh()}, " +
                   $"{tenHuyen.ChuanHoaTenDonViHanhChinh()}, " +
                   $"{tenTinh.ChuanHoaTenDonViHanhChinh()}";
        }
        catch (Exception e)
        {
            logger.Error(e, "Lỗi khi lấy thông tin Đơn vị hành chính theo Mã DVHC: {maDVHC}", maDvhc);
            throw;
        }
    }
}