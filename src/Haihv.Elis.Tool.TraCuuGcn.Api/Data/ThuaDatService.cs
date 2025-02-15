using Haihv.Elis.Tool.TraCuuGcn.Api.Settings;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using InterpolatedSql.Dapper;
using LanguageExt;
using LanguageExt.Common;
using ZiggyCreatures.Caching.Fusion;
using ILogger = Serilog.ILogger;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Data;

public class ThuaDatService(
    IConnectionElisData connectionElisData,
    IGiayChungNhanService giayChungNhanService,
    ILogger logger,
    IFusionCache fusionCache) : IThuaDatService
{
    private record ThuaDatToBanDo(int MaDvhc, string SoTo, int TyLe, string SoThua, string DiaChi, string GhiChu);

    /// <summary>
    /// Lấy thông tin Thửa đất theo Serial của Giấy chứng nhận.
    /// </summary>
    /// <param name="maGcn">Mã GCN của Giấy chứng nhận.</param>
    /// <param name="cancellationToken">Token hủy bỏ tác vụ không bắt buộc.</param>
    /// <returns>Kết quả chứa thông tin Thửa đất hoặc lỗi nếu không tìm thấy.</returns>
    public async Task<Result<ThuaDat>> GetResultAsync(long maGcn,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = CacheSettings.KeyThuaDat(maGcn);
        try
        {
            var thuaDat = await fusionCache.GetOrSetAsync(cacheKey,
                cancel => GetThuaDatInDatabaseAsync(maGcn, cancel),
                token: cancellationToken);
            return thuaDat ?? new Result<ThuaDat>(new ValueIsNullException("Không tìm thấy thông tin thửa đất!"));
        }
        catch (Exception exception)
        {
            return new Result<ThuaDat>(exception);
        }
    }

    /// <summary>
    /// Lấy thông tin Thửa đất theo Serial của Giấy chứng nhận từ cơ sở dữ liệu.
    /// </summary>
    /// <param name="maGcn">Mã GCN của Giấy chứng nhận.</param>
    /// <param name="cancellationToken">Token hủy bỏ tác vụ không bắt buộc.</param>
    /// <returns>Kết quả chứa thông tin Thửa đất hoặc lỗi nếu không tìm thấy.</returns>
    public async Task<ThuaDat?> GetThuaDatInDatabaseAsync(long maGcn = 0,
        CancellationToken cancellationToken = default)
    {
        if (maGcn <= 0) return null;
        var giayChungNhanResult =
            await giayChungNhanService.GetResultAsync(maGcn: maGcn, cancellationToken: cancellationToken);
        return await giayChungNhanResult.Match(
            giayChungNhan => GetThuaDatInDatabaseAsync(giayChungNhan, cancellationToken),
            ex => throw ex);
    }

    /// <summary>
    /// Lấy thông tin Thửa đất theo  Giấy chứng nhận từ cơ sở dữ liệu.
    /// </summary>
    /// <param name="giayChungNhan">Giấy chứng nhận.</param>
    /// <param name="cancellationToken">Token hủy bỏ tác vụ không bắt buộc.</param>
    /// <returns>Kết quả chứa thông tin Thửa đất hoặc lỗi nếu không tìm thấy.</returns>
    private async Task<ThuaDat?> GetThuaDatInDatabaseAsync(GiayChungNhan giayChungNhan,
        CancellationToken cancellationToken = default)
    {
        if (giayChungNhan.MaDangKy == 0 || giayChungNhan.MaGcn == 0 ) return null;
        var connectionName = await fusionCache.GetOrDefaultAsync<string>(
            CacheSettings.ElisConnectionName(giayChungNhan.MaGcn),
            token: cancellationToken);
        if (string.IsNullOrWhiteSpace(connectionName)) return null;
        var connectionString = connectionElisData.GetElisConnectionString(connectionName);
        try
        {
            var mucDichService = new MucDichAndHinhThucService(connectionString, logger);
            var nguonGocService = new NguonGocService(connectionString, logger);
            var (loaiDat, thoiHan, hinhThuc) =
                await mucDichService.GetMucDichSuDungAsync(giayChungNhan.MaGcn, cancellationToken);
            var nguonGoc = await nguonGocService.GetNguonGocSuDungAsync(giayChungNhan.MaGcn, cancellationToken);
            var thuaDatToBanDo =
                await GetThuaDatToBanDoAsync(giayChungNhan.MaDangKy, connectionString, cancellationToken);
            if (thuaDatToBanDo is null) return null;
            return new ThuaDat(
                thuaDatToBanDo.MaDvhc,
                thuaDatToBanDo.SoThua,
                thuaDatToBanDo.SoTo,
                thuaDatToBanDo.TyLe,
                thuaDatToBanDo.DiaChi,
                $"{giayChungNhan.DienTichRieng + giayChungNhan.DienTichChung} m²",
                loaiDat,
                thoiHan,
                hinhThuc,
                nguonGoc,
                thuaDatToBanDo.GhiChu
            );
        }
        catch (Exception e)
        {
            logger.Error(e, "Lỗi khi lấy thông tin Thửa đất theo Giấy chứng nhận: {Serial}", giayChungNhan.Serial);
            throw;
        }
    }

    private async Task<ThuaDatToBanDo?> GetThuaDatToBanDoAsync(long maDangKy, string connectionString,
        CancellationToken cancellationToken = default)
    {
        if (maDangKy <= 0) return null;
        try
        {
            await using var dbConnection = connectionString.GetConnection();
            var query = dbConnection.SqlBuilder(
                $"""
                 SELECT TBD.SoTo AS SoTo, 
                        TBD.TyLe AS TyLe,
                        TD.ThuaDatSo AS SoThua, 
                        TBD.MaDVHC AS maDvhc,
                        TD.DiaChi AS DiaChi,
                        TBD.GhiChu AS GhiChu
                 FROM ThuaDat TD 
                     INNER JOIN ToBanDo TBD ON TD.MaToBanDo = TBD.MaToBanDo 
                     INNER JOIN DangKyQSDD DK ON TD.MaThuaDat = DK.MaThuaDat
                 WHERE DK.MaDangKy = {maDangKy}
                 """);
            var thuaDatToBanDos = (await query.QueryAsync(cancellationToken: cancellationToken))
                .ToList();
            if (thuaDatToBanDos.Count == 0) return null;
            var thuaDatToBanDo = thuaDatToBanDos.First();
            if (!int.TryParse(thuaDatToBanDo.maDvhc.ToString(), out int maDvhc)) return null;
            string diaChi = thuaDatToBanDo.DiaChi;
            diaChi =
                $"{(string.IsNullOrWhiteSpace(diaChi) ? "" : $"{diaChi}, ")}{await GetDiaChiByMaDvhcAsync(maDvhc, connectionString, cancellationToken)}";
            string soThua = thuaDatToBanDo.SoThua.ToString();
            string toBanDo = thuaDatToBanDo.SoTo.ToString();
            int.TryParse(thuaDatToBanDo.TyLe.ToString(), out int tyLe);
            string ghiChu = thuaDatToBanDo.GhiChu?.ToString() ?? string.Empty;
            return new ThuaDatToBanDo(
                maDvhc,
                toBanDo.Trim(),
                tyLe,
                soThua.Trim(),
                diaChi.Trim().VietHoaDauChuoi(),
                ghiChu.Trim().VietHoaDauChuoi()
            );
        }
        catch (Exception e)
        {
            logger.Error(e, "Lỗi khi lấy thông tin Thửa đất theo Mã GCN: {maDangKy}", maDangKy);
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
            var diaChi = await fusionCache.GetOrSet(cacheKey,
                cancel => GetDiaChiByMaDvhcAsyncInDataAsync(maDvhc, connectionString, cancel),
                token: cancellationToken);
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

public static class ThuaDatExtension
{
    public static string ChuanHoaTenDonViHanhChinh(this string input, bool isLower = true)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;
        var words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return words.Length switch
        {
            0 => string.Empty,
            1 => isLower ? char.ToLower(input[0]) + input[1..] : input,
            _ =>
                $"{(isLower ? char.ToLower(words[0][0]) : char.ToUpper(words[0][0])) + words[0][1..]} {string.Join(' ', words.Skip(1))}"
        };
    }

    public static string VietHoaDauChuoi(this string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;
        return char.ToUpper(input[0]) + input[1..];
    }
}