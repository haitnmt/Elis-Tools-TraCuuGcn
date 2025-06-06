using Haihv.Elis.Tool.VbdLis.DanhMuc;

namespace VbdLisTest;

[TestFixture]
public class DanhMucLoaiMucDichSuDungTest
{
    [Test]
    public void GetMaByTen_ReturnsCorrectCode_ForValidName()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiMucDichSuDung.GetMaByTen("Đất ở tại đô thị"), Is.EqualTo("ODT"));
            Assert.That(DanhMucLoaiMucDichSuDung.GetMaByTen("Đất ở tại nông thôn"), Is.EqualTo("ONT"));
            Assert.That(DanhMucLoaiMucDichSuDung.GetMaByTen("Đất chợ"), Is.EqualTo("DCH"));
            Assert.That(DanhMucLoaiMucDichSuDung.GetMaByTen("Đất trồng lúa nước còn lại"), Is.EqualTo("LUK"));
            Assert.That(DanhMucLoaiMucDichSuDung.GetMaByTen("Đất rừng sản xuất"), Is.EqualTo("RSX"));
        });
    }

    [Test]
    public void GetMaByTen_ReturnsCorrectCode_WithNormalization()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiMucDichSuDung.GetMaByTen("dat o tai do thi"), Is.EqualTo("ODT"));
            Assert.That(DanhMucLoaiMucDichSuDung.GetMaByTen("dat o tai nong thon"), Is.EqualTo("ONT"));
            Assert.That(DanhMucLoaiMucDichSuDung.GetMaByTen("dat cho"), Is.EqualTo("DCH"));
            Assert.That(DanhMucLoaiMucDichSuDung.GetMaByTen("dat trong lua nuoc con lai"), Is.EqualTo("LUK"));
            Assert.That(DanhMucLoaiMucDichSuDung.GetMaByTen("dat rung san xuat"), Is.EqualTo("RSX"));
        });
    }

    [Test]
    public void GetMaByTen_ReturnsNull_ForInvalidName()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiMucDichSuDung.GetMaByTen("Nonexistent"), Is.Null);
            Assert.That(DanhMucLoaiMucDichSuDung.GetMaByTen(""), Is.Null);
            Assert.That(DanhMucLoaiMucDichSuDung.GetMaByTen(null!), Is.Null);
        });
    }

    [Test]
    public void GetTenByMa_ReturnsCorrectName_ForValidCode()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiMucDichSuDung.GetTenByMa("ODT"), Is.EqualTo("Đất ở tại đô thị"));
            Assert.That(DanhMucLoaiMucDichSuDung.GetTenByMa("ONT"), Is.EqualTo("Đất ở tại nông thôn"));
            Assert.That(DanhMucLoaiMucDichSuDung.GetTenByMa("DCH"), Is.EqualTo("Đất chợ"));
            Assert.That(DanhMucLoaiMucDichSuDung.GetTenByMa("LUK"), Is.EqualTo("Đất trồng lúa nước còn lại"));
        });
    }

    [Test]
    public void GetTenByMa_ReturnsNull_ForInvalidCode()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiMucDichSuDung.GetTenByMa("XXX"), Is.Null);
            Assert.That(DanhMucLoaiMucDichSuDung.GetTenByMa(""), Is.Null);
            Assert.That(DanhMucLoaiMucDichSuDung.GetTenByMa(null!), Is.Null);
        });
    }

    [Test]
    public void TatCa_ContainsAllPurposeTypes_AndNotEmpty()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiMucDichSuDung.TatCa, Is.Not.Empty);
            Assert.That(DanhMucLoaiMucDichSuDung.TatCa.Count, Is.GreaterThan(0));

            // Kiểm tra một số mục quan trọng có tồn tại
            Assert.That(DanhMucLoaiMucDichSuDung.TatCa.Any(x => x.Ma == "ODT"), Is.True);
            Assert.That(DanhMucLoaiMucDichSuDung.TatCa.Any(x => x.Ma == "ONT"), Is.True);
            Assert.That(DanhMucLoaiMucDichSuDung.TatCa.Any(x => x.Ma == "LUC"), Is.True);
        });
    }

    [Test]
    public void TatCa_AllItemsHaveValidMaAndTen()
    {
        foreach (var item in DanhMucLoaiMucDichSuDung.TatCa)
        {
            Assert.Multiple(() =>
            {
                Assert.That(string.IsNullOrWhiteSpace(item.Ma), Is.False, $"Mã không được rỗng cho item: {item.Ten}");
                Assert.That(string.IsNullOrWhiteSpace(item.Ten), Is.False, $"Tên không được rỗng cho mã: {item.Ma}");
            });
        }
    }
}
