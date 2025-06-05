using Haihv.Elis.Tools.Data.Models;

namespace Haihv.Elis.Tools.Data.DanhMuc;

/// <summary>
/// Danh mục loại đối tượng sử dụng đất trong hệ thống.
/// <para>
/// - Cung cấp các hằng số đại diện cho từng loại đối tượng sử dụng đất.
/// - Hỗ trợ lấy mã theo tên và ngược lại.
/// </para>
/// <para>
/// <b>Danh sách các loại đối tượng sử dụng đất:</b>
/// <list type="bullet">
/// <item><term>NSD</term><description>Người sử dụng đất</description></item>
/// <item><term>NQL</term><description>Người được giao quản lý đất</description></item>
/// <item><term>UBS</term><description>UBND xã sử dụng</description></item>
/// <item><term>TVN</term><description>Doanh nghiệp có vốn đầu tư nước ngoài</description></item>
/// <item><term>CDS</term><description>Cộng đồng dân cư và cơ sở tôn giáo</description></item>
/// <item><term>UBQ</term><description>Uỷ ban nhân dân cấp xã</description></item>
/// <item><term>TKT</term><description>Tổ chức kinh tế</description></item>
/// <item><term>CNN</term><description>Người Việt Nam định cư ở nước ngoài</description></item>
/// <item><term>TKQ</term><description>Cộng đồng dân cư và tổ chức khác</description></item>
/// <item><term>TLD</term><description>Liên doanh với nước ngoài</description></item>
/// <item><term>GDC</term><description>Hộ gia đình, cá nhân</description></item>
/// <item><term>TCN</term><description>Cơ quan, đơn vị của Nhà nước</description></item>
/// <item><term>NNG</term><description>Tổ chức nước ngoài</description></item>
/// <item><term>CDQ</term><description>Cộng đồng dân cư quản lý</description></item>
/// <item><term>TKH</term><description>Tổ chức khác</description></item>
/// <item><term>TPQ</term><description>Tổ chức phát triển quỹ đất</description></item>
/// <item><term>TSN</term><description>Tổ chức sự nghiệp công lập</description></item>
/// <item><term>TNG</term><description>Tổ chức nước ngoài có chức năng ngoại giao</description></item>
/// <item><term>TVD</term><description>Người Việt Nam định cư ở nước ngoài đầu tư</description></item>
/// </list>
/// </para>
/// </summary>
public static class DanhMucLoaiDoiTuongSuDungDat
{
    /// <summary>
    /// Người sử dụng đất
    /// </summary>
    public static readonly LoaiDoiTuongSuDungDat NSD = new("NSD", "Người sử dụng đất");
    /// <summary>
    /// Người được giao quản lý đất
    /// </summary>
    public static readonly LoaiDoiTuongSuDungDat NQL = new("NQL", "Người được giao quản lý đất");
    /// <summary>
    /// UBND xã sử dụng
    /// </summary>
    public static readonly LoaiDoiTuongSuDungDat UBS = new("UBS", "UBND xã sử dụng");
    /// <summary>
    /// Doanh nghiệp có vốn đầu tư nước ngoài
    /// </summary>
    public static readonly LoaiDoiTuongSuDungDat TVN = new("TVN", "Doanh nghiệp có vốn đầu tư nước ngoài");
    /// <summary>
    /// Cộng đồng dân cư và cơ sở tôn giáo
    /// </summary>
    public static readonly LoaiDoiTuongSuDungDat CDS = new("CDS", "Cộng đồng dân cư và cơ sở tôn giáo");
    /// <summary>
    /// Uỷ ban nhân dân cấp xã
    /// </summary>
    public static readonly LoaiDoiTuongSuDungDat UBQ = new("UBQ", "Uỷ ban nhân dân cấp xã");
    /// <summary>
    /// Tổ chức kinh tế
    /// </summary>
    public static readonly LoaiDoiTuongSuDungDat TKT = new("TKT", "Tổ chức kinh tế");
    /// <summary>
    /// Người Việt Nam định cư ở nước ngoài
    /// </summary>
    public static readonly LoaiDoiTuongSuDungDat CNN = new("CNN", "Người Việt Nam định cư ở nước ngoài");
    /// <summary>
    /// Cộng đồng dân cư và tổ chức khác
    /// </summary>
    public static readonly LoaiDoiTuongSuDungDat TKQ = new("TKQ", "Cộng đồng dân cư và tổ chức khác");
    /// <summary>
    /// Liên doanh với nước ngoài
    /// </summary>
    public static readonly LoaiDoiTuongSuDungDat TLD = new("TLD", "Liên doanh với nước ngoài");
    /// <summary>
    /// Hộ gia đình, cá nhân
    /// </summary>
    public static readonly LoaiDoiTuongSuDungDat GDC = new("GDC", "Hộ gia đình, cá nhân");
    /// <summary>
    /// Cơ quan, đơn vị của Nhà nước
    /// </summary>
    public static readonly LoaiDoiTuongSuDungDat TCN = new("TCN", "Cơ quan, đơn vị của Nhà nước");
    /// <summary>
    /// Tổ chức nước ngoài
    /// </summary>
    public static readonly LoaiDoiTuongSuDungDat NNG = new("NNG", "Tổ chức nước ngoài");
    /// <summary>
    /// Cộng đồng dân cư quản lý
    /// </summary>
    public static readonly LoaiDoiTuongSuDungDat CDQ = new("CDQ", "Cộng đồng dân cư quản lý");
    /// <summary>
    /// Tổ chức khác
    /// </summary>
    public static readonly LoaiDoiTuongSuDungDat TKH = new("TKH", "Tổ chức khác");
    /// <summary>
    /// Tổ chức phát triển quỹ đất
    /// </summary>
    public static readonly LoaiDoiTuongSuDungDat TPQ = new("TPQ", "Tổ chức phát triển quỹ đất");
    /// <summary>
    /// Tổ chức sự nghiệp công lập
    /// </summary>
    public static readonly LoaiDoiTuongSuDungDat TSN = new("TSN", "Tổ chức sự nghiệp công lập");
    /// <summary>
    /// Tổ chức nước ngoài có chức năng ngoại giao
    /// </summary>
    public static readonly LoaiDoiTuongSuDungDat TNG = new("TNG", "Tổ chức nước ngoài có chức năng ngoại giao");
    /// <summary>
    /// Người Việt Nam định cư ở nước ngoài đầu tư
    /// </summary>
    public static readonly LoaiDoiTuongSuDungDat TVD = new("TVD", "Người Việt Nam định cư ở nước ngoài đầu tư");

