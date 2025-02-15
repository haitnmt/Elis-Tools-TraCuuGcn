namespace Haihv.Elis.Tool.TraCuuGcn.Api.Settings;

public static class CacheSettings
{
    private static string ChuanHoa(this string input) => input.Trim().ToUpper();
    
    public const string ElisConnections = "ElisConnections";
    public const string SdeConnections = "SdeConnections";
    public static string ElisConnectionName(long maGcn) => $"ElisConnectionName:{maGcn}";
    public static string SdeConnectionName(long maGcn) => $"SdeConnectionName:{maGcn}";
    public static string KeyGiayChungNhan(long maGcn) => $"GiayChungNhan:{maGcn}";
    public static string KeyMaQr(long maGcn) => $"MaQr:{maGcn}";
    public static string KeyDonViInGcn(string maDonVi) => $"DonVi:{maDonVi.ChuanHoa()}";
    public static string KeyThuaDat(long maGcn) => $"ThuaDat:{maGcn}";
    public static string KeyDiaChiByMaDvhc(int maDvhc) => $"DVHC:{maDvhc}";
    public static string KeyChuSuDung(string soDinhDanh, long maGcn)
        => $"ChuSuDung:{soDinhDanh.ChuanHoa()}:{maGcn}";
    public static string KeyAuthentication(string soDinhDanh, long maGcn)
        => $"Authentication:{soDinhDanh.ChuanHoa()}:{maGcn}";
    public static string KeyQuocTich(int maQuocTich) => $"QuocTich:{maQuocTich}";
    public static string KeySearch(string query) => $"Search-Query:{query.ChuanHoa()}";
}