using Haihv.Elis.Tool.VbdLis.Models;

namespace Haihv.Elis.Tool.VbdLis.DanhMuc;

/// <summary>
/// Danh mục loại mục đích sử dụng đất sử dụng trong hệ thống. Cung cấp các phương thức lấy mã theo tên và ngược lại.
/// </summary>
public static class DanhMucLoaiMucDichSuDungQuyHoach
{
    /// <summary>
    /// Đất an ninh (Mã: CAN)
    /// </summary>
    public static readonly LoaiMucDichSuDung CAN = new("CAN", "Đất an ninh");

    /// <summary>
    /// Đất bãi thải, xử lý chất thải (Mã: DRA)
    /// </summary>
    public static readonly LoaiMucDichSuDung DRA = new("DRA", "Đất bãi thải, xử lý chất thải");

    /// <summary>
    /// Đất chưa sử dụng (Mã: CSD)
    /// </summary>
    public static readonly LoaiMucDichSuDung CSD = new("CSD", "Đất chưa sử dụng");

    /// <summary>
    /// Đất chuyên trồng lúa nước (Mã: LUC)
    /// </summary>
    public static readonly LoaiMucDichSuDung LUC = new("LUC", "Đất chuyên trồng lúa nước");

    /// <summary>
    /// Đất có di tích lịch sử - văn hóa (Mã: DDT)
    /// </summary>
    public static readonly LoaiMucDichSuDung DDT = new("DDT", "Đất có di tích lịch sử - văn hóa");

    /// <summary>
    /// Đất có mặt nước chuyên dùng (Mã: MNC)
    /// </summary>
    public static readonly LoaiMucDichSuDung MNC = new("MNC", "Đất có mặt nước chuyên dùng");

    /// <summary>
    /// Đất cơ sở sản xuất phi nông nghiệp (Mã: SKC)
    /// </summary>
    public static readonly LoaiMucDichSuDung SKC = new("SKC", "Đất cơ sở sản xuất phi nông nghiệp");

    /// <summary>
    /// Đất cơ sở tín ngưỡng (Mã: TIN)
    /// </summary>
    public static readonly LoaiMucDichSuDung TIN = new("TIN", "Đất cơ sở tín ngưỡng");

    /// <summary>
    /// Đất cơ sở tôn giáo (Mã: TON)
    /// </summary>
    public static readonly LoaiMucDichSuDung TON = new("TON", "Đất cơ sở tôn giáo");

    /// <summary>
    /// Đất cụm công nghiệp (Mã: SKN)
    /// </summary>
    public static readonly LoaiMucDichSuDung SKN = new("SKN", "Đất cụm công nghiệp");

    /// <summary>
    /// Đất danh lam thắng cảnh (Mã: DDL)
    /// </summary>
    public static readonly LoaiMucDichSuDung DDL = new("DDL", "Đất danh lam thắng cảnh");

    /// <summary>
    /// Đất đô thị (Mã: KDT)
    /// </summary>
    public static readonly LoaiMucDichSuDung KDT = new("KDT", "Đất đô thị");

    /// <summary>
    /// Đất khu chế xuất (Mã: SKT)
    /// </summary>
    public static readonly LoaiMucDichSuDung SKT = new("SKT", "Đất khu chế xuất");

    /// <summary>
    /// Đất khu công nghệ cao (Mã: KCN)
    /// </summary>
    public static readonly LoaiMucDichSuDung KCN = new("KCN", "Đất khu công nghệ cao");

    /// <summary>
    /// Đất khu công nghiệp (Mã: SKK)
    /// </summary>
    public static readonly LoaiMucDichSuDung SKK = new("SKK", "Đất khu công nghiệp");

    /// <summary>
    /// Đất khu kinh tế (Mã: KKT)
    /// </summary>
    public static readonly LoaiMucDichSuDung KKT = new("KKT", "Đất khu kinh tế");

