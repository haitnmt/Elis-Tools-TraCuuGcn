namespace Haihv.Elis.Tool.TraCuuGcn.Api.Settings;

public sealed class JwtTokenSettings
{
    public List<string> SecretKeys { get; set; } = [];
    public List<string> Issuers { get; set; } = [];
    public List<string> Audiences { get; set; } = [];
    
}