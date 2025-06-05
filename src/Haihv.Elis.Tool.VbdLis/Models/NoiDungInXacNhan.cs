using System.Xml.Serialization;

namespace Haihv.Elis.Tool.VbdLis.Models;

/// <summary>
/// Wrapper cho thông tin nội dung in xác nhận
/// </summary>
public class ThongTinNoiDungInXacNhan
{
    /// <summary>
    /// Thông tin chi tiết nội dung in xác nhận
    /// </summary>
    [XmlElement("DC_NoiDungInXacNhan")]
    public DcNoiDungInXacNhan? NoiDungInXacNhan { get; set; }
}

/// <summary>
/// Thông tin nội dung in xác nhận
/// </summary>
public class DcNoiDungInXacNhan
{
    /// <summary>
    /// Mã định danh nội dung in xác nhận
    /// </summary>
    [XmlElement("noiDungInXacNhanId")]
    public string? NoiDungInXacNhanId { get; set; }

    /// <summary>
    /// Mã định danh giấy chứng nhận
    /// </summary>
    [XmlElement("giayChungNhanId")]
    public string? GiayChungNhanId { get; set; }

    /// <summary>
    /// Mã định danh giao dịch
    /// </summary>
    [XmlElement("giaoDichId")]
    public string? GiaoDichId { get; set; }

    /// <summary>
    /// Nội dung thay đổi
    /// </summary>
    [XmlElement("noiDungThayDoi")]
    public string? NoiDungThayDoi { get; set; }

    /// <summary>
    /// Cơ quan xác nhận
    /// </summary>
    [XmlElement("coQuanXacNhan")]
    public string? CoQuanXacNhan { get; set; }

    /// <summary>
    /// Ngày xác nhận
    /// </summary>
    [XmlElement("ngayXacNhan")]
    public DateTime? NgayXacNhan { get; set; }
}
