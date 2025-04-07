using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Settings;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using InterpolatedSql.Dapper;
using LanguageExt.Common;
using Microsoft.Extensions.Caching.Hybrid;
using ILogger = Serilog.ILogger;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Services;

public record TaiSanData(
    string IdTaiSan,
    string? NTen,
    string? DiaChi,
    string? GhiChu,
    double? NDienTichXayDung,
    double? NDienTichSan,
    string? NNamHtxd,
    string? NKetCau,
    string? NSoTang,
    string? NIdCCapHangNha,
    string? HHangMucCongTrinh,
    double? HDienTichXayDung,
    string? HDienTichSan,
    string? HKetCau,
    string? HSoTang,
    string? HNamHtxd,
    string? HIdCapHangCongTrinh,
    string? HHinhThucSoHuu,
    string? HThoiHanSoHuu
);

public class TaiSanService(
    IConnectionElisData connectionElisData,
    ILogger logger,
    HybridCache hybridCache) : ITaiSanService
{
    private async Task<string> GetTenCapCongTrinhAsync(string? idCapCongTrinh, string sqlConnectionString)
    {
        if (idCapCongTrinh is null) return string.Empty;
        try
        {
            await using var dbConnection = sqlConnectionString.GetConnection();
            var query = dbConnection.SqlBuilder(
                $"""
                 SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
                 SELECT tenCapCongTrinh 
                 FROM TS_CapCongTrinh 
                 WHERE idCapCongTrinh = {idCapCongTrinh}
                 """);
            var result = await query.QueryFirstOrDefaultAsync<string?>();
            return result ?? string.Empty;
        }
        catch (Exception e)
        {
            logger.Error(e, "Lỗi lấy tên cấp công trình");
            return string.Empty;
        }
    }

    private async Task<string> GetTenCapHangNhaAsync(string? idCapHangNha, string sqlConnectionString)
    {
        if (idCapHangNha is null) return string.Empty;
        try
        {
            await using var dbConnection = sqlConnectionString.GetConnection();
            var query = dbConnection.SqlBuilder(
                $"""
                 SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
                 SELECT tenCapHangNha
                 FROM TS_CapHangNha
                 WHERE idCapHangNha = {idCapHangNha}
                 """);
            var result = await query.QueryFirstOrDefaultAsync<string?>();
            return result ?? string.Empty;
        }
        catch (Exception e)
        {
            logger.Error(e, "Lỗi lấy tên cấp hạng nhà");
            return string.Empty;
        }
    }
    
    
    public async Task<List<TaiSan>> GetTaiSanInDataBaseAsync(string? serial, List<long> dsMaThuaDat, List<long> dsMaChuSuDung)
    {
        serial = serial.ChuanHoa();
        if (string.IsNullOrWhiteSpace(serial) ||
            dsMaThuaDat.Count == 0 || 
            dsMaChuSuDung.Count == 0) return [];
        var connectionSqls = await connectionElisData.GetConnectionAsync(serial);
        if (connectionSqls is null) return [];
        try
        {
            var sqlConnectionString = connectionSqls.ElisConnectionString;
            var maThuaDatString = string.Join(",", dsMaThuaDat.Select(x => x.ToString()));
            var maChuSuDungString = string.Join(",", dsMaChuSuDung.Select(x => x.ToString()));
            await using var dbConnection = sqlConnectionString.GetConnection();
            var query = dbConnection.SqlBuilder(
                $"""
                 SELECT DISTINCT TS.idTaiSan AS IdTaiSan, 
                        TS.nTen AS NTen,
                        TS.diaChi AS DiaChi,
                        TS.GhiChu AS GhiChu,
                        TS.nDienTichXayDung AS NDienTichXayDung,
                        TS.nDienTichSan AS NDienTichSan,
                        TS.nNamHTXD AS NNamHtxd,
                        TS.nKetCau AS NKetCau,
                        TS.nSoTang AS NSoTang,
                        TS.nIdCapHangNha AS NIdCCapHangNha,
                        TS.hHangMucCongTrinh AS HHangMucCongTrinh,
                        TS.hDienTichXayDung AS HDienTichXayDung,
                        TS.hDienTichSan AS HDienTichSan,
                        TS.hKetCau AS HKetCau,
                        TS.hSoTang AS HSoTang,
                        TS.hNamHTXD AS HNamHtxd,
                        TS.hIdCapHangCongTrinh AS HIdCapHangCongTrinh,
                        TS.hHinhThucSoHuu AS HHinhThucSoHuu,
                        TS.hThoiHanSoHuu AS HThoiHanSoHuu
                 FROM TS_TaiSan TS 
                 WHERE (TS.idTaiSan IN (SELECT idTaiSan 
                     FROM TS_ChuSuDung_TaiSan 
                     WHERE idChuSuDung IN ({maChuSuDungString})))
                   AND (TS.idTaiSan IN (
                       SELECT idTaiSan
                       FROM TS_ThuaDat_TaiSan
                       WHERE idThuaDat IN ({maThuaDatString})))
                 """);
            var dsTaiSanInData = await query.QueryAsync<TaiSanData>();
            List<TaiSan> result = [];
            foreach (var taiSanInData in dsTaiSanInData)
            {
                var capHang = string.IsNullOrWhiteSpace(taiSanInData.NIdCCapHangNha) ?
                    await GetTenCapCongTrinhAsync(taiSanInData.HIdCapHangCongTrinh, sqlConnectionString) :
                    await GetTenCapHangNhaAsync(taiSanInData.NIdCCapHangNha, sqlConnectionString);
                if (double.TryParse(taiSanInData.NDienTichXayDung.ToString(), out var dienTichXayDung))
                    dienTichXayDung = Math.Round(dienTichXayDung,2);
                if (dienTichXayDung == 0)
                {
                    if (double.TryParse(taiSanInData.HDienTichXayDung.ToString(), out var dienTichXayDung1))
                        dienTichXayDung = Math.Round(dienTichXayDung1,2);
                }
                if (double.TryParse(taiSanInData.NDienTichSan.ToString(), out var dienTichSan))
                    dienTichSan = Math.Round(dienTichSan,2);
                if (dienTichSan == 0)
                {
                    if (double.TryParse(taiSanInData.HDienTichSan, out var dienTichSan1))
                        dienTichSan = Math.Round(dienTichSan1,2);
                }

                var taiSan = new TaiSan(
                    IdTaiSan: taiSanInData.IdTaiSan,
                    TenTaiSan: string.IsNullOrWhiteSpace(taiSanInData.NTen)
                        ? taiSanInData.HHangMucCongTrinh
                        : taiSanInData.NTen,
                    DiaChi: taiSanInData.DiaChi,
                    DienTichXayDung: dienTichXayDung,
                    DienTichSan: dienTichSan,
                    KetCau: string.IsNullOrWhiteSpace(taiSanInData.NKetCau)
                        ? taiSanInData.HKetCau
                        : taiSanInData.NKetCau,
                    CapHang: capHang,
                    NamHoanThanhXayDung: string.IsNullOrWhiteSpace(taiSanInData.NNamHtxd)
                        ? taiSanInData.HNamHtxd
                        : taiSanInData.NNamHtxd,
                    HinhThucSoHuu: taiSanInData.HHinhThucSoHuu,
                    ThoiHanSoHuu: taiSanInData.HThoiHanSoHuu,
                    SoTang: string.IsNullOrWhiteSpace(taiSanInData.NSoTang)
                        ? taiSanInData.HSoTang
                        : taiSanInData.NSoTang,
                    GhiChu: taiSanInData.GhiChu);
                result.Add(taiSan);
            }
            return result;
        }
        catch (Exception e)
        {
            logger.Error(e, "Lỗi truy vấn dữ liệu từ ELIS database {Database}",connectionSqls.Name);
            return [];
        }
    }
    
    public async Task<Result<List<TaiSan>>> GetTaiSanAsync(string? serial, List<long> dsMaThuaDat, List<long> dsMaChuSuDung)
    {
        serial = serial.ChuanHoa();
        if (serial is null || dsMaThuaDat.Count == 0 || dsMaChuSuDung.Count == 0)
            return new Result<List<TaiSan>>(new ArgumentException("Thông tin tìm kiếm không hợp lệ"));
        var cacheKey = CacheSettings.KeyTaiSan(serial);
        var results = await hybridCache.GetOrCreateAsync(cacheKey, 
            async _ => await GetTaiSanInDataBaseAsync(serial, dsMaThuaDat, dsMaChuSuDung),
            tags:[serial]);
        return results.Count == 0 ?
            new Result<List<TaiSan>>(new Exception("Không có kết quả")) : 
            new Result<List<TaiSan>>(results);
    }
    
    public async Task SetCacheAsync(string? serial, List<long> dsMaThuaDat, List<long> dsMaChuSuDung)
    {
        serial = serial.ChuanHoa();
        if (serial is null || dsMaThuaDat.Count == 0 || dsMaChuSuDung.Count == 0) return;
        await hybridCache.SetAsync(CacheSettings.KeyTaiSan(serial),
            await GetTaiSanInDataBaseAsync(serial, dsMaThuaDat, dsMaChuSuDung),
            tags: [serial]);
    }
}