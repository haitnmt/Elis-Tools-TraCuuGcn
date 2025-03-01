using Haihv.Elis.Tool.TraCuuGcn.Api.Settings;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using Haihv.Elis.Tool.TraCuuGcn.Models.Extensions;
using LanguageExt;
using LanguageExt.Common;
using ZiggyCreatures.Caching.Fusion;
using ILogger = Serilog.ILogger;
namespace Haihv.Elis.Tool.TraCuuGcn.Api.Services;

public class SearchService(IGcnQrService gcnQrService, 
    IGiayChungNhanService giayChungNhanService,
    ILogger logger, IFusionCache fusionCache) : ISearchService
{
    public async Task<Result<GiayChungNhanInfo>> GetResultAsync(string? query, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return new Result<GiayChungNhanInfo>(new ArgumentException("Tham số truy vấn không hợp lệ!"));
        }
        var giayChungNhanInfo = await GetInDatabaseAsync(query, cancellationToken);
        if (giayChungNhanInfo is null)
        {
            return new Result<GiayChungNhanInfo>(new ValueIsNullException("Không tìm thấy thông tin!"));
        }
        var maGcn = giayChungNhanInfo.MaGcnElis;
        if (maGcn <= 0) return giayChungNhanInfo;
        var cacheKey = CacheSettings.KeySearch(query);
        await fusionCache.SetAsync(cacheKey, maGcn, tags: [maGcn.ToString()], token: cancellationToken);
        return giayChungNhanInfo;

    }

    private async Task<GiayChungNhanInfo?> GetInDatabaseAsync(string query, CancellationToken cancellationToken = default)
    {
        var cacheKey = CacheSettings.KeySearch(query);
        var maGcn = await fusionCache.GetOrDefaultAsync<long>(cacheKey, token: cancellationToken);
        GiayChungNhan? giayChungNhan = null;
        MaQrInfo? maQrInfo = null;
        var hieuLuc = true;
        if (maGcn > 0)
        {
            giayChungNhan = await giayChungNhanService.GetAsync(maGcn: maGcn, cancellationToken: cancellationToken);
            if (giayChungNhan is not null)
            {
                maQrInfo = await gcnQrService.GetAsync(maGcnInDataBase: maGcn, cancellationToken: cancellationToken);
            }
        }
        if (giayChungNhan is null && maQrInfo is null)
        {
            if (query.Length > 15)
            {
                // Tim kiếm theo mã QR
                maQrInfo = await gcnQrService.GetAsync(query, query, cancellationToken: cancellationToken);
                if (maQrInfo is null)
                {
                    
                    logger.Warning("Không tìm thấy thông tin Mã QR: {Query}", query);
                }
                giayChungNhan = await giayChungNhanService.GetAsync(
                    maGcn: maQrInfo?.MaGcnElis ?? 0L, 
                    serial: maQrInfo?.SerialNumber, 
                    cancellationToken: cancellationToken);
            }
            else
            {
                if (long.TryParse(query, out var maVach))
                {
                    // Tìm kiếm theo mã vạch
                    giayChungNhan = await giayChungNhanService.GetAsync(maVach: maVach, cancellationToken: cancellationToken);
                }
                else
                {
                    // Tìm kiếm theo số serial
                    giayChungNhan = await giayChungNhanService.GetAsync(query, cancellationToken: cancellationToken);
                }
                if (giayChungNhan is null)
                {
                    logger.Warning("Không tìm thấy thông tin Giấy chứng nhận: {Query}", query);
                    return null;
                }
                maGcn = giayChungNhan.MaGcn;
                maQrInfo = await gcnQrService.GetAsync(maGcnInDataBase: maGcn, cancellationToken: cancellationToken);
            }
        }
        if (maQrInfo is null && giayChungNhan is null)
        {
            logger.Warning("Không tìm thấy thông tin Giấy chứng nhận: {Query}", query);
            return null;
        }
        
        if (giayChungNhan is not null && giayChungNhan.MaGcn > 0)
            _ = fusionCache.SetAsync(cacheKey, giayChungNhan.MaGcn, tags: [maGcn.ToString()], token: cancellationToken).AsTask();
        
        if (maQrInfo is not null) return giayChungNhan.ToGiayChungNhanInfo(maQrInfo);
        
        var tenDonVi = await gcnQrService.GetTenDonViInDataBaseAsync(giayChungNhan?.MaDonViInGCN, cancellationToken: cancellationToken);
        maQrInfo = new MaQrInfo
        {
            MaHoSoTthc = giayChungNhan?.MaHoSoDVC,
            MaDonVi = giayChungNhan?.MaDonViInGCN,
            TenDonVi = tenDonVi
        };
        return giayChungNhan.ToGiayChungNhanInfo(maQrInfo);
    }
}