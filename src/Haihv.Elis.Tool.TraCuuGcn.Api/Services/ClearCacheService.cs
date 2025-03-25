using ILogger = Serilog.ILogger;
namespace Haihv.Elis.Tool.TraCuuGcn.Api.Services;

public class ClearCacheService(IConnectionElisData connectionElisData, ILogger logger) : BackgroundService
{
    private const int SecondsDelay = 30;
    private readonly int _secondsTimeDifference = connectionElisData.GetTimeDifferenceAsync().Result;
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Đợi 30 giây
            await Task.Delay(1000 * SecondsDelay, stoppingToken);
            try
            {
                await connectionElisData.CacheRemoveAsync(TimeSpan.FromSeconds(SecondsDelay + _secondsTimeDifference), stoppingToken);
            }
            catch (Exception e)
            {
                logger.Error(e, "Lỗi khi xóa cache");
            }
        }
    }
}