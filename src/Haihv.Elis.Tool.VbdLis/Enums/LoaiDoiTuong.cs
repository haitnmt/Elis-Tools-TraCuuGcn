namespace Haihv.Elis.Tool.VbdLis.Enums;

public enum LoaiDoiTuongVbdLis
{
    CaNhan = 0,
    VoChong = 1,
    HoGiaDinh = 2,
    ToChuc = 3,
    CongDong = 4,
}
public static class LoaiDoiTuongVbdLisExtensions
{
    public static string ToTenTiengViet(this LoaiDoiTuongVbdLis loaiDoiTuong)
    {
        return loaiDoiTuong switch
        {
            LoaiDoiTuongVbdLis.CaNhan => "Cá nhân",
            LoaiDoiTuongVbdLis.VoChong => "Vợ chồng",
            LoaiDoiTuongVbdLis.HoGiaDinh => "Hộ gia đình",
            LoaiDoiTuongVbdLis.ToChuc => "Tổ chức",
            LoaiDoiTuongVbdLis.CongDong => "Cộng đồng",
            _ => "Không xác định"
        };
    }
}