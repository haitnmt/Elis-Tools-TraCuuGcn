using System.Xml.Serialization;

namespace Haihv.Elis.Tool.VbdLis.Models;

/// <summary>
/// Wrapper cho thông tin chủ sở hữu
/// </summary>
public class ThongTinChu
{
    /// <summary>
    /// Thông tin cá nhân
    /// </summary>
    [XmlElement("ThongTinCaNhan")]
    public ThongTinCaNhan? ThongTinCaNhan { get; set; }

    /// <summary>
    /// Thông tin vợ chồng
    /// </summary>
    [XmlElement("ThongTinVoChong")]
    public ThongTinVoChong? ThongTinVoChong { get; set; }

    /// <summary>
    /// Thông tin hộ gia đình
    /// </summary>
    [XmlElement("ThongTinHoGiaDinh")]
    public ThongTinHoGiaDinh? ThongTinHoGiaDinh { get; set; }

    /// <summary>
    /// Thông tin tổ chức
    /// </summary>
    [XmlElement("ThongTinToChuc")]
    public ThongTinToChuc? ThongTinToChuc { get; set; }

    /// <summary>
    /// Thông tin cộng đồng
    /// </summary>
    [XmlElement("ThongTinCongDong")]
    public ThongTinCongDong? ThongTinCongDong { get; set; }
}

/// <summary>
/// Wrapper cho thông tin cá nhân
/// </summary>
public class ThongTinCaNhan
{
    /// <summary>
    /// Thông tin chi tiết cá nhân
    /// </summary>
    [XmlElement("DC_CaNhan")]
    public DcCaNhan? CaNhan { get; set; }
}

/// <summary>
/// Thông tin cá nhân - Trang 39
/// </summary>
public class DcCaNhan
{
    /// <summary>
    /// Mã định danh cá nhân
    /// </summary>
    [XmlElement("caNhanId")]
    public string? CaNhanId { get; set; }

    /// <summary>
    /// Loại đối tượng sử dụng đất
    /// </summary>
    [XmlElement("loaiDoiTuongSuDungDat")]
    public string? LoaiDoiTuongSuDungDat { get; set; }

    /// <summary>
    /// Họ tên
    /// </summary>
    [XmlElement("hoTen")]
    public string? HoTen { get; set; }

    /// <summary>
    /// Ngày sinh
    /// </summary>
    [XmlElement("ngaySinh")]
    public DateTime? NgaySinh { get; set; }

    /// <summary>
    /// Năm sinh
    /// </summary>
    [XmlElement("namSinh")]
    public int? NamSinh { get; set; }

    /// <summary>
    /// Năm mất
    /// </summary>
    [XmlElement("namMat")]
    public int? NamMat { get; set; }

    /// <summary>
    /// Giới tính
    /// </summary>
    [XmlElement("gioiTinh")]
    public string? GioiTinh { get; set; }

    /// <summary>
    /// Mã số thuế
    /// </summary>
    [XmlElement("maSoThue")]
    public string? MaSoThue { get; set; }

