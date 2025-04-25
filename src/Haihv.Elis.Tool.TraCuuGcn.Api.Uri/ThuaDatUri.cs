namespace Haihv.Elis.Tool.TraCuuGcn.Api.Uri;

public static class ThuaDatUri
{
    private const string Group = $"{BaseUri.Api}/thua-dat";
    public const string GetThuaDat = Group;
    public const string GetThuaDatPublic = $"{Group}/public";
    public const string GetToaDo = $"{Group}/toado";
    
    public static string GetThuaDatWithQuery(string serial, string? soDinhDanh = null) 
        => $"{Group}?serial={serial}" + (soDinhDanh is not null ? $"&soDinhDanh={soDinhDanh}" : string.Empty);
    public static string GetToaDoWithQuery(string serial, string? soDinhDanh = null) 
        => $"{Group}/toado?serial={serial}" + (soDinhDanh is not null ? $"&soDinhDanh={soDinhDanh}" : string.Empty);
    public static string GetThuaDatPublicWithQuery(string serial) => $"{Group}/public?serial={serial}";
}