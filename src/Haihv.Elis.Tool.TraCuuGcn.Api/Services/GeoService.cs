using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Settings;
using LanguageExt.Common;
using Microsoft.Extensions.Caching.Hybrid;
using System.Collections.Concurrent;
using Haihv.Elis.Tool.TraCuuGcn.Api.Exceptions;
using ILogger = Serilog.ILogger;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Services;

/// <summary>
/// Định nghĩa các phương thức để tương tác với dữ liệu địa lý và tọa độ thửa đất.
/// </summary>
public interface IGeoService
{
    /// <summary>
    /// Lấy thông tin toạ độ thửa đất từ cơ sở dữ liệu.
    /// </summary>
    /// <param name="serial">Số phát hành (Serial) của Giấy chứng nhận.</param>
    /// <param name="cancellationToken">Token hủy bỏ tác vụ.</param>
    /// <returns>
    /// Kết quả chứa thông tin toạ độ thửa đất hoặc lỗi nếu không tìm thấy.
    /// </returns>
    Task<Result<List<Coordinates>>> GetResultAsync(string serial, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Lấy thông tin toạ độ thửa đất từ cơ sở dữ liệu hoặc cache.
    /// </summary>
    /// <param name="serial">Số phát hành (Serial) của Giấy chứng nhận.</param>
    /// <param name="cancellationToken">Token hủy bỏ tác vụ.</param>
    /// <returns>
    /// Danh sách tọa độ thửa đất.
    /// </returns>
    /// <exception cref="Exception">Có thể ném ra nhiều loại ngoại lệ khác nhau khi xử lý.</exception>
    Task<List<Coordinates>> GetAsync(string serial, CancellationToken cancellationToken = default);
}

/// <summary>
/// Dịch vụ quản lý và truy xuất thông tin địa lý và tọa độ thửa đất.
/// </summary>
public class GeoService : IGeoService
{
    private readonly IConnectionElisData _connectionElisData;
    private readonly IThuaDatService _thuaDatService;
    private readonly ILogger _logger;
    private readonly HybridCache _hybridCache;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _apiSdeUrl;

    /// <summary>
    /// Khởi tạo một instance mới của <see cref="GeoService"/>.
    /// </summary>
    /// <param name="connectionElisData">Dịch vụ kết nối dữ liệu ELIS.</param>
    /// <param name="thuaDatService">Dịch vụ quản lý thông tin thửa đất.</param>
    /// <param name="logger">Đối tượng ghi log.</param>
    /// <param name="hybridCache">Bộ nhớ đệm tổng hợp.</param>
    /// <param name="httpClientFactory">Factory tạo HttpClient.</param>
    /// <exception cref="SdeNotConfigException">Nếu không tìm thấy URL của API SDE trong cấu hình.</exception>
    public GeoService(
        IConnectionElisData connectionElisData,
        IThuaDatService thuaDatService,
        ILogger logger,
        HybridCache hybridCache,
        IHttpClientFactory httpClientFactory)
    {
        _connectionElisData = connectionElisData;
        _thuaDatService = thuaDatService;
        _logger = logger;
        _hybridCache = hybridCache;
        _httpClientFactory = httpClientFactory;
        
        // Lấy URL API SDE từ cấu hình
        _apiSdeUrl = connectionElisData.ApiSdeUrl();

        // Kiểm tra URL API SDE đã được cấu hình hay chưa
        if (!string.IsNullOrWhiteSpace(_apiSdeUrl)) return;
        
        var ex = new SdeNotConfigException();
        _logger.Error(ex, "Không tìm thấy đường dẫn API SDE trong cấu hình");
        throw ex;
    }

