using System.Xml.Serialization;

namespace Haihv.Elis.Tool.VbdLis.Models;

/// <summary>
/// Wrapper cho thông tin nhà riêng lẻ
/// </summary>
public class ThongTinNhaRiengLe
{
    /// <summary>
    /// Thông tin chi tiết nhà riêng lẻ
    /// </summary>
    [XmlElement("DC_NhaRiengLe")]
    public DcNhaRiengLe? NhaRiengLe { get; set; }
}

/// <summary>
/// Thông tin nhà riêng lẻ - Trang 43
/// </summary>
public class DcNhaRiengLe
{
    /// <summary>
    /// Mã định danh nhà riêng lẻ
    /// </summary>
    [XmlElement("nhaRiengLeId")]
    public string? NhaRiengLeId { get; set; }

    /// <summary>
    /// Mã xã
    /// </summary>
    [XmlElement("maXa")]
    public string? MaXa { get; set; }

    /// <summary>
    /// Loại nhà riêng lẻ
    /// </summary>
    [XmlElement("loaiNhaRiengLe")]
    public string? LoaiNhaRiengLe { get; set; }

    /// <summary>
    /// Loại quyền sở hữu nhà riêng lẻ
    /// </summary>
    [XmlElement("loaiQuyenSoHuuNhaRiengLe")]
    public string? LoaiQuyenSoHuuNhaRiengLe { get; set; }

    /// <summary>
    /// Số nhà
    /// </summary>
    [XmlElement("soNha")]
    public string? SoNha { get; set; }    /// <summary>
                                          /// Thời hạn sở hữu
                                          /// </summary>
    [XmlElement("thoiHanSoHuu")]
    public string? ThoiHanSoHuu { get; set; }

    /// <summary>
    /// Diện tích xây dựng
    /// </summary>
    [XmlElement("dienTichXayDung")]
    public decimal? DienTichXayDung { get; set; }

    /// <summary>
    /// Diện tích sử dụng
    /// </summary>
    [XmlElement("dienTichSuDung")]
    public decimal? DienTichSuDung { get; set; }

    /// <summary>
    /// Diện tích sàn
    /// </summary>
    [XmlElement("dienTichSan")]
    public decimal? DienTichSan { get; set; }

    /// <summary>
    /// Diện tích sàn phụ
    /// </summary>
    [XmlElement("dienTichSanPhu")]
    public decimal? DienTichSanPhu { get; set; }

    /// <summary>
    /// Số tầng
    /// </summary>
    [XmlElement("soTang")]
    public int? SoTang { get; set; }

    /// <summary>
    /// Số tầng hầm
    /// </summary>
    [XmlElement("soTangHam")]
    public int? SoTangHam { get; set; }

    /// <summary>
    /// Năm hoàn thành
    /// </summary>
    [XmlElement("namHoanThanh")]
    public int? NamHoanThanh { get; set; }

    /// <summary>
    /// Kết cấu
    /// </summary>
    [XmlElement("ketCau")]
    public string? KetCau { get; set; }

    /// <summary>
    /// Cấp hạng
    /// </summary>
    [XmlElement("capHang")]
    public string? CapHang { get; set; }

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
    /// Thông tin liên kết thửa đất
    /// </summary>
    [XmlElement("ThongTinLienKetThuaDat")]
    public ThongTinLienKetThuaDat? ThongTinLienKetThuaDat { get; set; }

    /// <summary>
    /// Trang
    /// </summary>
    [XmlAttribute("Trang")]
    public string? Trang { get; set; }
}

/// <summary>
/// Wrapper cho thông tin khu chung cư
/// </summary>
public class ThongTinKhuChungCu
{
    /// <summary>
    /// Thông tin chi tiết khu chung cư
    /// </summary>
    [XmlElement("DC_KhuChungCu")]
    public DcKhuChungCu? KhuChungCu { get; set; }
}

/// <summary>
/// Thông tin khu chung cư - Trang 44
/// </summary>
public class DcKhuChungCu
{
    /// <summary>
    /// Mã định danh khu chung cư
    /// </summary>
    [XmlElement("khuChungCuId")]
    public string? KhuChungCuId { get; set; }

    /// <summary>
    /// Mã xã
    /// </summary>
    [XmlElement("maXa")]
    public string? MaXa { get; set; }

    /// <summary>
    /// Tên khu
    /// </summary>
    [XmlElement("tenKhu")]
    public string? TenKhu { get; set; }

    /// <summary>
    /// Diện tích khu
    /// </summary>
    [XmlElement("dienTichKhu")]
    public decimal? DienTichKhu { get; set; }

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
    /// Thông tin liên kết thửa đất
    /// </summary>
    [XmlElement("ThongTinLienKetThuaDat")]
    public ThongTinLienKetThuaDat? ThongTinLienKetThuaDat { get; set; }

    /// <summary>
    /// Trang
    /// </summary>
    [XmlAttribute("Trang")]
    public string? Trang { get; set; }
}

/// <summary>
/// Wrapper cho thông tin nhà chung cư
/// </summary>
public class ThongTinNhaChungCu
{
    /// <summary>
    /// Thông tin chi tiết nhà chung cư
    /// </summary>
    [XmlElement("DC_NhaChungCu")]
    public DcNhaChungCu? NhaChungCu { get; set; }
}

/// <summary>
/// Thông tin nhà chung cư - Trang 44
/// </summary>
public class DcNhaChungCu
{
    /// <summary>
    /// Mã định danh nhà chung cư
    /// </summary>
    [XmlElement("nhaChungCuId")]
    public string? NhaChungCuId { get; set; }

