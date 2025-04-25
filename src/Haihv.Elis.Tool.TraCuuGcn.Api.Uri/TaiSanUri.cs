namespace Haihv.Elis.Tool.TraCuuGcn.Api.Uri;

public class TaiSanUri
{
    private const string Group = $"{BaseUri.Api}/tai-san";
    public const string GetTaiSan = $"{Group}";
    public static string GetTaiSanWithQuery(string serial, string? soDinhDanh = null) 
        => $"{Group}?serial={serial}" + (soDinhDanh is not null ? $"&soDinhDanh={soDinhDanh}" : string.Empty);
}