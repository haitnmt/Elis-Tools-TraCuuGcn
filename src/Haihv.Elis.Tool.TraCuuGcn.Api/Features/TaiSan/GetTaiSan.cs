using Carter;
using Haihv.Elis.Tool.TraCuuGcn.Api.Exceptions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Services;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using ILogger = Serilog.ILogger;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Features.TaiSan;

public static class GetTaiSan
{
    public record Query(string Serial, string? SoDinhDanh) : IRequest<IEnumerable<TraCuuGcn.Models.TaiSan>>;

    public class Handler(
        ILogger logger,
        IPermissionService permissionService,
        IHttpContextAccessor httpContextAccessor,
        IThuaDatService thuaDatService,
        IChuSuDungService chuSuDungService,
        ITaiSanService taiSanService) : IRequestHandler<Query, IEnumerable<TraCuuGcn.Models.TaiSan>>
    {
        public async Task<IEnumerable<TraCuuGcn.Models.TaiSan>> Handle(Query request, CancellationToken cancellationToken)
        {
            var serial = request.Serial.ChuanHoa();
            if (string.IsNullOrWhiteSpace(serial))
                throw new NoSerialException();
            var httpContext = httpContextAccessor.HttpContext
                              ?? throw new InvalidOperationException("HttpContext không khả dụng");
            var user = httpContext.User;
            if (!await permissionService.HasReadPermission(user, serial, request.SoDinhDanh, cancellationToken))
                throw new UnauthorizedAccessException();
            var email = user.GetEmail();
            var url = httpContext.Request.GetDisplayUrl();
            // Lấy danh sách mã thửa đất
            var dsMaThuaDatTask = thuaDatService.GetMaThuaDatAsync(serial, cancellationToken);
            // Lấy danh sách mã Chủ sử dugnj
            var dsMaChuSuDungTask = chuSuDungService.GetMaChuSuDungAsync(serial, cancellationToken);
            var dsMaThuaDat = await dsMaThuaDatTask;
            if (dsMaThuaDat.Count == 0)
            {
                logger.Warning("{Email} Không tìm thấy thông tin thửa đất: {Url} {Serial}",
                    email,
                    url,
                    serial);
                throw new ThuaDatNotFoundException(serial);
            }
            var dsMaChuSuDung = await dsMaChuSuDungTask;
            if (dsMaChuSuDung.Count == 0)
            {
                logger.Warning("{Email} Không tìm thấy thông tin chủ sử dụng: {Url} {Serial}",
                    email,
                    url,
                    serial);
                throw new ChuSuDungNotFoundException(serial);
            }
            var result = await taiSanService.GetTaiSanAsync(serial, dsMaThuaDat, dsMaChuSuDung);

            return await Task.FromResult(result.Match(
                taiSan =>
                {
                    logger.Information("{Email} lấy thông tin tài sản thành công: {Url} {Serial}",
                        email,
                        url,
                        serial);
                    if (taiSan.Count != 0) return taiSan;
                    logger.Warning("{Email} Không tìm thấy thông tin tài sản: {Url} {Serial}",
                        email,
                        url,
                        serial);
                    throw new TaiSanNotFoundException(serial);

                },
                ex =>
                {
                    logger.Error(ex, "{Email} Lỗi khi lấy thông tin tài sản:  {Url} {Serial}",
                        email,
                        url,
                        serial);
                    throw ex;
                }));
        }
    }
    public class TaiSanEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/tai-san/", async (ISender sender, string serial, string? soDinhDanh = null) =>
                {
                    // Không cần try-catch ở đây vì đã có middleware xử lý exception toàn cục
                    var response = await sender.Send(new Query(serial, soDinhDanh));
                    return Results.Ok(response);
                })
                .RequireAuthorization()
                .WithTags("TaiSan");
        }
    }
}