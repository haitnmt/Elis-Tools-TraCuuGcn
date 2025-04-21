namespace Haihv.Elis.Tool.TraCuuGcn.Api.Uri;

public static class GiayChungNhanUri
{
    private const string Group = $"{BaseUri.Api}/giay-chung-nhan";
    public const string Search = $"{BaseUri.Api}/search";
    public const string GetGiayChungNhan = $"{Group}";
    public const string UpdateGiayChungNhan = $"{Group}/update";
    public const string GetHasUpdatePermission = $"{Group}/has-update-permission";
    public const string DeleteMaQr = $"{Group}/delete-ma-qr";
    
    public static string SearchWithQuery(string query) => $"{Search}?query={query}";
    public static string GetGiayChungNhanWithQuery(string serial, string? soDinhDanh = null) 
        => $"{Group}?serial={serial}" + (soDinhDanh is not null ? $"&soDinhDanh={soDinhDanh}" : string.Empty);
    public static string GetHasUpdatePermissionWithQuery(string serial) => $"{Group}/has-update-permission?serial={serial}";
    public static string DeleteMaQrWithQuery(string serial) => $"{Group}/delete-ma-qr?serial={serial}";
}