using System.Xml.Serialization;

namespace Haihv.Elis.Tool.VbdLis.Models;

/// <summary>
/// Wrapper cho thông tin sổ địa chính
/// </summary>
public class ThongTinSoDiaChinh
{
    /// <summary>
    /// Thông tin chi tiết sổ địa chính
    /// </summary>
    [XmlElement("DC_SoDiaChinh")]
    public DcSoDiaChinh? SoDiaChinh { get; set; }
}

/// <summary>
/// Thông tin sổ địa chính
/// </summary>
public class DcSoDiaChinh
{
    /// <summary>
    /// Mã định danh sổ địa chính
    /// </summary>
    [XmlElement("soDiaChinhId")]
    public string? SoDiaChinhId { get; set; }

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
    /// Thời điểm lập sổ
    /// </summary>
    [XmlElement("thoiDiemLapSo")]
    public DateTime? ThoiDiemLapSo { get; set; }

    /// <summary>
    /// Bản quét
    /// </summary>
    [XmlElement("banQuet")]
    public string? BanQuet { get; set; }

    /// <summary>
    /// Tên file
    /// </summary>
    [XmlElement("tenFile")]
    public string? TenFile { get; set; }

    /// <summary>
    /// Mô tả
    /// </summary>
    [XmlElement("moTa")]
    public string? MoTa { get; set; }
}
