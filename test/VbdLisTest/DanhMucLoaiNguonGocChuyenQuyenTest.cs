using Haihv.Elis.Tool.VbdLis.DanhMuc;

namespace VbdLisTest;

[TestFixture]
public class DanhMucLoaiNguonGocChuyenQuyenTest
{
    [Test]
    public void GetMaByTen_ReturnsCorrectCode_ForValidName()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiNguonGocChuyenQuyen.GetMaByTen("Nhận chuyển đổi đất"), Is.EqualTo("1"));
            Assert.That(DanhMucLoaiNguonGocChuyenQuyen.GetMaByTen("Nhận chuyển nhượng đất"), Is.EqualTo("2"));
            Assert.That(DanhMucLoaiNguonGocChuyenQuyen.GetMaByTen("Nhận thừa kế đất"), Is.EqualTo("3"));
            Assert.That(DanhMucLoaiNguonGocChuyenQuyen.GetMaByTen("Nhận góp vốn đất"), Is.EqualTo("4"));
            Assert.That(DanhMucLoaiNguonGocChuyenQuyen.GetMaByTen("Được tặng cho đất"), Is.EqualTo("5"));
        });
    }

    [Test]
    public void GetMaByTen_ReturnsCorrectCode_WithNormalization()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiNguonGocChuyenQuyen.GetMaByTen("nhan chuyen doi dat"), Is.EqualTo("1"));
            Assert.That(DanhMucLoaiNguonGocChuyenQuyen.GetMaByTen("nhan chuyen nhuong dat"), Is.EqualTo("2"));
            Assert.That(DanhMucLoaiNguonGocChuyenQuyen.GetMaByTen("nhan thua ke dat"), Is.EqualTo("3"));
            Assert.That(DanhMucLoaiNguonGocChuyenQuyen.GetMaByTen("nhan gop von dat"), Is.EqualTo("4"));
            Assert.That(DanhMucLoaiNguonGocChuyenQuyen.GetMaByTen("duoc tang cho dat"), Is.EqualTo("5"));
        });
    }

    [Test]
    public void GetMaByTen_ReturnsNull_ForInvalidName()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiNguonGocChuyenQuyen.GetMaByTen("Nonexistent"), Is.Null);
            Assert.That(DanhMucLoaiNguonGocChuyenQuyen.GetMaByTen(""), Is.Null);
            Assert.That(DanhMucLoaiNguonGocChuyenQuyen.GetMaByTen(null!), Is.Null);
        });
    }

    [Test]
    public void GetTenByMa_ReturnsCorrectName_ForValidCode()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiNguonGocChuyenQuyen.GetTenByMa("1"), Is.EqualTo("Nhận chuyển đổi đất"));
            Assert.That(DanhMucLoaiNguonGocChuyenQuyen.GetTenByMa("2"), Is.EqualTo("Nhận chuyển nhượng đất"));
            Assert.That(DanhMucLoaiNguonGocChuyenQuyen.GetTenByMa("3"), Is.EqualTo("Nhận thừa kế đất"));
            Assert.That(DanhMucLoaiNguonGocChuyenQuyen.GetTenByMa("4"), Is.EqualTo("Nhận góp vốn đất"));
        });
    }

    [Test]
    public void GetTenByMa_ReturnsNull_ForInvalidCode()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiNguonGocChuyenQuyen.GetTenByMa("999"), Is.Null);
            Assert.That(DanhMucLoaiNguonGocChuyenQuyen.GetTenByMa(""), Is.Null);
            Assert.That(DanhMucLoaiNguonGocChuyenQuyen.GetTenByMa(null!), Is.Null);
        });
    }

    [Test]
    public void TatCa_ContainsAllTransferOriginTypes_AndNotEmpty()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiNguonGocChuyenQuyen.TatCa, Is.Not.Empty);
            Assert.That(DanhMucLoaiNguonGocChuyenQuyen.TatCa.Count, Is.GreaterThan(0));

            // Kiểm tra một số mục quan trọng có tồn tại
            Assert.That(DanhMucLoaiNguonGocChuyenQuyen.TatCa.Any(x => x.Ma == "1"), Is.True);
            Assert.That(DanhMucLoaiNguonGocChuyenQuyen.TatCa.Any(x => x.Ma == "2"), Is.True);
            Assert.That(DanhMucLoaiNguonGocChuyenQuyen.TatCa.Any(x => x.Ma == "3"), Is.True);
        });
    }

    [Test]
    public void TatCa_AllItemsHaveValidMaAndTen()
    {
        foreach (var item in DanhMucLoaiNguonGocChuyenQuyen.TatCa)
        {
            Assert.Multiple(() =>
            {
                Assert.That(string.IsNullOrWhiteSpace(item.Ma), Is.False, $"Mã không được rỗng cho item: {item.Ten}");
                Assert.That(string.IsNullOrWhiteSpace(item.Ten), Is.False, $"Tên không được rỗng cho mã: {item.Ma}");
            });
        }
    }
}
