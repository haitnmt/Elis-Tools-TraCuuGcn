using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Settings;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using InterpolatedSql.Dapper;
using LanguageExt;
using LanguageExt.Common;
using Newtonsoft.Json;
using ZiggyCreatures.Caching.Fusion;
using ILogger = Serilog.ILogger;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Services;

public sealed class ChuSuDungService(
    IConnectionElisData connectionElisData,
    ILogger logger,
    IFusionCache fusionCache) : IChuSuDungService
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
        if (string.IsNullOrWhiteSpace(soDinhDanh) || string.IsNullOrWhiteSpace(serial))
            return new Result<AuthChuSuDung>(new ValueIsNullException("Số định danh không hợp lệ!"));
        var cacheKey = CacheSettings.KeyAuthentication(serial, soDinhDanh);
        try
        {
            serial = serial.ChuanHoa();
            var tenChuSuDung = await fusionCache.GetOrSetAsync(cacheKey,
                
                cancel => GetTenChuSuDungInDataAsync(serial, soDinhDanh, cancel),
                tags:[serial],
                token: cancellationToken);
            return string.IsNullOrWhiteSpace(tenChuSuDung) ? 
                new Result<AuthChuSuDung>(new ValueIsNullException("Không tìm thấy chủ sử dụng!")) : 
                new AuthChuSuDung(serial, soDinhDanh, tenChuSuDung);
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
    private async Task<string?> GetTenChuSuDungInDataAsync(
        string? serial = null,
        string? soDinhDanh = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(soDinhDanh) || string.IsNullOrWhiteSpace(serial))
            return null;
        try
        {
            serial = serial.ChuanHoa();
            var connectionElis = await connectionElisData.GetConnection(serial);
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

    /// <summary>
    /// Lưu thông tin chủ sử dụng vào cache.
    /// </summary>
    /// <param name="serial"> Số Serial của Giấy chứng nhận.</param>
    /// <param name="cancellationToken">Token hủy bỏ.</param>
    /// <returns>Thông tin chủ sử dụng hoặc null nếu không tìm thấy.</returns>
    public async Task SetCacheAuthChuSuDungAsync(
        string serial,
        CancellationToken cancellationToken = default)
    {
        try
        {
            serial = serial.ChuanHoa();
            var connectionElis = await connectionElisData.GetConnection(serial);
            foreach (var connection in connectionElis)
            {
                await using var dbConnection = connection.ElisConnectionString.GetConnection();
                var query = dbConnection.SqlBuilder(
                    $"""
                              SELECT DISTINCT 
                                  COALESCE(CSD.SoDinhDanh1, CSD.SoDinhDanh2) AS SoDinhDanh,
                                  COALESCE(CSD.Ten1, CSD.Ten2) AS HoVaTen
                              FROM ChuSuDung CSD
                                  INNER JOIN GCNQSDD GCN ON CSD.MaChuSuDung = GCN.MaChuSuDung
                              WHERE UPPER(GCN.SoSerial) = {serial}
                                  AND NULLIF(COALESCE(CSD.SoDinhDanh1, CSD.SoDinhDanh2), '') IS NOT NULL
                                  AND NULLIF(COALESCE(CSD.Ten1, CSD.Ten2), '') IS NOT NULL
                              """);
                var chuSuDungDatas = (await query.QueryAsync<dynamic>(cancellationToken: cancellationToken)).ToList();
                if (chuSuDungDatas.Count == 0) continue;
                foreach (var chuSuDungData in chuSuDungDatas)
                {
                    string soDinhDanh = chuSuDungData.SoDinhDanh.ToString();
                    string hoVaTen = chuSuDungData.HoVaTen.ToString();
                    if (string.IsNullOrWhiteSpace(soDinhDanh) || string.IsNullOrWhiteSpace(hoVaTen)) continue;
                    var cacheKey = CacheSettings.KeyAuthentication(serial, soDinhDanh);
                    await fusionCache.SetAsync(cacheKey, hoVaTen, tags: [serial], token: cancellationToken);
                }
            }
        }
        catch (Exception exception)
        {
            logger.Error(exception, "Lỗi khi truy vấn dữ liệu chủ sử dụng từ cơ sở dữ liệu, {Serial}", serial);
        }
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
        if (string.IsNullOrWhiteSpace(serial))
            return new Result<List<ChuSuDungInfo>>(new ValueIsNullException("Không tìm thấy chủ sử dụng!"));
        serial = serial.ChuanHoa();
        var cacheKey = CacheSettings.KeyChuSuDung(serial);
        var chuSuDungs = await fusionCache.GetOrSetAsync(cacheKey,
            cancel => GetAsync(serial, cancel),
            tags: [serial],
            token: cancellationToken);
        return chuSuDungs.Count > 0 ? 
            chuSuDungs : 
            new Result<List<ChuSuDungInfo>>(new ValueIsNullException("Không tìm thấy chủ sử dụng!"));
    }
    
    public async Task<List<ChuSuDungInfo>> GetAsync(
        string? serial = null, CancellationToken cancellationToken = default)
    {
        List<ChuSuDungInfo> chuSuDungInfos = [];
        if (string.IsNullOrWhiteSpace(serial)) return chuSuDungInfos;
        try
        {
            serial = serial.ChuanHoa();
            var connectionElis = await connectionElisData.GetConnection(serial);
            foreach (var connection in connectionElis)
            {
                await using var dbConnection = connection.ElisConnectionString.GetConnection();
                var query = dbConnection.SqlBuilder(
                    $"""
                     SELECT DISTINCT CSD.MaDoiTuong AS MaDoiTuong,
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
                var chuSuDungDatas = (await query.QueryAsync<ChuSuDungData>(cancellationToken: cancellationToken)).ToList();
                if (chuSuDungDatas.Count == 0) continue;
                foreach (var chuSuDungData in chuSuDungDatas)
                {
                    var chuSuDung = await GetChuSuDungAsync(chuSuDungData);
                    if (chuSuDung is null) continue;
                    var chuSuDungQuanHe = await GetChuSuDungQuanHeAsync(chuSuDungData);
                    chuSuDungInfos.Add(new ChuSuDungInfo(chuSuDung, chuSuDungQuanHe));
                }
            }
        }
        catch (Exception exception)
        {
            logger.Error(exception, "Lỗi khi truy vấn dữ liệu chủ sử dụng từ cơ sở dữ liệu, {Serial}", serial);
            throw;
        }
        return chuSuDungInfos;
    }

    private record ChuSuDungData(
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
    
    private async Task<ChuSuDung?> GetChuSuDungAsync(ChuSuDungData chuSuDungData)
    {
        if (string.IsNullOrWhiteSpace(chuSuDungData.Ten)) return null;
        var ten = chuSuDungData.MaDoiTuong switch
        {
            16 => chuSuDungData.GioiTinh switch
            {
                1 => $"Ông {chuSuDungData.Ten}",
                0 => $"Bà {chuSuDungData.Ten}",
                _ => chuSuDungData.Ten
            },
            _ => chuSuDungData.Ten
        };
        var giayTo = chuSuDungData.MaDoiTuong switch
        {
            16 => chuSuDungData.LoaiSdd switch
            {
                1 => $"CMND: {chuSuDungData.SoDinhDanh}",
                2 => $"CMQĐ: {chuSuDungData.SoDinhDanh}",
                3 => $"Hộ chiếu: {chuSuDungData.SoDinhDanh}",
                4 => $"Giấy khai sinh: {chuSuDungData.SoDinhDanh}",
                5 => $"CCCD: {chuSuDungData.SoDinhDanh}",
                6 => $"CMSQ: {chuSuDungData.SoDinhDanh}",
                7 => $"CC: {chuSuDungData.SoDinhDanh}",
                _ => chuSuDungData.SoDinhDanh
            },
            _ => chuSuDungData.SoDinhDanh
        };
        var quocTich = await GetQuocTichAsync(chuSuDungData.MaQuocTich) ?? string.Empty;
        return new ChuSuDung(
            ten,
            giayTo,
            chuSuDungData.DiaChi,
            quocTich
        );
    }

    private async Task<ChuSuDungQuanHe?> GetChuSuDungQuanHeAsync(ChuSuDungData chuSuDungData)
    {
        if (string.IsNullOrWhiteSpace(chuSuDungData.Ten2)) return null;
        var quanHe = chuSuDungData.QuanHe;
        quanHe = string.IsNullOrWhiteSpace(quanHe)
            ? $"{(chuSuDungData.MaDoiTuong == 16 ? $"{(chuSuDungData.GioiTinh2 == 1 ? "chồng" : "vợ")}" : "")}"
            : quanHe;
        var ten = chuSuDungData.MaDoiTuong switch
        {
            16 => chuSuDungData.GioiTinh2 switch
            {
                1 => $"Ông {chuSuDungData.Ten2}",
                0 => $"Bà {chuSuDungData.Ten2}",
                _ => chuSuDungData.Ten2
            },
            _ => chuSuDungData.Ten2
        };
        ten = quanHe is "chồng" or "vợ" ? $"Và {quanHe}: {ten}" : $"{quanHe}: {ten}";
        var giayTo = chuSuDungData.MaDoiTuong switch
        {
            16 => chuSuDungData.LoaiSdd2 switch
            {
                1 => $"CMND: {chuSuDungData.SoDinhDanh2}",
                2 => $"CMQĐ: {chuSuDungData.SoDinhDanh2}",
                3 => $"Hộ chiếu: {chuSuDungData.SoDinhDanh2}",
                4 => $"Giấy khai sinh: {chuSuDungData.SoDinhDanh2}",
                5 => $"CCCD: {chuSuDungData.SoDinhDanh2}",
                6 => $"CMSQ: {chuSuDungData.SoDinhDanh2}",
                7 => $"CC: {chuSuDungData.SoDinhDanh2}",
                _ => chuSuDungData.SoDinhDanh2
            },
            _ => chuSuDungData.SoDinhDanh2
        };
        var quoctich = await GetQuocTichAsync(chuSuDungData.MaQuocTich2) ?? string.Empty;
        return new ChuSuDungQuanHe(
            ten,
            giayTo,
            string.IsNullOrWhiteSpace(chuSuDungData.DiaChi2) ? 
                chuSuDungData.DiaChi : chuSuDungData.DiaChi2,
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
            var quocTich = await fusionCache.GetOrSet(cacheKey,
                cancel => GetQuocTichInJsonAsync(null, maQuocTich, cancel),
                token: cancellationToken);
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
