namespace Haihv.Elis.Tool.VbdLis.DanhMuc;

public enum LoaiDoiTuong
{
    CaNhan = 0,
    VoChong = 1,
    HoGiaDinh = 2,
    ToChuc = 3,
    CongDong = 4,
}
public static class LoaiDoiTuongVbdLisExtensions
{
    public static string ToTenTiengViet(this LoaiDoiTuong loaiDoiTuong)
    {
        return loaiDoiTuong switch
        {
            LoaiDoiTuong.CaNhan => "Cá nhân",
            LoaiDoiTuong.VoChong => "Vợ chồng",
            LoaiDoiTuong.HoGiaDinh => "Hộ gia đình",
            LoaiDoiTuong.ToChuc => "Tổ chức",
            LoaiDoiTuong.CongDong => "Cộng đồng",
            _ => "Không xác định"
        };
    }
}