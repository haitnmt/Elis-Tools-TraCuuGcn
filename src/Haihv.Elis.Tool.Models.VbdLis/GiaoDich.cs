using System.Xml.Serialization;

namespace Haihv.Elis.Tool.Models.VbdLis;

/// <summary>
/// Wrapper cho thông tin giao dịch
/// </summary>
public class ThongTinGiaoDich
{
    /// <summary>
    /// Thông tin chi tiết giao dịch
    /// </summary>
    [XmlElement("DC_GiaoDich")]
    public DcGiaoDich? GiaoDich { get; set; }
}

/// <summary>
/// Thông tin giao dịch - Trang 57
/// </summary>
public class DcGiaoDich
{
    /// <summary>
    /// Mã định danh giao dịch
    /// </summary>
    [XmlElement("giaoDichId")]
    public string? GiaoDichId { get; set; }

    /// <summary>
    /// Loại giao dịch biến động
    /// </summary>
    [XmlElement("loaiGiaoDichBienDong")]
    public string? LoaiGiaoDichBienDong { get; set; }

    /// <summary>
    /// Thời điểm đăng ký biến động
    /// </summary>
    [XmlElement("thoiDiemDangKyBienDong")]
    public DateTime? ThoiDiemDangKyBienDong { get; set; }

    /// <summary>
    /// Nội dung biến động
    /// </summary>
    [XmlElement("noiDungBienDong")]
    public string? NoiDungBienDong { get; set; }

    /// <summary>
    /// Mã biến động
    /// </summary>
    [XmlElement("maBienDong")]
    public string? MaBienDong { get; set; }

    /// <summary>
    /// Mã hồ sơ thủ tục đăng ký
    /// </summary>
    [XmlElement("maHoSoThuTucDangKy")]
    public string? MaHoSoThuTucDangKy { get; set; }

    /// <summary>
    /// Số hợp đồng
    /// </summary>
    [XmlElement("soHopDong")]
    public string? SoHopDong { get; set; }

    /// <summary>
    /// Ngày hợp đồng
    /// </summary>
    [XmlElement("ngayHopDong")]
    public DateTime? NgayHopDong { get; set; }

    /// <summary>
    /// Giá trị hợp đồng
    /// </summary>
    [XmlElement("giaTriHopDong")]
    public decimal? GiaTriHopDong { get; set; }

    /// <summary>
    /// Nội dung hợp đồng
    /// </summary>
    [XmlElement("noiDungHopDong")]
    public string? NoiDungHopDong { get; set; }

    /// <summary>
    /// Quyền công chứng
    /// </summary>
    [XmlElement("quyenCongChung")]
    public bool? QuyenCongChung { get; set; }

    /// <summary>
    /// Số công chứng
    /// </summary>
    [XmlElement("soCongChung")]
    public string? SoCongChung { get; set; }

    /// <summary>
    /// Ngày công chứng
    /// </summary>
    [XmlElement("ngayCongChung")]
    public DateTime? NgayCongChung { get; set; }

    /// <summary>
    /// Nơi công chứng
    /// </summary>
    [XmlElement("noiCongChung")]
    public string? NoiCongChung { get; set; }

    /// <summary>
    /// Hồ sơ trước biến động
    /// </summary>
    [XmlElement("HoSoTruocBienDong")]
    public HoSoTruocBienDong? HoSoTruocBienDong { get; set; }

    /// <summary>
    /// Trang
    /// </summary>
    [XmlAttribute("Trang")]
    public string? Trang { get; set; }
}

/// <summary>
/// Hồ sơ trước biến động
/// </summary>
public class HoSoTruocBienDong
{
    /// <summary>
    /// Hồ sơ XML trước biến động
    /// </summary>
    [XmlElement("HoSoXml")]
    public HoSoXml? HoSoXml { get; set; }
}

/// <summary>
/// Wrapper cho thông tin giao dịch bảo đảm
/// </summary>
public class ThongTinGiaoDichBaoDam
{
    /// <summary>
    /// Thông tin chi tiết giao dịch bảo đảm
    /// </summary>
    [XmlElement("DC_GiaoDichBaoDam")]
    public DcGiaoDichBaoDam? GiaoDichBaoDam { get; set; }
}

/// <summary>
/// Thông tin giao dịch bảo đảm
/// </summary>
public class DcGiaoDichBaoDam
{
    /// <summary>
    /// Mã định danh giao dịch bảo đảm
    /// </summary>
    [XmlElement("giaoDichBaoDamId")]
    public string? GiaoDichBaoDamId { get; set; }

    /// <summary>
    /// Mã định danh giao dịch
    /// </summary>
    [XmlElement("giaoDichId")]
    public string? GiaoDichId { get; set; }