    /// <summary>
    /// Đất khu vui chơi, giải trí công cộng (Mã: DKV)
    /// </summary>
    public static readonly LoaiMucDichSuDung DKV = new("DKV", "Đất khu vui chơi, giải trí công cộng");

    /// <summary>
    /// Đất làm muối (Mã: LMU)
    /// </summary>
    public static readonly LoaiMucDichSuDung LMU = new("LMU", "Đất làm muối");

    /// <summary>
    /// Đất làm nghĩa trang, nghĩa địa, nhà tang lễ, nhà hỏa táng (Mã: NTD)
    /// </summary>
    public static readonly LoaiMucDichSuDung NTD = new("NTD", "Đất làm nghĩa trang, nghĩa địa, nhà tang lễ, nhà hỏa táng");

    /// <summary>
    /// Đất nông nghiệp (Mã: NNP)
    /// </summary>
    public static readonly LoaiMucDichSuDung NNP = new("NNP", "Đất nông nghiệp");

    /// <summary>
    /// Đất nông nghiệp khác (Mã: NKH)
    /// </summary>
    public static readonly LoaiMucDichSuDung NKH = new("NKH", "Đất nông nghiệp khác");

    /// <summary>
    /// Đất nuôi trồng thủy sản (Mã: NTS)
    /// </summary>
    public static readonly LoaiMucDichSuDung NTS = new("NTS", "Đất nuôi trồng thủy sản");

    /// <summary>
    /// Đất ở tại đô thị (Mã: ODT)
    /// </summary>
    public static readonly LoaiMucDichSuDung ODT = new("ODT", "Đất ở tại đô thị");

    /// <summary>
    /// Đất ở tại nông thôn (Mã: ONT)
    /// </summary>
    public static readonly LoaiMucDichSuDung ONT = new("ONT", "Đất ở tại nông thôn");

    /// <summary>
    /// Đất phát triển hạ tầng cấp quốc gia, cấp tỉnh, cấp huyện, cấp xã (Mã: DHT)
    /// </summary>
    public static readonly LoaiMucDichSuDung DHT = new("DHT", "Đất phát triển hạ tầng cấp quốc gia, cấp tỉnh, cấp huyện, cấp xã");

    /// <summary>
    /// Đất phi nông nghiệp (Mã: PNN)
    /// </summary>
    public static readonly LoaiMucDichSuDung PNN = new("PNN", "Đất phi nông nghiệp");

    /// <summary>
    /// Đất phi nông nghiệp khác (Mã: PNK)
    /// </summary>
    public static readonly LoaiMucDichSuDung PNK = new("PNK", "Đất phi nông nghiệp khác");

    /// <summary>
    /// Đất quốc phòng (Mã: CQP)
    /// </summary>
    public static readonly LoaiMucDichSuDung CQP = new("CQP", "Đất quốc phòng");

    /// <summary>
    /// Đất rừng đặc dụng (Mã: RDD)
    /// </summary>
    public static readonly LoaiMucDichSuDung RDD = new("RDD", "Đất rừng đặc dụng");

    /// <summary>
    /// Đất rừng phòng hộ (Mã: RPH)
    /// </summary>
    public static readonly LoaiMucDichSuDung RPH = new("RPH", "Đất rừng phòng hộ");

    /// <summary>
    /// Đất rừng sản xuất (Mã: RSX)
    /// </summary>
    public static readonly LoaiMucDichSuDung RSX = new("RSX", "Đất rừng sản xuất");

    /// <summary>
    /// Đất sản xuất vật liệu xây dựng, làm đồ gốm (Mã: SKX)
    /// </summary>
    public static readonly LoaiMucDichSuDung SKX = new("SKX", "Đất sản xuất vật liệu xây dựng, làm đồ gốm");

    /// <summary>
    /// Đất sinh hoạt cộng đồng (Mã: DSH)
    /// </summary>
    public static readonly LoaiMucDichSuDung DSH = new("DSH", "Đất sinh hoạt cộng đồng");

