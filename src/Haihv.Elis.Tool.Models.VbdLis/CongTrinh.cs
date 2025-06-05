using System.Xml.Serialization;

namespace Haihv.Elis.Tool.Models.VbdLis;

/// <summary>
/// Wrapper cho thông tin công trình xây dựng
/// </summary>
public class ThongTinCongTrinhXayDung
{
    /// <summary>
    /// Thông tin chi tiết công trình xây dựng
    /// </summary>
    [XmlElement("DC_CongTrinhXayDung")]
    public DcCongTrinhXayDung? CongTrinhXayDung { get; set; }
}

/// <summary>
/// Thông tin công trình xây dựng - Trang 48
/// </summary>
public class DcCongTrinhXayDung
{
    /// <summary>
    /// Mã định danh công trình xây dựng
    /// </summary>
    [XmlElement("congTrinhXayDungId")]
    public string? CongTrinhXayDungId { get; set; }

    /// <summary>
    /// Mã xã
    /// </summary>
    [XmlElement("maXa")]
    public string? MaXa { get; set; }

    /// <summary>
    /// Loại quyền sở hữu công trình xây dựng
    /// </summary>
    [XmlElement("loaiQuyenSoHuuCongTrinhXayDung")]
    public string? LoaiQuyenSoHuuCongTrinhXayDung { get; set; }

    /// <summary>
    /// Tên công trình
    /// </summary>
    [XmlElement("tenCongTrinh")]
    public string? TenCongTrinh { get; set; }

    /// <summary>
    /// Diện tích xây dựng
    /// </summary>
    [XmlElement("dienTichXayDung")]
    public decimal? DienTichXayDung { get; set; }

    /// <summary>
    /// Diện tích sàn
    /// </summary>
    [XmlElement("dienTichSan")]
    public decimal? DienTichSan { get; set; }

    /// <summary>
    /// Số tầng
    /// </summary>
    [XmlElement("soTang")]
    public int? SoTang { get; set; }

    /// <summary>
    /// Số tầng hầm
    /// </summary>
    [XmlElement("soTangHam")]
    public int? SoTangHam { get; set; }

    /// <summary>
    /// Năm xây dựng
    /// </summary>
    [XmlElement("namXayDung")]
    public int? NamXayDung { get; set; }

    /// <summary>
    /// Năm hoàn thành
    /// </summary>
    [XmlElement("namHoanThanh")]
    public int? NamHoanThanh { get; set; }

    /// <summary>
    /// Thời hạn sở hữu
    /// </summary>
    [XmlElement("thoiHanSoHuu")]
    public int? ThoiHanSoHuu { get; set; }

    /// <summary>
    /// Cấp hạng
    /// </summary>
    [XmlElement("capHang")]
    public string? CapHang { get; set; }

    /// <summary>
    /// Địa chỉ
    /// </summary>
    [XmlElement("diaChi")]
    public string? DiaChi { get; set; }

    /// <summary>
    /// Hạng mục công trình
    /// </summary>
    [XmlElement("DC_HangMucCongTrinh")]
    public DcHangMucCongTrinh? HangMucCongTrinh { get; set; }

    /// <summary>
    /// Thông tin địa chỉ chuẩn
    /// </summary>
    [XmlElement("ThongTinDiaChiChuan")]
    public ThongTinDiaChiChuan? ThongTinDiaChiChuan { get; set; }

    /// <summary>
    /// Thông tin liên kết thửa đất
    /// </summary>
    [XmlElement("ThongTinLienKetThuaDat")]
    public ThongTinLienKetThuaDat? ThongTinLienKetThuaDat { get; set; }

    /// <summary>
    /// Trang
    /// </summary>
    [XmlAttribute("Trang")]
    public string? Trang { get; set; }
}

/// <summary>
/// Hạng mục công trình - Trang 48
/// </summary>
public class DcHangMucCongTrinh
{
    /// <summary>
    /// Mã định danh hạng mục công trình
    /// </summary>
    [XmlElement("hangMucCongTrinhId")]
    public string? HangMucCongTrinhId { get; set; }

    /// <summary>
    /// Tên hạng mục
    /// </summary>
    [XmlElement("tenHangMuc")]
    public string? TenHangMuc { get; set; }

    /// <summary>
    /// Công năng
    /// </summary>
    [XmlElement("congNang")]
    public string? CongNang { get; set; }

