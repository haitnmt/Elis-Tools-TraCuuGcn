using System.Xml.Serialization;

namespace Haihv.Elis.Tool.VbdLis;

/// <summary>
/// Wrapper cho thông tin nghĩa vụ tài chính
/// </summary>
public class ThongTinNghiaVuTaiChinh
{
    /// <summary>
    /// Thông tin chi tiết nghĩa vụ tài chính
    /// </summary>
    [XmlElement("DC_NghiaVuTaiChinh")]
    public DcNghiaVuTaiChinh? NghiaVuTaiChinh { get; set; }
}

/// <summary>
/// Thông tin nghĩa vụ tài chính - Trang 52
/// </summary>
public class DcNghiaVuTaiChinh
{
    /// <summary>
    /// Mã định danh nghĩa vụ tài chính
    /// </summary>
    [XmlElement("nghiaVuTaiChinhId")]
    public string? NghiaVuTaiChinhId { get; set; }

    /// <summary>
    /// Loại nghĩa vụ tài chính
    /// </summary>
    [XmlElement("loaiNghiaVuTaiChinh")]
    public string? LoaiNghiaVuTaiChinh { get; set; }

    /// <summary>
    /// Tổng số tiền
    /// </summary>
    [XmlElement("tongSoTien")]
    public decimal? TongSoTien { get; set; }

    /// <summary>
    /// Tổng số tiền miễn giảm
    /// </summary>
    [XmlElement("tongSoTienMienGiam")]
    public decimal? TongSoTienMienGiam { get; set; }

    /// <summary>
    /// Tổng số tiền nợ
    /// </summary>
    [XmlElement("tongSoTienNo")]
    public decimal? TongSoTienNo { get; set; }

    /// <summary>
    /// Ngày bắt đầu
    /// </summary>
    [XmlElement("ngayBatDau")]
    public DateTime? NgayBatDau { get; set; }

    /// <summary>
    /// Hoàn thành
    /// </summary>
    [XmlElement("hoanThanh")]
    public bool? HoanThanh { get; set; }

    /// <summary>
    /// Trang
    /// </summary>
    [XmlAttribute("Trang")]
    public string? Trang { get; set; }
}

/// <summary>
/// Wrapper cho thông tin miễn giảm nghĩa vụ tài chính
/// </summary>
public class ThongTinMienGiamNghiaVuTaiChinh
{
    /// <summary>
    /// Thông tin chi tiết miễn giảm nghĩa vụ tài chính
    /// </summary>
    [XmlElement("DC_MienGiamNghiaVuTaiChinh")]
    public DcMienGiamNghiaVuTaiChinh? MienGiamNghiaVuTaiChinh { get; set; }
}

/// <summary>
/// Thông tin miễn giảm nghĩa vụ tài chính - Trang 53
/// </summary>
public class DcMienGiamNghiaVuTaiChinh
{
    /// <summary>
    /// Mã định danh miễn giảm nghĩa vụ tài chính
    /// </summary>
    [XmlElement("mienGiamNghiaVuTaiChinhId")]
    public string? MienGiamNghiaVuTaiChinhId { get; set; }

    /// <summary>
    /// Mã định danh nghĩa vụ tài chính
    /// </summary>
    [XmlElement("nghiaVuTaiChinhId")]
    public string? NghiaVuTaiChinhId { get; set; }

    /// <summary>
    /// Loại chế độ miễn giảm ID
    /// </summary>
    [XmlElement("loaiCheDoMienGiamId")]
    public string? LoaiCheDoMienGiamId { get; set; }

    /// <summary>
    /// Số tiền miễn giảm
    /// </summary>
    [XmlElement("soTienMienGiam")]
    public decimal? SoTienMienGiam { get; set; }

    /// <summary>
    /// Số quyết định miễn giảm
    /// </summary>
    [XmlElement("soQuyetDinhMienGiam")]
    public string? SoQuyetDinhMienGiam { get; set; }

    /// <summary>
    /// Ngày ra quyết định miễn giảm
    /// </summary>
    [XmlElement("ngayRaQuyetDinhMienGiam")]
    public DateTime? NgayRaQuyetDinhMienGiam { get; set; }

    /// <summary>
    /// Cơ quan ra quyền định miễn giảm
    /// </summary>
    [XmlElement("coQuanRaQuyenDinhMienGiam")]
    public string? CoQuanRaQuyenDinhMienGiam { get; set; }

    /// <summary>
    /// Trang
    /// </summary>
    [XmlAttribute("Trang")]
    public string? Trang { get; set; }
}

/// <summary>
/// Wrapper cho thông tin nợ nghĩa vụ tài chính
/// </summary>
public class ThongTinNoNghiaVuTaiChinh
{
    /// <summary>
    /// Thông tin chi tiết nợ nghĩa vụ tài chính
    /// </summary>
    [XmlElement("DC_NoNghiaVuTaiChinh")]
    public DcNoNghiaVuTaiChinh? NoNghiaVuTaiChinh { get; set; }
}

/// <summary>
/// Thông tin nợ nghĩa vụ tài chính - Trang 53
/// </summary>
public class DcNoNghiaVuTaiChinh
{
    /// <summary>
    /// Mã định danh nợ nghĩa vụ tài chính
    /// </summary>
    [XmlElement("noNghiaVuTaiChinhId")]
    public string? NoNghiaVuTaiChinhId { get; set; }

    /// <summary>
    /// Mã định danh nghĩa vụ tài chính
    /// </summary>
    [XmlElement("nghiaVuTaiChinhId")]
    public string? NghiaVuTaiChinhId { get; set; }

    /// <summary>
    /// Loại chế độ nợ ID
    /// </summary>
    [XmlElement("loaiCheDoNoId")]
    public string? LoaiCheDoNoId { get; set; }

    /// <summary>
    /// Số tiền nợ
    /// </summary>
    [XmlElement("soTienNo")]
    public decimal? SoTienNo { get; set; }

    /// <summary>
    /// Số quyết định nợ
    /// </summary>
    [XmlElement("soQuyetDinhNo")]
    public string? SoQuyetDinhNo { get; set; }

    /// <summary>
    /// Ngày ra quyết định nợ
    /// </summary>
    [XmlElement("ngayRaQuyetDinhNo")]
    public DateTime? NgayRaQuyetDinhNo { get; set; }

    /// <summary>
    /// Cơ quan ra quyền định nợ
    /// </summary>
    [XmlElement("coQuanRaQuyenDinhNo")]
    public string? CoQuanRaQuyenDinhNo { get; set; }

    /// <summary>
    /// Trang
    /// </summary>
    [XmlAttribute("Trang")]
    public string? Trang { get; set; }
}
