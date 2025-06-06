using Haihv.Elis.Tool.VbdLis.DanhMuc;

namespace VbdLisTest;

[TestFixture]
public class DanhMucLoaiNguonGocDatTest
{
    [Test]
    public void GetMaByTen_ReturnsCorrectCode_ForValidName()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiNguonGocDat.GetMaByTen("Nhà nước giao đất có thu tiền sử dụng đất"), Is.EqualTo("DG-CTT"));
            Assert.That(DanhMucLoaiNguonGocDat.GetMaByTen("Nhà nước giao đất không thu tiền sử dụng đất"), Is.EqualTo("DG-KTT"));
            Assert.That(DanhMucLoaiNguonGocDat.GetMaByTen("Nhà nước công nhận quyền sử dụng đất"), Is.EqualTo("CNQ"));
            Assert.That(DanhMucLoaiNguonGocDat.GetMaByTen("Nhà nước cho thuê đất trả tiền hàng năm"), Is.EqualTo("DT-THN"));
            Assert.That(DanhMucLoaiNguonGocDat.GetMaByTen("Nhà nước cho thuê đất trả tiền một lần"), Is.EqualTo("DT-TML"));
        });
    }

    [Test]
    public void GetMaByTen_ReturnsCorrectCode_WithNormalization()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiNguonGocDat.GetMaByTen("nha nuoc giao dat co thu tien su dung dat"), Is.EqualTo("DG-CTT"));
            Assert.That(DanhMucLoaiNguonGocDat.GetMaByTen("nha nuoc giao dat khong thu tien su dung dat"), Is.EqualTo("DG-KTT"));
            Assert.That(DanhMucLoaiNguonGocDat.GetMaByTen("nha nuoc cong nhan quyen su dung dat"), Is.EqualTo("CNQ"));
            Assert.That(DanhMucLoaiNguonGocDat.GetMaByTen("nha nuoc cho thue dat tra tien hang nam"), Is.EqualTo("DT-THN"));
            Assert.That(DanhMucLoaiNguonGocDat.GetMaByTen("nha nuoc cho thue dat tra tien mot lan"), Is.EqualTo("DT-TML"));
        });
    }

    [Test]
    public void GetMaByTen_ReturnsNull_ForInvalidName()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiNguonGocDat.GetMaByTen("Nonexistent"), Is.Null);
            Assert.That(DanhMucLoaiNguonGocDat.GetMaByTen(""), Is.Null);
            Assert.That(DanhMucLoaiNguonGocDat.GetMaByTen(null!), Is.Null);
        });
    }

    [Test]
    public void GetTenByMa_ReturnsCorrectName_ForValidCode()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiNguonGocDat.GetTenByMa("DG-CTT"), Is.EqualTo("Nhà nước giao đất có thu tiền sử dụng đất"));
            Assert.That(DanhMucLoaiNguonGocDat.GetTenByMa("DG-KTT"), Is.EqualTo("Nhà nước giao đất không thu tiền sử dụng đất"));
            Assert.That(DanhMucLoaiNguonGocDat.GetTenByMa("CNQ"), Is.EqualTo("Nhà nước công nhận quyền sử dụng đất"));
            Assert.That(DanhMucLoaiNguonGocDat.GetTenByMa("DT-THN"), Is.EqualTo("Nhà nước cho thuê đất trả tiền hàng năm"));
        });
    }

    [Test]
    public void GetTenByMa_ReturnsNull_ForInvalidCode()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiNguonGocDat.GetTenByMa("XXX"), Is.Null);
            Assert.That(DanhMucLoaiNguonGocDat.GetTenByMa(""), Is.Null);
            Assert.That(DanhMucLoaiNguonGocDat.GetTenByMa(null!), Is.Null);
        });
    }

    [Test]
    public void TatCa_ContainsAllOriginTypes_AndNotEmpty()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiNguonGocDat.TatCa, Is.Not.Empty);
            Assert.That(DanhMucLoaiNguonGocDat.TatCa.Count, Is.GreaterThan(0));

            // Kiểm tra một số mục quan trọng có tồn tại
            Assert.That(DanhMucLoaiNguonGocDat.TatCa.Any(x => x.Ma == "DG-CTT"), Is.True);
            Assert.That(DanhMucLoaiNguonGocDat.TatCa.Any(x => x.Ma == "DG-KTT"), Is.True);
            Assert.That(DanhMucLoaiNguonGocDat.TatCa.Any(x => x.Ma == "CNQ"), Is.True);
        });
    }

    [Test]
    public void TatCa_AllItemsHaveValidMaAndTen()
    {
        foreach (var item in DanhMucLoaiNguonGocDat.TatCa)
        {
            Assert.Multiple(() =>
            {
                Assert.That(string.IsNullOrWhiteSpace(item.Ma), Is.False, $"Mã không được rỗng cho item: {item.Ten}");
                Assert.That(string.IsNullOrWhiteSpace(item.Ten), Is.False, $"Tên không được rỗng cho mã: {item.Ma}");
            });
        }
    }
}
