using System.Xml.Serialization;

namespace Haihv.Elis.Tool.VbdLis;

/// <summary>
/// Wrapper cho thông tin hồ sơ quét
/// </summary>
public class ThongTinHoSoQuet
{
    /// <summary>
    /// Thông tin chi tiết hồ sơ quét
    /// </summary>
    [XmlElement("DC_HoSoQuet")]
    public DcHoSoQuet? HoSoQuet { get; set; }
}

/// <summary>
/// Thông tin hồ sơ quét
/// </summary>
public class DcHoSoQuet
{
    /// <summary>
    /// Mã định danh hồ sơ quét
    /// </summary>
    [XmlElement("hoSoQuetId")]
    public string? HoSoQuetId { get; set; }

    /// <summary>
    /// Kho
    /// </summary>
    [XmlElement("kho")]
    public string? Kho { get; set; }

    /// <summary>
    /// Dãy
    /// </summary>
    [XmlElement("day")]
    public string? Day { get; set; }

    /// <summary>
    /// Kệ
    /// </summary>
    [XmlElement("ke")]
    public string? Ke { get; set; }

    /// <summary>
    /// Hộp
    /// </summary>
    [XmlElement("hop")]
    public string? Hop { get; set; }

    /// <summary>
    /// Số thứ tự hồ sơ
    /// </summary>
    [XmlElement("soThuTuHoSo")]
    public int? SoThuTuHoSo { get; set; }

    /// <summary>
    /// Thông tin chi tiết hồ sơ quét
    /// </summary>
    [XmlElement("ThongTinChiTietHoSoQuet")]
    public ThongTinChiTietHoSoQuet? ThongTinChiTietHoSoQuet { get; set; }
}

/// <summary>
/// Wrapper cho thông tin chi tiết hồ sơ quét
/// </summary>
public class ThongTinChiTietHoSoQuet
{
    /// <summary>
    /// Chi tiết hồ sơ quét
    /// </summary>
    [XmlElement("DC_ChiTietHoSoQuet")]
    public DcChiTietHoSoQuet? ChiTietHoSoQuet { get; set; }
}

/// <summary>
/// Chi tiết hồ sơ quét
/// </summary>
public class DcChiTietHoSoQuet
{
    /// <summary>
    /// Mã định danh chi tiết hồ sơ quét
    /// </summary>
    [XmlElement("chiTietHoSoQuetId")]
    public string? ChiTietHoSoQuetId { get; set; }

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

    /// <summary>
    /// Là giấy tờ về nguồn gốc
    /// </summary>
    [XmlElement("laGiayToVeNguonGoc")]
    public bool? LaGiayToVeNguonGoc { get; set; }

    /// <summary>
    /// Là giấy chứng nhận
    /// </summary>
    [XmlElement("laGiayChungNhan")]
    public bool? LaGiayChungNhan { get; set; }

    /// <summary>
    /// Mã định danh giấy chứng nhận
    /// </summary>
    [XmlElement("giayChungNhanId")]
    public string? GiayChungNhanId { get; set; }
}