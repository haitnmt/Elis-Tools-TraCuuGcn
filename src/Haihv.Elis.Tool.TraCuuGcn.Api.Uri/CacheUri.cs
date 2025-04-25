
namespace Haihv.Elis.Tool.TraCuuGcn.Api.Uri;

public static class CacheUri
{
    private const string Group = $"{BaseUri.Api}/cache";
    public const string Delete = $"{Group}/delete";
    public static string DeleteWithQuery(string serial) => $"{Group}/delete?serial={serial}";
}