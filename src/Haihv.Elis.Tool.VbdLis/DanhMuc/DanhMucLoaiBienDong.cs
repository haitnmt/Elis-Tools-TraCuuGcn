using Haihv.Elis.Tools.Data.Models;

namespace Haihv.Elis.Tools.Data.DanhMuc;

/// <summary>
/// Danh mục loại biến động sử dụng trong hệ thống. Cung cấp các phương thức lấy mã theo tên và ngược lại.
/// </summary>
public static class DanhMucLoaiBienDong
{
    /// <summary>
    /// Cấp đổi, cấp lại (Mã: CL)
    /// </summary>
    public static readonly LoaiBienDong CL = new("CL", "Cấp đổi, cấp lại");

    /// <summary>
    /// Cho thuê trong khu công nghiệp (Mã: TL)
    /// </summary>
    public static readonly LoaiBienDong TL = new("TL", "Cho thuê trong khu công nghiệp");

    /// <summary>
    /// Cho thuê, cho thuê lại ngoài khu công nghiệp (Mã: CT)
    /// </summary>
    public static readonly LoaiBienDong CT = new("CT", "Cho thuê, cho thuê lại ngoài khu công nghiệp");

    /// <summary>
    /// Chủ đầu tư bán căn hộ chung cư (Mã: BN)
    /// </summary>
    public static readonly LoaiBienDong BN = new("BN", "Chủ đầu tư bán căn hộ chung cư");
    /// <summary>
    /// Chuyển đổi (Mã: CD)
    /// </summary>
    public static readonly LoaiBienDong CD = new("CD", "Chuyển đổi");

    /// <summary>
    /// Chuyển đổi công ty, tách hợp doanh nghiệp (Mã: CP)
    /// </summary>
    public static readonly LoaiBienDong CP = new("CP", "Chuyển đổi công ty, tách hợp doanh nghiệp");

    /// <summary>
    /// Chuyển đổi hộ gia đình, cá nhân sử dụng đất thành tổ chức kinh tế của hộ gia đình, cá nhân (Mã: DC)
    /// </summary>
    public static readonly LoaiBienDong DC = new("DC", "Chuyển đổi hộ gia đình, cá nhân sử dụng đất thành tổ chức kinh tế của hộ gia đình, cá nhân");

    /// <summary>
    /// Chuyển hình thức sử dụng đất (Mã: TG)
    /// </summary>
    public static readonly LoaiBienDong TG = new("TG", "Chuyển hình thức sử dụng đất");
    /// <summary>
    /// Chuyển mục đích (Mã: CM)
    /// </summary>
    public static readonly LoaiBienDong CM = new("CM", "Chuyển mục đích");

    /// <summary>
    /// Chuyển nhượng (Mã: CN)
    /// </summary>
    public static readonly LoaiBienDong CN = new("CN", "Chuyển nhượng");

    /// <summary>
    /// Chuyển quyền do giải quyết khiếu nại, tố cáo (Mã: GK)
    /// </summary>
    public static readonly LoaiBienDong GK = new("GK", "Chuyển quyền do giải quyết khiếu nại, tố cáo");

    /// <summary>
    /// Chuyển quyền do giải quyết tranh chấp (Mã: GT)
    /// </summary>
    public static readonly LoaiBienDong GT = new("GT", "Chuyển quyền do giải quyết tranh chấp");
    /// <summary>
    /// Chuyển quyền do xử lý nợ thế chấp (Mã: XN)
    /// </summary>
    public static readonly LoaiBienDong XN = new("XN", "Chuyển quyền do xử lý nợ thế chấp");

    /// <summary>
    /// Chuyển quyền theo kết quả đấu giá đất (Mã: DG)
    /// </summary>
    public static readonly LoaiBienDong DG = new("DG", "Chuyển quyền theo kết quả đấu giá đất");

    /// <summary>
    /// Chuyển quyền theo quyết định của toà án (Mã: GA)
    /// </summary>
    public static readonly LoaiBienDong GA = new("GA", "Chuyển quyền theo quyết định của toà án");

    /// <summary>
    /// Đính chính thông tin (Mã: SN)
    /// </summary>
    public static readonly LoaiBienDong SN = new("SN", "Đính chính thông tin");
    /// <summary>
    /// Gia hạn sử dụng đất (Mã: GH)
    /// </summary>
    public static readonly LoaiBienDong GH = new("GH", "Gia hạn sử dụng đất");

    /// <summary>
    /// Góp vốn (Mã: GP)
    /// </summary>
    public static readonly LoaiBienDong GP = new("GP", "Góp vốn");

    /// <summary>
    /// Hạn chế thửa đất liền kề (Mã: LK)
    /// </summary>
    public static readonly LoaiBienDong LK = new("LK", "Hạn chế thửa đất liền kề");

