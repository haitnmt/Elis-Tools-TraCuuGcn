using Carter;
using MediatR;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Features.AppSetting;

public static class GetVersionAsync
{
    public record Query : IRequest<string>;
    public class Handler : IRequestHandler<Query, string>
    {
        public Task<string> Handle(Query request, CancellationToken cancellationToken)
        {
            return Task.FromResult(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version!.ToString());
        }
    }
    public class AppSettingEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/version", async (ISender sender) =>
                {
                    // Không cần try-catch ở đây vì đã có middleware xử lý exception toàn cục
                    var response = await sender.Send(new Query());
                    return Results.Ok(response);
                })
                .WithTags("Setting");
        }
    }
}