    /// <summary>
    /// Số điện thoại
    /// </summary>
    [XmlElement("soDienThoai")]
    public string? SoDienThoai { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    [XmlElement("email")]
    public string? Email { get; set; }

    /// <summary>
    /// Mã số định danh
    /// </summary>
    [XmlElement("maSoDinhDanh")]
    public string? MaSoDinhDanh { get; set; }

    /// <summary>
    /// Mã định danh quốc tịch
    /// </summary>
    [XmlElement("quocTichId")]
    public string? QuocTichId { get; set; }

    /// <summary>
    /// Mã định danh dân tộc
    /// </summary>
    [XmlElement("danTocId")]
    public string? DanTocId { get; set; }

    /// <summary>
    /// Địa chỉ
    /// </summary>
    [XmlElement("diaChi")]
    public string? DiaChi { get; set; }

    /// <summary>
    /// Thông tin địa chỉ chuẩn
    /// </summary>
    [XmlElement("ThongTinDiaChiChuan")]
    public ThongTinDiaChiChuan? ThongTinDiaChiChuan { get; set; }

    /// <summary>
    /// Thông tin giấy tờ tùy thân
    /// </summary>
    [XmlElement("ThongTinGiayToTuyThan")]
    public ThongTinGiayToTuyThan? ThongTinGiayToTuyThan { get; set; }

    /// <summary>
    /// Trang
    /// </summary>
    [XmlAttribute("Trang")]
    public string? Trang { get; set; }
}

/// <summary>
/// Thông tin giấy tờ tùy thân
/// </summary>
public class ThongTinGiayToTuyThan
{
    /// <summary>
    /// Giấy tờ tùy thân
    /// </summary>
    [XmlElement("DC_GiayToTuyThan")]
    public DcGiayToTuyThan? GiayToTuyThan { get; set; }
}

/// <summary>
/// Giấy tờ tùy thân - Trang 42
/// </summary>
public class DcGiayToTuyThan
{
    /// <summary>
    /// Mã định danh giấy tờ tùy thân
    /// </summary>
    [XmlElement("giayToTuyThanId")]
    public string? GiayToTuyThanId { get; set; }

    /// <summary>
    /// Loại giấy tờ tùy thân
    /// </summary>
    [XmlElement("loaiGiayToTuyThan")]
    public string? LoaiGiayToTuyThan { get; set; }

    /// <summary>
    /// Số giấy tờ
    /// </summary>
    [XmlElement("soGiayTo")]
    public string? SoGiayTo { get; set; }

    /// <summary>
    /// Ngày cấp
    /// </summary>
    [XmlElement("ngayCap")]
    public DateTime? NgayCap { get; set; }

    /// <summary>
    /// Nơi cấp
    /// </summary>
    [XmlElement("noiCap")]
    public string? NoiCap { get; set; }

    /// <summary>
    /// Trang
    /// </summary>
    [XmlAttribute("Trang")]
    public string? Trang { get; set; }
}

/// <summary>
/// Wrapper cho thông tin vợ chồng
/// </summary>
public class ThongTinVoChong
{
    /// <summary>
    /// Thông tin chi tiết vợ chồng
    /// </summary>
    [XmlElement("DC_VoChong")]
    public DcVoChong? VoChong { get; set; }
}

/// <summary>
/// Thông tin vợ chồng - Trang 40
/// </summary>
public class DcVoChong
{
    /// <summary>
    /// Mã định danh vợ chồng
    /// </summary>
    [XmlElement("voChongId")]
    public string? VoChongId { get; set; }

    /// <summary>
    /// Mã định danh vợ
    /// </summary>
    [XmlElement("voId")]
    public string? VoId { get; set; }

    /// <summary>
    /// Mã định danh chồng
    /// </summary>
    [XmlElement("chongId")]
    public string? ChongId { get; set; }

    /// <summary>
    /// In vợ trước
    /// </summary>
    [XmlElement("inVoTruoc")]
    public bool? InVoTruoc { get; set; }

    /// <summary>
    /// Trang
    /// </summary>
    [XmlAttribute("Trang")]
    public string? Trang { get; set; }
}

/// <summary>
/// Wrapper cho thông tin hộ gia đình
/// </summary>
public class ThongTinHoGiaDinh
{
    /// <summary>
    /// Thông tin chi tiết hộ gia đình
    /// </summary>
    [XmlElement("DC_HoGiaDinh")]
    public DcHoGiaDinh? HoGiaDinh { get; set; }
}

/// <summary>
/// Thông tin hộ gia đình - Trang 40
/// </summary>
public class DcHoGiaDinh
{
    /// <summary>
    /// Mã định danh hộ gia đình
    /// </summary>
    [XmlElement("hoGiaDinhId")]
    public string? HoGiaDinhId { get; set; }

    /// <summary>
    /// Mã định danh chủ hộ
    /// </summary>
    [XmlElement("chuHoId")]
    public string? ChuHoId { get; set; }

    /// <summary>
    /// Mã định danh vợ chồng chủ hộ
    /// </summary>
    [XmlElement("voChongChuHoId")]
    public string? VoChongChuHoId { get; set; }

