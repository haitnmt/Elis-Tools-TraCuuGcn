namespace Haihv.Elis.Tool.TraCuuGcn.Api.Uri;

public static class ChuSuDungUri
{
    private const string Group = $"{BaseUri.Api}/chu-su-dung";
    public const string GetChuSuDung = $"{Group}";
    public const string GetHasReadPermission = $"{Group}/has-read-permission";
    public static string GetChuSuDungWithQuery(string serial, string? soDinhDanh = null) 
        => $"{Group}?serial={serial}" + (soDinhDanh is not null ? $"&soDinhDanh={soDinhDanh}" : string.Empty);
    public static string GetHasReadPermissionWithQuery(string serial, string soDinhDanh)  
        => $"{Group}/has-read-permission?serial={serial}&soDinhDanh={soDinhDanh}";
}