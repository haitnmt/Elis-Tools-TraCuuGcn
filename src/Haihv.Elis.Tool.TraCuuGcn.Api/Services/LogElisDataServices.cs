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
    Task WriteLogToElisDataAsync(string? serial, string? performer, string? task, string message,
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
    public async Task WriteLogToElisDataAsync(string? serial, string? performer, string? task, string message,
        LoaiTacVu loaiTacVu = LoaiTacVu.CapNhat,
        CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(serial))
        {
            var connectionSql = await connectionElisData.GetConnectionAsync(serial);
            if (connectionSql is not null)
                await WriteLogToElisData(connectionSql, performer, task, message, loaiTacVu, cancellationToken);
            else
                logger.Error("Không ghi được log Elis do: Không tìm thấy kết nối cơ sở dữ liệu cho serial {serial}!", serial);
        }
        else
        {
            logger.Error("Không ghi được log Elis do: Serial không được để trống!");
        }
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
            task = (task ?? loaiTacVu.ToString()).Length > 200 ? 
                (task ?? loaiTacVu.ToString())[..200] : task ?? loaiTacVu.ToString();
            var intLoaiTacVu = (int)loaiTacVu;

            var query = dbConnection.SqlBuilder($"""
                                                 exec [dbo].[Append1TaskIntoLog] 
                                                 @Performer={performer}, 
                                                 @Task={task}, 
                                                 @LoaiTacVu={intLoaiTacVu}, 
                                                 @MaGUID={guid}, 
                                                 @MoTa={message}
                                                 """);

            await query.ExecuteAsync(cancellationToken: cancellationToken);
        }
        catch (Exception exception)
        {
            logger.Error(exception, exception.Message + " - {Performer} - {Message}", performer, message);
        }

    }

    public enum LoaiTacVu
    {
        BienDong = 0,
        CapNhat = 1,
        Xoa = 2
    }
}