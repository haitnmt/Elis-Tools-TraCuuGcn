using Haihv.Extensions.String;

namespace Haihv.Elis.Tool.VbdLis.DanhMuc;

/// <summary>
/// Danh mục các loại mục đích sử dụng quy hoạch.
/// </summary>
public static class DanhMucLoaiMucDichSuDungQuyHoach
{
    /// <summary>
    /// Đất an ninh.
    /// </summary>
    public static readonly LoaiMucDichSuDungQuyHoach DatAnNinh = new("CAN", "Đất an ninh");

    /// <summary>
    /// Đất bãi thải, xử lý chất thải.
    /// </summary>
    public static readonly LoaiMucDichSuDungQuyHoach DatBaiThaiXuLyChatThai = new("DRA", "Đất bãi thải, xử lý chất thải");

    /// <summary>
    /// Đất chưa sử dụng.
    /// </summary>
    public static readonly LoaiMucDichSuDungQuyHoach DatChuaSuDung = new("CSD", "Đất chưa sử dụng");

    /// <summary>
    /// Đất chuyên trồng lúa nước.
    /// </summary>
    public static readonly LoaiMucDichSuDungQuyHoach DatChuyenTrongLuaNuoc = new("LUC", "Đất chuyên trồng lúa nước");

    /// <summary>
    /// Đất có di tích lịch sử - văn hóa.
    /// </summary>
    public static readonly LoaiMucDichSuDungQuyHoach DatCoDiTichLichSuVanHoa = new("DDT", "Đất có di tích lịch sử - văn hóa");

    /// <summary>
    /// Đất có mặt nước chuyên dùng.
    /// </summary>
    public static readonly LoaiMucDichSuDungQuyHoach DatCoMatNuocChuyenDung = new("MNC", "Đất có mặt nước chuyên dùng");

    /// <summary>
    /// Đất cơ sở sản xuất phi nông nghiệp.
    /// </summary>
    public static readonly LoaiMucDichSuDungQuyHoach DatCoSoSanXuatPhiNongNghiep = new("SKC", "Đất cơ sở sản xuất phi nông nghiệp");

    /// <summary>
    /// Đất cơ sở tín ngưỡng.
    /// </summary>
    public static readonly LoaiMucDichSuDungQuyHoach DatCoSoTinNguong = new("TIN", "Đất cơ sở tín ngưỡng");

    /// <summary>
    /// Đất cơ sở tôn giáo.
    /// </summary>
    public static readonly LoaiMucDichSuDungQuyHoach DatCoSoTonGiao = new("TON", "Đất cơ sở tôn giáo");

    /// <summary>
    /// Đất cụm công nghiệp.
    /// </summary>
    public static readonly LoaiMucDichSuDungQuyHoach DatCumCongNghiep = new("SKN", "Đất cụm công nghiệp");

    /// <summary>
    /// Đất danh lam thắng cảnh.
    /// </summary>
    public static readonly LoaiMucDichSuDungQuyHoach DatDanhLamThangCanh = new("DDL", "Đất danh lam thắng cảnh");

    /// <summary>
    /// Đất đô thị.
    /// </summary>
    public static readonly LoaiMucDichSuDungQuyHoach DatDoThi = new("KDT", "Đất đô thị");

    /// <summary>
    /// Đất khu chế xuất.
    /// </summary>
    public static readonly LoaiMucDichSuDungQuyHoach DatKhuCheXuat = new("SKT", "Đất khu chế xuất");

    /// <summary>
    /// Đất khu công nghệ cao.
    /// </summary>
    public static readonly LoaiMucDichSuDungQuyHoach DatKhuCongNgheCao = new("KCN", "Đất khu công nghệ cao");

    /// <summary>
    /// Đất khu công nghiệp.
    /// </summary>
    public static readonly LoaiMucDichSuDungQuyHoach DatKhuCongNghiep = new("SKK", "Đất khu công nghiệp");

    /// <summary>
    /// Đất khu kinh tế.
    /// </summary>
    public static readonly LoaiMucDichSuDungQuyHoach DatKhuKinhTe = new("KKT", "Đất khu kinh tế");

    /// <summary>
    /// Đất khu vui chơi, giải trí công cộng.
    /// </summary>
    public static readonly LoaiMucDichSuDungQuyHoach DatKhuVuiChoiGiaiTriCongCong = new("DKV", "Đất khu vui chơi, giải trí công cộng");

    /// <summary>
    /// Đất làm muối.
    /// </summary>
    public static readonly LoaiMucDichSuDungQuyHoach DatLamMuoi = new("LMU", "Đất làm muối");

