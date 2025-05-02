using Haihv.Elis.Tool.TraCuuGcn.Api.Exceptions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Settings;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using InterpolatedSql.Dapper;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.Extensions.Caching.Hybrid;
using Newtonsoft.Json;
using ILogger = Serilog.ILogger;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Services;

public sealed class ChuSuDungService(
    IConnectionElisData connectionElisData,
    ILogger logger,
    HybridCache hybridCache) : IChuSuDungService
{
    #region Xác thực chủ sử dụng

    /// <summary>
    /// Lấy thông tin chủ sử dụng theo số định danh.
    /// </summary>
    /// <param name="serial"> Số Serial của Giấy chứng nhận.</param>
    /// <param name="soDinhDanh">Số định danh.</param>
    /// <param name="cancellationToken">Token hủy bỏ.</param>
    /// <returns>Kết quả chứa thông tin chủ sử dụng hoặc lỗi.</returns>
    public async Task<Result<AuthChuSuDung>> GetResultAuthChuSuDungAsync(string? serial = null,
        string? soDinhDanh = null, CancellationToken cancellationToken = default)
    {
        serial = serial?.ChuanHoa();
        if (string.IsNullOrWhiteSpace(soDinhDanh) || string.IsNullOrWhiteSpace(serial))
            return new Result<AuthChuSuDung>(new ValueIsNullException("Số định danh không hợp lệ!"));
        var cacheKey = CacheSettings.KeyAuthentication(serial, soDinhDanh);
        try
        {
            var tenChuSuDung = await hybridCache.GetOrCreateAsync(cacheKey,

                cancel => GetTenChuSuDungInDataAsync(serial, soDinhDanh, cancel),
                tags: [serial],
                cancellationToken: cancellationToken);
            return string.IsNullOrWhiteSpace(tenChuSuDung)
                ? new Result<AuthChuSuDung>(new ValueIsNullException("Không tìm thấy chủ sử dụng!"))
                : new AuthChuSuDung(serial, soDinhDanh, tenChuSuDung);
        }
        catch (Exception exception)
        {
            logger.Error(exception, "Lỗi khi truy vấn dữ liệu chủ sử dụng từ cơ sở dữ liệu, {SoDDinhDanh}", soDinhDanh);
            return new Result<AuthChuSuDung>(exception);
        }
    }

    /// <summary>
    /// Lấy thông tin chủ sử dụng từ cơ sở dữ liệu theo số định danh và giấy chứng nhận.
    /// </summary>
    /// <param name="serial"> Số Serial của Giấy chứng nhận.</param>
    /// <param name="soDinhDanh">Số định danh.</param>
    /// <param name="cancellationToken">Token hủy bỏ.</param>
    /// <returns>
    /// Tên chủ sử dụng hoặc null nếu không tìm thấy.
    /// </returns>
    private async ValueTask<string?> GetTenChuSuDungInDataAsync(
        string? serial = null,
        string? soDinhDanh = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(soDinhDanh) || string.IsNullOrWhiteSpace(serial))
            return null;
        try
        {
            serial = serial.ChuanHoa();
            var connectionElis = await connectionElisData.GetAllConnectionAsync(serial);
            foreach (var connection in connectionElis)
            {
                await using var dbConnection = connection.ElisConnectionString.GetConnection();
                var query = dbConnection.SqlBuilder(
                    $"""
                     SELECT TOP(1) HoVaTen
                     FROM (SELECT DISTINCT CSD.Ten1 AS HoVaTen
                           FROM ChuSuDung CSD
                                INNER JOIN GCNQSDD GCN ON CSD.MaChuSuDung = GCN.MaChuSuDung
                            WHERE LOWER(CSD.SoDinhDanh1) = LOWER({soDinhDanh}) AND UPPER(GCN.SoSerial) = {serial}
                           UNION
                           SELECT DISTINCT CSD.Ten2 AS HoVaTen
                           FROM ChuSuDung CSD
                                INNER JOIN GCNQSDD GCN ON CSD.MaChuSuDung = GCN.MaChuSuDung
                            WHERE LOWER(CSD.SoDinhDanh2) = LOWER({soDinhDanh}) AND UPPER(GCN.SoSerial) = {serial}
                            ) AS CSD
                     """);
                var tenChuSuDung =
                    await query.QueryFirstOrDefaultAsync<string?>(cancellationToken: cancellationToken);
                if (string.IsNullOrWhiteSpace(tenChuSuDung)) continue;
                return tenChuSuDung;
            }
        }
        catch (Exception exception)
        {
            logger.Error(exception, "Lỗi khi truy vấn dữ liệu chủ sử dụng từ cơ sở dữ liệu, {SoDDinhDanh}", soDinhDanh);
            throw;
        }

        return null;
    }
    
    public async Task<List<long>> GetMaChuSuDungAsync(string? serial = null,
        CancellationToken cancellationToken = default)
    {
        // Lấy danh sách chủ sử dụng theo Serial
        var dsChuSuDung = await GetInDatabaseAsync(serial, cancellationToken);

        // Trả về danh sách các mã chủ sử dụng
        return dsChuSuDung.Select(x => x.MaChuSuDung).ToList();

    }

    #endregion

    #region Lấy thông tin chủ sử dụng

    /// <summary>
    /// Lấy thông tin chủ sử dụng và quan hệ chủ sử dụng.
    /// </summary>
    /// <param name="serial"> Số Serial của Giấy chứng nhận.</param>
    /// <param name="cancellationToken">Token hủy bỏ.</param>
    /// <returns>Kết quả chứa thông tin chủ sử dụng hoặc lỗi.</returns>
    public async Task<Result<List<ChuSuDungInfo>>> GetResultAsync(
        string? serial = null, CancellationToken cancellationToken = default)
    {
        serial = serial.ChuanHoa();
        if (string.IsNullOrWhiteSpace(serial))
            return new Result<List<ChuSuDungInfo>>(new NoSerialException());
        try
        {
            var chuSuDungElis = await GetAsync(serial, cancellationToken);
            if (chuSuDungElis.Count == 0)
                throw new ChuSuDungNotFoundException(serial);
            var chuSuDungInfos = new List<ChuSuDungInfo>();
            foreach (var chuSuDungData in chuSuDungElis)
            {
                var chuSuDung = await GetChuSuDungAsync(chuSuDungData);
                if (chuSuDung is null) continue;
                var chuSuDungQuanHe = await GetChuSuDungQuanHeAsync(chuSuDungData);
                chuSuDungInfos.Add(new ChuSuDungInfo(
                    chuSuDungData.MaChuSuDung,
                    chuSuDung,
                    chuSuDungQuanHe));
            }
            return chuSuDungInfos;
        }
        catch (Exception e)
        {
            return new Result<List<ChuSuDungInfo>>(e);
        }
    }

    public async Task<List<ChuSuDungElis>> GetAsync(string? serial = null,
        CancellationToken cancellationToken = default)
    {
        serial = serial.ChuanHoa();
        if (string.IsNullOrWhiteSpace(serial))
            throw new NoSerialException();
        var cacheKey = CacheSettings.KeyChuSuDung(serial);
        return await hybridCache.GetOrCreateAsync(cacheKey,
            async cancel => await GetInDatabaseAsync(serial, cancel),
            tags: [serial],
            cancellationToken: cancellationToken);
    }

    public async Task<List<ChuSuDungElis>> GetInDatabaseAsync(
        string? serial = null, CancellationToken cancellationToken = default)
    {
        List<ChuSuDungElis> chuSuDungElis = [];
        serial = serial.ChuanHoa();
        if (string.IsNullOrWhiteSpace(serial)) return chuSuDungElis;
        try
        {
            var connection = await connectionElisData.GetConnectionAsync(serial);
            if (connection is null) return chuSuDungElis;
            await using var dbConnection = connection.ElisConnectionString.GetConnection();
            var query = dbConnection.SqlBuilder(
                $"""
                 SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
                 SELECT DISTINCT CSD.MaChuSuDung AS MaChuSuDung,
                                 CSD.MaDoiTuong AS MaDoiTuong,
                                 CSD.Ten1 AS Ten, 
                                 CSD.SoDinhDanh1 AS SoDinhDanh, 
                                 CSD.loaiSDD1 AS LoaiSdd,
                                 CSD.GioiTinh1 AS GioiTinh, 
                                 CSD.DiaChi1 AS DiaChi,
                                 CSD.MaQuocTich1 AS MaQuocTich,
                                 CSD.Ten2 AS Ten2,
                                 CSD.SoDinhDanh2 AS SoDinhDanh2,
                                 CSD.loaiSDD2 AS LoaiSdd2,
                                 CSD.GioiTinh2 AS GioiTinh2,
                                 CSD.QuanHe AS QuanHe,
                                 CSD.DiaChi2 AS DiaChi2,
                                 CSD.MaQuocTich2 AS MaQuocTich2
                                       
                       FROM ChuSuDung CSD
                            INNER JOIN GCNQSDD GCN ON CSD.MaChuSuDung = GCN.MaChuSuDung
                       WHERE (GCN.SoSerial IS NOT NULL AND LEN(GCN.SoSerial) > 0 AND UPPER(GCN.SoSerial) = {serial})
                 """);
            chuSuDungElis = (await query.QueryAsync<ChuSuDungElis>(cancellationToken: cancellationToken)).ToList();
            if (chuSuDungElis.Count == 0) return chuSuDungElis;
            var cacheKey = CacheSettings.KeyChuSuDung(serial);
            _ = hybridCache.SetAsync(cacheKey, chuSuDungElis, tags: [serial], cancellationToken: cancellationToken)
                .AsTask();
            return chuSuDungElis;
        }
        catch (Exception exception)
        {
            logger.Error(exception, "Lỗi khi truy vấn dữ liệu chủ sử dụng từ cơ sở dữ liệu, {Serial}", serial);
            throw new ChuSuDungSqlException(serial, exception);
        }
    }

    private async Task<ChuSuDung?> GetChuSuDungAsync(ChuSuDungElis chuSuDungElis)
    {
        if (string.IsNullOrWhiteSpace(chuSuDungElis.Ten)) return null;
        var ten = chuSuDungElis.MaDoiTuong switch
        {
            16 => chuSuDungElis.GioiTinh switch
            {
                1 => $"Ông {chuSuDungElis.Ten}",
                0 => $"Bà {chuSuDungElis.Ten}",
                _ => chuSuDungElis.Ten
            },
            _ => chuSuDungElis.Ten
        };
        var giayTo = chuSuDungElis.MaDoiTuong switch
        {
            16 => chuSuDungElis.LoaiSdd switch
            {
                1 => $"CMND: {chuSuDungElis.SoDinhDanh}",
                2 => $"CMQĐ: {chuSuDungElis.SoDinhDanh}",
                3 => $"Hộ chiếu: {chuSuDungElis.SoDinhDanh}",
                4 => $"Giấy khai sinh: {chuSuDungElis.SoDinhDanh}",
                5 => $"CCCD: {chuSuDungElis.SoDinhDanh}",
                6 => $"CMSQ: {chuSuDungElis.SoDinhDanh}",
                7 => $"CC: {chuSuDungElis.SoDinhDanh}",
                _ => chuSuDungElis.SoDinhDanh
            },
            _ => chuSuDungElis.SoDinhDanh
        };
        var quocTich = await GetQuocTichAsync(chuSuDungElis.MaQuocTich) ?? string.Empty;
        return new ChuSuDung(
            ten,
            giayTo,
            chuSuDungElis.DiaChi,
            quocTich
        );
    }

    private async Task<ChuSuDungQuanHe?> GetChuSuDungQuanHeAsync(ChuSuDungElis chuSuDungElis)
    {
        if (string.IsNullOrWhiteSpace(chuSuDungElis.Ten2)) return null;
        var quanHe = chuSuDungElis.QuanHe;
        quanHe = string.IsNullOrWhiteSpace(quanHe)
            ? $"{(chuSuDungElis.MaDoiTuong == 16 ? $"{(chuSuDungElis.GioiTinh2 == 1 ? "chồng" : "vợ")}" : "")}"
            : quanHe;
        var ten = quanHe is "chồng" or "vợ" ? $"Và {quanHe} {chuSuDungElis.Ten2}" : $"{quanHe} {chuSuDungElis.Ten2}";
        var giayTo = chuSuDungElis.MaDoiTuong switch
        {
            16 => chuSuDungElis.LoaiSdd2 switch
            {
                1 => $"CMND: {chuSuDungElis.SoDinhDanh2}",
                2 => $"CMQĐ: {chuSuDungElis.SoDinhDanh2}",
                3 => $"Hộ chiếu: {chuSuDungElis.SoDinhDanh2}",
                4 => $"Giấy khai sinh: {chuSuDungElis.SoDinhDanh2}",
                5 => $"CCCD: {chuSuDungElis.SoDinhDanh2}",
                6 => $"CMSQ: {chuSuDungElis.SoDinhDanh2}",
                7 => $"CC: {chuSuDungElis.SoDinhDanh2}",
                _ => chuSuDungElis.SoDinhDanh2
            },
            _ => chuSuDungElis.SoDinhDanh2
        };
        var quoctich = await GetQuocTichAsync(chuSuDungElis.MaQuocTich2) ?? string.Empty;
        return new ChuSuDungQuanHe(
            ten,
            giayTo,
            string.IsNullOrWhiteSpace(chuSuDungElis.DiaChi2) ? chuSuDungElis.DiaChi : chuSuDungElis.DiaChi2,
            quoctich
        );
    }

    private async ValueTask<string?> GetQuocTichAsync(int maQuocTich = 1,
        CancellationToken cancellationToken = default)
    {
        if (maQuocTich <= 1) return null;
        var cacheKey = CacheSettings.KeyQuocTich(maQuocTich);
        try
        {
            var quocTich = await hybridCache.GetOrCreateAsync(cacheKey,
                cancel => GetQuocTichInJsonAsync(null, maQuocTich, cancel),
                cancellationToken: cancellationToken);
            return quocTich;
        }
        catch (Exception exception)
        {
            logger.Error(exception, "Lỗi khi truy vấn dữ liệu Quốc tịch, {MaQuocTich}", maQuocTich);
            return null;
        }
    }

    private record QuocTich(int MaQuocTich, string TenQuocTich);

    private async ValueTask<string?> GetQuocTichInJsonAsync(string? path = null, int maQuocTich = 1,
        CancellationToken cancellationToken = default)
    {
        switch (maQuocTich)
        {
            case <= 1:
                return null;
            default:
                path ??= "QuocTich.json";
                try
                {
                    // Đọc nội dung file JSON
                    var jsonContent = await File.ReadAllTextAsync(path, cancellationToken);
                    // Deserialize JSON thành danh sách đối tượng
                    var countries = JsonConvert.DeserializeObject<List<QuocTich>>(jsonContent);
                    // Lấy tên quốc tịch theo mã quốc tịch
                    var country = countries?.FirstOrDefault(c => c.MaQuocTich == maQuocTich);
                    return country?.TenQuocTich;
                }
                catch (Exception exception)
                {
                    logger.Error(exception, "Lỗi khi truy vấn dữ liệu Quốc tịch, {MaQuocTich}", maQuocTich);
                    return null;
                }
        }
    }

    #endregion
}
public record ChuSuDungElis(
    long MaChuSuDung,
    int MaDoiTuong,
    string Ten,
    string SoDinhDanh,
    int LoaiSdd,
    int GioiTinh,
    string DiaChi,
    int MaQuocTich,
    string Ten2,
    string SoDinhDanh2,
    int LoaiSdd2,
    int GioiTinh2,
    string QuanHe,
    string DiaChi2,
    int MaQuocTich2
);
