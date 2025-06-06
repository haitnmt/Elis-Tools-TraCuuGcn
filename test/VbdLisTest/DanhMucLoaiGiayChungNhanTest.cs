using Haihv.Elis.Tool.VbdLis.DanhMuc;

namespace VbdLisTest;

[TestFixture]
public class DanhMucLoaiGiayChungNhanTest
{
    [Test]
    public void GetMaByTen_ReturnsCorrectCode_ForValidName()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiGiayChungNhan.GetMaByTen("Giấy chứng nhận QSDĐ theo Luật Đất Đai 2003"), Is.EqualTo(1));
            Assert.That(DanhMucLoaiGiayChungNhan.GetMaByTen("Giấy chứng nhận QSDĐ theo Luật Đất Đai 1993"), Is.EqualTo(2));
            Assert.That(DanhMucLoaiGiayChungNhan.GetMaByTen("Giấy chứng nhận QSHNƠ & QSDĐƠ theo Nghị định 60/NĐ-CP"), Is.EqualTo(3));
            Assert.That(DanhMucLoaiGiayChungNhan.GetMaByTen("Giấy chứng nhận QSHNƠ & QSDĐƠ theo Nghị định 90/NĐ-CP"), Is.EqualTo(4));
            Assert.That(DanhMucLoaiGiayChungNhan.GetMaByTen("Giấy chứng nhận sở hữu công trình theo quy định 95"), Is.EqualTo(5));
            Assert.That(DanhMucLoaiGiayChungNhan.GetMaByTen("Giấy chứng nhận QSDĐƠ & QSHNƠ và TSKGLVĐ theo NĐ 88/NĐ-CP"), Is.EqualTo(6));
            Assert.That(DanhMucLoaiGiayChungNhan.GetMaByTen("Giấy hợp thức hóa"), Is.EqualTo(7));
            Assert.That(DanhMucLoaiGiayChungNhan.GetMaByTen("Giấy chứng nhận QSDĐƠ & QSHNƠ và TSKGLVĐ theo NĐ 43/NĐ-CP"), Is.EqualTo(11));
            Assert.That(DanhMucLoaiGiayChungNhan.GetMaByTen("Giấy phép Xây dựng"), Is.EqualTo(17));
            Assert.That(DanhMucLoaiGiayChungNhan.GetMaByTen("Các loại giấy chứng nhận khác"), Is.EqualTo(18));
            Assert.That(DanhMucLoaiGiayChungNhan.GetMaByTen("Giấy phép mua bán, chuyển dịch nhà"), Is.EqualTo(19));
            Assert.That(DanhMucLoaiGiayChungNhan.GetMaByTen("Hợp đồng mua bán tài sản hình thành trong tương lai"), Is.EqualTo(99));
        });
    }

    [Test]
    public void GetMaByTen_ReturnsCorrectCode_WithNormalization()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiGiayChungNhan.GetMaByTen("giay chung nhan qsdd theo luat dat dai 2003"), Is.EqualTo(1));
            Assert.That(DanhMucLoaiGiayChungNhan.GetMaByTen("giay chung nhan qsdd theo luat dat dai 1993"), Is.EqualTo(2));
            Assert.That(DanhMucLoaiGiayChungNhan.GetMaByTen("giay chung nhan qshno & qsddo theo nghi dinh 60/nd-cp"), Is.EqualTo(3));
            Assert.That(DanhMucLoaiGiayChungNhan.GetMaByTen("giay chung nhan qshno & qsddo theo nghi dinh 90/nd-cp"), Is.EqualTo(4));
            Assert.That(DanhMucLoaiGiayChungNhan.GetMaByTen("giay chung nhan so huu cong trinh theo quy dinh 95"), Is.EqualTo(5));
            Assert.That(DanhMucLoaiGiayChungNhan.GetMaByTen("giay chung nhan qsddo & qshno va tskglvd theo nd 88/nd-cp"), Is.EqualTo(6));
            Assert.That(DanhMucLoaiGiayChungNhan.GetMaByTen("giay hop thuc hoa"), Is.EqualTo(7));
            Assert.That(DanhMucLoaiGiayChungNhan.GetMaByTen("giay chung nhan qsddo & qshno va tskglvd theo nd 43/nd-cp"), Is.EqualTo(11));
            Assert.That(DanhMucLoaiGiayChungNhan.GetMaByTen("giay phep xay dung"), Is.EqualTo(17));
            Assert.That(DanhMucLoaiGiayChungNhan.GetMaByTen("cac loai giay chung nhan khac"), Is.EqualTo(18));
            Assert.That(DanhMucLoaiGiayChungNhan.GetMaByTen("giay phep mua ban, chuyen dich nha"), Is.EqualTo(19));
            Assert.That(DanhMucLoaiGiayChungNhan.GetMaByTen("hop dong mua ban tai san hinh thanh trong tuong lai"), Is.EqualTo(99));
        });
    }

    [Test]
    public void GetMaByTen_ReturnsCorrectCode_WithNoDiacritics()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiGiayChungNhan.GetMaByTen("giay chung nhan qsdd theo luat dat dai 2003"), Is.EqualTo(1));
            Assert.That(DanhMucLoaiGiayChungNhan.GetMaByTen("giay chung nhan qsdd theo luat dat dai 1993"), Is.EqualTo(2));
            Assert.That(DanhMucLoaiGiayChungNhan.GetMaByTen("giay hop thuc hoa"), Is.EqualTo(7));
            Assert.That(DanhMucLoaiGiayChungNhan.GetMaByTen("giay phep xay dung"), Is.EqualTo(17));
            Assert.That(DanhMucLoaiGiayChungNhan.GetMaByTen("giay phep mua ban, chuyen dich nha"), Is.EqualTo(19));
            Assert.That(DanhMucLoaiGiayChungNhan.GetMaByTen("hop dong mua ban tai san hinh thanh trong tuong lai"), Is.EqualTo(99));
        });
    }

    [Test]
    public void GetTenByMa_ReturnsCorrectName_ForValidCode()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiGiayChungNhan.GetTenByMa(1), Is.EqualTo("Giấy chứng nhận QSDĐ theo Luật Đất Đai 2003"));
            Assert.That(DanhMucLoaiGiayChungNhan.GetTenByMa(2), Is.EqualTo("Giấy chứng nhận QSDĐ theo Luật Đất Đai 1993"));
            Assert.That(DanhMucLoaiGiayChungNhan.GetTenByMa(3), Is.EqualTo("Giấy chứng nhận QSHNƠ & QSDĐƠ theo Nghị định 60/NĐ-CP"));
            Assert.That(DanhMucLoaiGiayChungNhan.GetTenByMa(4), Is.EqualTo("Giấy chứng nhận QSHNƠ & QSDĐƠ theo Nghị định 90/NĐ-CP"));
            Assert.That(DanhMucLoaiGiayChungNhan.GetTenByMa(5), Is.EqualTo("Giấy chứng nhận sở hữu công trình theo quy định 95"));
            Assert.That(DanhMucLoaiGiayChungNhan.GetTenByMa(6), Is.EqualTo("Giấy chứng nhận QSDĐƠ & QSHNƠ và TSKGLVĐ theo NĐ 88/NĐ-CP"));
            Assert.That(DanhMucLoaiGiayChungNhan.GetTenByMa(7), Is.EqualTo("Giấy hợp thức hóa"));
            Assert.That(DanhMucLoaiGiayChungNhan.GetTenByMa(11), Is.EqualTo("Giấy chứng nhận QSDĐƠ & QSHNƠ và TSKGLVĐ theo NĐ 43/NĐ-CP"));
            Assert.That(DanhMucLoaiGiayChungNhan.GetTenByMa(17), Is.EqualTo("Giấy phép Xây dựng"));
            Assert.That(DanhMucLoaiGiayChungNhan.GetTenByMa(18), Is.EqualTo("Các loại giấy chứng nhận khác"));
            Assert.That(DanhMucLoaiGiayChungNhan.GetTenByMa(19), Is.EqualTo("Giấy phép mua bán, chuyển dịch nhà"));
            Assert.That(DanhMucLoaiGiayChungNhan.GetTenByMa(99), Is.EqualTo("Hợp đồng mua bán tài sản hình thành trong tương lai"));
        });
    }

    [Test]
    public void GetTenByMa_ReturnsNull_ForInvalidCode()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiGiayChungNhan.GetTenByMa(0), Is.Null);
            Assert.That(DanhMucLoaiGiayChungNhan.GetTenByMa(100), Is.Null);
            Assert.That(DanhMucLoaiGiayChungNhan.GetTenByMa(-1), Is.Null);
        });
    }

    [Test]
    public void TatCa_ContainsAllCertificateTypes_AndNotEmpty()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiGiayChungNhan.TatCa, Is.Not.Empty);
            Assert.That(DanhMucLoaiGiayChungNhan.TatCa.Count, Is.GreaterThanOrEqualTo(12));
            Assert.That(DanhMucLoaiGiayChungNhan.TatCa.Any(x => x.Ma == 1), Is.True);
            Assert.That(DanhMucLoaiGiayChungNhan.TatCa.Any(x => x.Ma == 2), Is.True);
            Assert.That(DanhMucLoaiGiayChungNhan.TatCa.Any(x => x.Ma == 3), Is.True);
            Assert.That(DanhMucLoaiGiayChungNhan.TatCa.Any(x => x.Ma == 4), Is.True);
            Assert.That(DanhMucLoaiGiayChungNhan.TatCa.Any(x => x.Ma == 5), Is.True);
            Assert.That(DanhMucLoaiGiayChungNhan.TatCa.Any(x => x.Ma == 6), Is.True);
            Assert.That(DanhMucLoaiGiayChungNhan.TatCa.Any(x => x.Ma == 7), Is.True);
            Assert.That(DanhMucLoaiGiayChungNhan.TatCa.Any(x => x.Ma == 11), Is.True);
            Assert.That(DanhMucLoaiGiayChungNhan.TatCa.Any(x => x.Ma == 17), Is.True);
            Assert.That(DanhMucLoaiGiayChungNhan.TatCa.Any(x => x.Ma == 18), Is.True);
            Assert.That(DanhMucLoaiGiayChungNhan.TatCa.Any(x => x.Ma == 19), Is.True);
            Assert.That(DanhMucLoaiGiayChungNhan.TatCa.Any(x => x.Ma == 99), Is.True);
        });
    }

    [Test]
    public void TatCa_AllItemsHaveValidMaAndTen()
    {
        foreach (var item in DanhMucLoaiGiayChungNhan.TatCa)
        {
            Assert.Multiple(() =>
            {
                Assert.That(item.Ma, Is.GreaterThan(0), $"Mã phải lớn hơn 0 cho item: {item.Ten}");
                Assert.That(string.IsNullOrWhiteSpace(item.Ten), Is.False, $"Tên không được rỗng cho mã: {item.Ma}");
            });
        }
    }
}