    /// <summary>
    /// Mã định danh khu chung cư
    /// </summary>
    [XmlElement("khuChungCuId")]
    public string? KhuChungCuId { get; set; }

    /// <summary>
    /// Tên chung cư
    /// </summary>
    [XmlElement("tenChungCu")]
    public string? TenChungCu { get; set; }

    /// <summary>
    /// Diện tích xây dựng
    /// </summary>
    [XmlElement("dienTichXayDung")]
    public decimal? DienTichXayDung { get; set; }

    /// <summary>
    /// Diện tích sàn
    /// </summary>
    [XmlElement("dienTichSan")]
    public decimal? DienTichSan { get; set; }

    /// <summary>
    /// Tổng số căn
    /// </summary>
    [XmlElement("tongSoCan")]
    public int? TongSoCan { get; set; }

    /// <summary>
    /// Số tầng
    /// </summary>
    [XmlElement("soTang")]
    public int? SoTang { get; set; }

    /// <summary>
    /// Số tầng hầm
    /// </summary>
    [XmlElement("soTangHam")]
    public int? SoTangHam { get; set; }

    /// <summary>
    /// Năm xây dựng
    /// </summary>
    [XmlElement("namXayDung")]
    public int? NamXayDung { get; set; }

    /// <summary>
    /// Năm hoàn thành
    /// </summary>
    [XmlElement("namHoanThanh")]
    public int? NamHoanThanh { get; set; }

    /// <summary>
    /// Thời hạn sở hữu
    /// </summary>    [XmlElement("thoiHanSoHuu")]
    public string? ThoiHanSoHuu { get; set; }

    /// <summary>
    /// Cấp hạng
    /// </summary>
    [XmlElement("capHang")]
    public string? CapHang { get; set; }

    /// <summary>
    /// Địa chỉ
    /// </summary>
    [XmlElement("diaChi")]
    public string? DiaChi { get; set; }

    /// <summary>
    /// Hạng mục sở hữu chung
    /// </summary>
    [XmlElement("DC_HangMucSoHuuChung")]
    public DcHangMucSoHuuChung? HangMucSoHuuChung { get; set; }

    /// <summary>
    /// Thông tin địa chỉ chuẩn
    /// </summary>
    [XmlElement("ThongTinDiaChiChuan")]
    public ThongTinDiaChiChuan? ThongTinDiaChiChuan { get; set; }

    /// <summary>
    /// Thông tin liên kết thửa đất
    /// </summary>
    [XmlElement("ThongTinLienKetThuaDat")]
    public ThongTinLienKetThuaDat? ThongTinLienKetThuaDat { get; set; }

    /// <summary>
    /// Trang
    /// </summary>
    [XmlAttribute("Trang")]
    public string? Trang { get; set; }
}

/// <summary>
/// Hạng mục sở hữu chung - Trang 46
/// </summary>
public class DcHangMucSoHuuChung
{
    /// <summary>
    /// Mã định danh hạng mục sở hữu chung
    /// </summary>
    [XmlElement("hangMucSoHuuChungId")]
    public string? HangMucSoHuuChungId { get; set; }

    /// <summary>
    /// Tên hạng mục
    /// </summary>
    [XmlElement("tenHangMuc")]
    public string? TenHangMuc { get; set; }

    /// <summary>
    /// Diện tích
    /// </summary>
    [XmlElement("dienTich")]
    public decimal? DienTich { get; set; }

    /// <summary>
    /// Vị trí tầng
    /// </summary>
    [XmlElement("viTriTang")]
    public string? ViTriTang { get; set; }

    /// <summary>
    /// Ghi chú
    /// </summary>
    [XmlElement("ghiChu")]
    public string? GhiChu { get; set; }

    /// <summary>
    /// Trang
    /// </summary>
    [XmlAttribute("Trang")]
    public string? Trang { get; set; }
}

/// <summary>
/// Wrapper cho thông tin căn hộ
/// </summary>
public class ThongTinCanHo
{
    /// <summary>
    /// Thông tin chi tiết căn hộ
    /// </summary>
    [XmlElement("DC_CanHo")]
    public DcCanHo? CanHo { get; set; }
}

/// <summary>
/// Thông tin căn hộ - Trang 45
/// </summary>
public class DcCanHo
{
    /// <summary>
    /// Mã định danh căn hộ
    /// </summary>
    [XmlElement("canHoId")]
    public string? CanHoId { get; set; }

    /// <summary>
    /// Mã định danh nhà chung cư
    /// </summary>
    [XmlElement("nhaChungCuId")]
    public string? NhaChungCuId { get; set; }

    /// <summary>
    /// Loại quyền sở hữu căn hộ
    /// </summary>
    [XmlElement("loaiQuyenSoHuuCanHo")]
    public string? LoaiQuyenSoHuuCanHo { get; set; }

    /// <summary>
    /// Số hiệu căn hộ
    /// </summary>
    [XmlElement("soHieuCanHo")]
    public string? SoHieuCanHo { get; set; }

    /// <summary>
    /// Tầng số
    /// </summary>
    [XmlElement("tangSo")]
    public int? TangSo { get; set; }

    /// <summary>
    /// Diện tích sàn
    /// </summary>
    [XmlElement("dienTichSan")]
    public decimal? DienTichSan { get; set; }

    /// <summary>
    /// Diện tích sử dụng
    /// </summary>
    [XmlElement("dienTichSuDung")]
    public decimal? DienTichSuDung { get; set; }

    /// <summary>
    /// Trang
    /// </summary>
    [XmlAttribute("Trang")]
    public string? Trang { get; set; }
}