using ZiggyCreatures.Caching.Fusion;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Services;

public interface ICheckIpService
{
    /// <summary>
    /// Kiểm tra xem IP có bị khóa không.
    /// </summary>
    /// <param name="ip">Địa chỉ IP cần kiểm tra.</param>
    /// <returns>
    /// Thời gian còn lại của khóa theo giây.
    /// </returns>
    Task<(int Count, long ExprSecond)> CheckLockAsync(string ip);
    /// <summary>
    /// Đặt khóa cho IP.
    /// </summary>
    /// <param name="ip">
    /// Địa chỉ IP cần đặt khóa.
    /// </param>
    Task SetLockAsync(string ip);

    /// <summary>
    /// Xóa khóa của IP.
    /// </summary>
    /// <param name="ip">
    /// Địa chỉ IP cần xóa khóa.
    /// </param>
    Task ClearLockAsync(string ip);
}

public sealed class CheckIpService(IFusionCache fusionCache) : ICheckIpService
{
    private const string Key = "CheckIp";
    private const int SecondStep = 300;
    private const int MaxCount = 2;
    private static string LockKey(string ip) => $"{Key}:Lock:{ip}";

    /// <summary>
    /// Kiểm tra xem IP có bị khóa không.
    /// </summary>
    /// <param name="ip">Địa chỉ IP cần kiểm tra.</param>
    /// <returns>
    /// Thời gian còn lại của khóa theo giây.
    /// </returns>
    public async Task<(int Count, long ExprSecond)> CheckLockAsync(string ip)
    {
        var lockInfo = await fusionCache.GetOrDefaultAsync(LockKey(ip), new LockInfo());
        return lockInfo is null ? (0,0L) :
            // Tính thời gian lock còn lại theo giây (làm tròn kiểu long)
            (lockInfo.Count, (long) Math.Ceiling((lockInfo.ExprTime - DateTime.Now).TotalSeconds));
    }

    /// <summary>
    /// Đặt khóa cho IP.
    /// </summary>
    /// <param name="ip">
    /// Địa chỉ IP cần đặt khóa.
    /// </param>
    public async Task SetLockAsync(string ip)
    {
        var lockInfo = await fusionCache.GetOrDefaultAsync<LockInfo>(LockKey(ip));
        double expSecond = 0;
        const int totalSecond1Day = 86400;
        if (lockInfo is null)
        {
            lockInfo = new LockInfo
            {
                Count = 1
            };
        }
        else if (lockInfo.ExprTime >= DateTime.Now.AddDays(1))
        {
            lockInfo.Count = 10;
            expSecond = totalSecond1Day;
        }
        else
        {
            lockInfo.Count++;
            if (lockInfo.Count > MaxCount)
            {
                expSecond = Math.Pow(2, lockInfo.Count - MaxCount) * SecondStep;
                expSecond = expSecond > totalSecond1Day ? totalSecond1Day : expSecond;
            }
        }
        lockInfo.ExprTime = DateTime.Now.AddSeconds(expSecond);
        await fusionCache.SetAsync(LockKey(ip), lockInfo);
    }
    
    /// <summary>
    /// Xóa khóa của IP.
    /// </summary>
    /// <param name="ip">
    /// Địa chỉ IP cần xóa khóa.
    /// </param>
    public async Task ClearLockAsync(string ip)
    {
        await fusionCache.RemoveAsync(LockKey(ip));
    }
    
    private sealed class LockInfo(int count, DateTime exprTime)
    {
        public int Count { get; set; } = count;
        public DateTime ExprTime { get; set; } = exprTime;

        public LockInfo() : this(0, DateTime.MinValue)
        {}
    }
}