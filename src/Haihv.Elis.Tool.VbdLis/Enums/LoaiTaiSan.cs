namespace Haihv.Elis.Tool.VbdLis.Enums;

public enum LoaiTaiSanVbdLis
{
    ThuaDat = 6,
    NhaRiengLe = 7,
    CanHo = 8,
    NhaChungCu = 9,
    KhuChungCu = 10,
    CongTrinhXayDung = 11,
    CongTrinhNgam = 12,
    RungTrong = 13,
    CayLauNam = 14
}

public static class LoaiTaiSanVbdLisExtensions
{
    public static string ToTenTiengViet(this LoaiTaiSanVbdLis loaiTaiSan)
    {
        return loaiTaiSan switch
        {
            LoaiTaiSanVbdLis.ThuaDat => "Thửa đất",
            LoaiTaiSanVbdLis.NhaRiengLe => "Nhà riêng lẻ",
            LoaiTaiSanVbdLis.CanHo => "Căn hộ",
            LoaiTaiSanVbdLis.NhaChungCu => "Nhà chung cư",
            LoaiTaiSanVbdLis.KhuChungCu => "Khu chung cư",
            LoaiTaiSanVbdLis.CongTrinhXayDung => "Công trình xây dựng",
            LoaiTaiSanVbdLis.CongTrinhNgam => "Công trình ngầm",
            LoaiTaiSanVbdLis.RungTrong => "Rừng trồng",
            LoaiTaiSanVbdLis.CayLauNam => "Cây lâu năm",
            _ => "Không xác định"
        };
    }
}