    /// <summary>
    /// Địa chỉ
    /// </summary>
    [XmlElement("diaChi")]
    public string? DiaChi { get; set; }

    /// <summary>
    /// Thông tin địa chỉ chuẩn
    /// </summary>
    [XmlElement("ThongTinDiaChiChuan")]
    public ThongTinDiaChiChuan? ThongTinDiaChiChuan { get; set; }

    /// <summary>
    /// Thông tin thành viên hộ gia đình
    /// </summary>
    [XmlElement("ThongTinThanhVienHoGiaDinh")]
    public ThongTinThanhVienHoGiaDinh? ThongTinThanhVienHoGiaDinh { get; set; }

    /// <summary>
    /// Trang
    /// </summary>
    [XmlAttribute("Trang")]
    public string? Trang { get; set; }
}

/// <summary>
/// Thông tin thành viên hộ gia đình
/// </summary>
public class ThongTinThanhVienHoGiaDinh
{
    /// <summary>
    /// Thành viên hộ gia đình
    /// </summary>
    [XmlElement("ThanhVienHoGiaDinh")]
    public ThanhVienHoGiaDinh? ThanhVienHoGiaDinh { get; set; }
}

/// <summary>
/// Thành viên hộ gia đình
/// </summary>
public class ThanhVienHoGiaDinh
{
    /// <summary>
    /// Mã định danh cá nhân
    /// </summary>
    [XmlElement("caNhanId")]
    public string? CaNhanId { get; set; }

    /// <summary>
    /// Mã định danh vợ chồng
    /// </summary>
    [XmlElement("voChongId")]
    public string? VoChongId { get; set; }
}

/// <summary>
/// Wrapper cho thông tin tổ chức
/// </summary>
public class ThongTinToChuc
{
    /// <summary>
    /// Thông tin chi tiết tổ chức
    /// </summary>
    [XmlElement("DC_ToChuc")]
    public DcToChuc? ToChuc { get; set; }
}

/// <summary>
/// Thông tin tổ chức - Trang 41
/// </summary>
public class DcToChuc
{
    /// <summary>
    /// Mã định danh tổ chức
    /// </summary>
    [XmlElement("toChucId")]
    public string? ToChucId { get; set; }

    /// <summary>
    /// Loại đối tượng sử dụng đất
    /// </summary>
    [XmlElement("loaiDoiTuongSuDungDat")]
    public string? LoaiDoiTuongSuDungDat { get; set; }

    /// <summary>
    /// Tên tổ chức
    /// </summary>
    [XmlElement("tenToChuc")]
    public string? TenToChuc { get; set; }

    /// <summary>
    /// Tên viết tắt
    /// </summary>
    [XmlElement("tenVietTat")]
    public string? TenVietTat { get; set; }

    /// <summary>
    /// Tên tổ chức ta
    /// </summary>
    [XmlElement("tenToChucTa")]
    public string? TenToChucTa { get; set; }

    /// <summary>
    /// Mã định danh người đại diện
    /// </summary>
    [XmlElement("nguoiDaiDienId")]
    public string? NguoiDaiDienId { get; set; }

    /// <summary>
    /// Số quyết định
    /// </summary>
    [XmlElement("soQuyetDinh")]
    public string? SoQuyetDinh { get; set; }

    /// <summary>
    /// Ngày quyết định
    /// </summary>
    [XmlElement("ngayQuyetDinh")]
    public DateTime? NgayQuyetDinh { get; set; }

    /// <summary>
    /// Loại quyết định thành lập
    /// </summary>
    [XmlElement("loaiQuyetDinhThanhLap")]
    public string? LoaiQuyetDinhThanhLap { get; set; }

    /// <summary>
    /// Mã doanh nghiệp
    /// </summary>
    [XmlElement("maDoanhNghiep")]
    public string? MaDoanhNghiep { get; set; }

    /// <summary>
    /// Mã số thuế
    /// </summary>
    [XmlElement("maSoThue")]
    public string? MaSoThue { get; set; }

