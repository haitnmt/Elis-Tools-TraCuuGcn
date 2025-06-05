using Haihv.Elis.Tools.Data.Models;

namespace Haihv.Elis.Tools.Data.DanhMuc;

/// <summary>
/// Danh mục loại mục đích sử dụng đất sử dụng trong hệ thống. Cung cấp các phương thức lấy mã theo tên và ngược lại.
/// </summary>
public static class DanhMucLoaiMucDichSuDung
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
    /// Đất bằng chưa sử dụng (Mã: BCS)
    /// </summary>
    public static readonly LoaiMucDichSuDung BCS = new("BCS", "Đất bằng chưa sử dụng");

    /// <summary>
    /// Đất bằng trồng cây hàng năm khác (Mã: BHK)
    /// </summary>
    public static readonly LoaiMucDichSuDung BHK = new("BHK", "Đất bằng trồng cây hàng năm khác");

    /// <summary>
    /// Đất chợ (Mã: DCH)
    /// </summary>
    public static readonly LoaiMucDichSuDung DCH = new("DCH", "Đất chợ");

    /// <summary>
    /// Đất chuyên trồng lúa nước (Mã: LUC)
    /// </summary>
    public static readonly LoaiMucDichSuDung LUC = new("LUC", "Đất chuyên trồng lúa nước");

    /// <summary>
    /// Đất có danh lam thắng cảnh (Mã: DDL)
    /// </summary>
    public static readonly LoaiMucDichSuDung DDL = new("DDL", "Đất có danh lam thắng cảnh");

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
    /// Đất công trình bưu chính, viễn thông (Mã: DBV)
    /// </summary>
    public static readonly LoaiMucDichSuDung DBV = new("DBV", "Đất công trình bưu chính, viễn thông");

    /// <summary>
    /// Đất công trình công cộng khác (Mã: DCK)
    /// </summary>
    public static readonly LoaiMucDichSuDung DCK = new("DCK", "Đất công trình công cộng khác");

    /// <summary>
    /// Đất công trình năng lượng (Mã: DNL)
    /// </summary>
    public static readonly LoaiMucDichSuDung DNL = new("DNL", "Đất công trình năng lượng");

    /// <summary>
    /// Đất cụm công nghiệp (Mã: SKN)
    /// </summary>
    public static readonly LoaiMucDichSuDung SKN = new("SKN", "Đất cụm công nghiệp");

    /// <summary>
    /// Đất đồi núi chưa sử dụng (Mã: DCS)
    /// </summary>
    public static readonly LoaiMucDichSuDung DCS = new("DCS", "Đất đồi núi chưa sử dụng");

    /// <summary>
    /// Đất giao thông (Mã: DGT)
    /// </summary>
    public static readonly LoaiMucDichSuDung DGT = new("DGT", "Đất giao thông");

    /// <summary>
    /// Đất khu chế xuất (Mã: SKT)
    /// </summary>
    public static readonly LoaiMucDichSuDung SKT = new("SKT", "Đất khu chế xuất");

    /// <summary>
    /// Đất khu công nghiệp (Mã: SKK)
    /// </summary>
    public static readonly LoaiMucDichSuDung SKK = new("SKK", "Đất khu công nghiệp");

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
    /// Đất nông nghiệp khác (Mã: NKH)
    /// </summary>
    public static readonly LoaiMucDichSuDung NKH = new("NKH", "Đất nông nghiệp khác");

    /// <summary>
    /// Đất nuôi trồng thủy sản (Mã: NTS)
    /// </summary>
    public static readonly LoaiMucDichSuDung NTS = new("NTS", "Đất nuôi trồng thủy sản");

    /// <summary>
    /// Đất nương rẫy trồng cây hàng năm khác (Mã: NHK)
    /// </summary>
    public static readonly LoaiMucDichSuDung NHK = new("NHK", "Đất nương rẫy trồng cây hàng năm khác");

    /// <summary>
    /// Đất ở tại đô thị (Mã: ODT)
    /// </summary>
    public static readonly LoaiMucDichSuDung ODT = new("ODT", "Đất ở tại đô thị");

    /// <summary>
    /// Đất ở tại nông thôn (Mã: ONT)
    /// </summary>
    public static readonly LoaiMucDichSuDung ONT = new("ONT", "Đất ở tại nông thôn");

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
    /// Đất thủy lợi (Mã: DTL)
    /// </summary>
    public static readonly LoaiMucDichSuDung DTL = new("DTL", "Đất thủy lợi");

    /// <summary>
    /// Đất trồng cây lâu năm (Mã: CLN)
    /// </summary>
    public static readonly LoaiMucDichSuDung CLN = new("CLN", "Đất trồng cây lâu năm");

    /// <summary>
    /// Đất trồng lúa nước còn lại (Mã: LUK)
    /// </summary>
    public static readonly LoaiMucDichSuDung LUK = new("LUK", "Đất trồng lúa nước còn lại");

    /// <summary>
    /// Đất trồng lúa nương (Mã: LUN)
    /// </summary>
    public static readonly LoaiMucDichSuDung LUN = new("LUN", "Đất trồng lúa nương");

    /// <summary>
    /// Đất xây dựng cơ sở dịch vụ xã hội (Mã: DXH)
    /// </summary>
    public static readonly LoaiMucDichSuDung DXH = new("DXH", "Đất xây dựng cơ sở dịch vụ xã hội");

    /// <summary>
    /// Đất xây dựng cơ sở giáo dục và đào tạo (Mã: DGD)
    /// </summary>
    public static readonly LoaiMucDichSuDung DGD = new("DGD", "Đất xây dựng cơ sở giáo dục và đào tạo");

    /// <summary>
    /// Đất xây dựng cơ sở khoa học và công nghệ (Mã: DKH)
    /// </summary>
    public static readonly LoaiMucDichSuDung DKH = new("DKH", "Đất xây dựng cơ sở khoa học và công nghệ");

    /// <summary>
    /// Đất xây dựng cơ sở ngoại giao (Mã: DNG)
    /// </summary>
    public static readonly LoaiMucDichSuDung DNG = new("DNG", "Đất xây dựng cơ sở ngoại giao");

    /// <summary>
    /// Đất xây dựng cơ sở thể dục thể thao (Mã: DTT)
    /// </summary>
    public static readonly LoaiMucDichSuDung DTT = new("DTT", "Đất xây dựng cơ sở thể dục thể thao");

    /// <summary>
    /// Đất xây dựng cơ sở văn hóa (Mã: DVH)
    /// </summary>
    public static readonly LoaiMucDichSuDung DVH = new("DVH", "Đất xây dựng cơ sở văn hóa");

    /// <summary>
    /// Đất xây dựng cơ sở y tế (Mã: DYT)
    /// </summary>
    public static readonly LoaiMucDichSuDung DYT = new("DYT", "Đất xây dựng cơ sở y tế");

    /// <summary>
    /// Đất xây dựng công trình sự nghiệp khác (Mã: DSK)
    /// </summary>
    public static readonly LoaiMucDichSuDung DSK = new("DSK", "Đất xây dựng công trình sự nghiệp khác");

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
        CAN, DRA, BCS, BHK, DCH, LUC, DDL, DDT, MNC, SKC, TIN, TON, DBV, DCK, DNL, SKN, DCS, DGT, SKT, SKK, DKV, LMU, NTD, NKH, NTS, NHK, ODT, ONT, PNK, CQP, RDD, RPH, RSX, SKX, DSH, SON, SKS, TMD, DTL, CLN, LUK, LUN, DXH, DGD, DKH, DNG, DTT, DVH, DYT, DSK, TSC, DTS, NCS
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