    /// <summary>
    /// Hợp nhất hoặc phân chia tài sản hộ gia đình, nhóm người (Mã: TQ)
    /// </summary>
    public static readonly LoaiBienDong TQ = new("TQ", "Hợp nhất hoặc phân chia tài sản hộ gia đình, nhóm người");
    /// <summary>
    /// Hợp nhất hoặc phân chia tài sản vợ chồng (Mã: VC)
    /// </summary>
    public static readonly LoaiBienDong VC = new("VC", "Hợp nhất hoặc phân chia tài sản vợ chồng");

    /// <summary>
    /// Tách gộp thửa (Mã: TN)
    /// </summary>
    public static readonly LoaiBienDong TN = new("TN", "Tách gộp thửa");

    /// <summary>
    /// Tặng cho (Mã: TA)
    /// </summary>
    public static readonly LoaiBienDong TA = new("TA", "Tặng cho");

    /// <summary>
    /// Thay đổi diện tích do sạt lở (Mã: SA)
    /// </summary>
    public static readonly LoaiBienDong SA = new("SA", "Thay đổi diện tích do sạt lở");
    /// <summary>
    /// Thay đổi đơn vị hành chính (Mã: DH)
    /// </summary>
    public static readonly LoaiBienDong DH = new("DH", "Thay đổi đơn vị hành chính");

    /// <summary>
    /// Thay đổi hạn chế quyền sử dụng đất, tài sản (Mã: HC)
    /// </summary>
    public static readonly LoaiBienDong HC = new("HC", "Thay đổi hạn chế quyền sử dụng đất, tài sản");

    /// <summary>
    /// Thay đổi thông tin chủ (Mã: DT)
    /// </summary>
    public static readonly LoaiBienDong DT = new("DT", "Thay đổi thông tin chủ");

    /// <summary>
    /// Thay đổi thông tin tài sản (Mã: TS)
    /// </summary>
    public static readonly LoaiBienDong TS = new("TS", "Thay đổi thông tin tài sản");

    /// <summary>
    /// Thay đổi thông tin thửa (Mã: TD)
    /// </summary>
    public static readonly LoaiBienDong TD = new("TD", "Thay đổi thông tin thửa");
    /// <summary>
    /// Thế chấp, thay đổi nội dung thế chấp (Mã: TC)
    /// </summary>
    public static readonly LoaiBienDong TC = new("TC", "Thế chấp, thay đổi nội dung thế chấp");
    /// <summary>
    /// Thu hồi đất (Mã: TH)
    /// </summary>
    public static readonly LoaiBienDong TH = new("TH", "Thu hồi đất");
    /// <summary>
    /// Thừa kế (Mã: TK)
    /// </summary>
    public static readonly LoaiBienDong TK = new("TK", "Thừa kế");
    /// <summary>
    /// Xoá đăng ký cho thuê, cho thuê lại (Mã: XT)
    /// </summary>
    public static readonly LoaiBienDong XT = new("XT", "Xoá đăng ký cho thuê, cho thuê lại");
    /// <summary>
    /// Xoá đăng ký góp vốn (Mã: XV)
    /// </summary>
    public static readonly LoaiBienDong XV = new("XV", "Xoá góp vốn");
    /// <summary>
    /// Xoá thế chấp (Mã: XC)
    /// </summary>
    public static readonly LoaiBienDong XC = new("XC", "Xoá thế chấp");

    /// <summary>
    /// Danh sách tất cả các loại biến động.
    /// </summary>
    public static readonly IReadOnlyList<LoaiBienDong> TatCa =
    [
        CL, TL, CT, BN, CD, CP, DC, TG, CM, CN, GK, GT, XN, DG, GA, SN, GH, GP, LK, TQ, VC, TN, TA, SA, DH, HC, DT, TS, TD, TC, TH, TK, XT, XV, XC
    ];

    /// <summary>
    /// Lấy mã biến động theo tên.
    /// </summary>
    /// <param name="ten">Tên biến động</param>
    /// <returns>Mã biến động hoặc null nếu không tìm thấy</returns>
    public static string? GetMaByTen(string ten)
    {
        var item = TatCa.FirstOrDefault(x => x.Ten == ten);
        return item?.Ma;
    }

    /// <summary>
    /// Lấy tên biến động theo mã.
    /// </summary>
    /// <param name="ma">Mã biến động</param>
    /// <returns>Tên biến động hoặc null nếu không tìm thấy</returns>
    public static string? GetTenByMa(string ma)
    {
        var item = TatCa.FirstOrDefault(x => x.Ma == ma);
        return item?.Ten;
    }
}
