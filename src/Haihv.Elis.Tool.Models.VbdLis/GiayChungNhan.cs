using System.Xml.Serialization;

namespace Haihv.Elis.Tool.Models.VbdLis;

/// <summary>
/// Wrapper cho thông tin giấy chứng nhận
/// </summary>
public class ThongTinGiayChungNhan
{
    /// <summary>
    /// Thông tin chi tiết giấy chứng nhận
    /// </summary>
    [XmlElement("DC_GiayChungNhan")]
    public DcGiayChungNhan? GiayChungNhan { get; set; }
}

/// <summary>
/// Thông tin giấy chứng nhận - Trang 56
/// </summary>
public class DcGiayChungNhan
{
    /// <summary>
    /// Mã định danh giấy chứng nhận
    /// </summary>
    [XmlElement("giayChungNhanId")]
    public string? GiayChungNhanId { get; set; }

    /// <summary>
    /// Loại giấy chứng nhận
    /// </summary>
    [XmlElement("loaiGiayChungNhan")]
    public string? LoaiGiayChungNhan { get; set; }

    /// <summary>
    /// Số hồ sơ gốc
    /// </summary>
    [XmlElement("soHoSoGoc")]
    public string? SoHoSoGoc { get; set; }

    /// <summary>
    /// Số hồ sơ gốc cũ
    /// </summary>
    [XmlElement("soHoSoGocCu")]
    public string? SoHoSoGocCu { get; set; }

    /// <summary>
    /// Số vào sổ
    /// </summary>
    [XmlElement("soVaoSo")]
    public string? SoVaoSo { get; set; }

    /// <summary>
    /// Số vào sổ cũ
    /// </summary>
    [XmlElement("soVaoSoCu")]
    public string? SoVaoSoCu { get; set; }

    /// <summary>
    /// Số phát hành
    /// </summary>
    [XmlElement("soPhatHanh")]
    public string? SoPhatHanh { get; set; }

    /// <summary>
    /// Mã vạch
    /// </summary>
    [XmlElement("maVach")]
    public string? MaVach { get; set; }

    /// <summary>
    /// Tên người ký
    /// </summary>
    [XmlElement("tenNguoiKy")]
    public string? TenNguoiKy { get; set; }

    /// <summary>
    /// Đã công nhận pháp lý
    /// </summary>
    [XmlElement("daCongNhanPhapLy")]
    public bool? DaCongNhanPhapLy { get; set; }

    /// <summary>
    /// Đơn vị cấp
    /// </summary>
    [XmlElement("donViCap")]
    public string? DonViCap { get; set; }

    /// <summary>
    /// Ghi chú trang 1
    /// </summary>
    [XmlElement("ghiChuTrang1")]
    public string? GhiChuTrang1 { get; set; }

    /// <summary>
    /// Ghi chú trang 2
    /// </summary>
    [XmlElement("ghiChuTrang2")]
    public string? GhiChuTrang2 { get; set; }

    /// <summary>
    /// Ngày cấp
    /// </summary>
    [XmlElement("ngayCap")]
    public DateTime? NgayCap { get; set; }

    /// <summary>
    /// Bản quét
    /// </summary>
    [XmlElement("banQuet")]
    public string? BanQuet { get; set; }

    /// <summary>
    /// Trang
    /// </summary>
    [XmlAttribute("Trang")]
    public string? Trang { get; set; }
}
