namespace Haihv.Elis.Tool.TraCuuGcn.Api.Settings;

public static class CacheSettings
{
    private static string ChuanHoa(this string input) => input.Trim().ToUpper();
    
    public const string ElisConnections = "ElisConnections";
    public const string SdeConnections = "SdeConnections";
    public static string ElisConnectionName(long maGcnElis) => $"ElisConnectionName:{maGcnElis}";
    public static string SdeConnectionName(long maGcnElis) => $"SdeConnectionName:{maGcnElis}";
    public static string KeyGiayChungNhan(long maGcnElis) => $"GiayChungNhan:{maGcnElis}";
    public static string KeyMaQr(long maGcnElis) => $"MaQr:{maGcnElis}";
    public static string KeyDonViInGcn(string maDonVi) => $"DonVi:{maDonVi.ChuanHoa()}";
    public static string KeyThuaDat(long maGcnElis) => $"ThuaDat:{maGcnElis}";
    public static string KeyDiaChiByMaDvhc(int maDvhc) => $"DVHC:{maDvhc}";
    public static string KeyChuSuDung(string soDinhDanh, long maGcnElis)
        => $"ChuSuDung:{soDinhDanh.ChuanHoa()}:{maGcnElis}";
    public static string KeyAuthentication(string soDinhDanh, long maGcnElis)
        => $"Authentication:{soDinhDanh.ChuanHoa()}:{maGcnElis}";
    public static string KeyQuocTich(int maQuocTich) => $"QuocTich:{maQuocTich}";
    public static string KeySearch(string query) => $"Search-Query:{query.ChuanHoa()}";
}