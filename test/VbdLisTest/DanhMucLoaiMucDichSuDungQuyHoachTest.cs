using NUnit.Framework;
using Haihv.Elis.Tool.VbdLis.DanhMuc;

namespace VbdLisTest
{
    [TestFixture]
    public class DanhMucLoaiMucDichSuDungQuyHoachTest
    {
        [Test]
        public void GetMaByTen_ReturnsCorrectCode_ForValidName()
        {
            Assert.Multiple(() =>
            {
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("Đất an ninh"), Is.EqualTo("CAN"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("Đất bãi thải, xử lý chất thải"), Is.EqualTo("DRA"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("Đất chưa sử dụng"), Is.EqualTo("CSD"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("Đất chuyên trồng lúa nước"), Is.EqualTo("LUC"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("Đất có di tích lịch sử - văn hóa"), Is.EqualTo("DDT"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("Đất có mặt nước chuyên dùng"), Is.EqualTo("MNC"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("Đất cơ sở sản xuất phi nông nghiệp"), Is.EqualTo("SKC"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("Đất cơ sở tín ngưỡng"), Is.EqualTo("TIN"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("Đất cơ sở tôn giáo"), Is.EqualTo("TON"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("Đất cụm công nghiệp"), Is.EqualTo("SKN"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("Đất danh lam thắng cảnh"), Is.EqualTo("DDL"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("Đất đô thị"), Is.EqualTo("KDT"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("Đất khu chế xuất"), Is.EqualTo("SKT"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("Đất khu công nghệ cao"), Is.EqualTo("KCN"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("Đất khu công nghiệp"), Is.EqualTo("SKK"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("Đất khu kinh tế"), Is.EqualTo("KKT"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("Đất khu vui chơi, giải trí công cộng"), Is.EqualTo("DKV"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("Đất làm muối"), Is.EqualTo("LMU"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("Đất làm nghĩa trang, nghĩa địa, nhà tang lễ, nhà hỏa táng"), Is.EqualTo("NTD"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("Đất nông nghiệp"), Is.EqualTo("NNP"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("Đất nông nghiệp khác"), Is.EqualTo("NKH"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("Đất nuôi trồng thủy sản"), Is.EqualTo("NTS"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("Đất ở tại đô thị"), Is.EqualTo("ODT"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("Đất ở tại nông thôn"), Is.EqualTo("ONT"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("Đất phát triển hạ tầng cấp quốc gia, cấp tỉnh, cấp huyện, cấp xã"), Is.EqualTo("DHT"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("Đất phi nông nghiệp"), Is.EqualTo("PNN"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("Đất phi nông nghiệp khác"), Is.EqualTo("PNK"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("Đất quốc phòng"), Is.EqualTo("CQP"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("Đất rừng đặc dụng"), Is.EqualTo("RDD"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("Đất rừng phòng hộ"), Is.EqualTo("RPH"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("Đất rừng sản xuất"), Is.EqualTo("RSX"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("Đất sản xuất vật liệu xây dựng, làm đồ gốm"), Is.EqualTo("SKX"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("Đất sinh hoạt cộng đồng"), Is.EqualTo("DSH"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("Đất sông, ngòi, kênh, rạch, suối"), Is.EqualTo("SON"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("Đất sử dụng cho hoạt động khoáng sản"), Is.EqualTo("SKS"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("Đất thương mại, dịch vụ"), Is.EqualTo("TMD"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("Đất trồng cây hàng năm khác"), Is.EqualTo("HNK"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("Đất trồng cây lâu năm"), Is.EqualTo("CLN"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("Đất trồng lúa"), Is.EqualTo("LUA"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("Đất xây dựng cơ sở ngoại giao"), Is.EqualTo("DNG"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("Đất xây dựng trụ sở cơ quan"), Is.EqualTo("TSC"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("Đất xây dựng trụ sở của tổ chức sự nghiệp"), Is.EqualTo("DTS"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("dat an ninh"), Is.EqualTo("CAN"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("dat bai thai, xu ly chat thai"), Is.EqualTo("DRA"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("dat chua su dung"), Is.EqualTo("CSD"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("dat chuyen trong lua nuoc"), Is.EqualTo("LUC"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("dat co di tich lich su - van hoa"), Is.EqualTo("DDT"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("dat co mat nuoc chuyen dung"), Is.EqualTo("MNC"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("dat co so san xuat phi nong nghiep"), Is.EqualTo("SKC"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("dat co so tin nguong"), Is.EqualTo("TIN"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("dat co so ton giao"), Is.EqualTo("TON"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("dat cum cong nghiep"), Is.EqualTo("SKN"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("dat danh lam thang canh"), Is.EqualTo("DDL"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("dat do thi"), Is.EqualTo("KDT"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("dat khu che xuat"), Is.EqualTo("SKT"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("dat khu cong nghe cao"), Is.EqualTo("KCN"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("dat khu cong nghiep"), Is.EqualTo("SKK"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("dat khu kinh te"), Is.EqualTo("KKT"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("dat khu vui choi, giai tri cong cong"), Is.EqualTo("DKV"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("dat lam muoi"), Is.EqualTo("LMU"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("dat lam nghia trang, nghia dia, nha tang le, nha hoa tang"), Is.EqualTo("NTD"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("dat nong nghiệp"), Is.EqualTo("NNP"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("dat nong nghiệp khac"), Is.EqualTo("NKH"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("dat nuoi trong thuy san"), Is.EqualTo("NTS"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("dat o tai do thi"), Is.EqualTo("ODT"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("dat o tai nong thon"), Is.EqualTo("ONT"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("dat phat trien ha tang cap quoc gia, cap tinh, cap huyen, cap xa"), Is.EqualTo("DHT"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("dat phi nong nghiệp"), Is.EqualTo("PNN"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("dat phi nong nghiệp khac"), Is.EqualTo("PNK"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("dat quoc phong"), Is.EqualTo("CQP"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("dat rung dac dung"), Is.EqualTo("RDD"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("dat rung phong ho"), Is.EqualTo("RPH"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("dat rung san xuat"), Is.EqualTo("RSX"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("dat san xuat vat lieu xay dung, lam do gom"), Is.EqualTo("SKX"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("dat sinh hoat cong dong"), Is.EqualTo("DSH"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("dat song, ngoi, kenh, rach, suoi"), Is.EqualTo("SON"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("dat su dung cho hoat dong khoang san"), Is.EqualTo("SKS"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("dat thuong mai, dich vu"), Is.EqualTo("TMD"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("dat trong cay hang nam khac"), Is.EqualTo("HNK"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("dat trong cay lau nam"), Is.EqualTo("CLN"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("dat trong lua"), Is.EqualTo("LUA"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("dat xay dung co so ngoai giao"), Is.EqualTo("DNG"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("dat xay dung tru so co quan"), Is.EqualTo("TSC"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("dat xay dung tru so cua to chuc su nghiep"), Is.EqualTo("DTS"));
            });
        }
        
        [Test]
        public void GetMaByTen_ReturnsNull_ForInvalidName()
        {
            Assert.Multiple(() =>
            {
                // Act & Assert
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen("Tên không tồn tại"), Is.Null);
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetMaByTen(""), Is.Null);
            });
        }

        [Test]
        public void GetTenByMa_ReturnsCorrectName_ForValidCode()
        {
            Assert.Multiple(() =>
            {
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetTenByMa("CAN"), Is.EqualTo("Đất an ninh"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetTenByMa("DRA"), Is.EqualTo("Đất bãi thải, xử lý chất thải"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetTenByMa("CSD"), Is.EqualTo("Đất chưa sử dụng"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetTenByMa("LUC"), Is.EqualTo("Đất chuyên trồng lúa nước"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetTenByMa("DDT"), Is.EqualTo("Đất có di tích lịch sử - văn hóa"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetTenByMa("MNC"), Is.EqualTo("Đất có mặt nước chuyên dùng"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetTenByMa("SKC"), Is.EqualTo("Đất cơ sở sản xuất phi nông nghiệp"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetTenByMa("TIN"), Is.EqualTo("Đất cơ sở tín ngưỡng"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetTenByMa("TON"), Is.EqualTo("Đất cơ sở tôn giáo"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetTenByMa("SKN"), Is.EqualTo("Đất cụm công nghiệp"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetTenByMa("DDL"), Is.EqualTo("Đất danh lam thắng cảnh"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetTenByMa("KDT"), Is.EqualTo("Đất đô thị"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetTenByMa("SKT"), Is.EqualTo("Đất khu chế xuất"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetTenByMa("KCN"), Is.EqualTo("Đất khu công nghệ cao"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetTenByMa("SKK"), Is.EqualTo("Đất khu công nghiệp"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetTenByMa("KKT"), Is.EqualTo("Đất khu kinh tế"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetTenByMa("DKV"), Is.EqualTo("Đất khu vui chơi, giải trí công cộng"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetTenByMa("LMU"), Is.EqualTo("Đất làm muối"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetTenByMa("NTD"), Is.EqualTo("Đất làm nghĩa trang, nghĩa địa, nhà tang lễ, nhà hỏa táng"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetTenByMa("NNP"), Is.EqualTo("Đất nông nghiệp"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetTenByMa("NKH"), Is.EqualTo("Đất nông nghiệp khác"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetTenByMa("NTS"), Is.EqualTo("Đất nuôi trồng thủy sản"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetTenByMa("ODT"), Is.EqualTo("Đất ở tại đô thị"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetTenByMa("ONT"), Is.EqualTo("Đất ở tại nông thôn"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetTenByMa("DHT"), Is.EqualTo("Đất phát triển hạ tầng cấp quốc gia, cấp tỉnh, cấp huyện, cấp xã"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetTenByMa("PNN"), Is.EqualTo("Đất phi nông nghiệp"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetTenByMa("PNK"), Is.EqualTo("Đất phi nông nghiệp khác"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetTenByMa("CQP"), Is.EqualTo("Đất quốc phòng"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetTenByMa("RDD"), Is.EqualTo("Đất rừng đặc dụng"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetTenByMa("RPH"), Is.EqualTo("Đất rừng phòng hộ"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetTenByMa("RSX"), Is.EqualTo("Đất rừng sản xuất"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetTenByMa("SKX"), Is.EqualTo("Đất sản xuất vật liệu xây dựng, làm đồ gốm"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetTenByMa("DSH"), Is.EqualTo("Đất sinh hoạt cộng đồng"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetTenByMa("SON"), Is.EqualTo("Đất sông, ngòi, kênh, rạch, suối"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetTenByMa("SKS"), Is.EqualTo("Đất sử dụng cho hoạt động khoáng sản"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetTenByMa("TMD"), Is.EqualTo("Đất thương mại, dịch vụ"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetTenByMa("HNK"), Is.EqualTo("Đất trồng cây hàng năm khác"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetTenByMa("CLN"), Is.EqualTo("Đất trồng cây lâu năm"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetTenByMa("LUA"), Is.EqualTo("Đất trồng lúa"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetTenByMa("DNG"), Is.EqualTo("Đất xây dựng cơ sở ngoại giao"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetTenByMa("TSC"), Is.EqualTo("Đất xây dựng trụ sở cơ quan"));
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetTenByMa("DTS"), Is.EqualTo("Đất xây dựng trụ sở của tổ chức sự nghiệp"));
            });
        }

        [Test]
        public void GetTenByMa_ReturnsNull_ForInvalidCode()
        {
            Assert.Multiple(() =>
            {
                // Act & Assert
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetTenByMa("XXX"), Is.Null);
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetTenByMa(""), Is.Null);
                Assert.That(DanhMucLoaiMucDichSuDungQuyHoach.GetTenByMa(null), Is.Null);
            });
        }

        [Test]
        public void TatCa_ContainsAllLoaiMucDichSuDungQuyHoach_AndNotEmpty()
        {
            var tatCa = DanhMucLoaiMucDichSuDungQuyHoach.TatCa;
            Assert.That(tatCa, Is.Not.Null);
            Assert.That(tatCa, Is.Not.Empty);
            Assert.Multiple(() =>
            {
                Assert.That(tatCa.Any(item => item.Ma == "CAN" && item.Ten == "Đất an ninh"), Is.True);
                Assert.That(tatCa.Any(item => item.Ma == "DRA" && item.Ten == "Đất bãi thải, xử lý chất thải"), Is.True);
                Assert.That(tatCa.Any(item => item.Ma == "CSD" && item.Ten == "Đất chưa sử dụng"), Is.True);
                Assert.That(tatCa.Any(item => item.Ma == "LUC" && item.Ten == "Đất chuyên trồng lúa nước"), Is.True);
                Assert.That(tatCa.Any(item => item.Ma == "DDT" && item.Ten == "Đất có di tích lịch sử - văn hóa"), Is.True);
                Assert.That(tatCa.Any(item => item.Ma == "MNC" && item.Ten == "Đất có mặt nước chuyên dùng"), Is.True);
                Assert.That(tatCa.Any(item => item.Ma == "SKC" && item.Ten == "Đất cơ sở sản xuất phi nông nghiệp"), Is.True);
                Assert.That(tatCa.Any(item => item.Ma == "TIN" && item.Ten == "Đất cơ sở tín ngưỡng"), Is.True);
                Assert.That(tatCa.Any(item => item.Ma == "TON" && item.Ten == "Đất cơ sở tôn giáo"), Is.True);
                Assert.That(tatCa.Any(item => item.Ma == "SKN" && item.Ten == "Đất cụm công nghiệp"), Is.True);
                Assert.That(tatCa.Any(item => item.Ma == "DDL" && item.Ten == "Đất danh lam thắng cảnh"), Is.True);
                Assert.That(tatCa.Any(item => item.Ma == "KDT" && item.Ten == "Đất đô thị"), Is.True);
                Assert.That(tatCa.Any(item => item.Ma == "SKT" && item.Ten == "Đất khu chế xuất"), Is.True);
                Assert.That(tatCa.Any(item => item.Ma == "KCN" && item.Ten == "Đất khu công nghệ cao"), Is.True);
                Assert.That(tatCa.Any(item => item.Ma == "SKK" && item.Ten == "Đất khu công nghiệp"), Is.True);
                Assert.That(tatCa.Any(item => item.Ma == "KKT" && item.Ten == "Đất khu kinh tế"), Is.True);
                Assert.That(tatCa.Any(item => item.Ma == "DKV" && item.Ten == "Đất khu vui chơi, giải trí công cộng"), Is.True);
                Assert.That(tatCa.Any(item => item.Ma == "LMU" && item.Ten == "Đất làm muối"), Is.True);
                Assert.That(tatCa.Any(item => item.Ma == "NTD" && item.Ten == "Đất làm nghĩa trang, nghĩa địa, nhà tang lễ, nhà hỏa táng"), Is.True);
                Assert.That(tatCa.Any(item => item.Ma == "NNP" && item.Ten == "Đất nông nghiệp"), Is.True);
                Assert.That(tatCa.Any(item => item.Ma == "NKH" && item.Ten == "Đất nông nghiệp khác"), Is.True);
                Assert.That(tatCa.Any(item => item.Ma == "NTS" && item.Ten == "Đất nuôi trồng thủy sản"), Is.True);
                Assert.That(tatCa.Any(item => item.Ma == "ODT" && item.Ten == "Đất ở tại đô thị"), Is.True);
                Assert.That(tatCa.Any(item => item.Ma == "ONT" && item.Ten == "Đất ở tại nông thôn"), Is.True);
                Assert.That(tatCa.Any(item => item.Ma == "DHT" && item.Ten == "Đất phát triển hạ tầng cấp quốc gia, cấp tỉnh, cấp huyện, cấp xã"), Is.True);
                Assert.That(tatCa.Any(item => item.Ma == "PNN" && item.Ten == "Đất phi nông nghiệp"), Is.True);
                Assert.That(tatCa.Any(item => item.Ma == "PNK" && item.Ten == "Đất phi nông nghiệp khác"), Is.True);
                Assert.That(tatCa.Any(item => item.Ma == "CQP" && item.Ten == "Đất quốc phòng"), Is.True);
                Assert.That(tatCa.Any(item => item.Ma == "RDD" && item.Ten == "Đất rừng đặc dụng"), Is.True);
                Assert.That(tatCa.Any(item => item.Ma == "RPH" && item.Ten == "Đất rừng phòng hộ"), Is.True);
                Assert.That(tatCa.Any(item => item.Ma == "RSX" && item.Ten == "Đất rừng sản xuất"), Is.True);
                Assert.That(tatCa.Any(item => item.Ma == "SKX" && item.Ten == "Đất sản xuất vật liệu xây dựng, làm đồ gốm"), Is.True);
                Assert.That(tatCa.Any(item => item.Ma == "DSH" && item.Ten == "Đất sinh hoạt cộng đồng"), Is.True);
                Assert.That(tatCa.Any(item => item.Ma == "SON" && item.Ten == "Đất sông, ngòi, kênh, rạch, suối"), Is.True);
                Assert.That(tatCa.Any(item => item.Ma == "SKS" && item.Ten == "Đất sử dụng cho hoạt động khoáng sản"), Is.True);
                Assert.That(tatCa.Any(item => item.Ma == "TMD" && item.Ten == "Đất thương mại, dịch vụ"), Is.True);
                Assert.That(tatCa.Any(item => item.Ma == "HNK" && item.Ten == "Đất trồng cây hàng năm khác"), Is.True);
                Assert.That(tatCa.Any(item => item.Ma == "CLN" && item.Ten == "Đất trồng cây lâu năm"), Is.True);
                Assert.That(tatCa.Any(item => item.Ma == "LUA" && item.Ten == "Đất trồng lúa"), Is.True);
                Assert.That(tatCa.Any(item => item.Ma == "DNG" && item.Ten == "Đất xây dựng cơ sở ngoại giao"), Is.True);
                Assert.That(tatCa.Any(item => item.Ma == "TSC" && item.Ten == "Đất xây dựng trụ sở cơ quan"), Is.True);
                Assert.That(tatCa.Any(item => item.Ma == "DTS" && item.Ten == "Đất xây dựng trụ sở của tổ chức sự nghiệp"), Is.True);
            });
        }
    }
}
