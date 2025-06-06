using Haihv.Extensions.String;

namespace Haihv.Elis.Tool.VbdLis.DanhMuc;

/// <summary>
/// Danh mục các loại nguồn gốc chuyển quyền.
/// </summary>
public static class DanhMucLoaiNguonGocChuyenQuyen
{
    /// <summary>
    /// Nhận chuyển đổi đất.
    /// </summary>
    public static readonly LoaiNguonGocDat NhanChuyenDoiDat = new("1", "Nhận chuyển đổi đất");    /// <summary>
                                                                                                  /// Nhận chuyển nhượng đất.
                                                                                                  /// </summary>
    public static readonly LoaiNguonGocDat NhanChuyenNhuongDat = new("2", "Nhận chuyển nhượng đất");

    /// <summary>
    /// Nhận thừa kế đất.
    /// </summary>
    public static readonly LoaiNguonGocDat NhanThuaKeDat = new("3", "Nhận thừa kế đất");

    /// <summary>
    /// Nhận góp vốn đất.
    /// </summary>
    public static readonly LoaiNguonGocDat NhanGopVonDat = new("4", "Nhận góp vốn đất");

    /// <summary>
    /// Được tặng cho đất.
    /// </summary>
    public static readonly LoaiNguonGocDat DuocTangChoDat = new("5", "Được tặng cho đất");

    /// <summary>
    /// Nhận chuyển quyền do trúng đấu giá đất.
    /// </summary>
    public static readonly LoaiNguonGocDat NhanChuyenQuyenDoTrungDauGiaDat = new("6", "Nhận chuyển quyền do trúng đấu giá đất");

    /// <summary>
    /// Nhận chuyển quyền do xử lý nợ thế chấp đất.
    /// </summary>
    public static readonly LoaiNguonGocDat NhanChuyenQuyenDoXuLyNoTheChapDat = new("7", "Nhận chuyển quyền do xử lý nợ thế chấp đất");

    /// <summary>
    /// Nhận chuyển quyền do giải quyết tranh chấp đất.
    /// </summary>
    public static readonly LoaiNguonGocDat NhanChuyenQuyenDoGiaiQuyetTranhChapDat = new("8", "Nhận chuyển quyền do giải quyết tranh chấp đất");

    /// <summary>
    /// Nhận chuyển quyền do giải quyết khiếu nại về đất.
    /// </summary>
    public static readonly LoaiNguonGocDat NhanChuyenQuyenDoGiaiQuyetKhieuNaiVeDat = new("9", "Nhận chuyển quyền do giải quyết khiếu nại về đất");

    /// <summary>
    /// Nhận chuyển quyền do giải quyết tố cáo về đất.
    /// </summary>
    public static readonly LoaiNguonGocDat NhanChuyenQuyenDoGiaiQuyetToCaoVeDat = new("10", "Nhận chuyển quyền do giải quyết tố cáo về đất");

    /// <summary>
    /// Nhận chuyển quyền do kết quả hòa giải thành.
    /// </summary>
    public static readonly LoaiNguonGocDat NhanChuyenQuyenDoKetQuaHoaGiaiThanh = new("14", "Nhận chuyển quyền do kết quả hòa giải thành");

    /// <summary>
    /// Nhận chuyển quyền.
    /// </summary>
    public static readonly LoaiNguonGocDat NhanChuyenQuyen = new("15", "Nhận chuyển quyền");

    /// <summary>
    /// Nhận chuyển quyền theo kết quả đấu giá.
    /// </summary>
    public static readonly LoaiNguonGocDat NhanChuyenQuyenTheoKetQuaDauGia = new("16", "Nhận chuyển quyền theo kết quả đấu giá");

    /// <summary>
    /// Nhận chuyển quyền theo bản án.
    /// </summary>
    public static readonly LoaiNguonGocDat NhanChuyenQuyenTheoBanAn = new("17", "Nhận chuyển quyền theo bản án");

