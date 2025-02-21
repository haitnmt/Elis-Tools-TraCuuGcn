namespace Haihv.Elis.Tool.TraCuuGcn.Api.Settings;

public static class CacheSettings
{
    private static string ChuanHoa(this string input) => input.Trim().ToUpper();
    
    public const string ElisConnections = "ElisConnections";
    public static string ElisConnectionName(long maGcnElis) => $"ElisConnectionName:{maGcnElis}";
    public static string KeyGiayChungNhan(long maGcnElis) => $"GiayChungNhan:{maGcnElis}";
    public static string KeyMaQr(long maGcnElis) => $"MaQr:{maGcnElis}";
    public static string KeyDonViInGcn(string maDonVi) => $"DonVi:{maDonVi.ChuanHoa()}";
    public static string KeyThuaDat(long maGcnElis) => $"ThuaDat:{maGcnElis}";
    public static string KeyDiaChiByMaDvhc(int maDvhc) => $"DVHC:{maDvhc}";
    public static string KeyChuSuDung(long maGcnElis) => $"ChuSuDung:{maGcnElis}";
    public static string KeyAuthentication(long maGcnElis, string soDinhDanh)
        => $"Authentication:{maGcnElis}:{soDinhDanh.ChuanHoa()}";
    public static string KeyQuocTich(int maQuocTich) => $"QuocTich:{maQuocTich}";
    public static string KeySearch(string query) => $"Search-Query:{query.ChuanHoa()}";
}