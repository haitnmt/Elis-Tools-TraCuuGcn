using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Settings;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using Haihv.Elis.Tool.TraCuuGcn.Models.Extensions;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.Extensions.Caching.Hybrid;
using ILogger = Serilog.ILogger;
namespace Haihv.Elis.Tool.TraCuuGcn.Api.Services;

public class SearchService(IGcnQrService gcnQrService,
    IGiayChungNhanService giayChungNhanService,
    ILogger logger, HybridCache hybridCache) : ISearchService
{
    public async Task<Result<GiayChungNhanInfo>> GetResultAsync(string? query, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return new Result<GiayChungNhanInfo>(new ArgumentException("Tham số truy vấn không hợp lệ!"));
        }
        var cacheKey = CacheSettings.KeySearch(query);
        var giayChungNhanInfo = await hybridCache.GetOrCreateAsync(cacheKey, 
            _ => ValueTask.FromResult<GiayChungNhanInfo?>(null),
            cancellationToken: cancellationToken);
        if (giayChungNhanInfo is not null)
        {
            return giayChungNhanInfo;
        }
        giayChungNhanInfo = await GetInDatabaseAsync(query, cancellationToken);
        var serial = giayChungNhanInfo?.Serial?.ChuanHoa();
        if (giayChungNhanInfo is null || string.IsNullOrWhiteSpace(serial))
        {
            return new Result<GiayChungNhanInfo>(new ValueIsNullException("Không tìm thấy thông tin!"));
        }
        await hybridCache.SetAsync(cacheKey, 
            giayChungNhanInfo, 
            tags: [serial], 
            cancellationToken: cancellationToken);
        return giayChungNhanInfo;
    }

    private async Task<GiayChungNhanInfo?> GetInDatabaseAsync(string query, CancellationToken cancellationToken = default)
    {
        MaQrInfo? maQrInfo;
        GiayChungNhan? giayChungNhan;
        if (query.Length > 15)
        {
            // Tim kiếm theo mã QR
            maQrInfo = await gcnQrService.GetAsync(query, query, cancellationToken: cancellationToken);
            if (maQrInfo is null)
            {
                logger.Warning("Không tìm thấy thông tin Mã QR: {Query}", query);
                return null;
            }
            
            giayChungNhan = await giayChungNhanService.GetAsync(serial: maQrInfo.SerialNumber, 
                cancellationToken: cancellationToken);
            maQrInfo.Verified = giayChungNhan?.Serial == maQrInfo.SerialNumber?.Trim();
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
            maQrInfo = await gcnQrService.GetAsync(serial: giayChungNhan.Serial, cancellationToken: cancellationToken);
        }
        
        if (maQrInfo is null && giayChungNhan is null)
        {
            logger.Warning("Không tìm thấy thông tin Giấy chứng nhận: {Query}", query);
            return null;
        }
        
        if (maQrInfo is not null)
        {
            return giayChungNhan.ToGiayChungNhanInfo(maQrInfo);
        }
        
        var tenDonVi = await gcnQrService.GetTenDonViInDataBaseAsync(giayChungNhan?.MaDonViInGCN, cancellationToken: cancellationToken);
        
        maQrInfo = new MaQrInfo
        {
            HieuLuc = true,
            MaHoSoTthc = giayChungNhan?.MaHoSoDVC,
            MaDonVi = giayChungNhan?.MaDonViInGCN,
            TenDonVi = tenDonVi
        };
        return giayChungNhan.ToGiayChungNhanInfo(maQrInfo);
    }
}