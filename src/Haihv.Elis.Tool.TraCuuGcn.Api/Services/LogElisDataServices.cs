using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using InterpolatedSql.Dapper;
using ILogger = Serilog.ILogger;
namespace Haihv.Elis.Tool.TraCuuGcn.Api.Services;

public interface ILogElisDataServices
{
    /// <summary>
    /// Ghi log vào bảng Log của cơ sở dữ liệu ElisData.
    /// </summary>
    /// <param name="serial">
    /// Số serial của GCN.
    /// Để tìm thông tin kết nối dữ liệu
    /// </param>
    /// <param name="performer">
    /// Tên người thực hiện thao tác.
    /// </param>
    /// <param name="task">
    /// Tên tác vụ cần ghi log.
    /// </param>
    /// <param name="message">
    /// Nội dung thông điệp cần ghi log.
    /// </param>
    /// <param name="loaiTacVu">
    /// Loại tác vụ cần ghi log.
    /// </param>
    /// <param name="cancellationToken">
    /// Token hủy bỏ tác vụ không bắt buộc.
    /// </param>
    Task WriteLogToElisDataAsync(string? serial, string? performer, string? task, string? message,
        LogElisDataServices.LoaiTacVu loaiTacVu = LogElisDataServices.LoaiTacVu.CapNhat,
        CancellationToken cancellationToken = default);
}

public sealed class LogElisDataServices(IConnectionElisData connectionElisData, ILogger logger) : ILogElisDataServices
{

    /// <summary>
    /// Ghi log vào bảng Log của cơ sở dữ liệu ElisData.
    /// </summary>
    /// <param name="serial">
    /// Số serial của GCN.
    /// </param>
    /// <param name="performer">
    /// Tên người thực hiện thao tác.
    /// </param>
    /// <param name="task">
    /// Tên tác vụ cần ghi log.
    /// </param>
    /// <param name="message">
    /// Nội dung thông điệp cần ghi log.
    /// </param>
    /// <param name="loaiTacVu">
    /// Loại tác vụ cần ghi log.
    /// </param>
    /// <param name="cancellationToken">
    /// Token hủy bỏ tác vụ không bắt buộc.
    /// </param>
    public async Task WriteLogToElisDataAsync(string? serial, string? performer, string? task, string? message,
        LoaiTacVu loaiTacVu = LoaiTacVu.CapNhat,
        CancellationToken cancellationToken = default)
    {
        // Kiểm tra serial trước khi thực hiện
        if (string.IsNullOrWhiteSpace(serial))
        {
            logger.Error("Không ghi được log Elis do: Serial không được để trống!");
            return;
        }

        // Lấy kết nối và kiểm tra
        var connectionSql = await connectionElisData.GetConnectionAsync(serial);
        if (connectionSql is null)
        {
            logger.Error("Không ghi được log Elis do: Không tìm thấy kết nối cơ sở dữ liệu cho serial {Serial}!", serial);
            return;
        }

        // Thực hiện ghi log
        await WriteLogToElisData(connectionSql, performer, task, message, loaiTacVu, cancellationToken);
    }

    /// <summary>
    /// Ghi log vào bảng Log của cơ sở dữ liệu ElisData.
    /// </summary>
    /// <param name="connectionSql">
    /// Đối tượng <see cref="ConnectionSql"/> chứa chuỗi kết nối đến cơ sở dữ liệu.
    /// </param>
    /// <param name="performer">
    /// Tên người thực hiện thao tác.
    /// </param>
    /// <param name="task">
    /// Tên tác vụ cần ghi log.
    /// </param>
    /// <param name="message">
    /// Nội dung thông điệp cần ghi log.
    /// </param>
    /// <param name="loaiTacVu">
    /// Loại tác vụ cần ghi log.
    /// </param>
    /// <param name="cancellationToken">
    /// Token hủy bỏ tác vụ không bắt buộc.
    /// </param>
    private async Task WriteLogToElisData(ConnectionSql connectionSql, string? performer, string? task, string? message,
        LoaiTacVu loaiTacVu = LoaiTacVu.CapNhat,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await using var dbConnection = connectionSql.ElisConnectionString.GetConnection();
            var guid = Guid.CreateVersion7();

            // Tối ưu xử lý chuỗi - chỉ tính toán một lần
            var taskValue = task ?? loaiTacVu.ToString();
            if (taskValue.Length > 200)
            {
                taskValue = taskValue[..200];
            }

            var intLoaiTacVu = (int)loaiTacVu;
            var safeMessage = message ?? string.Empty; // Đảm bảo message không null

            var query = dbConnection.SqlBuilder($"""
                                                 exec [dbo].[Append1TaskIntoLog]
                                                 @Performer={performer},
                                                 @Task={taskValue},
                                                 @LoaiTacVu={intLoaiTacVu},
                                                 @MaGUID={guid},
                                                 @MoTa={safeMessage}
                                                 """);

            await query.ExecuteAsync(cancellationToken: cancellationToken);
        }
        catch (Exception exception)
        {
            logger.Error(exception, "Lỗi khi ghi log vào ElisData: {ErrorMessage} - Người thực hiện: {Performer} - Nội dung: {Message}",
                exception.Message, performer ?? "N/A", message ?? "N/A");
        }
    }

    public enum LoaiTacVu
    {
        BienDong = 0,
        CapNhat = 1,
        Xoa = 2
    }
}