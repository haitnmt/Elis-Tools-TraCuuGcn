using System.Xml.Serialization;

namespace Haihv.Elis.Tool.Models.VbdLis;

/// <summary>
/// Wrapper cho thông tin đăng ký thay đổi
/// </summary>
public class ThongTinDangKyThayDoi
{
    /// <summary>
    /// Thông tin chi tiết đăng ký thay đổi
    /// </summary>
    [XmlElement("DC_DangKyThayDoi")]
    public DcDangKyThayDoi? DangKyThayDoi { get; set; }
}

/// <summary>
/// Thông tin đăng ký thay đổi
/// </summary>
public class DcDangKyThayDoi
{
    /// <summary>
    /// Mã định danh đăng ký thay đổi
    /// </summary>
    [XmlElement("dangKyThayDoiId")]
    public string? DangKyThayDoiId { get; set; }

    /// <summary>
    /// Mã định danh giao dịch
    /// </summary>
    [XmlElement("giaoDichId")]
    public string? GiaoDichId { get; set; }

    /// <summary>
    /// Là thửa đất
    /// </summary>
    [XmlElement("laThuaDat")]
    public bool? LaThuaDat { get; set; }

    /// <summary>
    /// Mã định danh thửa đất
    /// </summary>
    [XmlElement("thuaDatId")]
    public string? ThuaDatId { get; set; }

    /// <summary>
    /// Mã định danh căn hộ
    /// </summary>
    [XmlElement("canHoId")]
    public string? CanHoId { get; set; }

    /// <summary>
    /// Thời điểm đăng ký thay đổi
    /// </summary>
    [XmlElement("thoiDiemDangKyThayDoi")]
    public DateTime? ThoiDiemDangKyThayDoi { get; set; }

    /// <summary>
    /// Nội dung đăng ký thay đổi
    /// </summary>
    [XmlElement("noiDungDangKyThayDoi")]
    public string? NoiDungDangKyThayDoi { get; set; }
}
