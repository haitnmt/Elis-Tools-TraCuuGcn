using System.Xml.Serialization;

namespace Haihv.Elis.Tool.VbdLis.Models;

/// <summary>
/// Wrapper cho thông tin hạn chế quyền
/// </summary>
public class ThongTinHanCheQuyen
{
    /// <summary>
    /// Thông tin chi tiết hạn chế quyền
    /// </summary>
    [XmlElement("DC_HanCheQuyen")]
    public DcHanCheQuyen? HanCheQuyen { get; set; }
}

/// <summary>
/// Thông tin hạn chế quyền - Trang 55
/// </summary>
public class DcHanCheQuyen
{
    /// <summary>
    /// Mã định danh hạn chế quyền
    /// </summary>
    [XmlElement("hanCheQuyenId")]
    public string? HanCheQuyenId { get; set; }

    /// <summary>
    /// Loại hạn chế
    /// </summary>
    [XmlElement("loaiHanChe")]
    public string? LoaiHanChe { get; set; }

    /// <summary>
    /// Diện tích
    /// </summary>
    [XmlElement("dienTich")]
    public decimal? DienTich { get; set; }

    /// <summary>
    /// Nội dung hạn chế
    /// </summary>
    [XmlElement("noiDungHanChe")]
    public string? NoiDungHanChe { get; set; }

    /// <summary>
    /// Hạn chế một phần
    /// </summary>
    [XmlElement("hanCheMotPhan")]
    public bool? HanCheMotPhan { get; set; }

    /// <summary>
    /// Sơ đồ ranh giới hạn chế
    /// </summary>
    [XmlElement("soDoRanhGioiHanChe")]
    public string? SoDoRanhGioiHanChe { get; set; }

    /// <summary>
    /// Loại văn bản hạn chế
    /// </summary>
    [XmlElement("loaiVanBanHanChe")]
    public string? LoaiVanBanHanChe { get; set; }

    /// <summary>
    /// Số văn bản
    /// </summary>
    [XmlElement("soVanBan")]
    public string? SoVanBan { get; set; }

    /// <summary>
    /// Ngày ban hành
    /// </summary>
    [XmlElement("ngayBanHanh")]
    public DateTime? NgayBanHanh { get; set; }

    /// <summary>
    /// Cơ quan ban hành
    /// </summary>
    [XmlElement("coQuanBanHanh")]
    public string? CoQuanBanHanh { get; set; }

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