    /// <summary>
    /// Mã định danh giao dịch bảo đảm cha
    /// </summary>
    [XmlElement("giaoDichBaoDamChaId")]
    public string? GiaoDichBaoDamChaId { get; set; }

    /// <summary>
    /// Loại giao dịch bảo đảm
    /// </summary>
    [XmlElement("loaiGiaoDichBaoDam")]
    public string? LoaiGiaoDichBaoDam { get; set; }

    /// <summary>
    /// Quyền số giao dịch bảo đảm
    /// </summary>
    [XmlElement("quyenSoGiaoDichBaoDam")]
    public string? QuyenSoGiaoDichBaoDam { get; set; }

    /// <summary>
    /// Số thứ tự
    /// </summary>
    [XmlElement("soThuTu")]
    public int? SoThuTu { get; set; }

    /// <summary>
    /// Số đăng ký
    /// </summary>
    [XmlElement("soDangKy")]
    public string? SoDangKy { get; set; }

    /// <summary>
    /// Thời điểm đăng ký
    /// </summary>
    [XmlElement("thoiDiemDangKy")]
    public DateTime? ThoiDiemDangKy { get; set; }

    /// <summary>
    /// Ngày bắt đầu
    /// </summary>
    [XmlElement("ngayBatDau")]
    public DateTime? NgayBatDau { get; set; }

    /// <summary>
    /// Ngày kết thúc
    /// </summary>
    [XmlElement("ngayKetThuc")]
    public DateTime? NgayKetThuc { get; set; }

    /// <summary>
    /// Hiệu lực
    /// </summary>
    [XmlElement("hieuLuc")]
    public bool? HieuLuc { get; set; }

    /// <summary>
    /// Chủ tham gia
    /// </summary>
    [XmlElement("ChuThamGia")]
    public ChuThamGia? ChuThamGia { get; set; }

    /// <summary>
    /// Thông tin chi tiết giao dịch bảo đảm
    /// </summary>
    [XmlElement("ThongTinChiTietGiaoDichBaoDam")]
    public ThongTinChiTietGiaoDichBaoDam? ThongTinChiTietGiaoDichBaoDam { get; set; }

    /// <summary>
    /// Ghi chú
    /// </summary>
    [XmlAttribute("GhiChu")]
    public string? GhiChu { get; set; }
}

/// <summary>
/// Chủ tham gia trong giao dịch bảo đảm
/// </summary>
public class ChuThamGia
{
    /// <summary>
    /// Bên thế chấp
    /// </summary>
    [XmlElement("BenTheChap")]
    public BenTheChap? BenTheChap { get; set; }

    /// <summary>
    /// Bên nhận thế chấp
    /// </summary>
    [XmlElement("BenNhanTheChap")]
    public BenNhanTheChap? BenNhanTheChap { get; set; }

    /// <summary>
    /// Bên bảo lãnh
    /// </summary>
    [XmlElement("BenBaoLanh")]
    public BenBaoLanh? BenBaoLanh { get; set; }
}

/// <summary>
/// Bên thế chấp
/// </summary>
public class BenTheChap
{
    /// <summary>
    /// Chủ sở hữu
    /// </summary>
    [XmlElement("Chu")]
    public ChuBenGiaoDich? Chu { get; set; }
}

/// <summary>
/// Bên nhận thế chấp
/// </summary>
public class BenNhanTheChap
{
    /// <summary>
    /// Chủ sở hữu
    /// </summary>
    [XmlElement("Chu")]
    public ChuBenGiaoDich? Chu { get; set; }
}

/// <summary>
/// Bên bảo lãnh
/// </summary>
public class BenBaoLanh
{
    /// <summary>
    /// Chủ sở hữu
    /// </summary>
    [XmlElement("Chu")]
    public ChuBenGiaoDich? Chu { get; set; }
}

/// <summary>
/// Chủ tham gia bên giao dịch
/// </summary>
public class ChuBenGiaoDich
{
    /// <summary>
    /// Loại đối tượng
    /// </summary>
    [XmlElement("loaiDoiTuong")]
    public string? LoaiDoiTuong { get; set; }

    /// <summary>
    /// Mã định danh chủ sở hữu
    /// </summary>
    [XmlElement("chuId")]
    public string? ChuId { get; set; }
}

/// <summary>
/// Thông tin chi tiết giao dịch bảo đảm
/// </summary>
public class ThongTinChiTietGiaoDichBaoDam
{
    /// <summary>
    /// Chi tiết giao dịch bảo đảm
    /// </summary>
    [XmlElement("DC_ChiTietGiaoDichBaoDam")]
    public DcChiTietGiaoDichBaoDam? ChiTietGiaoDichBaoDam { get; set; }
}

