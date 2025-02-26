namespace Haihv.Elis.Tool.TraCuuGcn.Api.Endpoints;

public static class AppSettingsEndpoints
{
    /// <summary>
    /// Định nghĩa các endpoint cho AppSettings.
    /// </summary>
    /// <param name="app">Ứng dụng web.</param>
    public static void MapAppSettingsEndpoints(this WebApplication app)
    {
        app.MapGet("/api/version", GetVersionAsync)
            .WithName("GetVersionAsync");
        
    }
    private static Task<IResult> GetVersionAsync()
    {
        var assemblyVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version!.ToString();
        return Task.FromResult(Results.Ok(assemblyVersion));
    }
}