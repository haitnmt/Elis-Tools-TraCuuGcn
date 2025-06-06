using System.Xml.Serialization;

namespace Haihv.Elis.Tool.VbdLis.Models;

/// <summary>
/// Wrapper cho thông tin địa chỉ
/// </summary>
public class ThongTinDiaChi
{
    /// <summary>
    /// Thông tin chi tiết địa chỉ
    /// </summary>
    [XmlElement("DC_DiaChi")]
    public DcDiaChi? DiaChi { get; set; }
}

/// <summary>
/// Thông tin địa chỉ - Trang 42
/// </summary>
public class DcDiaChi
{
    /// <summary>
    /// Mã định danh địa chỉ
    /// </summary>
    [XmlElement("diaChiId")]
    public string? DiaChiId { get; set; }

    /// <summary>
    /// Mã xã
    /// </summary>
    [XmlElement("maXa")]
    public string? MaXa { get; set; }

    /// <summary>
    /// Số nhà
    /// </summary>
    [XmlElement("soNha")]
    public string? SoNha { get; set; }

    /// <summary>
    /// Tên đường phố
    /// </summary>
    [XmlElement("tenDuongPho")]
    public string? TenDuongPho { get; set; }

    /// <summary>
    /// Tên tổ dân phố
    /// </summary>
    [XmlElement("tenToDanPho")]
    public string? TenToDanPho { get; set; }

    /// <summary>
    /// Tên xã
    /// </summary>
    [XmlElement("tenXa")]
    public string? TenXa { get; set; }

    /// <summary>
    /// Tên quận
    /// </summary>
    [XmlElement("tenQuan")]
    public string? TenQuan { get; set; }

    /// <summary>
    /// Tên tỉnh
    /// </summary>
    [XmlElement("tenTinh")]
    public string? TenTinh { get; set; }    /// <summary>
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