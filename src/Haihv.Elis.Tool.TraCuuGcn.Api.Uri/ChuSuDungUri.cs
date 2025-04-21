namespace Haihv.Elis.Tool.TraCuuGcn.Api.Uri;

public static class ChuSuDungUri
{
    private const string Group = $"{BaseUri.Api}/chu-su-dung";
    public const string GetChuSuDung = $"{Group}";
    public static string GetChuSuDungWithQuery(string serial, string? soDinhDanh = null) 
        => $"{Group}?serial={serial}" + (soDinhDanh is not null ? $"&soDinhDanh={soDinhDanh}" : string.Empty);
}