/// <summary>
/// Chi tiết giao dịch bảo đảm
/// </summary>
public class DcChiTietGiaoDichBaoDam
{
    /// <summary>
    /// Mã định danh chi tiết giao dịch bảo đảm
    /// </summary>
    [XmlElement("chiTietGiaoDichBaoDamId")]
    public string? ChiTietGiaoDichBaoDamId { get; set; }

    /// <summary>
    /// Mã định danh giấy chứng nhận
    /// </summary>
    [XmlElement("giayChungNhanId")]
    public string? GiayChungNhanId { get; set; }

    /// <summary>
    /// Loại tài sản
    /// </summary>
    [XmlElement("loaiTaiSan")]
    public string? LoaiTaiSan { get; set; }

    /// <summary>
    /// Mã định danh tài sản
    /// </summary>
    [XmlElement("taiSanId")]
    public string? TaiSanId { get; set; }

    /// <summary>
    /// Mã định danh hạng mục tài sản
    /// </summary>
    [XmlElement("hangMucTaiSanId")]
    public string? HangMucTaiSanId { get; set; }

    /// <summary>
    /// Diện tích thế chấp
    /// </summary>
    [XmlElement("dienTichTheChap")]
    public decimal? DienTichTheChap { get; set; }
}

/// <summary>
/// Wrapper cho thông tin đăng ký sở hữu chung riêng
/// </summary>
public class ThongTinDangKySoHuuChungRieng
{
    /// <summary>
    /// Thông tin chi tiết đăng ký chung riêng
    /// </summary>
    [XmlElement("DC_DangKyChungRieng")]
    public DcDangKyChungRieng? DangKyChungRieng { get; set; }
}

/// <summary>
/// Thông tin đăng ký chung riêng
/// </summary>
public class DcDangKyChungRieng
{
    /// <summary>
    /// Mã định danh đăng ký chung riêng
    /// </summary>
    [XmlElement("dangKyChungRiengId")]
    public string? DangKyChungRiengId { get; set; }

    /// <summary>
    /// Loại tài sản
    /// </summary>
    [XmlElement("loaiTaiSan")]
    public string? LoaiTaiSan { get; set; }

    /// <summary>
    /// Mã định danh tài sản
    /// </summary>
    [XmlElement("taiSanId")]
    public string? TaiSanId { get; set; }

    /// <summary>
    /// Mã định danh hạng mục tài sản
    /// </summary>
    [XmlElement("hangMucTaiSanId")]
    public string? HangMucTaiSanId { get; set; }

    /// <summary>
    /// Diện tích
    /// </summary>
    [XmlElement("dienTich")]
    public decimal? DienTich { get; set; }

    /// <summary>
    /// Sử dụng chung
    /// </summary>
    [XmlElement("suDungChung")]
    public bool? SuDungChung { get; set; }

    /// <summary>
    /// Đủ điều kiện
    /// </summary>
    [XmlElement("duDieuKien")]
    public bool? DuDieuKien { get; set; }

    /// <summary>
    /// Ngày bắt đầu
    /// </summary>
    [XmlElement("ngayBatDau")]
    public DateTime? NgayBatDau { get; set; }

    /// <summary>
    /// Ngày hết hạn
    /// </summary>
    [XmlElement("ngayHetHan")]
    public DateTime? NgayHetHan { get; set; }

    /// <summary>
    /// Thời hạn
    /// </summary>
    [XmlElement("thoiHan")]
    public int? ThoiHan { get; set; }

    /// <summary>
    /// Chung riêng
    /// </summary>
    [XmlElement("ChungRieng")]
    public ChungRieng? ChungRieng { get; set; }

    /// <summary>
    /// Ghi chú
    /// </summary>
    [XmlAttribute("GhiChu")]
    public string? GhiChu { get; set; }
}

/// <summary>
/// Chung riêng
/// </summary>
public class ChungRieng
{
    /// <summary>
    /// Chủ chung riêng
    /// </summary>
    [XmlElement("ChuChungRieng")]
    public ChuChungRieng? ChuChungRieng { get; set; }
}

/// <summary>
/// Chủ chung riêng
/// </summary>
public class ChuChungRieng
{
    /// <summary>
    /// Loại đối tượng
    /// </summary>
    [XmlElement("loaiDoiTuong")]
    public string? LoaiDoiTuong { get; set; }

    /// <summary>
    /// Mã định danh chủ sở hữu
    /// </summary>
    [XmlElement("chuId")]
    public string? ChuId { get; set; }
}