    /// <summary>
    /// Loại tổ chức
    /// </summary>
    [XmlElement("loaiToChuc")]
    public string? LoaiToChuc { get; set; }

    /// <summary>
    /// Địa chỉ
    /// </summary>
    [XmlElement("diaChi")]
    public string? DiaChi { get; set; }

    /// <summary>
    /// Thông tin địa chỉ chuẩn
    /// </summary>
    [XmlElement("ThongTinDiaChiChuan")]
    public ThongTinDiaChiChuan? ThongTinDiaChiChuan { get; set; }

    /// <summary>
    /// Thông tin giấy tờ tổ chức
    /// </summary>
    [XmlElement("ThongTinGiayToToChuc")]
    public ThongTinGiayToToChuc? ThongTinGiayToToChuc { get; set; }

    /// <summary>
    /// Trang
    /// </summary>
    [XmlAttribute("Trang")]
    public string? Trang { get; set; }
}

/// <summary>
/// Thông tin giấy tờ tổ chức
/// </summary>
public class ThongTinGiayToToChuc
{
    /// <summary>
    /// Giấy tờ tổ chức
    /// </summary>
    [XmlElement("DC_GiayToToChuc")]
    public DcGiayToToChuc? GiayToToChuc { get; set; }
}

/// <summary>
/// Giấy tờ tổ chức
/// </summary>
public class DcGiayToToChuc
{
    /// <summary>
    /// Mã định danh giấy tờ tổ chức
    /// </summary>
    [XmlElement("giayToToChucId")]
    public string? GiayToToChucId { get; set; }

    /// <summary>
    /// Loại giấy tờ tổ chức
    /// </summary>
    [XmlElement("loaiGiayToToChuc")]
    public string? LoaiGiayToToChuc { get; set; }

    /// <summary>
    /// Số giấy tờ
    /// </summary>
    [XmlElement("soGiayTo")]
    public string? SoGiayTo { get; set; }

    /// <summary>
    /// Ngày cấp
    /// </summary>
    [XmlElement("ngayCap")]
    public DateTime? NgayCap { get; set; }

    /// <summary>
    /// Nơi cấp
    /// </summary>
    [XmlElement("noiCap")]
    public string? NoiCap { get; set; }

    /// <summary>
    /// Ghi chú
    /// </summary>
    [XmlElement("ghiChu")]
    public string? GhiChu { get; set; }
}

/// <summary>
/// Wrapper cho thông tin cộng đồng
/// </summary>
public class ThongTinCongDong
{
    /// <summary>
    /// Thông tin chi tiết cộng đồng
    /// </summary>
    [XmlElement("DC_CongDong")]
    public DcCongDong? CongDong { get; set; }
}

/// <summary>
/// Thông tin cộng đồng - Trang 41
/// </summary>
public class DcCongDong
{
    /// <summary>
    /// Mã định danh cộng đồng
    /// </summary>
    [XmlElement("congDongId")]
    public string? CongDongId { get; set; }

    /// <summary>
    /// Loại đối tượng sử dụng đất
    /// </summary>
    [XmlElement("loaiDoiTuongSuDungDat")]
    public string? LoaiDoiTuongSuDungDat { get; set; }

    /// <summary>
    /// Tên cộng đồng
    /// </summary>
    [XmlElement("tenCongDong")]
    public string? TenCongDong { get; set; }

    /// <summary>
    /// Mã định danh người đại diện
    /// </summary>
    [XmlElement("nguoiDaiDienId")]
    public string? NguoiDaiDienId { get; set; }

    /// <summary>
    /// Địa danh cư trú
    /// </summary>
    [XmlElement("diaDanhCuTru")]
    public string? DiaDanhCuTru { get; set; }

    /// <summary>
    /// Thông tin địa chỉ chuẩn
    /// </summary>
    [XmlElement("ThongTinDiaChiChuan")]
    public ThongTinDiaChiChuan? ThongTinDiaChiChuan { get; set; }

    /// <summary>
    /// Trang
    /// </summary>
    [XmlAttribute("Trang")]
    public string? Trang { get; set; }
}