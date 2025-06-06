namespace Haihv.Elis.Tool.VbdLis.DanhMuc;

public enum LoaiTaiSan
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
    public static string ToTenTiengViet(this LoaiTaiSan loaiTaiSan)
    {
        return loaiTaiSan switch
        {
            LoaiTaiSan.ThuaDat => "Thửa đất",
            LoaiTaiSan.NhaRiengLe => "Nhà riêng lẻ",
            LoaiTaiSan.CanHo => "Căn hộ",
            LoaiTaiSan.NhaChungCu => "Nhà chung cư",
            LoaiTaiSan.KhuChungCu => "Khu chung cư",
            LoaiTaiSan.CongTrinhXayDung => "Công trình xây dựng",
            LoaiTaiSan.CongTrinhNgam => "Công trình ngầm",
            LoaiTaiSan.RungTrong => "Rừng trồng",
            LoaiTaiSan.CayLauNam => "Cây lâu năm",
            _ => "Không xác định"
        };
    }
}