    /// <summary>
    /// Diện tích xây dựng
    /// </summary>
    [XmlElement("dienTichXayDung")]
    public decimal? DienTichXayDung { get; set; }

    /// <summary>
    /// Diện tích sàn
    /// </summary>
    [XmlElement("dienTichSan")]
    public decimal? DienTichSan { get; set; }

    /// <summary>
    /// Số tầng
    /// </summary>
    [XmlElement("soTang")]
    public int? SoTang { get; set; }

    /// <summary>
    /// Số tầng hầm
    /// </summary>
    [XmlElement("soTangHam")]
    public int? SoTangHam { get; set; }

    /// <summary>
    /// Kết cấu
    /// </summary>
    [XmlElement("ketCau")]
    public string? KetCau { get; set; }

    /// <summary>
    /// Năm xây dựng
    /// </summary>
    [XmlElement("namXayDung")]
    public int? NamXayDung { get; set; }

    /// <summary>
    /// Năm hoàn thành
    /// </summary>
    [XmlElement("namHoanThanh")]
    public int? NamHoanThanh { get; set; }

    /// <summary>
    /// Thời hạn sở hữu
    /// </summary>
    [XmlElement("thoiHanSoHuu")]
    public int? ThoiHanSoHuu { get; set; }

    /// <summary>
    /// Cấp hạng
    /// </summary>
    [XmlElement("capHang")]
    public string? CapHang { get; set; }

    /// <summary>
    /// Địa chỉ chi tiết
    /// </summary>
    [XmlElement("diaChiChiTiet")]
    public string? DiaChiChiTiet { get; set; }

    /// <summary>
    /// Trang
    /// </summary>
    [XmlAttribute("Trang")]
    public string? Trang { get; set; }
}

/// <summary>
/// Wrapper cho thông tin công trình ngầm
/// </summary>
public class ThongTinCongTrinhNgam
{
    /// <summary>
    /// Thông tin chi tiết công trình ngầm
    /// </summary>
    [XmlElement("DC_CongTrinhNgam")]
    public DcCongTrinhNgam? CongTrinhNgam { get; set; }
}

/// <summary>
/// Thông tin công trình ngầm - Trang 47
/// </summary>
public class DcCongTrinhNgam
{
    /// <summary>
    /// Mã định danh công trình ngầm
    /// </summary>
    [XmlElement("congTrinhNgamId")]
    public string? CongTrinhNgamId { get; set; }

    /// <summary>
    /// Mã xã
    /// </summary>
    [XmlElement("maXa")]
    public string? MaXa { get; set; }

    /// <summary>
    /// Loại quyền sở hữu công trình ngầm
    /// </summary>
    [XmlElement("loaiQuyenSoHuuCongTrinhNgam")]
    public string? LoaiQuyenSoHuuCongTrinhNgam { get; set; }

    /// <summary>
    /// Tên công trình
    /// </summary>
    [XmlElement("tenCongTrinh")]
    public string? TenCongTrinh { get; set; }

    /// <summary>
    /// Loại công trình ngầm
    /// </summary>
    [XmlElement("loaiCongTrinhNgam")]
    public string? LoaiCongTrinhNgam { get; set; }

    /// <summary>
    /// Diện tích công trình
    /// </summary>
    [XmlElement("dienTichCongTrinh")]
    public decimal? DienTichCongTrinh { get; set; }

    /// <summary>
    /// Độ sâu tối đa
    /// </summary>
    [XmlElement("doSauToiDa")]
    public decimal? DoSauToiDa { get; set; }

    /// <summary>
    /// Vị trí đấu nối
    /// </summary>
    [XmlElement("viTriDauNoi")]
    public string? ViTriDauNoi { get; set; }

    /// <summary>
    /// Năm xây dựng
    /// </summary>
    [XmlElement("namXayDung")]
    public int? NamXayDung { get; set; }

    /// <summary>
    /// Năm hoàn thành
    /// </summary>
    [XmlElement("namHoanThanh")]
    public int? NamHoanThanh { get; set; }

    /// <summary>
    /// Thời hạn sở hữu
    /// </summary>
    [XmlElement("thoiHanSoHuu")]
    public int? ThoiHanSoHuu { get; set; }

    /// <summary>
    /// Địa chỉ
    /// </summary>
    [XmlElement("diaChi")]
    public string? DiaChi { get; set; }