    /// <summary>
    /// Đất sông, ngòi, kênh, rạch, suối (Mã: SON)
    /// </summary>
    public static readonly LoaiMucDichSuDung SON = new("SON", "Đất sông, ngòi, kênh, rạch, suối");

    /// <summary>
    /// Đất sử dụng cho hoạt động khoáng sản (Mã: SKS)
    /// </summary>
    public static readonly LoaiMucDichSuDung SKS = new("SKS", "Đất sử dụng cho hoạt động khoáng sản");

    /// <summary>
    /// Đất thương mại, dịch vụ (Mã: TMD)
    /// </summary>
    public static readonly LoaiMucDichSuDung TMD = new("TMD", "Đất thương mại, dịch vụ");

    /// <summary>
    /// Đất trồng cây hàng năm khác (Mã: HNK)
    /// </summary>
    public static readonly LoaiMucDichSuDung HNK = new("HNK", "Đất trồng cây hàng năm khác");

    /// <summary>
    /// Đất trồng cây lâu năm (Mã: CLN)
    /// </summary>
    public static readonly LoaiMucDichSuDung CLN = new("CLN", "Đất trồng cây lâu năm");

    /// <summary>
    /// Đất trồng lúa (Mã: LUA)
    /// </summary>
    public static readonly LoaiMucDichSuDung LUA = new("LUA", "Đất trồng lúa");

    /// <summary>
    /// Đất xây dựng cơ sở ngoại giao (Mã: DNG)
    /// </summary>
    public static readonly LoaiMucDichSuDung DNG = new("DNG", "Đất xây dựng cơ sở ngoại giao");

    /// <summary>
    /// Đất xây dựng trụ sở cơ quan (Mã: TSC)
    /// </summary>
    public static readonly LoaiMucDichSuDung TSC = new("TSC", "Đất xây dựng trụ sở cơ quan");

    /// <summary>
    /// Đất xây dựng trụ sở của tổ chức sự nghiệp (Mã: DTS)
    /// </summary>
    public static readonly LoaiMucDichSuDung DTS = new("DTS", "Đất xây dựng trụ sở của tổ chức sự nghiệp");

    /// <summary>
    /// Núi đá không có rừng cây (Mã: NCS)
    /// </summary>
    public static readonly LoaiMucDichSuDung NCS = new("NCS", "Núi đá không có rừng cây");

    /// <summary>
    /// Danh sách tất cả các loại mục đích sử dụng đất.
    /// </summary>
    public static readonly IReadOnlyList<LoaiMucDichSuDung> TatCa =
    [
        CAN, DRA, CSD, LUC, DDT, MNC, SKC, TIN, TON, SKN, DDL, KDT, SKT, KCN, 
        SKK, KKT, DKV, LMU, NTD, NNP, NKH, NTS, ODT, ONT, DHT, PNN, PNK, CQP, 
        RDD, RPH, RSX, SKX, DSH, SON, SKS, TMD, HNK, CLN, LUA, DNG, TSC, DTS, NCS
    ];

    /// <summary>
    /// Lấy mã mục đích sử dụng đất theo tên.
    /// </summary>
    /// <param name="ten">Tên mục đích sử dụng đất</param>
    /// <returns>Mã mục đích sử dụng đất hoặc null nếu không tìm thấy</returns>
    public static string? GetMaByTen(string ten)
    {
        var item = TatCa.FirstOrDefault(x => x.Ten == ten);
        return item?.Ma;
    }

    /// <summary>
    /// Lấy tên mục đích sử dụng đất theo mã.
    /// </summary>
    /// <param name="ma">Mã mục đích sử dụng đất</param>
    /// <returns>Tên mục đích sử dụng đất hoặc null nếu không tìm thấy</returns>
    public static string? GetTenByMa(string ma)
    {
        var item = TatCa.FirstOrDefault(x => x.Ma == ma);
        return item?.Ten;
    }
}