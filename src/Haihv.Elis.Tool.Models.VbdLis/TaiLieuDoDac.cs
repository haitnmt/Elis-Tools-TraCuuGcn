using System.Xml.Serialization;

namespace Haihv.Elis.Tool.Models.VbdLis;

/// <summary>
/// Wrapper cho thông tin tài liệu đo đạc
/// </summary>
public class ThongTinTaiLieuDoDac
{
    /// <summary>
    /// Thông tin chi tiết tài liệu đo đạc
    /// </summary>
    [XmlElement("DC_TaiLieuDoDac")]
    public DcTaiLieuDoDac? TaiLieuDoDac { get; set; }
}

/// <summary>
/// Thông tin tài liệu đo đạc - Trang 39
/// </summary>
public class DcTaiLieuDoDac
{
    /// <summary>
    /// Mã định danh tài liệu đo đạc
    /// </summary>
    [XmlElement("taiLieuDoDacId")]
    public string? TaiLieuDoDacId { get; set; }

    /// <summary>
    /// Mã xã
    /// </summary>
    [XmlElement("maXa")]
    public string? MaXa { get; set; }

    /// <summary>
    /// Loại bản đồ địa chính
    /// </summary>
    [XmlElement("loaiBanDoDiaChinh")]
    public string? LoaiBanDoDiaChinh { get; set; }

    /// <summary>
    /// Đơn vị đo đạc
    /// </summary>
    [XmlElement("donViDoDac")]
    public string? DonViDoDac { get; set; }

    /// <summary>
    /// Phương pháp đo
    /// </summary>
    [XmlElement("phuongPhapDo")]
    public string? PhuongPhapDo { get; set; }

    /// <summary>
    /// Mức độ chính xác
    /// </summary>
    [XmlElement("mucDoChinhXac")]
    public string? MucDoChinhXac { get; set; }

    /// <summary>
    /// Tỷ lệ đo đạc
    /// </summary>
    [XmlElement("tyLeDoDac")]
    public string? TyLeDoDac { get; set; }

    /// <summary>
    /// Ngày hoàn thành
    /// </summary>
    [XmlElement("ngayHoanThanh")]
    public DateTime? NgayHoanThanh { get; set; }

    /// <summary>
    /// Trang
    /// </summary>
    [XmlAttribute("Trang")]
    public string? Trang { get; set; }
}
