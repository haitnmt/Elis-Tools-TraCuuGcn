using System.Xml.Serialization;

namespace Haihv.Elis.Tool.Models.VbdLis;

/// <summary>
/// Wrapper cho thông tin quyền sử dụng đất
/// </summary>
public class ThongTinQuyenSuDungDat
{
    /// <summary>
    /// Thông tin chi tiết quyền sử dụng đất
    /// </summary>
    [XmlElement("DC_QuyenSuDungDat")]
    public DcQuyenSuDungDat? QuyenSuDungDat { get; set; }
}

/// <summary>
/// Thông tin quyền sử dụng đất - Trang 50
/// </summary>
public class DcQuyenSuDungDat
{
    /// <summary>
    /// Mã định danh quyền sử dụng đất
    /// </summary>
    [XmlElement("quyenSuDungDatId")]
    public string? QuyenSuDungDatId { get; set; }

    /// <summary>
    /// Loại đối tượng
    /// </summary>
    [XmlElement("loaiDoiTuong")]
    public string? LoaiDoiTuong { get; set; }

    /// <summary>
    /// Mã định danh chủ sở hữu
    /// </summary>
    [XmlElement("chuId")]
    public string? ChuId { get; set; }

    /// <summary>
    /// Mã định danh thửa đất
    /// </summary>
    [XmlElement("thuaDatId")]
    public string? ThuaDatId { get; set; }

    /// <summary>
    /// Mã định danh mục đích sử dụng
    /// </summary>
    [XmlElement("mucDichSuDungId")]
    public string? MucDichSuDungId { get; set; }

    /// <summary>
    /// Mã định danh giấy chứng nhận
    /// </summary>
    [XmlElement("giayChungNhanId")]
    public string? GiayChungNhanId { get; set; }

    /// <summary>
    /// Mã định danh nghĩa vụ tài chính
    /// </summary>
    [XmlElement("nghiaVuTaiChinhId")]
    public string? NghiaVuTaiChinhId { get; set; }

    /// <summary>
    /// Mã định danh hạn chế quyền
    /// </summary>
    [XmlElement("hanCheQuyenId")]
    public string? HanCheQuyenId { get; set; }

    /// <summary>
    /// Trang
    /// </summary>
    [XmlAttribute("Trang")]
    public string? Trang { get; set; }
}

/// <summary>
/// Wrapper cho thông tin quyền quản lý đất
/// </summary>
public class ThongTinQuyenQuanLyDat
{
    /// <summary>
    /// Thông tin chi tiết quyền quản lý đất
    /// </summary>
    [XmlElement("DC_QuyenQuanLyDat")]
    public DcQuyenQuanLyDat? QuyenQuanLyDat { get; set; }
}

/// <summary>
/// Thông tin quyền quản lý đất - Trang 51
/// </summary>
public class DcQuyenQuanLyDat
{
    /// <summary>
    /// Mã định danh quyền quản lý đất
    /// </summary>
    [XmlElement("quyenQuanLyDatId")]
    public string? QuyenQuanLyDatId { get; set; }

    /// <summary>
    /// Loại đối tượng
    /// </summary>
    [XmlElement("loaiDoiTuong")]
    public string? LoaiDoiTuong { get; set; }

    /// <summary>
    /// Mã định danh chủ sở hữu
    /// </summary>
    [XmlElement("chuId")]
    public string? ChuId { get; set; }

    /// <summary>
    /// Mã định danh thửa đất
    /// </summary>
    [XmlElement("thuaDatId")]
    public string? ThuaDatId { get; set; }

    /// <summary>
    /// Mã định danh mục đích sử dụng
    /// </summary>
    [XmlElement("mucDichSuDungId")]
    public string? MucDichSuDungId { get; set; }

    /// <summary>
    /// Trang
    /// </summary>
    [XmlAttribute("Trang")]
    public string? Trang { get; set; }
}

/// <summary>
/// Wrapper cho thông tin quyền sở hữu tài sản gắn liền với đất
/// </summary>
public class ThongTinQuyenSoHuuTaiSanGanLienVoiDat
{
    /// <summary>
    /// Thông tin chi tiết quyền sở hữu tài sản gắn liền với đất
    /// </summary>
    [XmlElement("DC_QuyenSoHuuTaiSanGanLienVoiDat")]
    public DcQuyenSoHuuTaiSanGanLienVoiDat? QuyenSoHuuTaiSanGanLienVoiDat { get; set; }
}

/// <summary>
/// Thông tin quyền sở hữu tài sản gắn liền với đất - Trang 51
/// </summary>
public class DcQuyenSoHuuTaiSanGanLienVoiDat
{
    /// <summary>
    /// Mã định danh quyền sở hữu tài sản gắn liền với đất
    /// </summary>
    [XmlElement("quyenSoHuuTaiSanGanLienVoiDatId")]
    public string? QuyenSoHuuTaiSanGanLienVoiDatId { get; set; }

    /// <summary>
    /// Loại đối tượng
    /// </summary>
    [XmlElement("loaiDoiTuong")]
    public string? LoaiDoiTuong { get; set; }

    /// <summary>
    /// Mã định danh chủ sở hữu
    /// </summary>
    [XmlElement("chuId")]
    public string? ChuId { get; set; }

    /// <summary>
    /// Loại tài sản
    /// </summary>
    [XmlElement("loaiTaiSan")]
    public string? LoaiTaiSan { get; set; }

    /// <summary>
    /// Mã định danh tài sản
    /// </summary>
    [XmlElement("taiSanId")]
    public string? TaiSanId { get; set; }

    /// <summary>
    /// Mã định danh hạng mục tài sản
    /// </summary>
    [XmlElement("hangMucTaiSanId")]
    public string? HangMucTaiSanId { get; set; }

    /// <summary>
    /// Mã định danh giấy chứng nhận
    /// </summary>
    [XmlElement("giayChungNhanId")]
    public string? GiayChungNhanId { get; set; }

    /// <summary>
    /// Mã định danh nghĩa vụ tài chính
    /// </summary>
    [XmlElement("nghiaVuTaiChinhId")]
    public string? NghiaVuTaiChinhId { get; set; }

    /// <summary>
    /// Mã định danh hạn chế quyền
    /// </summary>
    [XmlElement("hanCheQuyenId")]
    public string? HanCheQuyenId { get; set; }

    /// <summary>
    /// Trang
    /// </summary>
    [XmlAttribute("Trang")]
    public string? Trang { get; set; }
}