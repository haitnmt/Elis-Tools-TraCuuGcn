using System.Xml.Serialization;

namespace Haihv.Elis.Tool.VbdLis;

/// <summary>
/// Thông tin tình hình đăng ký - Trang 57
/// </summary>
public class DcTinhHinhDangKy
{
    /// <summary>
    /// Mã định danh tình hình đăng ký
    /// </summary>
    [XmlElement("tinhHinhDangKyId")]
    public string? TinhHinhDangKyId { get; set; }

    /// <summary>
    /// Mã xã
    /// </summary>
    [XmlElement("maXa")]
    public string? MaXa { get; set; }

    /// <summary>
    /// Mã đơn
    /// </summary>
    [XmlElement("maDon")]
    public string? MaDon { get; set; }

    /// <summary>
    /// Ngày tiếp nhận
    /// </summary>
    [XmlElement("ngayTiepNhan")]
    public DateTime? NgayTiepNhan { get; set; }

    /// <summary>
    /// Thời điểm đăng ký lần đầu
    /// </summary>
    [XmlElement("thoiDiemDangKyLanDau")]
    public DateTime? ThoiDiemDangKyLanDau { get; set; }

    /// <summary>
    /// Thời điểm đăng ký
    /// </summary>
    [XmlElement("thoiDiemDangKy")]
    public DateTime? ThoiDiemDangKy { get; set; }

    /// <summary>
    /// Số thứ tự
    /// </summary>
    [XmlElement("soThuTu")]
    public int? SoThuTu { get; set; }

    /// <summary>
    /// Mã định danh đại diện khai trình
    /// </summary>
    [XmlElement("daiDienKhaiTrinhId")]
    public string? DaiDienKhaiTrinhId { get; set; }

    /// <summary>
    /// Có quyền sử dụng
    /// </summary>
    [XmlElement("coQuyenSuDung")]
    public bool? CoQuyenSuDung { get; set; }

    /// <summary>
    /// Có quyền sở hữu
    /// </summary>
    [XmlElement("coQuyenSoHuu")]
    public bool? CoQuyenSoHuu { get; set; }

    /// <summary>
    /// Có quyền quản lý
    /// </summary>
    [XmlElement("coQuyenQuanLy")]
    public bool? CoQuyenQuanLy { get; set; }

    /// <summary>
    /// Cấp giấy người đại diện
    /// </summary>
    [XmlElement("capGiayNguoiDaiDien")]
    public bool? CapGiayNguoiDaiDien { get; set; }

    /// <summary>
    /// Trang
    /// </summary>
    [XmlAttribute("Trang")]
    public string? Trang { get; set; }
}