    /// <summary>
    /// Lấy thông tin toạ độ thửa đất và xử lý kết quả hoặc lỗi.
    /// </summary>
    /// <param name="serial">Số phát hành (Serial) của Giấy chứng nhận.</param>
    /// <param name="cancellationToken">Token hủy bỏ tác vụ.</param>
    /// <returns>
    /// Kết quả chứa danh sách tọa độ hoặc lỗi nếu không tìm thấy.
    /// </returns>
    public async Task<Result<List<Coordinates>>> GetResultAsync(string serial, CancellationToken cancellationToken = default)
    {
        try
        {
            // Lấy danh sách tọa độ
            var coordinates = await GetAsync(serial, cancellationToken);
            
            // Kiểm tra có dữ liệu tọa độ hay không
            if (coordinates.Count > 0) return coordinates;
            
            // Ghi log và trả về lỗi không tìm thấy tọa độ
            _logger.Error("Không tìm thấy toạ độ thửa trong cơ sở dữ liệu: {Serial}", serial);
            return new Result<List<Coordinates>>(new ToaDoNotFoundException(serial));
        }
        catch (Exception e)
        {
            // Ghi log và trả về lỗi khi xử lý
            _logger.Error(e, "Lỗi khi lấy thông tin toạ độ thửa: {Serial}", serial);
            return new Result<List<Coordinates>>(e);
        }
    }

    /// <summary>
    /// Lấy danh sách tọa độ thửa đất từ cache hoặc từ API SDE.
    /// </summary>
    /// <param name="serial">Số phát hành (Serial) của Giấy chứng nhận.</param>
    /// <param name="cancellationToken">Token hủy bỏ tác vụ.</param>
    /// <returns>Danh sách tọa độ thửa đất.</returns>
    /// <exception cref="Exception">Ném ra ngoại lệ khi có lỗi trong quá trình xử lý.</exception>
    public async Task<List<Coordinates>> GetAsync(string serial, CancellationToken cancellationToken = default)
    {
        // Chuẩn hóa số phát hành
        serial = serial.ChuanHoa() ?? "";
        if (string.IsNullOrWhiteSpace(serial)) return [];
        
        try
        {
            // Thiết lập thời gian hết hạn cho cache
            var options = new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromDays(7),         // Cache toàn cục hết hạn sau 7 ngày
                LocalCacheExpiration = TimeSpan.FromHours(1) // Cache cục bộ hết hạn sau 1 giờ
            };

            // Lấy dữ liệu từ cache hoặc từ nguồn (API SDE)
            return await _hybridCache.GetOrCreateAsync(
                CacheSettings.KeyToaDoThua(serial),         // Khóa cache
                async cancel => await GetPointFromApiSdeAsync(serial, cancel), // Hàm lấy dữ liệu từ nguồn
                options: options,                           // Tùy chọn cache
                tags: [serial],                            // Tag để quản lý cache
                cancellationToken: cancellationToken);
        }
        catch (Exception e)
        {
            // Ghi log và ném lại ngoại lệ
            _logger.Error(e, "Lỗi khi lấy thông tin toạ độ thửa: {Serial}", serial);
            throw;
        }
    }

    /// <summary>
    /// Định nghĩa cấu trúc phản hồi từ API SDE.
    /// </summary>
    /// <param name="Status">Trạng thái phản hồi.</param>
    /// <param name="Message">Thông điệp phản hồi.</param>
    /// <param name="Data">Dữ liệu tọa độ.</param>
    private record BodyResponse(string Status, string Message, Coordinates Data);

