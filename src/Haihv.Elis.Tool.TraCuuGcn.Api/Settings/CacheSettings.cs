using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Settings;

public static class CacheSettings
{
    public const string ElisConnections = "ElisConnections";
    public static string ElisConnectionName(string serial) => $"ElisConnectionName:{serial}";
    
    public static string KeySerial(string maVach) => $"Serial:{maVach}";
    public static string KeyGiayChungNhan(string serial) => $"GiayChungNhan:{serial}";
    public static string KeyMaQr(string serial) => $"MaQr:{serial}";
    public static string KeyDonViInGcn(string maDonVi) => $"DonVi:{maDonVi}";
    public static string KeyThuaDat(string serial) => $"ThuaDat:{serial}";
    public static string KeyDiaChiByMaDvhc(int maDvhc) => $"DVHC:{maDvhc}";
    public static string KeyChuSuDung(string serial) => $"ChuSuDung:{serial}";
    public static string KeyAuthentication(string serial, string soDinhDanh)
        => $"Authentication:{serial.ChuanHoa()}:{soDinhDanh.ChuanHoa()}";
    public static string KeyQuocTich(int maQuocTich) => $"QuocTich:{maQuocTich}";
    public static string KeySearch(string query) => $"Search-Query:{query.ChuanHoa()}";
    public static string KeyToaDoThua(string serial) => $"ToaDoThua:{serial}";
    public static string KeyTaiSan(string serial) => $"TaiSan:{serial}";
    public static string KeyHasReadPermission(string email, string serial)
        => $"HasReadPermission:{email}:{serial}";
    public static string KeyReadCount(string email)
        => $"HasReadPermission:{email}:Count";
    public static string KeyReadLock(string email)
        => $"HasReadPermission:{email}:Lock";
}