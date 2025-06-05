using System.Xml.Serialization;

namespace Haihv.Elis.Tool.VbdLis;

/// <summary>
/// Hồ sơ XML chính chứa tất cả thông tin về giấy chứng nhận quyền sử dụng đất
/// </summary>
[XmlRoot("HoSoXml")]
public class HoSoXml
{
    /// <summary>
    /// Mã định danh duy nhất của hồ sơ
    /// </summary>
    [XmlElement("hoSoGuid")]
    public string? HoSoGuid { get; set; }

    /// <summary>
    /// Thông tin tình hình đăng ký
    /// </summary>
    [XmlElement("DC_TinhHinhDangKy")]
    public DcTinhHinhDangKy? TinhHinhDangKy { get; set; }

    /// <summary>
    /// Thông tin giấy chứng nhận
    /// </summary>
    [XmlElement("ThongTinGiayChungNhan")]
    public ThongTinGiayChungNhan? ThongTinGiayChungNhan { get; set; }

    /// <summary>
    /// Thông tin đăng ký sở hữu chung riêng
    /// </summary>
    [XmlElement("ThongTinDangKySoHuuChungRieng")]
    public ThongTinDangKySoHuuChungRieng? ThongTinDangKySoHuuChungRieng { get; set; }

    /// <summary>
    /// Thông tin quyền sử dụng đất
    /// </summary>
    [XmlElement("ThongTinQuyenSuDungDat")]
    public ThongTinQuyenSuDungDat? ThongTinQuyenSuDungDat { get; set; }

    /// <summary>
    /// Thông tin quyền quản lý đất
    /// </summary>
    [XmlElement("ThongTinQuyenQuanLyDat")]
    public ThongTinQuyenQuanLyDat? ThongTinQuyenQuanLyDat { get; set; }

    /// <summary>
    /// Thông tin quyền sở hữu tài sản gắn liền với đất
    /// </summary>
    [XmlElement("ThongTinQuyenSoHuuTaiSanGanLienVoiDat")]
    public ThongTinQuyenSoHuuTaiSanGanLienVoiDat? ThongTinQuyenSoHuuTaiSanGanLienVoiDat { get; set; }

    /// <summary>
    /// Thông tin giao dịch
    /// </summary>
    [XmlElement("ThongTinGiaoDich")]
    public ThongTinGiaoDich? ThongTinGiaoDich { get; set; }

    /// <summary>
    /// Thông tin giao dịch bảo đảm
    /// </summary>
    [XmlElement("ThongTinGiaoDichBaoDam")]
    public ThongTinGiaoDichBaoDam? ThongTinGiaoDichBaoDam { get; set; }

    /// <summary>
    /// Thông tin tài sản
    /// </summary>
    [XmlElement("ThongTinTaiSan")]
    public ThongTinTaiSan? ThongTinTaiSan { get; set; }

    /// <summary>
    /// Thông tin chủ sở hữu
    /// </summary>
    [XmlElement("ThongTinChu")]
    public ThongTinChu? ThongTinChu { get; set; }

    /// <summary>
    /// Thông tin địa chỉ
    /// </summary>
    [XmlElement("ThongTinDiaChi")]
    public ThongTinDiaChi? ThongTinDiaChi { get; set; }

    /// <summary>
    /// Thông tin tài liệu đo đạc
    /// </summary>
    [XmlElement("ThongTinTaiLieuDoDac")]
    public ThongTinTaiLieuDoDac? ThongTinTaiLieuDoDac { get; set; }

    /// <summary>
    /// Thông tin nghĩa vụ tài chính
    /// </summary>
    [XmlElement("ThongTinNghiaVuTaiChinh")]
    public ThongTinNghiaVuTaiChinh? ThongTinNghiaVuTaiChinh { get; set; }

    /// <summary>
    /// Thông tin miễn giảm nghĩa vụ tài chính
    /// </summary>
    [XmlElement("ThongTinMienGiamNghiaVuTaiChinh")]
    public ThongTinMienGiamNghiaVuTaiChinh? ThongTinMienGiamNghiaVuTaiChinh { get; set; }

    /// <summary>
    /// Thông tin nợ nghĩa vụ tài chính
    /// </summary>
    [XmlElement("ThongTinNoNghiaVuTaiChinh")]
    public ThongTinNoNghiaVuTaiChinh? ThongTinNoNghiaVuTaiChinh { get; set; }

    /// <summary>
    /// Thông tin hạn chế quyền
    /// </summary>
    [XmlElement("ThongTinHanCheQuyen")]
    public ThongTinHanCheQuyen? ThongTinHanCheQuyen { get; set; }

    /// <summary>
    /// Thông tin hồ sơ quét
    /// </summary>
    [XmlElement("ThongTinHoSoQuet")]
    public ThongTinHoSoQuet? ThongTinHoSoQuet { get; set; }

    /// <summary>
    /// Thông tin sổ địa chính
    /// </summary>
    [XmlElement("ThongTinSoDiaChinh")]
    public ThongTinSoDiaChinh? ThongTinSoDiaChinh { get; set; }

    /// <summary>
    /// Thông tin đăng ký thay đổi
    /// </summary>
    [XmlElement("ThongTinDangKyThayDoi")]
    public ThongTinDangKyThayDoi? ThongTinDangKyThayDoi { get; set; }

    /// <summary>
    /// Thông tin nội dung in xác nhận
    /// </summary>
    [XmlElement("ThongTinNoiDungInXacNhan")]
    public ThongTinNoiDungInXacNhan? ThongTinNoiDungInXacNhan { get; set; }
}