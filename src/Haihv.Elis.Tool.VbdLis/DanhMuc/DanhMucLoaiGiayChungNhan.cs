using Haihv.Elis.Tools.Data.Models;
namespace Haihv.Elis.Tools.Data.DanhMuc;

/// <summary>
/// Danh mục các loại giấy chứng nhận quyền sử dụng đất, quyền sở hữu nhà ở và tài sản khác gắn liền với đất.
/// <para>
/// - Sử dụng thuộc tính <c>TatCa</c> để lấy danh sách tất cả các loại giấy chứng nhận.
/// - Mỗi loại giấy chứng nhận được đại diện bởi một đối tượng <c>LoaiGiayChungNhan</c> với mã và tên cụ thể.
/// - Sử dụng <c>GetMaByTen(string ten)</c> để lấy mã từ tên loại giấy chứng nhận.
/// - Sử dụng <c>GetTenByMa(int ma)</c> để lấy tên từ mã loại giấy chứng nhận.
/// </para>
/// </summary>
public static class DanhMucLoaiGiayChungNhan
{
    /// <summary>
    /// Giấy chứng nhận QSDĐ theo Luật Đất Đai 2003
    /// </summary>
    public static readonly LoaiGiayChungNhan Luat2003 = new(1, "Giấy chứng nhận QSDĐ theo Luật Đất Đai 2003");
    /// <summary>
    /// Giấy chứng nhận QSDĐ theo Luật Đất Đai 1993
    /// </summary>
    public static readonly LoaiGiayChungNhan Luat1993 = new(2, "Giấy chứng nhận QSDĐ theo Luật Đất Đai 1993");
    /// <summary>
    /// Giấy chứng nhận QSHNƠ & QSDĐƠ theo Nghị định 60/NĐ-CP
    /// </summary>
    public static readonly LoaiGiayChungNhan NghiDinh60 = new(3, "Giấy chứng nhận QSHNƠ & QSDĐƠ theo Nghị định 60/NĐ-CP");
    /// <summary>
    /// Giấy chứng nhận QSHNƠ & QSDĐƠ theo Nghị định 90/NĐ-CP
    /// </summary>
    public static readonly LoaiGiayChungNhan NghiDinh90 = new(4, "Giấy chứng nhận QSHNƠ & QSDĐƠ theo Nghị định 90/NĐ-CP");
    /// <summary>
    /// Giấy chứng nhận sở hữu công trình theo quy định 95
    /// </summary>
    public static readonly LoaiGiayChungNhan SoHuuCongTrinh95 = new(5, "Giấy chứng nhận sở hữu công trình theo quy định 95");
    /// <summary>
    /// Giấy chứng nhận QSDĐƠ & QSHNƠ và TSKGLVĐ theo NĐ 88/NĐ-CP
    /// </summary>
    public static readonly LoaiGiayChungNhan NghiDinh88 = new(6, "Giấy chứng nhận QSDĐƠ & QSHNƠ và TSKGLVĐ theo NĐ 88/NĐ-CP");
    /// <summary>
    /// Giấy hợp thức hóa
    /// </summary>
    public static readonly LoaiGiayChungNhan HopThucHoa = new(7, "Giấy hợp thức hóa");
    /// <summary>
    /// Giấy chứng nhận QSDĐƠ & QSHNƠ và TSKGLVĐ theo NĐ 43/NĐ-CP
    /// </summary>
    public static readonly LoaiGiayChungNhan NghiDinh43 = new(11, "Giấy chứng nhận QSDĐƠ & QSHNƠ và TSKGLVĐ theo NĐ 43/NĐ-CP");
    /// <summary>
    /// Giấy phép Xây dựng
    /// </summary>
    public static readonly LoaiGiayChungNhan GiayPhepXayDung = new(17, "Giấy phép Xây dựng");
    /// <summary>
    /// Các loại giấy chứng nhận khác
    /// </summary>
    public static readonly LoaiGiayChungNhan LoaiKhac = new(18, "Các loại giấy chứng nhận khác");
    /// <summary>
    /// Giấy phép mua bán, chuyển dịch nhà
    /// </summary>
    public static readonly LoaiGiayChungNhan GiayPhepMuaBanChuyenDichNha = new(19, "Giấy phép mua bán, chuyển dịch nhà");
    /// <summary>
    /// Hợp đồng mua bán tài sản hình thành trong tương lai
    /// </summary>
    public static readonly LoaiGiayChungNhan HopDongMuaBanTshttt = new(99, "Hợp đồng mua bán tài sản hình thành trong tương lai");

    /// <summary>
    /// Danh sách tất cả các loại giấy chứng nhận.
    /// </summary>
    public static readonly IReadOnlyList<LoaiGiayChungNhan> TatCa =
    [
        Luat2003,
        Luat1993,
        NghiDinh60,
        NghiDinh90,
        SoHuuCongTrinh95,
        NghiDinh88,
        HopThucHoa,
        NghiDinh43,
        GiayPhepXayDung,
        LoaiKhac,
        GiayPhepMuaBanChuyenDichNha,
        HopDongMuaBanTshttt
    ];

    /// <summary>
    /// Lấy mã loại giấy chứng nhận theo tên.
    /// </summary>
    /// <param name="ten">Tên loại giấy chứng nhận</param>
    /// <returns>Mã loại giấy chứng nhận, trả về 0 nếu không tìm thấy</returns>
    public static int GetMaByTen(string ten)
    {
        var item = TatCa.FirstOrDefault(x => x.Ten == ten);
        return item?.Ma ?? 0;
    }

    /// <summary>
    /// Lấy tên loại giấy chứng nhận theo mã.
    /// </summary>
    /// <param name="ma">Mã loại giấy chứng nhận</param>
    /// <returns>Tên loại giấy chứng nhận, trả về null nếu không tìm thấy</returns>
    public static string? GetTenByMa(int ma)
    {
        var item = TatCa.FirstOrDefault(x => x.Ma == ma);
        return item?.Ten;
    }
}