    /// <summary>
    /// Đất làm nghĩa trang, nghĩa địa, nhà tang lễ, nhà hỏa táng.
    /// </summary>
    public static readonly LoaiMucDichSuDungQuyHoach DatLamNghiaTrangNghiaDiaNhaTangLeNhaHoaTang = new("NTD", "Đất làm nghĩa trang, nghĩa địa, nhà tang lễ, nhà hỏa táng");

    /// <summary>
    /// Đất nông nghiệp.
    /// </summary>
    public static readonly LoaiMucDichSuDungQuyHoach DatNongNghiep = new("NNP", "Đất nông nghiệp");

    /// <summary>
    /// Đất nông nghiệp khác.
    /// </summary>
    public static readonly LoaiMucDichSuDungQuyHoach DatNongNghiepKhac = new("NKH", "Đất nông nghiệp khác");

    /// <summary>
    /// Đất nuôi trồng thủy sản.
    /// </summary>
    public static readonly LoaiMucDichSuDungQuyHoach DatNuoiTrongThuySan = new("NTS", "Đất nuôi trồng thủy sản");

    /// <summary>
    /// Đất ở tại đô thị.
    /// </summary>
    public static readonly LoaiMucDichSuDungQuyHoach DatOTaiDoThi = new("ODT", "Đất ở tại đô thị");

    /// <summary>
    /// Đất ở tại nông thôn.
    /// </summary>
    public static readonly LoaiMucDichSuDungQuyHoach DatOTaiNongThon = new("ONT", "Đất ở tại nông thôn");

    /// <summary>
    /// Đất phát triển hạ tầng cấp quốc gia, cấp tỉnh, cấp huyện, cấp xã.
    /// </summary>
    public static readonly LoaiMucDichSuDungQuyHoach DatPhatTrienHaTangCapQuocGiaCapTinhCapHuyenCapXa = new("DHT", "Đất phát triển hạ tầng cấp quốc gia, cấp tỉnh, cấp huyện, cấp xã");

    /// <summary>
    /// Đất phi nông nghiệp.
    /// </summary>
    public static readonly LoaiMucDichSuDungQuyHoach DatPhiNongNghiep = new("PNN", "Đất phi nông nghiệp");

    /// <summary>
    /// Đất phi nông nghiệp khác.
    /// </summary>
    public static readonly LoaiMucDichSuDungQuyHoach DatPhiNongNghiepKhac = new("PNK", "Đất phi nông nghiệp khác");

    /// <summary>
    /// Đất quốc phòng.
    /// </summary>
    public static readonly LoaiMucDichSuDungQuyHoach DatQuocPhong = new("CQP", "Đất quốc phòng");

    /// <summary>
    /// Đất rừng đặc dụng.
    /// </summary>
    public static readonly LoaiMucDichSuDungQuyHoach DatRungDacDung = new("RDD", "Đất rừng đặc dụng");

    /// <summary>
    /// Đất rừng phòng hộ.
    /// </summary>
    public static readonly LoaiMucDichSuDungQuyHoach DatRungPhongHo = new("RPH", "Đất rừng phòng hộ");

    /// <summary>
    /// Đất rừng sản xuất.
    /// </summary>
    public static readonly LoaiMucDichSuDungQuyHoach DatRungSanXuat = new("RSX", "Đất rừng sản xuất");

    /// <summary>
    /// Đất sản xuất vật liệu xây dựng, làm đồ gốm.
    /// </summary>
    public static readonly LoaiMucDichSuDungQuyHoach DatSanXuatVatLieuXayDungLamDoGom = new("SKX", "Đất sản xuất vật liệu xây dựng, làm đồ gốm");

    /// <summary>
    /// Đất sinh hoạt cộng đồng.
    /// </summary>
    public static readonly LoaiMucDichSuDungQuyHoach DatSinhHoatCongDong = new("DSH", "Đất sinh hoạt cộng đồng");

    /// <summary>
    /// Đất sông, ngòi, kênh, rạch, suối.
    /// </summary>
    public static readonly LoaiMucDichSuDungQuyHoach DatSongNgoiKenhRachSuoi = new("SON", "Đất sông, ngòi, kênh, rạch, suối");

    /// <summary>
    /// Đất sử dụng cho hoạt động khoáng sản.
    /// </summary>
    public static readonly LoaiMucDichSuDungQuyHoach DatSuDungChoHoatDongKhoangSan = new("SKS", "Đất sử dụng cho hoạt động khoáng sản");

