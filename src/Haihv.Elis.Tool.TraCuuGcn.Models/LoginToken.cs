namespace Haihv.Elis.Tool.TraCuuGcn.Models;

public record LoginToken(
    string AccessToken,
    string? RefreshToken = null);