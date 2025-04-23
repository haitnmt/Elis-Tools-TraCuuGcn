namespace Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;

public interface IAppSettingsService
{
    Task<AppSettings> GetAppSettingAsync(CancellationToken cancellationToken = default);
}
public sealed class AppSettings
{
    public string? ApiEndpoint { get; set; }
    public string? ApiVersion { get; set; }
    public string? AppVersion { get; set; }
    public bool IsDemoVersion { get; set; }
}