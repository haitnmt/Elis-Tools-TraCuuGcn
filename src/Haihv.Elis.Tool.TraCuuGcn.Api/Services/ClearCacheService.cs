using ILogger = Serilog.ILogger;
namespace Haihv.Elis.Tool.TraCuuGcn.Api.Services;

/// <summary>
/// Dịch vụ chạy nền định kỳ xóa cache dữ liệu hết hạn để đảm bảo tính nhất quán của dữ liệu.
/// </summary>
/// <remarks>
/// Dịch vụ này chạy như một Background Service, thực hiện xóa cache theo thời gian cấu hình.
/// </remarks>
/// <param name="connectionElisData">Đối tượng kết nối dữ liệu ELIS.</param>
/// <param name="logger">Đối tượng ghi log.</param>
public class ClearCacheService(IConnectionElisData connectionElisData, ILogger logger) : BackgroundService
{
    /// <summary>
    /// Số giây chờ giữa các lần xóa cache.
    /// </summary>
    private const int SecondsDelay = 30;
    
    /// <summary>
    /// Chênh lệch thời gian giữa máy chủ ứng dụng và máy chủ cơ sở dữ liệu (tính bằng giây).
    /// Được sử dụng để điều chỉnh thời gian khi xóa cache.
    /// </summary>
    private readonly int _secondsTimeDifference = connectionElisData.GetTimeDifferenceAsync().Result;
    
    /// <summary>
    /// Thực thi nhiệm vụ nền xóa cache định kỳ.
    /// </summary>
    /// <param name="stoppingToken">Token để thông báo dừng dịch vụ nền.</param>
    /// <returns>Task đại diện cho hoạt động nền.</returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Đợi 30 giây trước khi thực hiện xóa cache
            await Task.Delay(1000 * SecondsDelay, stoppingToken);
            
            try
            {
                // Xóa cache với thời gian tính cả độ chênh lệch giữa máy chủ ứng dụng và máy chủ cơ sở dữ liệu
                await connectionElisData.CacheRemoveAsync(
                    TimeSpan.FromSeconds(SecondsDelay + _secondsTimeDifference), 
                    stoppingToken);
            }
            catch (Exception e)
            {
                // Ghi log nếu xảy ra lỗi trong quá trình xóa cache
                logger.Error(e, "Lỗi khi xóa cache");
            }
        }
    }
}