    /// <summary>
    /// Đất thương mại, dịch vụ.
    /// </summary>
    public static readonly LoaiMucDichSuDungQuyHoach DatThuongMaiDichVu = new("TMD", "Đất thương mại, dịch vụ");

    /// <summary>
    /// Đất trồng cây hàng năm khác.
    /// </summary>
    public static readonly LoaiMucDichSuDungQuyHoach DatTrongCayHangNamKhac = new("HNK", "Đất trồng cây hàng năm khác");

    /// <summary>
    /// Đất trồng cây lâu năm.
    /// </summary>
    public static readonly LoaiMucDichSuDungQuyHoach DatTrongCayLauNam = new("CLN", "Đất trồng cây lâu năm");

    /// <summary>
    /// Đất trồng lúa.
    /// </summary>
    public static readonly LoaiMucDichSuDungQuyHoach DatTrongLua = new("LUA", "Đất trồng lúa");

    /// <summary>
    /// Đất xây dựng cơ sở ngoại giao.
    /// </summary>
    public static readonly LoaiMucDichSuDungQuyHoach DatXayDungCoSoNgoaiGiao = new("DNG", "Đất xây dựng cơ sở ngoại giao");

    /// <summary>
    /// Đất xây dựng trụ sở cơ quan.
    /// </summary>
    public static readonly LoaiMucDichSuDungQuyHoach DatXayDungTruSoCoQuan = new("TSC", "Đất xây dựng trụ sở cơ quan");

    /// <summary>
    /// Đất xây dựng trụ sở của tổ chức sự nghiệp.
    /// </summary>
    public static readonly LoaiMucDichSuDungQuyHoach DatXayDungTruSoCuaToChucSuNghiep = new("DTS", "Đất xây dựng trụ sở của tổ chức sự nghiệp");

    /// <summary>
    /// Danh sách tất cả các loại mục đích sử dụng quy hoạch.
    /// </summary>
    public static readonly IReadOnlyList<LoaiMucDichSuDungQuyHoach> TatCa = new[]
    {
        DatAnNinh,
        DatBaiThaiXuLyChatThai,
        DatChuaSuDung,
        DatChuyenTrongLuaNuoc,
        DatCoDiTichLichSuVanHoa,
        DatCoMatNuocChuyenDung,
        DatCoSoSanXuatPhiNongNghiep,
        DatCoSoTinNguong,
        DatCoSoTonGiao,
        DatCumCongNghiep,
        DatDanhLamThangCanh,
        DatDoThi,
        DatKhuCheXuat,
        DatKhuCongNgheCao,
        DatKhuCongNghiep,
        DatKhuKinhTe,
        DatKhuVuiChoiGiaiTriCongCong,
        DatLamMuoi,
        DatLamNghiaTrangNghiaDiaNhaTangLeNhaHoaTang,
        DatNongNghiep,
        DatNongNghiepKhac,
        DatNuoiTrongThuySan,
        DatOTaiDoThi,
        DatOTaiNongThon,
        DatPhatTrienHaTangCapQuocGiaCapTinhCapHuyenCapXa,
        DatPhiNongNghiep,
        DatPhiNongNghiepKhac,
        DatQuocPhong,
        DatRungDacDung,
        DatRungPhongHo,
        DatRungSanXuat,
        DatSanXuatVatLieuXayDungLamDoGom,
        DatSinhHoatCongDong,
        DatSongNgoiKenhRachSuoi,
        DatSuDungChoHoatDongKhoangSan,
        DatThuongMaiDichVu,
        DatTrongCayHangNamKhac,
        DatTrongCayLauNam,
        DatTrongLua,
        DatXayDungCoSoNgoaiGiao,
        DatXayDungTruSoCoQuan,
        DatXayDungTruSoCuaToChucSuNghiep
    };    /// <summary>
          /// Lấy mã loại mục đích sử dụng quy hoạch theo tên.
          /// </summary>
          /// <para>
          /// Trả về mã tương ứng với tên loại mục đích sử dụng quy hoạch. Nếu không tìm thấy sẽ trả về null.
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
    /// Tìm tên loại mục đích sử dụng quy hoạch theo mã.
    /// </summary>
    /// <param name="ma">Mã loại mục đích sử dụng quy hoạch.</param>
    /// <returns>Tên loại mục đích sử dụng quy hoạch hoặc null nếu không tìm thấy.</returns>
    public static string? GetTenByMa(string ma)
    {
        return TatCa.FirstOrDefault(x => x.Ma.Equals(ma, StringComparison.OrdinalIgnoreCase))?.Ten;
    }
}