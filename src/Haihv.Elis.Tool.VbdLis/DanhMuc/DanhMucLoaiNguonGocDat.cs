namespace Haihv.Elis.Tool.VbdLis.DanhMuc;

/// <summary>
/// Danh mục các loại nguồn gốc đất.
/// </summary>
public static class DanhMucLoaiNguonGocDat
{    /// <summary>
     /// Công nhận QSDĐ như giao đất có thu tiền sử dụng đất.
     /// </summary>
    public static readonly LoaiNguonGocDat CongNhanQuySuDungDatCoThuTien = new("CNQ-CTT", "Công nhận QSDĐ như giao đất có thu tiền sử dụng đất");

    /// <summary>
    /// Công nhận QSDĐ như giao đất không thu tiền sử dụng đất.
    /// </summary>
    public static readonly LoaiNguonGocDat CongNhanQuySuDungDatKhongThuTien = new("CNQ-KTT", "Công nhận QSDĐ như giao đất không thu tiền sử dụng đất");

    /// <summary>
    /// Nhà nước cho thuê đất trả tiền hàng năm.
    /// </summary>
    public static readonly LoaiNguonGocDat NhaNuocChoThueDatTraTienHangNam = new("DT-THN", "Nhà nước cho thuê đất trả tiền hàng năm");

    /// <summary>
    /// Nhà nước cho thuê đất trả tiền một lần.
    /// </summary>
    public static readonly LoaiNguonGocDat NhaNuocChoThueDatTraTienMotLan = new("DT-TML", "Nhà nước cho thuê đất trả tiền một lần");

    /// <summary>
    /// Nhà nước công nhận quyền sử dụng đất.
    /// </summary>
    public static readonly LoaiNguonGocDat NhaNuocCongNhanQuyenSuDungDat = new("CNQ", "Nhà nước công nhận quyền sử dụng đất");

    /// <summary>
    /// Nhà nước giao đất có thu tiền sử dụng đất.
    /// </summary>
    public static readonly LoaiNguonGocDat NhaNuocGiaoDatCoThuTien = new("DG-CTT", "Nhà nước giao đất có thu tiền sử dụng đất");

    /// <summary>
    /// Nhà nước giao đất không thu tiền sử dụng đất.
    /// </summary>
    public static readonly LoaiNguonGocDat NhaNuocGiaoDatKhongThuTien = new("DG-KTT", "Nhà nước giao đất không thu tiền sử dụng đất");

    /// <summary>
    /// Nhà nước giao đất để quản lý.
    /// </summary>
    public static readonly LoaiNguonGocDat NhaNuocGiaoDatDeQuanLy = new("DG-QL", "Nhà nước giao đất để quản lý");

    /// <summary>
    /// Thuê đất của doanh nghiệp đầu tư hạ tầng khu công nghiệp, khu kinh tế, khu công nghệ cao.
    /// </summary>
    public static readonly LoaiNguonGocDat ThueDatDoanhNghiepDauTuHaTangKhuCongNghiep = new("DT-KCN", "Thuê đất của doanh nghiệp đầu tư hạ tầng khu công nghiệp, khu kinh tế, khu công nghệ cao");

    /// <summary>
    /// Thuê đất trả tiền hàng năm của doanh nghiệp đầu tư hạ tầng khu công nghiệp, khi kinh tế, khu công nghệ cao.
    /// </summary>
    public static readonly LoaiNguonGocDat ThueDatTraTienHangNamDoanhNghiepDauTuHaTang = new("DT-KCN-THN", "Thuê đất trả tiền hàng năm của doanh nghiệp đầu tư hạ tầng khu công nghiệp, khi kinh tế, khu công nghệ cao");

    /// <summary>
    /// Thuê đất trả tiền một lần của doanh nghiệp đầu tư hạ tầng khu công nghiệp, khu kinh tế, khu công nghệ cao.
    /// </summary>
    public static readonly LoaiNguonGocDat ThueDatTraTienMotLanDoanhNghiepDauTuHaTang = new("DT-KCN-TML", "Thuê đất trả tiền một lần của doanh nghiệp đầu tư hạ tầng khu công nghiệp, khu kinh tế, khu công nghệ cao");    /// <summary>
                                                                                                                                                                                                                           /// Danh sách tất cả các loại nguồn gốc đất.
                                                                                                                                                                                                                           /// </summary>
    public static readonly IReadOnlyList<LoaiNguonGocDat> TatCa = new[]
    {
        CongNhanQuySuDungDatCoThuTien,
        CongNhanQuySuDungDatKhongThuTien,
        NhaNuocChoThueDatTraTienHangNam,
        NhaNuocChoThueDatTraTienMotLan,
        NhaNuocCongNhanQuyenSuDungDat,
        NhaNuocGiaoDatCoThuTien,
        NhaNuocGiaoDatKhongThuTien,
        NhaNuocGiaoDatDeQuanLy,
        ThueDatDoanhNghiepDauTuHaTangKhuCongNghiep,
        ThueDatTraTienHangNamDoanhNghiepDauTuHaTang,
        ThueDatTraTienMotLanDoanhNghiepDauTuHaTang
    };

    /// <summary>
    /// Tìm mã loại nguồn gốc đất theo tên.
    /// </summary>
    /// <param name="ten">Tên loại nguồn gốc đất.</param>
    /// <returns>Mã loại nguồn gốc đất hoặc null nếu không tìm thấy.</returns>
    public static string? GetMaByTen(string ten)
    {
        return TatCa.FirstOrDefault(x => x.Ten.Equals(ten, StringComparison.OrdinalIgnoreCase))?.Ma;
    }

    /// <summary>
    /// Tìm tên loại nguồn gốc đất theo mã.
    /// </summary>
    /// <param name="ma">Mã loại nguồn gốc đất.</param>
    /// <returns>Tên loại nguồn gốc đất hoặc null nếu không tìm thấy.</returns>
    public static string? GetTenByMa(string ma)
    {
        return TatCa.FirstOrDefault(x => x.Ma.Equals(ma, StringComparison.OrdinalIgnoreCase))?.Ten;
    }
}
