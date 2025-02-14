using Haihv.Elis.Tool.TraCuuGcn.Api.Settings;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using Haihv.Elis.Tool.TraCuuGcn.Models.Extensions;
using LanguageExt;
using LanguageExt.Common;
using ZiggyCreatures.Caching.Fusion;
using ILogger = Serilog.ILogger;
namespace Haihv.Elis.Tool.TraCuuGcn.Api.Data;

public class SearchService(IGcnQrService gcnQrService, IGiayChungNhanService giayChungNhanService,
    ILogger logger, IFusionCache fusionCache) : ISearchService
{
    public async Task<Result<GiayChungNhanInfo>> GetResultAsync(string? query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return new Result<GiayChungNhanInfo>(new ArgumentException("Tham số truy vấn không hợp lệ!"));
        }
        var giayChungNhanInfo = await GetInDatabaseAsync(query);
        if (giayChungNhanInfo is null)
        {
            return new Result<GiayChungNhanInfo>(new ValueIsNullException("Không tìm thấy thông tin!"));
        }
        var maGcn = giayChungNhanInfo.MaGcnElis;
        if (maGcn <= 0) return giayChungNhanInfo;
        var cacheKey = CacheSettings.KeySearch(query);
        await fusionCache.SetAsync(cacheKey, maGcn);
        return giayChungNhanInfo;

    }

    private async Task<GiayChungNhanInfo?> GetInDatabaseAsync(string query, CancellationToken cancellationToken = default)
    {
        var cacheKey = CacheSettings.KeySearch(query);
        var maGcn = await fusionCache.GetOrDefaultAsync<long>(cacheKey, token: cancellationToken);
        GiayChungNhan? giayChungNhan = null;
        MaQrInfo? maQrInfo = null;
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
                    return null;
                }

                maGcn = maQrInfo.MaGcnElis;
                giayChungNhan = await giayChungNhanService.GetAsync(maGcn: maGcn, cancellationToken: cancellationToken);
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
            if (maGcn <= 0)
            {
                return null;
            }
        }
        
        _ = fusionCache.SetAsync(cacheKey, cacheKey, token: cancellationToken).AsTask();
        return giayChungNhan.ToGiayChungNhanInfo(maQrInfo);
    }
}