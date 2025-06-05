using System.Xml.Serialization;

namespace Haihv.Elis.Tool.VbdLis;

/// <summary>
/// Wrapper cho thông tin tài sản
/// </summary>
public class ThongTinTaiSan
{
    /// <summary>
    /// Thông tin thửa đất
    /// </summary>
    [XmlElement("ThongTinThuaDat")]
    public ThongTinThuaDat? ThongTinThuaDat { get; set; }

    /// <summary>
    /// Thông tin nhà riêng lẻ
    /// </summary>
    [XmlElement("ThongTinNhaRiengLe")]
    public ThongTinNhaRiengLe? ThongTinNhaRiengLe { get; set; }

    /// <summary>
    /// Thông tin khu chung cư
    /// </summary>
    [XmlElement("ThongTinKhuChungCu")]
    public ThongTinKhuChungCu? ThongTinKhuChungCu { get; set; }

    /// <summary>
    /// Thông tin nhà chung cư
    /// </summary>
    [XmlElement("ThongTinNhaChungCu")]
    public ThongTinNhaChungCu? ThongTinNhaChungCu { get; set; }

    /// <summary>
    /// Thông tin căn hộ
    /// </summary>
    [XmlElement("ThongTinCanHo")]
    public ThongTinCanHo? ThongTinCanHo { get; set; }

    /// <summary>
    /// Thông tin công trình xây dựng
    /// </summary>
    [XmlElement("ThongTinCongTrinhXayDung")]
    public ThongTinCongTrinhXayDung? ThongTinCongTrinhXayDung { get; set; }

    /// <summary>
    /// Thông tin công trình ngầm
    /// </summary>
    [XmlElement("ThongTinCongTrinhNgam")]
    public ThongTinCongTrinhNgam? ThongTinCongTrinhNgam { get; set; }

    /// <summary>
    /// Thông tin rừng trồng
    /// </summary>
    [XmlElement("ThongTinRungTrong")]
    public ThongTinRungTrong? ThongTinRungTrong { get; set; }

    /// <summary>
    /// Thông tin cây lâu năm
    /// </summary>
    [XmlElement("ThongTinCayLauNam")]
    public ThongTinCayLauNam? ThongTinCayLauNam { get; set; }
}

/// <summary>
/// Wrapper cho thông tin thửa đất
/// </summary>
public class ThongTinThuaDat
{
    /// <summary>
    /// Thông tin chi tiết thửa đất
    /// </summary>
    [XmlElement("DC_ThuaDat")]
    public DcThuaDat? ThuaDat { get; set; }
}

/// <summary>
/// Thông tin thửa đất - Trang 37
/// </summary>
public class DcThuaDat
{
    /// <summary>
    /// Mã định danh thửa đất
    /// </summary>
    [XmlElement("thuaDatId")]
    public string? ThuaDatId { get; set; }

    /// <summary>
    /// Mã xã
    /// </summary>
    [XmlElement("maXa")]
    public string? MaXa { get; set; }

    /// <summary>
    /// Số hiệu tờ bản đồ
    /// </summary>
    [XmlElement("soHieuToBanDo")]
    public string? SoHieuToBanDo { get; set; }

    /// <summary>
    /// Số thứ tự thửa
    /// </summary>
    [XmlElement("soThuTuThua")]
    public string? SoThuTuThua { get; set; }

    /// <summary>
    /// Số hiệu tờ bản đồ cũ
    /// </summary>
    [XmlElement("soHieuToBanDoCu")]
    public string? SoHieuToBanDoCu { get; set; }

    /// <summary>
    /// Số thứ tự thửa cũ
    /// </summary>
    [XmlElement("soThuTuThuaCu")]
    public string? SoThuTuThuaCu { get; set; }

    /// <summary>
    /// Diện tích
    /// </summary>
    [XmlElement("dienTich")]
    public decimal? DienTich { get; set; }

    /// <summary>
    /// Diện tích pháp lý
    /// </summary>
    [XmlElement("dienTichPhapLy")]
    public decimal? DienTichPhapLy { get; set; }

    /// <summary>
    /// Là đối tượng chiếm đất
    /// </summary>
    [XmlElement("laDoiTuongChiemDat")]
    public bool? LaDoiTuongChiemDat { get; set; }

    /// <summary>
    /// Địa chỉ
    /// </summary>
    [XmlElement("diaChi")]
    public string? DiaChi { get; set; }

    /// <summary>
    /// In số liệu cũ
    /// </summary>
    [XmlElement("inSoLieuCu")]
    public bool? InSoLieuCu { get; set; }

    /// <summary>
    /// Loại thửa đất
    /// </summary>
    [XmlElement("loaiThuaDat")]
    public string? LoaiThuaDat { get; set; }

    /// <summary>
    /// Mã định danh tài liệu đo đạc
    /// </summary>
    [XmlElement("taiLieuDoDacId")]
    public string? TaiLieuDoDacId { get; set; }

    /// <summary>
    /// Thông tin mục đích sử dụng
    /// </summary>
    [XmlElement("ThongTinMucDichSuDung")]
    public ThongTinMucDichSuDung? ThongTinMucDichSuDung { get; set; }