    /// <summary>
    /// Lấy dữ liệu tọa độ từ API SDE dựa trên thông tin thửa đất.
    /// </summary>
    /// <param name="serial">Số phát hành (Serial) của Giấy chứng nhận.</param>
    /// <param name="cancellationToken">Token hủy bỏ tác vụ.</param>
    /// <returns>Danh sách tọa độ thửa đất.</returns>
    /// <exception cref="Exception">Ném ra ngoại lệ khi không thể lấy dữ liệu tọa độ.</exception>
    private async Task<List<Coordinates>> GetPointFromApiSdeAsync(string serial, CancellationToken cancellationToken = default)
    {
        // Lấy thông tin kết nối cơ sở dữ liệu
        var connectionSqls = await _connectionElisData.GetAllConnectionAsync(serial);
        if (connectionSqls.Count == 0) return [];

        // Lấy thông tin thửa đất
        var thuaDats = await _thuaDatService.GetAsync(serial, cancellationToken);
        if (thuaDats.Count == 0) return [];

        // Sử dụng ConcurrentBag để lưu kết quả và lỗi trong xử lý đa luồng
        var result = new ConcurrentBag<Coordinates>();
        var errors = new ConcurrentBag<Exception>();

        // Tạo HttpClient từ factory với cấu hình có sẵn
        var httpClient = _httpClientFactory.CreateClient("SdeApi");
        httpClient.BaseAddress = new System.Uri(_apiSdeUrl);

        // Xử lý song song cho mỗi thửa đất
        var thuaDatTasks = thuaDats.Select(async thuaDat =>
        {
            // Giải nén các thuộc tính của thửa đất
            var (_, _, maDvhc, thuaDatSo, toBanDo, tyLe, _, _, _, _, _, _, _) = thuaDat;
            var soTo = toBanDo.Trim().ToLower();
            var soThua = thuaDatSo.Trim().ToLower();

            // Xử lý song song cho mỗi kết nối cơ sở dữ liệu
            var connectionTasks = connectionSqls.Select(async connection =>
            {
                var (name, _, _, database, _) = connection;
                try
                {
                    // Tạo request body cho API SDE
                    var req = new { database, SoTo = soTo, SoThua = soThua, TyLe = tyLe, KvhcId = maDvhc };
                    
                    // Gửi request đến API SDE
                    var response = await httpClient.PostAsJsonAsync("/api/sde/toadothuadat", req, cancellationToken);

                    // Kiểm tra trạng thái phản hồi
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.Warning("API SDE trả về mã lỗi {StatusCode} cho {Serial} {Database}",
                            (int)response.StatusCode, serial, database);
                        throw new SdeGetException(response.ReasonPhrase ?? 
                                                  "Lỗi trong quá trình lấy dữ liệu từ API SDE", 
                            response.StatusCode);
                    }

                    // Đọc và kiểm tra dữ liệu phản hồi
                    var json = await response.Content.ReadFromJsonAsync<BodyResponse>(cancellationToken: cancellationToken);
                    if (json is null || json.Status != "success")
                    {
                        _logger.Warning("API SDE trả về trạng thái không thành công cho {Serial} {Database}: {Status}",
                            serial, database, json?.Status);
                        if (json is null)
                            throw new ToaDoNotFoundException(serial);
                        if (json.Status != "success")
                            throw new SdeGetException(json.Status, response.StatusCode);
                    }
                    
                    // Thêm tọa độ vào kết quả
                    result.Add(json.Data);
                }
                catch (Exception e)
                {
                    // Ghi log và lưu lỗi
                    _logger.Error(e, "Lỗi khi lấy vị trí thửa đất trong SDE {Serial} {Sde}",
                        serial, name);
                    errors.Add(e);
                }
            });

            // Chờ tất cả các kết nối được xử lý
            await Task.WhenAll(connectionTasks);
        });

        // Chờ tất cả các thửa đất được xử lý
        await Task.WhenAll(thuaDatTasks);

        // Nếu không có kết quả và có lỗi, ném ra lỗi đầu tiên
        if (result.IsEmpty && !errors.IsEmpty)
        {
            throw errors.First();
        }

        // Trả về danh sách tọa độ
        return result.ToList();
    }
}

/// <summary>
/// Lưu trữ thông tin tọa độ của một điểm trong không gian hai chiều.
/// </summary>
/// <param name="X">Tọa độ X (kinh độ).</param>
/// <param name="Y">Tọa độ Y (vĩ độ).</param>
public record Coordinates(double X, double Y);
