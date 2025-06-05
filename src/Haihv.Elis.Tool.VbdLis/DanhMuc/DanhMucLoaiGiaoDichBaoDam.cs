using Haihv.Elis.Tools.Data.Models;

namespace Haihv.Elis.Tools.Data.DanhMuc;

/// <summary>
/// Danh mục các loại giao dịch bảo đảm
/// </summary>
public static class DanhMucLoaiGiaoDichBaoDam
{
    /// <summary>
    /// Thế chấp (Mã: 0)
    /// </summary>
    public static readonly LoaiGiaoDichBaoDam TheChap = new(0, "Thế chấp");

    /// <summary>
    /// Thế chấp bổ sung (Mã: 1)
    /// </summary>
    public static readonly LoaiGiaoDichBaoDam TheChapBoSung = new(1, "Thế chấp bổ sung");

    /// <summary>
    /// Xóa thế chấp (Mã: 2)
    /// </summary>
    public static readonly LoaiGiaoDichBaoDam XoaTheChap = new(2, "Xóa thế chấp");

    /// <summary>
    /// Rút tài sản thế chấp (Mã: 3)
    /// </summary>
    public static readonly LoaiGiaoDichBaoDam RutTaiSanTheChap = new(3, "Rút tài sản thế chấp");

    /// <summary>
    /// Thay đổi nội dung thế chấp (Mã: 4)
    /// </summary>
    public static readonly LoaiGiaoDichBaoDam ThayDoiNoiDungTheChap = new(4, "Thay đổi nội dung thế chấp");

    /// <summary>
    /// Thay đổi thông tin bên thế chấp (Mã: 5)
    /// </summary>
    public static readonly LoaiGiaoDichBaoDam ThayDoiThongTinBenTheChap = new(5, "Thay đổi thông tin bên thế chấp");

    /// <summary>
    /// Thay đổi thông tin bên nhận thế chấp (Mã: 6)
    /// </summary>
    public static readonly LoaiGiaoDichBaoDam ThayDoiThongTinBenNhanTheChap = new(6, "Thay đổi thông tin bên nhận thế chấp");

    /// <summary>
    /// Bổ sung tài sản thế chấp (Mã: 7)
    /// </summary>
    public static readonly LoaiGiaoDichBaoDam BoSungTaiSanTheChap = new(7, "Bổ sung tài sản thế chấp");

    /// <summary>
    /// Thế chấp có bảo lãnh (Mã: 8)
    /// </summary>
    public static readonly LoaiGiaoDichBaoDam TheChapCoBaoLanh = new(8, "Thế chấp có bảo lãnh");

    /// <summary>
    /// Thế chấp tài sản hình thành trong tương lai (Mã: 9)
    /// </summary>
    public static readonly LoaiGiaoDichBaoDam TheChapTaiSanHinhThanhTrongTuongLai = new(9, "Thế chấp tài sản hình thành trong tương lai");

    /// <summary>
    /// Thay đổi bên nhận thế chấp (Mã: 10)
    /// </summary>
    public static readonly LoaiGiaoDichBaoDam ThayDoiBenNhanTheChap = new(10, "Thay đổi bên nhận thế chấp");

    /// <summary>
    /// Danh sách tất cả các loại giao dịch bảo đảm
    /// </summary>
    public static readonly LoaiGiaoDichBaoDam[] TatCa =
    [
        TheChap,
        TheChapBoSung,
        XoaTheChap,
        RutTaiSanTheChap,
        ThayDoiNoiDungTheChap,
        ThayDoiThongTinBenTheChap,
        ThayDoiThongTinBenNhanTheChap,
        BoSungTaiSanTheChap,
        TheChapCoBaoLanh,
        TheChapTaiSanHinhThanhTrongTuongLai,
        ThayDoiBenNhanTheChap
    ];

    /// <summary>
    /// Lấy mã loại giao dịch bảo đảm theo tên
    /// </summary>
    /// <param name="ten">Tên loại giao dịch bảo đảm</param>
    /// <returns>Mã loại giao dịch bảo đảm nếu tìm thấy, null nếu không tìm thấy</returns>
    public static int? GetMaByTen(string ten)
    {
        if (string.IsNullOrWhiteSpace(ten))
            return null;

        var loaiGiaoDich = TatCa.FirstOrDefault(x =>
            string.Equals(x.Ten, ten.Trim(), StringComparison.OrdinalIgnoreCase));

        return loaiGiaoDich?.Ma;
    }

    /// <summary>
    /// Lấy tên loại giao dịch bảo đảm theo mã
    /// </summary>
    /// <param name="ma">Mã loại giao dịch bảo đảm</param>
    /// <returns>Tên loại giao dịch bảo đảm nếu tìm thấy, null nếu không tìm thấy</returns>
    public static string? GetTenByMa(int ma)
    {
        var loaiGiaoDich = TatCa.FirstOrDefault(x => x.Ma == ma);
        return loaiGiaoDich?.Ten;
    }
}