    /// <summary>
    /// Thông tin địa chỉ chuẩn
    /// </summary>
    [XmlElement("ThongTinDiaChiChuan")]
    public ThongTinDiaChiChuan? ThongTinDiaChiChuan { get; set; }

    /// <summary>
    /// Trang
    /// </summary>
    [XmlAttribute("Trang")]
    public string? Trang { get; set; }
}

/// <summary>
/// Thông tin mục đích sử dụng
/// </summary>
public class ThongTinMucDichSuDung
{
    /// <summary>
    /// Mục đích sử dụng
    /// </summary>
    [XmlElement("DC_MucDichSuDung")]
    public DcMucDichSuDung? MucDichSuDung { get; set; }
}

/// <summary>
/// Mục đích sử dụng - Trang 38
/// </summary>
public class DcMucDichSuDung
{
    /// <summary>
    /// Mã định danh mục đích sử dụng
    /// </summary>
    [XmlElement("mucDichSuDungId")]
    public string? MucDichSuDungId { get; set; }

    /// <summary>
    /// Số thứ tự mục đích sử dụng
    /// </summary>
    [XmlElement("soThuTuMucDichSuDung")]
    public int? SoThuTuMucDichSuDung { get; set; }

    /// <summary>
    /// Mã mục đích sử dụng
    /// </summary>
    [XmlElement("maMucDichSuDung")]
    public string? MaMucDichSuDung { get; set; }

    /// <summary>
    /// Mã mục đích sử dụng phụ
    /// </summary>
    [XmlElement("maMucDichSuDungPhu")]
    public string? MaMucDichSuDungPhu { get; set; }

    /// <summary>
    /// Mã mục đích sử dụng quy hoạch
    /// </summary>
    [XmlElement("maMucDichSuDungQuyHoach")]
    public string? MaMucDichSuDungQuyHoach { get; set; }

    /// <summary>
    /// Diện tích
    /// </summary>
    [XmlElement("dienTich")]
    public decimal? DienTich { get; set; }

    /// <summary>
    /// Thời hạn sử dụng
    /// </summary>
    [XmlElement("thoiHanSuDung")]
    public int? ThoiHanSuDung { get; set; }

    /// <summary>
    /// Ngày hết hạn sử dụng
    /// </summary>
    [XmlElement("ngayHetHanSuDung")]
    public DateTime? NgayHetHanSuDung { get; set; }

    /// <summary>
    /// Thông tin nguồn gốc đất
    /// </summary>
    [XmlElement("ThongTinNguonGocDat")]
    public ThongTinNguonGocDat? ThongTinNguonGocDat { get; set; }

    /// <summary>
    /// Trang
    /// </summary>
    [XmlAttribute("Trang")]
    public string? Trang { get; set; }
}

/// <summary>
/// Thông tin nguồn gốc đất
/// </summary>
public class ThongTinNguonGocDat
{
    /// <summary>
    /// Nguồn gốc đất
    /// </summary>
    [XmlElement("DC_NguonGocDat")]
    public DcNguonGocDat? NguonGocDat { get; set; }
}

/// <summary>
/// Nguồn gốc đất - Trang 38
/// </summary>
public class DcNguonGocDat
{
    /// <summary>
    /// Mã định danh nguồn gốc đất
    /// </summary>
    [XmlElement("nguonGocDatId")]
    public string? NguonGocDatId { get; set; }

    /// <summary>
    /// Nguồn gốc
    /// </summary>
    [XmlElement("nguonGoc")]
    public string? NguonGoc { get; set; }

    /// <summary>
    /// Nguồn gốc chuyển quyền
    /// </summary>
    [XmlElement("nguonGocChuyenQuyen")]
    public string? NguonGocChuyenQuyen { get; set; }

    /// <summary>
    /// Nguồn gốc chi tiết
    /// </summary>
    [XmlElement("nguonGocChiTiet")]
    public string? NguonGocChiTiet { get; set; }

    /// <summary>
    /// Diện tích
    /// </summary>
    [XmlElement("dienTich")]
    public decimal? DienTich { get; set; }

    /// <summary>
    /// Trang
    /// </summary>
    [XmlAttribute("Trang")]
    public string? Trang { get; set; }
}

/// <summary>
/// Thông tin địa chỉ chuẩn
/// </summary>
public class ThongTinDiaChiChuan
{
    /// <summary>
    /// Địa chỉ chuẩn
    /// </summary>
    [XmlElement("DiaChiChuan")]
    public DiaChiChuan? DiaChiChuan { get; set; }
}

/// <summary>
/// Địa chỉ chuẩn
/// </summary>
public class DiaChiChuan
{
    /// <summary>
    /// Mã định danh địa chỉ
    /// </summary>
    [XmlElement("diaChiId")]
    public string? DiaChiId { get; set; }
}

/// <summary>
/// Thông tin liên kết thửa đất
/// </summary>
public class ThongTinLienKetThuaDat
{
    /// <summary>
    /// Liên kết thửa đất
    /// </summary>
    [XmlElement("LienKetThuaDat")]
    public LienKetThuaDat? LienKetThuaDat { get; set; }
}

/// <summary>
/// Liên kết thửa đất
/// </summary>
public class LienKetThuaDat
{
    /// <summary>
    /// Mã định danh thửa đất
    /// </summary>
    [XmlElement("thuaDatId")]
    public string? ThuaDatId { get; set; }
}