    /// <summary>
    /// Nhận chuyển quyền do thực hiện quyết định thi hành án.
    /// </summary>
    public static readonly LoaiNguonGocDat NhanChuyenQuyenDoThucHienQuyetDinhThiHanhAn = new("18", "Nhận chuyển quyền do thực hiện quyết định thi hành án");

    /// <summary>
    /// Phân chia quyền sử dụng đất.
    /// </summary>
    public static readonly LoaiNguonGocDat PhanChiaQuyenSuDungDat = new("19", "Phân chia quyền sử dụng đất");

    /// <summary>
    /// Nhận quyền sử dụng đất theo quyết định chia tách, sát nhập tổ chức.
    /// </summary>
    public static readonly LoaiNguonGocDat NhanQuyenSuDungDatTheoQuyetDinhChiaTachSatNhapToChuc = new("20", "Nhận quyền sử dụng đất theo quyết định chia tách, sát nhập tổ chức");

    /// <summary>
    /// Nhận quyền sử dụng đất từ quyền sử dụng chung của hộ gia đình.
    /// </summary>
    public static readonly LoaiNguonGocDat NhanQuyenSuDungDatTuQuyenSuDungChungCuaHoGiaDinh = new("21", "Nhận quyền sử dụng đất từ quyền sử dụng chung của hộ gia đình");    /// <summary>
                                                                                                                                                                             /// Danh sách tất cả các loại nguồn gốc chuyển quyền.
                                                                                                                                                                             /// </summary>
    public static readonly IReadOnlyList<LoaiNguonGocDat> TatCa = new[]
    {
        NhanChuyenDoiDat,
        NhanChuyenNhuongDat,
        NhanThuaKeDat,
        NhanGopVonDat,
        DuocTangChoDat,
        NhanChuyenQuyenDoTrungDauGiaDat,
        NhanChuyenQuyenDoXuLyNoTheChapDat,
        NhanChuyenQuyenDoGiaiQuyetTranhChapDat,
        NhanChuyenQuyenDoGiaiQuyetKhieuNaiVeDat,
        NhanChuyenQuyenDoGiaiQuyetToCaoVeDat,
        NhanChuyenQuyenDoKetQuaHoaGiaiThanh,
        NhanChuyenQuyen,
        NhanChuyenQuyenTheoKetQuaDauGia,
        NhanChuyenQuyenTheoBanAn,
        NhanChuyenQuyenDoThucHienQuyetDinhThiHanhAn,
        PhanChiaQuyenSuDungDat,
        NhanQuyenSuDungDatTheoQuyetDinhChiaTachSatNhapToChuc,
        NhanQuyenSuDungDatTuQuyenSuDungChungCuaHoGiaDinh
    };

    /// <summary>
    /// Lấy mã dân tộc theo tên.
    /// </summary>
    /// <para>
    /// Trả về mã tương ứng với tên dân tộc. Nếu không tìm thấy sẽ trả về null.
    /// </para>
    /// <para>
    /// Phương thức này không phân biệt chữ hoa/thường, bỏ qua dấu cách và ký tự đặc biệt,
    /// đồng thời chuẩn hóa các ký tự tiếng Việt (ví dụ: â, ă = a).
    /// </para>
    public static string? GetMaByTen(string ten)
    {
        if (string.IsNullOrWhiteSpace(ten))
            return null;

        var normalizedInput = ten.NormalizeVietnameseName();
        return TatCa.FirstOrDefault(x => x.Ten.NormalizeVietnameseName() == normalizedInput)?.Ma;
    }

    /// <summary>
    /// Tìm tên loại nguồn gốc chuyển quyền theo mã.
    /// </summary>
    /// <param name="ma">Mã loại nguồn gốc chuyển quyền.</param>
    /// <returns>Tên loại nguồn gốc chuyển quyền hoặc null nếu không tìm thấy.</returns>
    public static string? GetTenByMa(string ma)
    {
        return TatCa.FirstOrDefault(x => x.Ma.Equals(ma, StringComparison.OrdinalIgnoreCase))?.Ten;
    }
}