    /// <summary>
    /// Danh sách tất cả các loại đối tượng sử dụng đất.
    /// <para>
    /// Danh sách này chứa tất cả các đối tượng sử dụng đất được định nghĩa trong hệ thống.
    /// </para>
    /// </summary>
    public static readonly IReadOnlyList<LoaiDoiTuongSuDungDat> TatCa =
    [
        NSD, NQL, UBS, TVN, CDS, UBQ, TKT, CNN, TKQ, TLD, GDC, TCN, NNG, CDQ, TKH, TPQ, TSN, TNG, TVD
    ];

    /// <summary>
    /// Lấy mã đối tượng sử dụng đất theo tên.
    /// <para>
    /// Trả về mã tương ứng với tên đối tượng sử dụng đất. Nếu không tìm thấy sẽ trả về null.
    /// </para>
    /// </summary>
    /// <param name="ten">Tên đối tượng sử dụng đất</param>
    /// <returns>Mã đối tượng hoặc null nếu không tìm thấy</returns>
    public static string? GetMaByTen(string ten)
    {
        var item = TatCa.FirstOrDefault(x => x.Ten == ten);
        return item?.Ma;
    }

    /// <summary>
    /// Lấy tên đối tượng sử dụng đất theo mã.
    /// <para>
    /// Trả về tên tương ứng với mã đối tượng sử dụng đất. Nếu không tìm thấy sẽ trả về null.
    /// </para>
    /// </summary>
    /// <param name="ma">Mã đối tượng sử dụng đất</param>
    /// <returns>Tên đối tượng hoặc null nếu không tìm thấy</returns>
    public static string? GetTenByMa(string ma)
    {
        var item = TatCa.FirstOrDefault(x => x.Ma == ma);
        return item?.Ten;
    }
}