    /// <summary>
    /// Thông tin địa chỉ chuẩn
    /// </summary>
    [XmlElement("ThongTinDiaChiChuan")]
    public ThongTinDiaChiChuan? ThongTinDiaChiChuan { get; set; }

    /// <summary>
    /// Thông tin liên kết thửa đất
    /// </summary>
    [XmlElement("ThongTinLienKetThuaDat")]
    public ThongTinLienKetThuaDat? ThongTinLienKetThuaDat { get; set; }

    /// <summary>
    /// Trang
    /// </summary>
    [XmlAttribute("Trang")]
    public string? Trang { get; set; }
}

/// <summary>
/// Wrapper cho thông tin rừng trồng
/// </summary>
public class ThongTinRungTrong
{
    /// <summary>
    /// Thông tin chi tiết rừng trồng
    /// </summary>
    [XmlElement("DC_RungTrong")]
    public DcRungTrong? RungTrong { get; set; }
}

/// <summary>
/// Thông tin rừng trồng - Trang 49
/// </summary>
public class DcRungTrong
{
    /// <summary>
    /// Mã định danh rừng trồng
    /// </summary>
    [XmlElement("rungTrongId")]
    public string? RungTrongId { get; set; }

    /// <summary>
    /// Tên rừng
    /// </summary>
    [XmlElement("tenRung")]
    public string? TenRung { get; set; }

    /// <summary>
    /// Loại cây rừng
    /// </summary>
    [XmlElement("loaiCayRung")]
    public string? LoaiCayRung { get; set; }

    /// <summary>
    /// Diện tích
    /// </summary>
    [XmlElement("dienTich")]
    public decimal? DienTich { get; set; }

    /// <summary>
    /// Địa chỉ
    /// </summary>
    [XmlElement("diaChi")]
    public string? DiaChi { get; set; }

    /// <summary>
    /// Thông tin địa chỉ chuẩn
    /// </summary>
    [XmlElement("ThongTinDiaChiChuan")]
    public ThongTinDiaChiChuan? ThongTinDiaChiChuan { get; set; }

    /// <summary>
    /// Thông tin liên kết thửa đất
    /// </summary>
    [XmlElement("ThongTinLienKetThuaDat")]
    public ThongTinLienKetThuaDat? ThongTinLienKetThuaDat { get; set; }

    /// <summary>
    /// Trang
    /// </summary>
    [XmlAttribute("Trang")]
    public string? Trang { get; set; }
}

/// <summary>
/// Wrapper cho thông tin cây lâu năm
/// </summary>
public class ThongTinCayLauNam
{
    /// <summary>
    /// Thông tin chi tiết cây lâu năm
    /// </summary>
    [XmlElement("DC_CayLauNam")]
    public DcCayLauNam? CayLauNam { get; set; }
}

/// <summary>
/// Thông tin cây lâu năm - Trang 49
/// </summary>
public class DcCayLauNam
{
    /// <summary>
    /// Mã định danh cây lâu năm
    /// </summary>
    [XmlElement("cayLauNamId")]
    public string? CayLauNamId { get; set; }

    /// <summary>
    /// Tên cây lâu năm
    /// </summary>
    [XmlElement("tenCayLauNam")]
    public string? TenCayLauNam { get; set; }

    /// <summary>
    /// Loại cây trồng
    /// </summary>
    [XmlElement("loaiCayTrong")]
    public string? LoaiCayTrong { get; set; }

    /// <summary>
    /// Diện tích
    /// </summary>
    [XmlElement("dienTich")]
    public decimal? DienTich { get; set; }

    /// <summary>
    /// Địa chỉ
    /// </summary>
    [XmlElement("diaChi")]
    public string? DiaChi { get; set; }

    /// <summary>
    /// Thông tin địa chỉ chuẩn
    /// </summary>
    [XmlElement("ThongTinDiaChiChuan")]
    public ThongTinDiaChiChuan? ThongTinDiaChiChuan { get; set; }

    /// <summary>
    /// Thông tin liên kết thửa đất
    /// </summary>
    [XmlElement("ThongTinLienKetThuaDat")]
    public ThongTinLienKetThuaDat? ThongTinLienKetThuaDat { get; set; }

    /// <summary>
    /// Trang
    /// </summary>
    [XmlAttribute("Trang")]
    public string? Trang { get; set; }
}