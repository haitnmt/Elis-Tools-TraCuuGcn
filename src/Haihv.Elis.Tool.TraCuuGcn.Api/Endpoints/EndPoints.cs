namespace Haihv.Elis.Tool.TraCuuGcn.Api.Endpoints;

public static class EndPoints
{
    /// <summary>
    /// Định nghĩa tất cả các endpoint của ứng dụng.
    /// </summary>
    /// <param name="app">Ứng dụng web.</param>
    public static void MapEndPoints(this WebApplication app)
    {
        app.MapAdminEndPoints();
        app.MapGiayChungNhanEndpoints();
        app.MapChuSuDungEndpoints();
        app.MapAuthenticationEndpoints();
        app.MapAppSettingsEndpoints();
        app.MapGeoEndPoints();
        app.MapSearchEndpoints();
        app.MapTaiSanEndpoints();
    }
}
